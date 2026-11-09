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
	// Token: 0x02000045 RID: 69
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class BooleanPatch : IValuePatch
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000AF84 File Offset: 0x00009184
		// (set) Token: 0x060001FA RID: 506 RVA: 0x0000AF8C File Offset: 0x0000918C
		public string Path { get; set; }

		// Token: 0x060001FB RID: 507 RVA: 0x0000AF95 File Offset: 0x00009195
		public BooleanPatch(string path, string formula, IContext<float, float> context)
		{
			this._path = new JsonObjectPath(path);
			this.Path = path;
			this._value = MathParser.Parse(formula);
			this._context = context;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000AFC3 File Offset: 0x000091C3
		public int Apply(JsonObject asset)
		{
			IEnumerable<JsonObject> enumerable = this._path.Get(asset);
			EnumerableExtensions.Foreach<JsonObject>(enumerable, new Action<JsonObject>(this.ApplyToOne));
			return Enumerable.Count<JsonObject>(enumerable);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000AFE8 File Offset: 0x000091E8
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

		// Token: 0x040000E8 RID: 232
		private readonly JsonObjectPath _path;

		// Token: 0x040000E9 RID: 233
		private readonly INode<float, float, float> _value;

		// Token: 0x040000EA RID: 234
		private readonly IContext<float, float> _context;

		// Token: 0x040000EB RID: 235
		private const float _epsilon = 1E-10f;
	}
}
