using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000024 RID: 36
	[NullableContext(1)]
	[Nullable(0)]
	internal struct IOLibrary
	{
		// Token: 0x06000239 RID: 569 RVA: 0x0000E90C File Offset: 0x0000CB0C
		internal static void CopyAlways(string source, string target)
		{
			if (!File.Exists(source))
			{
				return;
			}
			Directory.CreateDirectory(Path.GetDirectoryName(target));
			IOLibrary.MakeWritable(target);
			File.Copy(source, target, true);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000E931 File Offset: 0x0000CB31
		internal static void MakeWritable(string path)
		{
			if (!File.Exists(path))
			{
				return;
			}
			File.SetAttributes(path, File.GetAttributes(path) & -2);
		}
	}
}
