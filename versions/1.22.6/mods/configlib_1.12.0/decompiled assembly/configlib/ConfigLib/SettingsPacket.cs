using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ProtoBuf;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x02000018 RID: 24
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	internal class SettingsPacket
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003FEB File Offset: 0x000021EB
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00003FF3 File Offset: 0x000021F3
		public string Domain { get; set; } = "";

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003FFC File Offset: 0x000021FC
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00004004 File Offset: 0x00002204
		public Dictionary<string, ConfigSettingPacket> Settings { get; set; } = new Dictionary<string, ConfigSettingPacket>();

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007C RID: 124 RVA: 0x0000400D File Offset: 0x0000220D
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00004015 File Offset: 0x00002215
		public byte[] Definition { get; set; } = Array.Empty<byte>();

		// Token: 0x0600007E RID: 126 RVA: 0x0000401E File Offset: 0x0000221E
		public SettingsPacket()
		{
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004048 File Offset: 0x00002248
		public SettingsPacket(string domain, Dictionary<string, ConfigSetting> settings, JsonObject definition)
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
			this.Definition = Encoding.UTF8.GetBytes(definition.ToString());
			this.Settings = serialized;
			this.Domain = domain;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004100 File Offset: 0x00002300
		public Dictionary<string, ConfigSetting> GetSettings()
		{
			Dictionary<string, ConfigSetting> deserialized = new Dictionary<string, ConfigSetting>();
			foreach (KeyValuePair<string, ConfigSettingPacket> keyValuePair in this.Settings)
			{
				string text;
				ConfigSettingPacket configSettingPacket;
				keyValuePair.Deconstruct(ref text, ref configSettingPacket);
				string key = text;
				ConfigSettingPacket value = configSettingPacket;
				deserialized.Add(key, new ConfigSetting(value));
			}
			return deserialized;
		}
	}
}
