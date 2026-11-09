using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace ConfigLib
{
	// Token: 0x02000047 RID: 71
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class JsonPatch : IValuePatch
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000B0E7 File Offset: 0x000092E7
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0000B0EF File Offset: 0x000092EF
		public string Path { get; set; }

		// Token: 0x06000205 RID: 517 RVA: 0x0000B0F8 File Offset: 0x000092F8
		public JsonPatch(string path, string value, Config config)
		{
			this._path = new JsonObjectPath(path);
			this.Path = path;
			ISetting setting = config.GetSetting(value);
			this._value = ((setting != null) ? setting.Value : null);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000B12C File Offset: 0x0000932C
		public int Apply(JsonObject asset)
		{
			IEnumerable<JsonObject> enumerable = this._path.Get(asset);
			EnumerableExtensions.Foreach<JsonObject>(enumerable, new Action<JsonObject>(this.ApplyToOne));
			return Enumerable.Count<JsonObject>(enumerable);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000B151 File Offset: 0x00009351
		public void ApplyToOne(JsonObject jsonValue)
		{
			if (this._value == null)
			{
				return;
			}
			if (jsonValue != null)
			{
				jsonValue.Token.Replace(this._value.Token);
			}
		}

		// Token: 0x040000F0 RID: 240
		private readonly JsonObjectPath _path;

		// Token: 0x040000F1 RID: 241
		[Nullable(2)]
		private readonly JsonObject _value;
	}
}
