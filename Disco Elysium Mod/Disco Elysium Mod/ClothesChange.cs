using System.Collections.Generic;
using UnityModManagerNet;

namespace Disco_Elysium_Mod
{
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
            updatingClothes = true;

            //  Call Unequip for original outfit
            foreach (string clothingName in originalOutfit)
            {
                TequilaClothing.Unequip(clothingName);
            }

            //  Call Unequip for original headwear
            foreach (string clothingName in originalHeadwear)
            {
                TequilaClothingHeadwear.UnequipHeadWear(clothingName);
            }

            originalOutfit.Clear();
            originalHeadwear.Clear();

            // Call equip for current outfit
            foreach (string clothingName in currentOutfit)
            {
                TequilaClothing.Equip(clothingName);
                originalOutfit.Add(clothingName);
            }

            // Call equip for current headwear
            foreach (string clothingName in currentHeadwear)
            {
                TequilaClothingHeadwear.EquipHeadWear(clothingName);
                originalHeadwear.Add(clothingName);
            }

            updatingClothes = false;
        }
    }
}
