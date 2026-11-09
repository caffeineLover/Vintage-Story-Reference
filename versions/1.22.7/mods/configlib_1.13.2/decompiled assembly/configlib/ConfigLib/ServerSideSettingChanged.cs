using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace ConfigLib
{
	// Token: 0x0200001B RID: 27
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	internal class ServerSideSettingChanged
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004498 File Offset: 0x00002698
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x000044A0 File Offset: 0x000026A0
		public Dictionary<string, ConfigSettingPacket> Settings { get; set; } = new Dictionary<string, ConfigSettingPacket>();

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000044A9 File Offset: 0x000026A9
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000044B1 File Offset: 0x000026B1
		public string ConfigDomain { get; set; } = "";

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000044BA File Offset: 0x000026BA
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000044C2 File Offset: 0x000026C2
		public bool IsSinglePlayer { get; set; }

		// Token: 0x060000AC RID: 172 RVA: 0x000044CB File Offset: 0x000026CB
		public ServerSideSettingChanged()
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000044EC File Offset: 0x000026EC
		public ServerSideSettingChanged(string domain, Dictionary<string, ConfigSetting> settings, bool isSinglePlayer)
		{
			Dictionary<string, ConfigSettingPacket> serialized = new Dictionary<string, ConfigSettingPacket>();
			foreach (KeyValuePair<string, ConfigSetting> keyValuePair in settings)
			{
				string text;
				ConfigSetting configSetting;
				keyValuePair.Deconstruct(ref text, ref configSetting);
				string key = text;
				ConfigSetting value = configSetting;
				serialized.Add(key, new ConfigSettingPacket(value));
			}
			this.Settings = serialized;
			this.ConfigDomain = domain;
			this.IsSinglePlayer = isSinglePlayer;
		}
	}
}
