using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.UI.ViewManagement
{
	// Token: 0x02000027 RID: 39
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[ProjectedRuntimeClass("_default")]
	[ObjectReferenceWrapper("_inner")]
	[SupportedOSPlatform("Windows10.0.18362.0")]
	[ContractVersion(typeof(UniversalApiContract), 524288U)]
	internal sealed class UISettingsAutoHideScrollBarsChangedEventArgs : ICustomQueryInterface, IWinRTObject, IDynamicInterfaceCastable, IEquatable<UISettingsAutoHideScrollBarsChangedEventArgs>
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600026B RID: 619 RVA: 0x000049EE File Offset: 0x00002BEE
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

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00004A0F File Offset: 0x00002C0F
		private IObjectReference _objRef_global__Windows_UI_ViewManagement_IUISettingsAutoHideScrollBarsChangedEventArgs
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00004A17 File Offset: 0x00002C17
		private IUISettingsAutoHideScrollBarsChangedEventArgs _default
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00004A1A File Offset: 0x00002C1A
		public static UISettingsAutoHideScrollBarsChangedEventArgs FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return MarshalInspectable<UISettingsAutoHideScrollBarsChangedEventArgs>.FromAbi(thisPtr);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00004A31 File Offset: 0x00002C31
		internal UISettingsAutoHideScrollBarsChangedEventArgs(IObjectReference objRef)
		{
			this._inner = objRef.As(GuidGenerator.GetIID(typeof(IUISettingsAutoHideScrollBarsChangedEventArgs).GetHelperType()));
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00004A59 File Offset: 0x00002C59
		public static bool operator ==(UISettingsAutoHideScrollBarsChangedEventArgs x, UISettingsAutoHideScrollBarsChangedEventArgs y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00004A80 File Offset: 0x00002C80
		public static bool operator !=(UISettingsAutoHideScrollBarsChangedEventArgs x, UISettingsAutoHideScrollBarsChangedEventArgs y)
		{
			return !(x == y);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00004A8C File Offset: 0x00002C8C
		public bool Equals(UISettingsAutoHideScrollBarsChangedEventArgs other)
		{
			return this == other;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00004A98 File Offset: 0x00002C98
		public override bool Equals(object obj)
		{
			UISettingsAutoHideScrollBarsChangedEventArgs uisettingsAutoHideScrollBarsChangedEventArgs = obj as UISettingsAutoHideScrollBarsChangedEventArgs;
			return uisettingsAutoHideScrollBarsChangedEventArgs != null && this == uisettingsAutoHideScrollBarsChangedEventArgs;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00004AB8 File Offset: 0x00002CB8
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00004AD3 File Offset: 0x00002CD3
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000276 RID: 630 RVA: 0x00004AD6 File Offset: 0x00002CD6
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00004ADE File Offset: 0x00002CDE
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00004AFA File Offset: 0x00002CFA
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00004B0E File Offset: 0x00002D0E
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600027A RID: 634 RVA: 0x00004B2A File Offset: 0x00002D2A
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00004B3E File Offset: 0x00002D3E
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00004B44 File Offset: 0x00002D44
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

		// Token: 0x04000078 RID: 120
		private IObjectReference _inner;

		// Token: 0x04000079 RID: 121
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x0400007A RID: 122
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x020002BA RID: 698
		private struct InterfaceTag<I>
		{
		}
	}
}
