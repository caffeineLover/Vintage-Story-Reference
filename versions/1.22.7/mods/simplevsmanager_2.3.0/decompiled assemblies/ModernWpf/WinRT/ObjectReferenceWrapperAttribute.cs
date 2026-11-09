using System;
using System.ComponentModel;

namespace WinRT
{
	// Token: 0x02000186 RID: 390
	[EditorBrowsable(1)]
	[AttributeUsage(4, Inherited = false, AllowMultiple = false)]
	internal sealed class ObjectReferenceWrapperAttribute : Attribute
	{
		// Token: 0x06000C42 RID: 3138 RVA: 0x00021BE2 File Offset: 0x0001FDE2
		public ObjectReferenceWrapperAttribute(string objectReferenceField)
		{
			this.ObjectReferenceField = objectReferenceField;
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00021BF1 File Offset: 0x0001FDF1
		public string ObjectReferenceField { get; }
	}
}
