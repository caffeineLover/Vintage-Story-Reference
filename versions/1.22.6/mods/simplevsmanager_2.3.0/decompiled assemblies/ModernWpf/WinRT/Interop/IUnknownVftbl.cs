using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001D2 RID: 466
	[Guid("00000000-0000-0000-C000-000000000046")]
	internal struct IUnknownVftbl
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E6B RID: 3691 RVA: 0x0002B321 File Offset: 0x00029521
		// (set) Token: 0x06000E6C RID: 3692 RVA: 0x0002B329 File Offset: 0x00029529
		public method QueryInterface
		{
			get
			{
				return this._QueryInterface;
			}
			set
			{
				this._QueryInterface = value;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E6D RID: 3693 RVA: 0x0002B332 File Offset: 0x00029532
		// (set) Token: 0x06000E6E RID: 3694 RVA: 0x0002B33A File Offset: 0x0002953A
		public method AddRef
		{
			get
			{
				return this._AddRef;
			}
			set
			{
				this._AddRef = value;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x0002B343 File Offset: 0x00029543
		// (set) Token: 0x06000E70 RID: 3696 RVA: 0x0002B34B File Offset: 0x0002954B
		public method Release
		{
			get
			{
				return this._Release;
			}
			set
			{
				this._Release = value;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0002B354 File Offset: 0x00029554
		public static IUnknownVftbl AbiToProjectionVftbl
		{
			get
			{
				return ComWrappersSupport.IUnknownVftbl;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0002B35B File Offset: 0x0002955B
		public static IntPtr AbiToProjectionVftblPtr
		{
			get
			{
				return ComWrappersSupport.IUnknownVftblPtr;
			}
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0002B362 File Offset: 0x00029562
		internal bool Equals(IUnknownVftbl other)
		{
			return this._QueryInterface == other._QueryInterface && this._AddRef == other._AddRef && this._Release == other._Release;
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0002B390 File Offset: 0x00029590
		internal unsafe static bool IsReferenceToManagedObject(IntPtr ptr)
		{
			return ((IntPtr*)((void*)ptr))->Equals(IUnknownVftbl.AbiToProjectionVftbl);
		}

		// Token: 0x040002E1 RID: 737
		private unsafe void* _QueryInterface;

		// Token: 0x040002E2 RID: 738
		private unsafe void* _AddRef;

		// Token: 0x040002E3 RID: 739
		private unsafe void* _Release;

		// Token: 0x040002E4 RID: 740
		internal static readonly Guid IID = InterfaceIIDs.IUnknown_IID;
	}
}
