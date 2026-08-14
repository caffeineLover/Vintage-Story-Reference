using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x020000CB RID: 203
	internal class RepeaterUIElementCollection : UIElementCollection
	{
		// Token: 0x06000A08 RID: 2568 RVA: 0x000225F9 File Offset: 0x000207F9
		public RepeaterUIElementCollection(UIElement visualParent, FrameworkElement logicalParent) : base(visualParent, logicalParent)
		{
			this._visualChildren = new VisualCollection(visualParent);
			this._visualParent = visualParent;
			this._logicalParent = logicalParent;
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0002261D File Offset: 0x0002081D
		public override int Count
		{
			get
			{
				return this._visualChildren.Count;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0002262A File Offset: 0x0002082A
		public override bool IsSynchronized
		{
			get
			{
				return this._visualChildren.IsSynchronized;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x00022637 File Offset: 0x00020837
		public override object SyncRoot
		{
			get
			{
				return this._visualChildren.SyncRoot;
			}
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00022644 File Offset: 0x00020844
		public override void CopyTo(Array array, int index)
		{
			this._visualChildren.CopyTo(array, index);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00022654 File Offset: 0x00020854
		public override void CopyTo(UIElement[] array, int index)
		{
			this._visualChildren.CopyTo(array, index);
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x00022670 File Offset: 0x00020870
		// (set) Token: 0x06000A0F RID: 2575 RVA: 0x0002267D File Offset: 0x0002087D
		public override int Capacity
		{
			get
			{
				return this._visualChildren.Capacity;
			}
			set
			{
				this._visualChildren.Capacity = value;
			}
		}

		// Token: 0x170001B4 RID: 436
		public override UIElement this[int index]
		{
			get
			{
				return this._visualChildren[index] as UIElement;
			}
			set
			{
				this.ValidateElement(value);
				VisualCollection visualChildren = this._visualChildren;
				if (visualChildren[index] != value)
				{
					UIElement uielement = visualChildren[index] as UIElement;
					if (uielement != null)
					{
						base.ClearLogicalParent(uielement);
					}
					visualChildren[index] = value;
					base.SetLogicalParent(value);
				}
			}
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x000226EB File Offset: 0x000208EB
		public override int Add(UIElement element)
		{
			return this.AddInternal(element);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x000226F4 File Offset: 0x000208F4
		internal int AddInternal(UIElement element)
		{
			this.ValidateElement(element);
			base.SetLogicalParent(element);
			return this._visualChildren.Add(element);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00022710 File Offset: 0x00020910
		public override int IndexOf(UIElement element)
		{
			return this._visualChildren.IndexOf(element);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0002271E File Offset: 0x0002091E
		public override void Remove(UIElement element)
		{
			this.RemoveInternal(element);
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00022727 File Offset: 0x00020927
		internal void RemoveInternal(UIElement element)
		{
			this._visualChildren.Remove(element);
			base.ClearLogicalParent(element);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0002273C File Offset: 0x0002093C
		public override bool Contains(UIElement element)
		{
			return this._visualChildren.Contains(element);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0002274A File Offset: 0x0002094A
		public override void Clear()
		{
			this.ClearInternal();
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00022754 File Offset: 0x00020954
		internal void ClearInternal()
		{
			VisualCollection visualChildren = this._visualChildren;
			int count = visualChildren.Count;
			if (count > 0)
			{
				Visual[] array = new Visual[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = visualChildren[i];
				}
				visualChildren.Clear();
				for (int j = 0; j < count; j++)
				{
					UIElement uielement = array[j] as UIElement;
					if (uielement != null)
					{
						base.ClearLogicalParent(uielement);
					}
				}
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x000227BD File Offset: 0x000209BD
		public override void Insert(int index, UIElement element)
		{
			this.InsertInternal(index, element);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x000227C7 File Offset: 0x000209C7
		internal void InsertInternal(int index, UIElement element)
		{
			this.ValidateElement(element);
			base.SetLogicalParent(element);
			this._visualChildren.Insert(index, element);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x000227E4 File Offset: 0x000209E4
		public override void RemoveAt(int index)
		{
			VisualCollection visualChildren = this._visualChildren;
			UIElement uielement = visualChildren[index] as UIElement;
			visualChildren.RemoveAt(index);
			if (uielement != null)
			{
				base.ClearLogicalParent(uielement);
			}
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00022814 File Offset: 0x00020A14
		public override void RemoveRange(int index, int count)
		{
			this.RemoveRangeInternal(index, count);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00022820 File Offset: 0x00020A20
		internal void RemoveRangeInternal(int index, int count)
		{
			VisualCollection visualChildren = this._visualChildren;
			int count2 = visualChildren.Count;
			if (count > count2 - index)
			{
				count = count2 - index;
			}
			if (count > 0)
			{
				Visual[] array = new Visual[count];
				int i = index;
				for (int j = 0; j < count; j++)
				{
					array[j] = visualChildren[i];
					i++;
				}
				visualChildren.RemoveRange(index, count);
				for (i = 0; i < count; i++)
				{
					UIElement uielement = array[i] as UIElement;
					if (uielement != null)
					{
						base.ClearLogicalParent(uielement);
					}
				}
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0002289C File Offset: 0x00020A9C
		private void ValidateElement(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException(string.Format("Children of '{0}' cannot be null. Object derived from UIElement expected.", base.GetType()));
			}
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x000228B7 File Offset: 0x00020AB7
		public override IEnumerator GetEnumerator()
		{
			return this._visualChildren.GetEnumerator();
		}

		// Token: 0x04000423 RID: 1059
		private readonly VisualCollection _visualChildren;

		// Token: 0x04000424 RID: 1060
		private readonly UIElement _visualParent;

		// Token: 0x04000425 RID: 1061
		private readonly FrameworkElement _logicalParent;
	}
}
