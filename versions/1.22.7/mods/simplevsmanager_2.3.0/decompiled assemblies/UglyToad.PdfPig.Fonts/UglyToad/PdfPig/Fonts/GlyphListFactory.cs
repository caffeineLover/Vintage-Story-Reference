using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace UglyToad.PdfPig.Fonts
{
	// Token: 0x02000006 RID: 6
	internal class GlyphListFactory
	{
		// Token: 0x06000025 RID: 37 RVA: 0x000026B4 File Offset: 0x000008B4
		public static GlyphList Get(string listName)
		{
			GlyphList result;
			using (Stream manifestResourceStream = typeof(GlyphListFactory).Assembly.GetManifestResourceStream("UglyToad.PdfPig.Fonts.Resources.GlyphList." + listName))
			{
				if (manifestResourceStream == null)
				{
					throw new ArgumentException("No embedded glyph list resource was found with the name " + listName + ".");
				}
				int? defaultDictionaryCapacity = default(int?);
				if (string.Equals("glyphlist", listName, 5))
				{
					defaultDictionaryCapacity = new int?(4300);
				}
				result = GlyphListFactory.ReadInternal(manifestResourceStream, defaultDictionaryCapacity);
			}
			return result;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002744 File Offset: 0x00000944
		public static GlyphList Read(Stream stream)
		{
			return GlyphListFactory.ReadInternal(stream, new int?(0));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002754 File Offset: 0x00000954
		private static GlyphList ReadInternal(Stream stream, int? defaultDictionaryCapacity = 0)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			Dictionary<string, string> dictionary = (defaultDictionaryCapacity != null) ? new Dictionary<string, string>(defaultDictionaryCapacity.Value) : new Dictionary<string, string>();
			using (StreamReader streamReader = new StreamReader(stream))
			{
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine();
					if (!string.IsNullOrWhiteSpace(text) && text.get_Chars(0) != '#')
					{
						string[] array = text.Split(new char[]
						{
							';'
						}, 1);
						if (array.Length != 2)
						{
							throw new InvalidOperationException("The line in the glyph list did not match the expected format. Line was: " + text);
						}
						string text2 = array[0];
						string[] array2 = array[1].Split(' ', 0);
						string text3 = string.Empty;
						string[] array3 = array2;
						for (int i = 0; i < array3.Length; i++)
						{
							int num = int.Parse(array3[i], 515, CultureInfo.InvariantCulture);
							text3 += char.ConvertFromUtf32(num);
						}
						dictionary[text2] = text3;
					}
				}
			}
			return new GlyphList(dictionary);
		}
	}
}
