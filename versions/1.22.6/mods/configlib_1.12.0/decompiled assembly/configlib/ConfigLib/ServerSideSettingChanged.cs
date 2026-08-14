using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace ConfigLib
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	internal class ServerSideSettingChanged
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00004174 File Offset: 0x00002374
		// (set) Token: 0x06000082 RID: 130 RVA: 0x0000417C File Offset: 0x0000237C
		public Dictionary<string, ConfigSettingPacket> Settings { get; set; } = new Dictionary<string, ConfigSettingPacket>();

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00004185 File Offset: 0x00002385
		// (set) Token: 0x06000084 RID: 132 RVA: 0x0000418D File Offset: 0x0000238D
		public string ConfigDomain { get; set; } = "";

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00004196 File Offset: 0x00002396
		// (set) Token: 0x06000086 RID: 134 RVA: 0x0000419E File Offset: 0x0000239E
		public bool IsSinglePlayer { get; set; }

		// Token: 0x06000087 RID: 135 RVA: 0x000041A7 File Offset: 0x000023A7
		public ServerSideSettingChanged()
		{
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000041C8 File Offset: 0x000023C8
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
