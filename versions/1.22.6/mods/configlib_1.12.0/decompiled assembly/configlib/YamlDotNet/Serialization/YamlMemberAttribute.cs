using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000F5 RID: 245
	[NullableContext(2)]
	[Nullable(0)]
	[AttributeUsage(384, AllowMultiple = false)]
	public sealed class YamlMemberAttribute : Attribute
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x000145D4 File Offset: 0x000127D4
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x000145DC File Offset: 0x000127DC
		public string Description { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x000145E5 File Offset: 0x000127E5
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x000145ED File Offset: 0x000127ED
		public Type SerializeAs { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x000145F6 File Offset: 0x000127F6
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x000145FE File Offset: 0x000127FE
		public int Order { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00014607 File Offset: 0x00012807
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x0001460F File Offset: 0x0001280F
		public string Alias { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00014618 File Offset: 0x00012818
		// (set) Token: 0x060005A5 RID: 1445 RVA: 0x00014620 File Offset: 0x00012820
		public bool ApplyNamingConventions { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00014629 File Offset: 0x00012829
		// (set) Token: 0x060005A7 RID: 1447 RVA: 0x00014631 File Offset: 0x00012831
		public ScalarStyle ScalarStyle { get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0001463A File Offset: 0x0001283A
		// (set) Token: 0x060005A9 RID: 1449 RVA: 0x00014647 File Offset: 0x00012847
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
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00014655 File Offset: 0x00012855
		public bool IsDefaultValuesHandlingSpecified
		{
			get
			{
				return this.defaultValuesHandling != null;
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00014662 File Offset: 0x00012862
		public YamlMemberAttribute()
		{
			this.ScalarStyle = ScalarStyle.Any;
			this.ApplyNamingConventions = true;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00014678 File Offset: 0x00012878
		[NullableContext(1)]
		public YamlMemberAttribute(Type serializeAs) : this()
		{
			if (serializeAs == null)
			{
				throw new ArgumentNullException("serializeAs");
			}
			this.SerializeAs = serializeAs;
		}

		// Token: 0x04000293 RID: 659
		private DefaultValuesHandling? defaultValuesHandling;
	}
}
