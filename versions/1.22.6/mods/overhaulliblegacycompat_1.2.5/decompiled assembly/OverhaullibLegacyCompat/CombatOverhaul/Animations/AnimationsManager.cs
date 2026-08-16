using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Utils;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200019A RID: 410
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnimationsManager
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x00062EDE File Offset: 0x000610DE
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x00062EE6 File Offset: 0x000610E6
		public Dictionary<string, Animation> Animations { get; private set; } = new Dictionary<string, Animation>();

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x00062EEF File Offset: 0x000610EF
		internal Dictionary<string, AnimationSource> AnimationSources { get; } = new Dictionary<string, AnimationSource>();

		// Token: 0x060013E3 RID: 5091 RVA: 0x00062EF7 File Offset: 0x000610F7
		public AnimationsManager(ICoreClientAPI api, ParticleEffectsManager particleEffectsManager)
		{
			this._api = api;
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00062F1C File Offset: 0x0006111C
		public void Load()
		{
			IEnumerable<IAsset> manyInCategory = this._api.Assets.GetManyInCategory("config", "animations", null, true);
			Dictionary<string, Animation> animationsByCode = new Dictionary<string, Animation>();
			this.AnimationSources.Clear();
			foreach (AnimationsManager.LoadedAnimation loadedAnimation in Enumerable.SelectMany<IAsset, AnimationsManager.LoadedAnimation>(manyInCategory, new Func<IAsset, IEnumerable<AnimationsManager.LoadedAnimation>>(this.FromAsset)))
			{
				if (!animationsByCode.TryAdd(loadedAnimation.Code, loadedAnimation.Animation))
				{
					ICoreAPI api = this._api;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Duplicate animation code '");
					defaultInterpolatedStringHandler.AppendFormatted(loadedAnimation.Code);
					defaultInterpolatedStringHandler.AppendLiteral("' from '");
					defaultInterpolatedStringHandler.AppendFormatted(loadedAnimation.Source.DisplayPath);
					defaultInterpolatedStringHandler.AppendLiteral("' ignored.");
					LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					this.AnimationSources[loadedAnimation.Code] = loadedAnimation.Source;
				}
			}
			this.Animations = animationsByCode;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00063034 File Offset: 0x00061234
		[return: Nullable(2)]
		public Animation GetAnimation(string code, params string[] tags)
		{
			return this.GetAnimationRecursive(code, tags);
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0006303E File Offset: 0x0006123E
		[return: Nullable(2)]
		public Animation GetAnimation(string code, EntityPlayer player, bool firstPerson = true)
		{
			return this.GetAnimationRecursive(code, Enumerable.ToArray<string>(this.GetTags(player, firstPerson)));
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00063054 File Offset: 0x00061254
		public bool GetAnimation([Nullable(2)] [NotNullWhen(true)] out Animation animation, string code, EntityPlayer player, bool firstPerson = true)
		{
			animation = this.GetAnimationRecursive(code, Enumerable.ToArray<string>(this.GetTags(player, firstPerson)));
			return animation != null;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00063074 File Offset: 0x00061274
		public IEnumerable<string> GetTags(EntityPlayer player, bool firstPerson = true)
		{
			List<string> tags = new List<string>();
			if (!firstPerson)
			{
				tags.Add("tp");
			}
			if (player.WatchedAttributes.GetFloat("intoxication", 0f) > 0.1f)
			{
				tags.Add("drunk");
			}
			string modelPrefix = player.WatchedAttributes.GetString("skinModel", "").Replace(':', '-');
			if (modelPrefix != "")
			{
				tags.Add(modelPrefix);
			}
			return tags;
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x000630F0 File Offset: 0x000612F0
		[return: Nullable(2)]
		private Animation GetAnimationRecursive(string code, IReadOnlyList<string> tags)
		{
			bool[] excluded = (tags.Count == 0) ? Array.Empty<bool>() : new bool[tags.Count];
			return this.GetAnimationRecursive(code, tags, excluded, tags.Count);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00063128 File Offset: 0x00061328
		[return: Nullable(2)]
		private Animation GetAnimationRecursive(string code, IReadOnlyList<string> tags, bool[] excluded, int remainingTags)
		{
			if (remainingTags > 0)
			{
				for (int index = 0; index < tags.Count; index++)
				{
					if (!excluded[index])
					{
						excluded[index] = true;
						Animation result = this.GetAnimationRecursive(code + "-" + tags[index], tags, excluded, remainingTags - 1);
						excluded[index] = false;
						if (result != null)
						{
							return result;
						}
					}
				}
				for (int index2 = 0; index2 < tags.Count; index2++)
				{
					if (!excluded[index2])
					{
						excluded[index2] = true;
						Animation result2 = this.GetAnimationRecursive(code, tags, excluded, remainingTags - 1);
						excluded[index2] = false;
						if (result2 != null)
						{
							return result2;
						}
					}
				}
			}
			Animation finalResult;
			if (this.Animations.TryGetValue(code, ref finalResult))
			{
				return finalResult;
			}
			return null;
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x000631C4 File Offset: 0x000613C4
		private IEnumerable<AnimationsManager.LoadedAnimation> FromAsset(IAsset asset)
		{
			List<AnimationsManager.LoadedAnimation> result = new List<AnimationsManager.LoadedAnimation>();
			string domain = asset.Location.Domain;
			JsonObject json;
			try
			{
				json = JsonObject.FromJson(asset.ToText());
			}
			catch (Exception exception)
			{
				ICoreAPI api = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(48, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error on parsing animations file '");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(asset.Location);
				defaultInterpolatedStringHandler.AppendLiteral("'.\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				return result;
			}
			foreach (KeyValuePair<string, JToken> entry in (json.Token as JObject))
			{
				string code = entry.Key;
				try
				{
					Animation animation = new JsonObject(entry.Value).AsObject<AnimationJson>(null).ToAnimation();
					string animationCode = code.Contains(':') ? code : (domain + ":" + code);
					result.Add(new AnimationsManager.LoadedAnimation(animationCode, animation, new AnimationSource(domain, code, AnimationSourceKind.ConfigAnimation, null, null)));
				}
				catch (Exception exception2)
				{
					ICoreAPI api2 = this._api;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(50, 3);
					defaultInterpolatedStringHandler2.AppendLiteral("Error on parsing animation '");
					defaultInterpolatedStringHandler2.AppendFormatted(code);
					defaultInterpolatedStringHandler2.AppendLiteral("' from '");
					defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>(asset.Location);
					defaultInterpolatedStringHandler2.AppendLiteral("'.\nException: ");
					defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception2);
					LoggerUtil.Error(api2, this, defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			return result;
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0006336C File Offset: 0x0006156C
		private IEnumerable<AnimationsManager.LoadedAnimation> FromShapeAsset(IAsset asset)
		{
			List<AnimationsManager.LoadedAnimation> result = new List<AnimationsManager.LoadedAnimation>();
			Shape shape;
			try
			{
				shape = asset.ToObject<Shape>(null);
				shape.ResolveReferences(this._api.Logger, asset.Location.ToString());
			}
			catch (Exception exception)
			{
				ICoreAPI api = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(54, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error on parsing shape animations file '");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(asset.Location);
				defaultInterpolatedStringHandler.AppendLiteral("'.\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				return result;
			}
			if (shape.Animations == null || shape.Animations.Length == 0)
			{
				return result;
			}
			foreach (Animation vanillaAnimation in shape.Animations)
			{
				if (!string.IsNullOrWhiteSpace(vanillaAnimation.Code) && vanillaAnimation.KeyFrames != null && vanillaAnimation.KeyFrames.Length != 0)
				{
					try
					{
						string animationCode = AnimationsManager.BuildShapeAnimationCode(asset.Location, vanillaAnimation.Code);
						Animation animation = AnimationsManager.FromVanillaShapeAnimation(vanillaAnimation);
						AnimationSource source = new AnimationSource(asset.Location.Domain, vanillaAnimation.Code, AnimationSourceKind.ShapeAnimation, asset.Location.Path, vanillaAnimation.Code);
						result.Add(new AnimationsManager.LoadedAnimation(animationCode, animation, source));
					}
					catch (Exception exception2)
					{
						ICoreAPI api2 = this._api;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
						defaultInterpolatedStringHandler2..ctor(58, 3);
						defaultInterpolatedStringHandler2.AppendLiteral("Error on importing shape animation '");
						defaultInterpolatedStringHandler2.AppendFormatted(vanillaAnimation.Code);
						defaultInterpolatedStringHandler2.AppendLiteral("' from '");
						defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>(asset.Location);
						defaultInterpolatedStringHandler2.AppendLiteral("'.\nException: ");
						defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception2);
						LoggerUtil.Warn(api2, this, defaultInterpolatedStringHandler2.ToStringAndClear());
					}
				}
			}
			return result;
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00063544 File Offset: 0x00061744
		private static Animation FromVanillaShapeAnimation(Animation vanillaAnimation)
		{
			bool hasPlayerFrames;
			List<PLayerKeyFrame> playerFrames = PLayerKeyFrame.FromVanillaAnimation(vanillaAnimation, out hasPlayerFrames);
			if (hasPlayerFrames)
			{
				return new Animation(playerFrames);
			}
			List<ItemKeyFrame> itemFrames = ItemKeyFrame.FromVanillaAnimation(vanillaAnimation);
			return new Animation(playerFrames, itemFrames);
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00063574 File Offset: 0x00061774
		private static string BuildShapeAnimationCode(AssetLocation location, string vanillaAnimationCode)
		{
			string shapePath = AnimationsManager.GetShapePathWithoutPrefixAndExtension(location.Path);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(8, 3);
			defaultInterpolatedStringHandler.AppendFormatted(location.Domain);
			defaultInterpolatedStringHandler.AppendLiteral(":shape/");
			defaultInterpolatedStringHandler.AppendFormatted(shapePath);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted(vanillaAnimationCode);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x000635D4 File Offset: 0x000617D4
		private static string GetShapePathWithoutPrefixAndExtension(string assetPath)
		{
			string path = assetPath.Replace('\\', '/');
			if (path.StartsWith("shapes/", 5))
			{
				string text = path;
				int length = "shapes/".Length;
				path = text.Substring(length, text.Length - length);
			}
			if (path.EndsWith(".json", 5))
			{
				string text = path;
				int length = ".json".Length;
				path = text.Substring(0, text.Length - length);
			}
			return path;
		}

		// Token: 0x040009E3 RID: 2531
		private readonly ICoreClientAPI _api;

		// Token: 0x020002CA RID: 714
		[Nullable(0)]
		private sealed class LoadedAnimation : IEquatable<AnimationsManager.LoadedAnimation>
		{
			// Token: 0x06001882 RID: 6274 RVA: 0x0006D768 File Offset: 0x0006B968
			public LoadedAnimation(string Code, Animation Animation, AnimationSource Source)
			{
				this.Code = Code;
				this.Animation = Animation;
				this.Source = Source;
				base..ctor();
			}

			// Token: 0x1700054C RID: 1356
			// (get) Token: 0x06001883 RID: 6275 RVA: 0x0006D785 File Offset: 0x0006B985
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(AnimationsManager.LoadedAnimation);
				}
			}

			// Token: 0x1700054D RID: 1357
			// (get) Token: 0x06001884 RID: 6276 RVA: 0x0006D791 File Offset: 0x0006B991
			// (set) Token: 0x06001885 RID: 6277 RVA: 0x0006D799 File Offset: 0x0006B999
			public string Code { get; set; }

			// Token: 0x1700054E RID: 1358
			// (get) Token: 0x06001886 RID: 6278 RVA: 0x0006D7A2 File Offset: 0x0006B9A2
			// (set) Token: 0x06001887 RID: 6279 RVA: 0x0006D7AA File Offset: 0x0006B9AA
			public Animation Animation { get; set; }

			// Token: 0x1700054F RID: 1359
			// (get) Token: 0x06001888 RID: 6280 RVA: 0x0006D7B3 File Offset: 0x0006B9B3
			// (set) Token: 0x06001889 RID: 6281 RVA: 0x0006D7BB File Offset: 0x0006B9BB
			public AnimationSource Source { get; set; }

			// Token: 0x0600188A RID: 6282 RVA: 0x0006D7C4 File Offset: 0x0006B9C4
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("LoadedAnimation");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600188B RID: 6283 RVA: 0x0006D810 File Offset: 0x0006BA10
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Code = ");
				builder.Append(this.Code);
				builder.Append(", Animation = ");
				builder.Append(this.Animation);
				builder.Append(", Source = ");
				builder.Append(this.Source);
				return true;
			}

			// Token: 0x0600188C RID: 6284 RVA: 0x0006D86E File Offset: 0x0006BA6E
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(AnimationsManager.LoadedAnimation left, AnimationsManager.LoadedAnimation right)
			{
				return !(left == right);
			}

			// Token: 0x0600188D RID: 6285 RVA: 0x0006D87A File Offset: 0x0006BA7A
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(AnimationsManager.LoadedAnimation left, AnimationsManager.LoadedAnimation right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600188E RID: 6286 RVA: 0x0006D890 File Offset: 0x0006BA90
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Code>k__BackingField)) * -1521134295 + EqualityComparer<Animation>.Default.GetHashCode(this.<Animation>k__BackingField)) * -1521134295 + EqualityComparer<AnimationSource>.Default.GetHashCode(this.<Source>k__BackingField);
			}

			// Token: 0x0600188F RID: 6287 RVA: 0x0006D8F2 File Offset: 0x0006BAF2
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as AnimationsManager.LoadedAnimation);
			}

			// Token: 0x06001890 RID: 6288 RVA: 0x0006D900 File Offset: 0x0006BB00
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(AnimationsManager.LoadedAnimation other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Code>k__BackingField, other.<Code>k__BackingField) && EqualityComparer<Animation>.Default.Equals(this.<Animation>k__BackingField, other.<Animation>k__BackingField) && EqualityComparer<AnimationSource>.Default.Equals(this.<Source>k__BackingField, other.<Source>k__BackingField));
			}

			// Token: 0x06001892 RID: 6290 RVA: 0x0006D979 File Offset: 0x0006BB79
			[CompilerGenerated]
			private LoadedAnimation(AnimationsManager.LoadedAnimation original)
			{
				this.Code = original.<Code>k__BackingField;
				this.Animation = original.<Animation>k__BackingField;
				this.Source = original.<Source>k__BackingField;
			}

			// Token: 0x06001893 RID: 6291 RVA: 0x0006D9A5 File Offset: 0x0006BBA5
			[CompilerGenerated]
			public void Deconstruct(out string Code, out Animation Animation, out AnimationSource Source)
			{
				Code = this.Code;
				Animation = this.Animation;
				Source = this.Source;
			}
		}
	}
}
