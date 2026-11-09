using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200001C RID: 28
	[NullableContext(1)]
	public interface IObjectAccessor
	{
		// Token: 0x060000B5 RID: 181
		void Set(string name, object target, object value);

		// Token: 0x060000B6 RID: 182
		[return: Nullable(2)]
		object Read(string name, object target);
	}
}
