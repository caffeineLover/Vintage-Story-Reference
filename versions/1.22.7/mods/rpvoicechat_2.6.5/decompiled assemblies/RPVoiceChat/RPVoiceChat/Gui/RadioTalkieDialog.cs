using System;
using RPVoiceChat.GameContent.Items;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200009F RID: 159
	public class RadioTalkieDialog : GuiDialog
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x00018F6D File Offset: 0x0001716D
		public RadioTalkieDialog(ICoreClientAPI capi, ItemSlot talkieSlot) : base(capi)
		{
			this.BindTalkieSlot(talkieSlot);
			RadioTalkieDialog.currentDialog = this;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00018F8E File Offset: 0x0001718E
		public static void CloseIfOpen()
		{
			if (RadioTalkieDialog.currentDialog != null && RadioTalkieDialog.currentDialog.IsOpened())
			{
				RadioTalkieDialog.currentDialog.TryClose();
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00018FB0 File Offset: 0x000171B0
		public static void OpenOrFocus(ICoreClientAPI capi, ItemSlot talkieSlot)
		{
			bool flag;
			if (capi == null)
			{
				flag = true;
			}
			else
			{
				IClientWorldAccessor world = capi.World;
				bool? flag2;
				if (world == null)
				{
					flag2 = default(bool?);
				}
				else
				{
					IClientPlayer player = world.Player;
					if (player == null)
					{
						flag2 = default(bool?);
					}
					else
					{
						EntityPlayer entity = player.Entity;
						if (entity == null)
						{
							flag2 = default(bool?);
						}
						else
						{
							EntityControls controls = entity.Controls;
							flag2 = ((controls != null) ? new bool?(controls.Sneak) : default(bool?));
						}
					}
				}
				bool? flag3 = flag2;
				flag = !flag3.GetValueOrDefault();
			}
			if (flag)
			{
				return;
			}
			if (RadioTalkieDialog.currentDialog == null)
			{
				RadioTalkieDialog.currentDialog = new RadioTalkieDialog(capi, talkieSlot);
			}
			else
			{
				RadioTalkieDialog.currentDialog.BindTalkieSlot(talkieSlot);
			}
			if (!RadioTalkieDialog.currentDialog.IsOpened())
			{
				RadioTalkieDialog.currentDialog.TryOpen();
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00019063 File Offset: 0x00017263
		public void BindTalkieSlot(ItemSlot slot)
		{
			this.talkieSlot = slot;
			ItemSlot itemSlot = this.talkieSlot;
			if (((itemSlot != null) ? itemSlot.Itemstack : null) != null)
			{
				this.pendingFrequency = ItemRadio.GetTunedFrequency(this.talkieSlot.Itemstack);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00019096 File Offset: 0x00017296
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00019099 File Offset: 0x00017299
		public override void OnGuiOpened()
		{
			ItemSlot itemSlot = this.talkieSlot;
			if (((itemSlot != null) ? itemSlot.Itemstack : null) != null)
			{
				this.pendingFrequency = ItemRadio.GetTunedFrequency(this.talkieSlot.Itemstack);
			}
			this.BuildComposer();
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000190CB File Offset: 0x000172CB
		public override void OnGuiClosed()
		{
			this.CancelPendingSettingsSync();
			this.ReleaseComposer();
			if (RadioTalkieDialog.currentDialog == this)
			{
				RadioTalkieDialog.currentDialog = null;
			}
			base.OnGuiClosed();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000190ED File Offset: 0x000172ED
		public override void Dispose()
		{
			this.CancelPendingSettingsSync();
			this.ReleaseComposer();
			if (RadioTalkieDialog.currentDialog == this)
			{
				RadioTalkieDialog.currentDialog = null;
			}
			base.Dispose();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001910F File Offset: 0x0001730F
		private void ReleaseComposer()
		{
			GuiComposer singleComposer = base.SingleComposer;
			if (singleComposer != null)
			{
				singleComposer.Dispose();
			}
			this.frequencyInput = null;
			this.listenVolumeSlider = null;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00019130 File Offset: 0x00017330
		private void BuildComposer()
		{
			this.ReleaseComposer();
			ItemSlot itemSlot = this.talkieSlot;
			int listenVolume = ItemRadio.GetListenVolumePercent((itemSlot != null) ? itemSlot.Itemstack : null);
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds labelBounds = ElementBounds.Fixed(0.0, 35.0, 320.0, 18.0);
			ElementBounds inputBounds = ElementBounds.Fixed(0.0, 55.0, 220.0, 26.0);
			ElementBounds saveBounds = ElementBounds.Fixed(232.0, 55.0, 88.0, 26.0);
			ElementBounds inventoryListenLabelBounds = ElementBounds.Fixed(0.0, 88.0, 280.0, 18.0);
			ElementBounds inventoryListenToggleBounds = ElementBounds.Fixed(288.0, 84.0, 32.0, 24.0);
			ElementBounds volumeLabelBounds = ElementBounds.Fixed(0.0, 120.0, 320.0, 18.0);
			ElementBounds volumeSliderBounds = ElementBounds.Fixed(0.0, 140.0, 320.0, 26.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			bgBounds.WithChildren(new ElementBounds[]
			{
				labelBounds,
				inputBounds,
				saveBounds,
				inventoryListenLabelBounds,
				inventoryListenToggleBounds,
				volumeLabelBounds,
				volumeSliderBounds
			});
			this.listenVolumeSlider = new NamedSlider(this.capi, "radioTalkieListenVolumeSlider", new ActionConsumable<int, string>(this.OnListenVolumeChanged), volumeSliderBounds);
			this.listenVolumeSlider.SetValues(listenVolume, 0, 100, 1, "%");
			base.SingleComposer = GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSwitch(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddTextInput(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("radiotalkie", dialogBounds), bgBounds, true, 5.0, 0.75f), UIUtils.I18n("Radio.Talkie.Gui.Title"), new Action(this.OnTitleBarCloseClicked), null, null, null), UIUtils.I18n("Radio.Talkie.Gui.Frequency"), CairoFont.WhiteSmallText(), labelBounds, null), inputBounds, delegate(string value)
			{
				this.pendingFrequency = (value ?? "");
			}, CairoFont.TextInput(), "radioTalkieFrequencyInput"), UIUtils.I18n("Radio.Gui.Save"), new ActionConsumable(this.OnSaveFrequencyClicked), saveBounds, 2, null), UIUtils.I18n("Radio.Talkie.Gui.InventoryListen"), CairoFont.WhiteSmallText(), inventoryListenLabelBounds, null), new Action<bool>(this.OnInventoryListenToggled), inventoryListenToggleBounds, "radioTalkieInventoryListen", 24.0, 4.0), UIUtils.I18n("Radio.Talkie.Gui.ListenVolume"), CairoFont.WhiteSmallText(), volumeLabelBounds, null).AddInteractiveElement(this.listenVolumeSlider, "radioTalkieListenVolumeSlider").Compose(true);
			this.frequencyInput = GuiComposerHelpers.GetTextInput(base.SingleComposer, "radioTalkieFrequencyInput");
			GuiElementTextInput guiElementTextInput = this.frequencyInput;
			if (guiElementTextInput != null)
			{
				guiElementTextInput.SetValue(this.pendingFrequency, true);
			}
			GuiElementSwitch inventoryListenToggle = GuiComposerHelpers.GetSwitch(base.SingleComposer, "radioTalkieInventoryListen");
			if (inventoryListenToggle != null)
			{
				GuiElementSwitch guiElementSwitch = inventoryListenToggle;
				ItemSlot itemSlot2 = this.talkieSlot;
				guiElementSwitch.On = ItemRadio.GetInventoryListen((itemSlot2 != null) ? itemSlot2.Itemstack : null);
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00019484 File Offset: 0x00017684
		private void OnInventoryListenToggled(bool on)
		{
			ItemSlot itemSlot = this.talkieSlot;
			if (((itemSlot != null) ? itemSlot.Itemstack : null) == null)
			{
				return;
			}
			ItemRadio.SetInventoryListen(this.talkieSlot.Itemstack, on);
			ItemRadio.SendTalkieSettings(this.capi, this.talkieSlot);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000194BD File Offset: 0x000176BD
		private bool OnListenVolumeChanged(int value, string _)
		{
			ItemSlot itemSlot = this.talkieSlot;
			if (((itemSlot != null) ? itemSlot.Itemstack : null) == null)
			{
				return true;
			}
			ItemRadio.SetListenVolumePercent(this.talkieSlot.Itemstack, value);
			this.QueueSettingsSync();
			return true;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000194F0 File Offset: 0x000176F0
		private bool OnSaveFrequencyClicked()
		{
			ItemSlot itemSlot = this.talkieSlot;
			if (((itemSlot != null) ? itemSlot.Itemstack : null) != null)
			{
				ItemRadio.SetTunedFrequency(this.talkieSlot.Itemstack, this.pendingFrequency);
				this.talkieSlot.MarkDirty();
				ItemRadio.SendTalkieSettings(this.capi, this.talkieSlot);
				this.capi.World.Player.InventoryManager.BroadcastHotbarSlot();
			}
			return true;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001955E File Offset: 0x0001775E
		private void QueueSettingsSync()
		{
			this.CancelPendingSettingsSync();
			this.pendingSettingsSyncId = this.capi.Event.RegisterCallback(delegate(float _)
			{
				this.pendingSettingsSyncId = 0L;
				ItemSlot itemSlot = this.talkieSlot;
				if (((itemSlot != null) ? itemSlot.Itemstack : null) != null)
				{
					ItemRadio.SendTalkieSettings(this.capi, this.talkieSlot);
				}
			}, 250);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001958D File Offset: 0x0001778D
		private void CancelPendingSettingsSync()
		{
			if (this.pendingSettingsSyncId != 0L)
			{
				this.capi.Event.UnregisterCallback(this.pendingSettingsSyncId);
				this.pendingSettingsSyncId = 0L;
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000195B5 File Offset: 0x000177B5
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x04000255 RID: 597
		private const string VolumeSliderKey = "radioTalkieListenVolumeSlider";

		// Token: 0x04000256 RID: 598
		private const int SettingsSyncDebounceMs = 250;

		// Token: 0x04000257 RID: 599
		private static RadioTalkieDialog currentDialog;

		// Token: 0x04000258 RID: 600
		private ItemSlot talkieSlot;

		// Token: 0x04000259 RID: 601
		private GuiElementTextInput frequencyInput;

		// Token: 0x0400025A RID: 602
		private NamedSlider listenVolumeSlider;

		// Token: 0x0400025B RID: 603
		private string pendingFrequency = "";

		// Token: 0x0400025C RID: 604
		private long pendingSettingsSyncId;
	}
}
