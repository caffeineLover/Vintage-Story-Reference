using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace ConfigLib
{
	// Token: 0x02000048 RID: 72
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ConstPatch : IValuePatch
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000B175 File Offset: 0x00009375
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000B17D File Offset: 0x0000937D
		public string Path { get; set; }

		// Token: 0x0600020A RID: 522 RVA: 0x0000B186 File Offset: 0x00009386
		public ConstPatch(string path, JsonObject value)
		{
			this._path = new JsonObjectPath(path);
			this.Path = path;
			this._value = value;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000B1A8 File Offset: 0x000093A8
		public int Apply(JsonObject asset)
		{
			IEnumerable<JsonObject> enumerable = this._path.Get(asset);
			EnumerableExtensions.Foreach<JsonObject>(enumerable, new Action<JsonObject>(this.ApplyToOne));
			return Enumerable.Count<JsonObject>(enumerable);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000B1CD File Offset: 0x000093CD
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

		// Token: 0x040000F3 RID: 243
		private readonly JsonObjectPath _path;

		// Token: 0x040000F4 RID: 244
		private readonly JsonObject _value;
	}
}
