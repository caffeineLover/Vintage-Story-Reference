using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul
{
	// Token: 0x02000008 RID: 8
	[NullableContext(1)]
	[Nullable(0)]
	public class WearableFueledLightSourceStats
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002ED7 File Offset: 0x000010D7
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002EDF File Offset: 0x000010DF
		public bool NeedsFuel { get; set; } = true;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002EE8 File Offset: 0x000010E8
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002EF0 File Offset: 0x000010F0
		public float FuelCapacityHours { get; set; } = 24f;

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002EF9 File Offset: 0x000010F9
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002F01 File Offset: 0x00001101
		public float FuelEfficiency { get; set; } = 1f;

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002F0A File Offset: 0x0000110A
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002F12 File Offset: 0x00001112
		public string FuelAttribute { get; set; } = "nightVisionFuelHours";

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002F1B File Offset: 0x0000111B
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002F23 File Offset: 0x00001123
		public string ToggleAttribute { get; set; } = "turnedOn";

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002F2C File Offset: 0x0000112C
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002F34 File Offset: 0x00001134
		public bool ConsumeFuelWhileSleeping { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002F3D File Offset: 0x0000113D
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002F45 File Offset: 0x00001145
		public byte[] LightHsv { get; set; } = new byte[3];

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002F4E File Offset: 0x0000114E
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002F56 File Offset: 0x00001156
		public byte[] TurnedOffLightHsv { get; set; } = new byte[3];

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002F5F File Offset: 0x0000115F
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002F67 File Offset: 0x00001167
		public Dictionary<string, float> FuelItems { get; set; } = new Dictionary<string, float>();

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002F70 File Offset: 0x00001170
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00002F78 File Offset: 0x00001178
		public float MaxFuelWasteFraction { get; set; } = 0.5f;
	}
}
