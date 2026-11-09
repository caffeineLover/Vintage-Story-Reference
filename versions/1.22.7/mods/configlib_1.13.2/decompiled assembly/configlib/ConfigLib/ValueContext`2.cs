using System;
using System.IO;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;

namespace ConfigLib
{
	// Token: 0x02000031 RID: 49
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ValueContext<[Nullable(2)] TResult, [Nullable(2)] TArguments> : IContext<TResult, TArguments>
	{
		// Token: 0x06000188 RID: 392 RVA: 0x00008D64 File Offset: 0x00006F64
		public ValueContext(TResult value)
		{
			this._value = value;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008D73 File Offset: 0x00006F73
		public bool Resolvable(string name)
		{
			return name == "value";
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008D80 File Offset: 0x00006F80
		public TResult Resolve(string name, params TArguments[] arguments)
		{
			if (name == "value")
			{
				return this._value;
			}
			throw new InvalidDataException("Unresolvable: '" + name + "'");
		}

		// Token: 0x040000AA RID: 170
		private readonly TResult _value;
	}
}
