using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000075 RID: 117
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModLoadingTimingService
	{
		// Token: 0x06000AAD RID: 2733 RVA: 0x000435E8 File Offset: 0x000417E8
		public void RecordIconLoadTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalIconLoadingTimeMs += milliseconds;
				this._iconLoadCount++;
			}
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00043640 File Offset: 0x00041840
		public void RecordTagsLoadTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalTagsLoadingTimeMs += milliseconds;
				this._tagsLoadCount++;
			}
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x00043698 File Offset: 0x00041898
		public void RecordUserReportsLoadTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalUserReportsLoadingTimeMs += milliseconds;
				this._userReportsLoadCount++;
			}
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x000436F0 File Offset: 0x000418F0
		public void RecordDependencyCheckTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDependencyChecksTimeMs += milliseconds;
				this._dependencyCheckCount++;
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00043748 File Offset: 0x00041948
		public void RecordUpdateCheckTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalUpdateCheckTimeMs += milliseconds;
				this._updateCheckCount++;
			}
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000437A0 File Offset: 0x000419A0
		public void RecordChangelogLoadTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalChangelogLoadingTimeMs += milliseconds;
				this._changelogLoadCount++;
			}
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000437F8 File Offset: 0x000419F8
		public void RecordDatabaseInfoLoadTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDatabaseInfoLoadingTimeMs += milliseconds;
				this._databaseInfoLoadCount++;
			}
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00043850 File Offset: 0x00041A50
		public void RecordDbCacheLoadTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbCacheLoadingTimeMs += milliseconds;
				this._dbCacheLoadCount++;
			}
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x000438A8 File Offset: 0x00041AA8
		public void RecordDbNetworkLoadTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbNetworkLoadingTimeMs += milliseconds;
				this._dbNetworkLoadCount++;
			}
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00043900 File Offset: 0x00041B00
		public void RecordDbApplyInfoTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbApplyInfoTimeMs += milliseconds;
				this._dbApplyInfoCount++;
			}
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00043958 File Offset: 0x00041B58
		public void RecordDbOfflineInfoTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbOfflineInfoTimeMs += milliseconds;
				this._dbOfflineInfoCount++;
			}
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000439B0 File Offset: 0x00041BB0
		public void RecordDbNetworkHttpTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbNetworkHttpTimeMs += milliseconds;
				this._dbNetworkHttpCount++;
			}
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00043A08 File Offset: 0x00041C08
		public void RecordDbNetworkParseTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbNetworkParseTimeMs += milliseconds;
				this._dbNetworkParseCount++;
			}
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00043A60 File Offset: 0x00041C60
		public void RecordDbNetworkExtractTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbNetworkExtractTimeMs += milliseconds;
				this._dbNetworkExtractCount++;
			}
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00043AB8 File Offset: 0x00041CB8
		public void RecordDbNetworkStoreTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbNetworkStoreTimeMs += milliseconds;
				this._dbNetworkStoreCount++;
			}
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00043B10 File Offset: 0x00041D10
		public void RecordDbApplyDispatcherTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbApplyDispatcherTimeMs += milliseconds;
				this._dbApplyDispatcherCount++;
			}
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00043B68 File Offset: 0x00041D68
		public void RecordDbApplyEntryUpdateTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbApplyEntryUpdateTimeMs += milliseconds;
				this._dbApplyEntryUpdateCount++;
			}
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00043BC0 File Offset: 0x00041DC0
		public void RecordDbApplyViewModelUpdateTime(double milliseconds)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbApplyViewModelUpdateTimeMs += milliseconds;
				this._dbApplyViewModelUpdateCount++;
			}
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00043C18 File Offset: 0x00041E18
		public void RecordDbApplyUiHandlerTime(double milliseconds, int updatesApplied)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._totalDbApplyUiHandlerTimeMs += milliseconds;
				this._dbApplyUiHandlerCount++;
				this._dbApplyUiHandlerUpdates += Math.Max(0, updatesApplied);
				this._maxDbApplyUiHandlerTimeMs = Math.Max(this._maxDbApplyUiHandlerTimeMs, milliseconds);
				if (milliseconds >= 50.0)
				{
					this._dbApplyUiHandlerSlowCount++;
				}
				if (updatesApplied <= 2)
				{
					this._dbApplyUiHandlerSmallBatchCount++;
				}
			}
			if (milliseconds < 50.0)
			{
			}
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00043CD0 File Offset: 0x00041ED0
		public string GetTimingSummary()
		{
			object @lock = this._lock;
			string result;
			lock (@lock)
			{
				List<string> list = new List<string>();
				list.Add("=== Mod Loading Performance Metrics ===");
				list.Add("");
				list.Add(ModLoadingTimingService.FormatMetric("Icon Loading", this._totalIconLoadingTimeMs, this._iconLoadCount));
				list.Add(ModLoadingTimingService.FormatMetric("Tags Loading", this._totalTagsLoadingTimeMs, this._tagsLoadCount));
				list.Add(ModLoadingTimingService.FormatMetric("User Reports/Votes Loading", this._totalUserReportsLoadingTimeMs, this._userReportsLoadCount));
				list.Add(ModLoadingTimingService.FormatMetric("Dependency/Error/Warning Checks", this._totalDependencyChecksTimeMs, this._dependencyCheckCount));
				list.Add(ModLoadingTimingService.FormatMetric("Update Checks", this._totalUpdateCheckTimeMs, this._updateCheckCount));
				list.Add(ModLoadingTimingService.FormatMetric("Changelog Loading", this._totalChangelogLoadingTimeMs, this._changelogLoadCount));
				list.Add(ModLoadingTimingService.FormatMetric("Database Info Loading", this._totalDatabaseInfoLoadingTimeMs, this._databaseInfoLoadCount));
				List<string> list2 = list;
				if (this._dbCacheLoadCount > 0 || this._dbNetworkLoadCount > 0 || this._dbApplyInfoCount > 0 || this._dbOfflineInfoCount > 0)
				{
					list2.Add("");
					list2.Add("  Database Info Loading Breakdown:");
					list2.Add("  " + ModLoadingTimingService.FormatMetric("Cache Loading", this._totalDbCacheLoadingTimeMs, this._dbCacheLoadCount));
					list2.Add("  " + ModLoadingTimingService.FormatMetric("Network Loading", this._totalDbNetworkLoadingTimeMs, this._dbNetworkLoadCount));
					if (this._dbNetworkHttpCount > 0 || this._dbNetworkParseCount > 0 || this._dbNetworkExtractCount > 0 || this._dbNetworkStoreCount > 0)
					{
						list2.Add("");
						list2.Add("    Network Loading Breakdown:");
						list2.Add("    " + ModLoadingTimingService.FormatMetric("HTTP Request/Response", this._totalDbNetworkHttpTimeMs, this._dbNetworkHttpCount));
						list2.Add("    " + ModLoadingTimingService.FormatMetric("JSON Parsing", this._totalDbNetworkParseTimeMs, this._dbNetworkParseCount));
						list2.Add("    " + ModLoadingTimingService.FormatMetric("Data Extraction", this._totalDbNetworkExtractTimeMs, this._dbNetworkExtractCount));
						list2.Add("    " + ModLoadingTimingService.FormatMetric("Cache Storage", this._totalDbNetworkStoreTimeMs, this._dbNetworkStoreCount));
					}
					list2.Add("  " + ModLoadingTimingService.FormatMetric("Applying Info", this._totalDbApplyInfoTimeMs, this._dbApplyInfoCount));
					if (this._dbApplyDispatcherCount > 0 || this._dbApplyEntryUpdateCount > 0 || this._dbApplyViewModelUpdateCount > 0 || this._dbApplyUiHandlerCount > 0)
					{
						list2.Add("");
						list2.Add("    Applying Info Breakdown:");
						list2.Add("    " + ModLoadingTimingService.FormatMetric("Dispatcher Wait", this._totalDbApplyDispatcherTimeMs, this._dbApplyDispatcherCount));
						list2.Add("    " + ModLoadingTimingService.FormatMetric("Entry Update", this._totalDbApplyEntryUpdateTimeMs, this._dbApplyEntryUpdateCount));
						list2.Add("    " + ModLoadingTimingService.FormatMetric("ViewModel Update", this._totalDbApplyViewModelUpdateTimeMs, this._dbApplyViewModelUpdateCount));
						list2.Add("    " + this.FormatUiHandlerMetric());
					}
					list2.Add("  " + ModLoadingTimingService.FormatMetric("Offline Info Population", this._totalDbOfflineInfoTimeMs, this._dbOfflineInfoCount));
				}
				list2.Add("");
				list2.Add("Total Time Across All Operations: " + ModLoadingTimingService.FormatTime(this._totalIconLoadingTimeMs + this._totalTagsLoadingTimeMs + this._totalUserReportsLoadingTimeMs + this._totalDependencyChecksTimeMs + this._totalUpdateCheckTimeMs + this._totalChangelogLoadingTimeMs + this._totalDatabaseInfoLoadingTimeMs));
				list2.Add("=======================================");
				result = string.Join(Environment.NewLine, list2);
			}
			return result;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x000440D0 File Offset: 0x000422D0
		private static string FormatMetric(string label, double totalMs, int count)
		{
			if (count == 0)
			{
				return label + ": No operations recorded";
			}
			double milliseconds = totalMs / (double)count;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(24, 4);
			defaultInterpolatedStringHandler.AppendFormatted(label);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(ModLoadingTimingService.FormatTime(totalMs));
			defaultInterpolatedStringHandler.AppendLiteral(" total (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(count);
			defaultInterpolatedStringHandler.AppendLiteral(" ops, avg ");
			defaultInterpolatedStringHandler.AppendFormatted(ModLoadingTimingService.FormatTime(milliseconds));
			defaultInterpolatedStringHandler.AppendLiteral("/op)");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0004415C File Offset: 0x0004235C
		private static string FormatTime(double milliseconds)
		{
			if (milliseconds < 1000.0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(2, 1);
				defaultInterpolatedStringHandler.AppendFormatted<double>(milliseconds, "F2");
				defaultInterpolatedStringHandler.AppendLiteral("ms");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			double num = milliseconds / 1000.0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(1, 1);
			defaultInterpolatedStringHandler2.AppendFormatted<double>(num, "F2");
			defaultInterpolatedStringHandler2.AppendLiteral("s");
			return defaultInterpolatedStringHandler2.ToStringAndClear();
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000441D4 File Offset: 0x000423D4
		private string FormatUiHandlerMetric()
		{
			if (this._dbApplyUiHandlerCount == 0)
			{
				return "UI Handler: No operations recorded";
			}
			string text = ModLoadingTimingService.FormatMetric("UI Handler", this._totalDbApplyUiHandlerTimeMs, this._dbApplyUiHandlerCount);
			string text2;
			if (this._dbApplyUiHandlerCount != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(17, 1);
				defaultInterpolatedStringHandler.AppendFormatted<double>((double)this._dbApplyUiHandlerUpdates / (double)this._dbApplyUiHandlerCount, "F1");
				defaultInterpolatedStringHandler.AppendLiteral(" updates/dispatch");
				text2 = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				text2 = "n/a";
			}
			string text3 = text2;
			string text4;
			if (this._dbApplyUiHandlerSlowCount <= 0)
			{
				text4 = string.Empty;
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(16, 1);
				defaultInterpolatedStringHandler2.AppendLiteral(", ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(this._dbApplyUiHandlerSlowCount);
				defaultInterpolatedStringHandler2.AppendLiteral(" slow (>=50ms)");
				text4 = defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			string text5 = text4;
			string text6;
			if (this._dbApplyUiHandlerSmallBatchCount <= 0)
			{
				text6 = string.Empty;
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
				defaultInterpolatedStringHandler3..ctor(30, 1);
				defaultInterpolatedStringHandler3.AppendLiteral(", ");
				defaultInterpolatedStringHandler3.AppendFormatted<int>(this._dbApplyUiHandlerSmallBatchCount);
				defaultInterpolatedStringHandler3.AppendLiteral(" small batches (<=2 updates)");
				text6 = defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			string text7 = text6;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
			defaultInterpolatedStringHandler4..ctor(13, 5);
			defaultInterpolatedStringHandler4.AppendFormatted(text);
			defaultInterpolatedStringHandler4.AppendLiteral(" (avg ");
			defaultInterpolatedStringHandler4.AppendFormatted(text3);
			defaultInterpolatedStringHandler4.AppendLiteral(", max ");
			defaultInterpolatedStringHandler4.AppendFormatted(ModLoadingTimingService.FormatTime(this._maxDbApplyUiHandlerTimeMs));
			defaultInterpolatedStringHandler4.AppendFormatted(text5);
			defaultInterpolatedStringHandler4.AppendFormatted(text7);
			defaultInterpolatedStringHandler4.AppendLiteral(")");
			return defaultInterpolatedStringHandler4.ToStringAndClear();
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00044343 File Offset: 0x00042543
		public IDisposable MeasureIconLoad()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordIconLoadTime));
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00044357 File Offset: 0x00042557
		public IDisposable MeasureTagsLoad()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordTagsLoadTime));
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0004436B File Offset: 0x0004256B
		public IDisposable MeasureUserReportsLoad()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordUserReportsLoadTime));
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0004437F File Offset: 0x0004257F
		public IDisposable MeasureDependencyCheck()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDependencyCheckTime));
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00044393 File Offset: 0x00042593
		public IDisposable MeasureUpdateCheck()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordUpdateCheckTime));
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x000443A7 File Offset: 0x000425A7
		public IDisposable MeasureChangelogLoad()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordChangelogLoadTime));
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x000443BB File Offset: 0x000425BB
		public IDisposable MeasureDatabaseInfoLoad()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDatabaseInfoLoadTime));
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x000443CF File Offset: 0x000425CF
		public IDisposable MeasureDbCacheLoad()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbCacheLoadTime));
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x000443E3 File Offset: 0x000425E3
		public IDisposable MeasureDbNetworkLoad()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbNetworkLoadTime));
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x000443F7 File Offset: 0x000425F7
		public IDisposable MeasureDbApplyInfo()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbApplyInfoTime));
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0004440B File Offset: 0x0004260B
		public IDisposable MeasureDbOfflineInfo()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbOfflineInfoTime));
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0004441F File Offset: 0x0004261F
		public IDisposable MeasureDbNetworkHttp()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbNetworkHttpTime));
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00044433 File Offset: 0x00042633
		public IDisposable MeasureDbNetworkParse()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbNetworkParseTime));
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00044447 File Offset: 0x00042647
		public IDisposable MeasureDbNetworkExtract()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbNetworkExtractTime));
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0004445B File Offset: 0x0004265B
		public IDisposable MeasureDbNetworkStore()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbNetworkStoreTime));
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0004446F File Offset: 0x0004266F
		public IDisposable MeasureDbApplyDispatcher()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbApplyDispatcherTime));
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00044483 File Offset: 0x00042683
		public IDisposable MeasureDbApplyEntryUpdate()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbApplyEntryUpdateTime));
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00044497 File Offset: 0x00042697
		public IDisposable MeasureDbApplyViewModelUpdate()
		{
			return new ModLoadingTimingService.TimingScope(this, new Action<double>(this.RecordDbApplyViewModelUpdateTime));
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x000444AB File Offset: 0x000426AB
		public IDisposable MeasureDbApplyUiHandler(int updatesApplied)
		{
			return new ModLoadingTimingService.CountedTimingScope(this, updatesApplied, new Action<double, int>(this.RecordDbApplyUiHandlerTime));
		}

		// Token: 0x040004DA RID: 1242
		private readonly object _lock = new object();

		// Token: 0x040004DB RID: 1243
		private double _totalIconLoadingTimeMs;

		// Token: 0x040004DC RID: 1244
		private double _totalTagsLoadingTimeMs;

		// Token: 0x040004DD RID: 1245
		private double _totalUserReportsLoadingTimeMs;

		// Token: 0x040004DE RID: 1246
		private double _totalDependencyChecksTimeMs;

		// Token: 0x040004DF RID: 1247
		private double _totalUpdateCheckTimeMs;

		// Token: 0x040004E0 RID: 1248
		private double _totalChangelogLoadingTimeMs;

		// Token: 0x040004E1 RID: 1249
		private double _totalDatabaseInfoLoadingTimeMs;

		// Token: 0x040004E2 RID: 1250
		private double _totalDbCacheLoadingTimeMs;

		// Token: 0x040004E3 RID: 1251
		private double _totalDbNetworkLoadingTimeMs;

		// Token: 0x040004E4 RID: 1252
		private double _totalDbApplyInfoTimeMs;

		// Token: 0x040004E5 RID: 1253
		private double _totalDbOfflineInfoTimeMs;

		// Token: 0x040004E6 RID: 1254
		private double _totalDbNetworkHttpTimeMs;

		// Token: 0x040004E7 RID: 1255
		private double _totalDbNetworkParseTimeMs;

		// Token: 0x040004E8 RID: 1256
		private double _totalDbNetworkExtractTimeMs;

		// Token: 0x040004E9 RID: 1257
		private double _totalDbNetworkStoreTimeMs;

		// Token: 0x040004EA RID: 1258
		private double _totalDbApplyDispatcherTimeMs;

		// Token: 0x040004EB RID: 1259
		private double _totalDbApplyEntryUpdateTimeMs;

		// Token: 0x040004EC RID: 1260
		private double _totalDbApplyViewModelUpdateTimeMs;

		// Token: 0x040004ED RID: 1261
		private double _totalDbApplyUiHandlerTimeMs;

		// Token: 0x040004EE RID: 1262
		private double _maxDbApplyUiHandlerTimeMs;

		// Token: 0x040004EF RID: 1263
		private int _iconLoadCount;

		// Token: 0x040004F0 RID: 1264
		private int _tagsLoadCount;

		// Token: 0x040004F1 RID: 1265
		private int _userReportsLoadCount;

		// Token: 0x040004F2 RID: 1266
		private int _dependencyCheckCount;

		// Token: 0x040004F3 RID: 1267
		private int _updateCheckCount;

		// Token: 0x040004F4 RID: 1268
		private int _changelogLoadCount;

		// Token: 0x040004F5 RID: 1269
		private int _databaseInfoLoadCount;

		// Token: 0x040004F6 RID: 1270
		private int _dbCacheLoadCount;

		// Token: 0x040004F7 RID: 1271
		private int _dbNetworkLoadCount;

		// Token: 0x040004F8 RID: 1272
		private int _dbApplyInfoCount;

		// Token: 0x040004F9 RID: 1273
		private int _dbOfflineInfoCount;

		// Token: 0x040004FA RID: 1274
		private int _dbNetworkHttpCount;

		// Token: 0x040004FB RID: 1275
		private int _dbNetworkParseCount;

		// Token: 0x040004FC RID: 1276
		private int _dbNetworkExtractCount;

		// Token: 0x040004FD RID: 1277
		private int _dbNetworkStoreCount;

		// Token: 0x040004FE RID: 1278
		private int _dbApplyDispatcherCount;

		// Token: 0x040004FF RID: 1279
		private int _dbApplyEntryUpdateCount;

		// Token: 0x04000500 RID: 1280
		private int _dbApplyViewModelUpdateCount;

		// Token: 0x04000501 RID: 1281
		private int _dbApplyUiHandlerCount;

		// Token: 0x04000502 RID: 1282
		private int _dbApplyUiHandlerSlowCount;

		// Token: 0x04000503 RID: 1283
		private int _dbApplyUiHandlerUpdates;

		// Token: 0x04000504 RID: 1284
		private int _dbApplyUiHandlerSmallBatchCount;

		// Token: 0x020002DD RID: 733
		[Nullable(0)]
		private sealed class TimingScope : IDisposable
		{
			// Token: 0x06001844 RID: 6212 RVA: 0x0008C8A0 File Offset: 0x0008AAA0
			public TimingScope(ModLoadingTimingService service, Action<double> recordAction)
			{
				this._service = service;
				this._recordAction = recordAction;
				this._stopwatch = Stopwatch.StartNew();
			}

			// Token: 0x06001845 RID: 6213 RVA: 0x0008C8C4 File Offset: 0x0008AAC4
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				this._stopwatch.Stop();
				this._recordAction.Invoke(this._stopwatch.Elapsed.TotalMilliseconds);
			}

			// Token: 0x0400123D RID: 4669
			private readonly ModLoadingTimingService _service;

			// Token: 0x0400123E RID: 4670
			private readonly Action<double> _recordAction;

			// Token: 0x0400123F RID: 4671
			private readonly Stopwatch _stopwatch;

			// Token: 0x04001240 RID: 4672
			private bool _disposed;
		}

		// Token: 0x020002DE RID: 734
		[Nullable(0)]
		private sealed class CountedTimingScope : IDisposable
		{
			// Token: 0x06001846 RID: 6214 RVA: 0x0008C90A File Offset: 0x0008AB0A
			public CountedTimingScope(ModLoadingTimingService service, int updatesApplied, Action<double, int> recordAction)
			{
				this._service = service;
				this._updatesApplied = updatesApplied;
				this._recordAction = recordAction;
				this._stopwatch = Stopwatch.StartNew();
			}

			// Token: 0x06001847 RID: 6215 RVA: 0x0008C934 File Offset: 0x0008AB34
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				this._stopwatch.Stop();
				this._recordAction.Invoke(this._stopwatch.Elapsed.TotalMilliseconds, this._updatesApplied);
			}

			// Token: 0x04001241 RID: 4673
			private readonly ModLoadingTimingService _service;

			// Token: 0x04001242 RID: 4674
			private readonly Action<double, int> _recordAction;

			// Token: 0x04001243 RID: 4675
			private readonly int _updatesApplied;

			// Token: 0x04001244 RID: 4676
			private readonly Stopwatch _stopwatch;

			// Token: 0x04001245 RID: 4677
			private bool _disposed;
		}
	}
}
