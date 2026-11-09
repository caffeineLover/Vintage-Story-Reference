using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace PetAI
{
	// Token: 0x02000018 RID: 24
	public class TaskSelectionGui : GuiDialog
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000059E6 File Offset: 0x00003BE6
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000059E9 File Offset: 0x00003BE9
		public TaskSelectionGui(ICoreClientAPI capi, EntityPlayer player, EntityAgent targetEntity = null) : base(capi)
		{
			this.targetEntity = targetEntity;
			this.player = player;
			this.ComposeGui();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005A08 File Offset: 0x00003C08
		public void ComposeGui()
		{
			this.currentY = 20;
			this.currentX = 0;
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			if (this.targetEntity == null || !this.targetEntity.HasBehavior<EntityBehaviorTameable>() || this.targetEntity.GetBehavior<EntityBehaviorReceiveCommand>().AvailableCommands.Keys.Count == 0)
			{
				this.ComposeStaticDialogue(dialogBounds, bgBounds);
				return;
			}
			this.ComposeDynamicDialogue(dialogBounds, bgBounds);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005A8C File Offset: 0x00003C8C
		private void ComposeDynamicDialogue(ElementBounds dialogBounds, ElementBounds bgBounds)
		{
			this.availableCommands = Enumerable.ToList<Command>(this.targetEntity.GetBehavior<EntityBehaviorReceiveCommand>().AvailableCommands.Keys);
			base.SingleComposer = GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("CommandDialog-", dialogBounds), bgBounds, true, 5.0, 0.75f), Lang.Get("petai:gui-command-title", Array.Empty<object>()), delegate()
			{
				this.TryClose();
			}, null, null, null).BeginChildElements(bgBounds);
			this.AddGuiRow(EnumCommandType.SIMPLE, "petai:gui-command-simple");
			this.AddGuiRow(EnumCommandType.COMPLEX, "petai:gui-command-complex");
			this.AddGuiRow(EnumCommandType.AGGRESSIONLEVEL, "petai:gui-command-aggressionlevel");
			GuiComposerHelpers.AddIconButton(base.SingleComposer, "necklace", new Action<bool>(this.OnToggleProfile), ElementBounds.FixedOffseted(9, 0.0, 20.0, 30.0, 30.0), null).EndChildElements().Compose(true);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00005B8C File Offset: 0x00003D8C
		private void AddGuiRow(EnumCommandType type, string headline)
		{
			if (this.availableCommands.Exists((Command command) => command.Type == type))
			{
				GuiComposerHelpers.AddStaticText(base.SingleComposer, Lang.Get(headline, Array.Empty<object>()), CairoFont.WhiteSmallishText(), ElementBounds.Fixed(0.0, (double)this.currentY, 300.0, 20.0), null);
				this.currentY += 35;
				this.currentX = 0;
				List<Command> list = this.availableCommands;
				Predicate<Command> <>9__1;
				Predicate<Command> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((Command command) => command.Type == type));
				}
				using (List<Command>.Enumerator enumerator = list.FindAll(predicate).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Command command = enumerator.Current;
						GuiComposerHelpers.AddButton(base.SingleComposer, Lang.Get(string.Format("petai:gui-command-{0}", command.CommandName.ToLower()), Array.Empty<object>()), () => this.OnCommandClick(command), ElementBounds.Fixed((double)this.currentX, (double)this.currentY, 135.0, 45.0), 2, null);
						this.currentX += 150;
					}
				}
				this.currentY += 50;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005D24 File Offset: 0x00003F24
		private void ComposeStaticDialogue(ElementBounds dialogBounds, ElementBounds bgBounds)
		{
			base.SingleComposer = GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddButton(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("CommandDialog-", dialogBounds), bgBounds, true, 5.0, 0.75f), Lang.Get("petai:gui-command-title", Array.Empty<object>()), delegate()
			{
				this.TryClose();
			}, null, null, null).BeginChildElements(bgBounds), Lang.Get("petai:gui-command-simple", Array.Empty<object>()), CairoFont.WhiteSmallishText(), ElementBounds.Fixed(0.0, 20.0, 300.0, 20.0), null), Lang.Get("petai:gui-command-sit", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.SIMPLE, "sit")), ElementBounds.Fixed(0.0, 50.0, 135.0, 45.0), 2, null), Lang.Get("petai:gui-command-lay", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.SIMPLE, "lay")), ElementBounds.Fixed(150.0, 50.0, 135.0, 45.0), 2, null), Lang.Get("petai:gui-command-speak", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.SIMPLE, "speak")), ElementBounds.Fixed(300.0, 50.0, 135.0, 45.0), 2, null), Lang.Get("petai:gui-command-complex", Array.Empty<object>()), CairoFont.WhiteSmallishText(), ElementBounds.Fixed(0.0, 100.0, 300.0, 20.0), null), Lang.Get("petai:gui-command-followmaster", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.COMPLEX, "followmaster")), ElementBounds.Fixed(0.0, 135.0, 135.0, 45.0), 2, null), Lang.Get("petai:gui-command-stay", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.COMPLEX, "stay")), ElementBounds.Fixed(150.0, 135.0, 135.0, 45.0), 2, null), Lang.Get("petai:gui-command-aggressionlevel", Array.Empty<object>()), CairoFont.WhiteSmallishText(), ElementBounds.Fixed(0.0, 185.0, 300.0, 20.0), null), Lang.Get("petai:gui-command-neutral", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.AGGRESSIONLEVEL, EnumAggressionLevel.NEUTRAL.ToString())), ElementBounds.Fixed(0.0, 220.0, 135.0, 45.0), 2, null), Lang.Get("petai:gui-command-protective", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.AGGRESSIONLEVEL, EnumAggressionLevel.PROTECTIVE.ToString())), ElementBounds.Fixed(150.0, 220.0, 135.0, 45.0), 2, null), Lang.Get("petai:gui-command-aggressive", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.AGGRESSIONLEVEL, EnumAggressionLevel.AGGRESSIVE.ToString())), ElementBounds.Fixed(300.0, 220.0, 135.0, 45.0), 2, null), Lang.Get("petai:gui-command-passive", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.AGGRESSIONLEVEL, EnumAggressionLevel.PASSIVE.ToString())), ElementBounds.Fixed(450.0, 220.0, 135.0, 45.0), 2, null), Lang.Get("petai:gui-command-attackorder", Array.Empty<object>()), CairoFont.WhiteSmallishText(), ElementBounds.Fixed(0.0, 270.0, 300.0, 20.0), null), Lang.Get("petai:gui-command-settarget", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.ATTACKORDER, "settarget")), ElementBounds.Fixed(0.0, 305.0, 135.0, 45.0), 2, null), Lang.Get("petai:gui-command-removetarget", Array.Empty<object>()), () => this.OnCommandClick(new Command(EnumCommandType.ATTACKORDER, "removetarget")), ElementBounds.Fixed(150.0, 305.0, 135.0, 45.0), 2, null).EndChildElements().Compose(true);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000061DC File Offset: 0x000043DC
		private bool OnCommandClick(Command command)
		{
			PetCommandMessage message = new PetCommandMessage
			{
				commandName = command.CommandName,
				commandType = command.Type.ToString(),
				playerUID = this.player.PlayerUID
			};
			if (this.targetEntity != null)
			{
				message.targetEntityUID = this.targetEntity.EntityId;
			}
			if (this.targetEntity != null && this.targetEntity.HasBehavior<EntityBehaviorTameable>() && command.CommandName != "dropgear" && this.targetEntity.GetBehavior<EntityBehaviorReceiveCommand>().AvailableCommands[command] > this.targetEntity.GetBehavior<EntityBehaviorTameable>().Obedience)
			{
				this.capi.ShowChatMessage(Lang.Get("petai:gui-pet-disobey", new object[]
				{
					this.targetEntity.GetBehavior<EntityBehaviorReceiveCommand>().AvailableCommands[command] * 100f
				}));
				return true;
			}
			this.TryClose();
			this.capi.Network.GetChannel("petainetwork").SendPacket<PetCommandMessage>(message);
			return true;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000062F5 File Offset: 0x000044F5
		private void OnToggleProfile(bool value)
		{
			PetProfileGUI petProfileGUI = new PetProfileGUI(this.capi, this.targetEntity.EntityId);
			petProfileGUI.TryClose();
			petProfileGUI.TryOpen();
			this.TryClose();
		}

		// Token: 0x0400004B RID: 75
		private readonly EntityAgent targetEntity;

		// Token: 0x0400004C RID: 76
		private readonly EntityPlayer player;

		// Token: 0x0400004D RID: 77
		private int currentX;

		// Token: 0x0400004E RID: 78
		private int currentY;

		// Token: 0x0400004F RID: 79
		private List<Command> availableCommands;
	}
}
