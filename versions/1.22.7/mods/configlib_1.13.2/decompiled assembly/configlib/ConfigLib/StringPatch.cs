using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace ConfigLib
{
	// Token: 0x02000046 RID: 70
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class StringPatch : IValuePatch
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000B057 File Offset: 0x00009257
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000B05F File Offset: 0x0000925F
		public string Path { get; set; }

		// Token: 0x06000200 RID: 512 RVA: 0x0000B068 File Offset: 0x00009268
		public StringPatch(string path, string value, Config config)
		{
			this._path = new JsonObjectPath(path);
			this.Path = path;
			ISetting setting = config.GetSetting(value);
			this._value = (((setting != null) ? setting.Value.AsString(null) : null) ?? value);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000B0A7 File Offset: 0x000092A7
		public int Apply(JsonObject asset)
		{
			IEnumerable<JsonObject> enumerable = this._path.Get(asset);
			EnumerableExtensions.Foreach<JsonObject>(enumerable, new Action<JsonObject>(this.ApplyToOne));
			return Enumerable.Count<JsonObject>(enumerable);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000B0CC File Offset: 0x000092CC
		public void ApplyToOne(JsonObject jsonValue)
		{
			if (jsonValue != null)
			{
				jsonValue.Token.Replace(new JValue(this._value));
			}
		}

		// Token: 0x040000ED RID: 237
		private readonly JsonObjectPath _path;

		// Token: 0x040000EE RID: 238
		private readonly string _value;
	}
}
