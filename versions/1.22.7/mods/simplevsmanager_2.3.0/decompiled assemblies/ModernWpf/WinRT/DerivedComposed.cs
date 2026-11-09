using System;
using System.ComponentModel;

namespace WinRT
{
	// Token: 0x0200018D RID: 397
	[EditorBrowsable(1)]
	internal class DerivedComposed
	{
		// Token: 0x06000C92 RID: 3218 RVA: 0x00023BA5 File Offset: 0x00021DA5
		private DerivedComposed()
		{
		}

		// Token: 0x04000220 RID: 544
		public static readonly DerivedComposed Instance = new DerivedComposed();
	}
}
