using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000017 RID: 23
	public class PetProfileGUI : GuiDialog
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000089 RID: 137 RVA: 0x0000542C File Offset: 0x0000362C
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005430 File Offset: 0x00003630
		public PetProfileGUI(ICoreClientAPI capi, long targetEntityId) : base(capi)
		{
			this.targetEntityId = targetEntityId;
			Entity targetEntity = capi.World.GetEntityById(targetEntityId);
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			base.SingleComposer = GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(capi.Gui.CreateCompo("PetProfileDialog-", dialogBounds), bgBounds, true, 5.0, 0.75f), Lang.Get("petai:gui-profile-title", Array.Empty<object>()), delegate()
			{
				this.TryClose();
			}, null, null, null).BeginChildElements(bgBounds);
			GuiComposerHelpers.AddStaticText(base.SingleComposer, Lang.Get("petai:gui-profile-name", Array.Empty<object>()), CairoFont.WhiteSmallishText(), ElementBounds.Fixed(0.0, (double)this.currentY, 200.0, 20.0), null);
			this.currentY += 35;
			GuiComposerHelpers.AddTextInput(base.SingleComposer, ElementBounds.Fixed(0.0, (double)this.currentY, 200.0, 40.0), delegate(string name)
			{
				if (!string.IsNullOrEmpty(name) && name.Length > 50)
				{
					name = name.Substring(0, 50);
					GuiComposerHelpers.GetTextInput(base.SingleComposer, "petName").SetValue(name, true);
				}
				this.petName = name;
			}, null, "petName");
			GuiElementEditableTextBase textInput = GuiComposerHelpers.GetTextInput(base.SingleComposer, "petName");
			string text;
			if (targetEntity == null)
			{
				text = null;
			}
			else
			{
				EntityBehaviorNameTag behavior = targetEntity.GetBehavior<EntityBehaviorNameTag>();
				text = ((behavior != null) ? behavior.DisplayName : null);
			}
			textInput.SetValue(text, true);
			this.currentY += 50;
			float? health;
			float? maxhealth;
			this.GetHealthSat(out health, out maxhealth, targetEntity);
			if (health != null && maxhealth != null)
			{
				GuiComposerHelpers.AddStaticText(base.SingleComposer, Lang.Get("petai:gui-profile-currenthealth", new object[]
				{
					health,
					maxhealth
				}), CairoFont.WhiteSmallishText(), ElementBounds.Fixed(0.0, (double)this.currentY, 240.0, 20.0), null);
				this.currentY += 50;
			}
			if (targetEntity != null && targetEntity.HasBehavior<EntityBehaviorMultiply>())
			{
				bool multiply = targetEntity.GetBehavior<EntityBehaviorTameable>().MultiplyAllowed;
				this.multiplyAllowed = multiply;
				GuiComposerHelpers.AddStaticText(base.SingleComposer, Lang.Get("petai:gui-profile-multiply", Array.Empty<object>()), CairoFont.WhiteSmallishText(), ElementBounds.Fixed(0.0, (double)this.currentY, 200.0, 20.0), null);
				GuiComposerHelpers.AddSwitch(base.SingleComposer, delegate(bool value)
				{
					this.multiplyAllowed = value;
				}, ElementBounds.Fixed(150.0, (double)this.currentY, 200.0, 20.0), "multiplyAllowed", 30.0, 4.0);
				GuiComposerHelpers.GetSwitch(base.SingleComposer, "multiplyAllowed").SetValue(multiply);
				this.currentY += 50;
			}
			GuiComposerHelpers.AddStaticText(base.SingleComposer, Lang.Get("petai:gui-profile-abandon", Array.Empty<object>()), CairoFont.WhiteSmallishText(), ElementBounds.Fixed(0.0, (double)this.currentY, 200.0, 20.0), null);
			GuiComposerHelpers.AddSwitch(base.SingleComposer, delegate(bool value)
			{
				this.abandon = value;
			}, ElementBounds.Fixed(150.0, (double)this.currentY, 200.0, 20.0), "abandon", 30.0, 4.0);
			this.currentY += 50;
			GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(base.SingleComposer, Lang.Get("petai:gui-profile-ok", Array.Empty<object>()), () => this.OnClick(), ElementBounds.Fixed(0.0, (double)this.currentY, 90.0, 40.0), 2, null), Lang.Get("petai:gui-profile-cancel", Array.Empty<object>()), () => this.TryClose(), ElementBounds.Fixed(150.0, (double)this.currentY, 90.0, 40.0), 2, null).EndChildElements().Compose(true);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000587C File Offset: 0x00003A7C
		private void GetHealthSat(out float? health, out float? maxHealth, Entity targetEntity)
		{
			health = default(float?);
			maxHealth = default(float?);
			ITreeAttribute healthTree = (targetEntity != null) ? targetEntity.WatchedAttributes.GetTreeAttribute("health") : null;
			if (healthTree != null)
			{
				health = healthTree.TryGetFloat("currenthealth");
				maxHealth = healthTree.TryGetFloat("maxhealth");
			}
			if (health != null)
			{
				health = new float?((float)Math.Round((double)health.Value, 1));
			}
			if (maxHealth != null)
			{
				maxHealth = new float?((float)Math.Round((double)maxHealth.Value, 1));
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005918 File Offset: 0x00003B18
		private bool OnClick()
		{
			PetProfileMessage message = new PetProfileMessage
			{
				petName = this.petName,
				multiplyAllowed = this.multiplyAllowed,
				abandon = this.abandon,
				targetEntityUID = this.targetEntityId
			};
			this.capi.Network.GetChannel("petainetwork").SendPacket<PetProfileMessage>(message);
			this.TryClose();
			return true;
		}

		// Token: 0x04000046 RID: 70
		private readonly long targetEntityId;

		// Token: 0x04000047 RID: 71
		private readonly int currentY = 20;

		// Token: 0x04000048 RID: 72
		private string petName;

		// Token: 0x04000049 RID: 73
		private bool multiplyAllowed = true;

		// Token: 0x0400004A RID: 74
		private bool abandon;
	}
}
