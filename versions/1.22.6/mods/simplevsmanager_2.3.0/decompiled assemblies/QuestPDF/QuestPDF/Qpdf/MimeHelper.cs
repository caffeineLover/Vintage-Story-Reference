using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace QuestPDF.Qpdf
{
	// Token: 0x02000032 RID: 50
	[NullableContext(1)]
	[Nullable(0)]
	internal static class MimeHelper
	{
		// Token: 0x06000137 RID: 311 RVA: 0x000044C0 File Offset: 0x000026C0
		private static IReadOnlyDictionary<string, string> LoadMimeMapping()
		{
			IReadOnlyDictionary<string, string> result;
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("QuestPDF.Resources.MimeTypes.csv"))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					result = Enumerable.ToDictionary<string[], string, string>(Enumerable.Select<string, string[]>(streamReader.ReadToEnd().Split('\n', 0), (string x) => x.Split(',', 0)), (string[] x) => Enumerable.First<string>(x), (string[] x) => Enumerable.Last<string>(x));
				}
			}
			return result;
		}

		// Token: 0x040000B5 RID: 181
		public static readonly IReadOnlyDictionary<string, string> FileExtensionToMimeConversionTable = MimeHelper.LoadMimeMapping();
	}
}
