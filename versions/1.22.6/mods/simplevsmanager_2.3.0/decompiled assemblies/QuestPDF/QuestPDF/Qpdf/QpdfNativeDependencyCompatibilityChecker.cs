using System;
using System.Runtime.CompilerServices;
using QuestPDF.Helpers;

namespace QuestPDF.Qpdf
{
	// Token: 0x02000034 RID: 52
	[NullableContext(1)]
	[Nullable(0)]
	internal static class QpdfNativeDependencyCompatibilityChecker
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000468E File Offset: 0x0000288E
		private static NativeDependencyCompatibilityChecker Instance { get; } = new NativeDependencyCompatibilityChecker
		{
			ExecuteNativeCode = new Action(QpdfNativeDependencyCompatibilityChecker.ExecuteNativeCode),
			ExceptionHint = new Func<string>(QpdfNativeDependencyCompatibilityChecker.GetHint)
		};

		// Token: 0x0600013E RID: 318 RVA: 0x00004695 File Offset: 0x00002895
		public static void Test()
		{
			QpdfNativeDependencyCompatibilityChecker.Instance.Test();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000046A1 File Offset: 0x000028A1
		private static void ExecuteNativeCode()
		{
			if (string.IsNullOrEmpty(QpdfAPI.GetQpdfVersion()))
			{
				throw new Exception();
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000046B5 File Offset: 0x000028B5
		private static string GetHint()
		{
			return "Please do NOT install the qpdf package.";
		}
	}
}
