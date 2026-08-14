using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000092 RID: 146
	[NullableContext(2)]
	[Nullable(0)]
	internal static class VersionStringUtility
	{
		// Token: 0x06000D21 RID: 3361 RVA: 0x0004FFB0 File Offset: 0x0004E1B0
		public static string Normalize(string version)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				return null;
			}
			string result;
			if (VersionStringUtility.NormalizedVersionCache.TryGetValue(version, ref result))
			{
				return result;
			}
			List<string> list = new List<string>(4);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			for (int i = 0; i < version.Length; i++)
			{
				char c = version.get_Chars(i);
				if (char.IsDigit(c))
				{
					stringBuilder.Append(c);
					flag = true;
				}
				else
				{
					if (stringBuilder.Length > 0)
					{
						list.Add(stringBuilder.ToString());
						stringBuilder.Clear();
						if (list.Count >= 4)
						{
							break;
						}
					}
					if (char.IsWhiteSpace(c) && flag)
					{
						break;
					}
				}
			}
			if (stringBuilder.Length > 0 && list.Count < 4)
			{
				list.Add(stringBuilder.ToString());
			}
			string text = (list.Count == 0) ? null : string.Join<string>('.', list);
			VersionStringUtility.NormalizedVersionCache.TryAdd(version, text);
			return text;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00050094 File Offset: 0x0004E294
		public static bool IsCandidateVersionNewer(string candidateVersion, string currentVersion)
		{
			if (string.IsNullOrWhiteSpace(candidateVersion))
			{
				return false;
			}
			string text = VersionStringUtility.Normalize(candidateVersion);
			string text2 = VersionStringUtility.Normalize(currentVersion);
			if (text == null)
			{
				return !string.IsNullOrWhiteSpace(currentVersion) && !string.Equals(candidateVersion.Trim(), currentVersion.Trim(), 5);
			}
			if (text2 == null)
			{
				return true;
			}
			int[] array;
			int[] array2;
			if (VersionStringUtility.TryParseVersionParts(text, out array) && VersionStringUtility.TryParseVersionParts(text2, out array2))
			{
				int num = Math.Max(array.Length, array2.Length);
				for (int i = 0; i < num; i++)
				{
					int num2 = (i < array.Length) ? array[i] : 0;
					int num3 = (i < array2.Length) ? array2[i] : 0;
					if (num2 > num3)
					{
						return true;
					}
					if (num2 < num3)
					{
						return false;
					}
				}
				return false;
			}
			return !string.Equals(text, text2, 5);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00050150 File Offset: 0x0004E350
		public static bool MatchesVersionOrPrefix(string candidateVersion, string targetVersion)
		{
			if (string.IsNullOrWhiteSpace(candidateVersion) || string.IsNullOrWhiteSpace(targetVersion))
			{
				return false;
			}
			string text = VersionStringUtility.Normalize(candidateVersion);
			string text2 = VersionStringUtility.Normalize(targetVersion);
			if (text == null || text2 == null)
			{
				return false;
			}
			if (string.Equals(text, text2, 5))
			{
				return true;
			}
			int[] array;
			int[] array2;
			if (!VersionStringUtility.TryParseVersionParts(text, out array) || !VersionStringUtility.TryParseVersionParts(text2, out array2))
			{
				return false;
			}
			if (array.Length == 0 || array.Length > array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x000501D4 File Offset: 0x0004E3D4
		public static bool SatisfiesMinimumVersion(string requestedVersion, string providedVersion)
		{
			if (string.IsNullOrWhiteSpace(requestedVersion) || string.Equals(requestedVersion.Trim(), "*", 4))
			{
				return true;
			}
			string text = VersionStringUtility.Normalize(requestedVersion);
			if (text == null)
			{
				return true;
			}
			string text2 = VersionStringUtility.Normalize(providedVersion);
			if (text2 == null)
			{
				return false;
			}
			int[] array;
			if (!VersionStringUtility.TryParseVersionParts(text, out array))
			{
				return true;
			}
			int[] array2;
			if (!VersionStringUtility.TryParseVersionParts(text2, out array2))
			{
				return false;
			}
			int num = Math.Max(array.Length, array2.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = (i < array.Length) ? array[i] : 0;
				int num3 = (i < array2.Length) ? array2[i] : 0;
				if (num3 > num2)
				{
					return true;
				}
				if (num3 < num2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00050280 File Offset: 0x0004E480
		public static bool MatchesFirstThreeDigits(string version1, string version2)
		{
			if (string.IsNullOrWhiteSpace(version1) || string.IsNullOrWhiteSpace(version2))
			{
				return false;
			}
			string text = VersionStringUtility.Normalize(version1);
			string text2 = VersionStringUtility.Normalize(version2);
			if (text == null || text2 == null)
			{
				return false;
			}
			int[] array;
			int[] array2;
			if (!VersionStringUtility.TryParseVersionParts(text, out array) || !VersionStringUtility.TryParseVersionParts(text2, out array2))
			{
				return false;
			}
			if (array.Length < 3 || array2.Length < 3)
			{
				return false;
			}
			for (int i = 0; i < 3; i++)
			{
				if (array[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x000502F8 File Offset: 0x0004E4F8
		public static bool SupportsVersion(string candidateTag, string targetVersion, bool requireExactMatch)
		{
			if (string.IsNullOrWhiteSpace(candidateTag) || string.IsNullOrWhiteSpace(targetVersion))
			{
				return false;
			}
			string text = candidateTag.Trim();
			string text2 = targetVersion.Trim();
			if (string.Equals(text, text2, 5))
			{
				return true;
			}
			if (VersionStringUtility.MatchesVersionOrPrefix(text, text2))
			{
				return true;
			}
			if (requireExactMatch)
			{
				return false;
			}
			string text3 = VersionStringUtility.Normalize(text);
			string text4 = VersionStringUtility.Normalize(text2);
			int[] array;
			int[] array2;
			return text3 != null && text4 != null && VersionStringUtility.TryParseVersionParts(text3, out array) && VersionStringUtility.TryParseVersionParts(text4, out array2) && array.Length >= 2 && array2.Length >= 2 && array[0] == array2[0] && array[1] == array2[1];
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00050394 File Offset: 0x0004E594
		[NullableContext(1)]
		private static bool TryParseVersionParts(string normalizedVersion, out int[] parts)
		{
			int[] array;
			if (VersionStringUtility.ParsedVersionPartsCache.TryGetValue(normalizedVersion, ref array))
			{
				parts = ((array.Length == 0) ? Array.Empty<int>() : ((int[])array.Clone()));
				return array.Length != 0;
			}
			ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(normalizedVersion);
			List<int> list = new List<int>(4);
			while (!readOnlySpan.IsEmpty)
			{
				int num = MemoryExtensions.IndexOf<char>(readOnlySpan, '.');
				int num2;
				if (!int.TryParse((num >= 0) ? readOnlySpan.Slice(0, num) : readOnlySpan, ref num2))
				{
					parts = Array.Empty<int>();
					VersionStringUtility.ParsedVersionPartsCache.TryAdd(normalizedVersion, parts);
					return false;
				}
				list.Add(num2);
				if (num < 0)
				{
					break;
				}
				readOnlySpan = readOnlySpan.Slice(num + 1);
			}
			parts = list.ToArray();
			VersionStringUtility.ParsedVersionPartsCache.TryAdd(normalizedVersion, (int[])parts.Clone());
			return true;
		}

		// Token: 0x040005CC RID: 1484
		[Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		private static readonly ConcurrentDictionary<string, string> NormalizedVersionCache = new ConcurrentDictionary<string, string>();

		// Token: 0x040005CD RID: 1485
		[Nullable(1)]
		private static readonly ConcurrentDictionary<string, int[]> ParsedVersionPartsCache = new ConcurrentDictionary<string, int[]>();
	}
}
