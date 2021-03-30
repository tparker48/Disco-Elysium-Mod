using HarmonyLib;

namespace Disco_Explorer_Mod
{
    public static class CheckPassFail
    {

        public static void Toggle()
        {
            if(!on)
            {
                on = true;
                passing = true;
            }
            else if (passing)
            {
                passing = false;
            }
            else
            {
                on = false;
            }
        }

        public static bool IsOn()
        {
            return on;
        }

        public static bool IsPassing()
        {
            return passing;
        }

        public static int GetDieValue()
        {
            if (passing)
            {
                return 100;
            }
            else
            {
                return -100;
            }
        }

        private static bool on = false;
        private static bool passing = false;
    }

    [HarmonyPatch(typeof(Sunshine.Metric.CheckResult))]
    [HarmonyPatch("Total")]
    class CheckPassFailPatch
    {
        static bool Prefix(ref int ___die1, ref int ___die2)
        {
            Main.mod.Logger.Log("Rolling dice...");
            if (CheckPassFail.IsOn())
            {
                Main.mod.Logger.Log("Dice fixed at " + CheckPassFail.GetDieValue());
                ___die1 = CheckPassFail.GetDieValue();
                ___die2 = CheckPassFail.GetDieValue();
            }


            return true;
        }
    }
}
