using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000072 RID: 114
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ModImageCacheService
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x000421DC File Offset: 0x000403DC
		[NullableContext(2)]
		public static byte[] TryGetCachedImage([Nullable(1)] string imageUrl, ModImageCacheDescriptor descriptor = null)
		{
			if (string.IsNullOrWhiteSpace(imageUrl))
			{
				return null;
			}
			string cachePath = ModImageCacheService.GetCachePath(imageUrl, descriptor);
			if (string.IsNullOrWhiteSpace(cachePath) || !File.Exists(cachePath))
			{
				return null;
			}
			byte[] result;
			try
			{
				result = File.ReadAllBytes(cachePath);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00042230 File Offset: 0x00040430
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public static Task<byte[]> TryGetCachedImageAsync(string imageUrl, CancellationToken cancellationToken, [Nullable(2)] ModImageCacheDescriptor descriptor = null)
		{
			ModImageCacheService.<TryGetCachedImageAsync>d__2 <TryGetCachedImageAsync>d__;
			<TryGetCachedImageAsync>d__.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			<TryGetCachedImageAsync>d__.imageUrl = imageUrl;
			<TryGetCachedImageAsync>d__.cancellationToken = cancellationToken;
			<TryGetCachedImageAsync>d__.descriptor = descriptor;
			<TryGetCachedImageAsync>d__.<>1__state = -1;
			<TryGetCachedImageAsync>d__.<>t__builder.Start<ModImageCacheService.<TryGetCachedImageAsync>d__2>(ref <TryGetCachedImageAsync>d__);
			return <TryGetCachedImageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00042284 File Offset: 0x00040484
		public static Task StoreImageAsync(string imageUrl, byte[] imageBytes, CancellationToken cancellationToken, [Nullable(2)] ModImageCacheDescriptor descriptor = null)
		{
			ModImageCacheService.<StoreImageAsync>d__3 <StoreImageAsync>d__;
			<StoreImageAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<StoreImageAsync>d__.imageUrl = imageUrl;
			<StoreImageAsync>d__.imageBytes = imageBytes;
			<StoreImageAsync>d__.cancellationToken = cancellationToken;
			<StoreImageAsync>d__.descriptor = descriptor;
			<StoreImageAsync>d__.<>1__state = -1;
			<StoreImageAsync>d__.<>t__builder.Start<ModImageCacheService.<StoreImageAsync>d__3>(ref <StoreImageAsync>d__);
			return <StoreImageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x000422E0 File Offset: 0x000404E0
		internal static void ClearCacheDirectory()
		{
			string modDatabaseImageCacheDirectory = ModCacheLocator.GetModDatabaseImageCacheDirectory();
			if (string.IsNullOrWhiteSpace(modDatabaseImageCacheDirectory) || !Directory.Exists(modDatabaseImageCacheDirectory))
			{
				return;
			}
			try
			{
				Directory.Delete(modDatabaseImageCacheDirectory, true);
			}
			catch (Exception ex)
			{
				throw new IOException("Failed to delete the mod image cache at " + modDatabaseImageCacheDirectory + ".", ex);
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00042338 File Offset: 0x00040538
		[NullableContext(2)]
		private static string GetCachePath([Nullable(1)] string imageUrl, ModImageCacheDescriptor descriptor)
		{
			string modDatabaseImageCacheDirectory = ModCacheLocator.GetModDatabaseImageCacheDirectory();
			if (string.IsNullOrWhiteSpace(modDatabaseImageCacheDirectory))
			{
				return null;
			}
			string text = ModImageCacheService.GenerateCacheFileName(imageUrl, descriptor);
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			return Path.Combine(modDatabaseImageCacheDirectory, text);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00042370 File Offset: 0x00040570
		private static string GenerateCacheFileName(string imageUrl, [Nullable(2)] ModImageCacheDescriptor descriptor)
		{
			string imageExtension = ModImageCacheService.GetImageExtension(imageUrl);
			if (descriptor == null)
			{
				string text = ModImageCacheService.ComputeUrlHash(imageUrl);
				if (!string.IsNullOrWhiteSpace(imageExtension))
				{
					return text + imageExtension;
				}
				return text;
			}
			else
			{
				List<string> list = new List<string>(3);
				string text2 = ModImageCacheService.NormalizeSegment(descriptor.ApiSource);
				if (!string.IsNullOrWhiteSpace(text2))
				{
					list.Add(text2);
				}
				string text3 = ModImageCacheService.NormalizeSegment(descriptor.ModId ?? descriptor.ModName);
				if (!string.IsNullOrWhiteSpace(text3))
				{
					list.Add(text3);
				}
				if (list.Count != 0)
				{
					return string.Join<string>('_', list) + imageExtension;
				}
				string text4 = ModImageCacheService.ComputeUrlHash(imageUrl);
				if (!string.IsNullOrWhiteSpace(imageExtension))
				{
					return text4 + imageExtension;
				}
				return text4;
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0004241C File Offset: 0x0004061C
		private static string NormalizeSegment([Nullable(2)] string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}
			string text = ModCacheLocator.SanitizeFileName(value, "image").Replace(' ', '_').Trim('_');
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text.ToLowerInvariant();
			}
			return string.Empty;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00042468 File Offset: 0x00040668
		private static string GetImageExtension(string imageUrl)
		{
			if (string.IsNullOrWhiteSpace(imageUrl))
			{
				return ".png";
			}
			try
			{
				ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(imageUrl);
				int num = MemoryExtensions.IndexOf<char>(readOnlySpan, '?');
				if (num >= 0)
				{
					readOnlySpan = readOnlySpan.Slice(0, num);
				}
				int num2 = MemoryExtensions.LastIndexOf<char>(readOnlySpan, '.');
				if (num2 < 0 || num2 == readOnlySpan.Length - 1)
				{
					return ".png";
				}
				ReadOnlySpan<char> readOnlySpan2 = readOnlySpan.Slice(num2);
				if (MemoryExtensions.Equals(readOnlySpan2, ".png", 5) || MemoryExtensions.Equals(readOnlySpan2, ".jpg", 5) || MemoryExtensions.Equals(readOnlySpan2, ".jpeg", 5) || MemoryExtensions.Equals(readOnlySpan2, ".gif", 5) || MemoryExtensions.Equals(readOnlySpan2, ".webp", 5) || MemoryExtensions.Equals(readOnlySpan2, ".bmp", 5))
				{
					return readOnlySpan2.ToString().ToLowerInvariant();
				}
			}
			catch (Exception)
			{
			}
			return ".png";
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00042578 File Offset: 0x00040778
		private unsafe static string ComputeUrlHash(string url)
		{
			int maxByteCount = Encoding.UTF8.GetMaxByteCount(url.Length);
			int num;
			Span<byte> span;
			if (maxByteCount <= 1024)
			{
				num = maxByteCount;
				span = new Span<byte>(stackalloc byte[(UIntPtr)num], num);
			}
			else
			{
				span = new byte[maxByteCount];
			}
			Span<byte> span2 = span;
			int bytes = Encoding.UTF8.GetBytes(MemoryExtensions.AsSpan(url), span2);
			Span<byte> span3 = new Span<byte>(stackalloc byte[(UIntPtr)32], 32);
			SHA256.HashData(span2.Slice(0, bytes), span3);
			Span<char> span4 = new Span<char>(stackalloc byte[(UIntPtr)88], 44);
			int num2;
			if (!Convert.TryToBase64Chars(span3, span4, ref num2, 0) || num2 == 0)
			{
				return Convert.ToBase64String(span3, 0).Replace('+', '-').Replace('/', '_').TrimEnd('=').Substring(0, 32);
			}
			int num3 = Math.Min(32, num2);
			num = num3;
			Span<char> span5 = new Span<char>(stackalloc byte[checked(unchecked((UIntPtr)num) * 2)], num);
			for (int i = 0; i < num3; i++)
			{
				char c = *span4[i];
				num = i;
				char c2;
				if (c != '+')
				{
					if (c != '/')
					{
						if (c != '=')
						{
							c2 = c;
						}
						else
						{
							c2 = '_';
						}
					}
					else
					{
						c2 = '_';
					}
				}
				else
				{
					c2 = '-';
				}
				*span5[num] = c2;
			}
			return span5.ToString();
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x000426CC File Offset: 0x000408CC
		private static Task<SemaphoreSlim> AcquireLockAsync(string path, CancellationToken cancellationToken)
		{
			ModImageCacheService.<AcquireLockAsync>d__10 <AcquireLockAsync>d__;
			<AcquireLockAsync>d__.<>t__builder = AsyncTaskMethodBuilder<SemaphoreSlim>.Create();
			<AcquireLockAsync>d__.path = path;
			<AcquireLockAsync>d__.cancellationToken = cancellationToken;
			<AcquireLockAsync>d__.<>1__state = -1;
			<AcquireLockAsync>d__.<>t__builder.Start<ModImageCacheService.<AcquireLockAsync>d__10>(ref <AcquireLockAsync>d__);
			return <AcquireLockAsync>d__.<>t__builder.Task;
		}

		// Token: 0x040004D4 RID: 1236
		private static readonly ConcurrentDictionary<string, SemaphoreSlim> FileLocks = new ConcurrentDictionary<string, SemaphoreSlim>(StringComparer.OrdinalIgnoreCase);
	}
}
