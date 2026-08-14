using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Proxy;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000DA RID: 218
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class MultiColumnChildDrawingObserver : ElementProxy
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00010C04 File Offset: 0x0000EE04
		public bool HasBeenDrawn
		{
			get
			{
				return this.ChildStateBeforeDrawingOperation != null;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x00010C0F File Offset: 0x0000EE0F
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x00010C17 File Offset: 0x0000EE17
		public object ChildStateBeforeDrawingOperation { get; private set; }

		// Token: 0x060006E7 RID: 1767 RVA: 0x00010C20 File Offset: 0x0000EE20
		internal override void Draw(Size availableSpace)
		{
			if (this.ChildStateBeforeDrawingOperation == null)
			{
				this.ChildStateBeforeDrawingOperation = (this.GetFirstElementChild() as IStateful).GetState();
			}
			base.Child.Draw(availableSpace);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00010C59 File Offset: 0x0000EE59
		internal void ResetDrawingState()
		{
			this.ChildStateBeforeDrawingOperation = null;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00010C62 File Offset: 0x0000EE62
		internal void RestoreState()
		{
			IStateful stateful = this.GetFirstElementChild() as IStateful;
			if (stateful == null)
			{
				return;
			}
			stateful.SetState(this.ChildStateBeforeDrawingOperation);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00010C80 File Offset: 0x0000EE80
		[NullableContext(1)]
		private Element GetFirstElementChild()
		{
			Element child = base.Child;
			for (;;)
			{
				ElementProxy elementProxy = child as ElementProxy;
				if (elementProxy == null)
				{
					break;
				}
				child = elementProxy.Child;
			}
			return child;
		}
	}
}
