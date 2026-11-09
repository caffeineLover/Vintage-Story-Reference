using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000045 RID: 69
	[NullableContext(2)]
	[Nullable(0)]
	[AttributeUsage(384, AllowMultiple = false)]
	public sealed class YamlMemberAttribute : Attribute
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00006B1E File Offset: 0x00004D1E
		// (set) Token: 0x060001FA RID: 506 RVA: 0x00006B26 File Offset: 0x00004D26
		public string Description { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00006B2F File Offset: 0x00004D2F
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00006B37 File Offset: 0x00004D37
		public Type SerializeAs { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00006B40 File Offset: 0x00004D40
		// (set) Token: 0x060001FE RID: 510 RVA: 0x00006B48 File Offset: 0x00004D48
		public int Order { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00006B51 File Offset: 0x00004D51
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00006B59 File Offset: 0x00004D59
		public string Alias { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00006B62 File Offset: 0x00004D62
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00006B6A File Offset: 0x00004D6A
		public bool ApplyNamingConventions { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00006B73 File Offset: 0x00004D73
		// (set) Token: 0x06000204 RID: 516 RVA: 0x00006B7B File Offset: 0x00004D7B
		public ScalarStyle ScalarStyle { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00006B84 File Offset: 0x00004D84
		// (set) Token: 0x06000206 RID: 518 RVA: 0x00006B91 File Offset: 0x00004D91
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00006B9F File Offset: 0x00004D9F
		public bool IsDefaultValuesHandlingSpecified
		{
			get
			{
				return this.defaultValuesHandling != null;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00006BAC File Offset: 0x00004DAC
		public YamlMemberAttribute()
		{
			this.ScalarStyle = ScalarStyle.Any;
			this.ApplyNamingConventions = true;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00006BC2 File Offset: 0x00004DC2
		[NullableContext(1)]
		public YamlMemberAttribute(Type serializeAs) : this()
		{
			if (serializeAs == null)
			{
				throw new ArgumentNullException("serializeAs");
			}
			this.SerializeAs = serializeAs;
		}

		// Token: 0x0400007C RID: 124
		private DefaultValuesHandling? defaultValuesHandling;
	}
}
