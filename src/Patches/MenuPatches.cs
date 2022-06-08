using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using TunicRandomizer.TunicArchipelago;

namespace TunicRandomizer.Patches
{
    public class MenuPatches
    {

        //public static RandomizerSettings randomizerSettings;
        private static Char[] s_azList = new Char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        private static bool showRandomizerOptions = false;
        private static bool allowNewGame = false;
        private static TitleScreen titleScreen;

        private static OptionsGUIButton hostButton = null;
        private static OptionsGUIButton portButton = null;
        private static OptionsGUIButton userButton = null;
        private static OptionsGUIButton passwordButton = null;

        public static void ApplyPatches(Harmony harmony)
        {
            // MAIN MENU PATCHES
            MethodInfo originalNewGame = AccessTools.Method(typeof(TitleScreen), "__NewGame");
            MethodInfo patchedNewGame = AccessTools.Method(typeof(MenuPatches), "__NewGame_MenuPatch");
            harmony.Patch(originalNewGame, new HarmonyMethod(patchedNewGame));

            MethodInfo originalOptions = AccessTools.Method(typeof(TitleScreen), "__Options");
            MethodInfo patchedOptions = AccessTools.Method(typeof(MenuPatches), "__Options_MenuPatch");
            harmony.Patch(originalOptions, new HarmonyMethod(patchedOptions));

            //OPTION MENU PATHES
            MethodInfo originalAddButtons = AccessTools.Method(typeof(OptionsGUI), "pushDefault");
            MethodInfo patchedAddButtons = AccessTools.Method(typeof(MenuPatches), "pushDefault_OptionsPatch");
            harmony.Patch(originalAddButtons, null, new HarmonyMethod(patchedAddButtons));

            MethodInfo originalOptionsUpdate = AccessTools.Method(typeof(OptionsGUI), "Update");
            MethodInfo patchedOptionsUpdate = AccessTools.Method(typeof(MenuPatches), "Update_OptionsPatch");
            harmony.Patch(originalOptionsUpdate, new HarmonyMethod(patchedOptionsUpdate));
        }

        public static bool __NewGame_MenuPatch(TitleScreen __instance)
        {
            if(!allowNewGame)
            {
                titleScreen = __instance;
                showRandomizerOptions = true;
                __instance.__Options();
                Plugin.Logger.LogInfo("GOTO randomizer options");
                return false;
            }
            Plugin.Logger.LogInfo("GOTO new game");
            __instance.gameObject.SetActive(true);
            __instance.lockout = false;
            return true;
        }

        public static bool __Options_MenuPatch(TitleScreen __instance)
        {
            return true;
        }

        public static void pushDefault_OptionsPatch(OptionsGUI __instance)
        {
            if (showRandomizerOptions)
            {
                __instance.clearPage();
                __instance.setHeading("Randomizer settings");

                __instance.addButton("Start new game", true, new Action(() =>
                {
                    __instance.exitOptions();
                    allowNewGame = true;
                    titleScreen.__NewGame();
                }));

                Il2CppSystem.Action doNothingAction = new Action(DoNothing);
                hostButton = __instance.addButton("Host", "localhost", doNothingAction);
                portButton = __instance.addButton("Port", "1234", doNothingAction);
                userButton = __instance.addButton("User", "Player", doNothingAction);
                passwordButton = __instance.addButton("Password", "", doNothingAction);

                __instance.addButton("Start randomized game", new Action(() =>
                {
                    if(TunicArchipelagoClient.Connect(
                        hostButton.secondaryText.text,
                        int.Parse(portButton.secondaryText.text),
                        userButton.secondaryText.text,
                        passwordButton.secondaryText.text))
                    {
                        __instance.exitOptions();
                        allowNewGame = true;
                        titleScreen.__NewGame();
                    }
                }));

            }
        }

        public static void Update_OptionsPatch()
        {
            OptionsGUIButton currSelectedButton = null;
            if(hostButton.button.currentSelectionState == UnityEngine.UI.Selectable.SelectionState.Selected)
            {
                currSelectedButton = hostButton;
            }
            else if(portButton.button.currentSelectionState == UnityEngine.UI.Selectable.SelectionState.Selected)
            {
                currSelectedButton = portButton;
            }
            else if (userButton.button.currentSelectionState == UnityEngine.UI.Selectable.SelectionState.Selected)
            {
                currSelectedButton = userButton;
            }
            else if (passwordButton.button.currentSelectionState == UnityEngine.UI.Selectable.SelectionState.Selected)
            {
                currSelectedButton = passwordButton;
            }
            if (currSelectedButton != null)
            {
                //Numbers 0 through 9: 48 - 57
                for(int i = 48; i < 58; i++)
                {
                    if(Input.GetKeyDown((KeyCode)i))
                    {
                        currSelectedButton.secondaryText.text += (i - 48);
                    }
                }
                //Keypad Numbers 0 through 9: 256 - 265
                for (int i = 256; i < 266; i++)
                {
                    if (Input.GetKeyDown((KeyCode)i))
                    {
                        currSelectedButton.secondaryText.text += (i - 256);
                    }
                }

                //Text Keys A through Z: 97 - 122
                bool isUppercase = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                for (int i = 97; i < 123; i++)
                {
                    if (Input.GetKeyDown((KeyCode)i))
                    {
                        if(isUppercase)
                        {
                            currSelectedButton.secondaryText.text += Char.ToUpper(s_azList[i - 97]);
                        }
                        else
                        {
                            currSelectedButton.secondaryText.text += s_azList[i - 97];
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    currSelectedButton.secondaryText.text = "";
                }
                else if(Input.GetKeyDown(KeyCode.Backspace) && currSelectedButton.secondaryText.text.Length > 0)
                {
                    currSelectedButton.secondaryText.text = currSelectedButton.secondaryText.text.Remove(currSelectedButton.secondaryText.text.Length - 1);
                }
            }
        }

        private static void DoNothing()
        {
        }

        //public class RandomizerSettings
        //{
        //    public bool randomizerEnabled = false;
        //    public int seed = 0;
            
        //    //Randomization pool
        //    public bool randomizeEquipment = true;
        //    public bool randomizeConsumables = true; //Supplies
        //    public bool randomizeFlaskContainersAndShards = true;
        //    public bool randomizeOfferings = true;
        //    public bool randomizeMoney = true;
        //    public bool randomizeEquipmentSlots = true;
        //    public bool randomizeHexagons = false;
        //    public bool randomizeFairies = false;
        //    public bool randomizeTrophies = false;

        //    //Additional randomization features
        //    public bool ensureEquipmentInEquipmentLocation = true;
        //    public bool progressiveStickToSword = true;
        //    public bool bushesOnlyDestructableBySword = true; //will probably deafault to false later


        //}
    }
}
