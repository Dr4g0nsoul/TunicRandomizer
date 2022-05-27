using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TunicRandomizer.Patches;
using UnityEngine;

namespace TunicRandomizer.Stores
{
    [Serializable]
    public class RandomItemStore
    {

        public string instanceId;
        public string itemName;
        public int itemQuantity;
        public string itemType;
        public int sceneId;
        public string sceneName;
        public string itemNearestExit;
        public int chestId;
        public string itemContainerName;
        public Vector3 itemContainerPosition;
        public int moneyQuantity;
        public string[] needsItems;

        public RandomItemStore(
            string instanceId, 
            string itemName, 
            int itemQuantity, 
            string itemType, 
            int sceneId, 
            string sceneName, 
            string itemNearestExit, 
            int chestId,
            string itemContainerName,
            Vector3 itemContainerPosition,
            int moneyQuantity
        )
        {
            this.instanceId = instanceId;
            this.itemName = itemName;
            this.itemQuantity = itemQuantity;
            this.itemType = itemType;
            this.sceneId = sceneId;
            this.sceneName = sceneName;
            this.itemNearestExit = itemNearestExit;
            this.chestId = chestId;
            this.itemContainerName = itemContainerName;
            this.itemContainerPosition = itemContainerPosition;
            this.moneyQuantity = moneyQuantity;
        }

        public RandomItemStore(
            string instanceId,
            string itemName,
            int itemQuantity,
            string itemType,
            int sceneId,
            string sceneName,
            string itemNearestExit,
            int chestId,
            string itemContainerName,
            Vector3 itemContainerPosition,
            int moneyQuantity,
            string[] needsItems
        )
        {
            this.instanceId = instanceId;
            this.itemName = itemName;
            this.itemQuantity = itemQuantity;
            this.itemType = itemType;
            this.sceneId = sceneId;
            this.sceneName = sceneName;
            this.itemNearestExit = itemNearestExit;
            this.chestId = chestId;
            this.itemContainerName = itemContainerName;
            this.itemContainerPosition = itemContainerPosition;
            this.moneyQuantity = moneyQuantity;
            this.needsItems = needsItems;
        }

        public static RandomItemStore ChestToRandomItemStore(Chest chest)
        {
            //var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            //PropertyInfo propertyInfo = chest.GetType().GetProperty("itemContents", bindingFlags);
            string uniqueId = "c";
            Item item = null;
            int money = 0;
            int quantity = 0;
            if (!chest.isFairy)
            {
                try
                {
                    item = chest.itemContentsfromDatabase;
                }
                catch (Exception)
                {
                    Plugin.Logger.LogError($"Could not retrieve Item from Chest {chest.name} ({chest.GetInstanceID()})");
                }
                try
                {
                    money = chest.moneySprayQuantityFromDatabase;
                }
                catch (Exception)
                {
                    Plugin.Logger.LogError($"No money found in Chest {chest.name} ({chest.GetInstanceID()})");
                }
                try
                {
                    quantity = chest.itemQuantityFromDatabase;
                }
                catch (Exception)
                {
                    Plugin.Logger.LogError($"Error reading quantity from Chest {chest.name} ({chest.GetInstanceID()})");
                }
            }

            if (chest.chestID > 0)
            {
                uniqueId += chest.chestID;
            }
            uniqueId += $"_{Plugin.s_currentSceneId}_{Math.Abs((int)chest.transform.position.x)}_{Math.Abs((int)chest.transform.position.y)}_{Math.Abs((int)chest.transform.position.z)}";

            return new RandomItemStore(
                uniqueId,
                chest.isFairy ? "Fairy" : item?.name,
                chest.isFairy ? 0 : quantity,
                chest.isFairy ? "FAIRY" : (money > 0 ? "MONEY" : (item != null ? item.Type.ToString() : "NONE/OTHER/MONEY")),
                Plugin.s_currentSceneId,
                Plugin.s_currentSceneName,
                ScenePatches.GetItemNearestExit(chest.transform.position),
                chest.chestID,
                chest.name,
                chest.characterOpeningTransform.position,
                chest.isFairy ? 0 : money
            );
        }

        public static RandomItemStore PickupItemToRandomItemStore(ItemPickup item)
        {
            string uniqueId = $"i_{Plugin.s_currentSceneId}_{Math.Abs((int)item.transform.position.x)}_{Math.Abs((int)item.transform.position.y)}_{Math.Abs((int)item.transform.position.z)}";
            return new RandomItemStore(
                uniqueId,
                item.itemToGive.name,
                item.QuantityToGive,
                item.itemToGive.Type.ToString(),
                Plugin.s_currentSceneId,
                Plugin.s_currentSceneName,
                ScenePatches.GetItemNearestExit(item.transform.position),
                -1,
                item.name,
                item.transform.position,
                0
            );
        }
    }
}
