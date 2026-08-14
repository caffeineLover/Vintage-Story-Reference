using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A4 RID: 164
	[NullableContext(1)]
	public interface ITypeInspector
	{
		// Token: 0x060003F8 RID: 1016
		IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container);

		// Token: 0x060003F9 RID: 1017
		IPropertyDescriptor GetProperty(Type type, [Nullable(2)] object container, string name, [MaybeNullWhen(true)] bool ignoreUnmatched);
	}
}
