using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200006C RID: 108
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ModConfigurationEncoding
	{
		// Token: 0x060009E6 RID: 2534 RVA: 0x0003C28C File Offset: 0x0003A48C
		[NullableContext(2)]
		public static string Encode(string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				return content;
			}
			string text = ModConfigurationEncoding.Decode(content);
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BrotliStream brotliStream = new BrotliStream(memoryStream, 3, true))
				{
					brotliStream.Write(bytes, 0, bytes.Length);
				}
				byte[] array = memoryStream.ToArray();
				if (array.Length + "br64:".Length < bytes.Length)
				{
					result = "br64:" + Convert.ToBase64String(array);
				}
				else
				{
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0003C348 File Offset: 0x0003A548
		public static string Decode([Nullable(2)] string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				return content ?? string.Empty;
			}
			if (!content.StartsWith("br64:", 4))
			{
				return content;
			}
			string result;
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(content.Substring("br64:".Length))))
				{
					using (BrotliStream brotliStream = new BrotliStream(memoryStream, 0))
					{
						using (StreamReader streamReader = new StreamReader(brotliStream, Encoding.UTF8))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch
			{
				result = content;
			}
			return result;
		}

		// Token: 0x040004A3 RID: 1187
		private const string BrotliPrefix = "br64:";
	}
}
