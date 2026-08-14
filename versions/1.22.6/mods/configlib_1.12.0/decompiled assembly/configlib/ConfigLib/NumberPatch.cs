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
	// Token: 0x02000042 RID: 66
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class NumberPatch : IValuePatch
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000AB45 File Offset: 0x00008D45
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000AB4D File Offset: 0x00008D4D
		public string Path { get; set; }

		// Token: 0x060001D1 RID: 465 RVA: 0x0000AB56 File Offset: 0x00008D56
		public NumberPatch(string path, string formula, IContext<float, float> context, ConfigSettingType type)
		{
			this._path = new JsonObjectPath(path);
			this.Path = path;
			this._value = MathParser.Parse(formula);
			this._context = context;
			this._returnType = type;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000AB8C File Offset: 0x00008D8C
		public int Apply(JsonObject asset)
		{
			IEnumerable<JsonObject> enumerable = this._path.Get(asset);
			EnumerableExtensions.Foreach<JsonObject>(enumerable, new Action<JsonObject>(this.ApplyToOne));
			return Enumerable.Count<JsonObject>(enumerable);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000ABB4 File Offset: 0x00008DB4
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

		// Token: 0x040000E0 RID: 224
		private readonly JsonObjectPath _path;

		// Token: 0x040000E1 RID: 225
		private readonly INode<float, float, float> _value;

		// Token: 0x040000E2 RID: 226
		private readonly IContext<float, float> _context;

		// Token: 0x040000E3 RID: 227
		private readonly ConfigSettingType _returnType;
	}
}
