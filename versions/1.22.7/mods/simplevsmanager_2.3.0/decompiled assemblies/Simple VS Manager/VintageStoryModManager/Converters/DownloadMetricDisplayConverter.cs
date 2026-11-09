using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000DB RID: 219
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DownloadMetricDisplayConverter : IMultiValueConverter
	{
		// Token: 0x06001078 RID: 4216 RVA: 0x00057860 File Offset: 0x00055A60
		[return: Nullable(2)]
		public object Convert(object[] values, Type targetType, [Nullable(2)] object parameter, CultureInfo culture)
		{
			if (values == null || values.Length < 5)
			{
				return Binding.DoNothing;
			}
			object obj = values[0];
			bool flag;
			bool flag2;
			if (obj is bool)
			{
				flag = (bool)obj;
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2 && flag;
			string text = values[1] as string;
			string text2 = values[2] as string;
			string text3 = values[3] as string;
			ModDatabaseAutoLoadMode? modDatabaseAutoLoadMode = values[4] as ModDatabaseAutoLoadMode?;
			string text4;
			if (flag3 && modDatabaseAutoLoadMode != null)
			{
				text4 = ((modDatabaseAutoLoadMode.Value == ModDatabaseAutoLoadMode.DownloadsLastTenDays) ? text3 : text2);
			}
			else
			{
				text4 = text;
			}
			if (string.IsNullOrWhiteSpace(text4) || text4 == "—")
			{
				return "—";
			}
			if (flag3 && modDatabaseAutoLoadMode != null)
			{
				ModDatabaseAutoLoadMode value = modDatabaseAutoLoadMode.Value;
				if (value == ModDatabaseAutoLoadMode.DownloadsLastThirtyDays)
				{
					return DownloadMetricDisplayConverter.FormatRecentMetric(text4, " (30 days)");
				}
				if (value == ModDatabaseAutoLoadMode.DownloadsLastTenDays)
				{
					return DownloadMetricDisplayConverter.FormatRecentMetric(text4, " (10 days)");
				}
			}
			return text4;
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0005793E File Offset: 0x00055B3E
		public object[] ConvertBack(object value, Type[] targetTypes, [Nullable(2)] object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00057948 File Offset: 0x00055B48
		private static string FormatRecentMetric(string value, string suffix)
		{
			string text = value.Trim();
			string text2 = text.StartsWith("≈", 4) ? text : ("≈" + text);
			if (text2.EndsWith(suffix, 4))
			{
				return text2;
			}
			return text2 + suffix;
		}
	}
}
