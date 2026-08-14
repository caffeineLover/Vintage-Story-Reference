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
	// Token: 0x0200004C RID: 76
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ConfigRegistry : RecipeRegistryBase
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000227 RID: 551 RVA: 0x0000CDDC File Offset: 0x0000AFDC
		// (remove) Token: 0x06000228 RID: 552 RVA: 0x0000CE10 File Offset: 0x0000B010
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
		// (add) Token: 0x06000229 RID: 553 RVA: 0x0000CE44 File Offset: 0x0000B044
		// (remove) Token: 0x0600022A RID: 554 RVA: 0x0000CE78 File Offset: 0x0000B078
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public static event Action OnToBytes;

		// Token: 0x0600022B RID: 555 RVA: 0x0000CEAC File Offset: 0x0000B0AC
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

		// Token: 0x0600022C RID: 556 RVA: 0x0000D018 File Offset: 0x0000B218
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

		// Token: 0x0600022D RID: 557 RVA: 0x0000D188 File Offset: 0x0000B388
		public void Register(string domain, Config config)
		{
			this._configs.Add(domain, config);
		}

		// Token: 0x04000115 RID: 277
		private readonly Dictionary<string, Config> _configs = new Dictionary<string, Config>();
	}
}
