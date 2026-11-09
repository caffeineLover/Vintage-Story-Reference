using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ProtoBuf;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	internal class SettingsPacket
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000430F File Offset: 0x0000250F
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00004317 File Offset: 0x00002517
		public string Domain { get; set; } = "";

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004320 File Offset: 0x00002520
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00004328 File Offset: 0x00002528
		public Dictionary<string, ConfigSettingPacket> Settings { get; set; } = new Dictionary<string, ConfigSettingPacket>();

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004331 File Offset: 0x00002531
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00004339 File Offset: 0x00002539
		public byte[] Definition { get; set; } = Array.Empty<byte>();

		// Token: 0x060000A3 RID: 163 RVA: 0x00004342 File Offset: 0x00002542
		public SettingsPacket()
		{
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000436C File Offset: 0x0000256C
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

		// Token: 0x060000A5 RID: 165 RVA: 0x00004424 File Offset: 0x00002624
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
