//using System;
using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;

namespace Disco_Elysium_Mod
{
    static class RunSpeed
    {
        public static float runSpeed = 2f;
    }
    static class Main
    {
        public static bool enabled;
        public static UnityModManager.ModEntry mod;
        public static string speed;
        public static string money;


        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            
            mod = modEntry;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            modEntry.OnGUI = OnGUI;

            RunSpeed.runSpeed = 2f;

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
            GUILayout.Label("RUN SPEED MULTIPLIER\n[1.0 - 3.0]");
            speed = GUILayout.TextField(speed, GUILayout.Width(100f));

            GUILayout.Label("Money\n[0 - 99]");
            money = GUILayout.TextField(money, GUILayout.Width(100f));
            if (GUILayout.Button("Apply"))
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
                
                // set money
                if (int.TryParse(money, out var m))
                {
                    m *= 100;
                    if (m >= 0 && m <= 99)
                    {
                        int currentBalance = LiteSingleton<Sunshine.Metric.PlayerCharacter>.Singleton.Money;
                        LiteSingleton<Sunshine.Metric.PlayerCharacter>.Singleton.Money = m;
                        NotificationSystem.NotificationUtil.ShowMoney(m - currentBalance);
                    }
                    
                }

            }
        }
    }



    [HarmonyPatch(typeof(Animator))]
    [HarmonyPatch("deltaPosition", MethodType.Getter)]
    class MyPatches
    {
        static void Postfix(ref Vector3 __result)
        {
            __result *= RunSpeed.runSpeed;
        }
    }

}