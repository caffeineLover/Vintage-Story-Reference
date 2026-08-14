using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x0200004D RID: 77
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModUsageVoteCandidateViewModel : ObservableObject
	{
		// Token: 0x0600085D RID: 2141 RVA: 0x00033C45 File Offset: 0x00031E45
		public ModUsageVoteCandidateViewModel(ModListItemViewModel mod, int usageCount, ModUsageTrackingKey trackingKey)
		{
			if (mod == null)
			{
				throw new ArgumentNullException("mod");
			}
			this.Mod = mod;
			this.UsageCount = ((usageCount < 0) ? 0 : usageCount);
			this.TrackingKey = trackingKey;
			this._isSelected = true;
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x00033C7F File Offset: 0x00031E7F
		public ModListItemViewModel Mod { get; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00033C87 File Offset: 0x00031E87
		public ModUsageTrackingKey TrackingKey { get; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x00033C8F File Offset: 0x00031E8F
		public string ModId
		{
			get
			{
				return this.Mod.ModId ?? string.Empty;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x00033CA8 File Offset: 0x00031EA8
		public string DisplayLabel
		{
			get
			{
				string displayName = this.Mod.DisplayName;
				string modId = this.Mod.ModId;
				if (string.IsNullOrWhiteSpace(modId))
				{
					return displayName ?? string.Empty;
				}
				string text = modId.Trim();
				if (string.IsNullOrWhiteSpace(displayName))
				{
					return text;
				}
				return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", displayName.Trim(), text);
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x00033D07 File Offset: 0x00031F07
		public int UsageCount { get; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x00033D0F File Offset: 0x00031F0F
		public string UsageSummary
		{
			get
			{
				if (this.UsageCount != 1)
				{
					return string.Format(CultureInfo.CurrentCulture, "Used in {0} long sessions", this.UsageCount);
				}
				return "Used in 1 long session";
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x00033D3A File Offset: 0x00031F3A
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x00033D42 File Offset: 0x00031F42
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				base.SetProperty<bool>(ref this._isSelected, value, "IsSelected");
			}
		}

		// Token: 0x04000423 RID: 1059
		private bool _isSelected;
	}
}
