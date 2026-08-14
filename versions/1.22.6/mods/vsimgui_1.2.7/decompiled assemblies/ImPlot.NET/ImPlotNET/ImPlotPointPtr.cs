using System;
using System.Runtime.CompilerServices;

namespace ImPlotNET
{
	// Token: 0x02000020 RID: 32
	public struct ImPlotPointPtr
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0002F936 File Offset: 0x0002DB36
		public unsafe readonly ImPlotPoint* NativePtr { get; }

		// Token: 0x060006B9 RID: 1721 RVA: 0x0002F93E File Offset: 0x0002DB3E
		public unsafe ImPlotPointPtr(ImPlotPoint* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002F947 File Offset: 0x0002DB47
		public unsafe ImPlotPointPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002F955 File Offset: 0x0002DB55
		public unsafe static implicit operator ImPlotPointPtr(ImPlotPoint* nativePtr)
		{
			return new ImPlotPointPtr(nativePtr);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002F95D File Offset: 0x0002DB5D
		public unsafe static implicit operator ImPlotPoint*(ImPlotPointPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0002F966 File Offset: 0x0002DB66
		public static implicit operator ImPlotPointPtr(IntPtr nativePtr)
		{
			return new ImPlotPointPtr(nativePtr);
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0002F96E File Offset: 0x0002DB6E
		public unsafe ref double x
		{
			get
			{
				return Unsafe.AsRef<double>((void*)(&this.NativePtr->x));
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x0002F981 File Offset: 0x0002DB81
		public unsafe ref double y
		{
			get
			{
				return Unsafe.AsRef<double>((void*)(&this.NativePtr->y));
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002F994 File Offset: 0x0002DB94
		public void Destroy()
		{
			ImPlotNative.ImPlotPoint_destroy(this.NativePtr);
		}
	}
}
