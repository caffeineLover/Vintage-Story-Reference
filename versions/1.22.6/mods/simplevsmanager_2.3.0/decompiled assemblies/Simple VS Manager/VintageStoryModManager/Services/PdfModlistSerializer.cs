using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000082 RID: 130
	[NullableContext(1)]
	[Nullable(0)]
	public static class PdfModlistSerializer
	{
		// Token: 0x06000B77 RID: 2935 RVA: 0x000473B8 File Offset: 0x000455B8
		public static string SerializeToBase64(SerializablePreset preset)
		{
			ArgumentNullException.ThrowIfNull(preset, "preset");
			string text = JsonSerializer.Serialize<SerializablePreset>(preset, PdfModlistSerializer.SerializationOptions);
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000473EC File Offset: 0x000455EC
		public static string SerializeConfigListToBase64(SerializableConfigList configList)
		{
			ArgumentNullException.ThrowIfNull(configList, "configList");
			string text = JsonSerializer.Serialize<SerializableConfigList>(configList, PdfModlistSerializer.SerializationOptions);
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00047420 File Offset: 0x00045620
		public static string CreateModlistMetadataValue(string encodedModlist)
		{
			ArgumentNullException.ThrowIfNull(encodedModlist, "encodedModlist");
			return "SVSM:MODLIST:" + encodedModlist;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00047438 File Offset: 0x00045638
		public static string CreateConfigMetadataValue([Nullable(2)] string encodedConfigList)
		{
			string text = string.IsNullOrWhiteSpace(encodedConfigList) ? "NONE" : encodedConfigList;
			return "SVSM:CONFIG:" + text;
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00047464 File Offset: 0x00045664
		[NullableContext(2)]
		public static bool TryDeserializeFromJson([Nullable(1)] string json, out SerializablePreset preset, out string errorMessage)
		{
			preset = null;
			errorMessage = null;
			if (string.IsNullOrWhiteSpace(json))
			{
				errorMessage = "The selected file was empty.";
				return false;
			}
			try
			{
				preset = JsonSerializer.Deserialize<SerializablePreset>(json, PdfModlistSerializer.DeserializationOptions);
			}
			catch (JsonException ex)
			{
				errorMessage = ex.Message;
				return false;
			}
			if (preset == null)
			{
				errorMessage = "The selected file was empty.";
				return false;
			}
			return true;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x000474C8 File Offset: 0x000456C8
		[NullableContext(2)]
		public static bool TryDeserializeConfigListFromJson([Nullable(1)] string json, out SerializableConfigList configList, out string errorMessage)
		{
			configList = null;
			errorMessage = null;
			if (string.IsNullOrWhiteSpace(json))
			{
				errorMessage = "The selected file was empty.";
				return false;
			}
			try
			{
				configList = JsonSerializer.Deserialize<SerializableConfigList>(json, PdfModlistSerializer.DeserializationOptions);
			}
			catch (JsonException ex)
			{
				errorMessage = ex.Message;
				return false;
			}
			if (configList == null)
			{
				errorMessage = "The selected file was empty.";
				return false;
			}
			return true;
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0004752C File Offset: 0x0004572C
		[NullableContext(2)]
		public static bool TryExtractModlistJson([Nullable(1)] string pdfText, out string json, out string errorMessage)
		{
			string normalized;
			if (!PdfModlistSerializer.TryExtractSection(pdfText, "###", true, "The PDF did not contain a modlist section.", "The PDF did not contain the end of the modlist section.", "The PDF did not contain any modlist data.", out normalized, out errorMessage))
			{
				json = null;
				return false;
			}
			return PdfModlistSerializer.TryConvertPayloadToJson(normalized, out json, out errorMessage, "The PDF modlist data was not in a recognized format.");
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0004756C File Offset: 0x0004576C
		[NullableContext(2)]
		public static bool TryExtractModlistJsonFromMetadata(string metadataValue, out string json, out string errorMessage)
		{
			json = null;
			errorMessage = null;
			if (string.IsNullOrWhiteSpace(metadataValue))
			{
				return false;
			}
			string text = metadataValue.Trim();
			if (!text.StartsWith("SVSM:MODLIST:", 4))
			{
				return false;
			}
			string text2 = text.Substring("SVSM:MODLIST:".Length);
			if (string.IsNullOrWhiteSpace(text2))
			{
				errorMessage = "The PDF modlist metadata did not contain any modlist data.";
				return false;
			}
			string text3 = PdfModlistSerializer.NormalizePayload(text2);
			if (string.IsNullOrWhiteSpace(text3))
			{
				errorMessage = "The PDF modlist metadata did not contain any modlist data.";
				return false;
			}
			return PdfModlistSerializer.TryConvertPayloadToJson(text3, out json, out errorMessage, "The PDF modlist metadata was not in a recognized format.");
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x000475E8 File Offset: 0x000457E8
		[NullableContext(2)]
		public static bool TryExtractConfigJson([Nullable(1)] string pdfText, out string json, out string errorMessage)
		{
			string text;
			if (!PdfModlistSerializer.TryExtractSection(pdfText, "@@@", false, "The PDF did not contain a configuration section.", "The PDF did not contain the end of the configuration section.", "The PDF did not contain any configuration data.", out text, out errorMessage))
			{
				json = null;
				return false;
			}
			if (text == null)
			{
				json = null;
				errorMessage = null;
				return true;
			}
			return PdfModlistSerializer.TryConvertPayloadToJson(text, out json, out errorMessage, "The PDF configuration data was not in a recognized format.");
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00047634 File Offset: 0x00045834
		[NullableContext(2)]
		public static bool TryExtractConfigJsonFromMetadata(string metadataValue, out string json, out string errorMessage)
		{
			json = null;
			errorMessage = null;
			if (string.IsNullOrWhiteSpace(metadataValue))
			{
				return false;
			}
			string text = metadataValue.Trim();
			if (!text.StartsWith("SVSM:CONFIG:", 4))
			{
				return false;
			}
			string text2 = text.Substring("SVSM:CONFIG:".Length);
			if (string.Equals(text2, "NONE", 4))
			{
				return true;
			}
			if (string.IsNullOrWhiteSpace(text2))
			{
				errorMessage = "The PDF configuration metadata did not contain any configuration data.";
				return false;
			}
			string text3 = PdfModlistSerializer.NormalizePayload(text2);
			if (string.IsNullOrWhiteSpace(text3))
			{
				errorMessage = "The PDF configuration metadata did not contain any configuration data.";
				return false;
			}
			return PdfModlistSerializer.TryConvertPayloadToJson(text3, out json, out errorMessage, "The PDF configuration metadata was not in a recognized format.");
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x000476C0 File Offset: 0x000458C0
		public static string NormalizePayload(string content)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return string.Empty;
			}
			string text = content.Replace("\r\n", "\n").Replace('\r', '\n').Replace("\0", string.Empty).Replace("\u00a0", " ");
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text2 in text.Split('\n', 0))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append('\n');
				}
				stringBuilder.Append(text2.TrimEnd());
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00047760 File Offset: 0x00045960
		private static bool TryExtractSection(string pdfText, string delimiter, bool required, string missingSectionMessage, string missingEndMessage, string missingDataMessage, [Nullable(2)] out string normalized, [Nullable(2)] out string errorMessage)
		{
			normalized = null;
			errorMessage = null;
			if (string.IsNullOrWhiteSpace(pdfText))
			{
				if (required)
				{
					errorMessage = missingDataMessage;
					return false;
				}
				return true;
			}
			else
			{
				int num = pdfText.IndexOf(delimiter, 4);
				if (num < 0)
				{
					if (required)
					{
						errorMessage = missingSectionMessage;
						return false;
					}
					return true;
				}
				else
				{
					num += delimiter.Length;
					int num2 = pdfText.IndexOf(delimiter, num, 4);
					if (num2 < 0)
					{
						errorMessage = missingEndMessage;
						return false;
					}
					if (num2 <= num)
					{
						errorMessage = missingDataMessage;
						return false;
					}
					string text = PdfModlistSerializer.NormalizePayload(pdfText.Substring(num, num2 - num));
					if (string.IsNullOrWhiteSpace(text))
					{
						errorMessage = missingDataMessage;
						return false;
					}
					normalized = text;
					return true;
				}
			}
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x000477F0 File Offset: 0x000459F0
		private static bool TryConvertPayloadToJson(string normalized, [Nullable(2)] out string json, [Nullable(2)] out string errorMessage, string formatErrorMessage)
		{
			json = null;
			errorMessage = null;
			if (PdfModlistSerializer.IsProbableJson(normalized))
			{
				json = normalized;
				return true;
			}
			string text;
			if (PdfModlistSerializer.TryDecodeBase64(normalized, out text))
			{
				json = text;
				return true;
			}
			errorMessage = formatErrorMessage;
			return false;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00047824 File Offset: 0x00045A24
		private static bool IsProbableJson(string content)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return false;
			}
			for (int i = 0; i < content.Length; i++)
			{
				char c = content.get_Chars(i);
				if (!char.IsWhiteSpace(c))
				{
					return c == '{' || c == '[';
				}
			}
			return false;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0004786C File Offset: 0x00045A6C
		private static bool TryDecodeBase64(string content, [Nullable(2)] out string decoded)
		{
			decoded = null;
			if (string.IsNullOrWhiteSpace(content))
			{
				return false;
			}
			string text = PdfModlistSerializer.RemoveWhitespace(content);
			if (text.Length == 0 || text.Length % 4 != 0)
			{
				return false;
			}
			Span<byte> span = new byte[text.Length];
			int num;
			if (!Convert.TryFromBase64String(text, span, ref num))
			{
				return false;
			}
			decoded = Encoding.UTF8.GetString(span.Slice(0, num));
			return !string.IsNullOrWhiteSpace(decoded);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x000478E4 File Offset: 0x00045AE4
		private static string RemoveWhitespace(string content)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(content.Length);
			for (int i = 0; i < content.Length; i++)
			{
				char c = content.get_Chars(i);
				if (!char.IsWhiteSpace(c))
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400053D RID: 1341
		private const string ModlistDelimiter = "###";

		// Token: 0x0400053E RID: 1342
		private const string ConfigDelimiter = "@@@";

		// Token: 0x0400053F RID: 1343
		private const string MetadataPrefix = "SVSM:";

		// Token: 0x04000540 RID: 1344
		private const string ModlistMetadataPrefix = "SVSM:MODLIST:";

		// Token: 0x04000541 RID: 1345
		private const string ConfigMetadataPrefix = "SVSM:CONFIG:";

		// Token: 0x04000542 RID: 1346
		private const string ConfigMetadataMissingValue = "NONE";

		// Token: 0x04000543 RID: 1347
		private static readonly JsonSerializerOptions SerializationOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = 3
		};

		// Token: 0x04000544 RID: 1348
		private static readonly JsonSerializerOptions DeserializationOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
	}
}
