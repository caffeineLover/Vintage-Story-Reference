using System;
using System.Runtime.CompilerServices;

namespace TentBagReworked.Config
{
	// Token: 0x02000009 RID: 9
	[NullableContext(1)]
	[Nullable(0)]
	public class TentBagReworkedConfig
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000051B4 File Offset: 0x000033B4
		// (set) Token: 0x0600006E RID: 110 RVA: 0x000051BC File Offset: 0x000033BC
		public int MaxRadius { get; set; } = 3;

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000051C5 File Offset: 0x000033C5
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000051CD File Offset: 0x000033CD
		public int MaxHeight { get; set; } = 7;

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000051D6 File Offset: 0x000033D6
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000051DE File Offset: 0x000033DE
		public float BuildEffort { get; set; } = 2f;

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000051E7 File Offset: 0x000033E7
		// (set) Token: 0x06000074 RID: 116 RVA: 0x000051EF File Offset: 0x000033EF
		public bool RequireSolidGround { get; set; } = false;

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000051F8 File Offset: 0x000033F8
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00005200 File Offset: 0x00003400
		public bool GrabFloor { get; set; } = false;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00005209 File Offset: 0x00003409
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00005211 File Offset: 0x00003411
		public bool CopyMode { get; set; } = false;

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000079 RID: 121 RVA: 0x0000521A File Offset: 0x0000341A
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00005222 File Offset: 0x00003422
		public bool ReplacePlantsAndRocks { get; set; } = true;

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000522B File Offset: 0x0000342B
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00005233 File Offset: 0x00003433
		public bool PutTentInInventoryOnUse { get; set; } = true;

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000523C File Offset: 0x0000343C
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00005244 File Offset: 0x00003444
		public string HighlightErrorColor { get; set; } = "#2FFF0000";

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000524D File Offset: 0x0000344D
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00005255 File Offset: 0x00003455
		public string HighlightPickupColor { get; set; } = "#5000FF00";

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000525E File Offset: 0x0000345E
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00005266 File Offset: 0x00003466
		public bool DropWaypoint { get; set; } = true;

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000526F File Offset: 0x0000346F
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00005277 File Offset: 0x00003477
		public string WaypointIcon { get; set; } = "home";

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00005280 File Offset: 0x00003480
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00005288 File Offset: 0x00003488
		public string WaypointColor { get; set; } = "dodgerblue";

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00005291 File Offset: 0x00003491
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00005299 File Offset: 0x00003499
		public bool PinWaypoint { get; set; } = true;

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000052A2 File Offset: 0x000034A2
		// (set) Token: 0x0600008A RID: 138 RVA: 0x000052AA File Offset: 0x000034AA
		public bool AllowListMode { get; set; } = false;

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000052B3 File Offset: 0x000034B3
		// (set) Token: 0x0600008C RID: 140 RVA: 0x000052BB File Offset: 0x000034BB
		public bool ShowChatNotification { get; set; } = true;

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000052C4 File Offset: 0x000034C4
		// (set) Token: 0x0600008E RID: 142 RVA: 0x000052CC File Offset: 0x000034CC
		public int MaxSchematicHistory { get; set; } = 10;

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000052D5 File Offset: 0x000034D5
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000052DD File Offset: 0x000034DD
		public string[] BannedBlocks { get; set; } = new string[]
		{
			"game:paperlantern-*",
			"game:chandelier-*",
			"game:log-grown-*",
			"game:log-resin-*",
			"game:log-resinharvested-*",
			"game:statictranslocator-*",
			"game:teleporterbase",
			"game:crop-*",
			"game:herb-*",
			"game:mushroom-*",
			"game:smallberrybush-*",
			"game:bigberrybush-*",
			"game:water-*",
			"game:lava-*",
			"game:farmland-*",
			"game:rawclay-*",
			"game:peat-*",
			"game:rock-*",
			"game:ore-*",
			"game:crock-burned-*",
			"game:bowl-meal",
			"game:claypot-cooked",
			"game:anvil-*",
			"game:forge"
		};

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000052E6 File Offset: 0x000034E6
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000052EE File Offset: 0x000034EE
		public string[] AllowedBlocks { get; set; } = new string[]
		{
			"game:soil-*"
		};
	}
}
