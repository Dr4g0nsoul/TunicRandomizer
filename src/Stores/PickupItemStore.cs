using System;
using System.Collections.Generic;
using System.Text;

namespace TunicRandomizer.Stores
{
    public class PickupItemStore : ItemStore
    {


        public PickupItemStore(string itemName, string denominationString, int itemQuantity, string itemType, string sceneName, int sceneId, string itemNearestExit)
            : base(itemName, denominationString, itemQuantity, itemType/*, sceneName, sceneId, itemNearestExit*/)
        {
        }
    }
}
