using System;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace TentBagReworked.Items
{
	// Token: 0x02000006 RID: 6
	[NullableContext(1)]
	[Nullable(0)]
	public class ItemTentBag : Item
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00004AE4 File Offset: 0x00002CE4
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			if (withDebugInfo)
			{
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
				appendInterpolatedStringHandler..ctor(32, 1, dsc);
				appendInterpolatedStringHandler.AppendLiteral("<font color=\"#bbbbbb\">Id:");
				appendInterpolatedStringHandler.AppendFormatted<int>(this.Id);
				appendInterpolatedStringHandler.AppendLiteral("</font>");
				dsc.AppendLine(ref appendInterpolatedStringHandler);
				appendInterpolatedStringHandler..ctor(35, 1, dsc);
				appendInterpolatedStringHandler.AppendLiteral("<font color=\"#bbbbbb\">Code: ");
				appendInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.Code);
				appendInterpolatedStringHandler.AppendLiteral("</font>");
				dsc.AppendLine(ref appendInterpolatedStringHandler);
			}
			foreach (CollectibleBehavior behavior in this.CollectibleBehaviors)
			{
				behavior.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			}
			string itemDescText = this.GetDynamicDescText();
			bool flag = itemDescText.Length > 0 && dsc.Length > 0;
			if (flag)
			{
				dsc.Append('\n');
			}
			dsc.Append(itemDescText);
			ItemStack itemstack = inSlot.Itemstack;
			int? num;
			if (itemstack == null)
			{
				num = default(int?);
			}
			else
			{
				ITreeAttribute attributes = itemstack.Attributes;
				num = ((attributes != null) ? new int?(attributes.GetInt("packed-rotation", 0)) : default(int?));
			}
			int? num2 = num;
			int rot = num2.GetValueOrDefault();
			bool flag2 = rot != 0;
			if (flag2)
			{
				bool flag3 = dsc.Length > 0;
				if (flag3)
				{
					dsc.Append('\n');
				}
				string text;
				if ((text = Lang.GetMatchingIfExists("tentbag:rotation-info", new object[]
				{
					rot
				})) == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(11, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Rotation: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(rot);
					defaultInterpolatedStringHandler.AppendLiteral("°");
					text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				dsc.Append(text);
			}
			bool flag4 = this.Code != null && this.Code.Domain != "game";
			if (flag4)
			{
				string text2 = "Mod: {0}";
				object[] array = new object[1];
				int num3 = 0;
				Mod mod = this.api.ModLoader.GetMod(this.Code.Domain);
				array[num3] = (((mod != null) ? mod.Info.Name : null) ?? this.Code.Domain);
				dsc.AppendLine(Lang.Get(text2, array));
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004D24 File Offset: 0x00002F24
		private string GetDynamicDescText()
		{
			bool flag = this.Code == null;
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				string hologramKey = this.GetHotkeyDisplay("tentbagpreview", "B");
				string grabAreaKey = this.GetHotkeyDisplay("tentbagreworked-showgraparea", "P");
				string rotateKey = this.GetHotkeyDisplay("tentbagrotate", "R");
				string key = this.Code.Domain + ":itemdesc-" + this.Code.Path;
				result = (Lang.GetMatchingIfExists(key, new object[]
				{
					hologramKey,
					grabAreaKey,
					rotateKey
				}) ?? "");
			}
			return result;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004DCC File Offset: 0x00002FCC
		private string GetHotkeyDisplay(string hotkeyCode, string fallback)
		{
			ICoreClientAPI capi = this.api as ICoreClientAPI;
			bool flag = capi != null;
			if (flag)
			{
				HotKey hotKeyByCode = capi.Input.GetHotKeyByCode(hotkeyCode);
				string text;
				if (hotKeyByCode == null)
				{
					text = null;
				}
				else
				{
					KeyCombination currentMapping = hotKeyByCode.CurrentMapping;
					text = ((currentMapping != null) ? currentMapping.ToString() : null);
				}
				string display = text;
				bool flag2 = !string.IsNullOrEmpty(display);
				if (flag2)
				{
					return display;
				}
			}
			return fallback;
		}

		// Token: 0x04000029 RID: 41
		private const string HotkeyToggleHologram = "tentbagpreview";

		// Token: 0x0400002A RID: 42
		private const string HotkeyGrabArea = "tentbagreworked-showgraparea";

		// Token: 0x0400002B RID: 43
		private const string HotkeyRotate = "tentbagrotate";
	}
}
