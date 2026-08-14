using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;

namespace ConfigLib
{
	// Token: 0x02000041 RID: 65
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class BooleanExpressionSelector
	{
		// Token: 0x060001CD RID: 461 RVA: 0x0000AAC4 File Offset: 0x00008CC4
		public BooleanExpressionSelector(string expression, string onTrue, string onFalse, IContext<float, float> context)
		{
			this._value = MathParser.Parse(expression);
			this._context = context;
			this._onTrue = onTrue;
			this._onFalse = onFalse;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000AAF0 File Offset: 0x00008CF0
		public string Resolve(float value)
		{
			List<IContext<float, float>> list = new List<IContext<float, float>>();
			list.Add(this._context);
			list.Add(new ValueContext<float, float>(value));
			CombinedContext<float, float> context = new CombinedContext<float, float>(list);
			if (Math.Abs(this._value.Evaluate(context)) <= 1E-10f)
			{
				return this._onFalse;
			}
			return this._onTrue;
		}

		// Token: 0x040000DA RID: 218
		private readonly INode<float, float, float> _value;

		// Token: 0x040000DB RID: 219
		private readonly IContext<float, float> _context;

		// Token: 0x040000DC RID: 220
		private readonly string _onTrue;

		// Token: 0x040000DD RID: 221
		private readonly string _onFalse;

		// Token: 0x040000DE RID: 222
		private const float _epsilon = 1E-10f;
	}
}
