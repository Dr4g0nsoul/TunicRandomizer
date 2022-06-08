using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using TunicRandomizer.Patches;
using TunicRandomizer.Stores;

namespace TunicRandomizer.TunicArchipelago
{
    public class TunicArchipelagoClient
    {
        private const string TUNIC_GAME_ID = "Tunic";

        private static ArchipelagoSession s_currentSession;
        private static List<string> s_checkedLocations = new List<string>();
        public static bool IsConnected { get => s_currentSession != null && s_currentSession.Socket.Connected; }
        public static string Seed
        {
            get
            {
                if (s_currentSession == null || !s_currentSession.Socket.Connected) return "NOT CONNECTED";
                return s_currentSession.RoomState.Seed;
            }
        }

        public static bool Connect(string host, int port, string player, string password = "")
        {
            if(s_currentSession != null && s_currentSession.Socket.Connected)
            {
                s_currentSession.Socket.Disconnect();
            }

            s_currentSession = ArchipelagoSessionFactory.CreateSession(host, port);
            LoginResult res = s_currentSession.TryConnectAndLogin(TUNIC_GAME_ID, player, new Version(0,3,2), Archipelago.MultiClient.Net.Enums.ItemsHandlingFlags.AllItems);

            Plugin.Logger.LogInfo($"Trying to connect to {host}:{port}");
            Plugin.Logger.LogInfo($"connected: {res.Successful}");

            if (res.Successful)
            {

                foreach (long location in s_currentSession.Locations.AllLocationsChecked)
                {
                    string checkedLocation = GenerateLocation(s_currentSession.Locations.GetLocationNameFromId(location));
                    s_checkedLocations = new List<string>();
                    if (!s_checkedLocations.Contains(checkedLocation))
                    {
                        s_checkedLocations.Add(checkedLocation);
                    }
                }

                s_currentSession.Items.ItemReceived += (helper) => RecieveItemEvent(helper);
                return true;
            }
            return false;
        }

        private static void RecieveItemEvent(ReceivedItemsHelper helper)
        {
            string newItemName = helper.PeekItemName();
            TunicArchipelagoItem archipelagoItem = GenerateArchipelagoItem(newItemName);
            ItemPatches.s_nextRandomItem = archipelagoItem;
            if(!ItemPatches.s_isOpeningChest)
            {
                ItemPatches.AwardItemToPlayer(archipelagoItem);
            }
            ItemPatches.s_isOpeningChest = false;
            helper.DequeueItem();
        }

        public static void CheckLocation(RandomItemStore tunicItem)
        {
            string locationName;
            if(tunicItem.itemType == "MONEY")
            {
                locationName = $"{tunicItem.moneyQuantity}$ ({tunicItem.itemType}) [{tunicItem.instanceId}]";
            }
            else if(tunicItem.itemType == "FAIRY")
            {
                locationName = $"Fairy x 0 (FAIRY) [{tunicItem.instanceId}]";
            }
            else if(tunicItem.itemType == "OTHER")
            {
                locationName = $"Empty chest (OTHER) [{tunicItem.instanceId}]";
            }
            else
            {
                locationName = $"{tunicItem.itemName} x {tunicItem.itemQuantity} ({tunicItem.itemType}) [{tunicItem.instanceId}]";
            }

            long locationId = s_currentSession.Locations.GetLocationIdFromName(TUNIC_GAME_ID, locationName);
            if(locationId < 0)
            {
                Plugin.Logger.LogError($"Archipelago Error: Could not find Location with name {locationName}");
                return;
            }

            Plugin.Logger.LogInfo($"Archipelago Info: Completing check {locationId} with location name {locationName}");
            s_currentSession.Locations.CompleteLocationChecks(locationId);
        }

        private static string GenerateLocation(string location)
        {
            int startIDIndex = location.IndexOf('[');
            int endIDIndex = location.IndexOf(']');
            return location.Substring(startIDIndex + 1, endIDIndex - startIDIndex - 1);
        }

        private static TunicArchipelagoItem GenerateArchipelagoItem(string item)
        {
            int startTypeIndex = item.IndexOf('(');
            int endTypeIndex = item.IndexOf(')');
            TunicArchipelagoItemType itemType = TunicArchipelagoItemType.Nothing;
            if (startTypeIndex != -1 && endTypeIndex != -1)
            {
                string type = item.Substring(startTypeIndex + 1, endTypeIndex - startTypeIndex - 1);
                if (type == "MONEY")
                {
                    itemType = TunicArchipelagoItemType.Money;
                }
                else if (type == "FAIRY")
                {
                    itemType = TunicArchipelagoItemType.Fairy;
                }
                else if (type == "TRINKETS")
                {
                    itemType = TunicArchipelagoItemType.Trinket;
                }
                else if (type != "OTHER")
                {
                    itemType = TunicArchipelagoItemType.Item;
                }
            }

            string itemName = "Empty";
            if (itemType == TunicArchipelagoItemType.Money)
            {
                itemName = "Money";
            }
            else if (itemType != TunicArchipelagoItemType.Nothing)
            {
                itemName = item[..item.IndexOf(" x ")];
            }


            int quantity = 0;
            if (itemType == TunicArchipelagoItemType.Money)
            {
                quantity = int.Parse(item[..item.IndexOf('$')]);
            }
            else if (itemType != TunicArchipelagoItemType.Nothing)
            {
                int startQuantityIndex = item.IndexOf(" x ") + 3;
                int endQuantityIndex = item.IndexOf("(");
                quantity = int.Parse(item.Substring(startQuantityIndex, endQuantityIndex - startQuantityIndex - 1));
            }

            int startIDIndex = item.IndexOf('[');
            int endIDIndex = item.IndexOf(']');
            int archipelagoItemId = int.Parse(item.Substring(startIDIndex + 1, endIDIndex - startIDIndex - 1));

            return new TunicArchipelagoItem()
            {
                archipelagoItemId = archipelagoItemId,
                itemName = itemName,
                quantity = quantity,
                itemType = itemType
            };
        }

        public enum TunicArchipelagoItemType
        {
            Item,
            Money,
            Fairy,
            Trinket,
            Nothing
        }

        public class TunicArchipelagoItem
        {
            public int archipelagoItemId;
            public string itemName;
            public int quantity;
            public TunicArchipelagoItemType itemType;
        }
    }
}
