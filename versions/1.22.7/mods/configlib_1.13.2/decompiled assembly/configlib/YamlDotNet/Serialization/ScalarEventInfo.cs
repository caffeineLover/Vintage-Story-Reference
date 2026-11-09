using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200008B RID: 139
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ScalarEventInfo : ObjectEventInfo
	{
		// Token: 0x06000364 RID: 868 RVA: 0x00010373 File Offset: 0x0000E573
		public ScalarEventInfo(IObjectDescriptor source) : base(source)
		{
			this.Style = source.ScalarStyle;
			this.RenderedValue = string.Empty;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00010393 File Offset: 0x0000E593
		// (set) Token: 0x06000366 RID: 870 RVA: 0x0001039B File Offset: 0x0000E59B
		public string RenderedValue { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000367 RID: 871 RVA: 0x000103A4 File Offset: 0x0000E5A4
		// (set) Token: 0x06000368 RID: 872 RVA: 0x000103AC File Offset: 0x0000E5AC
		public ScalarStyle Style { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000369 RID: 873 RVA: 0x000103B5 File Offset: 0x0000E5B5
		// (set) Token: 0x0600036A RID: 874 RVA: 0x000103BD File Offset: 0x0000E5BD
		public bool IsPlainImplicit { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600036B RID: 875 RVA: 0x000103C6 File Offset: 0x0000E5C6
		// (set) Token: 0x0600036C RID: 876 RVA: 0x000103CE File Offset: 0x0000E5CE
		public bool IsQuotedImplicit { get; set; }
	}
}
