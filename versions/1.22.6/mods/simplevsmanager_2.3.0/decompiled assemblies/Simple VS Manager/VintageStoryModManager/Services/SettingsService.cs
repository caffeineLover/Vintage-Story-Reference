using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200008A RID: 138
	[NullableContext(1)]
	[Nullable(0)]
	public class SettingsService : ISettingsService
	{
		// Token: 0x06000B96 RID: 2966 RVA: 0x00047BC8 File Offset: 0x00045DC8
		public SettingsService()
		{
			string text = Path.Combine(Environment.GetFolderPath(26), "NOT SET YET");
			Directory.CreateDirectory(text);
			this._settingsPath = Path.Combine(text, "NOT SET YET");
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x00047C10 File Offset: 0x00045E10
		// (set) Token: 0x06000B98 RID: 2968 RVA: 0x00047C1D File Offset: 0x00045E1D
		public List<int> FavoriteMods
		{
			get
			{
				return this._settings.FavoriteMods;
			}
			set
			{
				this._settings.FavoriteMods = value;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x00047C2B File Offset: 0x00045E2B
		// (set) Token: 0x06000B9A RID: 2970 RVA: 0x00047C38 File Offset: 0x00045E38
		[Nullable(2)]
		public string LastInstallationPath
		{
			[NullableContext(2)]
			get
			{
				return this._settings.LastInstallationPath;
			}
			[NullableContext(2)]
			set
			{
				this._settings.LastInstallationPath = value;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x00047C46 File Offset: 0x00045E46
		// (set) Token: 0x06000B9C RID: 2972 RVA: 0x00047C53 File Offset: 0x00045E53
		public string OrderBy
		{
			get
			{
				return this._settings.OrderBy;
			}
			set
			{
				this._settings.OrderBy = value;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x00047C61 File Offset: 0x00045E61
		// (set) Token: 0x06000B9E RID: 2974 RVA: 0x00047C6E File Offset: 0x00045E6E
		public string OrderByDirection
		{
			get
			{
				return this._settings.OrderByDirection;
			}
			set
			{
				this._settings.OrderByDirection = value;
			}
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00047C7C File Offset: 0x00045E7C
		public Task SaveAsync()
		{
			SettingsService.<SaveAsync>d__15 <SaveAsync>d__;
			<SaveAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveAsync>d__.<>4__this = this;
			<SaveAsync>d__.<>1__state = -1;
			<SaveAsync>d__.<>t__builder.Start<SettingsService.<SaveAsync>d__15>(ref <SaveAsync>d__);
			return <SaveAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00047CC0 File Offset: 0x00045EC0
		public Task LoadAsync()
		{
			SettingsService.<LoadAsync>d__16 <LoadAsync>d__;
			<LoadAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadAsync>d__.<>4__this = this;
			<LoadAsync>d__.<>1__state = -1;
			<LoadAsync>d__.<>t__builder.Start<SettingsService.<LoadAsync>d__16>(ref <LoadAsync>d__);
			return <LoadAsync>d__.<>t__builder.Task;
		}

		// Token: 0x04000557 RID: 1367
		private readonly string _settingsPath;

		// Token: 0x04000558 RID: 1368
		private SettingsService.AppSettings _settings = new SettingsService.AppSettings();

		// Token: 0x02000303 RID: 771
		[Nullable(0)]
		private class AppSettings
		{
			// Token: 0x17000533 RID: 1331
			// (get) Token: 0x06001905 RID: 6405 RVA: 0x0009188B File Offset: 0x0008FA8B
			// (set) Token: 0x06001906 RID: 6406 RVA: 0x00091893 File Offset: 0x0008FA93
			public List<int> FavoriteMods { get; set; } = new List<int>();

			// Token: 0x17000534 RID: 1332
			// (get) Token: 0x06001907 RID: 6407 RVA: 0x0009189C File Offset: 0x0008FA9C
			// (set) Token: 0x06001908 RID: 6408 RVA: 0x000918A4 File Offset: 0x0008FAA4
			[Nullable(2)]
			public string LastInstallationPath { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x17000535 RID: 1333
			// (get) Token: 0x06001909 RID: 6409 RVA: 0x000918AD File Offset: 0x0008FAAD
			// (set) Token: 0x0600190A RID: 6410 RVA: 0x000918B5 File Offset: 0x0008FAB5
			public string OrderBy { get; set; } = "follows";

			// Token: 0x17000536 RID: 1334
			// (get) Token: 0x0600190B RID: 6411 RVA: 0x000918BE File Offset: 0x0008FABE
			// (set) Token: 0x0600190C RID: 6412 RVA: 0x000918C6 File Offset: 0x0008FAC6
			public string OrderByDirection { get; set; } = "desc";
		}
	}
}
