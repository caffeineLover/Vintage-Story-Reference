using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001F6 RID: 502
	[Guid("00000035-0000-0000-C000-000000000046")]
	internal struct IActivationFactoryVftbl
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0002B415 File Offset: 0x00029615
		public method ActivateInstance
		{
			get
			{
				return this._ActivateInstance;
			}
		}

		// Token: 0x040002E6 RID: 742
		public IInspectable.Vftbl IInspectableVftbl;

		// Token: 0x040002E7 RID: 743
		private unsafe void* _ActivateInstance;

		// Token: 0x040002E8 RID: 744
		internal static readonly Guid IID = new Guid(53, 0, 0, 192, 0, 0, 0, 0, 0, 0, 70);
	}
}
