using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace ImPlotNET
{
	// Token: 0x0200002B RID: 43
	public struct ImPlotStylePtr
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0002FB5C File Offset: 0x0002DD5C
		public unsafe readonly ImPlotStyle* NativePtr { get; }

		// Token: 0x060006DE RID: 1758 RVA: 0x0002FB64 File Offset: 0x0002DD64
		public unsafe ImPlotStylePtr(ImPlotStyle* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002FB6D File Offset: 0x0002DD6D
		public unsafe ImPlotStylePtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0002FB7B File Offset: 0x0002DD7B
		public unsafe static implicit operator ImPlotStylePtr(ImPlotStyle* nativePtr)
		{
			return new ImPlotStylePtr(nativePtr);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0002FB83 File Offset: 0x0002DD83
		public unsafe static implicit operator ImPlotStyle*(ImPlotStylePtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0002FB8C File Offset: 0x0002DD8C
		public static implicit operator ImPlotStylePtr(IntPtr nativePtr)
		{
			return new ImPlotStylePtr(nativePtr);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0002FB94 File Offset: 0x0002DD94
		public unsafe ref float LineWeight
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->LineWeight));
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0002FBA7 File Offset: 0x0002DDA7
		public unsafe ref int Marker
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->Marker));
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0002FBBA File Offset: 0x0002DDBA
		public unsafe ref float MarkerSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->MarkerSize));
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0002FBCD File Offset: 0x0002DDCD
		public unsafe ref float MarkerWeight
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->MarkerWeight));
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0002FBE0 File Offset: 0x0002DDE0
		public unsafe ref float FillAlpha
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->FillAlpha));
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0002FBF3 File Offset: 0x0002DDF3
		public unsafe ref float ErrorBarSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ErrorBarSize));
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0002FC06 File Offset: 0x0002DE06
		public unsafe ref float ErrorBarWeight
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ErrorBarWeight));
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0002FC19 File Offset: 0x0002DE19
		public unsafe ref float DigitalBitHeight
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->DigitalBitHeight));
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0002FC2C File Offset: 0x0002DE2C
		public unsafe ref float DigitalBitGap
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->DigitalBitGap));
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0002FC3F File Offset: 0x0002DE3F
		public unsafe ref float PlotBorderSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->PlotBorderSize));
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x0002FC52 File Offset: 0x0002DE52
		public unsafe ref float MinorAlpha
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->MinorAlpha));
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0002FC65 File Offset: 0x0002DE65
		public unsafe ref Vector2 MajorTickLen
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MajorTickLen));
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0002FC78 File Offset: 0x0002DE78
		public unsafe ref Vector2 MinorTickLen
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MinorTickLen));
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0002FC8B File Offset: 0x0002DE8B
		public unsafe ref Vector2 MajorTickSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MajorTickSize));
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0002FC9E File Offset: 0x0002DE9E
		public unsafe ref Vector2 MinorTickSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MinorTickSize));
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0002FCB1 File Offset: 0x0002DEB1
		public unsafe ref Vector2 MajorGridSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MajorGridSize));
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0002FCC4 File Offset: 0x0002DEC4
		public unsafe ref Vector2 MinorGridSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MinorGridSize));
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0002FCD7 File Offset: 0x0002DED7
		public unsafe ref Vector2 PlotPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->PlotPadding));
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0002FCEA File Offset: 0x0002DEEA
		public unsafe ref Vector2 LabelPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->LabelPadding));
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0002FCFD File Offset: 0x0002DEFD
		public unsafe ref Vector2 LegendPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->LegendPadding));
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0002FD10 File Offset: 0x0002DF10
		public unsafe ref Vector2 LegendInnerPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->LegendInnerPadding));
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0002FD23 File Offset: 0x0002DF23
		public unsafe ref Vector2 LegendSpacing
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->LegendSpacing));
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0002FD36 File Offset: 0x0002DF36
		public unsafe ref Vector2 MousePosPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MousePosPadding));
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0002FD49 File Offset: 0x0002DF49
		public unsafe ref Vector2 AnnotationPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->AnnotationPadding));
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0002FD5C File Offset: 0x0002DF5C
		public unsafe ref Vector2 FitPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->FitPadding));
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0002FD6F File Offset: 0x0002DF6F
		public unsafe ref Vector2 PlotDefaultSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->PlotDefaultSize));
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0002FD82 File Offset: 0x0002DF82
		public unsafe ref Vector2 PlotMinSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->PlotMinSize));
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0002FD95 File Offset: 0x0002DF95
		public unsafe RangeAccessor<Vector4> Colors
		{
			get
			{
				return new RangeAccessor<Vector4>((void*)(&this.NativePtr->Colors_0), 21);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0002FDAA File Offset: 0x0002DFAA
		public unsafe ref ImPlotColormap Colormap
		{
			get
			{
				return Unsafe.AsRef<ImPlotColormap>((void*)(&this.NativePtr->Colormap));
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0002FDBD File Offset: 0x0002DFBD
		public unsafe ref bool UseLocalTime
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->UseLocalTime));
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0002FDD0 File Offset: 0x0002DFD0
		public unsafe ref bool UseISO8601
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->UseISO8601));
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0002FDE3 File Offset: 0x0002DFE3
		public unsafe ref bool Use24HourClock
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->Use24HourClock));
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0002FDF6 File Offset: 0x0002DFF6
		public void Destroy()
		{
			ImPlotNative.ImPlotStyle_destroy(this.NativePtr);
		}
	}
}
