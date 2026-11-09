using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace QuestPDF.Qpdf
{
	// Token: 0x02000033 RID: 51
	[NullableContext(1)]
	[Nullable(0)]
	internal static class QpdfAPI
	{
		// Token: 0x06000139 RID: 313 RVA: 0x0000459C File Offset: 0x0000279C
		[NullableContext(2)]
		public static string GetQpdfVersion()
		{
			return Marshal.PtrToStringAnsi(QpdfAPI.API.qpdf_get_qpdf_version());
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000045A8 File Offset: 0x000027A8
		public static void ExecuteJob(string jobJson)
		{
			QpdfNativeDependencyCompatibilityChecker.Test();
			StringBuilder stringBuilder = new StringBuilder();
			GCHandle gchandle = GCHandle.Alloc(stringBuilder);
			IntPtr udata = GCHandle.ToIntPtr(gchandle);
			IntPtr loggerHandle = QpdfAPI.API.qpdflogger_create();
			QpdfAPI.API.qpdflogger_set_error(loggerHandle, 4, QpdfAPI.LoggingCallbackPointer, udata);
			IntPtr jobHandle = QpdfAPI.API.qpdfjob_init();
			QpdfAPI.API.qpdfjob_set_logger(jobHandle, loggerHandle);
			QpdfAPI.API.qpdfjob_initialize_from_json(jobHandle, jobJson);
			QpdfAPI.API.qpdfjob_run(jobHandle);
			QpdfAPI.API.qpdfjob_cleanup(jobHandle);
			QpdfAPI.API.qpdflogger_cleanup(loggerHandle);
			string text = stringBuilder.ToString();
			gchandle.Free();
			if (!string.IsNullOrEmpty(text))
			{
				throw new Exception(text);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004624 File Offset: 0x00002824
		private static int LoggingCallback(IntPtr data, int length, IntPtr udata)
		{
			byte[] array = new byte[length];
			Marshal.Copy(data, array, 0, length);
			StringBuilder stringBuilder = (StringBuilder)GCHandle.FromIntPtr(udata).Target;
			if (stringBuilder != null)
			{
				stringBuilder.Append(Encoding.ASCII.GetString(array));
			}
			return 0;
		}

		// Token: 0x040000B6 RID: 182
		private static readonly QpdfAPI.CallbackDelegate LoggingCallbackDelegate = new QpdfAPI.CallbackDelegate(QpdfAPI.LoggingCallback);

		// Token: 0x040000B7 RID: 183
		private static readonly IntPtr LoggingCallbackPointer = Marshal.GetFunctionPointerForDelegate<QpdfAPI.CallbackDelegate>(QpdfAPI.LoggingCallbackDelegate);

		// Token: 0x0200015F RID: 351
		// (Invoke) Token: 0x06000B98 RID: 2968
		[NullableContext(0)]
		private delegate int CallbackDelegate(IntPtr data, int length, IntPtr udata);

		// Token: 0x02000160 RID: 352
		[Nullable(0)]
		private static class API
		{
			// Token: 0x06000B9B RID: 2971
			[DllImport("qpdf", CallingConvention = 2)]
			public static extern IntPtr qpdf_get_qpdf_version();

			// Token: 0x06000B9C RID: 2972
			[DllImport("qpdf", CallingConvention = 2)]
			public static extern IntPtr qpdfjob_init();

			// Token: 0x06000B9D RID: 2973
			[DllImport("qpdf", CallingConvention = 2)]
			public static extern void qpdfjob_cleanup(IntPtr jobHandle);

			// Token: 0x06000B9E RID: 2974
			[DllImport("qpdf", CallingConvention = 2)]
			public static extern int qpdfjob_initialize_from_json(IntPtr jobHandle, [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string json);

			// Token: 0x06000B9F RID: 2975
			[DllImport("qpdf", CallingConvention = 2)]
			public static extern int qpdfjob_run(IntPtr jobHandle);

			// Token: 0x06000BA0 RID: 2976
			[DllImport("qpdf", CallingConvention = 2)]
			public static extern IntPtr qpdflogger_create();

			// Token: 0x06000BA1 RID: 2977
			[DllImport("qpdf", CallingConvention = 2)]
			public static extern void qpdflogger_cleanup(IntPtr loggerHandle);

			// Token: 0x06000BA2 RID: 2978
			[DllImport("qpdf", CallingConvention = 2)]
			public static extern void qpdflogger_set_error(IntPtr loggerHandle, int destination, IntPtr callBackHandler, IntPtr udata);

			// Token: 0x06000BA3 RID: 2979
			[DllImport("qpdf", CallingConvention = 2)]
			public static extern void qpdfjob_set_logger(IntPtr jobHandle, IntPtr loggerHandle);

			// Token: 0x04000493 RID: 1171
			private const string LibraryName = "qpdf";
		}
	}
}
