using System;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Gui.CustomElements
{
	// Token: 0x0200007C RID: 124
	public sealed class LinearGauge
	{
		// Token: 0x060003B9 RID: 953 RVA: 0x0001608C File Offset: 0x0001428C
		public LinearGauge(ICoreClientAPI capi, ElementBounds bounds, bool vertical = false)
		{
			this.bar = new GuiElementStatbar(capi, bounds, new double[]
			{
				0.1,
				0.4,
				0.1
			}, false, vertical);
			this.bar.ShowValueOnHover = false;
			this.bar.SetValues(0f, 0f, 100f);
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060003BA RID: 954 RVA: 0x000160E5 File Offset: 0x000142E5
		public GuiElementStatbar Element
		{
			get
			{
				return this.bar;
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000160F0 File Offset: 0x000142F0
		public void SetRatio01(float ratio)
		{
			int v = (int)GameMath.Clamp(ratio * 100f, 0f, 100f);
			this.bar.SetValue((float)v);
		}

		// Token: 0x040001E7 RID: 487
		private const int Steps = 100;

		// Token: 0x040001E8 RID: 488
		private readonly GuiElementStatbar bar;
	}
}
