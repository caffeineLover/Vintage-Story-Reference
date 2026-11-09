using System;
using System.Collections.Generic;
using System.Linq;

namespace System
{
	// Token: 0x02000004 RID: 4
	public static class TimeExtensions
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000212C File Offset: 0x0000032C
		public static string ToFFmpeg(this TimeSpan ts)
		{
			int milliseconds = ts.Milliseconds;
			int seconds = ts.Seconds;
			int minutes = ts.Minutes;
			int num = (int)ts.TotalHours;
			return string.Format("{0:D}:{1:D2}:{2:D2}.{3:D3}", new object[]
			{
				num,
				minutes,
				seconds,
				milliseconds
			});
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002190 File Offset: 0x00000390
		public static TimeSpan ParseFFmpegTime(this string text)
		{
			List<string> list = Enumerable.ToList<string>(Enumerable.Reverse<string>(text.Split(new char[]
			{
				':'
			})));
			int num = 0;
			int num2;
			if (Enumerable.Contains<char>(list[0], '.'))
			{
				string[] array = list[0].Split(new char[]
				{
					'.'
				});
				num2 = int.Parse(array[0]);
				num = int.Parse(array[1]);
			}
			else
			{
				num2 = int.Parse(list[0]);
			}
			int num3 = int.Parse(list[1]);
			int num4 = int.Parse(list[2]);
			return new TimeSpan(0, num4, num3, num2, num);
		}
	}
}
