using System;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Gui.CustomElements
{
	// Token: 0x020000A5 RID: 165
	public sealed class LinearGauge
	{
		// Token: 0x060004BF RID: 1215 RVA: 0x0001C120 File Offset: 0x0001A320
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x0001C179 File Offset: 0x0001A379
		public GuiElementStatbar Element
		{
			get
			{
				return this.bar;
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001C184 File Offset: 0x0001A384
		public void SetRatio01(float ratio)
		{
			int v = (int)GameMath.Clamp(ratio * 100f, 0f, 100f);
			this.bar.SetValue((float)v);
		}

		// Token: 0x04000282 RID: 642
		private const int Steps = 100;

		// Token: 0x04000283 RID: 643
		private readonly GuiElementStatbar bar;
	}
}
