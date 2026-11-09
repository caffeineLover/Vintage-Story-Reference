using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Elements
{
	// Token: 0x020000F6 RID: 246
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SvgImage : Element, IStateful, IDisposable
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00013FCA File Offset: 0x000121CA
		// (set) Token: 0x0600082B RID: 2091 RVA: 0x00013FD2 File Offset: 0x000121D2
		public QuestPDF.Infrastructure.SvgImage Image { get; set; }

		// Token: 0x0600082C RID: 2092 RVA: 0x00013FDC File Offset: 0x000121DC
		~SvgImage()
		{
			this.WarnThatFinalizerIsReached<SvgImage>();
			this.Dispose();
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00014010 File Offset: 0x00012210
		public void Dispose()
		{
			if (this.Image != null && !this.Image.IsShared)
			{
				QuestPDF.Infrastructure.SvgImage image = this.Image;
				if (image != null)
				{
					image.Dispose();
				}
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001403E File Offset: 0x0001223E
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
			return SpacePlan.FullRender(Size.Zero);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001406C File Offset: 0x0001226C
		internal override void Draw(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return;
			}
			ValueTuple<float, float> valueTuple = this.Image.SkSvgImage.CalculateSpaceScale(availableSpace);
			float item = valueTuple.Item1;
			float item2 = valueTuple.Item2;
			base.Canvas.Save();
			base.Canvas.Scale(item, item2);
			base.Canvas.DrawSvg(this.Image.SkSvgImage, availableSpace);
			base.Canvas.Restore();
			this.IsRendered = true;
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x000140E1 File Offset: 0x000122E1
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x000140E9 File Offset: 0x000122E9
		private bool IsRendered { get; set; }

		// Token: 0x06000832 RID: 2098 RVA: 0x000140F2 File Offset: 0x000122F2
		public void ResetState(bool hardReset = false)
		{
			this.IsRendered = false;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x000140FB File Offset: 0x000122FB
		public object GetState()
		{
			return this.IsRendered;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00014108 File Offset: 0x00012308
		public void SetState(object state)
		{
			this.IsRendered = (bool)state;
		}
	}
}
