using System;
using HarmonyLib;

namespace Disco_Elysium_Mod
{
    [HarmonyPatch(typeof(SunshinePersistence))]
    class FileNameLoggerPatch
    {
        // save fast travel locations when creating a save
        [HarmonyPrefix]
        [HarmonyPatch("SaveCoR")]
        static bool Prefix1(string fileNamePrefix)
        {
            fileNamePrefix = SunshinePersistenceFileManager.ReplaceInvalidFileNameChars(fileNamePrefix);
            fileNamePrefix = SunshinePersistenceFileManager.PutDateSuffixOnPath(fileNamePrefix);

            FastTravel.Save(fileNamePrefix);
            return true;
        }

        // load fast travel locations when loading a save
        [HarmonyPrefix]
        [HarmonyPatch("Load")]
        static bool Prefix2(string fileName)
        {
            FastTravel.Load(fileName);
            return true;
        }
    }

    [HarmonyPatch(typeof(FortressOccident.ApplicationManager))]
    class NewDestinationsPatch
    {

        // Add new available fast travel location on "ChangeArea"
        [HarmonyPrefix]
        [HarmonyPatch("ChangeArea")]
        static bool Prefix1(string areaId, string destinationId)
        {
            if (areaId == FastTravel.areaId)
            {
                FastTravel.AddVisited(destinationId);
            }

            return true;
        }
    }
}
