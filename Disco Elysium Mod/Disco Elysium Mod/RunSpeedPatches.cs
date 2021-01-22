using HarmonyLib;
using UnityEngine;


namespace Disco_Elysium_Mod
{
    [HarmonyPatch(typeof(Animator))]
    [HarmonyPatch("deltaPosition", MethodType.Getter)]
    class MovementPatch
    {
        static void Postfix(ref Vector3 __result)
        {
            __result *= Main.runSpeed;
        }
    }
}
