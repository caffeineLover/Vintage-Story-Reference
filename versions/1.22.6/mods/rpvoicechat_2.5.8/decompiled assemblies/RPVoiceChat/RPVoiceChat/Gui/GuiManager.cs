using System;
using RPVoiceChat.Audio;
using RPVoiceChat.DB;
using Vintagestory.API.Client;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200006F RID: 111
	public class GuiManager : IDisposable
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00010959 File Offset: 0x0000EB59
		public AudioWizardDialog audioWizardDialog { get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000312 RID: 786 RVA: 0x00010961 File Offset: 0x0000EB61
		public FirstLaunchDialog firstLaunchDialog { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00010969 File Offset: 0x0000EB69
		public ModMenuDialog modMenuDialog { get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000314 RID: 788 RVA: 0x00010971 File Offset: 0x0000EB71
		public Announce announce { get; }

		// Token: 0x06000315 RID: 789 RVA: 0x0001097C File Offset: 0x0000EB7C
		public GuiManager(ICoreClientAPI capi, MicrophoneManager audioInputManager, AudioOutputManager audioOutputManager, ClientSettingsRepository settingsRepository)
		{
			this.audioWizardDialog = new AudioWizardDialog(capi, audioInputManager, audioOutputManager);
			this.firstLaunchDialog = new FirstLaunchDialog(capi, this);
			this.modMenuDialog = new ModMenuDialog(capi, audioInputManager, audioOutputManager, settingsRepository, this);
			capi.Gui.RegisterDialog(new GuiDialog[]
			{
				new SpeechIndicator(capi, audioInputManager)
			});
			capi.Gui.RegisterDialog(new GuiDialog[]
			{
				new VoiceLevelIcon(capi, audioInputManager)
			});
			this.announce = new Announce(capi);
			capi.Gui.RegisterDialog(new GuiDialog[]
			{
				this.announce
			});
			new PlayerNameTagRenderer(capi, audioOutputManager);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00010A1F File Offset: 0x0000EC1F
		public void Dispose()
		{
			AudioWizardDialog audioWizardDialog = this.audioWizardDialog;
			if (audioWizardDialog != null)
			{
				audioWizardDialog.Dispose();
			}
			FirstLaunchDialog firstLaunchDialog = this.firstLaunchDialog;
			if (firstLaunchDialog != null)
			{
				firstLaunchDialog.Dispose();
			}
			ModMenuDialog modMenuDialog = this.modMenuDialog;
			if (modMenuDialog == null)
			{
				return;
			}
			modMenuDialog.Dispose();
		}
	}
}
