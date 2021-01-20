using System;
using System.Collections.Generic;
using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;
using PixelCrushers.DialogueSystem;

namespace Disco_Elysium_Mod
{
    static class RunSpeed
    {
        public static float runSpeed = 1f;
    }

    static class ClothesChange
    {
        public static bool ready = false;
        public static bool on = true;
        public static bool updatingClothes = false;

        public static List<string> originalOutfit = new List<string>();
        public static List<string> currentOutfit = new List<string>();

        public static List<string> originalHeadwear = new List<string>();
        public static List<string> currentHeadwear = new List<string>();

        public static void UpdateClothing(UnityModManager.ModEntry modEntry)
        {
            modEntry.Logger.Log("Updating Clothing");
            modEntry.Logger.Log("Current Clothing = " + currentOutfit);
            modEntry.Logger.Log("Original Clothing = " + originalOutfit);
            
            updatingClothes = true;
            
            //  Call Unequip for original outfit
            foreach (string clothingName in originalOutfit)
            {
                modEntry.Logger.Log("Removing item: " + clothingName);
                TequilaClothing.Unequip(clothingName);
            }
            
            //  Call Unequip for original headwear
            foreach (string clothingName in originalHeadwear)
            {
                modEntry.Logger.Log("Removing item: " + clothingName);
                TequilaClothingHeadwear.UnequipHeadWear(clothingName);
            }
            
            originalOutfit.Clear();
            originalHeadwear.Clear();
            
            // Call equip for current outfit
            foreach (string clothingName in currentOutfit)
            {
                modEntry.Logger.Log("Adding item: " + clothingName);
                TequilaClothing.Equip(clothingName);
                originalOutfit.Add(clothingName);
            }
            
            // Call equip for current headwear
            foreach (string clothingName in currentHeadwear)
            {
                modEntry.Logger.Log("Adding item: " + clothingName);
                TequilaClothingHeadwear.EquipHeadWear(clothingName);
                originalHeadwear.Add(clothingName);
            }
            
            updatingClothes = false;
        }
    }


    static class Main
    {
        public static bool enabled;
        public static UnityModManager.ModEntry mod;

        public static ThoughtsAndItemsTests tester = new ThoughtsAndItemsTests();

        public static string speed;
        public static string money;
        public static string skillPoints;

        public static bool allThoughtsAdded = false;
        public static bool allClothesAdded = false;

        public static bool hudVisible = true;
        public static bool hudIsOn = true;


        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

            mod = modEntry;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            modEntry.OnGUI = OnGUI;

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            modEntry.Logger.Log("toggled");

