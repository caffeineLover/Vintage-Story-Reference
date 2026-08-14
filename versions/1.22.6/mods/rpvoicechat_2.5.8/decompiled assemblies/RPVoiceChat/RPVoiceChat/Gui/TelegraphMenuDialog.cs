using System;
using System.Collections.Generic;
using System.Reflection;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000079 RID: 121
	public class TelegraphMenuDialog : GuiDialogBlockEntity
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00013C5D File Offset: 0x00011E5D
		private int MinDelayBetweenKeysMs
		{
			get
			{
				return ServerConfigManager.TelegraphMinDelayBetweenKeysMs;
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00013C64 File Offset: 0x00011E64
		public TelegraphMenuDialog(ICoreClientAPI capi, BlockEntityTelegraph telegraphBlock) : base(UIUtils.I18n("Telegraph.Gui.Title"), telegraphBlock.Pos, capi)
		{
			this.telegraphBlock = telegraphBlock;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00013C90 File Offset: 0x00011E90
		public override void OnGuiOpened()
		{
			base.OnGuiOpened();
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds sentTextBounds = ElementBounds.Fixed(0.0, 40.0, 420.0, 30.0);
			ElementBounds receivedTextBounds = ElementBounds.Fixed(0.0, 80.0, 420.0, 30.0);
			ElementBounds countdownTextBounds = ElementBounds.Fixed(0.0, 120.0, 420.0, 30.0);
			ElementBounds sentCountdownTextBounds = ElementBounds.Fixed(0.0, 160.0, 420.0, 30.0);
			ElementBounds endpointNameLabelBounds = ElementBounds.Fixed(0.0, 200.0, 420.0, 20.0);
			ElementBounds endpointNameInputBounds = ElementBounds.Fixed(0.0, 225.0, 310.0, 26.0);
			ElementBounds endpointNameSaveBounds = ElementBounds.Fixed(322.0, 225.0, 98.0, 26.0);
			ElementBounds endpointTargetLabelBounds = ElementBounds.Fixed(0.0, 260.0, 420.0, 20.0);
			ElementBounds endpointTargetDropdownBounds = ElementBounds.Fixed(0.0, 285.0, 420.0, 26.0);
			ElementBounds noPowerInfoBounds = ElementBounds.Fixed(0.0, 320.0, 420.0, 24.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			List<ElementBounds> list = new List<ElementBounds>();
			list.Add(sentTextBounds);
			list.Add(receivedTextBounds);
			list.Add(countdownTextBounds);
			list.Add(sentCountdownTextBounds);
			List<ElementBounds> allBounds = list;
			bool managedBySwitchboard = this.telegraphBlock.IsManagedBySwitchboard();
			if (managedBySwitchboard)
			{
				allBounds.Add(endpointNameLabelBounds);
				allBounds.Add(endpointNameInputBounds);
				allBounds.Add(endpointNameSaveBounds);
				allBounds.Add(endpointTargetLabelBounds);
				allBounds.Add(endpointTargetDropdownBounds);
				if (!this.telegraphBlock.HasAdvancedRoutingEnabled())
				{
					allBounds.Add(noPowerInfoBounds);
				}
			}
			bgBounds.WithChildren(allBounds.ToArray());
			GuiComposer composer = GuiElementDynamicTextHelper.AddDynamicText(GuiElementDynamicTextHelper.AddDynamicText(GuiElementDynamicTextHelper.AddDynamicText(GuiElementDynamicTextHelper.AddDynamicText(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("telegraphmenu", dialogBounds), bgBounds, true, 5.0, 0.75f), UIUtils.I18n("Telegraph.Gui.Title"), new Action(this.OnTitleBarCloseClicked), null, null, null), UIUtils.I18n("Telegraph.Gui.Sent", new object[]
			{
				' '
			}), CairoFont.WhiteSmallText(), sentTextBounds, "sentText"), UIUtils.I18n("Telegraph.Gui.Received", new object[]
			{
				' '
			}), CairoFont.WhiteSmallText(), receivedTextBounds, "receivedText"), "", CairoFont.WhiteSmallText(), countdownTextBounds, "countdownText"), "", CairoFont.WhiteSmallText(), sentCountdownTextBounds, "sentCountdownText");
			if (managedBySwitchboard)
			{
				if (this.telegraphBlock.HasAdvancedRoutingEnabled())
				{
					string[] endpointValues = this.BuildEndpointValues();
					string[] endpointNames = this.BuildEndpointNames(endpointValues);
					int selectedIndex = TelegraphMenuDialog.ResolveSelectedTargetIndex(endpointValues, this.telegraphBlock.GetTargetEndpointName());
					composer = GuiComposerHelpers.AddDropDown(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddSmallButton(GuiComposerHelpers.AddTextInput(GuiComposerHelpers.AddStaticText(composer, UIUtils.I18n("Telegraph.Gui.EndpointName"), CairoFont.WhiteSmallText(), endpointNameLabelBounds, null), endpointNameInputBounds, new Action<string>(this.OnEndpointNameInputChanged), CairoFont.TextInput(), "endpointNameInput"), UIUtils.I18n("Telegraph.Gui.Save"), new ActionConsumable(this.OnSaveEndpointNameClicked), endpointNameSaveBounds, 2, null), UIUtils.I18n("Telegraph.Gui.Target"), CairoFont.WhiteSmallText(), endpointTargetLabelBounds, null), endpointValues, endpointNames, selectedIndex, new SelectionChangedDelegate(this.OnTargetSelected), endpointTargetDropdownBounds, "endpointTargetDropdown");
				}
				else
				{
					composer = GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddStaticText(GuiComposerHelpers.AddStaticText(composer, UIUtils.I18n("Telegraph.Gui.EndpointReadOnly", new object[]
					{
						this.telegraphBlock.GetCustomEndpointName()
					}), CairoFont.WhiteSmallText(), endpointNameInputBounds, null), UIUtils.I18n("Telegraph.Gui.TargetReadOnly", new object[]
					{
						UIUtils.I18n("Telegraph.Gui.TargetAll")
					}), CairoFont.WhiteSmallText(), endpointTargetLabelBounds, null), UIUtils.I18n(this.telegraphBlock.GetRoutingDisabledReasonLangKey()), CairoFont.WhiteSmallText(), noPowerInfoBounds, null);
				}
			}
			base.SingleComposer = composer.Compose(true);
			this.sentTextElem = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "sentText");
			this.receivedTextElem = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "receivedText");
			this.countdownTextElem = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "countdownText");
			this.sentCountdownTextElem = GuiElementDynamicTextHelper.GetDynamicText(base.SingleComposer, "sentCountdownText");
			this.UpdateSentText(this.telegraphBlock.GetSentMessage());
			this.UpdateReceivedText(this.telegraphBlock.GetReceivedMessage());
			this.RefreshRoutingControls();
			this.lastCanEditRouting = new bool?(this.telegraphBlock.HasAdvancedRoutingEnabled());
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000141AA File Offset: 0x000123AA
		private void OnTitleBarCloseClicked()
		{
			this.TryClose();
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000141B3 File Offset: 0x000123B3
		public void UpdateSentText(string text)
		{
			GuiElementDynamicText guiElementDynamicText = this.sentTextElem;
			if (guiElementDynamicText == null)
			{
				return;
			}
			guiElementDynamicText.SetNewText(UIUtils.I18n("Telegraph.Gui.Sent", new object[]
			{
				text
			}), false, false, false);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000141DC File Offset: 0x000123DC
		public void UpdateReceivedText(string text)
		{
			GuiElementDynamicText guiElementDynamicText = this.receivedTextElem;
			if (guiElementDynamicText == null)
			{
				return;
			}
			guiElementDynamicText.SetNewText(UIUtils.I18n("Telegraph.Gui.Received", new object[]
			{
				text
			}), false, false, false);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00014208 File Offset: 0x00012408
		public void UpdateCountdown(int seconds)
		{
			if (seconds > 0)
			{
				GuiElementDynamicText guiElementDynamicText = this.countdownTextElem;
				if (guiElementDynamicText == null)
				{
					return;
				}
				guiElementDynamicText.SetNewText(UIUtils.I18n("Telegraph.Gui.ReceivedCountdown", new object[]
				{
					seconds
				}), false, false, false);
				return;
			}
			else
			{
				GuiElementDynamicText guiElementDynamicText2 = this.countdownTextElem;
				if (guiElementDynamicText2 == null)
				{
					return;
				}
				guiElementDynamicText2.SetNewText("", false, false, false);
				return;
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00014260 File Offset: 0x00012460
		public void UpdateSentCountdown(int seconds)
		{
			if (seconds > 0)
			{
				GuiElementDynamicText guiElementDynamicText = this.sentCountdownTextElem;
				if (guiElementDynamicText == null)
				{
					return;
				}
				guiElementDynamicText.SetNewText(UIUtils.I18n("Telegraph.Gui.SentCountdown", new object[]
				{
					seconds
				}), false, false, false);
				return;
			}
			else
			{
				GuiElementDynamicText guiElementDynamicText2 = this.sentCountdownTextElem;
				if (guiElementDynamicText2 == null)
				{
					return;
				}
				guiElementDynamicText2.SetNewText("", false, false, false);
				return;
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000142B6 File Offset: 0x000124B6
		public override void OnKeyDown(KeyEvent args)
		{
			if (args.KeyCode == 50)
			{
				this.TryClose();
				return;
			}
			base.OnKeyDown(args);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000142D4 File Offset: 0x000124D4
		public override void OnKeyPress(KeyEvent args)
		{
			if (this.IsEndpointInputFocused())
			{
				if (!this.telegraphBlock.HasAdvancedRoutingEnabled())
				{
					return;
				}
				this.HandleEndpointInputKey(args);
				return;
			}
			else
			{
				if (this.telegraphBlock.IsPlaying)
				{
					return;
				}
				long nowMs = this.capi.World.ElapsedMilliseconds;
				if (nowMs - this.lastKeySentMs < (long)this.MinDelayBetweenKeysMs)
				{
					return;
				}
				if (args.KeyChar != '\0')
				{
					this.telegraphBlock.SendSignal(args.KeyChar);
					this.lastKeySentMs = nowMs;
				}
				return;
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00014350 File Offset: 0x00012550
		public override bool CaptureAllInputs()
		{
			return true;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00014353 File Offset: 0x00012553
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00014358 File Offset: 0x00012558
		public override void OnRenderGUI(float deltaTime)
		{
			base.OnRenderGUI(deltaTime);
			if (!this.telegraphBlock.IsManagedBySwitchboard())
			{
				return;
			}
			bool canEdit = this.telegraphBlock.HasAdvancedRoutingEnabled();
			if (this.lastCanEditRouting == null)
			{
				this.lastCanEditRouting = new bool?(canEdit);
				return;
			}
			if (this.lastCanEditRouting.Value != canEdit)
			{
				this.TryClose();
				return;
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x000143B8 File Offset: 0x000125B8
		public void RefreshRoutingControls()
		{
			if (base.SingleComposer == null || !this.telegraphBlock.IsManagedBySwitchboard())
			{
				return;
			}
			this.endpointNameInputElem = (base.SingleComposer.GetElement("endpointNameInput") as GuiElementTextInput);
			this.targetDropDownElem = (base.SingleComposer.GetElement("endpointTargetDropdown") as GuiElementDropDown);
			bool canEditRouting = this.telegraphBlock.HasAdvancedRoutingEnabled();
			if (this.endpointNameInputElem == null || this.targetDropDownElem == null)
			{
				return;
			}
			this.pendingEndpointName = this.telegraphBlock.GetCustomEndpointName();
			GuiElementTextInput guiElementTextInput = this.endpointNameInputElem;
			if (guiElementTextInput != null)
			{
				guiElementTextInput.SetValue(this.pendingEndpointName, true);
			}
			GuiElementTextInput guiElementTextInput2 = this.endpointNameInputElem;
			if (guiElementTextInput2 != null)
			{
				guiElementTextInput2.SetPlaceHolderText(UIUtils.I18n("Telegraph.Gui.EndpointNamePlaceholder"));
			}
			if (canEditRouting)
			{
				string[] endpointValues = this.BuildEndpointValues();
				string[] endpointNames = this.BuildEndpointNames(endpointValues);
				int selectedIndex = TelegraphMenuDialog.ResolveSelectedTargetIndex(endpointValues, this.telegraphBlock.GetTargetEndpointName());
				this.targetDropDownElem.SetList(endpointValues, endpointNames);
				this.targetDropDownElem.SetSelectedIndex(selectedIndex);
				return;
			}
			string[] endpointValues2 = new string[]
			{
				"all"
			};
			string[] endpointNames2 = new string[]
			{
				UIUtils.I18n("Telegraph.Gui.TargetAll")
			};
			this.targetDropDownElem.SetList(endpointValues2, endpointNames2);
			this.targetDropDownElem.SetSelectedIndex(0);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000144F0 File Offset: 0x000126F0
		private void OnEndpointNameInputChanged(string value)
		{
			if (this.telegraphBlock.HasAdvancedRoutingEnabled())
			{
				this.pendingEndpointName = (value ?? "");
				return;
			}
			GuiElementTextInput guiElementTextInput = this.endpointNameInputElem;
			if (guiElementTextInput == null)
			{
				return;
			}
			guiElementTextInput.SetValue(this.telegraphBlock.GetCustomEndpointName(), true);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001452C File Offset: 0x0001272C
		private bool OnSaveEndpointNameClicked()
		{
			if (!this.telegraphBlock.HasAdvancedRoutingEnabled())
			{
				ICoreClientAPI capi = this.capi;
				if (capi != null)
				{
					capi.TriggerChatMessage(UIUtils.I18n(this.telegraphBlock.GetRoutingDisabledReasonLangKey()));
				}
				return true;
			}
			string candidate = (this.pendingEndpointName ?? "").Trim();
			if (candidate.Length > 0)
			{
				string[] availableEndpointNames = this.telegraphBlock.GetAvailableEndpointNames();
				for (int i = 0; i < availableEndpointNames.Length; i++)
				{
					if (string.Equals(availableEndpointNames[i], candidate, 5))
					{
						ICoreClientAPI capi2 = this.capi;
						if (capi2 != null)
						{
							capi2.TriggerChatMessage(UIUtils.I18n("Telegraph.Settings.NameAlreadyUsed"));
						}
						return true;
					}
				}
			}
			this.telegraphBlock.RequestSaveCustomEndpointName(this.pendingEndpointName);
			this.TryUnfocusEndpointInput();
			return true;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000145E2 File Offset: 0x000127E2
		private void OnTargetSelected(string value, bool selected)
		{
			if (!selected)
			{
				return;
			}
			if (this.telegraphBlock.HasAdvancedRoutingEnabled())
			{
				this.telegraphBlock.RequestTargetEndpointChange(value);
				return;
			}
			GuiElementDropDown guiElementDropDown = this.targetDropDownElem;
			if (guiElementDropDown == null)
			{
				return;
			}
			guiElementDropDown.SetSelectedIndex(0);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00014614 File Offset: 0x00012814
		private string[] BuildEndpointValues()
		{
			string[] names = this.telegraphBlock.GetAvailableEndpointNames();
			string[] values = new string[names.Length + 1];
			values[0] = "all";
			Array.Copy(names, 0, values, 1, names.Length);
			return values;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00014650 File Offset: 0x00012850
		private string[] BuildEndpointNames(string[] values)
		{
			string[] names = new string[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				names[i] = ((values[i] == "all") ? UIUtils.I18n("Telegraph.Gui.TargetAll") : values[i]);
			}
			return names;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00014698 File Offset: 0x00012898
		private static int ResolveSelectedTargetIndex(string[] values, string selectedValue)
		{
			if (string.IsNullOrWhiteSpace(selectedValue))
			{
				return 0;
			}
			for (int i = 0; i < values.Length; i++)
			{
				if (string.Equals(values[i], selectedValue, 5))
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000146CC File Offset: 0x000128CC
		private bool IsEndpointInputFocused()
		{
			if (this.endpointNameInputElem == null)
			{
				return false;
			}
			Type type = this.endpointNameInputElem.GetType();
			try
			{
				foreach (string propName in new string[]
				{
					"HasFocus",
					"IsFocused",
					"Focused"
				})
				{
					PropertyInfo prop = type.GetProperty(propName, 52);
					if (prop != null && prop.PropertyType == typeof(bool) && (bool)prop.GetValue(this.endpointNameInputElem))
					{
						return true;
					}
				}
				foreach (string methodName in new string[]
				{
					"HasFocus",
					"IsFocused",
					"Focused"
				})
				{
					MethodInfo method = type.GetMethod(methodName, 52, null, Type.EmptyTypes, null);
					if (method != null && method.ReturnType == typeof(bool) && (bool)method.Invoke(this.endpointNameInputElem, null))
					{
						return true;
					}
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00014804 File Offset: 0x00012A04
		private void TryUnfocusEndpointInput()
		{
			if (this.endpointNameInputElem == null)
			{
				return;
			}
			Type type = this.endpointNameInputElem.GetType();
			foreach (string methodName in new string[]
			{
				"Unfocus",
				"ClearFocus",
				"Blur"
			})
			{
				MethodInfo method = type.GetMethod(methodName, 52, null, Type.EmptyTypes, null);
				if (method != null)
				{
					method.Invoke(this.endpointNameInputElem, null);
					return;
				}
			}
			foreach (string methodName2 in new string[]
			{
				"SetFocus",
				"SetFocused",
				"Focus"
			})
			{
				MethodInfo method2 = type.GetMethod(methodName2, 52, null, new Type[]
				{
					typeof(bool)
				}, null);
				if (method2 != null)
				{
					method2.Invoke(this.endpointNameInputElem, new object[]
					{
						false
					});
					return;
				}
			}
			foreach (string propName in new string[]
			{
				"HasFocus",
				"IsFocused",
				"Focused"
			})
			{
				PropertyInfo prop = type.GetProperty(propName, 52);
				if (prop != null && prop.PropertyType == typeof(bool) && prop.CanWrite)
				{
					prop.SetValue(this.endpointNameInputElem, false);
					return;
				}
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001497C File Offset: 0x00012B7C
		private bool TryForwardKeyToEndpointInput(KeyEvent args)
		{
			if (this.endpointNameInputElem == null)
			{
				return false;
			}
			Type type = this.endpointNameInputElem.GetType();
			try
			{
				MethodInfo onKeyPress = type.GetMethod("OnKeyPress", 52, null, new Type[]
				{
					typeof(KeyEvent)
				}, null);
				if (onKeyPress != null)
				{
					object result = onKeyPress.Invoke(this.endpointNameInputElem, new object[]
					{
						args
					});
					if (result is bool)
					{
						return (bool)result;
					}
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00014A14 File Offset: 0x00012C14
		private void HandleEndpointInputKey(KeyEvent args)
		{
			if (this.endpointNameInputElem == null)
			{
				return;
			}
			if (args.KeyCode == 53)
			{
				if (!string.IsNullOrEmpty(this.pendingEndpointName))
				{
					this.pendingEndpointName = this.pendingEndpointName.Substring(0, this.pendingEndpointName.Length - 1);
					this.endpointNameInputElem.SetValue(this.pendingEndpointName, true);
				}
				return;
			}
			if (this.TryForwardKeyToEndpointInput(args))
			{
				return;
			}
			if (args.KeyChar != '\0' && !char.IsControl(args.KeyChar))
			{
				ReadOnlySpan<char> readOnlySpan = this.pendingEndpointName;
				char keyChar = args.KeyChar;
				this.pendingEndpointName = readOnlySpan + new ReadOnlySpan<char>(ref keyChar);
				this.endpointNameInputElem.SetValue(this.pendingEndpointName, true);
			}
		}

		// Token: 0x040001CA RID: 458
		private BlockEntityTelegraph telegraphBlock;

		// Token: 0x040001CB RID: 459
		private long lastKeySentMs;

		// Token: 0x040001CC RID: 460
		private GuiElementDynamicText sentTextElem;

		// Token: 0x040001CD RID: 461
		private GuiElementDynamicText receivedTextElem;

		// Token: 0x040001CE RID: 462
		private GuiElementDynamicText countdownTextElem;

		// Token: 0x040001CF RID: 463
		private GuiElementDynamicText sentCountdownTextElem;

		// Token: 0x040001D0 RID: 464
		private GuiElementDropDown targetDropDownElem;

		// Token: 0x040001D1 RID: 465
		private GuiElementTextInput endpointNameInputElem;

		// Token: 0x040001D2 RID: 466
		private string pendingEndpointName = "";

		// Token: 0x040001D3 RID: 467
		private bool? lastCanEditRouting;
	}
}
