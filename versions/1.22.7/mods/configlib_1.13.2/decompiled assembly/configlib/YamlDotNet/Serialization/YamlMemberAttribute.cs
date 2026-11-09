using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000F1 RID: 241
	[NullableContext(2)]
	[Nullable(0)]
	[AttributeUsage(384, AllowMultiple = false)]
	public sealed class YamlMemberAttribute : Attribute
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00013C8C File Offset: 0x00011E8C
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x00013C94 File Offset: 0x00011E94
		public string Description { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00013C9D File Offset: 0x00011E9D
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x00013CA5 File Offset: 0x00011EA5
		public Type SerializeAs { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00013CAE File Offset: 0x00011EAE
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00013CB6 File Offset: 0x00011EB6
		public int Order { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00013CBF File Offset: 0x00011EBF
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x00013CC7 File Offset: 0x00011EC7
		public string Alias { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00013CD0 File Offset: 0x00011ED0
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x00013CD8 File Offset: 0x00011ED8
		public bool ApplyNamingConventions { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x00013CE1 File Offset: 0x00011EE1
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x00013CE9 File Offset: 0x00011EE9
		public ScalarStyle ScalarStyle { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00013CF2 File Offset: 0x00011EF2
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x00013CFF File Offset: 0x00011EFF
		public DefaultValuesHandling DefaultValuesHandling
		{
			get
			{
				return this.defaultValuesHandling.GetValueOrDefault();
			}
			set
			{
				this.defaultValuesHandling = new DefaultValuesHandling?(value);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00013D0D File Offset: 0x00011F0D
		public bool IsDefaultValuesHandlingSpecified
		{
			get
			{
				return this.defaultValuesHandling != null;
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00013D1A File Offset: 0x00011F1A
		public YamlMemberAttribute()
		{
			this.ScalarStyle = ScalarStyle.Any;
			this.ApplyNamingConventions = true;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00013D30 File Offset: 0x00011F30
		[NullableContext(1)]
		public YamlMemberAttribute(Type serializeAs) : this()
		{
			if (serializeAs == null)
			{
				throw new ArgumentNullException("serializeAs");
			}
			this.SerializeAs = serializeAs;
		}

		// Token: 0x04000265 RID: 613
		private DefaultValuesHandling? defaultValuesHandling;
	}
}
