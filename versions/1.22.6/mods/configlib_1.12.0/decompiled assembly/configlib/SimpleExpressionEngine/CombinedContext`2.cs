using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x02000060 RID: 96
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CombinedContext<[Nullable(2)] TResult, [Nullable(2)] TArguments> : IContext<TResult, TArguments>
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x0000E4B2 File Offset: 0x0000C6B2
		public CombinedContext(IEnumerable<IContext<TResult, TArguments>> contexts)
		{
			this.mContexts = contexts;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000E4C4 File Offset: 0x0000C6C4
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

		// Token: 0x060002BB RID: 699 RVA: 0x0000E518 File Offset: 0x0000C718
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

		// Token: 0x04000162 RID: 354
		private readonly IEnumerable<IContext<TResult, TArguments>> mContexts;
	}
}
