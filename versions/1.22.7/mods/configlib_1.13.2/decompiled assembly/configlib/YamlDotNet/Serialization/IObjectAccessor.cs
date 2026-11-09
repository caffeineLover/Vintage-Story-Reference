using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000096 RID: 150
	[NullableContext(1)]
	public interface IObjectAccessor
	{
		// Token: 0x0600038C RID: 908
		void Set(string name, object target, object value);

		// Token: 0x0600038D RID: 909
		[return: Nullable(2)]
		object Read(string name, object target);
	}
}
