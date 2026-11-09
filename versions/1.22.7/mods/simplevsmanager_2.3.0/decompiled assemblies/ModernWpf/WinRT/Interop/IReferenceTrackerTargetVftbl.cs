using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001F8 RID: 504
	[Guid("64BD43F8-bFEE-4EC4-B7EB-2935158DAE21")]
	internal struct IReferenceTrackerTargetVftbl
	{
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x0002B448 File Offset: 0x00029648
		public method AddRefFromReferenceTracker
		{
			get
			{
				return this._AddRefFromReferenceTracker;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x0002B450 File Offset: 0x00029650
		public method ReleaseFromReferenceTracker
		{
			get
			{
				return this._ReleaseFromReferenceTracker;
			}
		}

		// Token: 0x040002EB RID: 747
		public IUnknownVftbl IUnknownVftbl;

		// Token: 0x040002EC RID: 748
		private unsafe void* _AddRefFromReferenceTracker;

		// Token: 0x040002ED RID: 749
		private unsafe void* _ReleaseFromReferenceTracker;

		// Token: 0x040002EE RID: 750
		private unsafe void* _Peg;

		// Token: 0x040002EF RID: 751
		private unsafe void* _Unpeg;

		// Token: 0x040002F0 RID: 752
		internal static readonly Guid IID = new Guid(1690125304U, 49134, 20164, 183, 235, 41, 53, 21, 141, 174, 33);
	}
}
