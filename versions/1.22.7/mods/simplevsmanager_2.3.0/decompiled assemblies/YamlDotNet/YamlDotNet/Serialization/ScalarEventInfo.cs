using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000011 RID: 17
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ScalarEventInfo : ObjectEventInfo
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00003913 File Offset: 0x00001B13
		public ScalarEventInfo(IObjectDescriptor source) : base(source)
		{
			this.Style = source.ScalarStyle;
			this.RenderedValue = string.Empty;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003933 File Offset: 0x00001B33
		// (set) Token: 0x0600008E RID: 142 RVA: 0x0000393B File Offset: 0x00001B3B
		public string RenderedValue { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003944 File Offset: 0x00001B44
		// (set) Token: 0x06000090 RID: 144 RVA: 0x0000394C File Offset: 0x00001B4C
		public ScalarStyle Style { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003955 File Offset: 0x00001B55
		// (set) Token: 0x06000092 RID: 146 RVA: 0x0000395D File Offset: 0x00001B5D
		public bool IsPlainImplicit { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003966 File Offset: 0x00001B66
		// (set) Token: 0x06000094 RID: 148 RVA: 0x0000396E File Offset: 0x00001B6E
		public bool IsQuotedImplicit { get; set; }
	}
}
