using System;
using System.Runtime.CompilerServices;

namespace BetterErProspecting.Config
{
	// Token: 0x02000015 RID: 21
	[NullableContext(1)]
	[Nullable(0)]
	public class ModConfig
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00005F15 File Offset: 0x00004115
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00005F1C File Offset: 0x0000411C
		public static ModConfig Instance { get; set; } = new ModConfig();

		// Token: 0x0400002D RID: 45
		public static string ConfigName = "BetterErProspecting.json";

		// Token: 0x0400002F RID: 47
		public static string[] SettingsForceLoad = new string[]
		{
			"EnableDensityMode",
			"NewDensityMode",
			"AddBoreHoleMode",
			"AddStoneMode",
			"AddProximityMode"
		};

		// Token: 0x04000030 RID: 48
		public static string[] SettingsPatch = new string[]
		{
			"NewDensityMode",
			"StoneSearchCreatesReadings"
		};

		// Token: 0x04000031 RID: 49
		public bool EnableDensityMode = true;

		// Token: 0x04000032 RID: 50
		public bool OneShotDensity;

		// Token: 0x04000033 RID: 51
		public bool NewDensityMode = true;

		// Token: 0x04000034 RID: 52
		public int NewDensityDmg = 3;

		// Token: 0x04000035 RID: 53
		public string DensityBlackListedOres = "flint, quartz";

		// Token: 0x04000036 RID: 54
		public string StoneSearchBlackList = "rock-suevite";

		// Token: 0x04000037 RID: 55
		public bool AddProximityMode = true;

		// Token: 0x04000038 RID: 56
		public bool ProximityVagueDescriptors;

		// Token: 0x04000039 RID: 57
		public int ProximitySearchRadius = 5;

		// Token: 0x0400003A RID: 58
		public int ProximityDmg = 2;

		// Token: 0x0400003B RID: 59
		public bool AddStoneMode = true;

		// Token: 0x0400003C RID: 60
		public bool StoneSearchCreatesReadings;

		// Token: 0x0400003D RID: 61
		public bool StonePercentSearch = true;

		// Token: 0x0400003E RID: 62
		public int StoneSearchRadius = 64;

		// Token: 0x0400003F RID: 63
		public int StoneDmg = 4;

		// Token: 0x04000040 RID: 64
		public bool AddBoreHoleMode = true;

		// Token: 0x04000041 RID: 65
		public int BoreholeRadius = 8;

		// Token: 0x04000042 RID: 66
		public int BoreholeDmg = 2;

		// Token: 0x04000043 RID: 67
		public bool BoreholeWpMarker;

		// Token: 0x04000044 RID: 68
		public bool BoreholeScansOre = true;

		// Token: 0x04000045 RID: 69
		public bool BoreholeScansStone;

		// Token: 0x04000046 RID: 70
		public bool DebugMode;

		// Token: 0x04000047 RID: 71
		public int ReprospectParellelism;
	}
}
