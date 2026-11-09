using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000083 RID: 131
	public static class ServerCommandBuilder
	{
		// Token: 0x06000B88 RID: 2952 RVA: 0x0004796C File Offset: 0x00045B6C
		[NullableContext(2)]
		public static string TryBuildInstallCommand(string modId, string version)
		{
			if (string.IsNullOrWhiteSpace(modId) || string.IsNullOrWhiteSpace(version))
			{
				return null;
			}
			string text = modId.Trim();
			string text2 = version.Trim();
			if (text.Length == 0 || text2.Length == 0)
			{
				return null;
			}
			return string.Format(CultureInfo.InvariantCulture, "/moddb install {0}@{1}", text, text2);
		}

		// Token: 0x04000545 RID: 1349
		[Nullable(1)]
		private const string CommandTemplate = "/moddb install {0}@{1}";
	}
}
