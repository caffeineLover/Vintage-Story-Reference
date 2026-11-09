using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using QuestPDF.Helpers;

namespace QuestPDF.Skia
{
	// Token: 0x0200000F RID: 15
	[NullableContext(1)]
	[Nullable(0)]
	internal static class SkNativeDependencyCompatibilityChecker
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002BE0 File Offset: 0x00000DE0
		private static NativeDependencyCompatibilityChecker Instance { get; } = new NativeDependencyCompatibilityChecker
		{
			ExecuteNativeCode = new Action(SkNativeDependencyCompatibilityChecker.ExecuteNativeCode),
			CheckNativeLibraryVersion = new Func<bool>(SkNativeDependencyCompatibilityChecker.CheckNativeLibraryVersion)
		};

		// Token: 0x06000066 RID: 102 RVA: 0x00002BE7 File Offset: 0x00000DE7
		public static void Test()
		{
			SkNativeDependencyCompatibilityChecker.Instance.Test();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002BF4 File Offset: 0x00000DF4
		private static bool CheckNativeLibraryVersion()
		{
			bool result;
			try
			{
				result = (SkNativeDependencyCompatibilityChecker.API.get_questpdf_version() == 9);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002C24 File Offset: 0x00000E24
		private static void ExecuteNativeCode()
		{
			Random random = new Random();
			int num = random.Next();
			int num2 = random.Next();
			int num3 = num + num2;
			int num4 = SkNativeDependencyCompatibilityChecker.API.check_compatibility_by_calculating_sum(num, num2);
			if (num3 != num4)
			{
				throw new Exception();
			}
		}

		// Token: 0x0400002E RID: 46
		private const int ExpectedNativeLibraryVersion = 9;

		// Token: 0x02000137 RID: 311
		[NullableContext(0)]
		private static class API
		{
			// Token: 0x06000AFC RID: 2812
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern int get_questpdf_version();

			// Token: 0x06000AFD RID: 2813
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern int check_compatibility_by_calculating_sum(int a, int b);
		}
	}
}
