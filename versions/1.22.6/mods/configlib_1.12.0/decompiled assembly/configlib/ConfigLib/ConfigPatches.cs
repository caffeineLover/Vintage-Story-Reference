using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace ConfigLib
{
	// Token: 0x0200003C RID: 60
	[NullableContext(1)]
	[Nullable(0)]
	public class ConfigPatches
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x00009830 File Offset: 0x00007A30
		public ConfigPatches(ICoreAPI api, Config config)
		{
			this._api = api;
			foreach (string filePath in ConfigPatches.GetFiles(config.Definition["patches"]))
			{
				try
				{
					this._patches.Add(new AssetPatch(api, filePath, config));
				}
				catch (Exception exception)
				{
					this._api.Logger.Warning("[Config lib] Exception on creating patch to asset '" + filePath + "'.");
					ILogger logger = this._api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
					defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Exception on creating patch to asset '");
					defaultInterpolatedStringHandler.AppendFormatted(filePath);
					defaultInterpolatedStringHandler.AppendLiteral("':\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					defaultInterpolatedStringHandler.AppendLiteral("\n.");
					logger.VerboseDebug(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009944 File Offset: 0x00007B44
		public void Apply()
		{
			foreach (AssetPatch patch in this._patches)
			{
				try
				{
					bool serverSide;
					ValueTuple<int, int> valueTuple = patch.Apply(out serverSide);
					int successful = valueTuple.Item1;
					int failed = valueTuple.Item2;
					if (!serverSide && failed == 0)
					{
						ILogger logger = this._api.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
						defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Values patched: ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(successful);
						defaultInterpolatedStringHandler.AppendLiteral(" in asset: '");
						defaultInterpolatedStringHandler.AppendFormatted(patch.File);
						defaultInterpolatedStringHandler.AppendLiteral("'.");
						logger.VerboseDebug(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					else
					{
						ILogger logger2 = this._api.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(53, 3);
						defaultInterpolatedStringHandler2.AppendLiteral("[Config lib] Values patched: ");
						defaultInterpolatedStringHandler2.AppendFormatted<int>(successful);
						defaultInterpolatedStringHandler2.AppendLiteral(", failed: ");
						defaultInterpolatedStringHandler2.AppendFormatted<int>(failed);
						defaultInterpolatedStringHandler2.AppendLiteral(" in asset: '");
						defaultInterpolatedStringHandler2.AppendFormatted(patch.File);
						defaultInterpolatedStringHandler2.AppendLiteral("'.");
						logger2.VerboseDebug(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
				}
				catch (Exception exception)
				{
					this._api.Logger.Debug("[Config lib] Exception on applying patch to asset: '" + patch.File + "'.");
					ILogger logger3 = this._api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(53, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("[Config lib] Exception on applying patch to asset:\n");
					defaultInterpolatedStringHandler3.AppendFormatted<Exception>(exception);
					defaultInterpolatedStringHandler3.AppendLiteral("\n.");
					logger3.VerboseDebug(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00009B10 File Offset: 0x00007D10
		public static HashSet<string> GetFiles([Nullable(2)] JsonObject definition)
		{
			HashSet<string> files = new HashSet<string>();
			JObject categories = ((definition != null) ? definition.Token : null) as JObject;
			if (categories == null)
			{
				return files;
			}
			foreach (KeyValuePair<string, JToken> keyValuePair in categories)
			{
				string text;
				JToken jtoken;
				keyValuePair.Deconstruct(ref text, ref jtoken);
				JObject categoryObject = jtoken as JObject;
				if (categoryObject != null)
				{
					using (IEnumerator<KeyValuePair<string, JToken>> enumerator2 = categoryObject.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							keyValuePair = enumerator2.Current;
							keyValuePair.Deconstruct(ref text, ref jtoken);
							string file = text;
							if (!files.Contains(file))
							{
								files.Add(file);
							}
						}
					}
				}
			}
			return files;
		}

		// Token: 0x040000CC RID: 204
		private readonly ICoreAPI _api;

		// Token: 0x040000CD RID: 205
		private readonly List<AssetPatch> _patches = new List<AssetPatch>();
	}
}
