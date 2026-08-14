using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using ABI.Windows.UI.ViewManagement;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;
using WinRT.Interop;

namespace Windows.UI.ViewManagement
{
	// Token: 0x02000026 RID: 38
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[ProjectedRuntimeClass("_default")]
	[ObjectReferenceWrapper("_inner")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal sealed class UISettings : ICustomQueryInterface, IWinRTObject, IDynamicInterfaceCastable, IEquatable<UISettings>
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000455B File Offset: 0x0000275B
		private IntPtr ThisPtr
		{
			get
			{
				if (this._inner != null)
				{
					return this._inner.ThisPtr;
				}
				return ((IWinRTObject)this).NativeObject.ThisPtr;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000457C File Offset: 0x0000277C
		private IObjectReference _objRef_global__Windows_UI_ViewManagement_IUISettings
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00004584 File Offset: 0x00002784
		private IObjectReference Make___objRef_global__Windows_UI_ViewManagement_IUISettings2()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__Windows_UI_ViewManagement_IUISettings2, ((IWinRTObject)this).NativeObject.As<IUnknownVftbl>(GuidGenerator.GetIID(typeof(IUISettings2).GetHelperType())), null);
			return this.___objRef_global__Windows_UI_ViewManagement_IUISettings2;
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600023A RID: 570 RVA: 0x000045BA File Offset: 0x000027BA
		private IObjectReference _objRef_global__Windows_UI_ViewManagement_IUISettings2
		{
			get
			{
				return this.___objRef_global__Windows_UI_ViewManagement_IUISettings2 ?? this.Make___objRef_global__Windows_UI_ViewManagement_IUISettings2();
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000045CE File Offset: 0x000027CE
		private IObjectReference Make___objRef_global__Windows_UI_ViewManagement_IUISettings3()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__Windows_UI_ViewManagement_IUISettings3, ((IWinRTObject)this).NativeObject.As<IUnknownVftbl>(GuidGenerator.GetIID(typeof(IUISettings3).GetHelperType())), null);
			return this.___objRef_global__Windows_UI_ViewManagement_IUISettings3;
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00004604 File Offset: 0x00002804
		private IObjectReference _objRef_global__Windows_UI_ViewManagement_IUISettings3
		{
			get
			{
				return this.___objRef_global__Windows_UI_ViewManagement_IUISettings3 ?? this.Make___objRef_global__Windows_UI_ViewManagement_IUISettings3();
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00004618 File Offset: 0x00002818
		private IObjectReference Make___objRef_global__Windows_UI_ViewManagement_IUISettings4()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__Windows_UI_ViewManagement_IUISettings4, ((IWinRTObject)this).NativeObject.As<IUnknownVftbl>(GuidGenerator.GetIID(typeof(IUISettings4).GetHelperType())), null);
			return this.___objRef_global__Windows_UI_ViewManagement_IUISettings4;
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000464E File Offset: 0x0000284E
		private IObjectReference _objRef_global__Windows_UI_ViewManagement_IUISettings4
		{
			get
			{
				return this.___objRef_global__Windows_UI_ViewManagement_IUISettings4 ?? this.Make___objRef_global__Windows_UI_ViewManagement_IUISettings4();
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00004662 File Offset: 0x00002862
		private IObjectReference Make___objRef_global__Windows_UI_ViewManagement_IUISettings5()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__Windows_UI_ViewManagement_IUISettings5, ((IWinRTObject)this).NativeObject.As<IUnknownVftbl>(GuidGenerator.GetIID(typeof(IUISettings5).GetHelperType())), null);
			return this.___objRef_global__Windows_UI_ViewManagement_IUISettings5;
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00004698 File Offset: 0x00002898
		private IObjectReference _objRef_global__Windows_UI_ViewManagement_IUISettings5
		{
			get
			{
				return this.___objRef_global__Windows_UI_ViewManagement_IUISettings5 ?? this.Make___objRef_global__Windows_UI_ViewManagement_IUISettings5();
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000046AC File Offset: 0x000028AC
		private IUISettings _default
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x000046AF File Offset: 0x000028AF
		public UISettings() : this(ActivationFactory<UISettings>.ActivateInstance<IUnknownVftbl>())
		{
			ComWrappersSupport.RegisterObjectForInterface(this, this.ThisPtr);
			ComWrappersHelper.Init(this._inner, false);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000046D4 File Offset: 0x000028D4
		public static UISettings FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return MarshalInspectable<UISettings>.FromAbi(thisPtr);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000046EB File Offset: 0x000028EB
		internal UISettings(IObjectReference objRef)
		{
			this._inner = objRef.As(GuidGenerator.GetIID(typeof(IUISettings).GetHelperType()));
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00004713 File Offset: 0x00002913
		public static bool operator ==(UISettings x, UISettings y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000473A File Offset: 0x0000293A
		public static bool operator !=(UISettings x, UISettings y)
		{
			return !(x == y);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00004746 File Offset: 0x00002946
		public bool Equals(UISettings other)
		{
			return this == other;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00004750 File Offset: 0x00002950
		public override bool Equals(object obj)
		{
			UISettings uisettings = obj as UISettings;
			return uisettings != null && this == uisettings;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00004770 File Offset: 0x00002970
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000478B File Offset: 0x0000298B
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000478E File Offset: 0x0000298E
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00004796 File Offset: 0x00002996
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600024D RID: 589 RVA: 0x000047B2 File Offset: 0x000029B2
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000047C6 File Offset: 0x000029C6
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600024F RID: 591 RVA: 0x000047E2 File Offset: 0x000029E2
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000047F6 File Offset: 0x000029F6
		public Color UIElementColor(UIElementType desiredElement)
		{
			return IUISettingsMethods.UIElementColor(this._objRef_global__Windows_UI_ViewManagement_IUISettings, desiredElement);
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000251 RID: 593 RVA: 0x00004804 File Offset: 0x00002A04
		// (remove) Token: 0x06000252 RID: 594 RVA: 0x0000481D File Offset: 0x00002A1D
		public event TypedEventHandler<UISettings, object> TextScaleFactorChanged
		{
			add
			{
				IUISettings2Methods.Get_TextScaleFactorChanged(this._objRef_global__Windows_UI_ViewManagement_IUISettings2, this).Item1.Invoke(value);
			}
			remove
			{
				IUISettings2Methods.Get_TextScaleFactorChanged(this._objRef_global__Windows_UI_ViewManagement_IUISettings2, this).Item2.Invoke(value);
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00004836 File Offset: 0x00002A36
		public Color GetColorValue(UIColorType desiredColor)
		{
			return IUISettings3Methods.GetColorValue(this._objRef_global__Windows_UI_ViewManagement_IUISettings3, desiredColor);
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000254 RID: 596 RVA: 0x00004844 File Offset: 0x00002A44
		// (remove) Token: 0x06000255 RID: 597 RVA: 0x0000485D File Offset: 0x00002A5D
		public event TypedEventHandler<UISettings, object> ColorValuesChanged
		{
			add
			{
				IUISettings3Methods.Get_ColorValuesChanged(this._objRef_global__Windows_UI_ViewManagement_IUISettings3, this).Item1.Invoke(value);
			}
			remove
			{
				IUISettings3Methods.Get_ColorValuesChanged(this._objRef_global__Windows_UI_ViewManagement_IUISettings3, this).Item2.Invoke(value);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000256 RID: 598 RVA: 0x00004876 File Offset: 0x00002A76
		// (remove) Token: 0x06000257 RID: 599 RVA: 0x0000488F File Offset: 0x00002A8F
		[SupportedOSPlatform("Windows10.0.15063.0")]
		public event TypedEventHandler<UISettings, object> AdvancedEffectsEnabledChanged
		{
			add
			{
				IUISettings4Methods.Get_AdvancedEffectsEnabledChanged(this._objRef_global__Windows_UI_ViewManagement_IUISettings4, this).Item1.Invoke(value);
			}
			remove
			{
				IUISettings4Methods.Get_AdvancedEffectsEnabledChanged(this._objRef_global__Windows_UI_ViewManagement_IUISettings4, this).Item2.Invoke(value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000258 RID: 600 RVA: 0x000048A8 File Offset: 0x00002AA8
		// (remove) Token: 0x06000259 RID: 601 RVA: 0x000048C1 File Offset: 0x00002AC1
		[SupportedOSPlatform("Windows10.0.18362.0")]
		public event TypedEventHandler<UISettings, UISettingsAutoHideScrollBarsChangedEventArgs> AutoHideScrollBarsChanged
		{
			add
			{
				IUISettings5Methods.Get_AutoHideScrollBarsChanged(this._objRef_global__Windows_UI_ViewManagement_IUISettings5, this).Item1.Invoke(value);
			}
			remove
			{
				IUISettings5Methods.Get_AutoHideScrollBarsChanged(this._objRef_global__Windows_UI_ViewManagement_IUISettings5, this).Item2.Invoke(value);
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600025A RID: 602 RVA: 0x000048DA File Offset: 0x00002ADA
		[SupportedOSPlatform("Windows10.0.15063.0")]
		public bool AdvancedEffectsEnabled
		{
			get
			{
				return IUISettings4Methods.get_AdvancedEffectsEnabled(this._objRef_global__Windows_UI_ViewManagement_IUISettings4);
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600025B RID: 603 RVA: 0x000048E7 File Offset: 0x00002AE7
		public bool AnimationsEnabled
		{
			get
			{
				return IUISettingsMethods.get_AnimationsEnabled(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600025C RID: 604 RVA: 0x000048F4 File Offset: 0x00002AF4
		[SupportedOSPlatform("Windows10.0.18362.0")]
		public bool AutoHideScrollBars
		{
			get
			{
				return IUISettings5Methods.get_AutoHideScrollBars(this._objRef_global__Windows_UI_ViewManagement_IUISettings5);
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00004901 File Offset: 0x00002B01
		public uint CaretBlinkRate
		{
			get
			{
				return IUISettingsMethods.get_CaretBlinkRate(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000490E File Offset: 0x00002B0E
		public bool CaretBrowsingEnabled
		{
			get
			{
				return IUISettingsMethods.get_CaretBrowsingEnabled(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000491B File Offset: 0x00002B1B
		public uint CaretWidth
		{
			get
			{
				return IUISettingsMethods.get_CaretWidth(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00004928 File Offset: 0x00002B28
		public Size CursorSize
		{
			get
			{
				return IUISettingsMethods.get_CursorSize(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000261 RID: 609 RVA: 0x00004935 File Offset: 0x00002B35
		public uint DoubleClickTime
		{
			get
			{
				return IUISettingsMethods.get_DoubleClickTime(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00004942 File Offset: 0x00002B42
		public HandPreference HandPreference
		{
			get
			{
				return IUISettingsMethods.get_HandPreference(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000494F File Offset: 0x00002B4F
		public uint MessageDuration
		{
			get
			{
				return IUISettingsMethods.get_MessageDuration(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000495C File Offset: 0x00002B5C
		public uint MouseHoverTime
		{
			get
			{
				return IUISettingsMethods.get_MouseHoverTime(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00004969 File Offset: 0x00002B69
		public Size ScrollBarArrowSize
		{
			get
			{
				return IUISettingsMethods.get_ScrollBarArrowSize(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00004976 File Offset: 0x00002B76
		public Size ScrollBarSize
		{
			get
			{
				return IUISettingsMethods.get_ScrollBarSize(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00004983 File Offset: 0x00002B83
		public Size ScrollBarThumbBoxSize
		{
			get
			{
				return IUISettingsMethods.get_ScrollBarThumbBoxSize(this._objRef_global__Windows_UI_ViewManagement_IUISettings);
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00004990 File Offset: 0x00002B90
		public double TextScaleFactor
		{
			get
			{
				return IUISettings2Methods.get_TextScaleFactor(this._objRef_global__Windows_UI_ViewManagement_IUISettings2);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000499D File Offset: 0x00002B9D
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000049A0 File Offset: 0x00002BA0
		CustomQueryInterfaceResult ICustomQueryInterface.GetInterface(ref Guid iid, out IntPtr ppv)
		{
			ppv = IntPtr.Zero;
			if (this.IsOverridableInterface(iid) || InterfaceIIDs.IInspectable_IID == iid)
			{
				return 1;
			}
			if (((IWinRTObject)this).NativeObject.TryAs(iid, out ppv) >= 0)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x04000071 RID: 113
		private IObjectReference _inner;

		// Token: 0x04000072 RID: 114
		private volatile IObjectReference ___objRef_global__Windows_UI_ViewManagement_IUISettings2;

		// Token: 0x04000073 RID: 115
		private volatile IObjectReference ___objRef_global__Windows_UI_ViewManagement_IUISettings3;

		// Token: 0x04000074 RID: 116
		private volatile IObjectReference ___objRef_global__Windows_UI_ViewManagement_IUISettings4;

		// Token: 0x04000075 RID: 117
		private volatile IObjectReference ___objRef_global__Windows_UI_ViewManagement_IUISettings5;

		// Token: 0x04000076 RID: 118
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x04000077 RID: 119
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x020002B9 RID: 697
		private struct InterfaceTag<I>
		{
		}
	}
}
