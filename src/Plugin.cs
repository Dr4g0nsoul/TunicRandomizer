using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using TinyJson;
using TunicRandomizer.Patches;
using TunicRandomizer.Randomizer;
using TunicRandomizer.Stores;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, "Tunic Randomizer", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {

        public static ManualLogSource Logger;

        //Chest item list to warp to
        public static Queue<Transform> s_sceneItemList;

        //Portal list spawn points to use
        public static List<ScenePatches.SpawnPoint> s_spawnPoints;

        //Current scene info
        public static string s_currentSceneName = "";
        public static int s_currentSceneId = -1;

        //Item store to export
        public static List<RandomItemStore> s_itemStores;
        public static List<string> s_itemStoresAdded;

        //Item Randomizer reference
        //INFO: For now the randomizer starts when the first chest is loaded into a scene, to get randomness depending on when you press new game
        public static ItemRandomizer randomizer;
        //public static List<int> s_openedChests; not needed anymore

        public override void Load()
        {
            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Logger = Log;

            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            PlayerPatches.ApplyPatches(harmony);
            ItemPatches.ApplyPatches(harmony);
            ScenePatches.ApplyPatches(harmony);
            MenuPatches.ApplyPatches(harmony);
        }
    }
}
