using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TunicRandomizer.Patches;
using UnityEngine;

namespace TunicRandomizer.Stores
{
    [Serializable]
    public class ChestItemStore : ItemStore
    {

        public int chestId;
        public string chestName;
        public Vector3 chestOpeningPosition;
        public int moneyQuantity;

        public ChestItemStore(int chestId, string chestName, Vector3 chestOpeningPosition, string itemName, string denominationString, int itemQuantity, string itemType, string sceneName, int sceneId, string itemNearestExit, int moneyQuantity)
            : base(itemName, denominationString, itemQuantity, itemType, sceneName, sceneId, itemNearestExit)
        {
            this.chestId = chestId;
            this.chestName = chestName;
            this.chestOpeningPosition = chestOpeningPosition;
            this.moneyQuantity = moneyQuantity;
        }

        public static ChestItemStore ChestToChestItemStore(Chest chest)
        {
            //var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            //PropertyInfo propertyInfo = chest.GetType().GetProperty("itemContents", bindingFlags);
            Item item = null;
            int money = 0;
            if (!chest.isFairy)
            {
                try
                {
                    item = chest.itemContentsfromDatabase;
                }
                catch (Exception)
                {
                    Plugin.Logger.LogError($"Could not retrieve Item from Chest {chest.name} ({chest.chestID})");
                }
                try
                {
                    money = chest.moneySprayQuantityFromDatabase;
                }
                catch (Exception)
                {
                    Plugin.Logger.LogError($"No money found in Chest {chest.name} ({chest.chestID})");
                }
            }
            return new ChestItemStore(
                chest.chestID,
                chest.name,
                chest.characterOpeningTransform.position,
                chest.isFairy ? "Fairy" : item?.name,
                chest.isFairy ? "Fairy" : item?.DenominatorString,
                chest.isFairy ? 0 : chest.itemQuantityFromDatabase,
                chest.isFairy ? "FAIRY" : (money > 0 ? "MONEY" : (item != null ? item.Type.ToString() : "NONE/OTHER/MONEY")),
                Plugin.s_currentSceneName,
                Plugin.s_currentSceneId,
                ScenePatches.GetItemNearestExit(chest.transform.position),
                chest.isFairy ? 0 : money
            );
        }

    }
}
