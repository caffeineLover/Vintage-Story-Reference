using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing
{
	// Token: 0x0200010C RID: 268
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DocumentContainer : IDocumentContainer
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x00017272 File Offset: 0x00015472
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x0001727A File Offset: 0x0001547A
		internal List<IComponent> Pages { get; set; } = new List<IComponent>();

		// Token: 0x06000934 RID: 2356 RVA: 0x00017284 File Offset: 0x00015484
		internal Container Compose()
		{
			Container container = new Container();
			this.<Compose>g__ComposeContainer|4_0(container);
			return container;
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x000172B4 File Offset: 0x000154B4
		[CompilerGenerated]
		private void <Compose>g__ComposeContainer|4_0(IContainer container)
		{
			if (this.Pages.Count == 0)
			{
				this.Pages.Add(new Page());
			}
			container = container.DebugPointer(DebugPointerType.DocumentStructure, DocumentStructureTypes.Document.ToString());
			if (this.Pages.Count == 1)
			{
				container.Component(Enumerable.First<IComponent>(this.Pages));
				return;
			}
			container.Column(delegate(ColumnDescriptor column)
			{
				Action <>9__4;
				Enumerable.ToList<Action>(Enumerable.Skip<Action>(Enumerable.SelectMany<IComponent, Action>(this.Pages, delegate(IComponent x)
				{
					List<Action> list = new List<Action>();
					Action action;
					if ((action = <>9__4) == null)
					{
						action = (<>9__4 = delegate()
						{
							column.Item().PageBreak();
						});
					}
					list.Add(action);
					list.Add(delegate()
					{
						column.Item().SemanticTag("Part", null, null).Component(x);
					});
					return list;
				}), 1)).ForEach(delegate(Action x)
				{
					x.Invoke();
				});
			});
		}
	}
}
