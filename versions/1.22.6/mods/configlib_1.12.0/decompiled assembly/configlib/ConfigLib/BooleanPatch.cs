using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using SimpleExpressionEngine;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace ConfigLib
{
	// Token: 0x02000043 RID: 67
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class BooleanPatch : IValuePatch
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000AC68 File Offset: 0x00008E68
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x0000AC70 File Offset: 0x00008E70
		public string Path { get; set; }

		// Token: 0x060001D6 RID: 470 RVA: 0x0000AC79 File Offset: 0x00008E79
		public BooleanPatch(string path, string formula, IContext<float, float> context)
		{
			this._path = new JsonObjectPath(path);
			this.Path = path;
			this._value = MathParser.Parse(formula);
			this._context = context;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000ACA7 File Offset: 0x00008EA7
		public int Apply(JsonObject asset)
		{
			IEnumerable<JsonObject> enumerable = this._path.Get(asset);
			EnumerableExtensions.Foreach<JsonObject>(enumerable, new Action<JsonObject>(this.ApplyToOne));
			return Enumerable.Count<JsonObject>(enumerable);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000ACCC File Offset: 0x00008ECC
		public void ApplyToOne(JsonObject jsonValue)
		{
			float previousValue = (jsonValue != null && jsonValue.AsBool(false)) > false;
			List<IContext<float, float>> list = new List<IContext<float, float>>();
			list.Add(this._context);
			list.Add(new ValueContext<float, float>(previousValue));
			CombinedContext<float, float> context = new CombinedContext<float, float>(list);
			bool result = Math.Abs(this._value.Evaluate(context)) > 1E-10f;
			if (jsonValue != null)
			{
				jsonValue.Token.Replace(new JValue(result));
			}
		}

		// Token: 0x040000E5 RID: 229
		private readonly JsonObjectPath _path;

		// Token: 0x040000E6 RID: 230
		private readonly INode<float, float, float> _value;

		// Token: 0x040000E7 RID: 231
		private readonly IContext<float, float> _context;

		// Token: 0x040000E8 RID: 232
		private const float _epsilon = 1E-10f;
	}
}
