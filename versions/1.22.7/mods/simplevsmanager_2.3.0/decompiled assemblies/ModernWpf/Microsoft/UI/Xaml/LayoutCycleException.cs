using System;
using System.Runtime.Serialization;

namespace Microsoft.UI.Xaml
{
	// Token: 0x0200017A RID: 378
	[Serializable]
	internal class LayoutCycleException : Exception
	{
		// Token: 0x06000C09 RID: 3081 RVA: 0x000217A8 File Offset: 0x0001F9A8
		public LayoutCycleException() : base("A cycle occurred while laying out the GUI.")
		{
			base.HResult = -2144665580;
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000217C0 File Offset: 0x0001F9C0
		public LayoutCycleException(string message) : base(message)
		{
			base.HResult = -2144665580;
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x000217D4 File Offset: 0x0001F9D4
		public LayoutCycleException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2144665580;
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x000217E9 File Offset: 0x0001F9E9
		protected LayoutCycleException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
