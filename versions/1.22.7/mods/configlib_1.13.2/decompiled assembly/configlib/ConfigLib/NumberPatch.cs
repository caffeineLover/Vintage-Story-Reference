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
	// Token: 0x02000044 RID: 68
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class NumberPatch : IValuePatch
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000AE61 File Offset: 0x00009061
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000AE69 File Offset: 0x00009069
		public string Path { get; set; }

		// Token: 0x060001F6 RID: 502 RVA: 0x0000AE72 File Offset: 0x00009072
		public NumberPatch(string path, string formula, IContext<float, float> context, ConfigSettingType type)
		{
			this._path = new JsonObjectPath(path);
			this.Path = path;
			this._value = MathParser.Parse(formula);
			this._context = context;
			this._returnType = type;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000AEA8 File Offset: 0x000090A8
		public int Apply(JsonObject asset)
		{
			IEnumerable<JsonObject> enumerable = this._path.Get(asset);
			EnumerableExtensions.Foreach<JsonObject>(enumerable, new Action<JsonObject>(this.ApplyToOne));
			return Enumerable.Count<JsonObject>(enumerable);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000AED0 File Offset: 0x000090D0
		public void ApplyToOne(JsonObject jsonValue)
		{
			float previousValue = (jsonValue != null) ? jsonValue.AsFloat(0f) : 0f;
			if (jsonValue != null && ((JValue)jsonValue.Token).Value is bool)
			{
				previousValue = (jsonValue.AsBool(false) > false);
			}
			List<IContext<float, float>> list = new List<IContext<float, float>>();
			list.Add(new ValueContext<float, float>(previousValue));
			list.Add(this._context);
			CombinedContext<float, float> context = new CombinedContext<float, float>(list);
			float value = this._value.Evaluate(context);
			ConfigSettingType returnType = this._returnType;
			if (returnType != ConfigSettingType.Float)
			{
				if (returnType != ConfigSettingType.Integer)
				{
					return;
				}
				if (jsonValue != null)
				{
					jsonValue.Token.Replace(new JValue((long)((int)value)));
				}
			}
			else if (jsonValue != null)
			{
				jsonValue.Token.Replace(new JValue(value));
				return;
			}
		}

		// Token: 0x040000E3 RID: 227
		private readonly JsonObjectPath _path;

		// Token: 0x040000E4 RID: 228
		private readonly INode<float, float, float> _value;

		// Token: 0x040000E5 RID: 229
		private readonly IContext<float, float> _context;

		// Token: 0x040000E6 RID: 230
		private readonly ConfigSettingType _returnType;
	}
}
