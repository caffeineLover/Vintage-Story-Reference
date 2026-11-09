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
	// Token: 0x02000164 RID: 356
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PlayerModelLibCompatibilitySystem : ModSystem
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060011CD RID: 4557 RVA: 0x00054E98 File Offset: 0x00053098
		// (set) Token: 0x060011CE RID: 4558 RVA: 0x00054EA0 File Offset: 0x000530A0
		public Dictionary<string, PlayerModelConfig> CustomModelConfigs { get; private set; } = new Dictionary<string, PlayerModelConfig>();

		// Token: 0x060011CF RID: 4559 RVA: 0x00054EA9 File Offset: 0x000530A9
		public override double ExecuteOrder()
		{
			return 0.209;
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00054EB4 File Offset: 0x000530B4
		public override bool ShouldLoad(ICoreAPI api)
		{
			return api.ModLoader.IsModEnabled("playermodellib") && base.ShouldLoad(api);
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00054ED1 File Offset: 0x000530D1
		public override void Start(ICoreAPI api)
		{
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00054ED3 File Offset: 0x000530D3
		public override void AssetsFinalize(ICoreAPI api)
		{
			this.LoadModelsData(api);
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x00054EDC File Offset: 0x000530DC
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

		// Token: 0x060011D4 RID: 4564 RVA: 0x00054FC8 File Offset: 0x000531C8
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

		// Token: 0x04000842 RID: 2114
		public const string PlayerModelLibId = "playermodellib";

		// Token: 0x04000843 RID: 2115
		private const string _playerModelLibConfigs = "customplayermodels";
	}
}
