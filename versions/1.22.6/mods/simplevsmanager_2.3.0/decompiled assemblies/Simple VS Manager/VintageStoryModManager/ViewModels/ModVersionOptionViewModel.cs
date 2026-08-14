using System;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x0200004E RID: 78
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModVersionOptionViewModel : ObservableObject
	{
		// Token: 0x06000866 RID: 2150 RVA: 0x00033D57 File Offset: 0x00031F57
		[NullableContext(2)]
		private ModVersionOptionViewModel([Nullable(1)] string version, string normalizedVersion, ModReleaseInfo release, bool isCompatible, bool isInstalled, bool isFromDatabase)
		{
			this.Version = version;
			this.NormalizedVersion = normalizedVersion;
			this.Release = release;
			this.IsCompatible = isCompatible;
			this._isInstalled = isInstalled;
			this.IsFromDatabase = isFromDatabase;
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x00033D8C File Offset: 0x00031F8C
		public string Version { get; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x00033D94 File Offset: 0x00031F94
		[Nullable(2)]
		public string NormalizedVersion { [NullableContext(2)] get; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x00033D9C File Offset: 0x00031F9C
		[Nullable(2)]
		public ModReleaseInfo Release { [NullableContext(2)] get; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x00033DA4 File Offset: 0x00031FA4
		public bool IsCompatible { get; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x00033DAC File Offset: 0x00031FAC
		public bool IsFromDatabase { get; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x00033DB4 File Offset: 0x00031FB4
		public bool HasRelease
		{
			get
			{
				return this.Release != null;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x00033DBF File Offset: 0x00031FBF
		public string VersionDisplay
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.Version))
				{
					return this.Version;
				}
				return "Unknown";
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x00033DDA File Offset: 0x00031FDA
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x00033DE2 File Offset: 0x00031FE2
		public bool IsInstalled
		{
			get
			{
				return this._isInstalled;
			}
			set
			{
				if (base.SetProperty<bool>(ref this._isInstalled, value, "IsInstalled"))
				{
					base.OnPropertyChanged("SelectionDisplay");
				}
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00033E03 File Offset: 0x00032003
		public string SelectionDisplay
		{
			get
			{
				if (!this.IsInstalled)
				{
					return this.VersionDisplay;
				}
				return this.VersionDisplay + " (Installed)";
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00033E24 File Offset: 0x00032024
		public override string ToString()
		{
			return this.SelectionDisplay;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00033E2C File Offset: 0x0003202C
		public static ModVersionOptionViewModel FromRelease(ModReleaseInfo release, bool isInstalled)
		{
			ArgumentNullException.ThrowIfNull(release, "release");
			return new ModVersionOptionViewModel(release.Version, release.NormalizedVersion, release, release.IsCompatibleWithInstalledGame, isInstalled, true);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00033E53 File Offset: 0x00032053
		public static ModVersionOptionViewModel FromInstalledVersion(string version, [Nullable(2)] string normalizedVersion)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				version = "Unknown";
			}
			return new ModVersionOptionViewModel(version, normalizedVersion, null, false, true, false);
		}

		// Token: 0x04000427 RID: 1063
		private bool _isInstalled;
	}
}
