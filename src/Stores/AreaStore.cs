using System;
using System.Collections.Generic;
using System.Text;

namespace TunicRandomizer.Stores
{
    public class AreaStore
    {
        public string sceneName;
        public List<AreaExit> areaExits;

        public class AreaExit
        {
            public string destinationSceneName;
            public string destinationLocation;
        }
    }
}
