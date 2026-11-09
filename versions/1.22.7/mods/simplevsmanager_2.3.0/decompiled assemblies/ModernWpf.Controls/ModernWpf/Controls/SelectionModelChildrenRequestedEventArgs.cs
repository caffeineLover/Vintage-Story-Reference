using System;

namespace ModernWpf.Controls
{
	// Token: 0x020000D0 RID: 208
	public sealed class SelectionModelChildrenRequestedEventArgs : EventArgs
	{
		// Token: 0x06000A60 RID: 2656 RVA: 0x00023443 File Offset: 0x00021643
		internal SelectionModelChildrenRequestedEventArgs(object source, IndexPath sourceIndexPath, bool throwOnAccess)
		{
			this.Initialize(source, sourceIndexPath, throwOnAccess);
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0002345B File Offset: 0x0002165B
		public object Source
		{
			get
			{
				if (this.m_throwOnAccess)
				{
					throw new InvalidOperationException("Source can only be accesed in the ChildrenRequested event handler.");
				}
				return this.m_source;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00023476 File Offset: 0x00021676
		public IndexPath SourceIndex
		{
			get
			{
				if (this.m_throwOnAccess)
				{
					throw new Exception("SourceIndex can only be accesed in the ChildrenRequested event handler.");
				}
				return this.m_sourceIndexPath;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00023491 File Offset: 0x00021691
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x00023499 File Offset: 0x00021699
		public object Children { get; set; }

		// Token: 0x06000A65 RID: 2661 RVA: 0x000234A2 File Offset: 0x000216A2
		internal void Initialize(object source, IndexPath sourceIndexPath, bool throwOnAccess)
		{
			this.m_source = source;
			this.m_sourceIndexPath = sourceIndexPath;
			this.m_throwOnAccess = throwOnAccess;
			this.Children = null;
		}

		// Token: 0x04000436 RID: 1078
		private object m_source;

		// Token: 0x04000437 RID: 1079
		private IndexPath m_sourceIndexPath;

		// Token: 0x04000438 RID: 1080
		private bool m_throwOnAccess = true;
	}
}
