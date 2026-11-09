using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A0 RID: 160
	[NullableContext(1)]
	public interface ITypeInspector
	{
		// Token: 0x060003BC RID: 956
		IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container);

		// Token: 0x060003BD RID: 957
		IPropertyDescriptor GetProperty(Type type, [Nullable(2)] object container, string name, [MaybeNullWhen(true)] bool ignoreUnmatched);
	}
}
