using System;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.Client;

namespace VSImGui
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	internal class VSImGuiDialog : GuiDialog
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00002AFC File Offset: 0x00000CFC
		public VSImGuiDialog(ICoreClientAPI api, Controller controller, MainGameWindowWrapper windowWrapper, DrawCallbacksManager manager) : base(api)
		{
			this._controller = controller;
			this._manager = manager;
			this._api = api;
			windowWrapper.Draw += new Action<float>(this.Draw);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002B34 File Offset: 0x00000D34
		public override void Dispose()
		{
			base.Dispose();
			this.capi = null;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002B43 File Offset: 0x00000D43
		public override bool TryClose()
		{
			return this._allImGuiWindowsClosed && base.TryClose();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002B55 File Offset: 0x00000D55
		public void Update(float deltaTime)
		{
			this._controller.CanUpdateInputs = !this._api.Input.MouseGrabbed;
			this._controller.Update(deltaTime, this.opened);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002B88 File Offset: 0x00000D88
		public void RenderOffWindow(float deltaTime)
		{
			try
			{
				this._controller.RenderOffWindow(deltaTime);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002BB8 File Offset: 0x00000DB8
		public override void OnRenderGUI(float deltaTime)
		{
			try
			{
				this._controller.RenderMainWindow(deltaTime);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return "imguitoggle";
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002BEF File Offset: 0x00000DEF
		public override bool ShouldReceiveRenderEvents()
		{
			return true;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002BF2 File Offset: 0x00000DF2
		public override bool PrefersUngrabbedMouse
		{
			get
			{
				return this._grabMouse;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002BFA File Offset: 0x00000DFA
		public override EnumDialogType DialogType
		{
			get
			{
				if (!this._grabMouse)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002C07 File Offset: 0x00000E07
		public override void OnMouseDown(MouseEvent args)
		{
			this.HandleMouse(args);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002C10 File Offset: 0x00000E10
		public override void OnMouseUp(MouseEvent args)
		{
			this.HandleMouse(args);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002C19 File Offset: 0x00000E19
		public override void OnMouseWheel(MouseWheelEventArgs args)
		{
			this.HandleMouseWheel(args);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002C22 File Offset: 0x00000E22
		public override void OnMouseMove(MouseEvent args)
		{
			this.HandleMouseMovement(args);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002C2B File Offset: 0x00000E2B
		public override void OnKeyDown(KeyEvent args)
		{
			this.HandleKeyboard(args);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002C34 File Offset: 0x00000E34
		public override void OnKeyPress(KeyEvent args)
		{
			this.HandleKeyboard(args);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C3D File Offset: 0x00000E3D
		public override void OnKeyUp(KeyEvent args)
		{
			this.HandleKeyboard(args);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C46 File Offset: 0x00000E46
		private void HandleMouse(MouseEvent args)
		{
			if (!args.Handled)
			{
				args.Handled = Controller.MouseCaptured();
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002C5B File Offset: 0x00000E5B
		private void HandleMouseMovement(MouseEvent args)
		{
			if (!args.Handled)
			{
				args.Handled = Controller.MouseMovesCaptured();
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002C70 File Offset: 0x00000E70
		private void HandleMouseWheel(MouseWheelEventArgs args)
		{
			if (!args.IsHandled)
			{
				args.SetHandled(Controller.MouseMovesCaptured());
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002C85 File Offset: 0x00000E85
		private void HandleKeyboard(KeyEvent args)
		{
			if (!this.focused)
			{
				args.Handled = false;
				return;
			}
			if (!args.Handled || (args.KeyCode == 50 && !this.focused))
			{
				args.Handled = Controller.KeyboardCaptured();
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002CBC File Offset: 0x00000EBC
		private unsafe void Draw(float deltaSeconds)
		{
			ValueTuple<bool, bool, bool> valueTuple = this._manager.Draw(deltaSeconds);
			bool open = valueTuple.Item1;
			bool grab = valueTuple.Item2;
			bool item = valueTuple.Item3;
			if (open)
			{
				this._allImGuiWindowsClosed = false;
				this.TryOpen();
			}
			if (item)
			{
				this._allImGuiWindowsClosed = !grab;
				this.TryClose();
			}
			this._grabMouse = (grab || *ImGui.GetIO().WantTextInput);
			this.focused = (this.focused && Controller.IsAnyFocused());
		}

		// Token: 0x04000011 RID: 17
		private readonly ICoreClientAPI _api;

		// Token: 0x04000012 RID: 18
		private readonly Controller _controller;

		// Token: 0x04000013 RID: 19
		private readonly DrawCallbacksManager _manager;

		// Token: 0x04000014 RID: 20
		private bool _grabMouse;

		// Token: 0x04000015 RID: 21
		private bool _allImGuiWindowsClosed = true;
	}
}
