using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace ConfigLib
{
	// Token: 0x02000044 RID: 68
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class StringPatch : IValuePatch
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000AD3B File Offset: 0x00008F3B
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000AD43 File Offset: 0x00008F43
		public string Path { get; set; }

		// Token: 0x060001DB RID: 475 RVA: 0x0000AD4C File Offset: 0x00008F4C
		public StringPatch(string path, string value, Config config)
		{
			this._path = new JsonObjectPath(path);
			this.Path = path;
			ISetting setting = config.GetSetting(value);
			this._value = (((setting != null) ? setting.Value.AsString(null) : null) ?? value);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000AD8B File Offset: 0x00008F8B
		public int Apply(JsonObject asset)
		{
			IEnumerable<JsonObject> enumerable = this._path.Get(asset);
			EnumerableExtensions.Foreach<JsonObject>(enumerable, new Action<JsonObject>(this.ApplyToOne));
			return Enumerable.Count<JsonObject>(enumerable);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000ADB0 File Offset: 0x00008FB0
		public void ApplyToOne(JsonObject jsonValue)
		{
			if (jsonValue != null)
			{
				jsonValue.Token.Replace(new JValue(this._value));
			}
		}

		// Token: 0x040000EA RID: 234
		private readonly JsonObjectPath _path;

		// Token: 0x040000EB RID: 235
		private readonly string _value;
	}
}
