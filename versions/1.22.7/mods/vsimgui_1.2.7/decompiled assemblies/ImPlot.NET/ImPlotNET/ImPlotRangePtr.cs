using System;
using System.Runtime.CompilerServices;

namespace ImPlotNET
{
	// Token: 0x02000022 RID: 34
	public struct ImPlotRangePtr
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0002F9A1 File Offset: 0x0002DBA1
		public unsafe readonly ImPlotRange* NativePtr { get; }

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002F9A9 File Offset: 0x0002DBA9
		public unsafe ImPlotRangePtr(ImPlotRange* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002F9B2 File Offset: 0x0002DBB2
		public unsafe ImPlotRangePtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002F9C0 File Offset: 0x0002DBC0
		public unsafe static implicit operator ImPlotRangePtr(ImPlotRange* nativePtr)
		{
			return new ImPlotRangePtr(nativePtr);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0002F9C8 File Offset: 0x0002DBC8
		public unsafe static implicit operator ImPlotRange*(ImPlotRangePtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0002F9D1 File Offset: 0x0002DBD1
		public static implicit operator ImPlotRangePtr(IntPtr nativePtr)
		{
			return new ImPlotRangePtr(nativePtr);
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0002F9D9 File Offset: 0x0002DBD9
		public unsafe ref double Min
		{
			get
			{
				return Unsafe.AsRef<double>((void*)(&this.NativePtr->Min));
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0002F9EC File Offset: 0x0002DBEC
		public unsafe ref double Max
		{
			get
			{
				return Unsafe.AsRef<double>((void*)(&this.NativePtr->Max));
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002F9FF File Offset: 0x0002DBFF
		public double Clamp(double value)
		{
			return ImPlotNative.ImPlotRange_Clamp(this.NativePtr, value);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002FA0D File Offset: 0x0002DC0D
		public bool Contains(double value)
		{
			return ImPlotNative.ImPlotRange_Contains(this.NativePtr, value) > 0;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002FA1E File Offset: 0x0002DC1E
		public void Destroy()
		{
			ImPlotNative.ImPlotRange_destroy(this.NativePtr);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0002FA2B File Offset: 0x0002DC2B
		public double Size()
		{
			return ImPlotNative.ImPlotRange_Size(this.NativePtr);
		}
	}
}
