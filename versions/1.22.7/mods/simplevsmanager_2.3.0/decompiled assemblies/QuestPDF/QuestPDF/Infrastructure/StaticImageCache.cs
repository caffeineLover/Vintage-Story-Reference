using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Helpers;
using QuestPDF.Skia;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000064 RID: 100
	[NullableContext(1)]
	[Nullable(0)]
	internal static class StaticImageCache
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000261 RID: 609 RVA: 0x00005A9F File Offset: 0x00003C9F
		// (set) Token: 0x06000262 RID: 610 RVA: 0x00005AA6 File Offset: 0x00003CA6
		private static bool CacheIsEnabled { get; set; } = true;

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00005AAE File Offset: 0x00003CAE
		// (set) Token: 0x06000264 RID: 612 RVA: 0x00005AB5 File Offset: 0x00003CB5
		private static ConcurrentDictionary<string, Image> Items { get; set; } = new ConcurrentDictionary<string, Image>();

		// Token: 0x06000265 RID: 613 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public static Image LoadFromCache(string filePath)
		{
			filePath = StaticImageCache.AdjustPath(filePath);
			Image result;
			if (StaticImageCache.Items.TryGetValue(filePath, ref result))
			{
				return result;
			}
			if (new FileInfo(filePath).Length > 1000000L)
			{
				return StaticImageCache.DirectlyLoadFromFile(filePath, false);
			}
			if (!StaticImageCache.CacheIsEnabled)
			{
				return StaticImageCache.DirectlyLoadFromFile(filePath, false);
			}
			Image image = StaticImageCache.DirectlyLoadFromFile(filePath, true);
			StaticImageCache.Items.TryAdd(filePath, image);
			StaticImageCache.CacheIsEnabled = (Enumerable.Sum<Image>(StaticImageCache.Items.Values, (Image x) => x.SkImage.EncodedDataSize) < 25000000);
			return image;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00005B60 File Offset: 0x00003D60
		public static string AdjustPath(string filePath)
		{
			if (File.Exists(filePath))
			{
				return filePath;
			}
			if (Path.IsPathRooted(filePath))
			{
				throw new DocumentComposeException("Cannot load an image under the provided absolute path, file not found: " + filePath);
			}
			string text = Path.Combine(Helpers.ApplicationFilesPath, filePath);
			if (!File.Exists(text))
			{
				throw new DocumentComposeException("Cannot load an image under the provided relative path, file not found: " + filePath);
			}
			return text;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00005BB4 File Offset: 0x00003DB4
		public static Image DirectlyLoadFromFile(string filePath, bool isShared)
		{
			filePath = StaticImageCache.AdjustPath(filePath);
			Image result;
			using (SkData skData = SkData.FromFile(filePath))
			{
				result = StaticImageCache.DecodeImage(skData, isShared);
			}
			return result;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00005BF8 File Offset: 0x00003DF8
		public static Image DecodeImage(SkData imageData, bool isShared)
		{
			Image result;
			try
			{
				result = new Image(SkImage.FromData(imageData))
				{
					IsShared = isShared
				};
			}
			catch
			{
				throw new DocumentComposeException("Cannot decode the provided image.");
			}
			return result;
		}

		// Token: 0x04000144 RID: 324
		private const int MaxCacheSize = 25000000;

		// Token: 0x04000145 RID: 325
		private const int MaxItemSize = 1000000;
	}
}
