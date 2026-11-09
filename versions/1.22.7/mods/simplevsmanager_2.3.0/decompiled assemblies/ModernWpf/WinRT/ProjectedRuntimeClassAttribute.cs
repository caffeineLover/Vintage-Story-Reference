using System;
using System.ComponentModel;

namespace WinRT
{
	// Token: 0x02000185 RID: 389
	[EditorBrowsable(1)]
	[AttributeUsage(4, Inherited = false, AllowMultiple = false)]
	internal sealed class ProjectedRuntimeClassAttribute : Attribute
	{
		// Token: 0x06000C3E RID: 3134 RVA: 0x00021BB4 File Offset: 0x0001FDB4
		public ProjectedRuntimeClassAttribute(string defaultInterfaceProp)
		{
			this.DefaultInterfaceProperty = defaultInterfaceProp;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00021BC3 File Offset: 0x0001FDC3
		public ProjectedRuntimeClassAttribute(Type defaultInterface)
		{
			this.DefaultInterface = defaultInterface;
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x00021BD2 File Offset: 0x0001FDD2
		public string DefaultInterfaceProperty { get; }

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00021BDA File Offset: 0x0001FDDA
		public Type DefaultInterface { get; }
	}
}
