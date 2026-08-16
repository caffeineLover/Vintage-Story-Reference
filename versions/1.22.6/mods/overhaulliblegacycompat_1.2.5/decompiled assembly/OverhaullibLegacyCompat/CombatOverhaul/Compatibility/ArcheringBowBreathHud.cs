using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x02000155 RID: 341
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ArcheringBowBreathHud : HudElement
	{
		// Token: 0x06001149 RID: 4425 RVA: 0x000533A4 File Offset: 0x000515A4
		public ArcheringBowBreathHud(ICoreClientAPI capi) : base(capi)
		{
			ElementBounds dialogBounds = ElementBounds.Fixed(7, 0.0, -150.0, 200.0, 10.0);
			ElementBounds barBounds = ElementBounds.Fixed(0.0, 0.0, 200.0, 10.0);
			base.SingleComposer = GuiComposerHelpers.AddStatbar(capi.Gui.CreateCompo("combatOverhaulArcheringBreathHud", dialogBounds), barBounds, this._breathColor, "breathBar").Compose(true);
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x00053452 File Offset: 0x00051652
		public override bool Focusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00053455 File Offset: 0x00051655
		public override bool ShouldReceiveMouseEvents()
		{
			return false;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00053458 File Offset: 0x00051658
		public override bool ShouldReceiveKeyboardEvents()
		{
			return false;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0005345C File Offset: 0x0005165C
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

		// Token: 0x0600114E RID: 4430 RVA: 0x0005351F File Offset: 0x0005171F
		public override void OnRenderGUI(float deltaTime)
		{
			if (!this._visible)
			{
				return;
			}
			base.OnRenderGUI(deltaTime);
		}

		// Token: 0x04000815 RID: 2069
		private readonly double[] _breathColor = new double[]
		{
			0.2,
			0.7,
			0.8,
			0.8
		};

		// Token: 0x04000816 RID: 2070
		private bool _visible;
	}
}
