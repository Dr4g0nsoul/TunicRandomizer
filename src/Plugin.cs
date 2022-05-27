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

            /*
            MethodInfo originalOpeningChest = AccessTools.PropertyGetter(typeof(Chest), "OnOpen");
            MethodInfo patchedOpeningChest = AccessTools.Method(typeof(ItemPatches), "OnOpen_ChestPatch");
            harmony.Patch(originalOpeningChest, new HarmonyMethod(patchedOpeningChest));

            MethodInfo originalCheckOpenChest = AccessTools.PropertyGetter(typeof(Chest), "shouldShowAsOpen");
            MethodInfo patchedCheckOpenChest = AccessTools.Method(typeof(ItemPatches), "shouldShowAsOpen_Debug_ChestPatch");
            harmony.Patch(originalCheckOpenChest, null, new HarmonyMethod(patchedCheckOpenChest));
            */

            MethodInfo originalFairyCount = AccessTools.Method(typeof(FairyCollection), "getFairyCount");
            MethodInfo patchedFairyCount = AccessTools.Method(typeof(ItemPatches), "getFairyCount_Debug_ChestPatch");
            harmony.Patch(originalFairyCount, null, new HarmonyMethod(patchedFairyCount));

            MethodInfo originalChestMoney = AccessTools.PropertyGetter(typeof(Chest), "moneySprayQuantityFromDatabase");
            MethodInfo patchedChestMoney = AccessTools.Method(typeof(ItemPatches), "moneySprayQuantityFromDatabase_ChestPatch");
            harmony.Patch(originalChestMoney, null, new HarmonyMethod(patchedChestMoney));

            MethodInfo originalChestItem = AccessTools.PropertyGetter(typeof(Chest), "itemContentsfromDatabase");
            MethodInfo patchedChestItem = AccessTools.Method(typeof(ItemPatches), "itemContentsfromDatabase_ChestPatch");
            harmony.Patch(originalChestItem, null, new HarmonyMethod(patchedChestItem));

            MethodInfo originalChestItemQuantity = AccessTools.PropertyGetter(typeof(Chest), "itemQuantityFromDatabase");
            MethodInfo patchedChestItemQuantity = AccessTools.Method(typeof(ItemPatches), "itemQuantityFromDatabase_ChestPatch");
            harmony.Patch(originalChestItemQuantity, null, new HarmonyMethod(patchedChestItemQuantity));


            MethodInfo originalPickupItemQuantity2 = AccessTools.Method(typeof(ItemPickup), "onGetIt");
            MethodInfo patchedPickupItemQuantity2 = AccessTools.Method(typeof(ItemPatches), "onGetIt_ItemPickupPatch");
            harmony.Patch(originalPickupItemQuantity2, new HarmonyMethod(patchedPickupItemQuantity2));



        }
    }
}
