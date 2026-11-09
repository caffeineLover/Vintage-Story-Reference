using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Design
{
	// Token: 0x020000CA RID: 202
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModBrowserViewDesignData
	{
		// Token: 0x06001036 RID: 4150 RVA: 0x00056820 File Offset: 0x00054A20
		public ModBrowserViewDesignData()
		{
			ObservableCollection<DownloadableModOnList> observableCollection = new ObservableCollection<DownloadableModOnList>();
			DownloadableModOnList downloadableModOnList = new DownloadableModOnList();
			downloadableModOnList.ModId = 1;
			downloadableModOnList.AssetId = 101;
			downloadableModOnList.Name = "Better HUD";
			downloadableModOnList.Summary = "Adds additional HUD widgets and customization options for an improved user experience.";
			downloadableModOnList.Author = "Aurora";
			downloadableModOnList.Downloads = 15234;
			downloadableModOnList.Follows = 1823;
			downloadableModOnList.Comments = 142;
			downloadableModOnList.Side = "Client";
			downloadableModOnList.Type = "Code Mod";
			downloadableModOnList.LogoFileDatabase = "https://moddbcdn.vintagestory.at/playercorpse_86bd2916a59eeddb4b02df4d78a66304.png";
			downloadableModOnList.Logo = "https://moddbcdn.vintagestory.at/playercorpse_86bd2916a59eeddb4b02df4d78a66304.png";
			List<string> list = new List<string>();
			list.Add("UI");
			list.Add("Client");
			downloadableModOnList.Tags = list;
			downloadableModOnList.LastReleased = "2024-11-15";
			downloadableModOnList.UserReportDisplay = "Fully functional (12)";
			downloadableModOnList.UserReportTooltip = "Fully functional (12)\nNo issues noticed (4)\nSome issues but works (2)\nNot functional (1)\nCrashes/Freezes game (0)";
			downloadableModOnList.ShowUserReportBadge = true;
			downloadableModOnList.IsInstalled = true;
			observableCollection.Add(downloadableModOnList);
			DownloadableModOnList downloadableModOnList2 = new DownloadableModOnList();
			downloadableModOnList2.ModId = 2;
			downloadableModOnList2.AssetId = 102;
			downloadableModOnList2.Name = "Expanded Storage";
			downloadableModOnList2.Summary = "Improves and rebalances all storage-related blocks and recipes. Adds new storage types and sizes.";
			downloadableModOnList2.Author = "Epsilon";
			downloadableModOnList2.Downloads = 28901;
			downloadableModOnList2.Follows = 3456;
			downloadableModOnList2.Comments = 278;
			downloadableModOnList2.Side = "Both";
			downloadableModOnList2.Type = "Code Mod";
			downloadableModOnList2.LogoFileDatabase = "https://moddbcdn.vintagestory.at/modicon1_c4dff1d57426dd7a9e6fec18057f1fc7.png";
			downloadableModOnList2.Logo = "https://moddbcdn.vintagestory.at/modicon1_c4dff1d57426dd7a9e6fec18057f1fc7.png";
			List<string> list2 = new List<string>();
			list2.Add("Storage");
			list2.Add("Gameplay");
			downloadableModOnList2.Tags = list2;
			downloadableModOnList2.LastReleased = "2024-11-10";
			downloadableModOnList2.UserReportDisplay = "No issues noticed (8)";
			downloadableModOnList2.UserReportTooltip = "Fully functional (6)\nNo issues noticed (8)\nSome issues but works (1)\nNot functional (0)\nCrashes/Freezes game (0)";
			downloadableModOnList2.ShowUserReportBadge = true;
			downloadableModOnList2.IsInstalled = true;
			observableCollection.Add(downloadableModOnList2);
			DownloadableModOnList downloadableModOnList3 = new DownloadableModOnList();
			downloadableModOnList3.ModId = 3;
			downloadableModOnList3.AssetId = 103;
			downloadableModOnList3.Name = "World Edit Plus";
			downloadableModOnList3.Summary = "Advanced world editing tools with region presets, macros and powerful selection options.";
			downloadableModOnList3.Author = "Lyra";
			downloadableModOnList3.Downloads = 45678;
			downloadableModOnList3.Follows = 5234;
			downloadableModOnList3.Comments = 389;
			downloadableModOnList3.Side = "Both";
			downloadableModOnList3.Type = "Code Mod";
			downloadableModOnList3.LogoFileDatabase = "https://moddbcdn.vintagestory.at/modicon_910cef72d741fec6c488e5c5cf99f84f.png";
			downloadableModOnList3.Logo = "https://moddbcdn.vintagestory.at/modicon_910cef72d741fec6c488e5c5cf99f84f.png";
			List<string> list3 = new List<string>();
			list3.Add("Tools");
			list3.Add("Admin");
			downloadableModOnList3.Tags = list3;
			downloadableModOnList3.LastReleased = "2024-11-20";
			downloadableModOnList3.UserReportDisplay = "Mixed (6)";
			downloadableModOnList3.UserReportTooltip = "Fully functional (3)\nNo issues noticed (3)\nSome issues but works (3)\nNot functional (1)\nCrashes/Freezes game (0)";
			downloadableModOnList3.ShowUserReportBadge = true;
			downloadableModOnList3.IsInstalled = true;
			observableCollection.Add(downloadableModOnList3);
			DownloadableModOnList downloadableModOnList4 = new DownloadableModOnList();
			downloadableModOnList4.ModId = 4;
			downloadableModOnList4.AssetId = 104;
			downloadableModOnList4.Name = "Carry Capacity";
			downloadableModOnList4.Summary = "Realistic inventory weight system that adds depth to the survival experience.";
			downloadableModOnList4.Author = "Nova";
			downloadableModOnList4.Downloads = 12456;
			downloadableModOnList4.Follows = 1567;
			downloadableModOnList4.Comments = 98;
			downloadableModOnList4.Side = "Both";
			downloadableModOnList4.Type = "Code Mod";
			downloadableModOnList4.LogoFileDatabase = "https://moddbcdn.vintagestory.at/thumbnail2_594aaea088bc90b76bef20fedb95b70a.png";
			List<string> list4 = new List<string>();
			list4.Add("Gameplay");
			list4.Add("Survival");
			downloadableModOnList4.Tags = list4;
			downloadableModOnList4.LastReleased = "2024-10-30";
			downloadableModOnList4.UserReportDisplay = "Some issues but works (3)";
			downloadableModOnList4.UserReportTooltip = "Fully functional (1)\nNo issues noticed (0)\nSome issues but works (3)\nNot functional (0)\nCrashes/Freezes game (0)";
			downloadableModOnList4.ShowUserReportBadge = true;
			downloadableModOnList4.IsInstalled = false;
			observableCollection.Add(downloadableModOnList4);
			DownloadableModOnList downloadableModOnList5 = new DownloadableModOnList();
			downloadableModOnList5.ModId = 5;
			downloadableModOnList5.AssetId = 105;
			downloadableModOnList5.Name = "Medieval Expansion";
			downloadableModOnList5.Summary = "Adds hundreds of new medieval-themed blocks, items, and decorative elements to the game.";
			downloadableModOnList5.Author = "Rigel";
			downloadableModOnList5.Downloads = 67890;
			downloadableModOnList5.Follows = 8912;
			downloadableModOnList5.Comments = 523;
			downloadableModOnList5.Side = "Both";
			downloadableModOnList5.Type = "Content Mod";
			downloadableModOnList5.LogoFileDatabase = "https://moddbcdn.vintagestory.at/logo3_fbd3936aa3bd17ad6e69eb38b6bb80c8.jpg";
			downloadableModOnList5.Logo = "https://moddbcdn.vintagestory.at/logo3_fbd3936aa3bd17ad6e69eb38b6bb80c8.jpg";
			List<string> list5 = new List<string>();
			list5.Add("Content");
			list5.Add("Building");
			list5.Add("Medieval");
			downloadableModOnList5.Tags = list5;
			downloadableModOnList5.LastReleased = "2024-11-18";
			downloadableModOnList5.UserReportDisplay = "No votes";
			downloadableModOnList5.UserReportTooltip = "Fully functional (0)\nNo issues noticed (0)\nSome issues but works (0)\nNot functional (0)\nCrashes/Freezes game (0)";
			observableCollection.Add(downloadableModOnList5);
			DownloadableModOnList downloadableModOnList6 = new DownloadableModOnList();
			downloadableModOnList6.ModId = 6;
			downloadableModOnList6.AssetId = 106;
			downloadableModOnList6.Name = "Primitive Survival";
			downloadableModOnList6.Summary = "Enhanced survival mechanics focused on primitive technology and stone age gameplay.";
			downloadableModOnList6.Author = "Vega";
			downloadableModOnList6.Downloads = 34567;
			downloadableModOnList6.Follows = 4123;
			downloadableModOnList6.Comments = 267;
			downloadableModOnList6.Side = "Both";
			downloadableModOnList6.Type = "Code Mod";
			downloadableModOnList6.Logo = "https://mods.vintagestory.at/web/img/mod-default.png";
			List<string> list6 = new List<string>();
			list6.Add("Survival");
			list6.Add("Gameplay");
			list6.Add("Hardcore");
			downloadableModOnList6.Tags = list6;
			downloadableModOnList6.LastReleased = "2024-11-12";
			downloadableModOnList6.UserReportDisplay = "Crashes/Freezes game (2)";
			downloadableModOnList6.UserReportTooltip = "Fully functional (0)\nNo issues noticed (1)\nSome issues but works (0)\nNot functional (0)\nCrashes/Freezes game (2)";
			downloadableModOnList6.ShowUserReportBadge = true;
			downloadableModOnList6.IsInstalled = false;
			observableCollection.Add(downloadableModOnList6);
			this.ModsList = observableCollection;
			ObservableCollection<int> observableCollection2 = new ObservableCollection<int>();
			observableCollection2.Add(2);
			observableCollection2.Add(5);
			this.FavoriteMods = observableCollection2;
			ObservableCollection<int> observableCollection3 = new ObservableCollection<int>();
			observableCollection3.Add(1);
			observableCollection3.Add(2);
			observableCollection3.Add(3);
			this.InstalledMods = observableCollection3;
			this.AvailableVersions = new ObservableCollection<GameVersion>();
			this.AvailableTags = new ObservableCollection<ModTag>();
			this.SelectedVersions = new ObservableCollection<GameVersion>();
			this.SelectedTags = new ObservableCollection<ModTag>();
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x00056DA2 File Offset: 0x00054FA2
		public ObservableCollection<DownloadableModOnList> ModsList { get; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x00056DAA File Offset: 0x00054FAA
		public IEnumerable<DownloadableModOnList> VisibleMods
		{
			get
			{
				return this.ModsList;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x00056DB2 File Offset: 0x00054FB2
		public ObservableCollection<int> FavoriteMods { get; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x00056DBA File Offset: 0x00054FBA
		public ObservableCollection<int> InstalledMods { get; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x00056DC2 File Offset: 0x00054FC2
		public ObservableCollection<GameVersion> AvailableVersions { get; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600103C RID: 4156 RVA: 0x00056DCA File Offset: 0x00054FCA
		public ObservableCollection<ModTag> AvailableTags { get; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x00056DD2 File Offset: 0x00054FD2
		// (set) Token: 0x0600103E RID: 4158 RVA: 0x00056DDA File Offset: 0x00054FDA
		public ObservableCollection<GameVersion> SelectedVersions { get; set; }

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x00056DE3 File Offset: 0x00054FE3
		// (set) Token: 0x06001040 RID: 4160 RVA: 0x00056DEB File Offset: 0x00054FEB
		public ObservableCollection<ModTag> SelectedTags { get; set; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x00056DF4 File Offset: 0x00054FF4
		public bool IsSearching
		{
			get
			{
				return false;
			}
		}
	}
}
