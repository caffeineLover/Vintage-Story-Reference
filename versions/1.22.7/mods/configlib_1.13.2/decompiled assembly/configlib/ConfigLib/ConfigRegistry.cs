using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.Common;

namespace ConfigLib
{
	// Token: 0x0200004E RID: 78
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ConfigRegistry : RecipeRegistryBase
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600024C RID: 588 RVA: 0x0000D0F8 File Offset: 0x0000B2F8
		// (remove) Token: 0x0600024D RID: 589 RVA: 0x0000D12C File Offset: 0x0000B32C
		[Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})]
		public static event Action<Dictionary<string, Config>> ConfigsLoaded;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600024E RID: 590 RVA: 0x0000D160 File Offset: 0x0000B360
		// (remove) Token: 0x0600024F RID: 591 RVA: 0x0000D194 File Offset: 0x0000B394
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public static event Action OnToBytes;

		// Token: 0x06000250 RID: 592 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
		public override void FromBytes(IWorldAccessor resolver, int quantity, byte[] data)
		{
			using (MemoryStream serializedRecipesList = new MemoryStream(data))
			{
				using (BinaryReader reader = new BinaryReader(serializedRecipesList))
				{
					for (int count = 0; count < quantity; count++)
					{
						string domain = reader.ReadString();
						string modName = reader.ReadString();
						int num = reader.ReadInt32();
						string jsonFile = reader.ReadString();
						int length = reader.ReadInt32();
						SettingsPacket packet = SerializerUtil.Deserialize<SettingsPacket>(reader.ReadBytes(length));
						Config config;
						if (num == 1)
						{
							config = new Config(resolver.Api, packet.Domain, modName, new JsonObject(JObject.Parse(Asset.BytesToString(packet.Definition))), jsonFile, packet.GetSettings());
						}
						else
						{
							config = new Config(resolver.Api, packet.Domain, modName, new JsonObject(JObject.Parse(Asset.BytesToString(packet.Definition))), packet.GetSettings());
						}
						this._configs[domain] = config;
					}
					ILogger logger = resolver.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 1);
					defaultInterpolatedStringHandler.AppendLiteral("[Config lib] [Registry] Received config from server: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(quantity);
					logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
					Action<Dictionary<string, Config>> configsLoaded = ConfigRegistry.ConfigsLoaded;
					if (configsLoaded != null)
					{
						configsLoaded.Invoke(this._configs);
					}
				}
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000D334 File Offset: 0x0000B534
		public override void ToBytes(IWorldAccessor resolver, out byte[] data, out int quantity)
		{
			quantity = 0;
			using (MemoryStream serializedConfigs = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(serializedConfigs))
				{
					foreach (KeyValuePair<string, Config> keyValuePair in this._configs)
					{
						string text;
						Config config2;
						keyValuePair.Deconstruct(ref text, ref config2);
						string domain = text;
						Config config = config2;
						writer.Write(domain);
						writer.Write(config.ModName);
						writer.Write((int)config.FileType);
						writer.Write(config.RelativeFilePath);
						byte[] configData = SerializerUtil.Serialize<SettingsPacket>(new SettingsPacket(domain, config.Settings, config.Definition));
						writer.Write(configData.Length);
						writer.Write(configData);
						quantity++;
					}
					ILogger logger = resolver.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 1);
					defaultInterpolatedStringHandler.AppendLiteral("[Config lib] [Registry] Configs prepared to send to client: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(quantity);
					logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
					data = serializedConfigs.ToArray();
					Action onToBytes = ConfigRegistry.OnToBytes;
					if (onToBytes != null)
					{
						onToBytes.Invoke();
					}
				}
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000D4A4 File Offset: 0x0000B6A4
		public void Register(string domain, Config config)
		{
			this._configs.Add(domain, config);
		}

		// Token: 0x04000118 RID: 280
		private readonly Dictionary<string, Config> _configs = new Dictionary<string, Config>();
	}
}
