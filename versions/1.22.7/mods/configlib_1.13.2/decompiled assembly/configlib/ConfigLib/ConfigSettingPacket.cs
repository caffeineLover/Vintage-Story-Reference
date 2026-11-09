using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using ProtoBuf;

namespace ConfigLib
{
	// Token: 0x02000027 RID: 39
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class ConfigSettingPacket
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000866F File Offset: 0x0000686F
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00008677 File Offset: 0x00006877
		public string Value { get; set; } = string.Empty;

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00008680 File Offset: 0x00006880
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00008688 File Offset: 0x00006888
		[Nullable(2)]
		public string MappingKey { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00008691 File Offset: 0x00006891
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00008699 File Offset: 0x00006899
		public bool ClientSide { get; private set; }

		// Token: 0x06000150 RID: 336 RVA: 0x000086A2 File Offset: 0x000068A2
		public ConfigSettingPacket()
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000086B8 File Offset: 0x000068B8
		public ConfigSettingPacket(ConfigSetting settings)
		{
			this.Value = ConfigSettingPacket.Wrap(settings.Value.Token).ToString();
			this.MappingKey = settings.MappingKey;
			this.ClientSide = settings.ClientSide;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00008709 File Offset: 0x00006909
		public static implicit operator ConfigSetting(ConfigSettingPacket setting)
		{
			return new ConfigSetting(setting);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00008711 File Offset: 0x00006911
		private static JObject Wrap(JToken token)
		{
			JObject jobject = new JObject();
			jobject.Add("value", token);
			return jobject;
		}
	}
}
