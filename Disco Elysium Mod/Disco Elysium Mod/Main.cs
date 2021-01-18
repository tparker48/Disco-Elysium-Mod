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
        public static string ammo;


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
            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    modEntry.Logger.Log("B was pressed!");
            //    if (RunSpeed.runSpeed == 2f)
            //    {
            //        RunSpeed.runSpeed = 1f;
            //    }
            //    else
            //    {
            //        RunSpeed.runSpeed = 2f;
            //    }
            //}
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.Label("RUN SPEED MULTIPLIER\n[1.0 - 3.0");
            speed = GUILayout.TextField(speed, GUILayout.Width(100f));
            if (GUILayout.Button("Apply") && float.TryParse(speed, out var s))
            {
                if(s >= 1.0f && s <= 3.0f)
                {
                    RunSpeed.runSpeed = s;
                }
                else
                {
                    RunSpeed.runSpeed = 1.0f;
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