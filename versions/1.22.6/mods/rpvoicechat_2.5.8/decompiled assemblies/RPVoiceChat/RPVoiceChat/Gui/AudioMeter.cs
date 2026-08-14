using System;
using RPVoiceChat.Audio;
using Vintagestory.API.Client;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200006A RID: 106
	public class AudioMeter : GuiElementStatbar, IExtendedGuiElement
	{
		// Token: 0x060002ED RID: 749 RVA: 0x0000FDF8 File Offset: 0x0000DFF8
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

		// Token: 0x060002EE RID: 750 RVA: 0x0000FE6D File Offset: 0x0000E06D
		public void Init(string elementKey, ElementBounds bounds, GuiComposer composer)
		{
			this.key = elementKey;
			this.Bounds = bounds.FlatCopy().WithFixedWidth(200.0);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000FE90 File Offset: 0x0000E090
		private void SetCoefficient()
		{
			this.coefficient = 100.0;
			if (!this.shouldScale)
			{
				return;
			}
			this.coefficient /= this._audioInputManager.GetMaxInputThreshold();
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000FEC2 File Offset: 0x0000E0C2
		private void OnDialogOpen()
		{
			this.gameTickListenerId = this.api.Event.RegisterGameTickListener(new Action<float>(this.TickUpdate), 20, 0);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000FEE9 File Offset: 0x0000E0E9
		private void OnDialogClosed()
		{
			this.api.Event.UnregisterGameTickListener(this.gameTickListenerId);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000FF01 File Offset: 0x0000E101
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

		// Token: 0x060002F3 RID: 755 RVA: 0x0000FF38 File Offset: 0x0000E138
		private void UpdateVisuals()
		{
			float displayValue = (float)Math.Round(this._audioInputManager.Amplitude * this.coefficient);
			this.ShouldFlash = this._audioInputManager.Transmitting;
			base.SetValue(displayValue);
		}

		// Token: 0x04000169 RID: 361
		private const double audioMeterWidth = 200.0;

		// Token: 0x0400016A RID: 362
		private MicrophoneManager _audioInputManager;

		// Token: 0x0400016B RID: 363
		private GuiDialog parrentDialog;

		// Token: 0x0400016C RID: 364
		private bool shouldScale;

		// Token: 0x0400016D RID: 365
		private string key;

		// Token: 0x0400016E RID: 366
		private long gameTickListenerId;

		// Token: 0x0400016F RID: 367
		private double coefficient;
	}
}
