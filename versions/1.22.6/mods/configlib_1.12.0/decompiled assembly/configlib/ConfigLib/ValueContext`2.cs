using System;
using System.IO;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;

namespace ConfigLib
{
	// Token: 0x0200002F RID: 47
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ValueContext<[Nullable(2)] TResult, [Nullable(2)] TArguments> : IContext<TResult, TArguments>
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00008A48 File Offset: 0x00006C48
		public ValueContext(TResult value)
		{
			this._value = value;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00008A57 File Offset: 0x00006C57
		public bool Resolvable(string name)
		{
			return name == "value";
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00008A64 File Offset: 0x00006C64
		public TResult Resolve(string name, params TArguments[] arguments)
		{
			if (name == "value")
			{
				return this._value;
			}
			throw new InvalidDataException("Unresolvable: '" + name + "'");
		}

		// Token: 0x040000A7 RID: 167
		private readonly TResult _value;
	}
}
