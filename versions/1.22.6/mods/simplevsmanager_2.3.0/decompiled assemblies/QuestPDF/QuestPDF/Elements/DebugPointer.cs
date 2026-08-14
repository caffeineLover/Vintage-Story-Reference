using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000BE RID: 190
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DebugPointer : ContainerElement
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x0000E3FB File Offset: 0x0000C5FB
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x0000E403 File Offset: 0x0000C603
		public DebugPointerType Type { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0000E40C File Offset: 0x0000C60C
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x0000E414 File Offset: 0x0000C614
		public string Label { get; set; }

		// Token: 0x060005AE RID: 1454 RVA: 0x0000E41D File Offset: 0x0000C61D
		[NullableContext(2)]
		internal override string GetCompanionSearchableContent()
		{
			return this.Label;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0000E425 File Offset: 0x0000C625
		public DebugPointer()
		{
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0000E42D File Offset: 0x0000C62D
		public DebugPointer(DebugPointerType type, string label)
		{
			this.Type = type;
			this.Label = label;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0000E443 File Offset: 0x0000C643
		[return: Nullable(new byte[]
		{
			2,
			0,
			1,
			1
		})]
		internal override IEnumerable<KeyValuePair<string, string>> GetCompanionProperties()
		{
			DebugPointer.<GetCompanionProperties>d__11 <GetCompanionProperties>d__ = new DebugPointer.<GetCompanionProperties>d__11(-2);
			<GetCompanionProperties>d__.<>4__this = this;
			return <GetCompanionProperties>d__;
		}
	}
}
