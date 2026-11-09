using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x020000A3 RID: 163
	public class TelephoneMenuDialog : GuiDialogBlockEntity
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x0001AB5C File Offset: 0x00018D5C
		public TelephoneMenuDialog(ICoreClientAPI capi, BlockEntityTelephone telephoneBlock) : base(UIUtils.I18n("Telephone.Gui.Title"), telephoneBlock.Pos, capi)
		{
			this.telephoneBlock = telephoneBlock;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001ABB0 File Offset: 0x00018DB0
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

		// Token: 0x06000497 RID: 1175 RVA: 0x0001AC14 File Offset: 0x00018E14
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

		// Token: 0x06000498 RID: 1176 RVA: 0x0001B338 File Offset: 0x00019538
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

		// Token: 0x06000499 RID: 1177 RVA: 0x0001B3BC File Offset: 0x000195BC
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001B43B File Offset: 0x0001963B
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001B43E File Offset: 0x0001963E
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001B447 File Offset: 0x00019647
		public override void OnGuiClosed()
		{
			base.OnGuiClosed();
			if (this.telephoneBlock.IsCallSessionActive())
			{
				this.telephoneBlock.RequestEndCall();
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001B468 File Offset: 0x00019668
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

		// Token: 0x0600049E RID: 1182 RVA: 0x0001B4F4 File Offset: 0x000196F4
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

		// Token: 0x0600049F RID: 1183 RVA: 0x0001B57C File Offset: 0x0001977C
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

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001B5E8 File Offset: 0x000197E8
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

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001B641 File Offset: 0x00019841
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

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001B66C File Offset: 0x0001986C
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

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001B758 File Offset: 0x00019958
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

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001B790 File Offset: 0x00019990
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

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001B858 File Offset: 0x00019A58
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

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001B91C File Offset: 0x00019B1C
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

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001B97C File Offset: 0x00019B7C
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

		// Token: 0x0400026F RID: 623
		private readonly BlockEntityTelephone telephoneBlock;

		// Token: 0x04000270 RID: 624
		private GuiElementTextInput numberInput;

		// Token: 0x04000271 RID: 625
		private GuiElementDynamicText statusTextElem;

		// Token: 0x04000272 RID: 626
		private GuiElementDynamicText dialNumberTextElem;

		// Token: 0x04000273 RID: 627
		private GuiElement actionButtonElem;

		// Token: 0x04000274 RID: 628
		private string pendingNumber = "";

		// Token: 0x04000275 RID: 629
		private string pendingDialNumber = "";

		// Token: 0x04000276 RID: 630
		private string actionButtonLangKey = "Telephone.Gui.Call";

		// Token: 0x04000277 RID: 631
		private bool actionButtonInteractive = true;

		// Token: 0x04000278 RID: 632
		private bool managedBySwitchboard;

		// Token: 0x04000279 RID: 633
		private bool canEditManagedOptions;

		// Token: 0x02000160 RID: 352
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040005B1 RID: 1457
			public static Predicate<char> <0>__IsDigit;
		}
	}
}
