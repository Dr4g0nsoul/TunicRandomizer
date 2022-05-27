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

        public static void OnSceneLoaded_SceneLoader_ScenePatches(Scene loadingScene, LoadSceneMode mode)
        {

            Plugin.s_currentSceneName = loadingScene.name;
            Plugin.s_currentSceneId = loadingScene.buildIndex;

            if(Plugin.s_spawnPoints == null) Plugin.s_spawnPoints = new List<SpawnPoint>();
            else Plugin.s_spawnPoints.Clear();

            Plugin.Logger.LogInfo("------------ SPAWN POINTS ------------");
            foreach (ScenePortal scenePortal in GameObject.FindObjectsOfType<ScenePortal>())
            {
                Plugin.Logger.LogInfo($"Added Spawn {Plugin.s_spawnPoints.Count}: {scenePortal.name} to {scenePortal.destinationSceneName} at {scenePortal.FullID} | {scenePortal.optionalIDToSpawnAt}");
                SpawnPoint spawn = new SpawnPoint(scenePortal.destinationSceneName, scenePortal.FullID, scenePortal.playerSpawnTransform.position);
                Plugin.s_spawnPoints.Add(spawn);


                ExportItemsUtils.s_scenesToVisit.Enqueue(spawn);
            }
            Plugin.Logger.LogInfo("------------ SPAWN POINTS END ------------");

            Plugin.Logger.LogInfo("------------ CHESTS ------------");
            
            bool first = true;
            if (Plugin.s_sceneItemList == null) Plugin.s_sceneItemList = new Queue<Transform>();
            else Plugin.s_sceneItemList.Clear();
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


            //ExportItemsUtils.TraverseNextScene(); UNCOMMENT TO EXCECUTE ITEM EXPORT WHEN STARTING A NEW GAME
        }

        private static void AddChestToFoundChests(Chest chest)
        {
            Plugin.Logger.LogInfo($"Chest {chest.name} ({chest.GetInstanceID()}): ");
            
            Plugin.s_sceneItemList.Enqueue(chest.characterOpeningTransform);

            // Convert into Chest item Store
            RandomItemStore chestItemStore = RandomItemStore.ChestToRandomItemStore(chest);

            // Add chest to store for output
            if (Plugin.s_itemStoresAdded == null) Plugin.s_itemStoresAdded = new List<string>();
            if (!Plugin.s_itemStoresAdded.Contains(chestItemStore.instanceId))
            {
                Plugin.s_itemStoresAdded.Add(chestItemStore.instanceId);
                if (Plugin.s_itemStores == null) Plugin.s_itemStores = new List<RandomItemStore>();
                Plugin.s_itemStores.Add(chestItemStore);
            }

        }

        private static void AddPickupItemToFoundItems(ItemPickup item)
        {
            Plugin.Logger.LogInfo($"Pickup item {item.itemToGive.name} ({item.itemToGive.Type}) x {item.QuantityToGive}");

            Plugin.s_sceneItemList.Enqueue(item.transform);

            // Convert into Chest item Store
            RandomItemStore pickupItemStore = RandomItemStore.PickupItemToRandomItemStore(item);

            // Add chest to store for output
            if (Plugin.s_itemStoresAdded == null) Plugin.s_itemStoresAdded = new List<string>();
            if (!Plugin.s_itemStoresAdded.Contains(pickupItemStore.instanceId))
            {
                Plugin.s_itemStoresAdded.Add(pickupItemStore.instanceId);
                if (Plugin.s_itemStores == null) Plugin.s_itemStores = new List<RandomItemStore>();
                Plugin.s_itemStores.Add(pickupItemStore);
            }

        }

        public static string GetItemNearestExit(Vector3 itemPos)
        {
            string closest = "";
            float closestDistance = 999999f;
            foreach(SpawnPoint spawnPoint in Plugin.s_spawnPoints)
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
