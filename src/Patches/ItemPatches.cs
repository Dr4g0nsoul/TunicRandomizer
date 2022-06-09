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
using System.Threading.Tasks;

namespace TunicRandomizer.Patches
{
    public class ItemPatches
    {

        public static int s_fairiesFound = 0;
        public static bool s_isOriginalFairyChest = false;
        public static bool s_isOpeningChest = false;

        public static TunicArchipelagoClient.TunicArchipelagoItem s_nextArchipelagoItem = null;

        private static bool s_chestOpeningFailed = false;

        public static void ApplyPatches(Harmony harmony)
        {
            /* ITEM RANDO PATCHES */
            MethodInfo originalOpenChest = AccessTools.Method(typeof(PlayerCharacter), "PlayOpenChestAnimation");
            MethodInfo patchedOpenChest = AccessTools.Method(typeof(ItemPatches), "PlayOpenChestAnimation_PlayerPatch");
            harmony.Patch(originalOpenChest, new HarmonyMethod(patchedOpenChest));

            MethodInfo originalOpenChestInterrupt = AccessTools.Method(typeof(Chest), "InterruptOpening");
            MethodInfo patchedOpenChestInterrupt = AccessTools.Method(typeof(ItemPatches), "InterruptOpening_ChestPatch");
            harmony.Patch(originalOpenChestInterrupt, new HarmonyMethod(patchedOpenChestInterrupt));


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

        public static bool PlayOpenChestAnimation_PlayerPatch(PlayerCharacter __instance, Chest chest)
        {
            s_isOriginalFairyChest = false;
            // ORIGINAL ITEM
            Item item = null;
            int money = 0;
            if (!chest.isFairy)
            {
                try
                {
                    item = chest.itemContentsfromDatabase;
                }
                catch (Exception) { }
                try
                {
                    money = chest.moneySprayQuantityFromDatabase;
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
                Plugin.Logger.LogWarning($"Chest: {chest.name} has no valid contents or is fairy chest");
                s_isOriginalFairyChest = true;
            }

            //Plugin.s_openedChests.Add(__instance.chestID); not needed anymore
            RandomItemStore originalItem = RandomItemStore.ChestToRandomItemStore(chest);
            if(TunicArchipelagoClient.IsConnected)
            {
                s_isOpeningChest = true;
                s_chestOpeningFailed = false;
                chest.isFairy = false;
                int openingDelay = (int)(PlayerCharacter.openChestAnimationTransitionDuration * 1000) + 1000;

                if (TunicArchipelagoClient.IsConnected && s_isOpeningChest)
                {
                    Task.Delay(openingDelay).ContinueWith((task) =>
                    {
                        s_isOpeningChest = false;
                        if(s_chestOpeningFailed)
                        {
                            Plugin.Logger.LogInfo($"Failed to open chest");
                        }
                        else
                        {
                            Plugin.Logger.LogInfo($"Chest was successfully opened after {openingDelay}ms with additional check delay of 1000ms");
                            TunicArchipelagoClient.CheckLocation(originalItem);
                        }
                    });
                }
            }
            

            return true;

        }

        public static void InterruptOpening_ChestPatch()
        {
            Plugin.Logger.LogInfo("Interrupt chest opening!");
            s_chestOpeningFailed = true;
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
            if (!TunicArchipelagoClient.IsConnected || !s_isOpeningChest) return true;
            
            __result = 0;

            return false;
        }

        public static bool itemContentsfromDatabase_ChestPatch(Chest __instance, ref Item __result)
        {
            if (!TunicArchipelagoClient.IsConnected || !s_isOpeningChest) return true;

            __result = null;

            return false;
        }

        public static bool itemQuantityFromDatabase_ChestPatch(Chest __instance, ref int __result)
        {
            if (!TunicArchipelagoClient.IsConnected || !s_isOpeningChest) return true;

            __result = 0;

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
            Plugin.Logger.LogInfo("Handing out new item");

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
                        Plugin.Logger.LogError($"Trinket item {archipelagoItem.itemName} not found in Assets");
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
