using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000052 RID: 82
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UpdateModSelectionViewModel : ObservableObject
	{
		// Token: 0x06000889 RID: 2185 RVA: 0x0003443C File Offset: 0x0003263C
		public UpdateModSelectionViewModel(ModListItemViewModel mod, bool isSelected, [Nullable(2)] ModReleaseInfo overrideRelease, bool isServerOptionsEnabled)
		{
			if (mod == null)
			{
				throw new ArgumentNullException("mod");
			}
			this.Mod = mod;
			this._overrideRelease = overrideRelease;
			this._isSelected = isSelected;
			this._isServerOptionsEnabled = isServerOptionsEnabled;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00034470 File Offset: 0x00032670
		public ModListItemViewModel Mod { get; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00034478 File Offset: 0x00032678
		public string DisplayName
		{
			get
			{
				return this.Mod.DisplayName;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00034485 File Offset: 0x00032685
		public string InstalledVersionDisplay
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.Mod.Version))
				{
					return this.Mod.Version;
				}
				return "—";
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x000344AC File Offset: 0x000326AC
		public string TargetVersionDisplay
		{
			get
			{
				ModReleaseInfo targetRelease = this.TargetRelease;
				string text = ((targetRelease != null) ? targetRelease.Version : null) ?? this.Mod.Version;
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
				return "—";
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x000344EA File Offset: 0x000326EA
		[Nullable(2)]
		public ModReleaseInfo TargetRelease
		{
			[NullableContext(2)]
			get
			{
				return this._overrideRelease ?? this.Mod.LatestCompatibleRelease;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x00034501 File Offset: 0x00032701
		[Nullable(2)]
		public string TargetUpdateVersion
		{
			[NullableContext(2)]
			get
			{
				ModReleaseInfo targetRelease = this.TargetRelease;
				if (targetRelease == null)
				{
					return null;
				}
				return targetRelease.Version;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00034514 File Offset: 0x00032714
		public bool CanSkip
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.TargetUpdateVersion);
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x00034524 File Offset: 0x00032724
		[Nullable(2)]
		private string LatestServerInstallVersion
		{
			[NullableContext(2)]
			get
			{
				ModReleaseInfo targetRelease = this.TargetRelease;
				return ((targetRelease != null) ? targetRelease.Version : null) ?? this.Mod.Version;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00034547 File Offset: 0x00032747
		public bool ShowServerInstallCommand
		{
			get
			{
				return this._isServerOptionsEnabled && !string.IsNullOrWhiteSpace(this.Mod.ModId) && !string.IsNullOrWhiteSpace(this.LatestServerInstallVersion);
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x00034573 File Offset: 0x00032773
		[Nullable(2)]
		public string LatestInstallCommand
		{
			[NullableContext(2)]
			get
			{
				if (!this.ShowServerInstallCommand)
				{
					return null;
				}
				return ServerCommandBuilder.TryBuildInstallCommand(this.Mod.ModId, this.LatestServerInstallVersion);
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00034598 File Offset: 0x00032798
		public string VersionSummary
		{
			get
			{
				string installedVersionDisplay = this.InstalledVersionDisplay;
				string targetVersionDisplay = this.TargetVersionDisplay;
				if (string.Equals(installedVersionDisplay, targetVersionDisplay, 5))
				{
					return "Installed: " + installedVersionDisplay;
				}
				return "Installed: " + installedVersionDisplay + " → Update to: " + targetVersionDisplay;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x000345DA File Offset: 0x000327DA
		public IReadOnlyList<ModListItemViewModel.ReleaseChangelog> Changelogs
		{
			get
			{
				return this.Mod.GetChangelogEntriesForUpgrade(this.TargetUpdateVersion);
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x000345ED File Offset: 0x000327ED
		public int ChangelogCount
		{
			get
			{
				return this.Changelogs.Count;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x000345FA File Offset: 0x000327FA
		public bool HasChangelogs
		{
			get
			{
				return this.ChangelogCount > 0;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00034608 File Offset: 0x00032808
		public string ChangelogHeader
		{
			get
			{
				if (this.ChangelogCount > 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(13, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Changelogs (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.ChangelogCount);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return "Changelog";
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0003465A File Offset: 0x0003285A
		public bool ShowExcludeVersionButton
		{
			get
			{
				return !this.IsSelected && this.CanSkip;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0003466C File Offset: 0x0003286C
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x00034674 File Offset: 0x00032874
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(this._isSelected, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsSelected);
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ShowExcludeVersionButton);
					this._isSelected = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsSelected);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ShowExcludeVersionButton);
				}
			}
		}

		// Token: 0x04000434 RID: 1076
		private readonly bool _isServerOptionsEnabled;

		// Token: 0x04000435 RID: 1077
		[Nullable(2)]
		private readonly ModReleaseInfo _overrideRelease;

		// Token: 0x04000436 RID: 1078
		[ObservableProperty]
		[NotifyPropertyChangedFor("ShowExcludeVersionButton")]
		private bool _isSelected;
	}
}
