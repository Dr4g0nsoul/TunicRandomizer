using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TunicRandomizer;
using TunicRandomizer.Randomizer;
using TunicRandomizer.Stores;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicRandomizer.Patches
{
    public class ScenePatches
    {

        //Current scene info
        public static string s_currentSceneName = "";
        public static int s_currentSceneId = -1;

        //Chest item list to warp to
        public static Queue<Transform> s_sceneItemList;

        //Portal list spawn points to use
        public static List<SpawnPoint> s_spawnPoints;


        public static void ApplyPatches(Harmony harmony)
        {
            MethodInfo sceneInfoUpdateOriginal = AccessTools.Method(typeof(SceneLoader), "OnSceneLoaded");
            MethodInfo sceneInfoUpdatePatch = AccessTools.Method(typeof(ScenePatches), "OnSceneLoaded_SceneLoader_ScenePatches");
            harmony.Patch(sceneInfoUpdateOriginal, null, new HarmonyMethod(sceneInfoUpdatePatch));
        }

        public static void OnSceneLoaded_SceneLoader_ScenePatches(Scene loadingScene, LoadSceneMode mode)
        {

            s_currentSceneName = loadingScene.name;
            s_currentSceneId = loadingScene.buildIndex;

            if(s_spawnPoints == null) s_spawnPoints = new List<SpawnPoint>();
            else s_spawnPoints.Clear();

            Plugin.Logger.LogInfo("------------ SPAWN POINTS ------------");
            AreaStore newArea = new AreaStore();
            newArea.sceneName = s_currentSceneName;
            newArea.areaExits = new List<AreaStore.AreaExit>();

            foreach (ScenePortal scenePortal in GameObject.FindObjectsOfType<ScenePortal>())
            {
                Plugin.Logger.LogInfo($"Added Spawn {s_spawnPoints.Count}: {scenePortal.name} to {scenePortal.destinationSceneName} at {scenePortal.FullID} | {scenePortal.optionalIDToSpawnAt}");
                SpawnPoint spawn = new SpawnPoint(scenePortal.destinationSceneName, scenePortal.FullID, scenePortal.playerSpawnTransform.position);
                s_spawnPoints.Add(spawn);

                AreaStore.AreaExit newExit = new();
                newExit.destinationSceneName = scenePortal.destinationSceneName;
                newExit.destinationLocation = scenePortal.optionalIDToSpawnAt;
                newArea.areaExits.Add(newExit);


                ExportItemsUtils.s_scenesToVisit.Enqueue(spawn);
            }

            if (ExportItemsUtils.s_areaStoresAdded == null) ExportItemsUtils.s_areaStoresAdded = new();
            if (!ExportItemsUtils.s_areaStoresAdded.Contains(s_currentSceneName))
            {
                if (ExportItemsUtils.s_areaStores == null) ExportItemsUtils.s_areaStores = new();
                ExportItemsUtils.s_areaStores.Add(newArea);
                ExportItemsUtils.s_areaStoresAdded.Add(s_currentSceneName);
            }

            Plugin.Logger.LogInfo("------------ SPAWN POINTS END ------------");

            Plugin.Logger.LogInfo("------------ CHESTS ------------");
            
            bool first = true;
            if (s_sceneItemList == null) s_sceneItemList = new Queue<Transform>();
            else s_sceneItemList.Clear();
            foreach (Chest chest in GameObject.FindObjectsOfType<Chest>())
            {
                if (!first) Plugin.Logger.LogInfo("-------------------------------");
                else if(Plugin.randomizer == null || !Plugin.randomizer.IsRandomized)
                {
                    Plugin.randomizer = new ItemRandomizer();
                    Plugin.randomizer.Randomize();
                }

                first = false;
                AddChestToFoundChests(chest);
            }
            Plugin.Logger.LogInfo("------------ CHESTS END ------------");

            Plugin.Logger.LogInfo("------------ PICKUP ITEMS ------------");

            first = true;
            foreach (ItemPickup itemPickup in GameObject.FindObjectsOfType<ItemPickup>())
            {
                if (!first) Plugin.Logger.LogInfo("-------------------------------");
                else if (Plugin.randomizer == null || !Plugin.randomizer.IsRandomized)
                {
                    Plugin.randomizer = new ItemRandomizer();
                    Plugin.randomizer.Randomize();
                }

                first = false;
                AddPickupItemToFoundItems(itemPickup);
            }
            Plugin.Logger.LogInfo("------------ PICKUP ITEMS END ------------");


            //ExportItemsUtils.TraverseNextScene(); // UNCOMMENT TO EXCECUTE ITEM EXPORT WHEN STARTING A NEW GAME
        }

        private static void AddChestToFoundChests(Chest chest)
        {
            Plugin.Logger.LogInfo($"Chest {chest.name} ({chest.GetInstanceID()}): ");
            
            s_sceneItemList.Enqueue(chest.characterOpeningTransform);

            // Convert into Chest item Store
            RandomItemStore chestItemStore = RandomItemStore.ChestToRandomItemStore(chest);

            // Add chest to store for output
            if (ExportItemsUtils.s_itemStoresAdded == null) ExportItemsUtils.s_itemStoresAdded = new List<string>();
            if (!ExportItemsUtils.s_itemStoresAdded.Contains(chestItemStore.instanceId))
            {
                ExportItemsUtils.s_itemStoresAdded.Add(chestItemStore.instanceId);
                if (ExportItemsUtils.s_itemStores == null) ExportItemsUtils.s_itemStores = new List<RandomItemStore>();
                ExportItemsUtils.s_itemStores.Add(chestItemStore);
            }

        }

        private static void AddPickupItemToFoundItems(ItemPickup item)
        {
            Plugin.Logger.LogInfo($"Pickup item {item.itemToGive.name} ({item.itemToGive.Type}) x {item.QuantityToGive}");

            s_sceneItemList.Enqueue(item.transform);

            // Convert into Chest item Store
            RandomItemStore pickupItemStore = RandomItemStore.PickupItemToRandomItemStore(item);

            // Add chest to store for output
            if (ExportItemsUtils.s_itemStoresAdded == null) ExportItemsUtils.s_itemStoresAdded = new List<string>();
            if (!ExportItemsUtils.s_itemStoresAdded.Contains(pickupItemStore.instanceId))
            {
                ExportItemsUtils.s_itemStoresAdded.Add(pickupItemStore.instanceId);
                if (ExportItemsUtils.s_itemStores == null) ExportItemsUtils.s_itemStores = new List<RandomItemStore>();
                ExportItemsUtils.s_itemStores.Add(pickupItemStore);
            }

        }

        public static string GetItemNearestExit(Vector3 itemPos)
        {
            string closest = "";
            float closestDistance = 999999f;
            foreach(SpawnPoint spawnPoint in s_spawnPoints)
            {
                float currDistance = Vector3.Distance(itemPos, spawnPoint.position);
                if(currDistance < closestDistance)
                {
                    closestDistance = currDistance;
                    closest = spawnPoint.id;
                }
            }
            return closest;
        }

        public class SpawnPoint
        {
            public string scene;
            public string id;
            public Vector3 position;

            public SpawnPoint(string scene, string id, Vector3 position)
            {
                this.scene = scene; 
                this.id = id;
                this.position = position;
            }
        }
    }
}
