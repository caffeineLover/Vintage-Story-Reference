using System;
using RPVoiceChat.Audio;
using RPVoiceChat.DB;
using Vintagestory.API.Client;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000092 RID: 146
	public class GuiManager : IDisposable
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00014A8C File Offset: 0x00012C8C
		public MicrophoneManager audioInputManager { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00014A94 File Offset: 0x00012C94
		public AudioWizardDialog audioWizardDialog { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00014A9C File Offset: 0x00012C9C
		public FirstLaunchDialog firstLaunchDialog { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00014AA4 File Offset: 0x00012CA4
		public ModMenuDialog modMenuDialog { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00014AAC File Offset: 0x00012CAC
		public Announce announce { get; }

		// Token: 0x060003D8 RID: 984 RVA: 0x00014AB4 File Offset: 0x00012CB4
		public GuiManager(ICoreClientAPI capi, MicrophoneManager audioInputManager, AudioOutputManager audioOutputManager, ClientSettingsRepository settingsRepository)
		{
			this.audioInputManager = audioInputManager;
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

		// Token: 0x060003D9 RID: 985 RVA: 0x00014B5E File Offset: 0x00012D5E
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
