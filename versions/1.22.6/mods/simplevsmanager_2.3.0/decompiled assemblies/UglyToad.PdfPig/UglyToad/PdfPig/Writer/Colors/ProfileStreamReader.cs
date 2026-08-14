using System;
using System.IO;
using System.Linq;
using UglyToad.PdfPig.PdfFonts.Parser;

namespace UglyToad.PdfPig.Writer.Colors
{
	// Token: 0x02000024 RID: 36
	internal static class ProfileStreamReader
	{
		// Token: 0x06000111 RID: 273 RVA: 0x0000A508 File Offset: 0x00008708
		public static byte[] GetSRgb2014()
		{
			string text = Enumerable.FirstOrDefault<string>(typeof(ProfileStreamReader).Assembly.GetManifestResourceNames(), (string x) => x.EndsWith("sRGB2014.icc", 3));
			if (text == null)
			{
				throw new InvalidOperationException("Could not find the sRGB ICC color profile stream.");
			}
			byte[] result;
			using (Stream manifestResourceStream = typeof(CMapParser).Assembly.GetManifestResourceStream(text))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					if (manifestResourceStream != null)
					{
						manifestResourceStream.CopyTo(memoryStream);
					}
					result = memoryStream.ToArray();
				}
			}
			return result;
		}
	}
}
