using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TunicRandomizer;
using UnhollowerBaseLib;
using UnityEngine;

namespace TunicRandomizer.Patches
{
    public class PlayerPatches
    {

        public static string s_spawnInput;
        public static bool s_spawnInputStarted = false;


        public static void ApplyPatches(Harmony harmony)
        {
            /* PLAYER PATCHES */
            //MethodInfo playerStartOriginal = AccessTools.Method(typeof(PlayerCharacter), "Start");
            //MethodInfo playerStartPatch = AccessTools.Method(typeof(PlayerPatches), "Start_PlayerPatches");
            //harmony.Patch(playerStartOriginal, null, new HarmonyMethod(playerStartPatch));

            MethodInfo playerUpdateOriginal = AccessTools.Method(typeof(PlayerCharacter), "Update");
            MethodInfo playerUpdatePatch = AccessTools.Method(typeof(PlayerPatches), "Update_PlayerPatches");
            harmony.Patch(playerUpdateOriginal, null, new HarmonyMethod(playerUpdatePatch));
        }

        public static void Start_PlayerPatches(PlayerCharacter __instance)
        {
            if (__instance != null && __instance.maxWalkSpeed != null)
            {
                __instance.maxWalkSpeed.Value = 30f;
                //__instance.ModifyAttackPower(9000f);
                //__instance.attackUpgradeExponentBase.Value = 9000f;
                //Item attackItem = Inventory.GetItemByName("Upgrade Offering - Attack - Tooth");
                //attackItem.Quantity = 10;
                //Inventory.itemList.Add(attackItem);
            }
            //__instance.maxWalkSpeed.Value = 9000f;
        }

        public static void Update_PlayerPatches(PlayerCharacter __instance)
        {
            if (false && Input.GetKeyDown(KeyCode.O)) //Disable warp to chest functionality for now
            {
                if (Plugin.s_sceneItemList == null) Plugin.s_sceneItemList = new Queue<Transform>();
                Transform warpItemTransform = Plugin.s_sceneItemList.Dequeue();
                __instance.transform.position = warpItemTransform.position;
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                Item helpItem = Inventory.GetItemByName("Firecracker");
                int helpItemGiveQuantity = 4;

                int quantityDelta = Mathf.Max(helpItemGiveQuantity - helpItem.Quantity, 0);
                if (quantityDelta > 0)
                {
                    __instance.hp /= 2;
                    helpItem.Quantity = helpItemGiveQuantity;
                    ItemPresentation.PresentItem(helpItem, quantityDelta);
                } 
                else
                {
                    LanguageLine maxHelpItemReachedText = ScriptableObject.CreateInstance<LanguageLine>();
                    maxHelpItemReachedText.text = $"\"You cannot have more than \"";
                    NPCDialogue.DisplayDialogue(maxHelpItemReachedText, true);
                }

            }
            else if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (s_spawnInputStarted)
                {
                    ScenePatches.SpawnPoint warpPoint = null;
                    int warpIndex;
                    if (Int32.TryParse(s_spawnInput, out warpIndex))
                    {
                        if (warpIndex >= 0 && warpIndex < Plugin.s_spawnPoints.Count)
                        {
                            warpPoint = Plugin.s_spawnPoints[warpIndex];
                        }
                    }
                    if (warpPoint != null)
                    {
                        Plugin.Logger.LogInfo($"Warp to warpPoint ({warpIndex}) in scene {warpPoint.scene} at destination {warpPoint.id}");
                        __instance.transform.position = warpPoint.position;
                    }
                    else
                    {
                        Plugin.Logger.LogInfo($"Invalid warp point {s_spawnInput}");
                    }
                }
                else
                {
                    s_spawnInput = "";
                }
                s_spawnInputStarted = !s_spawnInputStarted;
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                LanguageLine showSeed = ScriptableObject.CreateInstance<LanguageLine>();
                showSeed.text = $"\"Seed: {Plugin.randomizer.Seed}\"";
                NPCDialogue.DisplayDialogue(showSeed, false);
            }
            else if(Input.GetKeyDown(KeyCode.N))
            {
                bool isNight = !CycleController.IsNight;
                CycleController.IsNight = isNight;
                CycleController.nightStateVar.BoolValue = isNight;
            }

            if (s_spawnInputStarted)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0)) s_spawnInput += "0";
                else if(Input.GetKeyDown(KeyCode.Alpha1)) s_spawnInput += "1";
                else if(Input.GetKeyDown(KeyCode.Alpha2)) s_spawnInput += "2";
                else if(Input.GetKeyDown(KeyCode.Alpha3)) s_spawnInput += "3";
                else if(Input.GetKeyDown(KeyCode.Alpha4)) s_spawnInput += "4";
                else if(Input.GetKeyDown(KeyCode.Alpha5)) s_spawnInput += "5";
                else if(Input.GetKeyDown(KeyCode.Alpha6)) s_spawnInput += "6";
                else if(Input.GetKeyDown(KeyCode.Alpha7)) s_spawnInput += "7";
                else if(Input.GetKeyDown(KeyCode.Alpha8)) s_spawnInput += "8";
                else if(Input.GetKeyDown(KeyCode.Alpha9)) s_spawnInput += "9";
            }
        }
    }
}
