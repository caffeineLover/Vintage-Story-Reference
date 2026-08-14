using System;

namespace Jaunt.Config
{
	// Token: 0x0200000C RID: 12
	public class JauntConfig
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00003955 File Offset: 0x00001B55
		// (set) Token: 0x0600004C RID: 76 RVA: 0x0000395D File Offset: 0x00001B5D
		public string ConfigVersion { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003966 File Offset: 0x00001B66
		// (set) Token: 0x0600004E RID: 78 RVA: 0x0000396E File Offset: 0x00001B6E
		public float GlobalMaxStaminaMultiplier { get; set; } = 1f;

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003977 File Offset: 0x00001B77
		// (set) Token: 0x06000050 RID: 80 RVA: 0x0000397F File Offset: 0x00001B7F
		public float GlobalStaminaRegenMultiplier { get; set; } = 1f;

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003988 File Offset: 0x00001B88
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00003990 File Offset: 0x00001B90
		public float GlobalSwimStaminaCostMultiplier { get; set; } = 1f;

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003999 File Offset: 0x00001B99
		// (set) Token: 0x06000054 RID: 84 RVA: 0x000039A1 File Offset: 0x00001BA1
		public float GlobalSprintStaminaCostMultiplier { get; set; } = 1f;

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000039AA File Offset: 0x00001BAA
		// (set) Token: 0x06000056 RID: 86 RVA: 0x000039B2 File Offset: 0x00001BB2
		public bool EnableStamina { get; set; } = true;

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000039BB File Offset: 0x00001BBB
		// (set) Token: 0x06000058 RID: 88 RVA: 0x000039C3 File Offset: 0x00001BC3
		public bool HideStaminaOnFull { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000039CC File Offset: 0x00001BCC
		// (set) Token: 0x0600005A RID: 90 RVA: 0x000039D4 File Offset: 0x00001BD4
		public string StaminaBarLocation { get; set; } = "AboveHealth";

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000039DD File Offset: 0x00001BDD
		// (set) Token: 0x0600005C RID: 92 RVA: 0x000039E5 File Offset: 0x00001BE5
		public float StaminaBarWidthMultiplier { get; set; } = 1f;

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000039EE File Offset: 0x00001BEE
		// (set) Token: 0x0600005E RID: 94 RVA: 0x000039F6 File Offset: 0x00001BF6
		public float StaminaBarXOffset { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000039FF File Offset: 0x00001BFF
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00003A07 File Offset: 0x00001C07
		public float StaminaBarYOffset { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003A10 File Offset: 0x00001C10
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00003A18 File Offset: 0x00001C18
		public bool ShowGaitIcon { get; set; } = true;

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003A21 File Offset: 0x00001C21
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00003A29 File Offset: 0x00001C29
		public float IconOffsetX { get; set; } = -385f;

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00003A32 File Offset: 0x00001C32
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00003A3A File Offset: 0x00001C3A
		public float IconOffsetY { get; set; } = -105f;

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003A43 File Offset: 0x00001C43
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00003A4B File Offset: 0x00001C4B
		public float IconSize { get; set; } = 75f;

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003A54 File Offset: 0x00001C54
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003A5C File Offset: 0x00001C5C
		public float DefaultMaxStamina { get; set; } = 100f;

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003A65 File Offset: 0x00001C65
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003A6D File Offset: 0x00001C6D
		public float DefaultSprintFatigue { get; set; } = 0.2f;

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003A76 File Offset: 0x00001C76
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00003A7E File Offset: 0x00001C7E
		public float DefaultSwimFatigue { get; set; } = 0.2f;

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003A87 File Offset: 0x00001C87
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00003A8F File Offset: 0x00001C8F
		public float DefaultBaseFatigueRate { get; set; } = 1f;

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003A98 File Offset: 0x00001C98
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00003AA0 File Offset: 0x00001CA0
		public float DefaultStaminaRegenRate { get; set; } = 1f;

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003AA9 File Offset: 0x00001CA9
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00003AB1 File Offset: 0x00001CB1
		public float DefaultRegenPenaltyWounded { get; set; } = 0.5f;

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003ABA File Offset: 0x00001CBA
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00003AC2 File Offset: 0x00001CC2
		public float DefaultRegenPenaltyMounted { get; set; } = 0.3f;

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003ACB File Offset: 0x00001CCB
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003AD3 File Offset: 0x00001CD3
		public float DefaultExhaustionThreshold { get; set; } = 0.1f;

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003ADC File Offset: 0x00001CDC
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003AE4 File Offset: 0x00001CE4
		public bool DefaultDontFleeWhenExhausted { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003AED File Offset: 0x00001CED
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003AF5 File Offset: 0x00001CF5
		public bool GlobalDebugMode { get; set; }
	}
}
