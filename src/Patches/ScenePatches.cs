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
                Plugin.s_spawnPoints.Add(new SpawnPoint(scenePortal.destinationSceneName, scenePortal.FullID, scenePortal.playerSpawnTransform.position));
            }
            Plugin.Logger.LogInfo("------------ SPAWN POINTS END ------------");

            if (Plugin.s_traversedScenes == null) Plugin.s_traversedScenes = new List<string>();
            if (Plugin.s_traversedScenes.Contains(loadingScene.name)) Plugin.Logger.LogInfo($"Scene {loadingScene.name} already scanned for items");

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
            if (!Plugin.s_traversedScenes.Contains(loadingScene.name)) Plugin.s_traversedScenes.Add(loadingScene.name);
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
            if(!Plugin.s_traversedScenes.Contains(loadingScene.name)) Plugin.s_traversedScenes.Add(loadingScene.name);
            Plugin.Logger.LogInfo("------------ PICKUP ITEMS END ------------");
        }

        private static void AddChestToFoundChests(Chest chest)
        {
            Plugin.Logger.LogInfo($"Chest {chest.name} ({chest.GetInstanceID()}): ");
            
            Plugin.s_sceneItemList.Enqueue(chest.characterOpeningTransform);

            // Convert into Chest item Store
            RandomItemStore chestItemStore = RandomItemStore.ChestToRandomItemStore(chest);

            // ! NOT ADDING FAIRIES AND OTHER STUFF WITH CHESTID 0 FOR NOW
            if (chest.GetInstanceID() >= 0)
            {
                // Add chest to store for output
                if (Plugin.s_itemStores == null) Plugin.s_itemStores = new List<RandomItemStore>();
                Plugin.s_itemStores.Add(chestItemStore);
            }
            else
            {
                Plugin.Logger.LogWarning("THIS CHEST HAS INSTANCE_ID < 0 AND WAS THEREFORE (FOR NOW) NOT ADDED TO THE RANDOMIZER POOL");
            }

        }

        private static void AddPickupItemToFoundItems(ItemPickup item)
        {
            Plugin.Logger.LogInfo($"Pickup item {item.itemToGive.name} ({item.itemToGive.Type}) x {item.QuantityToGive}");

            Plugin.s_sceneItemList.Enqueue(item.transform);

            // Convert into Chest item Store
            RandomItemStore chestItemStore = RandomItemStore.PickupItemToRandomItemStore(item);

            // ! NOT ADDING FAIRIES AND OTHER STUFF WITH CHESTID 0 FOR NOW
            if (item.GetInstanceID() >= 0)
            {
                // Add chest to store for output
                if (Plugin.s_itemStores == null) Plugin.s_itemStores = new List<RandomItemStore>();
                Plugin.s_itemStores.Add(chestItemStore);
            }
            else
            {
                Plugin.Logger.LogWarning("THIS PICKUP ITEM HAS INSTANCE_ID < 0 AND WAS THEREFORE (FOR NOW) NOT ADDED TO THE RANDOMIZER POOL");
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
