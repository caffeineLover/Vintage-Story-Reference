using System;

namespace Microsoft.UI.Xaml.Markup
{
	// Token: 0x0200017C RID: 380
	internal class XamlParseException : Exception
	{
		// Token: 0x06000C15 RID: 3093 RVA: 0x000217F3 File Offset: 0x0001F9F3
		public XamlParseException() : base("XAML parsing failed.")
		{
			base.HResult = -2144665590;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0002180B File Offset: 0x0001FA0B
		public XamlParseException(string message) : base(message)
		{
			base.HResult = -2144665590;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0002181F File Offset: 0x0001FA1F
		public XamlParseException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2144665590;
		}
	}
}
