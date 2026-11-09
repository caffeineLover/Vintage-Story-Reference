using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000CB RID: 203
	public sealed class BooleanToVisibilityConverter : IValueConverter
	{
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x00056DF7 File Offset: 0x00054FF7
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x00056DFF File Offset: 0x00054FFF
		public bool IsInverted { get; set; }

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00056E08 File Offset: 0x00055008
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x00056E10 File Offset: 0x00055010
		public bool UseHidden { get; set; }

		// Token: 0x06001046 RID: 4166 RVA: 0x00056E1C File Offset: 0x0005501C
		[NullableContext(1)]
		public object Convert([Nullable(2)] object value, Type targetType, [Nullable(2)] object parameter, CultureInfo culture)
		{
			bool flag;
			bool flag2;
			if (value is bool)
			{
				flag = (bool)value;
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2 && flag;
			if (this.IsInverted)
			{
				flag3 = !flag3;
			}
			return flag3 ? 0 : (this.UseHidden ? 1 : 2);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00056E64 File Offset: 0x00055064
		[NullableContext(1)]
		public object ConvertBack([Nullable(2)] object value, Type targetType, [Nullable(2)] object parameter, CultureInfo culture)
		{
			if (value is Visibility)
			{
				Visibility visibility = (Visibility)value;
				bool flag = visibility == 0;
				if (this.IsInverted)
				{
					flag = !flag;
				}
				return flag;
			}
			return Binding.DoNothing;
		}
	}
}
