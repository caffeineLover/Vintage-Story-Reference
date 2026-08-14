using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Elements;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200003A RID: 58
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class ContainerElement : Element, IContainer
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00004C2D File Offset: 0x00002E2D
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00004C35 File Offset: 0x00002E35
		internal Element Child { get; set; } = Empty.Instance;

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00004C3E File Offset: 0x00002E3E
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00004C46 File Offset: 0x00002E46
		IElement IContainer.Child
		{
			get
			{
				return this.Child;
			}
			set
			{
				this.Child = (value as Element);
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00004C54 File Offset: 0x00002E54
		internal override IEnumerable<Element> GetChildren()
		{
			ContainerElement.<GetChildren>d__7 <GetChildren>d__ = new ContainerElement.<GetChildren>d__7(-2);
			<GetChildren>d__.<>4__this = this;
			return <GetChildren>d__;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00004C64 File Offset: 0x00002E64
		internal override void CreateProxy(Func<Element, Element> create)
		{
			this.Child = create.Invoke(this.Child);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00004C78 File Offset: 0x00002E78
		internal override SpacePlan Measure(Size availableSpace)
		{
			SpacePlan spacePlan = this.Child.Measure(availableSpace);
			if (spacePlan.Type == SpacePlanType.Wrap)
			{
				return SpacePlan.Wrap("Forwarded from child");
			}
			return spacePlan;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00004CA7 File Offset: 0x00002EA7
		internal override void Draw(Size availableSpace)
		{
			Element child = this.Child;
			if (child == null)
			{
				return;
			}
			child.Draw(availableSpace);
		}
	}
}
