using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using Newtonsoft.Json.Linq;
using SimpleExpressionEngine;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.Common;

namespace ConfigLib
{
	// Token: 0x0200003F RID: 63
	[NullableContext(1)]
	[Nullable(0)]
	internal class AssetPatch
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00009EFC File Offset: 0x000080FC
		public string File
		{
			get
			{
				return this._asset;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009F04 File Offset: 0x00008104
		public AssetPatch(ICoreAPI api, string assetPath, Config config)
		{
			HashSet<AssetCategory> hashSet = new HashSet<AssetCategory>();
			hashSet.Add(AssetCategory.itemtypes);
			hashSet.Add(AssetCategory.blocktypes);
			hashSet.Add(AssetCategory.entities);
			hashSet.Add(AssetCategory.recipes);
			this._serverSideCategories = hashSet;
			base..ctor();
			this._api = api;
			this._asset = assetPath;
			JsonObject definition = config.Definition["patches"];
			List<IContext<float, float>> list = new List<IContext<float, float>>();
			list.Add(new MathContext());
			list.Add(new BooleanMathContext());
			list.Add(new NumberSettingsContext(config.Settings));
			CombinedContext<float, float> context = new CombinedContext<float, float>(list);
			if (!this.ConstructBooleanPatches(definition, assetPath, context))
			{
				this._api.Logger.Debug("[Config lib] Error on parsing 'boolean' patches for '" + assetPath + "'.");
			}
			if (!this.ConstructNumberPatches(definition, assetPath, context, "number", ConfigSettingType.Float))
			{
				this._api.Logger.Debug("[Config lib] Error on parsing 'number' patches for '" + assetPath + "'.");
			}
			if (!this.ConstructNumberPatches(definition, assetPath, context, "float", ConfigSettingType.Float))
			{
				this._api.Logger.Debug("[Config lib] Error on parsing 'float' patches for '" + assetPath + "'.");
			}
			if (!this.ConstructNumberPatches(definition, assetPath, context, "integer", ConfigSettingType.Integer))
			{
				this._api.Logger.Debug("[Config lib] Error on parsing 'integer' patches for '" + assetPath + "'.");
			}
			if (!this.ConstructConstPatches(definition, assetPath))
			{
				this._api.Logger.Debug("[Config lib] Error on parsing 'const' patches for '" + assetPath + "'.");
			}
			if (!this.ConstructStringPatches(definition, assetPath, config, context))
			{
				this._api.Logger.Debug("[Config lib] Error on parsing 'string' patches for '" + assetPath + "'.");
			}
			if (!this.ConstructJsonPatches(definition, assetPath, config, context))
			{
				this._api.Logger.Debug("[Config lib] Error on parsing 'other' patches for '" + assetPath + "'.");
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A0F0 File Offset: 0x000082F0
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"successful",
			"failed"
		})]
		public ValueTuple<int, int> Apply(out bool serverSideAsset)
		{
			IEnumerable<ValueTuple<JsonObject, string>> assets;
			if (this._asset.StartsWith('@'))
			{
				assets = this.RetrieveAssetsByWildcard();
				serverSideAsset = false;
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(46, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Retrieved ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Enumerable.Count<ValueTuple<JsonObject, string>>(assets));
				defaultInterpolatedStringHandler.AppendLiteral(" assets by wildcard '");
				defaultInterpolatedStringHandler.AppendFormatted(this._asset);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				logger.VerboseDebug(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else
			{
				JsonObject assetValue = this.RetrieveAsset(out serverSideAsset);
				List<ValueTuple<JsonObject, string>> list = new List<ValueTuple<JsonObject, string>>();
				list.Add(new ValueTuple<JsonObject, string>(assetValue, this._asset));
				assets = list;
				if (serverSideAsset)
				{
					return new ValueTuple<int, int>(0, 0);
				}
				if (assetValue == null)
				{
					this._api.Logger.VerboseDebug("[Config lib] Failed to retrieve asset by path '" + this._asset + "'");
					return new ValueTuple<int, int>(0, 0);
				}
			}
			int failedCount = 0;
			int successfulCount = 0;
			foreach (IValuePatch patch in this._patches)
			{
				foreach (ValueTuple<JsonObject, string> valueTuple in assets)
				{
					JsonObject asset = valueTuple.Item1;
					string path = valueTuple.Item2;
					if (asset != null)
					{
						try
						{
							int pathCount = patch.Apply(asset);
							successfulCount += pathCount;
							ILogger logger2 = this._api.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(39, 2);
							defaultInterpolatedStringHandler2.AppendLiteral("[Config lib] Patched ");
							defaultInterpolatedStringHandler2.AppendFormatted<int>(pathCount);
							defaultInterpolatedStringHandler2.AppendLiteral(" values by path '");
							defaultInterpolatedStringHandler2.AppendFormatted(patch.Path);
							defaultInterpolatedStringHandler2.AppendLiteral("'");
							logger2.VerboseDebug(defaultInterpolatedStringHandler2.ToStringAndClear());
						}
						catch (Exception exception)
						{
							ILogger logger3 = this._api.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(71, 3);
							defaultInterpolatedStringHandler3.AppendLiteral("[Config lib] Failed to apply patch by path '");
							defaultInterpolatedStringHandler3.AppendFormatted(patch.Path);
							defaultInterpolatedStringHandler3.AppendLiteral("' in asset '");
							defaultInterpolatedStringHandler3.AppendFormatted(path);
							defaultInterpolatedStringHandler3.AppendLiteral("'.\nException: ");
							defaultInterpolatedStringHandler3.AppendFormatted<Exception>(exception);
							defaultInterpolatedStringHandler3.AppendLiteral("\n");
							logger3.VerboseDebug(defaultInterpolatedStringHandler3.ToStringAndClear());
							failedCount++;
						}
						this.StoreAsset(asset, path);
					}
				}
			}
			return new ValueTuple<int, int>(successfulCount, failedCount);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000A39C File Offset: 0x0000859C
		[return: TupleElementNames(new string[]
		{
			"asset",
			"path"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			2,
			1
		})]
		private IEnumerable<ValueTuple<JsonObject, string>> RetrieveAssetsByWildcard()
		{
			string wildcard = this._asset.Substring(1, this._asset.Length - 1);
			return Enumerable.Select<ValueTuple<bool, JsonObject, string>, ValueTuple<JsonObject, string>>(Enumerable.Where<ValueTuple<bool, JsonObject, string>>(Enumerable.Where<ValueTuple<bool, JsonObject, string>>(Enumerable.Select<string, ValueTuple<bool, JsonObject, string>>(Enumerable.Select<AssetLocation, string>(Enumerable.Where<AssetLocation>(this._api.Assets.GetLocations("", null), (AssetLocation path) => WildcardUtil.Match(wildcard, path.ToString())), (AssetLocation path) => path.ToString()), new Func<string, ValueTuple<bool, JsonObject, string>>(this.RetrieveAsset)), ([TupleElementNames(new string[]
			{
				"serverSide",
				"asset",
				"path"
			})] ValueTuple<bool, JsonObject, string> entry) => !entry.Item1), ([TupleElementNames(new string[]
			{
				"serverSide",
				"asset",
				"path"
			})] ValueTuple<bool, JsonObject, string> entry) => entry.Item2 != null), ([TupleElementNames(new string[]
			{
				"serverSide",
				"asset",
				"path"
			})] ValueTuple<bool, JsonObject, string> entry) => new ValueTuple<JsonObject, string>(entry.Item2, entry.Item3));
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000A496 File Offset: 0x00008696
		[NullableContext(2)]
		private JsonObject RetrieveAsset(out bool serverSide)
		{
			return this.RetrieveAsset(this._asset, out serverSide);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000A4A8 File Offset: 0x000086A8
		[return: TupleElementNames(new string[]
		{
			"serverSide",
			"asset",
			"path"
		})]
		[return: Nullable(new byte[]
		{
			0,
			2,
			1
		})]
		private ValueTuple<bool, JsonObject, string> RetrieveAsset(string path)
		{
			bool serverSide;
			JsonObject asset = this.RetrieveAsset(path, out serverSide);
			return new ValueTuple<bool, JsonObject, string>(serverSide, asset, path);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000A4C8 File Offset: 0x000086C8
		[return: Nullable(2)]
		private JsonObject RetrieveAsset(string path, out bool serverSide)
		{
			serverSide = false;
			AssetLocation location = new AssetLocation(path);
			if (this._api.Side == 2 && this._serverSideCategories.Contains(location.Category))
			{
				serverSide = true;
				return null;
			}
			IAsset asset;
			try
			{
				asset = this._api.Assets.Get(path);
			}
			catch
			{
				this._api.Logger.Debug("[Config lib] Asset '" + path + "' not found, skipping it.");
				return null;
			}
			if (asset == null)
			{
				return null;
			}
			string json = Asset.BytesToString(asset.Data);
			JsonObject result;
			try
			{
				result = new JsonObject(JArray.Parse(json));
			}
			catch
			{
				try
				{
					result = new JsonObject(JObject.Parse(json));
				}
				catch
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000A5A0 File Offset: 0x000087A0
		private void StoreAsset(JsonObject data, string path)
		{
			IAsset asset = this._api.Assets.Get(path);
			if (asset == null)
			{
				return;
			}
			asset.Data = Encoding.UTF8.GetBytes(data.ToString());
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000A5E0 File Offset: 0x000087E0
		private bool ConstructBooleanPatches(JsonObject definition, string assetPath, CombinedContext<float, float> context)
		{
			if (!definition.KeyExists("boolean"))
			{
				return true;
			}
			if (!definition["boolean"].KeyExists(assetPath))
			{
				return true;
			}
			JsonObject jsonObject = definition["boolean"][assetPath];
			JObject booleanPatches = ((jsonObject != null) ? jsonObject.Token : null) as JObject;
			if (booleanPatches == null)
			{
				return false;
			}
			bool failed = false;
			foreach (KeyValuePair<string, JToken> keyValuePair in booleanPatches)
			{
				string text;
				JToken jtoken;
				keyValuePair.Deconstruct(ref text, ref jtoken);
				string key = text;
				JToken value = jtoken;
				JValue boolValue = value as JValue;
				if (boolValue == null || boolValue.Type != 8)
				{
					ILogger logger = this._api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 3);
					defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Error on parsing patch '");
					defaultInterpolatedStringHandler.AppendFormatted(assetPath);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted(key);
					defaultInterpolatedStringHandler.AppendLiteral("': patch value '");
					defaultInterpolatedStringHandler.AppendFormatted<JToken>(value);
					defaultInterpolatedStringHandler.AppendLiteral("' is not string.");
					logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					failed = true;
				}
				else
				{
					string setting = ((string)boolValue.Value) ?? "";
					setting = this.Process(setting, context);
					this._patches.Add(new BooleanPatch(key, setting, context));
				}
			}
			return !failed;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000A748 File Offset: 0x00008948
		private bool ConstructStringPatches(JsonObject definition, string assetPath, Config config, CombinedContext<float, float> context)
		{
			if (!definition.KeyExists("string"))
			{
				return true;
			}
			if (!definition["string"].KeyExists(assetPath))
			{
				return true;
			}
			JsonObject jsonObject = definition["string"][assetPath];
			JObject stringPatches = ((jsonObject != null) ? jsonObject.Token : null) as JObject;
			if (stringPatches == null)
			{
				return false;
			}
			bool failed = false;
			foreach (KeyValuePair<string, JToken> keyValuePair in stringPatches)
			{
				string text;
				JToken jtoken;
				keyValuePair.Deconstruct(ref text, ref jtoken);
				string key = text;
				JToken value = jtoken;
				JValue boolValue = value as JValue;
				if (boolValue == null || boolValue.Type != 8)
				{
					ILogger logger = this._api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 3);
					defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Error on parsing patch '");
					defaultInterpolatedStringHandler.AppendFormatted(assetPath);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted(key);
					defaultInterpolatedStringHandler.AppendLiteral("': patch value '");
					defaultInterpolatedStringHandler.AppendFormatted<JToken>(value);
					defaultInterpolatedStringHandler.AppendLiteral("' is not string.");
					logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					failed = true;
				}
				else
				{
					string setting = ((string)boolValue.Value) ?? "";
					setting = this.Process(setting, context);
					if (!(setting == "value"))
					{
						this._patches.Add(new StringPatch(key, setting, config));
					}
				}
			}
			return !failed;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000A8C0 File Offset: 0x00008AC0
		private bool ConstructJsonPatches(JsonObject definition, string assetPath, Config config, CombinedContext<float, float> context)
		{
			if (!definition.KeyExists("other"))
			{
				return true;
			}
			if (!definition["other"].KeyExists(assetPath))
			{
				return true;
			}
			JsonObject jsonObject = definition["other"][assetPath];
			JObject jsonPatches = ((jsonObject != null) ? jsonObject.Token : null) as JObject;
			if (jsonPatches == null)
			{
				return false;
			}
			bool failed = false;
			foreach (KeyValuePair<string, JToken> keyValuePair in jsonPatches)
			{
				string text;
				JToken jtoken;
				keyValuePair.Deconstruct(ref text, ref jtoken);
				string key = text;
				JToken value = jtoken;
				JValue boolValue = value as JValue;
				if (boolValue == null || boolValue.Type != 8)
				{
					ILogger logger = this._api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 3);
					defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Error on parsing patch '");
					defaultInterpolatedStringHandler.AppendFormatted(assetPath);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted(key);
					defaultInterpolatedStringHandler.AppendLiteral("': patch value '");
					defaultInterpolatedStringHandler.AppendFormatted<JToken>(value);
					defaultInterpolatedStringHandler.AppendLiteral("' is not string.");
					logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					failed = true;
				}
				else
				{
					string setting = ((string)boolValue.Value) ?? "";
					setting = this.Process(setting, context);
					if (!(setting == "value"))
					{
						if (config.GetSetting(setting) == null)
						{
							ILogger logger2 = this._api.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(68, 3);
							defaultInterpolatedStringHandler2.AppendLiteral("[Config lib] Error on parsing patch '");
							defaultInterpolatedStringHandler2.AppendFormatted(assetPath);
							defaultInterpolatedStringHandler2.AppendLiteral("/");
							defaultInterpolatedStringHandler2.AppendFormatted(key);
							defaultInterpolatedStringHandler2.AppendLiteral("': other setting '");
							defaultInterpolatedStringHandler2.AppendFormatted(setting);
							defaultInterpolatedStringHandler2.AppendLiteral("' not found.");
							logger2.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
							failed = true;
						}
						else
						{
							this._patches.Add(new JsonPatch(key, setting, config));
						}
					}
				}
			}
			return !failed;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000AAC0 File Offset: 0x00008CC0
		private bool ConstructNumberPatches(JsonObject definition, string assetPath, CombinedContext<float, float> context, string category, ConfigSettingType type)
		{
			if (!definition.KeyExists(category))
			{
				return true;
			}
			if (!definition[category].KeyExists(assetPath))
			{
				return true;
			}
			JsonObject jsonObject = definition[category][assetPath];
			JObject jsonPatches = ((jsonObject != null) ? jsonObject.Token : null) as JObject;
			if (jsonPatches == null)
			{
				return false;
			}
			bool failed = false;
			foreach (KeyValuePair<string, JToken> keyValuePair in jsonPatches)
			{
				string text;
				JToken jtoken;
				keyValuePair.Deconstruct(ref text, ref jtoken);
				string key = text;
				JToken value = jtoken;
				JValue formula = value as JValue;
				if (formula == null || formula.Type != 8)
				{
					ILogger logger = this._api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 4);
					defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Error on parsing '");
					defaultInterpolatedStringHandler.AppendFormatted(category);
					defaultInterpolatedStringHandler.AppendLiteral("' patch '");
					defaultInterpolatedStringHandler.AppendFormatted(assetPath);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted(key);
					defaultInterpolatedStringHandler.AppendLiteral("': patch value '");
					defaultInterpolatedStringHandler.AppendFormatted<JToken>(value);
					defaultInterpolatedStringHandler.AppendLiteral("' is not string.");
					logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					failed = true;
				}
				else
				{
					string setting = ((string)formula.Value) ?? "";
					setting = this.Process(setting, context);
					this._patches.Add(new NumberPatch(key, setting, context, type));
				}
			}
			return !failed;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000AC44 File Offset: 0x00008E44
		private bool ConstructConstPatches(JsonObject definition, string assetPath)
		{
			if (!definition.KeyExists("const"))
			{
				return true;
			}
			if (!definition["const"].KeyExists(assetPath))
			{
				return true;
			}
			JsonObject jsonObject = definition["const"][assetPath];
			JObject jsonPatches = ((jsonObject != null) ? jsonObject.Token : null) as JObject;
			if (jsonPatches == null)
			{
				return false;
			}
			bool failed = false;
			foreach (KeyValuePair<string, JToken> keyValuePair in jsonPatches)
			{
				string text;
				JToken jtoken;
				keyValuePair.Deconstruct(ref text, ref jtoken);
				string key = text;
				JToken value = jtoken;
				this._patches.Add(new ConstPatch(key, new JsonObject(value)));
			}
			return !failed;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000AD04 File Offset: 0x00008F04
		[GeneratedRegex("\\((?<expression>.+)\\) \\? (?<true>.+) \\: (?<false>.+)", 8)]
		[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.37416")]
		private static Regex GetBooleanExpressionRegex()
		{
			return <RegexGenerator_g>F7CDC02ACD2274424726E5BB4CB938FCE26A3C714A357453E70371D3BC4916E37__GetBooleanExpressionRegex_0.Instance;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000AD0C File Offset: 0x00008F0C
		private string Process(string value, IContext<float, float> context)
		{
			Match match = AssetPatch._booleanExpressionRegex.Match(value);
			if (!match.Success)
			{
				return value;
			}
			string expression = match.Groups["expression"].Value;
			string onTrue = match.Groups["true"].Value;
			string onFalse = match.Groups["false"].Value;
			if (Math.Abs(MathParser.Parse(expression).Evaluate(context)) <= 1E-45f)
			{
				return onFalse;
			}
			return onTrue;
		}

		// Token: 0x040000D1 RID: 209
		private readonly string _asset;

		// Token: 0x040000D2 RID: 210
		private readonly List<IValuePatch> _patches = new List<IValuePatch>();

		// Token: 0x040000D3 RID: 211
		private readonly ICoreAPI _api;

		// Token: 0x040000D4 RID: 212
		private readonly HashSet<AssetCategory> _serverSideCategories;

		// Token: 0x040000D5 RID: 213
		private static readonly Regex _booleanExpressionRegex = AssetPatch.GetBooleanExpressionRegex();

		// Token: 0x040000D6 RID: 214
		private const float _epsilon = 1E-45f;
	}
}
