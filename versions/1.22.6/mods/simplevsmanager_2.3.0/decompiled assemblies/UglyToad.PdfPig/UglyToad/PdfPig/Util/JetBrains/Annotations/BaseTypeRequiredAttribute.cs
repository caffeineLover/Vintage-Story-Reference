using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000043 RID: 67
	[AttributeUsage(4, AllowMultiple = true)]
	[BaseTypeRequired(typeof(Attribute))]
	internal sealed class BaseTypeRequiredAttribute : Attribute
	{
		// Token: 0x06000179 RID: 377 RVA: 0x0000E098 File Offset: 0x0000C298
		public BaseTypeRequiredAttribute([NotNull] Type baseType)
		{
			this.BaseType = baseType;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000E0A7 File Offset: 0x0000C2A7
		// (set) Token: 0x0600017B RID: 379 RVA: 0x0000E0AF File Offset: 0x0000C2AF
		[NotNull]
		public Type BaseType { get; private set; }
	}
}
