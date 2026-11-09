using System;
using System.Collections.Generic;

namespace CritchersCalculator
{
	// Token: 0x02000005 RID: 5
	public static class FuelData
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002228 File Offset: 0x00000428
		public static int GetMeltingPointC(string alloyCode)
		{
			int mp;
			if (!FuelData.alloyMeltingPoints.TryGetValue(alloyCode, ref mp))
			{
				return 0;
			}
			return mp;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002248 File Offset: 0x00000448
		public static List<FuelType> GetValidFuels(string alloyCode)
		{
			List<FuelType> result = new List<FuelType>();
			string profile;
			if (!FuelData.alloyProfiles.TryGetValue(alloyCode, ref profile))
			{
				return result;
			}
			Dictionary<string, int[]> costs = FuelData.fuelCosts[profile];
			for (int i = 0; i < FuelData.Fuels.Count; i++)
			{
				if (costs.ContainsKey(FuelData.Fuels[i].Code))
				{
					result.Add(FuelData.Fuels[i]);
				}
			}
			return result;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022B8 File Offset: 0x000004B8
		public static int GetFuelCost(string alloyCode, int ingots, string fuelCode)
		{
			if (ingots < 1 || ingots > 24)
			{
				return -1;
			}
			string profile;
			if (!FuelData.alloyProfiles.TryGetValue(alloyCode, ref profile))
			{
				return -1;
			}
			int[] costs;
			if (!FuelData.fuelCosts[profile].TryGetValue(fuelCode, ref costs))
			{
				return -1;
			}
			return costs[ingots - 1];
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002300 File Offset: 0x00000500
		// Note: this type is marked as 'beforefieldinit'.
		static FuelData()
		{
			List<FuelType> list = new List<FuelType>();
			list.Add(new FuelType
			{
				Code = "firewood",
				LangKey = "critcherscalculator:fuel-firewood",
				BurnTemperatureC = 700,
				BurnDurationSec = 24f
			});
			list.Add(new FuelType
			{
				Code = "peat",
				LangKey = "critcherscalculator:fuel-peat",
				BurnTemperatureC = 900,
				BurnDurationSec = 50f
			});
			list.Add(new FuelType
			{
				Code = "peatbrick",
				LangKey = "critcherscalculator:fuel-peatbrick",
				BurnTemperatureC = 900,
				BurnDurationSec = 25f
			});
			list.Add(new FuelType
			{
				Code = "ore-lignite",
				LangKey = "critcherscalculator:fuel-lignite",
				BurnTemperatureC = 1100,
				BurnDurationSec = 77f
			});
			list.Add(new FuelType
			{
				Code = "ore-bituminouscoal",
				LangKey = "critcherscalculator:fuel-bituminouscoal",
				BurnTemperatureC = 1200,
				BurnDurationSec = 84f
			});
			list.Add(new FuelType
			{
				Code = "ore-anthracite",
				LangKey = "critcherscalculator:fuel-anthracite",
				BurnTemperatureC = 1200,
				BurnDurationSec = 196f
			});
			list.Add(new FuelType
			{
				Code = "charcoal",
				LangKey = "critcherscalculator:fuel-charcoal",
				BurnTemperatureC = 1300,
				BurnDurationSec = 40f
			});
			list.Add(new FuelType
			{
				Code = "coke",
				LangKey = "critcherscalculator:fuel-coke",
				BurnTemperatureC = 1340,
				BurnDurationSec = 40f
			});
			FuelData.Fuels = list;
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary["tinBronze"] = 1084;
			dictionary["bismuthBronze"] = 1084;
			dictionary["blackBronze"] = 1084;
			dictionary["brass"] = 1084;
			dictionary["molybdochalkos"] = 1084;
			dictionary["electrum"] = 1063;
			dictionary["silverSolder"] = 961;
			dictionary["leadSolder"] = 327;
			dictionary["cupronickel"] = 1325;
			FuelData.alloyMeltingPoints = dictionary;
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2["tinBronze"] = "mp1084";
			dictionary2["bismuthBronze"] = "mp1084";
			dictionary2["blackBronze"] = "mp1084";
			dictionary2["brass"] = "mp1084";
			dictionary2["molybdochalkos"] = "mp1084";
			dictionary2["electrum"] = "mp1063";
			dictionary2["silverSolder"] = "mp961";
			dictionary2["leadSolder"] = "mp327";
			dictionary2["cupronickel"] = "mp1325";
			FuelData.alloyProfiles = dictionary2;
			Dictionary<string, Dictionary<string, int[]>> dictionary3 = new Dictionary<string, Dictionary<string, int[]>>();
			Dictionary<string, Dictionary<string, int[]>> dictionary4 = dictionary3;
			string text = "mp1084";
			Dictionary<string, int[]> dictionary5 = new Dictionary<string, int[]>();
			dictionary5["ore-lignite"] = new int[]
			{
				2,
				3,
				3,
				3,
				4,
				4,
				4,
				5,
				5,
				6,
				6,
				6,
				7,
				7,
				8,
				8,
				8,
				9,
				9,
				10,
				10,
				10,
				11,
				11
			};
			dictionary5["ore-bituminouscoal"] = new int[]
			{
				2,
				2,
				2,
				3,
				3,
				3,
				4,
				4,
				5,
				5,
				5,
				6,
				6,
				6,
				7,
				7,
				7,
				8,
				8,
				8,
				9,
				9,
				10,
				10
			};
			dictionary5["ore-anthracite"] = new int[]
			{
				1,
				1,
				1,
				1,
				2,
				2,
				2,
				2,
				2,
				2,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				4,
				4,
				4,
				4,
				4,
				4,
				5
			};
			dictionary5["charcoal"] = new int[]
			{
				3,
				3,
				4,
				5,
				6,
				6,
				7,
				8,
				9,
				9,
				10,
				11,
				12,
				12,
				13,
				14,
				15,
				15,
				16,
				17,
				18,
				18,
				19,
				20
			};
			dictionary5["coke"] = new int[]
			{
				3,
				3,
				4,
				5,
				6,
				6,
				7,
				8,
				9,
				9,
				10,
				11,
				12,
				12,
				13,
				14,
				15,
				15,
				16,
				17,
				18,
				18,
				19,
				20
			};
			dictionary4[text] = dictionary5;
			Dictionary<string, Dictionary<string, int[]>> dictionary6 = dictionary3;
			string text2 = "mp1063";
			dictionary5 = new Dictionary<string, int[]>();
			dictionary5["ore-lignite"] = new int[]
			{
				2,
				2,
				3,
				3,
				4,
				4,
				4,
				5,
				5,
				6,
				6,
				6,
				7,
				7,
				7,
				8,
				8,
				9,
				9,
				9,
				10,
				10,
				11,
				11
			};
			dictionary5["ore-bituminouscoal"] = new int[]
			{
				2,
				2,
				2,
				3,
				3,
				3,
				4,
				4,
				4,
				5,
				5,
				6,
				6,
				6,
				7,
				7,
				7,
				8,
				8,
				8,
				9,
				9,
				9,
				10
			};
			dictionary5["ore-anthracite"] = new int[]
			{
				1,
				1,
				1,
				1,
				2,
				2,
				2,
				2,
				2,
				2,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				4,
				4,
				4,
				4,
				4,
				4,
				5
			};
			dictionary5["charcoal"] = new int[]
			{
				3,
				3,
				4,
				5,
				6,
				6,
				7,
				8,
				9,
				9,
				10,
				11,
				12,
				12,
				13,
				14,
				15,
				15,
				16,
				17,
				18,
				18,
				19,
				20
			};
			dictionary5["coke"] = new int[]
			{
				3,
				3,
				4,
				5,
				6,
				6,
				7,
				8,
				9,
				9,
				10,
				11,
				12,
				12,
				13,
				14,
				15,
				15,
				16,
				17,
				18,
				18,
				19,
				20
			};
			dictionary6[text2] = dictionary5;
			Dictionary<string, Dictionary<string, int[]>> dictionary7 = dictionary3;
			string text3 = "mp961";
			dictionary5 = new Dictionary<string, int[]>();
			dictionary5["ore-lignite"] = new int[]
			{
				2,
				2,
				2,
				3,
				3,
				4,
				4,
				4,
				5,
				5,
				6,
				6,
				6,
				7,
				7,
				8,
				8,
				8,
				9,
				9,
				9,
				10,
				10,
				11
			};
			dictionary5["ore-bituminouscoal"] = new int[]
			{
				1,
				2,
				2,
				3,
				3,
				3,
				4,
				4,
				4,
				5,
				5,
				5,
				6,
				6,
				6,
				7,
				7,
				8,
				8,
				8,
				9,
				9,
				9,
				10
			};
			dictionary5["ore-anthracite"] = new int[]
			{
				1,
				1,
				1,
				1,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				3,
				3,
				3,
				3,
				3,
				3,
				4,
				4,
				4,
				4,
				4,
				4,
				4
			};
			dictionary5["charcoal"] = new int[]
			{
				2,
				3,
				4,
				5,
				5,
				6,
				7,
				8,
				8,
				9,
				10,
				11,
				11,
				12,
				13,
				14,
				14,
				15,
				16,
				17,
				17,
				18,
				19,
				20
			};
			dictionary5["coke"] = new int[]
			{
				2,
				3,
				4,
				5,
				5,
				6,
				7,
				8,
				8,
				9,
				10,
				11,
				11,
				12,
				13,
				14,
				14,
				15,
				16,
				17,
				17,
				18,
				19,
				20
			};
			dictionary7[text3] = dictionary5;
			Dictionary<string, Dictionary<string, int[]>> dictionary8 = dictionary3;
			string text4 = "mp327";
			dictionary5 = new Dictionary<string, int[]>();
			dictionary5["firewood"] = new int[]
			{
				3,
				4,
				5,
				7,
				8,
				9,
				10,
				11,
				12,
				12,
				13,
				14,
				14,
				15,
				15,
				16,
				17,
				17,
				18,
				19,
				19,
				20,
				20,
				21
			};
			dictionary5["peat"] = new int[]
			{
				2,
				2,
				3,
				3,
				4,
				4,
				4,
				4,
				5,
				5,
				5,
				6,
				6,
				6,
				7,
				7,
				7,
				7,
				8,
				8,
				8,
				9,
				9,
				9
			};
			dictionary5["peatbrick"] = new int[]
			{
				3,
				4,
				5,
				6,
				7,
				7,
				8,
				8,
				9,
				10,
				10,
				11,
				11,
				12,
				13,
				13,
				14,
				14,
				15,
				16,
				16,
				17,
				17,
				18
			};
			dictionary5["ore-lignite"] = new int[]
			{
				1,
				2,
				2,
				2,
				2,
				2,
				3,
				3,
				3,
				3,
				3,
				4,
				4,
				4,
				4,
				4,
				4,
				4,
				4,
				5,
				5,
				5,
				5,
				5
			};
			dictionary5["ore-bituminouscoal"] = new int[]
			{
				1,
				1,
				2,
				2,
				2,
				2,
				2,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				3,
				4,
				4,
				4,
				4,
				4,
				4,
				4,
				4,
				5
			};
			dictionary5["ore-anthracite"] = new int[]
			{
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				1,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2,
				2
			};
			dictionary5["charcoal"] = new int[]
			{
				2,
				2,
				3,
				3,
				4,
				4,
				4,
				5,
				5,
				5,
				5,
				6,
				6,
				6,
				6,
				7,
				7,
				7,
				7,
				8,
				8,
				8,
				8,
				9
			};
			dictionary5["coke"] = new int[]
			{
				2,
				2,
				3,
				3,
				4,
				4,
				4,
				5,
				5,
				5,
				5,
				6,
				6,
				6,
				6,
				7,
				7,
				7,
				7,
				8,
				8,
				8,
				8,
				9
			};
			dictionary8[text4] = dictionary5;
			Dictionary<string, Dictionary<string, int[]>> dictionary9 = dictionary3;
			string text5 = "mp1325";
			dictionary5 = new Dictionary<string, int[]>();
			dictionary5["coke"] = new int[]
			{
				4,
				5,
				6,
				6,
				7,
				8,
				9,
				10,
				11,
				11,
				12,
				13,
				14,
				15,
				16,
				16,
				17,
				18,
				19,
				20,
				21,
				22,
				22,
				23
			};
			dictionary9[text5] = dictionary5;
			FuelData.fuelCosts = dictionary3;
		}

		// Token: 0x04000008 RID: 8
		public const int MAX_INGOTS = 24;

		// Token: 0x04000009 RID: 9
		public static readonly List<FuelType> Fuels;

		// Token: 0x0400000A RID: 10
		private static readonly Dictionary<string, int> alloyMeltingPoints;

		// Token: 0x0400000B RID: 11
		private static readonly Dictionary<string, string> alloyProfiles;

		// Token: 0x0400000C RID: 12
		private static readonly Dictionary<string, Dictionary<string, int[]>> fuelCosts;
	}
}