            return true;
        }

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                modEntry.Logger.Log("B was pressed!");
            }
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.Label("Run Speed Multiplier\n[1.0 - 3.0]");
            speed = GUILayout.TextField(speed, GUILayout.Width(100f));
            if (GUILayout.Button("Apply", GUILayout.Width(100f)))
            {
                // set speed 
                if (float.TryParse(speed, out var s))
                {
                    if (s >= 1.0f && s <= 3.0f)
                    {
                        RunSpeed.runSpeed = s;
                    }
                    else
                    {
                        RunSpeed.runSpeed = 1.0f;
                    }
                }
            }

            GUILayout.Label("\nMoney\n[0 - 999]");
            money = GUILayout.TextField(money, GUILayout.Width(100f));
            if (GUILayout.Button("Apply", GUILayout.Width(100f)))
            {
                // set money
                if (int.TryParse(money, out var m))
                { 
                    if (m >= 0 && m <= 999)
                    {
                        m *= 100;
                        int currentBalance = LiteSingleton<Sunshine.Metric.PlayerCharacter>.Singleton.Money;
                        LiteSingleton<Sunshine.Metric.PlayerCharacter>.Singleton.Money = m;
                        NotificationSystem.NotificationUtil.ShowMoney(m - currentBalance);
                    }

                }
            }

            GUILayout.Label("\nSkillPoints\n[0 - 100]");
            skillPoints = GUILayout.TextField(skillPoints, GUILayout.Width(100f));
            if (GUILayout.Button("Apply", GUILayout.Width(100f)))
            {
                // set skill points
                if (int.TryParse(skillPoints, out var p))
                {
                    if (p >= 0 && p <= 100)
                    {
                        LiteSingleton<Sunshine.Metric.PlayerCharacter>.Singleton.SkillPoints = p;
                    }

                }
            }

            GUILayout.Label("\n");

            if (GUILayout.Button("Add All Clothes", GUILayout.Width(400f)))
            {
                tester.AddAllClothes();
            }

            //GUILayout.Label("\n");

            if (GUILayout.Button("Add All Thoughts", GUILayout.Width(400f)))
            {
                tester.AddAllThoughts();
            }

            GUILayout.Label("\n");

            if (GUILayout.Button("Changing Clothes Doesn't Change Appearance: " + (ClothesChange.on ? "OFF" : "ON"), GUILayout.Width(400f)))
            {
                ClothesChange.on = !ClothesChange.on;
                if (ClothesChange.on)
                {
                    ClothesChange.UpdateClothing(modEntry);
                }
            }

            if (GUILayout.Button("Toggle HUD", GUILayout.Width(400f)))
            {
                Sunshine.Views.HudToggle.Singleton.ToggleVisibility();
            }
        }

    }


    [HarmonyPatch(typeof(Animator))]
    [HarmonyPatch("deltaPosition", MethodType.Getter)]
    class MovementPatch
    {
        static void Postfix(ref Vector3 __result)
        {
            __result *= RunSpeed.runSpeed;
        }
    }

    [HarmonyPatch(typeof(SunshinePersistenceLoadDataManager))]
    class ClothingChangeReadyPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("LoadDataAfterLoadingArea")]
        static bool Prefix()
        {
            ClothesChange.ready = false;
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch("LoadDataAfterLoadingArea")]
        static void Postfix()
        {
            ClothesChange.ready = true;
        }
    }

    [HarmonyPatch(typeof(TequilaClothing))]
    class ClothesChangePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Equip")]
        [HarmonyPatch(new Type[] { typeof(string), typeof(bool) })]
        static bool Prefix1(string itemName)
        {
            if (ClothesChange.ready)
            {
                if (!ClothesChange.updatingClothes)
                {
                    Main.mod.Logger.Log("Adding item to current outfit: " + itemName);
                    ClothesChange.currentOutfit.Add(itemName);
                    if (ClothesChange.on) ClothesChange.originalOutfit.Add(itemName);
                }
    
                return ClothesChange.on;
            }
            else
            {
                return true;
            }
        }
    
        [HarmonyPrefix]
        [HarmonyPatch("Unequip")]
        static bool Prefix2(string itemname)
        {
            if (ClothesChange.ready)
            {
                if (!ClothesChange.updatingClothes)
                {
                    Main.mod.Logger.Log("Removing item from current outfit: " + itemname);
                    ClothesChange.currentOutfit.Remove(itemname);
                    if (ClothesChange.on) ClothesChange.originalOutfit.Remove(itemname);
                }
    
                return ClothesChange.on;
            }
            else
            {
                return true;
            }
        }
    }
    
    [HarmonyPatch(typeof(TequilaClothingHeadwear))]
    class HeadwearChangePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("EquipHeadWear")]
        static bool Prefix1(string itemName)
        {
            if (ClothesChange.ready)
            {
                if(!ClothesChange.updatingClothes)
                {
                    Main.mod.Logger.Log("Adding item to current headwear: " + itemName);
                    ClothesChange.currentHeadwear.Add(itemName);
                    if (ClothesChange.on) ClothesChange.originalHeadwear.Add(itemName);
                }
    
                return ClothesChange.on;
            }
            else
            {
                return true;
            }
        }
    
        [HarmonyPrefix]
        [HarmonyPatch("UnequipHeadWear")]
        static bool Prefix2(string itemName)
        {
            if(!ClothesChange.updatingClothes)
            {
                if (!ClothesChange.updatingClothes)
                {
                    Main.mod.Logger.Log("Removing from current headwear: " + itemName);
                    ClothesChange.currentHeadwear.Remove(itemName);
                    if (ClothesChange.on) ClothesChange.originalHeadwear.Remove(itemName);
                }
    
                return ClothesChange.on;
            }
            else
            {
                return true;
            }
        }
    }
}