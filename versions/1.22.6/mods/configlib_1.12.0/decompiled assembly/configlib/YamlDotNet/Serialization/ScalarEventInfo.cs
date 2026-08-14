using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200008F RID: 143
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ScalarEventInfo : ObjectEventInfo
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x00010CBB File Offset: 0x0000EEBB
		public ScalarEventInfo(IObjectDescriptor source) : base(source)
		{
			this.Style = source.ScalarStyle;
			this.RenderedValue = string.Empty;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00010CDB File Offset: 0x0000EEDB
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00010CE3 File Offset: 0x0000EEE3
		public string RenderedValue { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00010CEC File Offset: 0x0000EEEC
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x00010CF4 File Offset: 0x0000EEF4
		public ScalarStyle Style { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00010CFD File Offset: 0x0000EEFD
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00010D05 File Offset: 0x0000EF05
		public bool IsPlainImplicit { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00010D0E File Offset: 0x0000EF0E
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00010D16 File Offset: 0x0000EF16
		public bool IsQuotedImplicit { get; set; }
	}
}
