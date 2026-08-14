using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.PdfFonts.Parser;

namespace UglyToad.PdfPig.PdfFonts.Cmap
{
	// Token: 0x020000B4 RID: 180
	internal static class CMapCache
	{
		// Token: 0x06000348 RID: 840 RVA: 0x0001517C File Offset: 0x0001337C
		public static bool TryGet(string name, out CMap result)
		{
			result = null;
			object @lock = CMapCache.Lock;
			bool result2;
			lock (@lock)
			{
				if (CMapCache.Cache.TryGetValue(name, ref result))
				{
					result2 = true;
				}
				else if (CMapCache.CMapParser.TryParseExternal(name, out result))
				{
					CMapCache.Cache[name] = result;
					result2 = true;
				}
				else
				{
					result2 = false;
				}
			}
			return result2;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000151EC File Offset: 0x000133EC
		public static CMap Parse(IInputBytes bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			return CMapCache.CMapParser.Parse(bytes);
		}

		// Token: 0x040001BE RID: 446
		private static readonly Dictionary<string, CMap> Cache = new Dictionary<string, CMap>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040001BF RID: 447
		private static readonly object Lock = new object();

		// Token: 0x040001C0 RID: 448
		private static readonly CMapParser CMapParser = new CMapParser();
	}
}
