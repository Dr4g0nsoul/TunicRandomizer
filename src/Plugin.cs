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

        


        //Item Randomizer reference
        //INFO: For now the randomizer starts when the first chest is loaded into a scene, to get randomness depending on when you press new game
        public static ItemRandomizer randomizer;

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
