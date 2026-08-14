using System;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace RPVoiceChat.GameContent.Items
{
	// Token: 0x0200009E RID: 158
	public class ItemTelegram : ItemBook
	{
		// Token: 0x060004E9 RID: 1257 RVA: 0x0001A36C File Offset: 0x0001856C
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			ItemStack itemstack = inSlot.Itemstack;
			if (((itemstack != null) ? itemstack.Attributes : null) != null)
			{
				string networkUID = inSlot.Itemstack.Attributes.GetString("networkUID", "");
				string networkName = inSlot.Itemstack.Attributes.GetString("networkName", "");
				string networkLabel = (!string.IsNullOrWhiteSpace(networkName)) ? networkName : networkUID;
				if (!string.IsNullOrEmpty(networkLabel))
				{
					dsc.AppendLine();
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
					appendInterpolatedStringHandler..ctor(9, 1, dsc);
					appendInterpolatedStringHandler.AppendLiteral("Network: ");
					appendInterpolatedStringHandler.AppendFormatted(networkLabel);
					dsc.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001A414 File Offset: 0x00018614
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
		{
			ItemStack itemstack = slot.Itemstack;
			if (((itemstack != null) ? itemstack.Attributes : null) != null)
			{
				string message = slot.Itemstack.Attributes.GetString("message", "");
				string networkUID = slot.Itemstack.Attributes.GetString("networkUID", "");
				string networkName = slot.Itemstack.Attributes.GetString("networkName", "");
				string networkLabel = (!string.IsNullOrWhiteSpace(networkName)) ? networkName : networkUID;
				string sourceEndpointName = slot.Itemstack.Attributes.GetString("sourceEndpointName", "");
				string targetEndpointName = slot.Itemstack.Attributes.GetString("targetEndpointName", "");
				if (!slot.Itemstack.Attributes.HasAttribute("text"))
				{
					string text = (!string.IsNullOrEmpty(message)) ? message : "Empty telegram";
					string title = "Telegram";
					if (!string.IsNullOrWhiteSpace(sourceEndpointName))
					{
						string text2 = title;
						string text3;
						if (string.IsNullOrEmpty(networkLabel))
						{
							text3 = " - " + sourceEndpointName;
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
							defaultInterpolatedStringHandler..ctor(14, 2);
							defaultInterpolatedStringHandler.AppendLiteral(" - ");
							defaultInterpolatedStringHandler.AppendFormatted(sourceEndpointName);
							defaultInterpolatedStringHandler.AppendLiteral(" (Network ");
							defaultInterpolatedStringHandler.AppendFormatted(networkLabel);
							defaultInterpolatedStringHandler.AppendLiteral(")");
							text3 = defaultInterpolatedStringHandler.ToStringAndClear();
						}
						title = text2 + text3;
					}
					else if (!string.IsNullOrWhiteSpace(targetEndpointName))
					{
						string text4 = title;
						string text5;
						if (string.IsNullOrEmpty(networkLabel))
						{
							text5 = " - " + targetEndpointName;
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
							defaultInterpolatedStringHandler2..ctor(14, 2);
							defaultInterpolatedStringHandler2.AppendLiteral(" - ");
							defaultInterpolatedStringHandler2.AppendFormatted(targetEndpointName);
							defaultInterpolatedStringHandler2.AppendLiteral(" (Network ");
							defaultInterpolatedStringHandler2.AppendFormatted(networkLabel);
							defaultInterpolatedStringHandler2.AppendLiteral(")");
							text5 = defaultInterpolatedStringHandler2.ToStringAndClear();
						}
						title = text4 + text5;
					}
					else if (!string.IsNullOrEmpty(networkLabel))
					{
						title = title + " - Network " + networkLabel;
					}
					slot.Itemstack.Attributes.SetString("text", text);
					slot.Itemstack.Attributes.SetString("title", title);
				}
			}
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001A638 File Offset: 0x00018838
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					MouseButton = 2,
					ActionLangCode = "heldhelp-read",
					ShouldApply = ((WorldInteraction wi, BlockSelection bs, EntitySelection es) => ItemBook.isReadable(inSlot) && (inSlot.Itemstack.Attributes.HasAttribute("text") || inSlot.Itemstack.Attributes.HasAttribute("textCodes")))
				}
			};
		}
	}
}
