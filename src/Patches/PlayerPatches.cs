﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TunicRandomizer;
using UnityEngine;

namespace TunicRandomizer.Patches
{
    public class PlayerPatches
    {

        public static string s_spawnInput;
        public static bool s_spawnInputStarted = false;

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
            if(Input.GetKeyDown(KeyCode.O))
            {
                if (Plugin.s_chestItemList == null) Plugin.s_chestItemList= new Queue<Chest>();
                Chest warpChest = Plugin.s_chestItemList.Dequeue();
                __instance.transform.position = warpChest.characterOpeningTransform.position;
            }
            else if(Input.GetKeyDown(KeyCode.I))
            {
                Plugin.ExportItems();
            }
            else if(Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if(s_spawnInputStarted)
                {
                    ScenePatches.SpawnPoint warpPoint = null;
                    int warpIndex;
                    if(Int32.TryParse(s_spawnInput, out warpIndex))
                    {
                        if(warpIndex >= 0 && warpIndex < Plugin.s_spawnPoints.Count)
                        {
                            warpPoint = Plugin.s_spawnPoints[warpIndex];
                        }
                    }
                    if(warpPoint != null)
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
            
            if(s_spawnInputStarted)
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
