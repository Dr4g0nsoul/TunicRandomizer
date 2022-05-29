using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine.Events;

namespace TunicRandomizer.Patches
{
    public class MenuPatches
    {

        public static RandomizerSettings randomizerSettings;
        private static bool showRandomizerOptions = false;

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
            //foreach(MethodInfo info in AccessTools.GetDeclaredMethods(typeof(OptionsGUI)))
            //{
            //    //Plugin.Logger.LogInfo("Method: "+info.Name+" with params:");
            //    //foreach (ParameterInfo pInfo in info.GetParameters())
            //    //{
            //    //    Plugin.Logger.LogInfo(pInfo.ParameterType.FullName + " " + pInfo.Name);
            //    //}
            //}
            //MethodInfo originalDebugAddButton = AccessTools.Method(typeof(OptionsGUI), "addButton", new Type[] { typeof(string), typeof(Il2CppSystem.Action) });
            MethodInfo originalDebugAddButton = AccessTools.Method(typeof(OptionsGUI), "pushDefault");
            MethodInfo patchedDebugAddButton = AccessTools.Method(typeof(MenuPatches), "addButton_OptionsPatch");
            harmony.Patch(originalDebugAddButton, null, new HarmonyMethod(patchedDebugAddButton));
            
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

        public static void addButton_OptionsPatch(OptionsGUI __instance)
        {
            Plugin.Logger.LogInfo("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA "+showRandomizerOptions);
            if (showRandomizerOptions)
            {
                __instance.clearPage();
                //__instance.layoutElements = new Il2CppSystem.Collections.Generic.List<UnityEngine.GameObject> ();
                //__instance.pageStack = new Il2CppSystem.Collections.Generic.List<OptionsGUI.PageStackEntry> ();
                __instance.setHeading("Randomizer settings");
                Il2CppSystem.Action toggleRandomizerAction = new Action(ToggleRandomizer);
                __instance.addButton("Enable Randomizer", true, toggleRandomizerAction);
            }
        }

        private static void ToggleRandomizer()
        {
            Plugin.Logger.LogInfo("HIIIIIIIIIIIII");
        }

        public class RandomizerSettings
        {
            public bool randomizerEnabled = false;
            public int seed = 0;


        }
    }
}
