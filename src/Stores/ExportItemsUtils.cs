using System;
using System.Collections.Generic;
using System.Text;
using TinyJson;
using TunicRandomizer.Patches;
using UnityEngine.SceneManagement;

namespace TunicRandomizer.Stores
{
    public class ExportItemsUtils
    {

        //Item store to export
        public static List<RandomItemStore> s_itemStores;
        public static List<string> s_itemStoresAdded;

        //Location store to export
        public static List<AreaStore> s_areaStores;
        public static List<string> s_areaStoresAdded;

        //Scenes to visit
        public static List<string> s_visited_scenes = new();
        public static Queue<ScenePatches.SpawnPoint> s_scenesToVisit = new ();


        public static void ExportItems()
        {

            //foreach(Chest chest in Resources.FindObjectsOfTypeAll<Chest>())
            //{
            //    Logger.LogInfo($"HI {chest.name} {chest.chestID} {chest.GetInstanceID()}");
            //    s_itemStores.Add(ChestItemStore.ChestToChestItemStore(chest));
            //}

            Plugin.Logger.LogInfo("-----------------------  EXPORT  ---------------------");
            Plugin.Logger.LogInfo("---------------------  EXPORT ITEMS  ---------------------");
            string jsonItems = s_itemStores.ToJson();
            Plugin.Logger.LogInfo(jsonItems);
            Plugin.Logger.LogInfo("-------------------  EXPORT ITEMS END  ---------------------");
            Plugin.Logger.LogInfo("---------------------  EXPORT AREAS  ---------------------");
            string jsonAreas = s_areaStores.ToJson();
            Plugin.Logger.LogInfo(jsonAreas);
            Plugin.Logger.LogInfo("-------------------  EXPORT AREAS END  ---------------------");
            Plugin.Logger.LogInfo("---------------------  EXPORT END  -------------------");

        }

        public static void TraverseNextScene()
        {
            while(s_scenesToVisit.Count > 0)
            {
                ScenePatches.SpawnPoint nextWarp = s_scenesToVisit.Dequeue();
                if(!s_visited_scenes.Contains(nextWarp.scene) && nextWarp.scene != null && nextWarp.scene.Trim().Length > 0 && nextWarp.id != null && nextWarp.id.Trim().Length > 0/* && SceneManager.GetSceneByName(nextWarp.scene).buildIndex >= 0*/)
                {
                    Plugin.Logger.LogWarning($"NEXT WARP: {nextWarp.scene} :: {nextWarp.id}");
                    s_visited_scenes.Add(nextWarp.scene);
                    Cheats.CheatWarpToPortal(nextWarp.scene, nextWarp.id);
                    break;
                }
            }
            if (s_scenesToVisit.Count <= 0)
            {
                ExportItems();
            }
        }


    }
}
