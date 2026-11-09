using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009C RID: 156
	[NullableContext(1)]
	public interface IPropertyDescriptor
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003A4 RID: 932
		string Name { get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060003A5 RID: 933
		bool CanWrite { get; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060003A6 RID: 934
		Type Type { get; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060003A7 RID: 935
		// (set) Token: 0x060003A8 RID: 936
		[Nullable(2)]
		Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060003A9 RID: 937
		// (set) Token: 0x060003AA RID: 938
		int Order { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060003AB RID: 939
		// (set) Token: 0x060003AC RID: 940
		ScalarStyle ScalarStyle { get; set; }

		// Token: 0x060003AD RID: 941
		[NullableContext(0)]
		[return: Nullable(2)]
		T GetCustomAttribute<T>() where T : Attribute;

		// Token: 0x060003AE RID: 942
		IObjectDescriptor Read(object target);

		// Token: 0x060003AF RID: 943
		void Write(object target, [Nullable(2)] object value);
	}
}
