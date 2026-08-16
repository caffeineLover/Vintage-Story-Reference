using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x02000158 RID: 344
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PlayerModelLibCompatibilitySystem : ModSystem
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001159 RID: 4441 RVA: 0x00053590 File Offset: 0x00051790
		// (set) Token: 0x0600115A RID: 4442 RVA: 0x00053598 File Offset: 0x00051798
		public Dictionary<string, PlayerModelConfig> CustomModelConfigs { get; private set; } = new Dictionary<string, PlayerModelConfig>();

		// Token: 0x0600115B RID: 4443 RVA: 0x000535A1 File Offset: 0x000517A1
		public override double ExecuteOrder()
		{
			return 0.209;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x000535AC File Offset: 0x000517AC
		public override bool ShouldLoad(ICoreAPI api)
		{
			return api.ModLoader.IsModEnabled("playermodellib") && base.ShouldLoad(api);
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x000535C9 File Offset: 0x000517C9
		public override void Start(ICoreAPI api)
		{
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x000535CB File Offset: 0x000517CB
		public override void AssetsFinalize(ICoreAPI api)
		{
			this.LoadModelsData(api);
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x000535D4 File Offset: 0x000517D4
		private void LoadModelsData(ICoreAPI api)
		{
			IEnumerable<IAsset> manyInCategory = api.Assets.GetManyInCategory("config", "customplayermodels", null, true);
			Func<IAsset, Dictionary<string, PlayerModelConfig>> <>9__0;
			Func<IAsset, Dictionary<string, PlayerModelConfig>> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = ((IAsset asset) => this.FromAsset(asset, api)));
			}
			foreach (Dictionary<string, PlayerModelConfig> dictionary in Enumerable.Select<IAsset, Dictionary<string, PlayerModelConfig>>(manyInCategory, func))
			{
				foreach (KeyValuePair<string, PlayerModelConfig> keyValuePair in dictionary)
				{
					string text;
					PlayerModelConfig playerModelConfig;
					keyValuePair.Deconstruct(ref text, ref playerModelConfig);
					string code = text;
					PlayerModelConfig modelConfig = playerModelConfig;
					this.CustomModelConfigs.Add(code, modelConfig);
				}
			}
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x000536C0 File Offset: 0x000518C0
		private Dictionary<string, PlayerModelConfig> FromAsset(IAsset asset, ICoreAPI api)
		{
			Dictionary<string, PlayerModelConfig> result = new Dictionary<string, PlayerModelConfig>();
			string domain = asset.Location.Domain;
			JObject json;
			try
			{
				json = (JsonObject.FromJson(asset.ToText()).Token as JObject);
				if (json == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Error when trying to load model config '");
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(asset.Location);
					defaultInterpolatedStringHandler.AppendLiteral("'.");
					LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
					return result;
				}
			}
			catch (Exception exception)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(47, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Exception when trying to load model config '");
				defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>(asset.Location);
				defaultInterpolatedStringHandler2.AppendLiteral("':\n");
				defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api, this, defaultInterpolatedStringHandler2.ToStringAndClear());
				return result;
			}
			foreach (KeyValuePair<string, JToken> keyValuePair in json)
			{
				string text;
				JToken jtoken;
				keyValuePair.Deconstruct(ref text, ref jtoken);
				string code = text;
				JToken token = jtoken;
				try
				{
					PlayerModelConfigJson config = new JsonObject(token).AsObject<PlayerModelConfigJson>(null);
					if (config.CombatOverhaulConfig != null)
					{
						config.CombatOverhaulConfig.Domain = domain;
						result.Add(domain + ":" + code, config.CombatOverhaulConfig);
					}
				}
				catch (Exception exception2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(60, 3);
					defaultInterpolatedStringHandler3.AppendLiteral("Exception when trying to load model config '");
					defaultInterpolatedStringHandler3.AppendFormatted<AssetLocation>(asset.Location);
					defaultInterpolatedStringHandler3.AppendLiteral("' for model '");
					defaultInterpolatedStringHandler3.AppendFormatted(code);
					defaultInterpolatedStringHandler3.AppendLiteral("':\n");
					defaultInterpolatedStringHandler3.AppendFormatted<Exception>(exception2);
					LoggerUtil.Error(api, this, defaultInterpolatedStringHandler3.ToStringAndClear());
				}
			}
			return result;
		}

		// Token: 0x0400081C RID: 2076
		public const string PlayerModelLibId = "playermodellib";

		// Token: 0x0400081D RID: 2077
		private const string _playerModelLibConfigs = "customplayermodels";
	}
}
