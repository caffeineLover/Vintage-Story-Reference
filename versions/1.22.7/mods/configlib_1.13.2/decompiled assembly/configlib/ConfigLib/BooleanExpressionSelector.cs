using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;

namespace ConfigLib
{
	// Token: 0x02000043 RID: 67
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class BooleanExpressionSelector
	{
		// Token: 0x060001F2 RID: 498 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		public BooleanExpressionSelector(string expression, string onTrue, string onFalse, IContext<float, float> context)
		{
			this._value = MathParser.Parse(expression);
			this._context = context;
			this._onTrue = onTrue;
			this._onFalse = onFalse;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000AE0C File Offset: 0x0000900C
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

		// Token: 0x040000DD RID: 221
		private readonly INode<float, float, float> _value;

		// Token: 0x040000DE RID: 222
		private readonly IContext<float, float> _context;

		// Token: 0x040000DF RID: 223
		private readonly string _onTrue;

		// Token: 0x040000E0 RID: 224
		private readonly string _onFalse;

		// Token: 0x040000E1 RID: 225
		private const float _epsilon = 1E-10f;
	}
}
