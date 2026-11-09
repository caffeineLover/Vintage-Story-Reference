using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace RPVoiceChat.GameContent.Inventory
{
	// Token: 0x020000D0 RID: 208
	public class SlotPrinterTelegram : SlotRestrictedItemPath
	{
		// Token: 0x0600063E RID: 1598 RVA: 0x00021CAB File Offset: 0x0001FEAB
		public SlotPrinterTelegram(InventoryBase inventory) : base(inventory, "telegram", 1)
		{
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00021CBC File Offset: 0x0001FEBC
		public bool TryStoreTelegram(string message, string networkUID = "", string networkName = "", string sourceEndpointName = "", string targetEndpointName = "")
		{
			if (this.Empty)
			{
				ItemStack telegramStack = new ItemStack(this.inventory.Api.World.GetItem(new AssetLocation("rpvoicechat:telegram")), 1);
				telegramStack.Attributes.SetString("message", message);
				telegramStack.Attributes.SetLong("timestamp", (long)this.inventory.Api.World.Calendar.TotalDays);
				if (!string.IsNullOrEmpty(networkUID))
				{
					telegramStack.Attributes.SetString("networkUID", networkUID);
				}
				if (!string.IsNullOrWhiteSpace(networkName))
				{
					telegramStack.Attributes.SetString("networkName", networkName);
				}
				if (!string.IsNullOrWhiteSpace(sourceEndpointName))
				{
					telegramStack.Attributes.SetString("sourceEndpointName", sourceEndpointName);
				}
				if (!string.IsNullOrWhiteSpace(targetEndpointName))
				{
					telegramStack.Attributes.SetString("targetEndpointName", targetEndpointName);
				}
				telegramStack.Attributes.SetString("text", message);
				telegramStack.Attributes.SetString("title", SlotPrinterTelegram.CreateTelegramTitle(networkUID, networkName, sourceEndpointName, targetEndpointName));
				string description = SlotPrinterTelegram.CreateTelegramDescription(message, networkUID, networkName, sourceEndpointName, targetEndpointName);
				telegramStack.Attributes.SetString("description", description);
				base.Itemstack = telegramStack;
				this.MarkDirty();
				return true;
			}
			return false;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00021DF8 File Offset: 0x0001FFF8
		private static string CreateTelegramDescription(string message, string networkUID, string networkName, string sourceEndpointName, string targetEndpointName)
		{
			string networkLabel = SlotPrinterTelegram.ResolveNetworkLabel(networkUID, networkName);
			if (!string.IsNullOrWhiteSpace(sourceEndpointName))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(22, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Network: ");
				defaultInterpolatedStringHandler.AppendFormatted(sourceEndpointName);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted(networkLabel);
				defaultInterpolatedStringHandler.AppendLiteral(")\nMessage: ");
				defaultInterpolatedStringHandler.AppendFormatted(message);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (!string.IsNullOrWhiteSpace(targetEndpointName))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(22, 3);
				defaultInterpolatedStringHandler2.AppendLiteral("Network: ");
				defaultInterpolatedStringHandler2.AppendFormatted(targetEndpointName);
				defaultInterpolatedStringHandler2.AppendLiteral(" (");
				defaultInterpolatedStringHandler2.AppendFormatted(networkLabel);
				defaultInterpolatedStringHandler2.AppendLiteral(")\nMessage: ");
				defaultInterpolatedStringHandler2.AppendFormatted(message);
				return defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			return "Network: " + networkLabel + "\nMessage: " + message;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00021ECC File Offset: 0x000200CC
		private static string CreateTelegramTitle(string networkUID, string networkName, string sourceEndpointName, string targetEndpointName)
		{
			string networkLabel = SlotPrinterTelegram.ResolveNetworkLabel(networkUID, networkName);
			if (!string.IsNullOrWhiteSpace(sourceEndpointName))
			{
				if (!string.IsNullOrWhiteSpace(networkLabel))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(22, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Telegram - ");
					defaultInterpolatedStringHandler.AppendFormatted(sourceEndpointName);
					defaultInterpolatedStringHandler.AppendLiteral(" (Network ");
					defaultInterpolatedStringHandler.AppendFormatted(networkLabel);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return "Telegram - " + sourceEndpointName;
			}
			else if (!string.IsNullOrWhiteSpace(targetEndpointName))
			{
				if (!string.IsNullOrWhiteSpace(networkLabel))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(22, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("Telegram - ");
					defaultInterpolatedStringHandler2.AppendFormatted(targetEndpointName);
					defaultInterpolatedStringHandler2.AppendLiteral(" (Network ");
					defaultInterpolatedStringHandler2.AppendFormatted(networkLabel);
					defaultInterpolatedStringHandler2.AppendLiteral(")");
					return defaultInterpolatedStringHandler2.ToStringAndClear();
				}
				return "Telegram - " + targetEndpointName;
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(networkLabel))
				{
					return "Telegram - Network " + networkLabel;
				}
				return "Telegram";
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00021FBE File Offset: 0x000201BE
		private static string ResolveNetworkLabel(string networkUID, string networkName)
		{
			if (!string.IsNullOrWhiteSpace(networkName))
			{
				return networkName;
			}
			if (!string.IsNullOrWhiteSpace(networkUID))
			{
				return networkUID;
			}
			return "Unknown";
		}
	}
}
