﻿using System;
using System.Collections.Generic;
using System.Text;
using TinyJson;
using TunicRandomizer.Stores;

namespace TunicRandomizer.Randomizer
{
    public class ChestItemRandomizer
    {

        public static string s_chestDB = "[{\"chestId\":17,\"chestName\":\"Chest: Piggybank L1 x 1\",\"chestOpeningPosition\":{\"x\":-83.95175,\"y\":12,\"z\":-175.759},\"moneyQuantity\":0,\"itemName\":\"Piggybank L1\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Town Basement_beach\"},{\"chestId\":285,\"chestName\":\"Chest: Firebomb x 2\",\"chestOpeningPosition\":{\"x\":-55.9621,\"y\":24,\"z\":-97.171},\"moneyQuantity\":0,\"itemName\":\"Firecracker\",\"denominationString\":\"\",\"itemQuantity\":4,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Changing Room_\"},{\"chestId\":255,\"chestName\":\"Chest: $48\",\"chestOpeningPosition\":{\"x\":77.42447,\"y\":2.518,\"z\":-173.7491},\"moneyQuantity\":48,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Overworld Cave_\"},{\"chestId\":266,\"chestName\":\"Chest: Flask Shard x 1\",\"chestOpeningPosition\":{\"x\":46,\"y\":54.10489,\"z\":-11.99999},\"moneyQuantity\":0,\"itemName\":\"Flask Shard\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"PatrolCave_\"},{\"chestId\":273,\"chestName\":\"Chest: $16\",\"chestOpeningPosition\":{\"x\":-32.87853,\"y\":0.25,\"z\":-171.7152},\"moneyQuantity\":16,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Maze Room_\"},{\"chestId\":13,\"chestName\":\"Chest: $20\",\"chestOpeningPosition\":{\"x\":-75,\"y\":40,\"z\":-3.999996},\"moneyQuantity\":20,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Waterfall_\"},{\"chestId\":208,\"chestName\":\"Chest: Ivy x 3\",\"chestOpeningPosition\":{\"x\":-60,\"y\":9,\"z\":-121},\"moneyQuantity\":0,\"itemName\":\"Ivy\",\"denominationString\":\"\",\"itemQuantity\":3,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Changing Room_\"},{\"chestId\":15,\"chestName\":\"Chest: $25\",\"chestOpeningPosition\":{\"x\":96.25921,\"y\":28,\"z\":-138.9855},\"moneyQuantity\":25,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"EastFiligreeCache_\"},{\"chestId\":209,\"chestName\":\"Chest: $20\",\"chestOpeningPosition\":{\"x\":-130.9002,\"y\":20,\"z\":-110.8761},\"moneyQuantity\":20,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruined Shop_\"},{\"chestId\":258,\"chestName\":\"Chest: $32\",\"chestOpeningPosition\":{\"x\":-160.9988,\"y\":3,\"z\":-113.4303},\"moneyQuantity\":32,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Archipelagos Redux_lowest\"},{\"chestId\":1,\"chestName\":\"Chest: $15\",\"chestOpeningPosition\":{\"x\":-68.00001,\"y\":40,\"z\":-31.5},\"moneyQuantity\":15,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Windmill_\"},{\"chestId\":90,\"chestName\":\"Chest: Piggybank L1 x 1\",\"chestOpeningPosition\":{\"x\":26,\"y\":28,\"z\":-118},\"moneyQuantity\":0,\"itemName\":\"Piggybank L1\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruins Passage_west\"},{\"chestId\":11,\"chestName\":\"Chest: Firecracker x 2\",\"chestOpeningPosition\":{\"x\":-49,\"y\":28,\"z\":-87.49999},\"moneyQuantity\":0,\"itemName\":\"Firecracker\",\"denominationString\":\"\",\"itemQuantity\":2,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Changing Room_\"},{\"chestId\":2,\"chestName\":\"Chest: Trinket - Heartdrops x 1\",\"chestOpeningPosition\":{\"x\":-119.7269,\"y\":16,\"z\":-112.9593},\"moneyQuantity\":0,\"itemName\":\"Trinket - Heartdrops\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"TRINKETS\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruined Shop_\"},{\"chestId\":7,\"chestName\":\"Chest: Upgrade Offering - PotionEfficiency Swig - Ash x 1\",\"chestOpeningPosition\":{\"x\":-82.51556,\"y\":4,\"z\":-175.1911},\"moneyQuantity\":0,\"itemName\":\"Upgrade Offering - PotionEfficiency Swig - Ash\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"OFFERINGS\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Town Basement_beach\"},{\"chestId\":6,\"chestName\":\"Chest: $30\",\"chestOpeningPosition\":{\"x\":66.95113,\"y\":66,\"z\":23.9994},\"moneyQuantity\":0,\"itemName\":\"Berry_MP\",\"denominationString\":\"\",\"itemQuantity\":3,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"PatrolCave_\"},{\"chestId\":16,\"chestName\":\"Chest: Upgrade Offering - Health HP - Flower x 1\",\"chestOpeningPosition\":{\"x\":-113.6761,\"y\":66,\"z\":38.19462},\"moneyQuantity\":0,\"itemName\":\"Upgrade Offering - Health HP - Flower\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"OFFERINGS\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Crypt Redux_\"},{\"chestId\":91,\"chestName\":\"Chest: $25\",\"chestOpeningPosition\":{\"x\":-118,\"y\":28,\"z\":-48.5},\"moneyQuantity\":25,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Furnace_gyro_upper_east\"},{\"chestId\":207,\"chestName\":\"Chest: Upgrade Offering - Attack - Tooth x 1\",\"chestOpeningPosition\":{\"x\":-19.80129,\"y\":43,\"z\":19.79114},\"moneyQuantity\":0,\"itemName\":\"Upgrade Offering - Attack - Tooth\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"OFFERINGS\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Temple_rafters\"},{\"chestId\":4,\"chestName\":\"Chest: $25\",\"chestOpeningPosition\":{\"x\":-130,\"y\":12,\"z\":-117},\"moneyQuantity\":25,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruined Shop_\"},{\"chestId\":3,\"chestName\":\"Chest: Berry_MP x 1\",\"chestOpeningPosition\":{\"x\":-101.8312,\"y\":40,\"z\":-42.01446},\"moneyQuantity\":0,\"itemName\":\"Berry_MP\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Furnace_gyro_upper_east\"},{\"chestId\":245,\"chestName\":\"Chest: Trinket - MP Flasks x 1\",\"chestOpeningPosition\":{\"x\":-140.2197,\"y\":28,\"z\":10.074},\"moneyQuantity\":0,\"itemName\":\"Trinket - MP Flasks\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"TRINKETS\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Sewer_west_aqueduct\"},{\"chestId\":8,\"chestName\":\"Chest: Flask Shard x 1\",\"chestOpeningPosition\":{\"x\":-19.03083,\"y\":28,\"z\":-91},\"moneyQuantity\":0,\"itemName\":\"Flask Shard\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"CubeRoom_\"},{\"chestId\":9,\"chestName\":\"Chest: Pepper x 2\",\"chestOpeningPosition\":{\"x\":-140.034,\"y\":40,\"z\":28.6326},\"moneyQuantity\":0,\"itemName\":\"Pepper\",\"denominationString\":\"\",\"itemQuantity\":2,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Sewer_west_aqueduct\"},{\"chestId\":12,\"chestName\":\"Chest: Pepper x 2\",\"chestOpeningPosition\":{\"x\":23,\"y\":36,\"z\":-110},\"moneyQuantity\":0,\"itemName\":\"Pepper\",\"denominationString\":\"\",\"itemQuantity\":2,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruins Passage_west\"},{\"chestId\":14,\"chestName\":\"Chest: $15\",\"chestOpeningPosition\":{\"x\":-138,\"y\":12,\"z\":-62},\"moneyQuantity\":15,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Furnace_gyro_lower\"},{\"chestId\":267,\"chestName\":\"Chest: Trinket Coin x 1\",\"chestOpeningPosition\":{\"x\":-178.5,\"y\":1.000013,\"z\":-80.5},\"moneyQuantity\":0,\"itemName\":\"Trinket Coin\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Archipelagos Redux_lower\"},{\"chestId\":18,\"chestName\":\"Chest: Bait x 1\",\"chestOpeningPosition\":{\"x\":-161,\"y\":1.000013,\"z\":-74},\"moneyQuantity\":0,\"itemName\":\"Bait\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Furnace_gyro_west\"},{\"chestId\":10,\"chestName\":\"Chest: Trinket Coin x 1\",\"chestOpeningPosition\":{\"x\":13.5,\"y\":1,\"z\":-147.5},\"moneyQuantity\":0,\"itemName\":\"Trinket Coin\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Maze Room_\"},{\"chestId\":5,\"chestName\":\"Chest: (empty)\",\"chestOpeningPosition\":{\"x\":63,\"y\":22,\"z\":-138},\"moneyQuantity\":0,\"itemQuantity\":0,\"itemType\":\"NONE/OTHER/MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruins Passage_east\"},{\"chestId\":1013,\"chestName\":\"Chest: GoldenTrophy_9 x 1\",\"chestOpeningPosition\":{\"x\":-6.19172,\"y\":11.929,\"z\":-206.4},\"moneyQuantity\":0,\"itemName\":\"GoldenTrophy_9\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Transit_teleporter_starting island\"},{\"chestId\":1008,\"chestName\":\"Chest: GoldenTrophy_8 x 1\",\"chestOpeningPosition\":{\"x\":-37.2089,\"y\":28.029,\"z\":-65.51782},\"moneyQuantity\":0,\"itemName\":\"GoldenTrophy_8\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Overworld Interiors_house\"},{\"chestId\":1004,\"chestName\":\"Chest: GoldenTrophy_4 x 1\",\"chestOpeningPosition\":{\"x\":-34,\"y\":-0.471,\"z\":-180.3917},\"moneyQuantity\":0,\"itemName\":\"GoldenTrophy_4\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Transit_teleporter_starting island\"},{\"chestId\":1003,\"chestName\":\"Chest: GoldenTrophy_3 x 1\",\"chestOpeningPosition\":{\"x\":-62,\"y\":40.029,\"z\":-42.39172},\"moneyQuantity\":0,\"itemName\":\"GoldenTrophy_3\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Windmill_\"},{\"chestId\":17,\"chestName\":\"Chest: Piggybank L1 x 1\",\"chestOpeningPosition\":{\"x\":-83.95175,\"y\":12,\"z\":-175.759},\"moneyQuantity\":0,\"itemName\":\"Piggybank L1\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Town Basement_beach\"},{\"chestId\":285,\"chestName\":\"Chest: Firebomb x 2\",\"chestOpeningPosition\":{\"x\":-55.9621,\"y\":24,\"z\":-97.171},\"moneyQuantity\":0,\"itemName\":\"Firecracker\",\"denominationString\":\"\",\"itemQuantity\":4,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Changing Room_\"},{\"chestId\":255,\"chestName\":\"Chest: $48\",\"chestOpeningPosition\":{\"x\":77.42447,\"y\":2.518,\"z\":-173.7491},\"moneyQuantity\":48,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Overworld Cave_\"},{\"chestId\":266,\"chestName\":\"Chest: Flask Shard x 1\",\"chestOpeningPosition\":{\"x\":46,\"y\":54.10489,\"z\":-11.99999},\"moneyQuantity\":0,\"itemName\":\"Flask Shard\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"PatrolCave_\"},{\"chestId\":273,\"chestName\":\"Chest: $16\",\"chestOpeningPosition\":{\"x\":-32.87853,\"y\":0.25,\"z\":-171.7152},\"moneyQuantity\":16,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Maze Room_\"},{\"chestId\":13,\"chestName\":\"Chest: $20\",\"chestOpeningPosition\":{\"x\":-75,\"y\":40,\"z\":-3.999996},\"moneyQuantity\":20,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Waterfall_\"},{\"chestId\":208,\"chestName\":\"Chest: Ivy x 3\",\"chestOpeningPosition\":{\"x\":-60,\"y\":9,\"z\":-121},\"moneyQuantity\":0,\"itemName\":\"Ivy\",\"denominationString\":\"\",\"itemQuantity\":3,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Changing Room_\"},{\"chestId\":15,\"chestName\":\"Chest: $25\",\"chestOpeningPosition\":{\"x\":96.25921,\"y\":28,\"z\":-138.9855},\"moneyQuantity\":25,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"EastFiligreeCache_\"},{\"chestId\":209,\"chestName\":\"Chest: $20\",\"chestOpeningPosition\":{\"x\":-130.9002,\"y\":20,\"z\":-110.8761},\"moneyQuantity\":20,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruined Shop_\"},{\"chestId\":258,\"chestName\":\"Chest: $32\",\"chestOpeningPosition\":{\"x\":-160.9988,\"y\":3,\"z\":-113.4303},\"moneyQuantity\":32,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Archipelagos Redux_lowest\"},{\"chestId\":1,\"chestName\":\"Chest: $15\",\"chestOpeningPosition\":{\"x\":-68.00001,\"y\":40,\"z\":-31.5},\"moneyQuantity\":15,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Windmill_\"},{\"chestId\":90,\"chestName\":\"Chest: Piggybank L1 x 1\",\"chestOpeningPosition\":{\"x\":26,\"y\":28,\"z\":-118},\"moneyQuantity\":0,\"itemName\":\"Piggybank L1\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruins Passage_west\"},{\"chestId\":11,\"chestName\":\"Chest: Firecracker x 2\",\"chestOpeningPosition\":{\"x\":-49,\"y\":28,\"z\":-87.49999},\"moneyQuantity\":0,\"itemName\":\"Firecracker\",\"denominationString\":\"\",\"itemQuantity\":2,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Changing Room_\"},{\"chestId\":2,\"chestName\":\"Chest: Trinket - Heartdrops x 1\",\"chestOpeningPosition\":{\"x\":-119.7269,\"y\":16,\"z\":-112.9593},\"moneyQuantity\":0,\"itemName\":\"Trinket - Heartdrops\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"TRINKETS\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruined Shop_\"},{\"chestId\":7,\"chestName\":\"Chest: Upgrade Offering - PotionEfficiency Swig - Ash x 1\",\"chestOpeningPosition\":{\"x\":-82.51556,\"y\":4,\"z\":-175.1911},\"moneyQuantity\":0,\"itemName\":\"Upgrade Offering - PotionEfficiency Swig - Ash\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"OFFERINGS\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Town Basement_beach\"},{\"chestId\":6,\"chestName\":\"Chest: $30\",\"chestOpeningPosition\":{\"x\":66.95113,\"y\":66,\"z\":23.9994},\"moneyQuantity\":0,\"itemName\":\"Berry_MP\",\"denominationString\":\"\",\"itemQuantity\":3,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"PatrolCave_\"},{\"chestId\":16,\"chestName\":\"Chest: Upgrade Offering - Health HP - Flower x 1\",\"chestOpeningPosition\":{\"x\":-113.6761,\"y\":66,\"z\":38.19462},\"moneyQuantity\":0,\"itemName\":\"Upgrade Offering - Health HP - Flower\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"OFFERINGS\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Crypt Redux_\"},{\"chestId\":91,\"chestName\":\"Chest: $25\",\"chestOpeningPosition\":{\"x\":-118,\"y\":28,\"z\":-48.5},\"moneyQuantity\":25,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Furnace_gyro_upper_east\"},{\"chestId\":207,\"chestName\":\"Chest: Upgrade Offering - Attack - Tooth x 1\",\"chestOpeningPosition\":{\"x\":-19.80129,\"y\":43,\"z\":19.79114},\"moneyQuantity\":0,\"itemName\":\"Upgrade Offering - Attack - Tooth\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"OFFERINGS\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Temple_rafters\"},{\"chestId\":4,\"chestName\":\"Chest: $25\",\"chestOpeningPosition\":{\"x\":-130,\"y\":12,\"z\":-117},\"moneyQuantity\":25,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruined Shop_\"},{\"chestId\":3,\"chestName\":\"Chest: Berry_MP x 1\",\"chestOpeningPosition\":{\"x\":-101.8312,\"y\":40,\"z\":-42.01446},\"moneyQuantity\":0,\"itemName\":\"Berry_MP\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Furnace_gyro_upper_east\"},{\"chestId\":245,\"chestName\":\"Chest: Trinket - MP Flasks x 1\",\"chestOpeningPosition\":{\"x\":-140.2197,\"y\":28,\"z\":10.074},\"moneyQuantity\":0,\"itemName\":\"Trinket - MP Flasks\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"TRINKETS\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Sewer_west_aqueduct\"},{\"chestId\":8,\"chestName\":\"Chest: Flask Shard x 1\",\"chestOpeningPosition\":{\"x\":-19.03083,\"y\":28,\"z\":-91},\"moneyQuantity\":0,\"itemName\":\"Flask Shard\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"CubeRoom_\"},{\"chestId\":9,\"chestName\":\"Chest: Pepper x 2\",\"chestOpeningPosition\":{\"x\":-140.034,\"y\":40,\"z\":28.6326},\"moneyQuantity\":0,\"itemName\":\"Pepper\",\"denominationString\":\"\",\"itemQuantity\":2,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Sewer_west_aqueduct\"},{\"chestId\":12,\"chestName\":\"Chest: Pepper x 2\",\"chestOpeningPosition\":{\"x\":23,\"y\":36,\"z\":-110},\"moneyQuantity\":0,\"itemName\":\"Pepper\",\"denominationString\":\"\",\"itemQuantity\":2,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruins Passage_west\"},{\"chestId\":14,\"chestName\":\"Chest: $15\",\"chestOpeningPosition\":{\"x\":-138,\"y\":12,\"z\":-62},\"moneyQuantity\":15,\"itemQuantity\":0,\"itemType\":\"MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Furnace_gyro_lower\"},{\"chestId\":267,\"chestName\":\"Chest: Trinket Coin x 1\",\"chestOpeningPosition\":{\"x\":-178.5,\"y\":1.000013,\"z\":-80.5},\"moneyQuantity\":0,\"itemName\":\"Trinket Coin\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Archipelagos Redux_lower\"},{\"chestId\":18,\"chestName\":\"Chest: Bait x 1\",\"chestOpeningPosition\":{\"x\":-161,\"y\":1.000013,\"z\":-74},\"moneyQuantity\":0,\"itemName\":\"Bait\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Furnace_gyro_west\"},{\"chestId\":10,\"chestName\":\"Chest: Trinket Coin x 1\",\"chestOpeningPosition\":{\"x\":13.5,\"y\":1,\"z\":-147.5},\"moneyQuantity\":0,\"itemName\":\"Trinket Coin\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"SUPPLIES\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Maze Room_\"},{\"chestId\":5,\"chestName\":\"Chest: (empty)\",\"chestOpeningPosition\":{\"x\":63,\"y\":22,\"z\":-138},\"moneyQuantity\":0,\"itemQuantity\":0,\"itemType\":\"NONE/OTHER/MONEY\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Ruins Passage_east\"},{\"chestId\":1013,\"chestName\":\"Chest: GoldenTrophy_9 x 1\",\"chestOpeningPosition\":{\"x\":-6.19172,\"y\":11.929,\"z\":-206.4},\"moneyQuantity\":0,\"itemName\":\"GoldenTrophy_9\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Transit_teleporter_starting island\"},{\"chestId\":1008,\"chestName\":\"Chest: GoldenTrophy_8 x 1\",\"chestOpeningPosition\":{\"x\":-37.2089,\"y\":28.029,\"z\":-65.51782},\"moneyQuantity\":0,\"itemName\":\"GoldenTrophy_8\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Overworld Interiors_house\"},{\"chestId\":1004,\"chestName\":\"Chest: GoldenTrophy_4 x 1\",\"chestOpeningPosition\":{\"x\":-34,\"y\":-0.471,\"z\":-180.3917},\"moneyQuantity\":0,\"itemName\":\"GoldenTrophy_4\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Transit_teleporter_starting island\"},{\"chestId\":1003,\"chestName\":\"Chest: GoldenTrophy_3 x 1\",\"chestOpeningPosition\":{\"x\":-62,\"y\":40.029,\"z\":-42.39172},\"moneyQuantity\":0,\"itemName\":\"GoldenTrophy_3\",\"denominationString\":\"\",\"itemQuantity\":1,\"itemType\":\"GEAR\",\"sceneName\":\"Overworld Redux\",\"sceneId\":24,\"itemNearestExit\":\"Windmill_\"}]";

