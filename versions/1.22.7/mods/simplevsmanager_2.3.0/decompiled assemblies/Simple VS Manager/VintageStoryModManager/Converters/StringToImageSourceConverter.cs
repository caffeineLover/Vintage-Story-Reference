using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace VintageStoryModManager.Converters
{
	// Token: 0x020000D5 RID: 213
	[NullableContext(1)]
	[Nullable(0)]
	public class StringToImageSourceConverter : IValueConverter
	{
		// Token: 0x06001064 RID: 4196 RVA: 0x000572F0 File Offset: 0x000554F0
		[return: Nullable(2)]
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (text == null || string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			StringToImageSourceConverter.CleanupCacheIfNeeded();
			WeakReference<BitmapImage> weakReference;
			BitmapImage result;
			if (StringToImageSourceConverter._imageCache.TryGetValue(text, ref weakReference) && weakReference.TryGetTarget(ref result))
			{
				return result;
			}
			object result2;
			try
			{
				Uri uriSource = new Uri(text, 0);
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.UriSource = uriSource;
				bitmapImage.CacheOption = 1;
				bitmapImage.CreateOptions = 4;
				bitmapImage.EndInit();
				if (bitmapImage.CanFreeze)
				{
					bitmapImage.Freeze();
				}
				StringToImageSourceConverter._imageCache[text] = new WeakReference<BitmapImage>(bitmapImage);
				result2 = bitmapImage;
			}
			catch (Exception)
			{
				result2 = null;
			}
			return result2;
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000573A8 File Offset: 0x000555A8
		private static void CleanupCacheIfNeeded()
		{
			if (!Monitor.TryEnter(StringToImageSourceConverter._cleanupLock))
			{
				return;
			}
			try
			{
				DateTime utcNow = DateTime.UtcNow;
				if ((utcNow - StringToImageSourceConverter._lastCleanup).TotalMinutes >= 5.0)
				{
					StringToImageSourceConverter._lastCleanup = utcNow;
					WeakReference<BitmapImage> weakReference;
					foreach (KeyValuePair<string, WeakReference<BitmapImage>> keyValuePair in StringToImageSourceConverter._imageCache.ToArray())
					{
						BitmapImage bitmapImage;
						if (!keyValuePair.Value.TryGetTarget(ref bitmapImage))
						{
							StringToImageSourceConverter._imageCache.TryRemove(keyValuePair.Key, ref weakReference);
						}
					}
					if (StringToImageSourceConverter._imageCache.Count > 500)
					{
						string[] array2 = Enumerable.ToArray<string>(StringToImageSourceConverter._imageCache.Keys);
						int num = array2.Length / 2;
						int num2 = 0;
						while (num2 < num && num2 < array2.Length)
						{
							StringToImageSourceConverter._imageCache.TryRemove(array2[num2], ref weakReference);
							num2++;
						}
					}
				}
			}
			finally
			{
				Monitor.Exit(StringToImageSourceConverter._cleanupLock);
			}
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000574AC File Offset: 0x000556AC
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000720 RID: 1824
		private static readonly ConcurrentDictionary<string, WeakReference<BitmapImage>> _imageCache = new ConcurrentDictionary<string, WeakReference<BitmapImage>>();

		// Token: 0x04000721 RID: 1825
		private static readonly object _cleanupLock = new object();

		// Token: 0x04000722 RID: 1826
		private static DateTime _lastCleanup = DateTime.UtcNow;

		// Token: 0x04000723 RID: 1827
		private const int MaxCacheSize = 500;

		// Token: 0x04000724 RID: 1828
		private const int CleanupIntervalMinutes = 5;
	}
}
