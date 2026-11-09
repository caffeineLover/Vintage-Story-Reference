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
	// Token: 0x020001A8 RID: 424
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnimationsManager
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x0006495E File Offset: 0x00062B5E
		// (set) Token: 0x06001459 RID: 5209 RVA: 0x00064966 File Offset: 0x00062B66
		public Dictionary<string, Animation> Animations { get; private set; } = new Dictionary<string, Animation>();

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x0006496F File Offset: 0x00062B6F
		internal Dictionary<string, AnimationSource> AnimationSources { get; } = new Dictionary<string, AnimationSource>();

		// Token: 0x0600145B RID: 5211 RVA: 0x00064977 File Offset: 0x00062B77
		public AnimationsManager(ICoreClientAPI api, ParticleEffectsManager particleEffectsManager)
		{
			this._api = api;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0006499C File Offset: 0x00062B9C
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

		// Token: 0x0600145D RID: 5213 RVA: 0x00064AB4 File Offset: 0x00062CB4
		[return: Nullable(2)]
		public Animation GetAnimation(string code, params string[] tags)
		{
			return this.GetAnimationRecursive(code, tags);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x00064ABE File Offset: 0x00062CBE
		[return: Nullable(2)]
		public Animation GetAnimation(string code, EntityPlayer player, bool firstPerson = true)
		{
			return this.GetAnimationRecursive(code, Enumerable.ToArray<string>(this.GetTags(player, firstPerson)));
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x00064AD4 File Offset: 0x00062CD4
		public bool GetAnimation([Nullable(2)] [NotNullWhen(true)] out Animation animation, string code, EntityPlayer player, bool firstPerson = true)
		{
			animation = this.GetAnimationRecursive(code, Enumerable.ToArray<string>(this.GetTags(player, firstPerson)));
			return animation != null;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00064AF4 File Offset: 0x00062CF4
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

		// Token: 0x06001461 RID: 5217 RVA: 0x00064B70 File Offset: 0x00062D70
		[return: Nullable(2)]
		private Animation GetAnimationRecursive(string code, IReadOnlyList<string> tags)
		{
			bool[] excluded = (tags.Count == 0) ? Array.Empty<bool>() : new bool[tags.Count];
			return this.GetAnimationRecursive(code, tags, excluded, tags.Count);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x00064BA8 File Offset: 0x00062DA8
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

		// Token: 0x06001463 RID: 5219 RVA: 0x00064C44 File Offset: 0x00062E44
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

		// Token: 0x06001464 RID: 5220 RVA: 0x00064DEC File Offset: 0x00062FEC
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

		// Token: 0x06001465 RID: 5221 RVA: 0x00064FC4 File Offset: 0x000631C4
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

		// Token: 0x06001466 RID: 5222 RVA: 0x00064FF4 File Offset: 0x000631F4
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

		// Token: 0x06001467 RID: 5223 RVA: 0x00065054 File Offset: 0x00063254
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

		// Token: 0x04000A0B RID: 2571
		private readonly ICoreClientAPI _api;

		// Token: 0x020002DD RID: 733
		[Nullable(0)]
		private sealed class LoadedAnimation : IEquatable<AnimationsManager.LoadedAnimation>
		{
			// Token: 0x06001927 RID: 6439 RVA: 0x0006FD04 File Offset: 0x0006DF04
			public LoadedAnimation(string Code, Animation Animation, AnimationSource Source)
			{
				this.Code = Code;
				this.Animation = Animation;
				this.Source = Source;
				base..ctor();
			}

			// Token: 0x17000576 RID: 1398
			// (get) Token: 0x06001928 RID: 6440 RVA: 0x0006FD21 File Offset: 0x0006DF21
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(AnimationsManager.LoadedAnimation);
				}
			}

			// Token: 0x17000577 RID: 1399
			// (get) Token: 0x06001929 RID: 6441 RVA: 0x0006FD2D File Offset: 0x0006DF2D
			// (set) Token: 0x0600192A RID: 6442 RVA: 0x0006FD35 File Offset: 0x0006DF35
			public string Code { get; set; }

			// Token: 0x17000578 RID: 1400
			// (get) Token: 0x0600192B RID: 6443 RVA: 0x0006FD3E File Offset: 0x0006DF3E
			// (set) Token: 0x0600192C RID: 6444 RVA: 0x0006FD46 File Offset: 0x0006DF46
			public Animation Animation { get; set; }

			// Token: 0x17000579 RID: 1401
			// (get) Token: 0x0600192D RID: 6445 RVA: 0x0006FD4F File Offset: 0x0006DF4F
			// (set) Token: 0x0600192E RID: 6446 RVA: 0x0006FD57 File Offset: 0x0006DF57
			public AnimationSource Source { get; set; }

			// Token: 0x0600192F RID: 6447 RVA: 0x0006FD60 File Offset: 0x0006DF60
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

			// Token: 0x06001930 RID: 6448 RVA: 0x0006FDAC File Offset: 0x0006DFAC
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

			// Token: 0x06001931 RID: 6449 RVA: 0x0006FE0A File Offset: 0x0006E00A
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(AnimationsManager.LoadedAnimation left, AnimationsManager.LoadedAnimation right)
			{
				return !(left == right);
			}

			// Token: 0x06001932 RID: 6450 RVA: 0x0006FE16 File Offset: 0x0006E016
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(AnimationsManager.LoadedAnimation left, AnimationsManager.LoadedAnimation right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06001933 RID: 6451 RVA: 0x0006FE2C File Offset: 0x0006E02C
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Code>k__BackingField)) * -1521134295 + EqualityComparer<Animation>.Default.GetHashCode(this.<Animation>k__BackingField)) * -1521134295 + EqualityComparer<AnimationSource>.Default.GetHashCode(this.<Source>k__BackingField);
			}

			// Token: 0x06001934 RID: 6452 RVA: 0x0006FE8E File Offset: 0x0006E08E
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as AnimationsManager.LoadedAnimation);
			}

			// Token: 0x06001935 RID: 6453 RVA: 0x0006FE9C File Offset: 0x0006E09C
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(AnimationsManager.LoadedAnimation other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Code>k__BackingField, other.<Code>k__BackingField) && EqualityComparer<Animation>.Default.Equals(this.<Animation>k__BackingField, other.<Animation>k__BackingField) && EqualityComparer<AnimationSource>.Default.Equals(this.<Source>k__BackingField, other.<Source>k__BackingField));
			}

			// Token: 0x06001937 RID: 6455 RVA: 0x0006FF15 File Offset: 0x0006E115
			[CompilerGenerated]
			private LoadedAnimation(AnimationsManager.LoadedAnimation original)
			{
				this.Code = original.<Code>k__BackingField;
				this.Animation = original.<Animation>k__BackingField;
				this.Source = original.<Source>k__BackingField;
			}

			// Token: 0x06001938 RID: 6456 RVA: 0x0006FF41 File Offset: 0x0006E141
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