        public int Seed { get => randomizer.Seed; set => randomizer.Seed = value; }
        public bool IsRandomized { get => randomizer.Seed != 0; }

        private SeedRandomizer randomizer;
        private bool m_isRandomized = false;
        private int[] m_chestIndexMatching;
        private ChestItemStore[] chests;

        public ChestItemRandomizer() : this(Environment.TickCount)
        {
        }

        public ChestItemRandomizer(int seed)
        {
            randomizer = new SeedRandomizer(seed);
        }

        public void Randomize()
        {
            m_isRandomized = true;
            chests = s_chestDB.FromJson<ChestItemStore[]>();
            m_chestIndexMatching = randomizer.RandomizeList(chests.Length);
        }

        public ChestItemStore GetRandomizedItem(Chest chest)
        {
            ChestItemStore originalChestItem = ChestItemStore.ChestToChestItemStore(chest);
            if (!m_isRandomized)
            {
                Plugin.Logger.LogWarning($"Randomization not started: Chest {chest.name} ({chest.chestID}) is not randomized");
                return originalChestItem;
            }
            if(chest.chestID <= 0)
            {
                Plugin.Logger.LogWarning("THIS CHEST HAS ID <= 0 AND WAS THEREFORE (FOR NOW) NOT ADDED TO THE RANDOMIZER POOL");
                return originalChestItem;
            }

            //Get index of chestId in list and swap it out using m_chestIndexMatching
            for(int i = 0; i < chests.Length; i++)
            {
                if (chests[i].chestId == chest.chestID)
                {
                    ChestItemStore newChestItem = chests[m_chestIndexMatching[i]];
                    Plugin.Logger.LogInfo($"Original Item: {originalChestItem.itemName} ({originalChestItem.itemType})");
                    Plugin.Logger.LogInfo($"Randomized New Item: {newChestItem.itemName} ({newChestItem.itemType})");
                    return newChestItem;
                }
            }

            Plugin.Logger.LogError($"Couldn't find chest {chest.name} ({chest.chestID}) in the randomizer pool");
            return originalChestItem;
        }
    }
}
