using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000A0 RID: 160
	[NullableContext(1)]
	[Nullable(0)]
	public class DownloadableModOnList : INotifyPropertyChanged
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x00051231 File Offset: 0x0004F431
		// (set) Token: 0x06000DAE RID: 3502 RVA: 0x00051239 File Offset: 0x0004F439
		[JsonPropertyName("modid")]
		public int ModId { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x00051242 File Offset: 0x0004F442
		// (set) Token: 0x06000DB0 RID: 3504 RVA: 0x0005124A File Offset: 0x0004F44A
		[JsonPropertyName("assetid")]
		public int AssetId { get; set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x00051253 File Offset: 0x0004F453
		// (set) Token: 0x06000DB2 RID: 3506 RVA: 0x0005125B File Offset: 0x0004F45B
		[JsonPropertyName("downloads")]
		public int Downloads { get; set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x00051264 File Offset: 0x0004F464
		// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x0005126C File Offset: 0x0004F46C
		[JsonPropertyName("follows")]
		public int Follows { get; set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00051275 File Offset: 0x0004F475
		// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x0005127D File Offset: 0x0004F47D
		[JsonPropertyName("trendingpoints")]
		public int TrendingPoints { get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00051286 File Offset: 0x0004F486
		// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x0005128E File Offset: 0x0004F48E
		[JsonPropertyName("comments")]
		public int Comments { get; set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x00051297 File Offset: 0x0004F497
		// (set) Token: 0x06000DBA RID: 3514 RVA: 0x0005129F File Offset: 0x0004F49F
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x000512A8 File Offset: 0x0004F4A8
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x000512B0 File Offset: 0x0004F4B0
		[Nullable(2)]
		[JsonPropertyName("summary")]
		public string Summary { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x000512B9 File Offset: 0x0004F4B9
		// (set) Token: 0x06000DBE RID: 3518 RVA: 0x000512C1 File Offset: 0x0004F4C1
		[JsonPropertyName("modidstrs")]
		public List<string> ModIdStrings { get; set; } = new List<string>();

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x000512CA File Offset: 0x0004F4CA
		// (set) Token: 0x06000DC0 RID: 3520 RVA: 0x000512D2 File Offset: 0x0004F4D2
		[JsonPropertyName("author")]
		public string Author { get; set; } = string.Empty;

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x000512DB File Offset: 0x0004F4DB
		// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x000512E3 File Offset: 0x0004F4E3
		[Nullable(2)]
		[JsonPropertyName("urlalias")]
		public string UrlAlias { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x000512EC File Offset: 0x0004F4EC
		// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x000512F4 File Offset: 0x0004F4F4
		[JsonPropertyName("side")]
		public string Side { get; set; } = string.Empty;

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x000512FD File Offset: 0x0004F4FD
		// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x00051305 File Offset: 0x0004F505
		[JsonPropertyName("type")]
		public string Type { get; set; } = string.Empty;

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x0005130E File Offset: 0x0004F50E
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x00051318 File Offset: 0x0004F518
		[Nullable(2)]
		[JsonPropertyName("logo")]
		public string Logo
		{
			[NullableContext(2)]
			get
			{
				return this._logo;
			}
			[NullableContext(2)]
			set
			{
				if (value == this._logo)
				{
					return;
				}
				string logoUrl = this.LogoUrl;
				this._logo = value;
				this.OnPropertyChanged("Logo");
				if (this.LogoUrl != logoUrl)
				{
					this.OnPropertyChanged("LogoUrl");
				}
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00051366 File Offset: 0x0004F566
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x00051370 File Offset: 0x0004F570
		[Nullable(2)]
		[JsonPropertyName("logofiledb")]
		public string LogoFileDatabase
		{
			[NullableContext(2)]
			get
			{
				return this._logoFileDatabase;
			}
			[NullableContext(2)]
			set
			{
				if (value == this._logoFileDatabase)
				{
					return;
				}
				string logoUrl = this.LogoUrl;
				this._logoFileDatabase = value;
				this.OnPropertyChanged("LogoFileDatabase");
				if (this.LogoUrl != logoUrl)
				{
					this.OnPropertyChanged("LogoUrl");
				}
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x000513BE File Offset: 0x0004F5BE
		// (set) Token: 0x06000DCC RID: 3532 RVA: 0x000513C6 File Offset: 0x0004F5C6
		[JsonPropertyName("tags")]
		public List<string> Tags { get; set; } = new List<string>();

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x000513CF File Offset: 0x0004F5CF
		// (set) Token: 0x06000DCE RID: 3534 RVA: 0x000513D7 File Offset: 0x0004F5D7
		[JsonPropertyName("lastreleased")]
		public string LastReleased { get; set; } = string.Empty;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000DCF RID: 3535 RVA: 0x000513E0 File Offset: 0x0004F5E0
		// (remove) Token: 0x06000DD0 RID: 3536 RVA: 0x00051418 File Offset: 0x0004F618
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x0005144D File Offset: 0x0004F64D
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x00051455 File Offset: 0x0004F655
		public string UserReportDisplay
		{
			get
			{
				return this._userReportDisplay;
			}
			set
			{
				if (value == this._userReportDisplay)
				{
					return;
				}
				this._userReportDisplay = value;
				this.OnPropertyChanged("UserReportDisplay");
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x00051478 File Offset: 0x0004F678
		// (set) Token: 0x06000DD4 RID: 3540 RVA: 0x00051480 File Offset: 0x0004F680
		public bool ShowUserReportBadge
		{
			get
			{
				return this._showUserReportBadge;
			}
			set
			{
				if (value == this._showUserReportBadge)
				{
					return;
				}
				this._showUserReportBadge = value;
				this.OnPropertyChanged("ShowUserReportBadge");
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x0005149E File Offset: 0x0004F69E
		// (set) Token: 0x06000DD6 RID: 3542 RVA: 0x000514A6 File Offset: 0x0004F6A6
		public string UserReportTooltip
		{
			get
			{
				return this._userReportTooltip;
			}
			set
			{
				if (value == this._userReportTooltip)
				{
					return;
				}
				this._userReportTooltip = value;
				this.OnPropertyChanged("UserReportTooltip");
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x000514C9 File Offset: 0x0004F6C9
		// (set) Token: 0x06000DD8 RID: 3544 RVA: 0x000514D1 File Offset: 0x0004F6D1
		public bool IsInstalled
		{
			get
			{
				return this._isInstalled;
			}
			set
			{
				if (value == this._isInstalled)
				{
					return;
				}
				this._isInstalled = value;
				this.OnPropertyChanged("IsInstalled");
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x000514F0 File Offset: 0x0004F6F0
		public string FormattedDownloads
		{
			get
			{
				if (this.Downloads <= 10000)
				{
					return this.Downloads.ToString();
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Downloads / 1000);
				defaultInterpolatedStringHandler.AppendLiteral("K");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x00051548 File Offset: 0x0004F748
		public string FormattedFollows
		{
			get
			{
				if (this.Follows <= 10000)
				{
					return this.Follows.ToString();
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Follows / 1000);
				defaultInterpolatedStringHandler.AppendLiteral("K");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x000515A0 File Offset: 0x0004F7A0
		public string FormattedComments
		{
			get
			{
				if (this.Comments <= 10000)
				{
					return this.Comments.ToString();
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Comments / 1000);
				defaultInterpolatedStringHandler.AppendLiteral("K");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x000515F8 File Offset: 0x0004F7F8
		public string LogoUrl
		{
			get
			{
				string result;
				if ((result = this.LogoFileDatabase) == null)
				{
					result = (this.Logo ?? string.Empty);
				}
				return result;
			}
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00051613 File Offset: 0x0004F813
		[NullableContext(2)]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x04000616 RID: 1558
		[Nullable(2)]
		private string _logo;

		// Token: 0x04000617 RID: 1559
		[Nullable(2)]
		private string _logoFileDatabase;

		// Token: 0x0400061A RID: 1562
		private string _userReportDisplay = string.Empty;

		// Token: 0x0400061B RID: 1563
		private string _userReportTooltip = "User reports require a known Vintage Story version.";

		// Token: 0x0400061C RID: 1564
		private bool _showUserReportBadge;

		// Token: 0x0400061D RID: 1565
		private bool _isInstalled;
	}
}
