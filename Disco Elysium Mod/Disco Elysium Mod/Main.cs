using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;

namespace Disco_Elysium_Mod
{   
    static class Main
    {
        public static bool enabled;

        public static float runSpeed = 1f;
        public static string speed;
        public static string money;
        public static string skillPoints;

        public static bool allThoughtsAdded = false;
        public static bool allClothesAdded = false;

        public static bool hudVisible = true;
        public static bool hudIsOn = true;

        public static UnityModManager.ModEntry mod;
        public static ThoughtsAndItemsTests thoughtsAndItemsAdder;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

            mod = modEntry;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            modEntry.OnGUI = OnGUI;

            thoughtsAndItemsAdder = new ThoughtsAndItemsTests();

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
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
            // set run speed
            GUILayout.Label("Run Speed Multiplier\n[1.0 - 3.0]");
            speed = GUILayout.TextField(speed, GUILayout.Width(100f));
            if (GUILayout.Button("Apply", GUILayout.Width(100f)))
            {
                // set speed 
                if (float.TryParse(speed, out var s))
                {
                    if (s >= 1.0f && s <= 3.0f)
                    {
                        runSpeed = s;
                    }
                    else
                    {
                        runSpeed = 1.0f;
                    }
                }
            }

            // set money
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

            // set skill points
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

            // add all clothes
            if (GUILayout.Button("Add All Clothes", GUILayout.Width(400f)))
            {
                thoughtsAndItemsAdder.AddAllClothes();
            }
            
            // add all thoughts
            if (GUILayout.Button("Add All Thoughts", GUILayout.Width(400f)))
            {
                thoughtsAndItemsAdder.AddAllThoughts();
            }

            GUILayout.Label("\n");

            // toggle appearance lock
            if (GUILayout.Button("Changing Clothes Doesn't Change Appearance: " + (ClothesChange.on ? "OFF" : "ON"), GUILayout.Width(400f)))
            {
                ClothesChange.on = !ClothesChange.on;
                if (ClothesChange.on)
                {
                    ClothesChange.UpdateClothing(modEntry);
                }
            }

            // toggle hud
            if (GUILayout.Button("Toggle HUD", GUILayout.Width(400f)))
            {
                Sunshine.Views.HudToggle.Singleton.ToggleVisibility();
            }

            // fast travel

            // whirling
            if (GUILayout.Button((FastTravel.CheckVisited(FastTravel.whirling) ? "Whirling-In-Rags" : "Undiscovered"), GUILayout.Width(400f)))
            {
                if(FastTravel.CheckVisited(FastTravel.whirling))
                {
                    FastTravel.GoTo(FastTravel.whirling);
                }
            }

            // claire's office
            if (GUILayout.Button((FastTravel.CheckVisited(FastTravel.union) ? "Evrart Claire's Office" : "Undiscovered"), GUILayout.Width(400f)))
            {
                if (FastTravel.CheckVisited(FastTravel.union))
                {
                    FastTravel.GoTo(FastTravel.union);
                }
            }

            // pier
            if (GUILayout.Button((FastTravel.CheckVisited(FastTravel.pier) ? "Pier Apartments" : "Undiscovered"), GUILayout.Width(400f)))
            {
                if (FastTravel.CheckVisited(FastTravel.pier))
                {
                    FastTravel.GoTo(FastTravel.pier);
                }
            }

            // shack
            if (GUILayout.Button((FastTravel.CheckVisited(FastTravel.shack) ? "Shack on the coast" : "Undiscovered"), GUILayout.Width(400f)))
            {
                if (FastTravel.CheckVisited(FastTravel.shack))
                {
                    FastTravel.GoTo(FastTravel.shack);
                }
            }

        }

    }

    
}