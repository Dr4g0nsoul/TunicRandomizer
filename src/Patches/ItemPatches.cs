using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TunicRandomizer.TunicArchipelago;
using TunicRandomizer.Stores;
using UnityEngine;

namespace TunicRandomizer.Patches
{
    public class ItemPatches
    {

        public static int s_fairiesFound = 0;
        public static TunicArchipelagoClient.TunicArchipelagoItem s_nextRandomItem = null;
        public static bool s_isOriginalFairyChest = false;
        public static bool s_isOpeningChest = false;

        public static void ApplyPatches(Harmony harmony)
        {
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
            harmony.Patch(originalChestMoney, new HarmonyMethod(patchedChestMoney));

            MethodInfo originalChestItem = AccessTools.PropertyGetter(typeof(Chest), "itemContentsfromDatabase");
            MethodInfo patchedChestItem = AccessTools.Method(typeof(ItemPatches), "itemContentsfromDatabase_ChestPatch");
            harmony.Patch(originalChestItem, new HarmonyMethod(patchedChestItem));

            MethodInfo originalChestItemQuantity = AccessTools.PropertyGetter(typeof(Chest), "itemQuantityFromDatabase");
            MethodInfo patchedChestItemQuantity = AccessTools.Method(typeof(ItemPatches), "itemQuantityFromDatabase_ChestPatch");
            harmony.Patch(originalChestItemQuantity, new HarmonyMethod(patchedChestItemQuantity));

            MethodInfo originalPickupItem = AccessTools.Method(typeof(ItemPickup), "onGetIt");
            MethodInfo patchedPickupItem = AccessTools.Method(typeof(ItemPatches), "onGetIt_ItemPickupPatch");
            harmony.Patch(originalPickupItem, new HarmonyMethod(patchedPickupItem));

        }

        public static bool IInteractionReceiver_Interact_ChestPatch(Item i, Chest __instance)
        {
            s_isOriginalFairyChest = false;
            s_nextRandomItem = null;
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
            if(TunicArchipelagoClient.IsConnected)
            {
                s_isOpeningChest = true;
                TunicArchipelagoClient.CheckLocation(originalItem);
            }
            else
            {
                s_nextRandomItem = null;
            }

            //RANDOMIZED ITEM
            /*
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
            */

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
            if (s_nextRandomItem == null) return true;

            if (s_nextRandomItem.itemType == TunicArchipelagoClient.TunicArchipelagoItemType.Money)
            {
                __result = s_nextRandomItem.quantity;
            }

            return false;
        }

        public static bool itemContentsfromDatabase_ChestPatch(Chest __instance, ref Item __result)
        {
            if (s_nextRandomItem == null) return true;

            __result = null;

            if (s_nextRandomItem.itemName != null)
            {
                // HANDLE TRINKETS
                if (s_nextRandomItem.itemType == TunicArchipelagoClient.TunicArchipelagoItemType.Trinket)
                {
                    TrinketItem newItem = null;
                    foreach (TrinketItem trinketItem in Resources.FindObjectsOfTypeAll<TrinketItem>())
                    {
                        if (trinketItem.name == s_nextRandomItem.itemName)
                        {
                            newItem = trinketItem;
                            break;
                        }
                    }

                    if (newItem == null)
                    {
                        Plugin.Logger.LogError($"Trinket item {s_nextRandomItem.itemName} not found in Assets");
                    }
                    else
                    {
                        __result = newItem;
                    }
                }
                else // HANDLE OTHER ITEMS
                {
                    __result = Inventory.GetItemByName(s_nextRandomItem.itemName);
                }
            }
            return false;
        }

        public static bool itemQuantityFromDatabase_ChestPatch(Chest __instance, ref int __result)
        {
            if (s_nextRandomItem == null) return true;

            __result = s_nextRandomItem.quantity;
            return false;
        }






        public static bool onGetIt_ItemPickupPatch(ItemPickup __instance)
        {
            if (TunicArchipelagoClient.IsConnected)
            {
                // ORIGINAL ITEM
                RandomItemStore originalItem = RandomItemStore.PickupItemToRandomItemStore(__instance);
                Plugin.Logger.LogInfo($"Original pickup item: {__instance.itemToGive.name} ({__instance.itemToGive.Type}) x {__instance.QuantityToGive}");
                TunicArchipelagoClient.CheckLocation(originalItem);
                __instance.pickupStateVar.BoolValue = true;

                return false;
            }
            return true;

        }

        public static void AwardItemToPlayer(TunicArchipelagoClient.TunicArchipelagoItem archipelagoItem)
        {
            //RANDOMIZED ITEM
            if (archipelagoItem.itemType == TunicArchipelagoClient.TunicArchipelagoItemType.Fairy)
            {
                LanguageLine fairyAcquiredText = ScriptableObject.CreateInstance<LanguageLine>();
                fairyAcquiredText.text = $"\"Fairy Acquired\"";
                NPCDialogue.DisplayDialogue(fairyAcquiredText, true);

                Plugin.Logger.LogInfo($"Randomized item: Fairy");
                s_fairiesFound += 1;
            }
            else if (archipelagoItem.itemType == TunicArchipelagoClient.TunicArchipelagoItemType.Money)
            {
                SmallMoneyItem.PlayerQuantity += archipelagoItem.quantity;
                LanguageLine moneyEarnedText = ScriptableObject.CreateInstance<LanguageLine>();
                moneyEarnedText.text = $"\"You get {archipelagoItem.quantity} $\"";
                NPCDialogue.DisplayDialogue(moneyEarnedText, true);

                Plugin.Logger.LogInfo($"Randomized item: {archipelagoItem.quantity}$");
            }
            else if (archipelagoItem.itemName != null)
            {
                // HANDLE TRINKETS
                if (archipelagoItem.itemType == TunicArchipelagoClient.TunicArchipelagoItemType.Trinket)
                {
                    TrinketItem newItem = null;
                    foreach (TrinketItem trinketItem in Resources.FindObjectsOfTypeAll<TrinketItem>())
                    {
                        if (trinketItem.name == archipelagoItem.itemName)
                        {
                            newItem = trinketItem;
                            break;
                        }
                    }

                    if (newItem == null)
                    {
                        Plugin.Logger.LogError($"Trinket item {s_nextRandomItem.itemName} not found in Assets");
                    }

                    newItem.Quantity += 1;
                    ItemPresentation.PresentItem(newItem, archipelagoItem.quantity);
                    newItem.UnlockAcquisitionAchievement();
                }
                else // HANDLE OTHER ITEMS
                {
                    Item newItem = Inventory.GetItemByName(archipelagoItem.itemName);
                    newItem.Quantity += archipelagoItem.quantity;
                    ItemPresentation.PresentItem(newItem, archipelagoItem.quantity);
                }
                Plugin.Logger.LogInfo($"Randomized item: {archipelagoItem.itemName} ({archipelagoItem.itemType}) x {archipelagoItem.quantity}");
            }
            else
            {
                Plugin.Logger.LogError($"Unhandled randomization for original pickup item: {archipelagoItem.itemName} ({archipelagoItem.itemType}) [{archipelagoItem.archipelagoItemId}].");
            }
        }
    }
}
