using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using ProtoBuf;

namespace ConfigLib
{
	// Token: 0x02000025 RID: 37
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class ConfigSettingPacket
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00008353 File Offset: 0x00006553
		// (set) Token: 0x06000126 RID: 294 RVA: 0x0000835B File Offset: 0x0000655B
		public string Value { get; set; } = string.Empty;

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00008364 File Offset: 0x00006564
		// (set) Token: 0x06000128 RID: 296 RVA: 0x0000836C File Offset: 0x0000656C
		[Nullable(2)]
		public string MappingKey { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00008375 File Offset: 0x00006575
		// (set) Token: 0x0600012A RID: 298 RVA: 0x0000837D File Offset: 0x0000657D
		public bool ClientSide { get; private set; }

		// Token: 0x0600012B RID: 299 RVA: 0x00008386 File Offset: 0x00006586
		public ConfigSettingPacket()
		{
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000839C File Offset: 0x0000659C
		public ConfigSettingPacket(ConfigSetting settings)
		{
			this.Value = ConfigSettingPacket.Wrap(settings.Value.Token).ToString();
			this.MappingKey = settings.MappingKey;
			this.ClientSide = settings.ClientSide;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000083ED File Offset: 0x000065ED
		public static implicit operator ConfigSetting(ConfigSettingPacket setting)
		{
			return new ConfigSetting(setting);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000083F5 File Offset: 0x000065F5
		private static JObject Wrap(JToken token)
		{
			JObject jobject = new JObject();
			jobject.Add("value", token);
			return jobject;
		}
	}
}
