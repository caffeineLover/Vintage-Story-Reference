using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000026 RID: 38
	[NullableContext(1)]
	public interface ITypeInspector
	{
		// Token: 0x060000E8 RID: 232
		IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container);

		// Token: 0x060000E9 RID: 233
		IPropertyDescriptor GetProperty(Type type, [Nullable(2)] object container, string name, [MaybeNullWhen(true)] bool ignoreUnmatched, bool caseInsensitivePropertyMatching);

		// Token: 0x060000EA RID: 234
		string GetEnumName(Type enumType, string name);

		// Token: 0x060000EB RID: 235
		string GetEnumValue(object enumValue);
	}
}
