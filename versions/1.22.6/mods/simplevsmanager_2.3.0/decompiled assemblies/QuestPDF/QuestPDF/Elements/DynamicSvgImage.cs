using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Elements
{
	// Token: 0x020000C9 RID: 201
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DynamicSvgImage : Element, IStateful, IDisposable
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0000F2A3 File Offset: 0x0000D4A3
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x0000F2AB File Offset: 0x0000D4AB
		public GenerateDynamicSvgDelegate SvgSource { get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0000F2B4 File Offset: 0x0000D4B4
		[TupleElementNames(new string[]
		{
			"Size",
			"Image"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			2
		})]
		private List<ValueTuple<Size, SkSvgImage>> Cache { [return: TupleElementNames(new string[]
		{
			"Size",
			"Image"
		})] [return: Nullable(new byte[]
		{
			1,
			0,
			2
		})] get; } = new List<ValueTuple<Size, SkSvgImage>>(1);

		// Token: 0x0600062B RID: 1579 RVA: 0x0000F2BC File Offset: 0x0000D4BC
		~DynamicSvgImage()
		{
			this.WarnThatFinalizerIsReached<DynamicSvgImage>();
			this.Dispose();
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0000F2F0 File Offset: 0x0000D4F0
		public void Dispose()
		{
			foreach (ValueTuple<Size, SkSvgImage> valueTuple in this.Cache)
			{
				SkSvgImage item = valueTuple.Item2;
				if (item != null)
				{
					item.Dispose();
				}
			}
			this.Cache.Clear();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0000F35C File Offset: 0x0000D55C
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			if (availableSpace.IsNegative())
			{
				return SpacePlan.Wrap("The available space is negative.");
			}
			return SpacePlan.FullRender(availableSpace);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0000F388 File Offset: 0x0000D588
		internal override void Draw(Size availableSpace)
		{
			SkSvgImage skSvgImage = Enumerable.FirstOrDefault<ValueTuple<Size, SkSvgImage>>(this.Cache, ([TupleElementNames(new string[]
			{
				"Size",
				"Image"
			})] ValueTuple<Size, SkSvgImage> x) => Size.Equal(x.Item1, availableSpace)).Item2;
			if (skSvgImage == null)
			{
				skSvgImage = this.GetImage(availableSpace);
				this.Cache.Add(new ValueTuple<Size, SkSvgImage>(availableSpace, skSvgImage));
			}
			if (skSvgImage != null)
			{
				ValueTuple<float, float> valueTuple = skSvgImage.CalculateSpaceScale(availableSpace);
				float item = valueTuple.Item1;
				float item2 = valueTuple.Item2;
				base.Canvas.Save();
				base.Canvas.Scale(item, item2);
				base.Canvas.DrawSvg(skSvgImage, availableSpace);
				base.Canvas.Restore();
			}
			this.IsRendered = true;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0000F440 File Offset: 0x0000D640
		[NullableContext(2)]
		private SkSvgImage GetImage(Size availableSpace)
		{
			GenerateDynamicSvgDelegate svgSource = this.SvgSource;
			string text = (svgSource != null) ? svgSource(availableSpace) : null;
			if (text == null)
			{
				return null;
			}
			return new SkSvgImage(text, SkResourceProvider.CurrentResourceProvider, FontManager.CurrentFontManager);
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0000F476 File Offset: 0x0000D676
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x0000F47E File Offset: 0x0000D67E
		private bool IsRendered { get; set; }

		// Token: 0x06000632 RID: 1586 RVA: 0x0000F487 File Offset: 0x0000D687
		public void ResetState(bool hardReset = false)
		{
			this.IsRendered = false;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0000F490 File Offset: 0x0000D690
		public object GetState()
		{
			return this.IsRendered;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0000F49D File Offset: 0x0000D69D
		public void SetState(object state)
		{
			this.IsRendered = (bool)state;
		}
	}
}
