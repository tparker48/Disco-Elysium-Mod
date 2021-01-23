using HarmonyLib;
using UnityEngine;


namespace Disco_Explorer_Mod
{
    public static class RunSpeed
    {
        public static float speed = 1.0f;

        public static void SetRunSpeed(float desiredSpeed)
        {
            if (desiredSpeed >= 1.0f && desiredSpeed <= 3.0f)
            {
                speed = desiredSpeed;
            }
        }
    }

    [HarmonyPatch(typeof(Animator))]
    [HarmonyPatch("deltaPosition", MethodType.Getter)]
    class MovementPatch
    {
        static void Postfix(ref Vector3 __result)
        {
            __result *= RunSpeed.speed;
        }
    }
}
