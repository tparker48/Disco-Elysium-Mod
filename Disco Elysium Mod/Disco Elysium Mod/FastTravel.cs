using System;
using System.IO;
using System.Collections.Generic;

namespace Disco_Elysium_Mod
{
    public static class FastTravel
    {
        public static void Load(string saveName)
        {
            if (!LoadData(saveName))
            {
                InitData();
            }
        }

        public static void Save(string saveName)
        {
            try
            {
                
                StreamWriter writer = new StreamWriter(Path.Combine(dir, CleanFileName(saveName)));
                serializer.Serialize(writer, locations);
                writer.Close();
            }
            catch
            {
                
                Main.mod.Logger.Log("DiscoMod: Could not save fast travel locations");
            }
        }

        public static void AddVisited(string destination)
        {
            if (destination == whirling || destination == shack || destination == pier || destination == union)
            {
                if (!locations.Contains(destination))
                {
                    locations.Add(destination);
                }
            }
        }
        public static bool CheckVisited(string destination)
        {
            return locations.Contains(destination);
        }
        public static void GoTo(string destination)
        {
            Sunshine.Dialogue.MapLuaFunctions.GoToDestination(FastTravel.areaId, destination);
        }

        public static string areaId = "Martinaise-ext";

        public static string whirling = "whirling";
        public static string shack = "second-home";
        public static string union = "union-boss";
        public static string pier = "apartments-pier-1";

        private static string dir = Path.Combine(Environment.CurrentDirectory,"Mods","DiscoMod","FastTravelLocations");

        private static List<string> locations = new List<string>();
        private static System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(locations.GetType());

        private static void InitData()
        {
            locations = new List<string>();
        }

        
        private static bool LoadData(string saveName)
        {
            try
            {
                Main.mod.Logger.Log("Loading Fast travel data: " + saveName);
        
                FileStream fs = new FileStream(Path.Combine(dir,CleanFileName(saveName)), FileMode.Open);
                TextReader reader = new StreamReader(fs);
                locations = (List<string>)serializer.Deserialize(reader);
                reader.Close();
                fs.Close();

                Main.mod.Logger.Log("Success!");

                return true;
            }
            catch
            {
                Main.mod.Logger.Log("Fail!");
                return false;
            }
        }

        private static string CleanFileName(string saveName)
        {
            int milliseconds = saveName.LastIndexOf('-');
            saveName = saveName.Substring(0, milliseconds) + ")";
            return saveName;
        }

    }
}
