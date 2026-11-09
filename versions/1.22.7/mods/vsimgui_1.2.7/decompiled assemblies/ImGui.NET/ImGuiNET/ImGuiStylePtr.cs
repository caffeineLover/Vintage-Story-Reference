using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x0200005D RID: 93
	public struct ImGuiStylePtr
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00015486 File Offset: 0x00013686
		public unsafe readonly ImGuiStyle* NativePtr { get; }

		// Token: 0x0600082C RID: 2092 RVA: 0x0001548E File Offset: 0x0001368E
		public unsafe ImGuiStylePtr(ImGuiStyle* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00015497 File Offset: 0x00013697
		public unsafe ImGuiStylePtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000154A5 File Offset: 0x000136A5
		public unsafe static implicit operator ImGuiStylePtr(ImGuiStyle* nativePtr)
		{
			return new ImGuiStylePtr(nativePtr);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000154AD File Offset: 0x000136AD
		public unsafe static implicit operator ImGuiStyle*(ImGuiStylePtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000154B6 File Offset: 0x000136B6
		public static implicit operator ImGuiStylePtr(IntPtr nativePtr)
		{
			return new ImGuiStylePtr(nativePtr);
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x000154BE File Offset: 0x000136BE
		public unsafe ref float Alpha
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->Alpha));
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x000154D1 File Offset: 0x000136D1
		public unsafe ref float DisabledAlpha
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->DisabledAlpha));
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x000154E4 File Offset: 0x000136E4
		public unsafe ref Vector2 WindowPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->WindowPadding));
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x000154F7 File Offset: 0x000136F7
		public unsafe ref float WindowRounding
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->WindowRounding));
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0001550A File Offset: 0x0001370A
		public unsafe ref float WindowBorderSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->WindowBorderSize));
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x0001551D File Offset: 0x0001371D
		public unsafe ref Vector2 WindowMinSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->WindowMinSize));
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x00015530 File Offset: 0x00013730
		public unsafe ref Vector2 WindowTitleAlign
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->WindowTitleAlign));
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x00015543 File Offset: 0x00013743
		public unsafe ref ImGuiDir WindowMenuButtonPosition
		{
			get
			{
				return Unsafe.AsRef<ImGuiDir>((void*)(&this.NativePtr->WindowMenuButtonPosition));
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x00015556 File Offset: 0x00013756
		public unsafe ref float ChildRounding
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ChildRounding));
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x00015569 File Offset: 0x00013769
		public unsafe ref float ChildBorderSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ChildBorderSize));
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x0001557C File Offset: 0x0001377C
		public unsafe ref float PopupRounding
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->PopupRounding));
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x0001558F File Offset: 0x0001378F
		public unsafe ref float PopupBorderSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->PopupBorderSize));
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x000155A2 File Offset: 0x000137A2
		public unsafe ref Vector2 FramePadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->FramePadding));
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x000155B5 File Offset: 0x000137B5
		public unsafe ref float FrameRounding
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->FrameRounding));
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x000155C8 File Offset: 0x000137C8
		public unsafe ref float FrameBorderSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->FrameBorderSize));
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x000155DB File Offset: 0x000137DB
		public unsafe ref Vector2 ItemSpacing
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->ItemSpacing));
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x000155EE File Offset: 0x000137EE
		public unsafe ref Vector2 ItemInnerSpacing
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->ItemInnerSpacing));
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x00015601 File Offset: 0x00013801
		public unsafe ref Vector2 CellPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->CellPadding));
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x00015614 File Offset: 0x00013814
		public unsafe ref Vector2 TouchExtraPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->TouchExtraPadding));
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x00015627 File Offset: 0x00013827
		public unsafe ref float IndentSpacing
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->IndentSpacing));
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x0001563A File Offset: 0x0001383A
		public unsafe ref float ColumnsMinSpacing
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ColumnsMinSpacing));
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x0001564D File Offset: 0x0001384D
		public unsafe ref float ScrollbarSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ScrollbarSize));
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x00015660 File Offset: 0x00013860
		public unsafe ref float ScrollbarRounding
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ScrollbarRounding));
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x00015673 File Offset: 0x00013873
		public unsafe ref float GrabMinSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->GrabMinSize));
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x00015686 File Offset: 0x00013886
		public unsafe ref float GrabRounding
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->GrabRounding));
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x00015699 File Offset: 0x00013899
		public unsafe ref float LogSliderDeadzone
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->LogSliderDeadzone));
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x000156AC File Offset: 0x000138AC
		public unsafe ref float TabRounding
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->TabRounding));
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x000156BF File Offset: 0x000138BF
		public unsafe ref float TabBorderSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->TabBorderSize));
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x000156D2 File Offset: 0x000138D2
		public unsafe ref float TabMinWidthForCloseButton
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->TabMinWidthForCloseButton));
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x000156E5 File Offset: 0x000138E5
		public unsafe ref ImGuiDir ColorButtonPosition
		{
			get
			{
				return Unsafe.AsRef<ImGuiDir>((void*)(&this.NativePtr->ColorButtonPosition));
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x000156F8 File Offset: 0x000138F8
		public unsafe ref Vector2 ButtonTextAlign
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->ButtonTextAlign));
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x0001570B File Offset: 0x0001390B
		public unsafe ref Vector2 SelectableTextAlign
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->SelectableTextAlign));
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x0001571E File Offset: 0x0001391E
		public unsafe ref float SeparatorTextBorderSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->SeparatorTextBorderSize));
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x00015731 File Offset: 0x00013931
		public unsafe ref Vector2 SeparatorTextAlign
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->SeparatorTextAlign));
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x00015744 File Offset: 0x00013944
		public unsafe ref Vector2 SeparatorTextPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->SeparatorTextPadding));
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x00015757 File Offset: 0x00013957
		public unsafe ref Vector2 DisplayWindowPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->DisplayWindowPadding));
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x0001576A File Offset: 0x0001396A
		public unsafe ref Vector2 DisplaySafeAreaPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->DisplaySafeAreaPadding));
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0001577D File Offset: 0x0001397D
		public unsafe ref float DockingSeparatorSize
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->DockingSeparatorSize));
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x00015790 File Offset: 0x00013990
		public unsafe ref float MouseCursorScale
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->MouseCursorScale));
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x000157A3 File Offset: 0x000139A3
		public unsafe ref bool AntiAliasedLines
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->AntiAliasedLines));
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x000157B6 File Offset: 0x000139B6
		public unsafe ref bool AntiAliasedLinesUseTex
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->AntiAliasedLinesUseTex));
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x000157C9 File Offset: 0x000139C9
		public unsafe ref bool AntiAliasedFill
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->AntiAliasedFill));
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x000157DC File Offset: 0x000139DC
		public unsafe ref float CurveTessellationTol
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->CurveTessellationTol));
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x000157EF File Offset: 0x000139EF
		public unsafe ref float CircleTessellationMaxError
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->CircleTessellationMaxError));
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00015802 File Offset: 0x00013A02
		public unsafe RangeAccessor<Vector4> Colors
		{
			get
			{
				return new RangeAccessor<Vector4>((void*)(&this.NativePtr->Colors_0), 55);
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x00015817 File Offset: 0x00013A17
		public unsafe ref float HoverStationaryDelay
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->HoverStationaryDelay));
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x0001582A File Offset: 0x00013A2A
		public unsafe ref float HoverDelayShort
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->HoverDelayShort));
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x0001583D File Offset: 0x00013A3D
		public unsafe ref float HoverDelayNormal
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->HoverDelayNormal));
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x00015850 File Offset: 0x00013A50
		public unsafe ref ImGuiHoveredFlags HoverFlagsForTooltipMouse
		{
			get
			{
				return Unsafe.AsRef<ImGuiHoveredFlags>((void*)(&this.NativePtr->HoverFlagsForTooltipMouse));
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x00015863 File Offset: 0x00013A63
		public unsafe ref ImGuiHoveredFlags HoverFlagsForTooltipNav
		{
			get
			{
				return Unsafe.AsRef<ImGuiHoveredFlags>((void*)(&this.NativePtr->HoverFlagsForTooltipNav));
			}
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00015876 File Offset: 0x00013A76
		public void Destroy()
		{
			ImGuiNative.ImGuiStyle_destroy(this.NativePtr);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00015883 File Offset: 0x00013A83
		public void ScaleAllSizes(float scale_factor)
		{
			ImGuiNative.ImGuiStyle_ScaleAllSizes(this.NativePtr, scale_factor);
		}
	}
}
