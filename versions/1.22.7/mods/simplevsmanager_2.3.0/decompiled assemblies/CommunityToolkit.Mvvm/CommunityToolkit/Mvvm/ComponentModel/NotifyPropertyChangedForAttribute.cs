using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.ComponentModel
{
	// Token: 0x02000030 RID: 48
	[NullableContext(1)]
	[Nullable(0)]
	[AttributeUsage(384, AllowMultiple = true, Inherited = false)]
	public sealed class NotifyPropertyChangedForAttribute : Attribute
	{
		// Token: 0x06000126 RID: 294 RVA: 0x0000580A File Offset: 0x00003A0A
		public NotifyPropertyChangedForAttribute(string propertyName)
		{
			this.PropertyNames = new string[]
			{
				propertyName
			};
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005822 File Offset: 0x00003A22
		public NotifyPropertyChangedForAttribute(string propertyName, params string[] otherPropertyNames)
		{
			this.PropertyNames = Enumerable.ToArray<string>(Enumerable.Concat<string>(new string[]
			{
				propertyName
			}, otherPropertyNames));
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00005845 File Offset: 0x00003A45
		public string[] PropertyNames { get; }
	}
}
