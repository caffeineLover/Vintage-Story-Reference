using System;
using RPVoiceChat.Audio;
using Vintagestory.API.Client;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200008D RID: 141
	public class AudioMeter : GuiElementStatbar, IExtendedGuiElement
	{
		// Token: 0x060003AF RID: 943 RVA: 0x00013EA0 File Offset: 0x000120A0
		public AudioMeter(ICoreClientAPI capi, MicrophoneManager audioInputManager, GuiDialog parrent, bool unscaled = false) : base(capi, null, new double[]
		{
			0.1,
			0.4,
			0.1
		}, false, true)
		{
			this._audioInputManager = audioInputManager;
			this.parrentDialog = parrent;
			this.shouldScale = !unscaled;
			this.SetCoefficient();
			this.parrentDialog.OnOpened += new Action(this.OnDialogOpen);
			this.parrentDialog.OnClosed += new Action(this.OnDialogClosed);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00013F15 File Offset: 0x00012115
		public void Init(string elementKey, ElementBounds bounds, GuiComposer composer)
		{
			this.key = elementKey;
			this.Bounds = bounds.FlatCopy().WithFixedWidth(200.0);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00013F38 File Offset: 0x00012138
		private void SetCoefficient()
		{
			this.coefficient = 100.0;
			if (!this.shouldScale)
			{
				return;
			}
			this.coefficient /= this._audioInputManager.GetMaxInputThreshold();
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00013F6A File Offset: 0x0001216A
		private void OnDialogOpen()
		{
			this.gameTickListenerId = this.api.Event.RegisterGameTickListener(new Action<float>(this.TickUpdate), 20, 0);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00013F91 File Offset: 0x00012191
		private void OnDialogClosed()
		{
			this.api.Event.UnregisterGameTickListener(this.gameTickListenerId);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00013FA9 File Offset: 0x000121A9
		private void TickUpdate(float _)
		{
			if (!this.parrentDialog.IsOpened() || this.key == null)
			{
				return;
			}
			if (this.parrentDialog.SingleComposer.GetElement(this.key) == null)
			{
				return;
			}
			this.UpdateVisuals();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00013FE0 File Offset: 0x000121E0
		private void UpdateVisuals()
		{
			float displayValue = (float)Math.Round(this._audioInputManager.Amplitude * this.coefficient);
			this.ShouldFlash = this._audioInputManager.Transmitting;
			base.SetValue(displayValue);
		}

		// Token: 0x040001DA RID: 474
		private const double audioMeterWidth = 200.0;

		// Token: 0x040001DB RID: 475
		private MicrophoneManager _audioInputManager;

		// Token: 0x040001DC RID: 476
		private GuiDialog parrentDialog;

		// Token: 0x040001DD RID: 477
		private bool shouldScale;

		// Token: 0x040001DE RID: 478
		private string key;

		// Token: 0x040001DF RID: 479
		private long gameTickListenerId;

		// Token: 0x040001E0 RID: 480
		private double coefficient;
	}
}
