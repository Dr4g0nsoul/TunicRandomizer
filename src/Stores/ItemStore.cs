using System;
using System.Collections.Generic;
using System.Text;

namespace TunicRandomizer.Stores
{
    [Serializable]
    public abstract class ItemStore
    {

        public string itemName;
        public string denominationString;
        public int itemQuantity;
        public string itemType;
        //public string sceneName;
        //public int sceneId;
        //public string itemNearestExit;

        public ItemStore (string itemName, string denominationString, int itemQuantity, string itemType/*, string sceneName, int sceneId, string itemNearestExit*/)
        {
            this.itemName = itemName;
            this.denominationString = denominationString;
            this.itemQuantity = itemQuantity;
            this.itemType = itemType;
            //this.sceneName = sceneName;
            //this.sceneId = sceneId;
            //this.itemNearestExit = itemNearestExit;
        }
    }
}
