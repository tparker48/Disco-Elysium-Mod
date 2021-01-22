using System;
using HarmonyLib;

namespace Disco_Elysium_Mod
{
    [HarmonyPatch(typeof(SunshinePersistence))]
    class FileNameLoggerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("SaveCoR")]
        static bool Prefix1(string fileNamePrefix)
        {
            fileNamePrefix = SunshinePersistenceFileManager.ReplaceInvalidFileNameChars(fileNamePrefix);
            fileNamePrefix = SunshinePersistenceFileManager.PutDateSuffixOnPath(fileNamePrefix);
            //Main.mod.Logger.Log("Saving: " + fileNamePrefix);
            FastTravel.Save(fileNamePrefix);
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("Load")]
        static bool Prefix2(string fileName)
        {
            //Main.mod.Logger.Log("Loading: " + fileName);
            FastTravel.Load(fileName);
            return true;
        }
    }

    [HarmonyPatch(typeof(FortressOccident.ApplicationManager))]
    class NewDestinationsPatch
    {
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
