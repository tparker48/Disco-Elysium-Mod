using System;
using HarmonyLib;

namespace Disco_Explorer_Mod
{
    [HarmonyPatch(typeof(SunshinePersistenceLoadDataManager))]
    class ClothingChangeReadyPatch
    {
        // toggle clothing change is not ready before load
        [HarmonyPrefix]
        [HarmonyPatch("LoadDataAfterLoadingArea")]
        static bool Prefix()
        {
            ClothesChange.ready = false;
            return true;
        }

        // toggle clothing change is ready after load
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
            if (!ClothesChange.updatingClothes)
            {
                ClothesChange.currentOutfit.Add(itemName);
                if (ClothesChange.on) ClothesChange.originalOutfit.Add(itemName);

                if (ClothesChange.ready)
                {
                    return ClothesChange.on;
                }
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("Unequip")]
        static bool Prefix2(string itemname)
        {
            if (!ClothesChange.updatingClothes)
            {
                ClothesChange.currentOutfit.Remove(itemname);
                if (ClothesChange.on) ClothesChange.originalOutfit.Remove(itemname);

                if (ClothesChange.ready)
                {
                    return ClothesChange.on;
                }
            }

            return true;

        }
    }

    [HarmonyPatch(typeof(TequilaClothingHeadwear))]
    class HeadwearChangePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("EquipHeadWear")]
        static bool Prefix1(string itemName)
        {
            if (!ClothesChange.updatingClothes)
            {
                ClothesChange.currentHeadwear.Add(itemName);
                if (ClothesChange.on) ClothesChange.originalHeadwear.Add(itemName);

                if (ClothesChange.ready)
                {
                    return ClothesChange.on;
                }
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("UnequipHeadWear")]
        static bool Prefix2(string itemName)
        {
            if (!ClothesChange.updatingClothes)
            {
                ClothesChange.currentHeadwear.Remove(itemName);
                if (ClothesChange.on) ClothesChange.originalHeadwear.Remove(itemName);

                if (ClothesChange.ready)
                {
                    return ClothesChange.on;
                }
            }

            return true;
        }
    }
}
