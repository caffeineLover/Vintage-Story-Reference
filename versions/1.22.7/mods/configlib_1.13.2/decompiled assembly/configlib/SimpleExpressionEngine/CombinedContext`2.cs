using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x0200005C RID: 92
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CombinedContext<[Nullable(2)] TResult, [Nullable(2)] TArguments> : IContext<TResult, TArguments>
	{
		// Token: 0x0600027D RID: 637 RVA: 0x0000DB6B File Offset: 0x0000BD6B
		public CombinedContext(IEnumerable<IContext<TResult, TArguments>> contexts)
		{
			this.mContexts = contexts;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000DB7C File Offset: 0x0000BD7C
		public bool Resolvable(string name)
		{
			using (IEnumerator<IContext<TResult, TArguments>> enumerator = this.mContexts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Resolvable(name))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		public TResult Resolve(string name, params TArguments[] arguments)
		{
			foreach (IContext<TResult, TArguments> context in this.mContexts)
			{
				if (context.Resolvable(name))
				{
					return context.Resolve(name, arguments);
				}
			}
			throw new InvalidDataException("Unresolvable: '" + name + "'");
		}

		// Token: 0x04000134 RID: 308
		private readonly IEnumerable<IContext<TResult, TArguments>> mContexts;
	}
}
