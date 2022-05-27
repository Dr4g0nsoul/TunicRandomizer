﻿using BepInEx.Logging;
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
        public static ChestItemStore s_nextRandomChest = null;

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

            //Plugin.s_openedChests.Add(__instance.chestID); not needed anymore

            //RANDOMIZED ITEM
            ChestItemStore randomChestItem = Plugin.randomizer.GetRandomizedItem(__instance);
            s_nextRandomChest = randomChestItem;

            if (randomChestItem.itemType == "FAIRY")
            {
                Plugin.Logger.LogInfo($"Randomized item: Fairy");
                __instance.isFairy = true;
            }
            else if (randomChestItem.itemType == "MONEY")
            {
                Plugin.Logger.LogInfo($"Randomized item: {randomChestItem.moneyQuantity}$");
            }
            else if (randomChestItem.itemName != null)
            {
                Plugin.Logger.LogInfo($"Randomized item: {randomChestItem.itemName} x {randomChestItem.itemQuantity}");
            }
            else
            {
                Plugin.Logger.LogError($"Unhandled randomization for original chest: {__instance.name} ({__instance.chestID}). Random Item {randomChestItem.chestName} ({randomChestItem.chestId}) {randomChestItem.itemType}");
            }

            return true;

        }

        private static bool HandleFairy(Chest chest)
        {
            s_fairiesFound += 1;
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

        /* NOT NEEDED FOR NOW
        public static void shouldShowAsOpen_Debug_ChestPatch(Chest __instance, ref bool __result)
        {
            if (Plugin.s_openedChests == null) Plugin.s_openedChests = new List<int>();
            if(Plugin.s_openedChests.Contains(__instance.chestID))
            {
                __result = true;
            }
        }
        */

        public static void moneySprayQuantityFromDatabase_ChestPatch(Chest __instance, ref int __result)
        {
            if (s_nextRandomChest == null) return;

            if (s_nextRandomChest.itemType == "MONEY")
            {
                __result = s_nextRandomChest.moneyQuantity;
            }
        }

        public static void itemContentsfromDatabase_ChestPatch(Chest __instance, ref Item __result)
        {
            if (s_nextRandomChest == null) return;

            __result = null;

            if (s_nextRandomChest.itemName != null)
            {
                // HANDLE TRINKETS
                if (s_nextRandomChest.itemType == Item.ItemType.TRINKETS.ToString())
                {
                    TrinketItem newItem = null;
                    foreach (TrinketItem trinketItem in Resources.FindObjectsOfTypeAll<TrinketItem>())
                    {
                        if (trinketItem.name == s_nextRandomChest.itemName)
                        {
                            newItem = trinketItem;
                            break;
                        }
                    }

                    if (newItem == null)
                    {
                        Plugin.Logger.LogError($"Trinket item {s_nextRandomChest.itemName} not found in Assets");
                    }
                    else
                    {
                        __result = newItem;
                    }
                }
                else // HANDLE OTHER ITEMS
                {
                    __result = Inventory.GetItemByName(s_nextRandomChest.itemName);
                }
            }
        }

        public static void itemQuantityFromDatabase_ChestPatch(Chest __instance, ref int __result)
        {
            if (s_nextRandomChest == null) return;

            __result = s_nextRandomChest.itemQuantity;
        }
    }
}
