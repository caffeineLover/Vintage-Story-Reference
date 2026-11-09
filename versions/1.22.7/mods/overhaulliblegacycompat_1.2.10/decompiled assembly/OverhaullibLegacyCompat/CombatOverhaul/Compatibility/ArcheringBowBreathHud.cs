using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x02000161 RID: 353
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ArcheringBowBreathHud : HudElement
	{
		// Token: 0x060011BD RID: 4541 RVA: 0x00054CAC File Offset: 0x00052EAC
		public ArcheringBowBreathHud(ICoreClientAPI capi) : base(capi)
		{
			ElementBounds dialogBounds = ElementBounds.Fixed(7, 0.0, -150.0, 200.0, 10.0);
			ElementBounds barBounds = ElementBounds.Fixed(0.0, 0.0, 200.0, 10.0);
			base.SingleComposer = GuiComposerHelpers.AddStatbar(capi.Gui.CreateCompo("combatOverhaulArcheringBreathHud", dialogBounds), barBounds, this._breathColor, "breathBar").Compose(true);
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060011BE RID: 4542 RVA: 0x00054D5A File Offset: 0x00052F5A
		public override bool Focusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00054D5D File Offset: 0x00052F5D
		public override bool ShouldReceiveMouseEvents()
		{
			return false;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00054D60 File Offset: 0x00052F60
		public override bool ShouldReceiveKeyboardEvents()
		{
			return false;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00054D64 File Offset: 0x00052F64
		public void UpdateBar(float currentBreath, float maxBreath, bool isRecovering, bool visible)
		{
			this._visible = visible;
			if (isRecovering)
			{
				this._breathColor[0] = 0.8;
				this._breathColor[1] = 0.2;
				this._breathColor[2] = 0.2;
				this._breathColor[3] = 0.8;
			}
			else
			{
				this._breathColor[0] = 0.2;
				this._breathColor[1] = 0.7;
				this._breathColor[2] = 0.8;
				this._breathColor[3] = 0.8;
			}
			GuiElementStatbar statbar = GuiComposerHelpers.GetStatbar(base.SingleComposer, "breathBar");
			if (statbar == null)
			{
				return;
			}
			statbar.SetValues(currentBreath, 0f, maxBreath);
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00054E27 File Offset: 0x00053027
		public override void OnRenderGUI(float deltaTime)
		{
			if (!this._visible)
			{
				return;
			}
			base.OnRenderGUI(deltaTime);
		}

		// Token: 0x0400083B RID: 2107
		private readonly double[] _breathColor = new double[]
		{
			0.2,
			0.7,
			0.8,
			0.8
		};

		// Token: 0x0400083C RID: 2108
		private bool _visible;
	}
}
