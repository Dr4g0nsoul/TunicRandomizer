using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TunicRandomizer;
using TunicRandomizer.Stores;
using UnityEngine;

namespace TunicRandomizer.Patches
{
    public class ItemPatches
    {

        public static int s_fairiesFound = 0;

        public static bool IInteractionReceiver_Interact_ChestPatch(Item i, Chest __instance)
        {
            // ORIGINAL ITEM
            Item item = null;
            int money = 0;
            if (!__instance.isFairy)
            {
                try
                {
                    item = __instance.itemContentsfromDatabase;
                }
                catch (Exception) { }
                try
                {
                    money = __instance.moneySprayQuantityFromDatabase;
                }
                catch (Exception) { }
            }
            else
            {
                Plugin.Logger.LogInfo($"Original item: Fairy");
            }

            if (money > 0) {
                Plugin.Logger.LogInfo($"Original item: {money}$");
            }
            else
            {
                Plugin.Logger.LogInfo($"Original item: {item.name} x {item.Quantity}");
            }

            Plugin.s_openedChests.Add(__instance.chestID);

            //RANDOMIZED ITEM
            ChestItemStore randomChestItem = Plugin.randomizer.GetRandomizedItem(__instance);
            if(randomChestItem.itemType == "FAIRY")
            {
                Plugin.Logger.LogInfo($"Tandomized item: Fairy");
                return HandleFairy(__instance);
            }
            else if(randomChestItem.itemType == "MONEY")
            {
                Plugin.Logger.LogInfo($"Randomized item: {randomChestItem.moneyQuantity}$");
                return HandleMoney(__instance, randomChestItem.moneyQuantity);
            } 
            else if(randomChestItem.itemName != null) {
                Plugin.Logger.LogInfo($"Randomized item: {randomChestItem.itemName} x {randomChestItem.itemQuantity}");
                return HandleEquipment(__instance, randomChestItem.itemName, randomChestItem.itemQuantity);
            }

            Plugin.Logger.LogError($"Unhandled randomization for original chest: {__instance.name} ({__instance.chestID}). Random Item {randomChestItem.chestName} ({randomChestItem.chestId}) {randomChestItem.itemType}");

            return true;

            //return HandleFairy(__instance);

            //return HandleTrinket(__instance, "Trinket - MP Flasks", 1);

            //return HandleEquipment(__instance, "Flask Container", 1);

            //return HandleMoney(__instance, 50);


            //__instance.isFairy = true;

            //Item sword = Inventory.GetItemByName("Sword");
            //sword.Quantity += 1;
            //ItemPresentation.PresentItem(sword);
            //foreach(StateVariable stateVariable in StateVariable.stateVariableList)
            //{
            //    Plugin.Logger.LogInfo(stateVariable.name);
            //}
            ////Plugin.Logger.LogInfo(__instance.openedStateVar.name);
            //return false;

            //if(__instance.chestID > 0)
            //{
            //    ChestItemStore newChest = Plugin.randomizer.GetRandomizedItem(__instance);
            //    Item newItem = Inventory.GetItemByName(newChest.itemName);
            //    newItem.Quantity += 1;
            //}
        }

        private static bool HandleFairy(Chest chest)
        {
            chest.isFairy = true; //To block the original item
            s_fairiesFound += 1;
            return true;
        }

        /**
         * Handles most items/eqipments including:
         * -> Equipment, Trophies, Fairies, Supplies, Offerings, Trinkets/Cards, Gear/Flasks
         */
        private static bool HandleEquipment(Chest chest, string itemName, int quantity)
        {
            Plugin.Logger.LogInfo("HI: "+Inventory.AnyWellSupplyItemsUnlocked());
            chest.isFairy = true; //To block the original item
            Item newItem = Inventory.GetItemByName(itemName);
            if (newItem != null)
            {
                newItem.Quantity += quantity;
                ItemPresentation.instance.presentItem(newItem, quantity);
            }
            else
            {
                Plugin.Logger.LogError($"Could not retrieve Item \"{itemName}\" x {quantity} from Chest {chest.name} ({chest.chestID})");
                return false;
            }
            return true;
        }

        private static bool HandleTrinket(Chest chest, string itemName, int quantity)
        {
            //bool ret = HandleEquipment(chest, itemName, quantity);
            //if(ret)
            //{
            //    Item itemToConvert = Inventory.GetItemByName(itemName);
            //    TrinketItem.trinketList.Add(trinketItem);
            //    TrinketItem.SetSlot(trinketItem, 0);
            //}
            //return ret;
            return true;
        }

        public static bool HandleMoney(Chest chest, int quantity)
        {
            chest.isFairy = true; //Also removes coins already in the chest
            SmallMoneyItem.PlayerQuantity += quantity;
            Item money = Inventory.GetItemByName("Trinket Coin");
            money.collectionMessage = new LanguageLine();

            if (quantity == 1) money.collectionMessage.text = "$$$";
            else if(quantity < 20) money.collectionMessage.text = "$$$$$$";
            else if(quantity < 50) money.collectionMessage.text = "$$$$$$$$$";
            else money.collectionMessage.text = "$$$$$$$$$$$$";

            ItemPresentation.PresentItem(money, quantity);
            return true;
        }

        /**
         * Can be used in the future as workaround for collecting fairies and unlocking the 2 chests if there is no alternative
         */
        public static void getFairyCount_Debug_ChestPatch(ref int __result)
        {
            FairyCollection collection = GameObject.FindObjectOfType<FairyCollection>();
            if(s_fairiesFound >= 10 && !collection.enoughFairiesFound.BoolValue) collection.enoughFairiesFound.BoolValue = true;
            if(s_fairiesFound >= 20 && !collection.allFairiesFound.BoolValue) collection.allFairiesFound.BoolValue = true;
        }

        public static void shouldShowAsOpen_Debug_ChestPatch(Chest __instance, ref bool __result)
        {
            if(Plugin.s_openedChests.Contains(__instance.chestID))
            {
                __result = true;
            }
        }
    }
}
