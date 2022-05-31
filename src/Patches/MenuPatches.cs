using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace TunicRandomizer.Patches
{
    public class MenuPatches
    {

        public static RandomizerSettings randomizerSettings;

        private static bool showRandomizerOptions = false;

        private static OptionsGUIButton seedButton = null;

        public static void ApplyPatches(Harmony harmony)
        {
            //// MAIN MENU PATCHES
            //MethodInfo originalNewGame = AccessTools.Method(typeof(TitleScreen), "__NewGame");
            //MethodInfo patchedNewGame = AccessTools.Method(typeof(MenuPatches), "__NewGame_MenuPatch");
            //harmony.Patch(originalNewGame, new HarmonyMethod(patchedNewGame));

            //MethodInfo originalOptions = AccessTools.Method(typeof(TitleScreen), "__Options");
            //MethodInfo patchedOptions = AccessTools.Method(typeof(MenuPatches), "__Options_MenuPatch");
            //harmony.Patch(originalOptions, new HarmonyMethod(patchedOptions));

            ////OPTION MENU PATHES
            //MethodInfo originalAddButtons = AccessTools.Method(typeof(OptionsGUI), "pushDefault");
            //MethodInfo patchedAddButtons = AccessTools.Method(typeof(MenuPatches), "pushDefault_OptionsPatch");
            //harmony.Patch(originalAddButtons, null, new HarmonyMethod(patchedAddButtons));

            //MethodInfo originalOptionsUpdate = AccessTools.Method(typeof(OptionsGUI), "Update");
            //MethodInfo patchedOptionsUpdate = AccessTools.Method(typeof(MenuPatches), "Update_OptionsPatch");
            //harmony.Patch(originalOptionsUpdate, new HarmonyMethod(patchedOptionsUpdate));
        }

        public static bool __NewGame_MenuPatch(TitleScreen __instance)
        {
            showRandomizerOptions = true;
            if(randomizerSettings == null) randomizerSettings = new RandomizerSettings();
            __instance.__Options();
            return false;
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

                Il2CppSystem.Action startStandardAction = new Action(StartStandard);
                __instance.addButton("Start new game", true, startStandardAction);



                OptionsGUIMultiSelect.MultiSelectAction toggleRandomizerAction = new Action<int>((enabled) => randomizerSettings.randomizerEnabled = enabled == 1 ? true : false);
                __instance.addToggle("Enable Randomizer", "Disabled", "Enabled", 0, toggleRandomizerAction);

                randomizerSettings.seed = Environment.TickCount;
                Il2CppSystem.Action changeSeedAction = new Action(ChangeSeed);
                seedButton = __instance.addButton("Seed", randomizerSettings.seed + "", changeSeedAction);



                OptionsGUIMultiSelect.MultiSelectAction toggleRandomizeEquipmentAction = new Action<int>((enabled) => randomizerSettings.randomizeEquipment = enabled == 1 ? true : false);
                __instance.addToggle("Enable Randomizer", "Disabled", "Enabled", randomizerSettings.randomizeEquipment ? 1 : 0, toggleRandomizerAction);

                OptionsGUIMultiSelect.MultiSelectAction toggleRandomizeConsumablesAction = new Action<int>((enabled) => randomizerSettings.randomizeConsumables = enabled == 1 ? true : false);
                __instance.addToggle("Enable Randomizer", "Disabled", "Enabled", randomizerSettings.randomizeConsumables ? 1 : 0, toggleRandomizeConsumablesAction);

                OptionsGUIMultiSelect.MultiSelectAction toggleRandomizeFlaskContainersAndShardsAction = new Action<int>((enabled) => randomizerSettings.randomizeFlaskContainersAndShards = enabled == 1 ? true : false);
                __instance.addToggle("Enable Randomizer", "Disabled", "Enabled", randomizerSettings.randomizeFlaskContainersAndShards ? 1 : 0, toggleRandomizeFlaskContainersAndShardsAction);

                OptionsGUIMultiSelect.MultiSelectAction toggleRandomizeOfferingsAction = new Action<int>((enabled) => randomizerSettings.randomizeOfferings = enabled == 1 ? true : false);
                __instance.addToggle("Enable Randomizer", "Disabled", "Enabled", randomizerSettings.randomizeOfferings ? 1 : 0, toggleRandomizeOfferingsAction);

                OptionsGUIMultiSelect.MultiSelectAction toggleRandomizeMoneyAction = new Action<int>((enabled) => randomizerSettings.randomizeMoney = enabled == 1 ? true : false);
                __instance.addToggle("Enable Randomizer", "Disabled", "Enabled", randomizerSettings.randomizeMoney ? 1 : 0, toggleRandomizeMoneyAction);

                OptionsGUIMultiSelect.MultiSelectAction toggleRandomizeEquipmentSlotsAction = new Action<int>((enabled) => randomizerSettings.randomizeEquipmentSlots = enabled == 1 ? true : false);
                __instance.addToggle("Enable Randomizer", "Disabled", "Enabled", randomizerSettings.randomizeEquipmentSlots ? 1 : 0, toggleRandomizeEquipmentSlotsAction);

                OptionsGUIMultiSelect.MultiSelectAction toggleRandomizeHexagonsAction = new Action<int>((enabled) => randomizerSettings.randomizeHexagons = enabled == 1 ? true : false);
                __instance.addToggle("Enable Randomizer", "Disabled", "Enabled", randomizerSettings.randomizeHexagons ? 1 : 0, toggleRandomizeHexagonsAction);

                OptionsGUIMultiSelect.MultiSelectAction toggleRandomizeFairiesAction = new Action<int>((enabled) => randomizerSettings.randomizeFairies = enabled == 1 ? true : false);
                __instance.addToggle("Enable Randomizer", "Disabled", "Enabled", randomizerSettings.randomizeFairies ? 1 : 0, toggleRandomizeFairiesAction);

                OptionsGUIMultiSelect.MultiSelectAction toggleRandomizeTrophiesAction = new Action<int>((enabled) => randomizerSettings.randomizeTrophies = enabled == 1 ? true : false);
                __instance.addToggle("Enable Randomizer", "Disabled", "Enabled", randomizerSettings.randomizeTrophies ? 1 : 0, toggleRandomizeTrophiesAction);
            }
        }

        public static void Update_OptionsPatch()
        {
            if(seedButton.button.currentSelectionState == UnityEngine.UI.Selectable.SelectionState.Selected)
            {
                if (seedButton.secondaryText.text.Length < 10)
                {
                    if (seedButton.secondaryText.text.Length > 0 && (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)))
                    {
                        seedButton.secondaryText.text += "0";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                    {
                        seedButton.secondaryText.text += "1";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                    {
                        seedButton.secondaryText.text += "2";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                    {
                        seedButton.secondaryText.text += "3";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                    {
                        seedButton.secondaryText.text += "4";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
                    {
                        seedButton.secondaryText.text += "5";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
                    {
                        seedButton.secondaryText.text += "6";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
                    {
                        seedButton.secondaryText.text += "7";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
                    {
                        seedButton.secondaryText.text += "8";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
                    {
                        seedButton.secondaryText.text += "9";
                    }
                }
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    if (seedButton.secondaryText.text.Length > 0)
                    {
                        randomizerSettings.seed = int.Parse(seedButton.secondaryText.text);
                    }
                    else
                    {
                        seedButton.secondaryText.text = randomizerSettings.seed+"";
                    }
                }
                else if(Input.GetKeyDown(KeyCode.Backspace) && seedButton.secondaryText.text.Length > 0)
                {
                    seedButton.secondaryText.text = seedButton.secondaryText.text.Remove(seedButton.secondaryText.text.Length - 1);
                }
            }
            else if (seedButton.secondaryText.text.Length < 1)
            {
                randomizerSettings.seed = Environment.TickCount;
                seedButton.secondaryText.text = randomizerSettings.seed+"";
            } 
            else if (seedButton.secondaryText.text != (randomizerSettings.seed + ""))
            {
                randomizerSettings.seed = int.Parse(seedButton.secondaryText.text);
            }
        }


        private static void StartStandard()
        {

        }

        private static void ChangeSeed()
        {
        }

        public class RandomizerSettings
        {
            public bool randomizerEnabled = false;
            public int seed = 0;
            
            //Randomization pool
            public bool randomizeEquipment = true;
            public bool randomizeConsumables = true; //Supplies
            public bool randomizeFlaskContainersAndShards = true;
            public bool randomizeOfferings = true;
            public bool randomizeMoney = true;
            public bool randomizeEquipmentSlots = true;
            public bool randomizeHexagons = false;
            public bool randomizeFairies = false;
            public bool randomizeTrophies = false;

            //Additional randomization features
            public bool ensureEquipmentInEquipmentLocation = true;
            public bool progressiveStickToSword = true;
            public bool bushesOnlyDestructableBySword = true; //will probably deafault to false later


        }
    }
}
