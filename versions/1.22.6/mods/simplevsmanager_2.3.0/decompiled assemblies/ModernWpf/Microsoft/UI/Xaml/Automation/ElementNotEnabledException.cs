using System;

namespace Microsoft.UI.Xaml.Automation
{
	// Token: 0x0200017E RID: 382
	internal class ElementNotEnabledException : Exception
	{
		// Token: 0x06000C1C RID: 3100 RVA: 0x0002187F File Offset: 0x0001FA7F
		public ElementNotEnabledException() : base("The element is not enabled.")
		{
			base.HResult = -2144665570;
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00021897 File Offset: 0x0001FA97
		public ElementNotEnabledException(string message) : base(message)
		{
			base.HResult = -2144665570;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x000218AB File Offset: 0x0001FAAB
		public ElementNotEnabledException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2144665570;
		}
	}
}
