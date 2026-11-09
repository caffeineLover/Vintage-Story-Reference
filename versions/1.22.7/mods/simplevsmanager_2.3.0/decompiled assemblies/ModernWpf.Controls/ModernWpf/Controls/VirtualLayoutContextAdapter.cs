using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000CA RID: 202
	internal class VirtualLayoutContextAdapter : NonVirtualizingLayoutContext
	{
		// Token: 0x06000A04 RID: 2564 RVA: 0x00022565 File Offset: 0x00020765
		public VirtualLayoutContextAdapter(VirtualizingLayoutContext virtualizingContext)
		{
			this.m_virtualizingContext = new WeakReference<VirtualizingLayoutContext>(virtualizingContext);
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0002257C File Offset: 0x0002077C
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x000225A0 File Offset: 0x000207A0
		protected override object LayoutStateCore
		{
			get
			{
				VirtualizingLayoutContext virtualizingLayoutContext;
				if (this.m_virtualizingContext.TryGetTarget(ref virtualizingLayoutContext))
				{
					return virtualizingLayoutContext.LayoutState;
				}
				return null;
			}
			set
			{
				VirtualizingLayoutContext virtualizingLayoutContext;
				if (this.m_virtualizingContext.TryGetTarget(ref virtualizingLayoutContext))
				{
					virtualizingLayoutContext.LayoutState = value;
				}
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x000225C4 File Offset: 0x000207C4
		protected override IReadOnlyList<UIElement> ChildrenCore
		{
			get
			{
				if (this.m_children == null)
				{
					VirtualizingLayoutContext virtualizingLayoutContext;
					this.m_virtualizingContext.TryGetTarget(ref virtualizingLayoutContext);
					this.m_children = new VirtualLayoutContextAdapter.ChildrenCollection<UIElement>(virtualizingLayoutContext);
				}
				return this.m_children;
			}
		}

		// Token: 0x04000421 RID: 1057
		private readonly WeakReference<VirtualizingLayoutContext> m_virtualizingContext;

		// Token: 0x04000422 RID: 1058
		private IReadOnlyList<UIElement> m_children;

		// Token: 0x02000122 RID: 290
		private class ChildrenCollection<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : UIElement
		{
			// Token: 0x06000CE6 RID: 3302 RVA: 0x0002B6AD File Offset: 0x000298AD
			public ChildrenCollection(VirtualizingLayoutContext context)
			{
				this.m_context = context;
			}

			// Token: 0x17000249 RID: 585
			// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x0002B6BC File Offset: 0x000298BC
			public int Count
			{
				get
				{
					return this.m_context.ItemCount;
				}
			}

			// Token: 0x1700024A RID: 586
			public T this[int index]
			{
				get
				{
					return (T)((object)this.m_context.GetOrCreateElementAt(index, ElementRealizationOptions.None));
				}
			}

			// Token: 0x06000CE9 RID: 3305 RVA: 0x0002B6DD File Offset: 0x000298DD
			public IEnumerator<T> GetEnumerator()
			{
				return new VirtualLayoutContextAdapter.ChildrenCollection<T>.Iterator(this);
			}

			// Token: 0x06000CEA RID: 3306 RVA: 0x0002B6E5 File Offset: 0x000298E5
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040005C0 RID: 1472
			private readonly VirtualizingLayoutContext m_context;

			// Token: 0x02000136 RID: 310
			private class Iterator : IEnumerator<T>, IEnumerator, IDisposable
			{
				// Token: 0x06000D24 RID: 3364 RVA: 0x0002BEFC File Offset: 0x0002A0FC
				public Iterator(IReadOnlyList<T> childCollection)
				{
					this.m_currentIndex = -1;
					base..ctor();
					this.m_childCollection = childCollection;
				}

				// Token: 0x06000D25 RID: 3365 RVA: 0x0002BF14 File Offset: 0x0002A114
				~Iterator()
				{
				}

				// Token: 0x1700024F RID: 591
				// (get) Token: 0x06000D26 RID: 3366 RVA: 0x0002BF3C File Offset: 0x0002A13C
				public T Current
				{
					get
					{
						if (this.m_currentIndex < this.m_childCollection.Count)
						{
							return this.m_childCollection[this.m_currentIndex];
						}
						throw new InvalidOperationException();
					}
				}

				// Token: 0x17000250 RID: 592
				// (get) Token: 0x06000D27 RID: 3367 RVA: 0x0002BF68 File Offset: 0x0002A168
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				// Token: 0x06000D28 RID: 3368 RVA: 0x0002BF75 File Offset: 0x0002A175
				public bool MoveNext()
				{
					if (this.m_currentIndex < this.m_childCollection.Count)
					{
						this.m_currentIndex++;
						return this.m_currentIndex < this.m_childCollection.Count;
					}
					throw new InvalidOperationException();
				}

				// Token: 0x06000D29 RID: 3369 RVA: 0x0002BFB1 File Offset: 0x0002A1B1
				public void Reset()
				{
					this.m_currentIndex = -1;
				}

				// Token: 0x06000D2A RID: 3370 RVA: 0x0002BFBA File Offset: 0x0002A1BA
				public void Dispose()
				{
				}

				// Token: 0x040005EA RID: 1514
				private readonly IReadOnlyList<T> m_childCollection;

				// Token: 0x040005EB RID: 1515
				private int m_currentIndex;
			}
		}
	}
}
