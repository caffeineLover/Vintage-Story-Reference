using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009A RID: 154
	[NullableContext(1)]
	public interface IObjectAccessor
	{
		// Token: 0x060003C8 RID: 968
		void Set(string name, object target, object value);

		// Token: 0x060003C9 RID: 969
		[return: Nullable(2)]
		object Read(string name, object target);
	}
}
