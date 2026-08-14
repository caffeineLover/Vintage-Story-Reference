using System;

namespace WinRT.Interop
{
	// Token: 0x020001C6 RID: 454
	internal sealed class ManagedExceptionErrorInfo : IErrorInfo, ISupportErrorInfo
	{
		// Token: 0x06000E4E RID: 3662 RVA: 0x0002B1EF File Offset: 0x000293EF
		public ManagedExceptionErrorInfo(Exception ex)
		{
			this._exception = ex;
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0002B1FE File Offset: 0x000293FE
		public bool InterfaceSupportsErrorInfo(Guid riid)
		{
			return true;
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0002B204 File Offset: 0x00029404
		public Guid GetGuid()
		{
			return default(Guid);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0002B21A File Offset: 0x0002941A
		public string GetSource()
		{
			return this._exception.Source;
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0002B228 File Offset: 0x00029428
		public string GetDescription()
		{
			string text = this._exception.Message;
			if (string.IsNullOrEmpty(text))
			{
				text = this._exception.GetType().FullName;
			}
			return text;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0002B25B File Offset: 0x0002945B
		public string GetHelpFile()
		{
			return this._exception.HelpLink;
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0002B268 File Offset: 0x00029468
		public string GetHelpFileContent()
		{
			return string.Empty;
		}

		// Token: 0x040002C8 RID: 712
		private readonly Exception _exception;
	}
}
