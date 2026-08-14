using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace VSImGui
{
	// Token: 0x02000014 RID: 20
	[NullableContext(1)]
	[Nullable(0)]
	[JsonObject(0)]
	public struct FontData
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000B199 File Offset: 0x00009399
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000B1A1 File Offset: 0x000093A1
		public string Name { readonly get; set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000B1AA File Offset: 0x000093AA
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000B1B2 File Offset: 0x000093B2
		public int Size { readonly get; set; }

		// Token: 0x060001D3 RID: 467 RVA: 0x0000B1BB File Offset: 0x000093BB
		public FontData(string path, int size)
		{
			this.Name = path;
			this.Size = size;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000B1CB File Offset: 0x000093CB
		public FontData([TupleElementNames(new string[]
		{
			"path",
			"size"
		})] [Nullable(new byte[]
		{
			0,
			1
		})] ValueTuple<string, int> values)
		{
			this.Name = values.Item1;
			this.Size = values.Item2;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000B1E5 File Offset: 0x000093E5
		public static implicit operator FontData([TupleElementNames(new string[]
		{
			"path",
			"size"
		})] [Nullable(new byte[]
		{
			0,
			1
		})] ValueTuple<string, int> values)
		{
			return new FontData(values);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000B1ED File Offset: 0x000093ED
		[return: TupleElementNames(new string[]
		{
			"path",
			"size"
		})]
		[return: Nullable(new byte[]
		{
			0,
			1
		})]
		public static implicit operator ValueTuple<string, int>(FontData values)
		{
			return new ValueTuple<string, int>(values.Name, values.Size);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000B204 File Offset: 0x00009404
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 2);
			defaultInterpolatedStringHandler.AppendFormatted(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Size);
			defaultInterpolatedStringHandler.AppendLiteral("px");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
