using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;

namespace Disco_Elysium_Mod
{   
    static class Main
    {
        public static bool enabled;

        public static string speed;
        public static string money;
        public static string skillPoints;

        public static string intellect;
        public static string psyche;
        public static string physique;
        public static string motorics;

        public static string checkPassFailStatus;

        public static UnityModManager.ModEntry mod;
        
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
            GUILayout.BeginHorizontal(GUILayout.Height(200f), GUILayout.Width(550f));
            GUILayout.BeginVertical(GUILayout.Height(200f), GUILayout.Width(200f));

            // set run speed
            GUILayout.Label("Run Speed Multiplier\n[1.0 - 3.0]");
            speed = GUILayout.TextField(speed, GUILayout.Width(100f));
            if (GUILayout.Button("Apply", GUILayout.Width(100f)))
            {
                // set speed 
                if (float.TryParse(speed, out var s))
                {
                    RunSpeed.SetRunSpeed(s);
                }
            }

            // set skill points
            GUILayout.Label("\nSkill Points\n[0 - 100]");
            skillPoints = GUILayout.TextField(skillPoints, GUILayout.Width(100f));
            if (GUILayout.Button("Apply", GUILayout.Width(100f)))
            {
                // set skill points
                if (int.TryParse(skillPoints, out var p))
                {
                    GeneralUtilities.SetSkillPoints(p);
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
                    GeneralUtilities.SetMoney(m);
                }
            }

            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Height(200f), GUILayout.Width(200f));

            GUILayout.Label("Attributes\n");

            GUILayout.Label("Intellect");
            intellect = GUILayout.TextField(intellect, GUILayout.Width(100f));

            GUILayout.Label("Psyche");
            psyche = GUILayout.TextField(psyche, GUILayout.Width(100f));

            GUILayout.Label("Physique");
            physique = GUILayout.TextField(physique, GUILayout.Width(100f));

            GUILayout.Label("Motorics");
            motorics = GUILayout.TextField(motorics, GUILayout.Width(100f));

            GUILayout.Label("");

            if (GUILayout.Button("Apply", GUILayout.Width(100f)))
            {
                if (int.TryParse(intellect, out var i))
                {
                    GeneralUtilities.SetAbilityLevel(Sunshine.Metric.AbilityType.INT, i);
                }
                if (int.TryParse(psyche, out var p))
                {
                    GeneralUtilities.SetAbilityLevel(Sunshine.Metric.AbilityType.PSY, p);
                }
                if (int.TryParse(physique, out var ph))
                {
                    GeneralUtilities.SetAbilityLevel(Sunshine.Metric.AbilityType.FYS, ph);
                }
                if (int.TryParse(motorics, out var m))
                {
                    GeneralUtilities.SetAbilityLevel(Sunshine.Metric.AbilityType.MOT, m);
                }
            }

            GUILayout.EndVertical();

            // fast travel
            GUILayout.BeginVertical(GUILayout.Height(50f), GUILayout.Width(150f));
            GUILayout.Label("Fast Travel\n");

            // whirling
            if (GUILayout.Button((FastTravel.CheckVisited(FastTravel.whirling) ? "Whirling-In-Rags" : "Undiscovered"), GUILayout.Width(150f)))
            {
                if (FastTravel.CheckVisited(FastTravel.whirling))
                {
                    FastTravel.GoTo(FastTravel.whirling);
                }
            }

            GUILayout.Label("");

            // claire's office
            if (GUILayout.Button((FastTravel.CheckVisited(FastTravel.union) ? "Evrart Claire's Office" : "Undiscovered"), GUILayout.Width(150f)))
            {
                if (FastTravel.CheckVisited(FastTravel.union))
                {
                    FastTravel.GoTo(FastTravel.union);
                }
            }

            GUILayout.Label("");

            // pier
            if (GUILayout.Button((FastTravel.CheckVisited(FastTravel.pier) ? "Pier Apartments" : "Undiscovered"), GUILayout.Width(150f)))
            {
                if (FastTravel.CheckVisited(FastTravel.pier))
                {
                    FastTravel.GoTo(FastTravel.pier);
                }
            }

            GUILayout.Label("");

            // shack
            if (GUILayout.Button((FastTravel.CheckVisited(FastTravel.shack) ? "Shack on the coast" : "Undiscovered"), GUILayout.Width(150f)))
            {
                if (FastTravel.CheckVisited(FastTravel.shack))
                {
                    FastTravel.GoTo(FastTravel.shack);
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Label("");

            checkPassFailStatus = "Checks : ";
            if (!CheckPassFail.IsOn())
            {
                checkPassFailStatus += " Unaffected";
            }
            else if (CheckPassFail.IsPassing())
            {
                checkPassFailStatus += " Always PASS";
            }
            else
            {
                checkPassFailStatus += " Always FAIL";
            }


            if (GUILayout.Button(checkPassFailStatus, GUILayout.Width(200f)))
            {
                CheckPassFail.Toggle();
            }
            GUILayout.Label("");

            GUILayout.BeginHorizontal(GUILayout.Height(50f), GUILayout.Width(810f));

            if (GUILayout.Button("Add All Clothes", GUILayout.Width(200f)))
            {
                GeneralUtilities.AddAllClothes();
            }

            GUILayout.Label(" ");

            if (GUILayout.Button("Add All Thoughts", GUILayout.Width(200f)))
            {
                GeneralUtilities.AddAllThoughts();
            }

            GUILayout.Label(" ");

            // toggle appearance lock
            if (GUILayout.Button("Lock Appearance: " + (ClothesChange.on ? "OFF" : "ON"), GUILayout.Width(200f)))
            {
                ClothesChange.on = !ClothesChange.on;
                if (ClothesChange.on)
                {
                    ClothesChange.UpdateClothing(modEntry);
                }
            }

            GUILayout.Label(" ");

            // toggle hud
            if (GUILayout.Button("Toggle HUD", GUILayout.Width(200f)))
            {
                GeneralUtilities.ToggleHud();
            }

            GUILayout.EndHorizontal();
            
        }
    }

    
}