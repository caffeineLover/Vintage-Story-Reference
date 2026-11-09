using System;
using System.Runtime.Serialization;

namespace Microsoft.UI.Xaml.Automation
{
	// Token: 0x0200017D RID: 381
	[Serializable]
	internal class ElementNotAvailableException : Exception
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x00021834 File Offset: 0x0001FA34
		public ElementNotAvailableException() : base("The element is not available.")
		{
			base.HResult = -2144665569;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0002184C File Offset: 0x0001FA4C
		public ElementNotAvailableException(string message) : base(message)
		{
			base.HResult = -2144665569;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00021860 File Offset: 0x0001FA60
		public ElementNotAvailableException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2144665569;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00021875 File Offset: 0x0001FA75
		protected ElementNotAvailableException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
