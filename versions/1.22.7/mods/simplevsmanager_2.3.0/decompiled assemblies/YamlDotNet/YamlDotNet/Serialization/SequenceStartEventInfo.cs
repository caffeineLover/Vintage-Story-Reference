using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000014 RID: 20
	public sealed class SequenceStartEventInfo : ObjectEventInfo
	{
		// Token: 0x0600009B RID: 155 RVA: 0x000039AB File Offset: 0x00001BAB
		[NullableContext(1)]
		public SequenceStartEventInfo(IObjectDescriptor source) : base(source)
		{
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000039B4 File Offset: 0x00001BB4
		// (set) Token: 0x0600009D RID: 157 RVA: 0x000039BC File Offset: 0x00001BBC
		public bool IsImplicit { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000039C5 File Offset: 0x00001BC5
		// (set) Token: 0x0600009F RID: 159 RVA: 0x000039CD File Offset: 0x00001BCD
		public SequenceStyle Style { get; set; }
	}
}
