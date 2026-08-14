using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace CritchersCalculator
{
	// Token: 0x02000003 RID: 3
	public class CritchersCalculatorMod : ModSystem
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000206C File Offset: 0x0000026C
		public override bool ShouldLoad(EnumAppSide forSide)
		{
			return forSide == 2;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002084 File Offset: 0x00000284
		public override void StartClientSide(ICoreClientAPI api)
		{
			this.capi = api;
			try
			{
				api.Logger.Notification("Critcher's Calculator: Initializing");
				this.calculator = new GuiDialogOreCalculator(api);
				api.Input.RegisterHotKey("critcherscalculator_open", Lang.Get("critcherscalculator:hotkey-open-calculator", Array.Empty<object>()), 18, 2, false, false, false);
				api.Input.SetHotKeyHandler("critcherscalculator_open", new ActionConsumable<KeyCombination>(this.OnCalculatorHotkey));
				api.Logger.Notification("Critcher's Calculator: Initialized successfully");
			}
			catch (Exception ex)
			{
				api.Logger.Error("Critcher's Calculator: Error initializing mod: {0}", new object[]
				{
					ex
				});
				api.Logger.Error("Critcher's Calculator: Stack trace: {0}", new object[]
				{
					ex.StackTrace
				});
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002160 File Offset: 0x00000360
		private bool OnCalculatorHotkey(KeyCombination keyComb)
		{
			ICoreClientAPI coreClientAPI = this.capi;
			IPlayer player2;
			if (coreClientAPI == null)
			{
				player2 = null;
			}
			else
			{
				IClientWorldAccessor world = coreClientAPI.World;
				player2 = ((world != null) ? world.Player : null);
			}
			IPlayer player = player2;
			bool flag = ((player != null) ? player.WorldData : null) == null || this.calculator == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				EnumGameMode mode = player.WorldData.CurrentGameMode;
				bool flag2 = mode != 1 && mode != 2;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = this.calculator.IsOpened();
					if (flag3)
					{
						this.calculator.TryClose();
					}
					else
					{
						this.calculator.TryOpen();
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002209 File Offset: 0x00000409
		public override void Dispose()
		{
			GuiDialogOreCalculator guiDialogOreCalculator = this.calculator;
			if (guiDialogOreCalculator != null)
			{
				guiDialogOreCalculator.Dispose();
			}
		}

		// Token: 0x04000002 RID: 2
		private ICoreClientAPI capi;

		// Token: 0x04000003 RID: 3
		private GuiDialogOreCalculator calculator;
	}
}
