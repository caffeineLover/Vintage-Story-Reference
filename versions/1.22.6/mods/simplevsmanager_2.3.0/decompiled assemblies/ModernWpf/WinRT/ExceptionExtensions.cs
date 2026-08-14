using System;

namespace WinRT
{
	// Token: 0x02000190 RID: 400
	internal static class ExceptionExtensions
	{
		// Token: 0x06000CA7 RID: 3239 RVA: 0x00024223 File Offset: 0x00022423
		public static void SetHResult(this Exception ex, int value)
		{
			ex.GetType().GetProperty("HResult").SetValue(ex, value);
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00024244 File Offset: 0x00022444
		internal static Exception GetExceptionForHR(this Exception innerException, int hresult, string messageResource)
		{
			Exception ex;
			if (innerException != null)
			{
				ex = new Exception(innerException.Message ?? messageResource, innerException);
			}
			else
			{
				ex = new Exception(messageResource);
			}
			ex.SetHResult(hresult);
			return ex;
		}
	}
}
