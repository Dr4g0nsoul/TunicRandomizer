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

        public static List<string> s_visited_scenes = new();
        public static Queue<ScenePatches.SpawnPoint> s_scenesToVisit = new ();


        public static void ExportItems()
        {

            //foreach(Chest chest in Resources.FindObjectsOfTypeAll<Chest>())
            //{
            //    Logger.LogInfo($"HI {chest.name} {chest.chestID} {chest.GetInstanceID()}");
            //    s_itemStores.Add(ChestItemStore.ChestToChestItemStore(chest));
            //}

            string json = Plugin.s_itemStores.ToJson();
            Plugin.Logger.LogInfo("-----------------------EXPORT---------------------");
            Plugin.Logger.LogInfo(json);
            Plugin.Logger.LogInfo("-----------------------EXPORT END---------------------");

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
