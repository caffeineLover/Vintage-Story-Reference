using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001D1 RID: 465
	[Guid("11D3B13A-180E-4789-A8BE-7712882893E6")]
	internal struct IReferenceTrackerVftbl
	{
		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0002B2B8 File Offset: 0x000294B8
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x0002B2C0 File Offset: 0x000294C0
		public method AddRefFromTrackerSource
		{
			get
			{
				return this._AddRefFromTrackerSource_4;
			}
			set
			{
				this._AddRefFromTrackerSource_4 = value;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0002B2C9 File Offset: 0x000294C9
		// (set) Token: 0x06000E69 RID: 3689 RVA: 0x0002B2D1 File Offset: 0x000294D1
		public method ReleaseFromTrackerSource
		{
			get
			{
				return this._ReleaseFromTrackerSource_5;
			}
			set
			{
				this._ReleaseFromTrackerSource_5 = value;
			}
		}

		// Token: 0x040002D8 RID: 728
		public IUnknownVftbl IUnknownVftbl;

		// Token: 0x040002D9 RID: 729
		private unsafe void* _ConnectFromTrackerSource_0;

		// Token: 0x040002DA RID: 730
		private unsafe void* _DisconnectFromTrackerSource_1;

		// Token: 0x040002DB RID: 731
		private unsafe void* _FindTrackerTargets_2;

		// Token: 0x040002DC RID: 732
		private unsafe void* _GetReferenceTrackerManager_3;

		// Token: 0x040002DD RID: 733
		private unsafe void* _AddRefFromTrackerSource_4;

		// Token: 0x040002DE RID: 734
		private unsafe void* _ReleaseFromTrackerSource_5;

		// Token: 0x040002DF RID: 735
		private unsafe void* _PegFromTrackerSource_6;

		// Token: 0x040002E0 RID: 736
		internal static readonly Guid IID = new Guid(299086138, 6158, 18313, 168, 190, 119, 18, 136, 40, 147, 230);
	}
}
