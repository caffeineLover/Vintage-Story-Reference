using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;

namespace ModernWpf
{
	// Token: 0x0200022D RID: 557
	internal class UISettingsResources : ResourceDictionary
	{
		// Token: 0x06001061 RID: 4193 RVA: 0x0002EE22 File Offset: 0x0002D022
		public UISettingsResources()
		{
			if (DesignMode.DesignModeEnabled)
			{
				return;
			}
			if (OSVersionHelper.IsWindows10OrGreater)
			{
				this.Initialize();
			}
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0002EE4A File Offset: 0x0002D04A
		[MethodImpl(8)]
		private void Initialize()
		{
			this._uiSettings = new UISettings();
			if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 4))
			{
				this.InitializeForContract4();
			}
			if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
			{
				this.InitializeForContract8();
			}
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0002EE7D File Offset: 0x0002D07D
		[MethodImpl(8)]
		private void InitializeForContract4()
		{
			this._uiSettings.AdvancedEffectsEnabledChanged += delegate(UISettings sender, object args)
			{
				this._dispatcher.BeginInvoke(new Action(this.ApplyAdvancedEffectsEnabled), Array.Empty<object>());
			};
			if (PackagedAppHelper.IsPackagedApp)
			{
				SystemEvents.UserPreferenceChanged += delegate(object sender, UserPreferenceChangedEventArgs args)
				{
					if (args.Category == 4)
					{
						this.ApplyAdvancedEffectsEnabled();
					}
				};
			}
			this.ApplyAdvancedEffectsEnabled();
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0002EEB4 File Offset: 0x0002D0B4
		[MethodImpl(8)]
		private void InitializeForContract8()
		{
			this._uiSettings.AutoHideScrollBarsChanged += delegate(UISettings sender, UISettingsAutoHideScrollBarsChangedEventArgs args)
			{
				this._dispatcher.BeginInvoke(new Action(this.ApplyAutoHideScrollBars), Array.Empty<object>());
			};
			this.ApplyAutoHideScrollBars();
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0002EED4 File Offset: 0x0002D0D4
		[MethodImpl(8)]
		private void ApplyAdvancedEffectsEnabled()
		{
			ResourceKey dropShadowKey = SystemParameters.DropShadowKey;
			if (this._uiSettings.AdvancedEffectsEnabled)
			{
				base.Remove(dropShadowKey);
				return;
			}
			base[dropShadowKey] = false;
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0002EF09 File Offset: 0x0002D109
		[MethodImpl(8)]
		private void ApplyAutoHideScrollBars()
		{
			base["AutoHideScrollBars"] = this._uiSettings.AutoHideScrollBars;
		}

		// Token: 0x04000448 RID: 1096
		private const string UniversalApiContractName = "Windows.Foundation.UniversalApiContract";

		// Token: 0x04000449 RID: 1097
		private const string AutoHideScrollBarsKey = "AutoHideScrollBars";

		// Token: 0x0400044A RID: 1098
		private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

		// Token: 0x0400044B RID: 1099
		private UISettings _uiSettings;
	}
}
