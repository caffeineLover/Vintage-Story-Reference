using System;
using System.Runtime.CompilerServices;

namespace ImPlotNET
{
	// Token: 0x02000024 RID: 36
	public struct ImPlotRectPtr
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0002FA38 File Offset: 0x0002DC38
		public unsafe readonly ImPlotRect* NativePtr { get; }

		// Token: 0x060006CE RID: 1742 RVA: 0x0002FA40 File Offset: 0x0002DC40
		public unsafe ImPlotRectPtr(ImPlotRect* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0002FA49 File Offset: 0x0002DC49
		public unsafe ImPlotRectPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002FA57 File Offset: 0x0002DC57
		public unsafe static implicit operator ImPlotRectPtr(ImPlotRect* nativePtr)
		{
			return new ImPlotRectPtr(nativePtr);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002FA5F File Offset: 0x0002DC5F
		public unsafe static implicit operator ImPlotRect*(ImPlotRectPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002FA68 File Offset: 0x0002DC68
		public static implicit operator ImPlotRectPtr(IntPtr nativePtr)
		{
			return new ImPlotRectPtr(nativePtr);
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0002FA70 File Offset: 0x0002DC70
		public unsafe ref ImPlotRange X
		{
			get
			{
				return Unsafe.AsRef<ImPlotRange>((void*)(&this.NativePtr->X));
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0002FA83 File Offset: 0x0002DC83
		public unsafe ref ImPlotRange Y
		{
			get
			{
				return Unsafe.AsRef<ImPlotRange>((void*)(&this.NativePtr->Y));
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002FA98 File Offset: 0x0002DC98
		public unsafe ImPlotPoint Clamp(ImPlotPoint p)
		{
			ImPlotPoint result;
			ImPlotNative.ImPlotRect_Clamp_PlotPoInt(&result, this.NativePtr, p);
			return result;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002FAB8 File Offset: 0x0002DCB8
		public unsafe ImPlotPoint Clamp(double x, double y)
		{
			ImPlotPoint result;
			ImPlotNative.ImPlotRect_Clamp_double(&result, this.NativePtr, x, y);
			return result;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002FAD6 File Offset: 0x0002DCD6
		public bool Contains(ImPlotPoint p)
		{
			return ImPlotNative.ImPlotRect_Contains_PlotPoInt(this.NativePtr, p) > 0;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002FAE7 File Offset: 0x0002DCE7
		public bool Contains(double x, double y)
		{
			return ImPlotNative.ImPlotRect_Contains_double(this.NativePtr, x, y) > 0;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002FAF9 File Offset: 0x0002DCF9
		public void Destroy()
		{
			ImPlotNative.ImPlotRect_destroy(this.NativePtr);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0002FB08 File Offset: 0x0002DD08
		public unsafe ImPlotPoint Max()
		{
			ImPlotPoint result;
			ImPlotNative.ImPlotRect_Max(&result, this.NativePtr);
			return result;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002FB24 File Offset: 0x0002DD24
		public unsafe ImPlotPoint Min()
		{
			ImPlotPoint result;
			ImPlotNative.ImPlotRect_Min(&result, this.NativePtr);
			return result;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0002FB40 File Offset: 0x0002DD40
		public unsafe ImPlotPoint Size()
		{
			ImPlotPoint result;
			ImPlotNative.ImPlotRect_Size(&result, this.NativePtr);
			return result;
		}
	}
}
