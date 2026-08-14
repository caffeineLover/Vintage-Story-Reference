using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000F8 RID: 248
	internal sealed class Translate : ContainerElement
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x000141E9 File Offset: 0x000123E9
		// (set) Token: 0x06000843 RID: 2115 RVA: 0x000141F1 File Offset: 0x000123F1
		public float TranslateX { get; set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x000141FA File Offset: 0x000123FA
		// (set) Token: 0x06000845 RID: 2117 RVA: 0x00014202 File Offset: 0x00012402
		public float TranslateY { get; set; }

		// Token: 0x06000846 RID: 2118 RVA: 0x0001420C File Offset: 0x0001240C
		internal override void Draw(Size availableSpace)
		{
			Position vector = new Position(this.TranslateX, this.TranslateY);
			base.Canvas.Translate(vector);
			base.Draw(availableSpace);
			base.Canvas.Translate(vector.Reverse());
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00014254 File Offset: 0x00012454
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			return string.Join("   ", Enumerable.Select<ValueTuple<string, float>, string>(Enumerable.Where<ValueTuple<string, float>>(this.<GetCompanionHint>g__GetOptions|9_2(), ([TupleElementNames(new string[]
			{
				"Label",
				"value"
			})] ValueTuple<string, float> x) => x.Item2 != 0f), ([TupleElementNames(new string[]
			{
				"Label",
				"value"
			})] ValueTuple<string, float> x) => x.Item1 + "=" + x.Item2.FormatAsCompanionNumber()));
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x000142C1 File Offset: 0x000124C1
		[CompilerGenerated]
		[return: TupleElementNames(new string[]
		{
			"Label",
			"value"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private IEnumerable<ValueTuple<string, float>> <GetCompanionHint>g__GetOptions|9_2()
		{
			Translate.<<GetCompanionHint>g__GetOptions|9_2>d <<GetCompanionHint>g__GetOptions|9_2>d = new Translate.<<GetCompanionHint>g__GetOptions|9_2>d(-2);
			<<GetCompanionHint>g__GetOptions|9_2>d.<>4__this = this;
			return <<GetCompanionHint>g__GetOptions|9_2>d;
		}
	}
}
