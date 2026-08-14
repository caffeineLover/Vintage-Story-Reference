using System;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000E8 RID: 232
	internal sealed class ScaleToFit : ContainerElement
	{
		// Token: 0x0600078E RID: 1934 RVA: 0x00012714 File Offset: 0x00010914
		internal override SpacePlan Measure(Size availableSpace)
		{
			float? num = this.FindPerfectScale(availableSpace);
			if (num == null)
			{
				return SpacePlan.Wrap("Cannot find the perfect scale to fit the child element in the available space.");
			}
			Size availableSpace2 = ScaleToFit.ScaleSize(availableSpace, 1f / num.Value);
			return SpacePlan.FullRender(ScaleToFit.ScaleSize(base.Measure(availableSpace2), num.Value));
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00012770 File Offset: 0x00010970
		internal override void Draw(Size availableSpace)
		{
			float? num = this.FindPerfectScale(availableSpace);
			if (num == null)
			{
				return;
			}
			float value = num.Value;
			Size availableSpace2 = ScaleToFit.ScaleSize(availableSpace, 1f / value);
			base.Canvas.Scale(value, value);
			Element child = base.Child;
			if (child != null)
			{
				child.Draw(availableSpace2);
			}
			base.Canvas.Scale(1f / value, 1f / value);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x000127DD File Offset: 0x000109DD
		private static Size ScaleSize(Size size, float factor)
		{
			return new Size(size.Width * factor, size.Height * factor);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x000127F4 File Offset: 0x000109F4
		private float? FindPerfectScale(Size availableSpace)
		{
			ScaleToFit.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.availableSpace = availableSpace;
			CS$<>8__locals1.<>4__this = this;
			if (this.<FindPerfectScale>g__ChildFits|3_0(1f, ref CS$<>8__locals1))
			{
				return new float?((float)1);
			}
			float num = 1f;
			float num2 = 0.01f;
			float? result = default(float?);
			foreach (int num3 in Enumerable.Range(0, 8))
			{
				float num4 = (num + num2) / 2f;
				if (this.<FindPerfectScale>g__ChildFits|3_0(num4, ref CS$<>8__locals1))
				{
					num2 = num4;
					result = new float?(num4);
				}
				else
				{
					num = num4;
				}
			}
			return result;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x000128AC File Offset: 0x00010AAC
		[CompilerGenerated]
		private bool <FindPerfectScale>g__ChildFits|3_0(float scale, ref ScaleToFit.<>c__DisplayClass3_0 A_2)
		{
			Size availableSpace = ScaleToFit.ScaleSize(A_2.availableSpace, 1f / scale);
			SpacePlanType type = base.Measure(availableSpace).Type;
			return type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
		}
	}
}
