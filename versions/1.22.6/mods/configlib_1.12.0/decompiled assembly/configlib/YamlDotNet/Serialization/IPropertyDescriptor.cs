using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A0 RID: 160
	[NullableContext(1)]
	public interface IPropertyDescriptor
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003E0 RID: 992
		string Name { get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060003E1 RID: 993
		bool CanWrite { get; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060003E2 RID: 994
		Type Type { get; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060003E3 RID: 995
		// (set) Token: 0x060003E4 RID: 996
		[Nullable(2)]
		Type TypeOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060003E5 RID: 997
		// (set) Token: 0x060003E6 RID: 998
		int Order { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060003E7 RID: 999
		// (set) Token: 0x060003E8 RID: 1000
		ScalarStyle ScalarStyle { get; set; }

		// Token: 0x060003E9 RID: 1001
		[NullableContext(0)]
		[return: Nullable(2)]
		T GetCustomAttribute<T>() where T : Attribute;

		// Token: 0x060003EA RID: 1002
		IObjectDescriptor Read(object target);

		// Token: 0x060003EB RID: 1003
		void Write(object target, [Nullable(2)] object value);
	}
}
