using System;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using RPVoiceChat.Audio;
using RPVoiceChat.Config;
using RPVoiceChat.DB;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000074 RID: 116
	public class ModMenuDialog : ConfigDialog
	{
		// Token: 0x0600033C RID: 828 RVA: 0x00011AD4 File Offset: 0x0000FCD4
		public ModMenuDialog(ICoreClientAPI capi, MicrophoneManager _audioInputManager, AudioOutputManager _audioOutputManager, ClientSettingsRepository settingsRepository, GuiManager guiManager) : base(capi)
		{
			this.audioInputManager = _audioInputManager;
			this.audioOutputManager = _audioOutputManager;
			this.guiManager = guiManager;
			guiManager.audioWizardDialog.GainCalibrationDone += new Action(this.OnAudioWizardClosed);
			ConfigDialog.ConfigTab audioInputTab = new ConfigDialog.ConfigTab("AudioInput");
			ConfigDialog.ConfigTab audioOutputTab = new ConfigDialog.ConfigTab("AudioOutput");
			ConfigDialog.ConfigTab effectsTab = new ConfigDialog.ConfigTab("Effects");
			ConfigDialog.ConfigTab interfaceTab = new ConfigDialog.ConfigTab("Interface");
			ConfigDialog.ConfigTab playerListTab = new ConfigDialog.ConfigTab("PlayerList");
			ConfigDialog.ConfigTab advancedTab = new ConfigDialog.ConfigTab("Advanced");
			base.RegisterTab(audioInputTab);
			base.RegisterTab(audioOutputTab);
			base.RegisterTab(effectsTab);
			base.RegisterTab(interfaceTab);
			base.RegisterTab(playerListTab);
			base.RegisterTab(advancedTab);
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "inputDevice",
				Type = ConfigDialog.ElementType.Dropdown,
				Label = true,
				Tooltip = true,
				Tab = audioInputTab,
				DropdownNames = this.audioInputManager.GetInputDeviceNames(),
				DropdownValues = this.audioInputManager.GetInputDeviceNames(),
				DropdownSelect = new SelectionChangedDelegate(this.OnChangeInputDevice)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "togglePushToTalk",
				Type = ConfigDialog.ElementType.Switch,
				Label = true,
				Tooltip = true,
				Tab = audioInputTab,
				ToggleAction = new Action<bool>(this.OnTogglePushToTalk)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "muteMicrophone",
				Type = ConfigDialog.ElementType.Switch,
				Label = true,
				Tooltip = true,
				Tab = audioInputTab,
				ToggleAction = new Action<bool>(this.OnToggleMuted)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "loopback",
				Type = ConfigDialog.ElementType.Switch,
				Label = true,
				Tooltip = true,
				Tab = audioOutputTab,
				ToggleAction = new Action<bool>(this.OnToggleLoopback)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "monoMode",
				Type = ConfigDialog.ElementType.Switch,
				Label = true,
				Tooltip = true,
				Tab = audioOutputTab,
				ToggleAction = new Action<bool>(this.OnToggleMonoMode)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "devicesVoiceFeedback",
				Type = ConfigDialog.ElementType.Switch,
				Label = true,
				Tooltip = true,
				Tab = audioOutputTab,
				ToggleAction = new Action<bool>(this.OnToggleDevicesVoiceFeedback)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "outputVoice",
				Type = ConfigDialog.ElementType.Slider,
				Label = true,
				Tooltip = true,
				Tab = audioOutputTab,
				SlideAction = new ActionConsumable<int>(this.SlideOutputVoice)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "outputBlock",
				Type = ConfigDialog.ElementType.Slider,
				Label = true,
				Tooltip = true,
				Tab = audioOutputTab,
				SlideAction = new ActionConsumable<int>(this.SlideOutputBlock)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "outputItem",
				Type = ConfigDialog.ElementType.Slider,
				Label = true,
				Tooltip = true,
				Tab = audioOutputTab,
				SlideAction = new ActionConsumable<int>(this.SlideOutputItem)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "inputGain",
				Type = ConfigDialog.ElementType.Slider,
				Label = true,
				Tooltip = true,
				Tab = audioInputTab,
				SlideAction = new ActionConsumable<int>(this.SlideInputGain),
				SlideTooltip = new SliderTooltipDelegate(this.SlideInputGainTooltip)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "inputThreshold",
				Type = ConfigDialog.ElementType.Slider,
				Label = true,
				Tooltip = true,
				Tab = audioInputTab,
				SlideAction = new ActionConsumable<int>(this.SlideInputThreshold)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "audioMeter",
				Type = ConfigDialog.ElementType.Custom,
				Label = true,
				Tooltip = true,
				Tab = audioInputTab,
				CustomElement = new AudioMeter(capi, this.audioInputManager, this, false)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "audioWizard",
				Type = ConfigDialog.ElementType.Button,
				Label = true,
				Tooltip = true,
				Tab = audioInputTab,
				ButtonAction = new ActionConsumable(this.OpenAudioWizard)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "toggleHUD",
				Type = ConfigDialog.ElementType.Switch,
				Label = true,
				Tooltip = true,
				Tab = interfaceTab,
				ToggleAction = new Action<bool>(this.OnToggleHUD)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "minimalHUD",
				Type = ConfigDialog.ElementType.Switch,
				Label = true,
				Tooltip = true,
				Tab = interfaceTab,
				ToggleAction = new Action<bool>(this.OnToggleMinimalHUD)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "toggleMuffling",
				Type = ConfigDialog.ElementType.Switch,
				Label = true,
				Tooltip = true,
				Tab = effectsTab,
				ToggleAction = new Action<bool>(this.OnToggleMuffling)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Enabled = this.audioInputManager.IsDenoisingAvailable,
				Key = "toggleDenoising",
				Type = ConfigDialog.ElementType.Switch,
				Label = true,
				Tooltip = true,
				Tab = effectsTab,
				ToggleAction = new Action<bool>(this.OnToggleDenoising)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Enabled = this.audioInputManager.IsDenoisingAvailable,
				Key = "denoisingSensitivity",
				Type = ConfigDialog.ElementType.Slider,
				Label = true,
				Tooltip = true,
				Tab = effectsTab,
				SlideAction = new ActionConsumable<int>(this.SlideDenoisingSensitivity)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Enabled = this.audioInputManager.IsDenoisingAvailable,
				Key = "denoisingStrength",
				Type = ConfigDialog.ElementType.Slider,
				Label = true,
				Tooltip = true,
				Tab = effectsTab,
				SlideAction = new ActionConsumable<int>(this.SlideDenoisingStrength)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "playerList",
				Type = ConfigDialog.ElementType.Custom,
				Tab = playerListTab,
				CustomElement = new PlayerList(capi, settingsRepository, this)
			});
			base.RegisterOption(new ConfigDialog.ConfigOption
			{
				Key = "toggleChannelGuessing",
				Type = ConfigDialog.ElementType.Switch,
				Label = true,
				Tooltip = true,
				Tab = advancedTab,
				ToggleAction = new Action<bool>(this.OnToggleChannelGuessing)
			});
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00012174 File Offset: 0x00010374
		public override bool TryOpen()
		{
			return this.audioInputManager.AudioWizardActive || this.guiManager.firstLaunchDialog.IsOpened() || base.TryOpen();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000121A0 File Offset: 0x000103A0
		public override bool TryClose()
		{
			return this.audioInputManager.AudioWizardActive || base.TryClose();
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000121B8 File Offset: 0x000103B8
		protected override void RefreshValues()
		{
			if (!this.IsOpened())
			{
				return;
			}
			float outputVoice = ModConfig.ClientConfig.OutputVoice * 100f;
			float outputBlock = ModConfig.ClientConfig.OutputBlock * 100f;
			float outputItem = ModConfig.ClientConfig.OutputItem * 100f;
			float inputGainDBS = AudioUtils.FactorToDBs(ModConfig.ClientConfig.InputGain) * 10f;
			float inputThreshold = ModConfig.ClientConfig.InputThreshold * 100f;
			float denoisingSensitivity = ModConfig.ClientConfig.DenoisingSensitivity * 100f;
			float denoisingStrength = ModConfig.ClientConfig.DenoisingStrength * 100f;
			this.SetValue("configTabs", ModConfig.ClientConfig.ActiveConfigTab);
			this.SetValue("inputDevice", ModConfig.ClientConfig.InputDevice ?? "Default");
			this.SetValue("togglePushToTalk", ModConfig.ClientConfig.PushToTalkEnabled);
			this.SetValue("muteMicrophone", ModConfig.ClientConfig.IsMuted);
			this.SetValue("loopback", ModConfig.ClientConfig.Loopback);
			this.SetValue("monoMode", ModConfig.ClientConfig.IsMonoMode);
			this.SetValue("devicesVoiceFeedback", ModConfig.ClientConfig.DevicesVoiceFeedback);
			this.SetValue("outputVoice", new object[]
			{
				outputVoice,
				0,
				200,
				1,
				"%"
			});
			this.SetValue("inputGain", new object[]
			{
				inputGainDBS,
				-200,
				200,
				1,
				""
			});
			this.SetValue("outputBlock", new object[]
			{
				outputBlock,
				0,
				200,
				1,
				"%"
			});
			this.SetValue("outputItem", new object[]
			{
				outputItem,
				0,
				200,
				1,
				"%"
			});
			this.SetValue("inputThreshold", new object[]
			{
				inputThreshold,
				0,
				100,
				1,
				""
			});
			this.SetValue("toggleHUD", ModConfig.ClientConfig.ShowHud);
			this.SetValue("minimalHUD", ModConfig.ClientConfig.IsMinimalHud);
			this.SetValue("toggleMuffling", ModConfig.ClientConfig.Muffling);
			this.SetValue("toggleDenoising", ModConfig.ClientConfig.Denoising);
			this.SetValue("denoisingSensitivity", new object[]
			{
				denoisingSensitivity,
				0,
				100,
				1,
				"%"
			});
			this.SetValue("denoisingStrength", new object[]
			{
				denoisingStrength,
				0,
				100,
				1,
				"%"
			});
			this.SetValue("playerList", null);
			this.SetValue("toggleChannelGuessing", ModConfig.ClientConfig.ChannelGuessing);
			if (this.audioInputManager.AudioWizardActive)
			{
				base.DisableHoverText();
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00012570 File Offset: 0x00010770
		private void SetValue(string key, dynamic value)
		{
			GuiElement element = base.SingleComposer.GetElement(key);
			if (element == null)
			{
				return;
			}
			GuiElementDropDown dropDown = element as GuiElementDropDown;
			if (dropDown != null)
			{
				if (ModMenuDialog.<>o__7.<>p__0 == null)
				{
					ModMenuDialog.<>o__7.<>p__0 = CallSite<Action<CallSite, GuiElementDropDown, object>>.Create(Binder.InvokeMember(256, "SetSelectedValue", null, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(1, null),
						CSharpArgumentInfo.Create(0, null)
					}));
				}
				ModMenuDialog.<>o__7.<>p__0.Target.Invoke(ModMenuDialog.<>o__7.<>p__0, dropDown, value);
				return;
			}
			GuiElementSwitch switchBox = element as GuiElementSwitch;
			if (switchBox != null)
			{
				GuiElementSwitch guiElementSwitch = switchBox;
				if (ModMenuDialog.<>o__7.<>p__1 == null)
				{
					ModMenuDialog.<>o__7.<>p__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(0, typeof(bool), typeof(ModMenuDialog)));
				}
				guiElementSwitch.On = ModMenuDialog.<>o__7.<>p__1.Target.Invoke(ModMenuDialog.<>o__7.<>p__1, value);
				return;
			}
			GuiElementSlider slider = element as GuiElementSlider;
			if (slider != null)
			{
				if (ModMenuDialog.<>o__7.<>p__7 == null)
				{
					ModMenuDialog.<>o__7.<>p__7 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(0, typeof(int), typeof(ModMenuDialog)));
				}
				Func<CallSite, object, int> target = ModMenuDialog.<>o__7.<>p__7.Target;
				CallSite <>p__ = ModMenuDialog.<>o__7.<>p__7;
				if (ModMenuDialog.<>o__7.<>p__6 == null)
				{
					ModMenuDialog.<>o__7.<>p__6 = CallSite<Func<CallSite, Type, int, object, object, object>>.Create(Binder.InvokeMember(0, "Clamp", null, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(33, null),
						CSharpArgumentInfo.Create(1, null),
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(0, null)
					}));
				}
				Func<CallSite, Type, int, object, object, object> target2 = ModMenuDialog.<>o__7.<>p__6.Target;
				CallSite <>p__2 = ModMenuDialog.<>o__7.<>p__6;
				Type typeFromHandle = typeof(GameMath);
				if (ModMenuDialog.<>o__7.<>p__3 == null)
				{
					ModMenuDialog.<>o__7.<>p__3 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(16, typeof(int), typeof(ModMenuDialog)));
				}
				Func<CallSite, object, int> target3 = ModMenuDialog.<>o__7.<>p__3.Target;
				CallSite <>p__3 = ModMenuDialog.<>o__7.<>p__3;
				if (ModMenuDialog.<>o__7.<>p__2 == null)
				{
					ModMenuDialog.<>o__7.<>p__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(0, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(3, null)
					}));
				}
				int num = target3.Invoke(<>p__3, ModMenuDialog.<>o__7.<>p__2.Target.Invoke(ModMenuDialog.<>o__7.<>p__2, value, 0));
				if (ModMenuDialog.<>o__7.<>p__4 == null)
				{
					ModMenuDialog.<>o__7.<>p__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(0, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(3, null)
					}));
				}
				object obj = ModMenuDialog.<>o__7.<>p__4.Target.Invoke(ModMenuDialog.<>o__7.<>p__4, value, 1);
				if (ModMenuDialog.<>o__7.<>p__5 == null)
				{
					ModMenuDialog.<>o__7.<>p__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(0, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(3, null)
					}));
				}
				int sliderValue = target.Invoke(<>p__, target2.Invoke(<>p__2, typeFromHandle, num, obj, ModMenuDialog.<>o__7.<>p__5.Target.Invoke(ModMenuDialog.<>o__7.<>p__5, value, 2)));
				if (ModMenuDialog.<>o__7.<>p__12 == null)
				{
					ModMenuDialog.<>o__7.<>p__12 = CallSite<Action<CallSite, GuiElementSlider, int, object, object, object, object>>.Create(Binder.InvokeMember(256, "SetValues", null, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(1, null),
						CSharpArgumentInfo.Create(1, null),
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(0, null)
					}));
				}
				Action<CallSite, GuiElementSlider, int, object, object, object, object> target4 = ModMenuDialog.<>o__7.<>p__12.Target;
				CallSite <>p__4 = ModMenuDialog.<>o__7.<>p__12;
				GuiElementSlider guiElementSlider = slider;
				int num2 = sliderValue;
				if (ModMenuDialog.<>o__7.<>p__8 == null)
				{
					ModMenuDialog.<>o__7.<>p__8 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(0, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(3, null)
					}));
				}
				object obj2 = ModMenuDialog.<>o__7.<>p__8.Target.Invoke(ModMenuDialog.<>o__7.<>p__8, value, 1);
				if (ModMenuDialog.<>o__7.<>p__9 == null)
				{
					ModMenuDialog.<>o__7.<>p__9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(0, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(3, null)
					}));
				}
				object obj3 = ModMenuDialog.<>o__7.<>p__9.Target.Invoke(ModMenuDialog.<>o__7.<>p__9, value, 2);
				if (ModMenuDialog.<>o__7.<>p__10 == null)
				{
					ModMenuDialog.<>o__7.<>p__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(0, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(3, null)
					}));
				}
				object obj4 = ModMenuDialog.<>o__7.<>p__10.Target.Invoke(ModMenuDialog.<>o__7.<>p__10, value, 3);
				if (ModMenuDialog.<>o__7.<>p__11 == null)
				{
					ModMenuDialog.<>o__7.<>p__11 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(0, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(3, null)
					}));
				}
				target4.Invoke(<>p__4, guiElementSlider, num2, obj2, obj3, obj4, ModMenuDialog.<>o__7.<>p__11.Target.Invoke(ModMenuDialog.<>o__7.<>p__11, value, 4));
				if (ModMenuDialog.<>o__7.<>p__15 == null)
				{
					ModMenuDialog.<>o__7.<>p__15 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(0, 83, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null)
					}));
				}
				Func<CallSite, object, bool> target5 = ModMenuDialog.<>o__7.<>p__15.Target;
				CallSite <>p__5 = ModMenuDialog.<>o__7.<>p__15;
				if (ModMenuDialog.<>o__7.<>p__14 == null)
				{
					ModMenuDialog.<>o__7.<>p__14 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(0, 20, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(3, null)
					}));
				}
				Func<CallSite, object, int, object> target6 = ModMenuDialog.<>o__7.<>p__14.Target;
				CallSite <>p__6 = ModMenuDialog.<>o__7.<>p__14;
				if (ModMenuDialog.<>o__7.<>p__13 == null)
				{
					ModMenuDialog.<>o__7.<>p__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(0, "Length", typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null)
					}));
				}
				if (target5.Invoke(<>p__5, target6.Invoke(<>p__6, ModMenuDialog.<>o__7.<>p__13.Target.Invoke(ModMenuDialog.<>o__7.<>p__13, value), 6)))
				{
					return;
				}
				if (ModMenuDialog.<>o__7.<>p__17 == null)
				{
					ModMenuDialog.<>o__7.<>p__17 = CallSite<Action<CallSite, GuiElementSlider, object>>.Create(Binder.InvokeMember(256, "SetAlarmValue", null, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(1, null),
						CSharpArgumentInfo.Create(0, null)
					}));
				}
				Action<CallSite, GuiElementSlider, object> target7 = ModMenuDialog.<>o__7.<>p__17.Target;
				CallSite <>p__7 = ModMenuDialog.<>o__7.<>p__17;
				GuiElementSlider guiElementSlider2 = slider;
				if (ModMenuDialog.<>o__7.<>p__16 == null)
				{
					ModMenuDialog.<>o__7.<>p__16 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(0, typeof(ModMenuDialog), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(0, null),
						CSharpArgumentInfo.Create(3, null)
					}));
				}
				target7.Invoke(<>p__7, guiElementSlider2, ModMenuDialog.<>o__7.<>p__16.Target.Invoke(ModMenuDialog.<>o__7.<>p__16, value, 5));
				return;
			}
			else
			{
				GuiElementVerticalTabs verticalTabs = element as GuiElementVerticalTabs;
				if (verticalTabs != null)
				{
					GuiElementVerticalTabs guiElementVerticalTabs = verticalTabs;
					if (ModMenuDialog.<>o__7.<>p__18 == null)
					{
						ModMenuDialog.<>o__7.<>p__18 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(0, typeof(int), typeof(ModMenuDialog)));
					}
					guiElementVerticalTabs.ActiveElement = ModMenuDialog.<>o__7.<>p__18.Target.Invoke(ModMenuDialog.<>o__7.<>p__18, value);
					return;
				}
				PlayerList playerList = element as PlayerList;
				if (playerList != null)
				{
					playerList.SetupElement();
					return;
				}
				throw new Exception("Unknown element type");
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00012C07 File Offset: 0x00010E07
		private void OnAudioWizardClosed()
		{
			base.EnableHoverText();
			this.RefreshValues();
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00012C15 File Offset: 0x00010E15
		private void OnChangeInputDevice(string value, bool selected)
		{
			this.audioInputManager.SetInputDevice(value);
			this.SetValue("inputDevice", ModConfig.ClientConfig.InputDevice ?? "Default");
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00012C41 File Offset: 0x00010E41
		private void OnTogglePushToTalk(bool enabled)
		{
			ModConfig.ClientConfig.PushToTalkEnabled = enabled;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00012C4E File Offset: 0x00010E4E
		private void OnToggleMuted(bool enabled)
		{
			ModConfig.ClientConfig.IsMuted = enabled;
			this.capi.Event.PushEvent("rpvoicechat:hudUpdate", null);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00012C71 File Offset: 0x00010E71
		protected void OnToggleLoopback(bool enabled)
		{
			ModConfig.ClientConfig.Loopback = enabled;
			this.audioOutputManager.IsLoopbackEnabled = enabled;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00012C8A File Offset: 0x00010E8A
		protected void OnToggleMonoMode(bool enabled)
		{
			ModConfig.ClientConfig.IsMonoMode = enabled;
			this.capi.Event.PushEvent("rpvoicechat:hudUpdate", null);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00012CAD File Offset: 0x00010EAD
		protected void OnToggleDevicesVoiceFeedback(bool enabled)
		{
			ModConfig.ClientConfig.DevicesVoiceFeedback = enabled;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00012CBA File Offset: 0x00010EBA
		protected void OnToggleMinimalHUD(bool enabled)
		{
			ModConfig.ClientConfig.IsMinimalHud = enabled;
			this.capi.Event.PushEvent("rpvoicechat:hudUpdate", null);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00012CE0 File Offset: 0x00010EE0
		private bool SlideOutputVoice(int intGain)
		{
			float gain = (float)intGain / 100f;
			ModConfig.ClientConfig.OutputVoice = gain;
			PlayerListener.SetVoiceGain(gain);
			return true;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00012D08 File Offset: 0x00010F08
		private bool SlideOutputBlock(int intGain)
		{
			float gain = (float)intGain / 100f;
			ModConfig.ClientConfig.OutputBlock = gain;
			PlayerListener.SetBlockGain(gain);
			return true;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00012D30 File Offset: 0x00010F30
		private bool SlideOutputItem(int intGain)
		{
			float gain = (float)intGain / 100f;
			ModConfig.ClientConfig.OutputItem = gain;
			PlayerListener.SetItemGain(gain);
			return true;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00012D58 File Offset: 0x00010F58
		private bool SlideInputGain(int intDBGain)
		{
			float gain = AudioUtils.DBsToFactor((float)intDBGain / 10f);
			if (intDBGain == -200)
			{
				gain = 0f;
			}
			ModConfig.ClientConfig.InputGain = gain;
			this.audioInputManager.SetGain(gain);
			return true;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00012D99 File Offset: 0x00010F99
		private string SlideInputGainTooltip(int value)
		{
			if (value == -200)
			{
				return "Off";
			}
			return this.SlideDecibelsTooltip(value);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00012DB0 File Offset: 0x00010FB0
		private string SlideDecibelsTooltip(int value)
		{
			string sign = (value < 0) ? "-" : "+";
			int integerPart = Math.Abs(value / 10);
			int decimalPart = Math.Abs(value % 10);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 3);
			defaultInterpolatedStringHandler.AppendFormatted(sign);
			defaultInterpolatedStringHandler.AppendFormatted<int>(integerPart);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<int>(decimalPart);
			return defaultInterpolatedStringHandler.ToStringAndClear() + "dB";
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00012E20 File Offset: 0x00011020
		private bool SlideInputThreshold(int intThreshold)
		{
			float threshold = (float)intThreshold / 100f;
			ModConfig.ClientConfig.InputThreshold = threshold;
			this.audioInputManager.SetThreshold(threshold);
			return true;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00012E4E File Offset: 0x0001104E
		private bool OpenAudioWizard()
		{
			if (this.guiManager.audioWizardDialog.IsOpened())
			{
				return true;
			}
			base.DisableHoverText();
			this.guiManager.audioWizardDialog.TryOpen();
			return true;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00012E7C File Offset: 0x0001107C
		private void OnToggleHUD(bool enabled)
		{
			ModConfig.ClientConfig.ShowHud = enabled;
			this.capi.Event.PushEvent("rpvoicechat:hudUpdate", null);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00012E9F File Offset: 0x0001109F
		private void OnToggleMuffling(bool enabled)
		{
			ModConfig.ClientConfig.Muffling = enabled;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00012EAC File Offset: 0x000110AC
		private void OnToggleDenoising(bool enabled)
		{
			ModConfig.ClientConfig.Denoising = enabled;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00012EBC File Offset: 0x000110BC
		private bool SlideDenoisingSensitivity(int intSensitivity)
		{
			float sensitivity = (float)intSensitivity / 100f;
			ModConfig.ClientConfig.DenoisingSensitivity = sensitivity;
			this.audioInputManager.SetDenoisingSensitivity(sensitivity);
			return true;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00012EEC File Offset: 0x000110EC
		private bool SlideDenoisingStrength(int intStrength)
		{
			float strength = (float)intStrength / 100f;
			ModConfig.ClientConfig.DenoisingStrength = strength;
			this.audioInputManager.SetDenoisingStrength(strength);
			return true;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00012F1A File Offset: 0x0001111A
		private void OnToggleChannelGuessing(bool enabled)
		{
			ModConfig.ClientConfig.ChannelGuessing = enabled;
		}

		// Token: 0x040001B7 RID: 439
		private MicrophoneManager audioInputManager;

		// Token: 0x040001B8 RID: 440
		private AudioOutputManager audioOutputManager;

		// Token: 0x040001B9 RID: 441
		private GuiManager guiManager;
	}
}
