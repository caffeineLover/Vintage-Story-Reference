using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000044 RID: 68
	[NullableContext(1)]
	[Nullable(0)]
	internal static class MouseWheelPatch
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x00018180 File Offset: 0x00016380
		public static void Patch(string harmonyId, ICoreClientAPI api)
		{
			MouseWheelPatch._clientApi = api;
			IClientEventAPI @event = api.Event;
			MouseWheelEventDelegate mouseWheelEventDelegate;
			if ((mouseWheelEventDelegate = MouseWheelPatch.<>O.<0>__OnMouseWheelEvent) == null)
			{
				mouseWheelEventDelegate = (MouseWheelPatch.<>O.<0>__OnMouseWheelEvent = new MouseWheelEventDelegate(MouseWheelPatch.OnMouseWheelEvent));
			}
			@event.MouseWheelMove += mouseWheelEventDelegate;
			new Harmony(harmonyId).Patch(typeof(HudHotbar).GetMethod("OnMouseWheel", AccessTools.all), new HarmonyMethod(AccessTools.Method(typeof(MouseWheelPatch), "OnMouseWheel", null, null)), null, null, null);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000181FC File Offset: 0x000163FC
		public static void Unpatch(string harmonyId)
		{
			new Harmony(harmonyId).Unpatch(typeof(HudHotbar).GetMethod("OnMouseWheel", AccessTools.all), 1, harmonyId);
			if (MouseWheelPatch._clientApi != null)
			{
				IClientEventAPI @event = MouseWheelPatch._clientApi.Event;
				MouseWheelEventDelegate mouseWheelEventDelegate;
				if ((mouseWheelEventDelegate = MouseWheelPatch.<>O.<0>__OnMouseWheelEvent) == null)
				{
					mouseWheelEventDelegate = (MouseWheelPatch.<>O.<0>__OnMouseWheelEvent = new MouseWheelEventDelegate(MouseWheelPatch.OnMouseWheelEvent));
				}
				@event.MouseWheelMove -= mouseWheelEventDelegate;
				MouseWheelPatch._clientApi = null;
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00018266 File Offset: 0x00016466
		public static float GetDelta(MouseWheelEventArgs args)
		{
			if (args.deltaPrecise == 0f)
			{
				return (float)args.delta;
			}
			return args.deltaPrecise;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00018283 File Offset: 0x00016483
		private static void OnMouseWheelEvent(MouseWheelEventArgs args)
		{
			MouseWheelPatch.OnMouseWheel(args);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0001828C File Offset: 0x0001648C
		private static bool OnMouseWheel(MouseWheelEventArgs args)
		{
			if (MouseWheelPatch._clientApi == null)
			{
				return true;
			}
			if (args.IsHandled)
			{
				return true;
			}
			float delta = MouseWheelPatch.GetDelta(args);
			if (delta == 0f)
			{
				return true;
			}
			ItemSlot slot = MouseWheelPatch._clientApi.World.Player.InventoryManager.ActiveHotbarSlot;
			ItemStack itemstack = slot.Itemstack;
			IMouseWheelInput mouseWheelInput;
			if (itemstack == null)
			{
				mouseWheelInput = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				mouseWheelInput = ((collectible != null) ? collectible.GetCollectibleInterface<IMouseWheelInput>() : null);
			}
			IMouseWheelInput item = mouseWheelInput;
			if (item != null)
			{
				IClientPlayer player = MouseWheelPatch._clientApi.World.Player;
				bool flag = item.OnMouseWheel(slot, player, delta);
				if (flag)
				{
					args.SetHandled(true);
				}
				return !flag;
			}
			return true;
		}

		// Token: 0x04000194 RID: 404
		[Nullable(2)]
		private static ICoreClientAPI _clientApi;

		// Token: 0x020001EC RID: 492
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000B32 RID: 2866
			[Nullable(0)]
			public static MouseWheelEventDelegate <0>__OnMouseWheelEvent;
		}
	}
}
