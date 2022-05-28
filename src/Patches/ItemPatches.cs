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
        public static RandomItemStore s_nextRandomChest = null;
        public static bool s_isOriginalFairyChest = false;

        public static bool IInteractionReceiver_Interact_ChestPatch(Item i, Chest __instance)
        {
            s_isOriginalFairyChest = false;
            s_nextRandomChest = null;
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
            else if(item != null)
            {
                Plugin.Logger.LogInfo($"Original item: {item.name} x {item.Quantity}");
            }
            else
            {
                Plugin.Logger.LogWarning($"Chest: {__instance.name} has no valid contents or is fairy chest");
                s_isOriginalFairyChest = true;
            }

            //Plugin.s_openedChests.Add(__instance.chestID); not needed anymore
            RandomItemStore originalItem = RandomItemStore.ChestToRandomItemStore(__instance);

            //RANDOMIZED ITEM
            RandomItemStore randomChestItem = Plugin.randomizer.GetRandomizedItem(originalItem.instanceId, __instance);
            s_nextRandomChest = randomChestItem;

            if (randomChestItem.itemType == "FAIRY")
            {
                Plugin.Logger.LogInfo($"Randomized item: Fairy");
                __instance.isFairy = true;
                s_fairiesFound += 1;

                LanguageLine fairyAcquiredText = ScriptableObject.CreateInstance<LanguageLine>();
                fairyAcquiredText.text = $"\"Fairy Acquired\"";
                NPCDialogue.DisplayDialogue(fairyAcquiredText, true);
            } else __instance.isFairy = false;
            if (randomChestItem.itemType == "MONEY")
            {
                Plugin.Logger.LogInfo($"Randomized item: {randomChestItem.moneyQuantity}$");
            }
            else if (randomChestItem.itemName != null)
            {
                Plugin.Logger.LogInfo($"Randomized item: {randomChestItem.itemName} x {randomChestItem.itemQuantity}");
            }
            else
            {
                Plugin.Logger.LogError($"Unhandled randomization for original chest: {__instance.name} ({originalItem.instanceId}). Random Item {randomChestItem.itemContainerName} ({randomChestItem.instanceId}) {randomChestItem.itemType}");
            }

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

        /* NOT NEEDED FOR NOW EVENTUALLY FOR FAIRY CHESTS
        public static void shouldShowAsOpen_Debug_ChestPatch(Chest __instance, ref bool __result)
        {
            if (Plugin.s_openedChests == null) Plugin.s_openedChests = new List<int>();
            if(Plugin.s_openedChests.Contains(__instance.chestID))
            {
                __result = true;
            }
        }
        */

        public static bool moneySprayQuantityFromDatabase_ChestPatch(Chest __instance, ref int __result)
        {
            if (s_nextRandomChest == null) return true;

            if (s_nextRandomChest.itemType == "MONEY")
            {
                __result = s_nextRandomChest.moneyQuantity;
            }

            return false;
        }

        public static bool itemContentsfromDatabase_ChestPatch(Chest __instance, ref Item __result)
        {
            if (s_nextRandomChest == null) return true;

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
            return false;
        }

        public static bool itemQuantityFromDatabase_ChestPatch(Chest __instance, ref int __result)
        {
            if (s_nextRandomChest == null) return true;

            __result = s_nextRandomChest.itemQuantity;
            return false;
        }






        public static bool onGetIt_ItemPickupPatch(ItemPickup __instance)
        {
            //Skip money pickups
            if (__instance.itemToGive.Type == Item.ItemType.MONEY) return true;

            // ORIGINAL ITEM
            RandomItemStore originalItem = RandomItemStore.PickupItemToRandomItemStore(__instance);
            Plugin.Logger.LogInfo($"Original pickup item: {__instance.itemToGive.name} ({__instance.itemToGive.Type}) x {__instance.QuantityToGive}");

            //RANDOMIZED ITEM
            RandomItemStore randomPickupItem = Plugin.randomizer.GetRandomizedItem(originalItem.instanceId, null, __instance);

            if (randomPickupItem.itemType == "FAIRY")
            {
                LanguageLine fairyAcquiredText = ScriptableObject.CreateInstance<LanguageLine>();
                fairyAcquiredText.text = $"\"Fairy Acquired\"";
                NPCDialogue.DisplayDialogue(fairyAcquiredText, true);

                Plugin.Logger.LogInfo($"Randomized item: Fairy");
                s_fairiesFound += 1;
            }
            else if (randomPickupItem.itemType == "MONEY")
            {
                SmallMoneyItem.PlayerQuantity += randomPickupItem.moneyQuantity;
                LanguageLine moneyEarnedText = ScriptableObject.CreateInstance<LanguageLine>();
                moneyEarnedText.text = $"\"You get {randomPickupItem.moneyQuantity} $\"";
                NPCDialogue.DisplayDialogue(moneyEarnedText, true);

                Plugin.Logger.LogInfo($"Randomized item: {randomPickupItem.moneyQuantity}$");
            }
            else if (randomPickupItem.itemName != null)
            {
                // HANDLE TRINKETS
                if (randomPickupItem.itemType == Item.ItemType.TRINKETS.ToString())
                {
                    TrinketItem newItem = null;
                    foreach (TrinketItem trinketItem in Resources.FindObjectsOfTypeAll<TrinketItem>())
                    {
                        if (trinketItem.name == randomPickupItem.itemName)
                        {
                            newItem = trinketItem;
                            break;
                        }
                    }

                    if (newItem == null)
                    {
                        Plugin.Logger.LogError($"Trinket item {s_nextRandomChest.itemName} not found in Assets");
                    }

                    newItem.Quantity += 1;
                    ItemPresentation.PresentItem(newItem, randomPickupItem.itemQuantity);
                    newItem.UnlockAcquisitionAchievement();
                }
                else // HANDLE OTHER ITEMS
                {
                    Item newItem = Inventory.GetItemByName(randomPickupItem.itemName);
                    newItem.Quantity += randomPickupItem.itemQuantity;
                    ItemPresentation.PresentItem(newItem, randomPickupItem.itemQuantity);
                }
                Plugin.Logger.LogInfo($"Randomized item: {randomPickupItem.itemName} ({randomPickupItem.itemType}) x {randomPickupItem.itemQuantity}");
            }
            else
            {
                Plugin.Logger.LogError($"Unhandled randomization for original pickup item: {__instance.name} ({originalItem.instanceId}). Random Item {randomPickupItem.itemContainerName} ({randomPickupItem.instanceId}) {randomPickupItem.itemType}");
            }

            __instance.pickupStateVar.BoolValue = true;

            return false;
        }
    }
}
