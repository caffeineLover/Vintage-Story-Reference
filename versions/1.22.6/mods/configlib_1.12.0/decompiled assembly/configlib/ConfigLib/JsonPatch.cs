using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace ConfigLib
{
	// Token: 0x02000045 RID: 69
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class JsonPatch : IValuePatch
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000ADCB File Offset: 0x00008FCB
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000ADD3 File Offset: 0x00008FD3
		public string Path { get; set; }

		// Token: 0x060001E0 RID: 480 RVA: 0x0000ADDC File Offset: 0x00008FDC
		public JsonPatch(string path, string value, Config config)
		{
			this._path = new JsonObjectPath(path);
			this.Path = path;
			ISetting setting = config.GetSetting(value);
			this._value = ((setting != null) ? setting.Value : null);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000AE10 File Offset: 0x00009010
		public int Apply(JsonObject asset)
		{
			IEnumerable<JsonObject> enumerable = this._path.Get(asset);
			EnumerableExtensions.Foreach<JsonObject>(enumerable, new Action<JsonObject>(this.ApplyToOne));
			return Enumerable.Count<JsonObject>(enumerable);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000AE35 File Offset: 0x00009035
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

		// Token: 0x040000ED RID: 237
		private readonly JsonObjectPath _path;

		// Token: 0x040000EE RID: 238
		[Nullable(2)]
		private readonly JsonObject _value;
	}
}
