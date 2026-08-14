using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200007A RID: 122
	public class TelephoneMenuDialog : GuiDialogBlockEntity
	{
		// Token: 0x0600038F RID: 911 RVA: 0x00014AC8 File Offset: 0x00012CC8
		public TelephoneMenuDialog(ICoreClientAPI capi, BlockEntityTelephone telephoneBlock) : base(UIUtils.I18n("Telephone.Gui.Title"), telephoneBlock.Pos, capi)
		{
			this.telephoneBlock = telephoneBlock;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00014B1C File Offset: 0x00012D1C
		public override void OnGuiOpened()
		{
			base.OnGuiOpened();
			this.BuildComposer();
			this.pendingNumber = this.telephoneBlock.GetPhoneNumber();
			this.pendingDialNumber = (this.telephoneBlock.GetTargetNumber() ?? "");
			GuiElementTextInput guiElementTextInput = this.numberInput;
			if (guiElementTextInput != null)
			{
				guiElementTextInput.SetValue(this.pendingNumber, true);
			}
			this.RefreshData();
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00014B80 File Offset: 0x00012D80
		private void BuildComposer()
		{
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds statusBounds = ElementBounds.Fixed(0.0, 35.0, 420.0, 18.0);
			ElementBounds numberLabelBounds = ElementBounds.Fixed(0.0, 62.0, 420.0, 18.0);
			ElementBounds numberInputBounds = ElementBounds.Fixed(0.0, 82.0, 320.0, 26.0);
			ElementBounds numberSaveBounds = ElementBounds.Fixed(332.0, 82.0, 88.0, 26.0);
			ElementBounds dialDisplayBounds = ElementBounds.Fixed(0.0, 122.0, 420.0, 22.0);
			ElementBounds key1Bounds = ElementBounds.Fixed(0.0, 170.0, 132.0, 28.0);
			ElementBounds key2Bounds = ElementBounds.Fixed(144.0, 170.0, 132.0, 28.0);
			ElementBounds key3Bounds = ElementBounds.Fixed(288.0, 170.0, 132.0, 28.0);
			ElementBounds key4Bounds = ElementBounds.Fixed(0.0, 204.0, 132.0, 28.0);
			ElementBounds key5Bounds = ElementBounds.Fixed(144.0, 204.0, 132.0, 28.0);
			ElementBounds key6Bounds = ElementBounds.Fixed(288.0, 204.0, 132.0, 28.0);
			ElementBounds key7Bounds = ElementBounds.Fixed(0.0, 238.0, 132.0, 28.0);
			ElementBounds key8Bounds = ElementBounds.Fixed(144.0, 238.0, 132.0, 28.0);
			ElementBounds key9Bounds = ElementBounds.Fixed(288.0, 238.0, 132.0, 28.0);
			ElementBounds keyClearBounds = ElementBounds.Fixed(0.0, 272.0, 132.0, 28.0);
			ElementBounds key0Bounds = ElementBounds.Fixed(144.0, 272.0, 132.0, 28.0);
			ElementBounds keyBackBounds = ElementBounds.Fixed(288.0, 272.0, 132.0, 28.0);
			ElementBounds callButtonBounds = ElementBounds.Fixed(0.0, 308.0, 420.0, 28.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			this.managedBySwitchboard = this.telephoneBlock.IsManagedBySwitchboard();
			this.canEditManagedOptions = (this.managedBySwitchboard && this.telephoneBlock.CanCompose());
			if (this.managedBySwitchboard)
			{
				bgBounds.WithChildren(new ElementBounds[]
				{
					statusBounds,
					numberLabelBounds,
					numberInputBounds,
					numberSaveBounds,
					dialDisplayBounds,
					key1Bounds,
					key2Bounds,
					key3Bounds,
					key4Bounds,
					key5Bounds,
					key6Bounds,
					key7Bounds,
					key8Bounds,
					key9Bounds,
					keyClearBounds,
					key0Bounds,
					keyBackBounds,
					callButtonBounds
				});
			}
			else
			{
				bgBounds.WithChildren(new ElementBounds[]
				{
					statusBounds,
					callButtonBounds
				});
			}
			this.actionButtonLangKey = this.ResolveActionButtonLangKey();
			this.actionButtonInteractive = !this.telephoneBlock.IsWaitingForAnswer();
			GuiComposer composer = GuiElementDynamicTextHelper.AddDynamicText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("telephonemenu", dialogBounds), bgBounds, true, 5.0, 0.75f), UIUtils.I18n("Telephone.Gui.Title"), new Action(this.OnTitleBarCloseClicked), null, null, null), this.ResolveStatusText(), CairoFont.WhiteSmallText(), statusBounds, "telephoneStatusText");
			if (this.managedBySwitchboard)
			{
				if (this.canEditManagedOptions)
				{
					composer = GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddSmallButton(GuiElementDynamicTextHelper.AddDynamicText(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddTextInput(GuiComposerHelpers.AddStaticText(composer, UIUtils.I18n("Telephone.Gui.Number"), CairoFont.WhiteSmallText(), numberLabelBounds, null), numberInputBounds, new Action<string>(this.OnNumberInputChanged), CairoFont.TextInput(), "telephoneNumberInput"), UIUtils.I18n("Telephone.Gui.Save"), new ActionConsumable(this.OnSaveNumberClicked), numberSaveBounds, 2, null), "", CairoFont.WhiteSmallText(), dialDisplayBounds, "telephoneDialNumber"), "1", () => this.OnDialKeyPressed('1'), key1Bounds, 2, null), "2", () => this.OnDialKeyPressed('2'), key2Bounds, 2, null), "3", () => this.OnDialKeyPressed('3'), key3Bounds, 2, null), "4", () => this.OnDialKeyPressed('4'), key4Bounds, 2, null), "5", () => this.OnDialKeyPressed('5'), key5Bounds, 2, null), "6", () => this.OnDialKeyPressed('6'), key6Bounds, 2, null), "7", () => this.OnDialKeyPressed('7'), key7Bounds, 2, null), "8", () => this.OnDialKeyPressed('8'), key8Bounds, 2, null), "9", () => this.OnDialKeyPressed('9'), key9Bounds, 2, null), UIUtils.I18n("Telephone.Gui.Clear"), new ActionConsumable(this.OnDialClearClicked), keyClearBounds, 2, null), "0", () => this.OnDialKeyPressed('0'), key0Bounds, 2, null), UIUtils.I18n("Telephone.Gui.Backspace"), new ActionConsumable(this.OnDialBackspaceClicked), keyBackBounds, 2, null);
				}
				else
				{
					string ownNumber = this.telephoneBlock.GetPhoneNumber();
					composer = GuiComposerHelpers.AddStaticText(composer, UIUtils.I18n("Telephone.Gui.NetworkUnavailable"), CairoFont.WhiteSmallText(), numberInputBounds, null);
					if (!string.IsNullOrWhiteSpace(ownNumber))
					{
						composer = GuiComposerHelpers.AddStaticText(composer, UIUtils.I18n("Telephone.Gui.LocalNumber", new object[]
						{
							ownNumber
						}), CairoFont.WhiteSmallText(), dialDisplayBounds, null);
					}
				}
			}
			GuiComposerHelpers.AddSmallButton(composer, UIUtils.I18n(this.actionButtonLangKey), new ActionConsumable(this.OnCallClicked), callButtonBounds, 2, "telephoneActionButton");
			base.SingleComposer = composer.Compose(true);
			this.numberInput = (base.SingleComposer.GetElement("telephoneNumberInput") as GuiElementTextInput);
			this.statusTextElem = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "telephoneStatusText");
			this.dialNumberTextElem = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "telephoneDialNumber");
			this.actionButtonElem = base.SingleComposer.GetElement("telephoneActionButton");
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000152A4 File Offset: 0x000134A4
		public void RefreshData()
		{
			if (base.SingleComposer == null)
			{
				return;
			}
			if (!this.canEditManagedOptions || this.numberInput == null)
			{
				this.RefreshActionUi();
				return;
			}
			this.RefreshActionUi();
			this.pendingNumber = this.telephoneBlock.GetPhoneNumber();
			this.numberInput.SetValue(this.pendingNumber, true);
			if (string.IsNullOrWhiteSpace(this.pendingDialNumber))
			{
				this.pendingDialNumber = (this.telephoneBlock.GetTargetNumber() ?? "");
			}
			this.UpdateDialDisplay();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00015328 File Offset: 0x00013528
		private void RefreshActionUi()
		{
			GuiElementDynamicText guiElementDynamicText = this.statusTextElem;
			if (guiElementDynamicText != null)
			{
				guiElementDynamicText.SetNewText(this.ResolveStatusText(), false, false, false);
			}
			string desiredButton = this.ResolveActionButtonLangKey();
			bool shouldBeInteractive = !this.telephoneBlock.IsWaitingForAnswer();
			if (desiredButton != this.actionButtonLangKey)
			{
				this.actionButtonLangKey = desiredButton;
				this.TrySetActionButtonText(UIUtils.I18n(this.actionButtonLangKey));
			}
			if (shouldBeInteractive != this.actionButtonInteractive)
			{
				this.actionButtonInteractive = shouldBeInteractive;
				this.TrySetActionButtonEnabled(this.actionButtonInteractive);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000394 RID: 916 RVA: 0x000153A7 File Offset: 0x000135A7
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000153AA File Offset: 0x000135AA
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000153B3 File Offset: 0x000135B3
		public override void OnGuiClosed()
		{
			base.OnGuiClosed();
			if (this.telephoneBlock.IsCallSessionActive())
			{
				this.telephoneBlock.RequestEndCall();
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000153D4 File Offset: 0x000135D4
		private void OnNumberInputChanged(string value)
		{
			if (!this.canEditManagedOptions)
			{
				return;
			}
			if (this.telephoneBlock.IsCallSessionActive())
			{
				return;
			}
			string digits = new string((value ?? "").ToCharArray());
			char[] array = digits.ToCharArray();
			Predicate<char> predicate;
			if ((predicate = TelephoneMenuDialog.<>O.<0>__IsDigit) == null)
			{
				predicate = (TelephoneMenuDialog.<>O.<0>__IsDigit = new Predicate<char>(char.IsDigit));
			}
			this.pendingNumber = new string(Array.FindAll<char>(array, predicate));
			if (this.pendingNumber.Length > 6)
			{
				this.pendingNumber = this.pendingNumber.Substring(0, 6);
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00015460 File Offset: 0x00013660
		private bool OnSaveNumberClicked()
		{
			if (!this.canEditManagedOptions)
			{
				return true;
			}
			if (this.telephoneBlock.IsCallSessionActive())
			{
				return true;
			}
			string[] availableTargetNumbers = this.telephoneBlock.GetAvailableTargetNumbers();
			for (int i = 0; i < availableTargetNumbers.Length; i++)
			{
				if (string.Equals(availableTargetNumbers[i], this.pendingNumber, 5))
				{
					ICoreClientAPI capi = this.capi;
					if (capi != null)
					{
						capi.TriggerIngameError(this, "telephone-number-used", UIUtils.I18n("Telegraph.Settings.NameAlreadyUsed"));
					}
					return true;
				}
			}
			this.telephoneBlock.RequestSavePhoneNumber(this.pendingNumber);
			return true;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000154E8 File Offset: 0x000136E8
		private bool OnDialKeyPressed(char digit)
		{
			if (!this.canEditManagedOptions || this.telephoneBlock.IsCallSessionActive())
			{
				return true;
			}
			if (!char.IsDigit(digit))
			{
				return true;
			}
			string text = this.pendingDialNumber;
			if (((text != null) ? text.Length : 0) >= 6)
			{
				return true;
			}
			ReadOnlySpan<char> readOnlySpan = this.pendingDialNumber;
			char c = digit;
			this.pendingDialNumber = readOnlySpan + new ReadOnlySpan<char>(ref c);
			this.UpdateDialDisplay();
			return true;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00015554 File Offset: 0x00013754
		private bool OnDialBackspaceClicked()
		{
			if (!this.canEditManagedOptions || this.telephoneBlock.IsCallSessionActive())
			{
				return true;
			}
			if (string.IsNullOrEmpty(this.pendingDialNumber))
			{
				return true;
			}
			this.pendingDialNumber = this.pendingDialNumber.Substring(0, this.pendingDialNumber.Length - 1);
			this.UpdateDialDisplay();
			return true;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000155AD File Offset: 0x000137AD
		private bool OnDialClearClicked()
		{
			if (!this.canEditManagedOptions || this.telephoneBlock.IsCallSessionActive())
			{
				return true;
			}
			this.pendingDialNumber = "";
			this.UpdateDialDisplay();
			return true;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000155D8 File Offset: 0x000137D8
		private bool OnCallClicked()
		{
			if (this.telephoneBlock.IsWaitingForAnswer())
			{
				return true;
			}
			if (this.telephoneBlock.IsInCall())
			{
				this.telephoneBlock.RequestEndCall();
				return true;
			}
			if (this.telephoneBlock.HasIncomingCall())
			{
				this.telephoneBlock.RequestStartCall("");
				return true;
			}
			if (this.managedBySwitchboard && this.canEditManagedOptions)
			{
				if (string.IsNullOrWhiteSpace(this.pendingDialNumber))
				{
					ICoreClientAPI capi = this.capi;
					if (capi != null)
					{
						capi.TriggerIngameError(this, "telephone-call-failed", UIUtils.I18n("Telephone.Call.Failed.NoTarget"));
					}
					return true;
				}
				this.telephoneBlock.RequestStartCall(this.pendingDialNumber);
				return true;
			}
			else
			{
				string failureLangKey = this.telephoneBlock.GetCallFailureLangKeyForUi();
				if (!string.IsNullOrWhiteSpace(failureLangKey))
				{
					ICoreClientAPI capi2 = this.capi;
					if (capi2 != null)
					{
						capi2.TriggerIngameError(this, "telephone-call-failed", UIUtils.I18n(failureLangKey));
					}
					return true;
				}
				this.telephoneBlock.RequestStartCall("");
				return true;
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000156C4 File Offset: 0x000138C4
		private void UpdateDialDisplay()
		{
			GuiElementDynamicText guiElementDynamicText = this.dialNumberTextElem;
			if (guiElementDynamicText == null)
			{
				return;
			}
			guiElementDynamicText.SetNewText(UIUtils.I18n("Telephone.Gui.DialDisplay", new object[]
			{
				this.pendingDialNumber ?? ""
			}), false, false, false);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000156FC File Offset: 0x000138FC
		private void TrySetActionButtonText(string text)
		{
			if (this.actionButtonElem == null || string.IsNullOrEmpty(text))
			{
				return;
			}
			Type type = this.actionButtonElem.GetType();
			try
			{
				MethodInfo setNewText = type.GetMethod("SetNewText", 52, null, new Type[]
				{
					typeof(string)
				}, null);
				if (setNewText != null)
				{
					setNewText.Invoke(this.actionButtonElem, new object[]
					{
						text
					});
				}
				else
				{
					MethodInfo setText = type.GetMethod("SetText", 52, null, new Type[]
					{
						typeof(string)
					}, null);
					if (setText != null)
					{
						setText.Invoke(this.actionButtonElem, new object[]
						{
							text
						});
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x000157C4 File Offset: 0x000139C4
		private void TrySetActionButtonEnabled(bool enabled)
		{
			if (this.actionButtonElem == null)
			{
				return;
			}
			Type type = this.actionButtonElem.GetType();
			try
			{
				PropertyInfo enabledProp = type.GetProperty("Enabled", 52);
				if (enabledProp != null && enabledProp.PropertyType == typeof(bool) && enabledProp.CanWrite)
				{
					enabledProp.SetValue(this.actionButtonElem, enabled);
				}
				else
				{
					FieldInfo enabledField = type.GetField("Enabled", 52);
					if (enabledField != null && enabledField.FieldType == typeof(bool))
					{
						enabledField.SetValue(this.actionButtonElem, enabled);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00015888 File Offset: 0x00013A88
		private string ResolveActionButtonLangKey()
		{
			if (this.telephoneBlock.IsInCall())
			{
				return "Telephone.Gui.Hangup";
			}
			if (this.telephoneBlock.IsNotInService())
			{
				return "Telephone.Gui.Hangup";
			}
			if (this.telephoneBlock.HasIncomingCall())
			{
				return "Telephone.Gui.Answer";
			}
			if (this.telephoneBlock.IsWaitingForAnswer())
			{
				return "Telephone.Gui.Waiting";
			}
			return "Telephone.Gui.Call";
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000158E8 File Offset: 0x00013AE8
		private string ResolveStatusText()
		{
			if (this.telephoneBlock.IsInCall())
			{
				return UIUtils.I18n("Telephone.Gui.InCall");
			}
			if (this.telephoneBlock.IsNotInService())
			{
				return UIUtils.I18n("Telephone.Gui.NotInServiceStatus");
			}
			if (this.telephoneBlock.HasIncomingCall())
			{
				return UIUtils.I18n("Telephone.Gui.IncomingCall");
			}
			if (this.telephoneBlock.IsWaitingForAnswer())
			{
				return UIUtils.I18n("Telephone.Gui.WaitingStatus");
			}
			if (!this.managedBySwitchboard)
			{
				return UIUtils.I18n("Telephone.Gui.DirectModeReady");
			}
			if (this.telephoneBlock.CanCompose())
			{
				return "";
			}
			return UIUtils.I18n(this.telephoneBlock.GetComposeDisabledReasonLangKey());
		}

		// Token: 0x040001D4 RID: 468
		private readonly BlockEntityTelephone telephoneBlock;

		// Token: 0x040001D5 RID: 469
		private GuiElementTextInput numberInput;

		// Token: 0x040001D6 RID: 470
		private GuiElementDynamicText statusTextElem;

		// Token: 0x040001D7 RID: 471
		private GuiElementDynamicText dialNumberTextElem;

		// Token: 0x040001D8 RID: 472
		private GuiElement actionButtonElem;

		// Token: 0x040001D9 RID: 473
		private string pendingNumber = "";

		// Token: 0x040001DA RID: 474
		private string pendingDialNumber = "";

		// Token: 0x040001DB RID: 475
		private string actionButtonLangKey = "Telephone.Gui.Call";

		// Token: 0x040001DC RID: 476
		private bool actionButtonInteractive = true;

		// Token: 0x040001DD RID: 477
		private bool managedBySwitchboard;

		// Token: 0x040001DE RID: 478
		private bool canEditManagedOptions;

		// Token: 0x0200010E RID: 270
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000451 RID: 1105
			public static Predicate<char> <0>__IsDigit;
		}
	}
}
