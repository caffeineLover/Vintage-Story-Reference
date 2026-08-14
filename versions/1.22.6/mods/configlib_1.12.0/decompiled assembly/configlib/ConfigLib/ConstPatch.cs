using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace ConfigLib
{
	// Token: 0x02000046 RID: 70
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ConstPatch : IValuePatch
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000AE59 File Offset: 0x00009059
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000AE61 File Offset: 0x00009061
		public string Path { get; set; }

		// Token: 0x060001E5 RID: 485 RVA: 0x0000AE6A File Offset: 0x0000906A
		public ConstPatch(string path, JsonObject value)
		{
			this._path = new JsonObjectPath(path);
			this.Path = path;
			this._value = value;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000AE8C File Offset: 0x0000908C
		public int Apply(JsonObject asset)
		{
			IEnumerable<JsonObject> enumerable = this._path.Get(asset);
			EnumerableExtensions.Foreach<JsonObject>(enumerable, new Action<JsonObject>(this.ApplyToOne));
			return Enumerable.Count<JsonObject>(enumerable);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000AEB1 File Offset: 0x000090B1
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
		private readonly JsonObject _value;
	}
}
