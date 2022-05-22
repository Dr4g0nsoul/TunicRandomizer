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

namespace TunicRandomizer
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, "Tunic Randomizer", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {

        public static ManualLogSource Logger;

        //Chest item list to warp to
        public static Queue<Chest> s_chestItemList;

        //Portal list spawn points to use
        public static List<ScenePatches.SpawnPoint> s_spawnPoints;

        //Current scene info
        public static string s_currentSceneName = "";
        public static int s_currentSceneId = -1;

        //Item store to export
        public static List<string> s_traversedScenes; //To not rescan scene
        public static List<ItemStore> s_itemStores;

        //Item Randomizer reference
        //INFO: For now the randomizer starts when the first chest is loaded into a scene, to get randomness depending on when you press new game
        public static ChestItemRandomizer randomizer;
        public static List<int> s_openedChests;

        public override void Load()
        {
            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Logger = Log;

            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            /* DEBUG / GATHER INFO PATCHES (to disable in production) */
            MethodInfo playerStartOriginal = AccessTools.Method(typeof(PlayerCharacter), "Start");
            MethodInfo playerStartPatch = AccessTools.Method(typeof(PlayerPatches), "Start_PlayerPatches");
            harmony.Patch(playerStartOriginal, null, new HarmonyMethod(playerStartPatch));

            MethodInfo playerUpdateOriginal = AccessTools.Method(typeof(PlayerCharacter), "Update");
            MethodInfo playerUpdatePatch = AccessTools.Method(typeof(PlayerPatches), "Update_PlayerPatches");
            harmony.Patch(playerUpdateOriginal, null, new HarmonyMethod(playerUpdatePatch));

            MethodInfo sceneInfoUpdateOriginal = AccessTools.Method(typeof(SceneLoader), "OnSceneLoaded");
            MethodInfo sceneInfoUpdatePatch = AccessTools.Method(typeof(ScenePatches), "OnSceneLoaded_SceneLoader_ScenePatches");
            harmony.Patch(sceneInfoUpdateOriginal, null, new HarmonyMethod(sceneInfoUpdatePatch));

            /* ITEM RANDO PATCHES */
            MethodInfo originalOpenChest = AccessTools.Method(typeof(Chest), "IInteractionReceiver_Interact");
            MethodInfo patchedOpenChest = AccessTools.Method(typeof(ItemPatches), "IInteractionReceiver_Interact_ChestPatch");
            harmony.Patch(originalOpenChest, new HarmonyMethod(patchedOpenChest));

            MethodInfo originalCheckOpenChest = AccessTools.PropertyGetter(typeof(Chest), "shouldShowAsOpen");
            MethodInfo patchedCheckOpenChest = AccessTools.Method(typeof(ItemPatches), "shouldShowAsOpen_Debug_ChestPatch");
            harmony.Patch(originalCheckOpenChest, null, new HarmonyMethod(patchedCheckOpenChest));

            MethodInfo originalFairyCount = AccessTools.Method(typeof(FairyCollection), "getFairyCount");
            MethodInfo patchedFairyCount = AccessTools.Method(typeof(ItemPatches), "getFairyCount_Debug_ChestPatch");
            harmony.Patch(originalFairyCount, null, new HarmonyMethod(patchedFairyCount));
        }

        public static void ExportItems()
        {
            string json = s_itemStores.ToJson();
            /*
            string json = "{";
            json += "\"chests\":[";
            bool isFirst = true;
            foreach (ItemStore item in s_itemStores)
            {
                ChestItemStore chestItem = item as ChestItemStore;
                if (chestItem != null)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        json += "{";
                    }
                    else
                    {
                        json += ",{";
                    }

                    json += $"\"chestId\":\"{chestItem.chestId}\",";
                    json += $"\"chestName\":\"{chestItem.chestName}\",";
                    json += "\"chestOpeningPosition\":{";
                    json += $"\"x\":{chestItem.chestOpeningPosition.x.ToString().Replace(',', '.')},";
                    json += $"\"y\":{chestItem.chestOpeningPosition.y.ToString().Replace(',', '.')},";
                    json += $"\"z\":{chestItem.chestOpeningPosition.z.ToString().Replace(',', '.')}";
                    json += "},";
                    json += $"\"itemName\":\"{chestItem.itemName}\",";
                    json += $"\"denominatorString\":\"{chestItem.denominatorString}\",";
                    json += $"\"itemQuantity\":{chestItem.itemQuantity},";
                    json += $"\"itemType\":\"{chestItem.itemType}\",";
                    json += $"\"sceneName\":\"{chestItem.sceneName}\",";
                    json += $"\"sceneId\":{chestItem.sceneId},";
                    json += $"\"itemNearestExit\":\"{chestItem.itemNearestExit}\"";

                    json += "}";

                    //ublic string itemName;
                    //public string denominatorString;
                    //public int itemQuantity;
                    //public Item.ItemType itemType;
                    //public string sceneName;
                    //public int sceneId;
                }
            }
            json += "]}";
            */
            Logger.LogInfo("-----------------------EXPORT---------------------");
            Logger.LogInfo(json);
            Logger.LogInfo("-----------------------EXPORT END---------------------");

        }
    }
}
