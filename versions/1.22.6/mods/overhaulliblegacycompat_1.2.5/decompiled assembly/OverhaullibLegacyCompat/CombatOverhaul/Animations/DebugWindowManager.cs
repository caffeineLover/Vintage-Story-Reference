using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using CombatOverhaul.MeleeSystems;
using CombatOverhaul.Utils;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A5 RID: 421
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DebugWindowManager : IDisposable
	{
		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x00065342 File Offset: 0x00063542
		// (set) Token: 0x06001448 RID: 5192 RVA: 0x00065349 File Offset: 0x00063549
		public static bool PlayAnimationsInThirdPerson { get; set; } = false;

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x00065351 File Offset: 0x00063551
		// (set) Token: 0x0600144A RID: 5194 RVA: 0x00065358 File Offset: 0x00063558
		public static bool RenderDebugColliders { get; set; } = false;

		// Token: 0x0600144B RID: 5195 RVA: 0x00065360 File Offset: 0x00063560
		public DebugWindowManager(ICoreClientAPI api, ParticleEffectsManager particleEffectsManager, AnimationsManager animationsManager)
		{
			this._api = api;
			this._particleEffectsManager = particleEffectsManager;
			this._animationsManager = animationsManager;
			DebugWindowManager._instance = this;
			DebugWindowManager._colliders.Clear();
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00065433 File Offset: 0x00063633
		public void Load(ICoreClientAPI api)
		{
			this._behavior = api.World.Player.Entity.GetBehavior<FirstPersonAnimationsBehavior>();
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00065450 File Offset: 0x00063650
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			this._behavior = null;
			this._transforms.Clear();
			DebugWindowManager._colliders.Clear();
			if (DebugWindowManager._instance == this)
			{
				DebugWindowManager._instance = null;
			}
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0006548C File Offset: 0x0006368C
		public static void RegisterTransformByCode(ModelTransform transform, string code)
		{
			DebugWindowManager._instance.RegisterTransform(transform, code);
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0006549A File Offset: 0x0006369A
		public void RegisterTransform(ModelTransform transform, string code)
		{
			this._transforms[code] = new DebugWindowManager.EditableTransform(transform, null, null);
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x000654B0 File Offset: 0x000636B0
		private void RegisterTransform(ModelTransform transform, string code, Action<ModelTransform> apply, [Nullable(new byte[]
		{
			2,
			1,
			1
		})] Func<ModelTransform, DebugWindowManager.SourceSaveResult> saveToSource = null)
		{
			this._transforms[code] = new DebugWindowManager.EditableTransform(transform, apply, saveToSource);
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x000654C8 File Offset: 0x000636C8
		private void RegisterCollectibleTransformAttributes(ICoreClientAPI api)
		{
			foreach (Item item in api.World.Items)
			{
				if (!(((item != null) ? item.Code : null) == null))
				{
					this.RegisterCollectibleTransformAttributes(item);
				}
			}
			foreach (Block block in api.World.Blocks)
			{
				if (!(((block != null) ? block.Code : null) == null))
				{
					this.RegisterCollectibleTransformAttributes(block);
				}
			}
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00065584 File Offset: 0x00063784
		private void RegisterCollectibleTransformAttributes(CollectibleObject collectible)
		{
			foreach (string attributeCode in DebugWindowManager.DirectTransformAttributeCodes)
			{
				this.RegisterDirectTransformAttribute(collectible, attributeCode);
			}
			foreach (string attributeCode2 in DebugWindowManager.TypedTransformAttributeCodes)
			{
				this.RegisterTypedTransformAttribute(collectible, attributeCode2);
			}
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x000655D4 File Offset: 0x000637D4
		private void RegisterDirectTransformAttribute(CollectibleObject collectible, string attributeCode)
		{
			JsonObject attributes = collectible.Attributes;
			JsonObject jsonTransform = (attributes != null) ? attributes[attributeCode] : null;
			if (jsonTransform == null || !jsonTransform.Exists)
			{
				return;
			}
			ModelTransform transform = jsonTransform.AsObject<ModelTransform>(null);
			if (transform == null)
			{
				return;
			}
			transform.EnsureDefaultValues();
			ModelTransform transform2 = transform;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 2);
			defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(collectible.Code);
			defaultInterpolatedStringHandler.AppendLiteral(" / ");
			defaultInterpolatedStringHandler.AppendFormatted(attributeCode);
			this.RegisterTransform(transform2, defaultInterpolatedStringHandler.ToStringAndClear(), delegate(ModelTransform value)
			{
				DebugWindowManager.ApplyDirectTransformAttribute(collectible, attributeCode, value);
			}, (ModelTransform value) => DebugWindowManager.TrySaveTransformToSource(collectible, attributeCode, value, null));
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x00065694 File Offset: 0x00063894
		private void RegisterTypedTransformAttribute(CollectibleObject collectible, string attributeCode)
		{
			JsonObject attributes = collectible.Attributes;
			JObject transformsByType = ((attributes != null) ? attributes[attributeCode].Token : null) as JObject;
			if (transformsByType == null)
			{
				return;
			}
			foreach (JProperty property in transformsByType.Properties())
			{
				ModelTransform transform = new JsonObject(property.Value).AsObject<ModelTransform>(null);
				if (transform != null)
				{
					string typeCode = property.Name;
					transform.EnsureDefaultValues();
					ModelTransform transform2 = transform;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(collectible.Code);
					defaultInterpolatedStringHandler.AppendLiteral(" / ");
					defaultInterpolatedStringHandler.AppendFormatted(attributeCode);
					defaultInterpolatedStringHandler.AppendLiteral(" / ");
					defaultInterpolatedStringHandler.AppendFormatted(typeCode);
					this.RegisterTransform(transform2, defaultInterpolatedStringHandler.ToStringAndClear(), delegate(ModelTransform value)
					{
						DebugWindowManager.ApplyTypedTransformAttribute(collectible, attributeCode, typeCode, value);
					}, (ModelTransform value) => DebugWindowManager.TrySaveTransformToSource(collectible, attributeCode, value, typeCode));
				}
			}
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x000657E4 File Offset: 0x000639E4
		private static void ApplyDirectTransformAttribute(CollectibleObject collectible, string attributeCode, ModelTransform transform)
		{
			JsonObject attributes2 = collectible.Attributes;
			JObject source = ((attributes2 != null) ? attributes2.Token : null) as JObject;
			if (source == null)
			{
				return;
			}
			JObject attributes = (JObject)source.DeepClone();
			attributes[attributeCode] = DebugWindowManager.TransformToToken(transform);
			collectible.Attributes = new JsonObject(attributes);
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x00065834 File Offset: 0x00063A34
		private static void ApplyTypedTransformAttribute(CollectibleObject collectible, string attributeCode, string typeCode, ModelTransform transform)
		{
			JsonObject attributes2 = collectible.Attributes;
			JObject source = ((attributes2 != null) ? attributes2.Token : null) as JObject;
			if (source == null)
			{
				return;
			}
			JObject attributes = (JObject)source.DeepClone();
			JObject transformsByType = (attributes[attributeCode] as JObject) ?? new JObject();
			transformsByType[typeCode] = DebugWindowManager.TransformToToken(transform);
			attributes[attributeCode] = transformsByType;
			collectible.Attributes = new JsonObject(attributes);
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x000658A0 File Offset: 0x00063AA0
		private static JToken TransformToToken(ModelTransform transform)
		{
			return JToken.Parse(JsonUtil.ToPrettyString<ModelTransform>(transform));
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x000658B0 File Offset: 0x00063AB0
		private static DebugWindowManager.SourceSaveResult TrySaveTransformToSource(CollectibleObject collectible, string attributeCode, ModelTransform transform, [Nullable(2)] string typedKey = null)
		{
			string sourceFile = DebugWindowManager.FindCollectibleSourceFile(collectible);
			if (sourceFile == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(22, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Source not found for ");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(collectible.Code);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				return DebugWindowManager.SourceSaveResult.Fail(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			DebugWindowManager.SourceSaveResult result;
			try
			{
				string oldText = File.ReadAllText(sourceFile);
				if (DebugWindowManager.SourceHasComments(oldText))
				{
					result = DebugWindowManager.SourceSaveResult.Fail("Source has comments; cannot safely rewrite. Strip comments first or edit by hand.");
				}
				else
				{
					JObject json = JObject.Parse(oldText);
					JObject attributes = (json["attributes"] as JObject) ?? new JObject();
					if (typedKey == null)
					{
						attributes[attributeCode] = DebugWindowManager.TransformToToken(transform);
					}
					else
					{
						JObject transformsByType = (attributes[attributeCode] as JObject) ?? new JObject();
						transformsByType[typedKey] = DebugWindowManager.TransformToToken(transform);
						attributes[attributeCode] = transformsByType;
					}
					json["attributes"] = attributes;
					string newText = JsonUtil.ToPrettyString<JObject>(json);
					string sourceFile2 = sourceFile;
					string oldText2 = oldText;
					string newText2 = newText;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(11, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("Saved ");
					defaultInterpolatedStringHandler2.AppendFormatted(attributeCode);
					defaultInterpolatedStringHandler2.AppendLiteral(" to ");
					defaultInterpolatedStringHandler2.AppendFormatted(sourceFile);
					defaultInterpolatedStringHandler2.AppendLiteral(".");
					result = DebugWindowManager.SourceSaveResult.Preview(new DebugWindowManager.SourceSaveRequest(sourceFile2, oldText2, newText2, defaultInterpolatedStringHandler2.ToStringAndClear(), () => DebugWindowManager.AtomicWriteWithBackup(sourceFile, newText)));
				}
			}
			catch (Exception exception)
			{
				result = DebugWindowManager.SourceSaveResult.Fail("Save failed for " + sourceFile + ": " + exception.Message);
			}
			return result;
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x00065A70 File Offset: 0x00063C70
		[return: Nullable(2)]
		private static string FindCollectibleSourceFile(CollectibleObject collectible)
		{
			string sourceRoot = DebugWindowManager.GetSourceRoot();
			if (!Directory.Exists(sourceRoot))
			{
				return null;
			}
			string domain = collectible.Code.Domain;
			string kind = (collectible is Block) ? "blocktypes" : "itemtypes";
			string path = collectible.Code.Path;
			IEnumerable<string> enumerable = Enumerable.Where<string>(Directory.EnumerateFiles(sourceRoot, "*.json", 1), delegate(string file)
			{
				string normalized = file.Replace('\\', '/');
				string text2 = normalized;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(20, 2);
				defaultInterpolatedStringHandler.AppendLiteral("/resources/assets/");
				defaultInterpolatedStringHandler.AppendFormatted(domain);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted(kind);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				if (!text2.Contains(defaultInterpolatedStringHandler.ToStringAndClear(), 5))
				{
					string text3 = normalized;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(10, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("/assets/");
					defaultInterpolatedStringHandler2.AppendFormatted(domain);
					defaultInterpolatedStringHandler2.AppendLiteral("/");
					defaultInterpolatedStringHandler2.AppendFormatted(kind);
					defaultInterpolatedStringHandler2.AppendLiteral("/");
					return text3.Contains(defaultInterpolatedStringHandler2.ToStringAndClear(), 5);
				}
				return true;
			});
			string bestFile = null;
			int bestScore = -1;
			foreach (string file2 in enumerable)
			{
				try
				{
					JToken jtoken = JObject.Parse(File.ReadAllText(file2))["code"];
					string code = (jtoken != null) ? jtoken.ToString() : null;
					if (!string.IsNullOrEmpty(code))
					{
						if (code.Contains(':'))
						{
							string text = code;
							int num = code.IndexOf(':') + 1;
							code = text.Substring(num, text.Length - num);
						}
						int score = -1;
						if (string.Equals(code, path, 5))
						{
							score = 10000 + code.Length;
						}
						else if (path.StartsWith(code + "-", 5))
						{
							score = 1000 + code.Length;
						}
						else if (Path.GetFileNameWithoutExtension(file2).Equals(code, 5) && path.Contains(code, 5))
						{
							score = 100 + code.Length;
						}
						if (score > bestScore)
						{
							bestScore = score;
							bestFile = file2;
						}
					}
				}
				catch
				{
				}
			}
			return bestFile;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00065C1C File Offset: 0x00063E1C
		private static DebugWindowManager.SourceSaveResult TrySaveAnimationToSource(string animationCode, Animation animation)
		{
			AnimationSource source;
			if (!DebugWindowManager._instance._animationsManager.AnimationSources.TryGetValue(animationCode, ref source))
			{
				return DebugWindowManager.SourceSaveResult.Fail("Source not tracked for " + animationCode + ".");
			}
			if (source.Kind == AnimationSourceKind.ShapeAnimation)
			{
				return DebugWindowManager.SourceSaveResult.Fail("This animation was imported from a shape file. Export it as CO JSON before saving it back to source.");
			}
			string sourceRoot = DebugWindowManager.GetSourceRoot();
			if (sourceRoot == null || !Directory.Exists(sourceRoot))
			{
				return DebugWindowManager.SourceSaveResult.Fail("VS mods source root not found.");
			}
			using (IEnumerator<string> enumerator = Enumerable.Where<string>(Directory.EnumerateFiles(sourceRoot, "*.json", 1), delegate(string file)
			{
				string normalized = file.Replace('\\', '/');
				return normalized.Contains("/resources/assets/" + source.Domain + "/config/animations/", 5) || normalized.Contains("/assets/" + source.Domain + "/config/animations/", 5);
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string file = enumerator.Current;
					try
					{
						string oldText = File.ReadAllText(file);
						if (DebugWindowManager.SourceHasComments(oldText))
						{
							if (oldText.Contains(source.SourceKey, 5))
							{
								return DebugWindowManager.SourceSaveResult.Fail("Source has comments; cannot safely rewrite. Strip comments first or edit by hand.");
							}
						}
						else
						{
							JObject json = JObject.Parse(oldText);
							if (json.ContainsKey(source.SourceKey))
							{
								json[source.SourceKey] = JToken.Parse(AnimationJson.FromAnimation(animation).ToString());
								string newText = JsonUtil.ToPrettyString<JObject>(json);
								string file2 = file;
								string oldText2 = oldText;
								string newText2 = newText;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Saved ");
								defaultInterpolatedStringHandler.AppendFormatted(animationCode);
								defaultInterpolatedStringHandler.AppendLiteral(" to ");
								defaultInterpolatedStringHandler.AppendFormatted(file);
								defaultInterpolatedStringHandler.AppendLiteral(".");
								return DebugWindowManager.SourceSaveResult.Preview(new DebugWindowManager.SourceSaveRequest(file2, oldText2, newText2, defaultInterpolatedStringHandler.ToStringAndClear(), () => DebugWindowManager.AtomicWriteWithBackup(file, newText)));
							}
						}
					}
					catch
					{
					}
				}
			}
			return DebugWindowManager.SourceSaveResult.Fail("Source JSON not found for " + animationCode + ".");
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00065E3C File Offset: 0x0006403C
		private static bool SourceHasComments(string text)
		{
			if (text.Contains("/*", 4))
			{
				return true;
			}
			bool result;
			using (StringReader reader = new StringReader(text))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					string trimmed = line.TrimStart();
					if (trimmed.StartsWith("//", 4) || trimmed.StartsWith("#", 4))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00065EB4 File Offset: 0x000640B4
		private static string AtomicWriteWithBackup(string sourceFile, string newText)
		{
			string tmpPath = sourceFile + ".tmp";
			string result;
			try
			{
				DebugWindowManager.CreateSourceBackup(sourceFile);
				using (FileStream stream = new FileStream(tmpPath, 2, 2, 0, 4096, int.MinValue))
				{
					using (StreamWriter writer = new StreamWriter(stream))
					{
						writer.Write(newText);
						writer.Flush();
						stream.Flush(true);
					}
				}
				File.Move(tmpPath, sourceFile, true);
				result = "";
			}
			finally
			{
				if (File.Exists(tmpPath))
				{
					File.Delete(tmpPath);
				}
			}
			return result;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x00065F60 File Offset: 0x00064160
		private static void CreateSourceBackup(string sourceFile)
		{
			string backup = sourceFile + ".bak";
			DateTime now = DateTime.Now;
			if (File.Exists(backup))
			{
				DateTime last = File.GetLastWriteTime(backup);
				if (last.Year == now.Year && last.Month == now.Month && last.Day == now.Day && last.Hour == now.Hour && last.Minute == now.Minute)
				{
					return;
				}
			}
			File.Copy(sourceFile, backup, true);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x00065FE8 File Offset: 0x000641E8
		[NullableContext(2)]
		private static string GetSourceRoot()
		{
			string envRoot = Environment.GetEnvironmentVariable("VS_MODS_ROOT");
			string documentsRoot = Path.Combine(Environment.GetFolderPath(5), "VSMods");
			string legacyRoot = Path.Combine(Environment.GetFolderPath(16), "ModsNeedUpdate");
			foreach (string sourceRoot in new string[]
			{
				envRoot,
				documentsRoot,
				legacyRoot
			})
			{
				if (!string.IsNullOrWhiteSpace(sourceRoot) && Directory.Exists(sourceRoot))
				{
					return sourceRoot;
				}
			}
			return null;
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x00066064 File Offset: 0x00064264
		private static string GetAnimationSourceText(string animationCode)
		{
			AnimationSource source;
			if (!DebugWindowManager._instance._animationsManager.AnimationSources.TryGetValue(animationCode, ref source))
			{
				return "Source: not tracked";
			}
			string kind = (source.Kind == AnimationSourceKind.ShapeAnimation) ? "shape" : "CO config";
			return "Source: " + kind + " / " + source.DisplayPath;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x000660BC File Offset: 0x000642BC
		private static ModelTransform CreateDefaultTransform()
		{
			ModelTransform modelTransform = new ModelTransform();
			modelTransform.Translation = new Vec3f();
			modelTransform.Rotation = new Vec3f();
			modelTransform.Origin = new Vec3f(0.5f, 0.5f, 0.5f);
			modelTransform.ScaleXYZ = new Vec3f(1f, 1f, 1f);
			modelTransform.EnsureDefaultValues();
			return modelTransform;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x00066134 File Offset: 0x00064334
		public static void RegisterCollider(string item, string type, MeleeDamageType collider)
		{
			if (!DebugWindowManager._colliders.ContainsKey(item))
			{
				DebugWindowManager._colliders.Add(item, new Dictionary<string, ValueTuple<Action<LineSegmentCollider>, Func<LineSegmentCollider>>>());
			}
			DebugWindowManager._colliders[item].Add(type, new ValueTuple<Action<LineSegmentCollider>, Func<LineSegmentCollider>>(delegate(LineSegmentCollider value)
			{
				collider.RelativeCollider = value;
			}, () => collider.RelativeCollider));
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x00066199 File Offset: 0x00064399
		public static void RegisterCollider(string item, string type, Action<LineSegmentCollider> setter, Func<LineSegmentCollider> getter)
		{
			if (!DebugWindowManager._colliders.ContainsKey(item))
			{
				DebugWindowManager._colliders.Add(item, new Dictionary<string, ValueTuple<Action<LineSegmentCollider>, Func<LineSegmentCollider>>>());
			}
			DebugWindowManager._colliders[item].Add(type, new ValueTuple<Action<LineSegmentCollider>, Func<LineSegmentCollider>>(setter, getter));
		}

		// Token: 0x04000A15 RID: 2581
		private bool _showAnimationEditor;

		// Token: 0x04000A16 RID: 2582
		private int _selectedAnimationIndex;

		// Token: 0x04000A17 RID: 2583
		private int _selectedAnimationIndexFiltered;

		// Token: 0x04000A18 RID: 2584
		[Nullable(2)]
		private FirstPersonAnimationsBehavior _behavior;

		// Token: 0x04000A19 RID: 2585
		private readonly ICoreClientAPI _api;

		// Token: 0x04000A1A RID: 2586
		private string _itemAnimation = "";

		// Token: 0x04000A1B RID: 2587
		private string _animationKey = "";

		// Token: 0x04000A1C RID: 2588
		private readonly FieldInfo _mainCameraInfo = typeof(ClientMain).GetField("MainCamera", 36);

		// Token: 0x04000A1D RID: 2589
		private readonly FieldInfo _cameraFov = typeof(Camera).GetField("Fov", 36);

		// Token: 0x04000A1E RID: 2590
		private string _playerAnimationKey = "";

		// Token: 0x04000A1F RID: 2591
		private float _animationSpeed = 1f;

		// Token: 0x04000A20 RID: 2592
		private ParticleEffectsManager _particleEffectsManager;

		// Token: 0x04000A21 RID: 2593
		private readonly AnimationsManager _animationsManager;

		// Token: 0x04000A22 RID: 2594
		private AnimationJson _animationBuffer;

		// Token: 0x04000A23 RID: 2595
		internal static DebugWindowManager _instance;

		// Token: 0x04000A24 RID: 2596
		private bool _disposed;

		// Token: 0x04000A25 RID: 2597
		private string _animationsFilter = "";

		// Token: 0x04000A26 RID: 2598
		private string _filter = "";

		// Token: 0x04000A27 RID: 2599
		private string _collidersItemsFilter = "";

		// Token: 0x04000A28 RID: 2600
		private int _transformIndex;

		// Token: 0x04000A29 RID: 2601
		private int _colliderItemIndex;

		// Token: 0x04000A2A RID: 2602
		private int _colliderIndex;

		// Token: 0x04000A2B RID: 2603
		private int _heldTransformAttributeIndex;

		// Token: 0x04000A2C RID: 2604
		private string _transformSaveStatus = "";

		// Token: 0x04000A2D RID: 2605
		private readonly Dictionary<string, DebugWindowManager.EditableTransform> _transforms = new Dictionary<string, DebugWindowManager.EditableTransform>();

		// Token: 0x04000A2E RID: 2606
		[TupleElementNames(new string[]
		{
			"setter",
			"getter"
		})]
		[Nullable(new byte[]
		{
			1,
			1,
			1,
			1,
			0,
			1,
			1
		})]
		private static Dictionary<string, Dictionary<string, ValueTuple<Action<LineSegmentCollider>, Func<LineSegmentCollider>>>> _colliders = new Dictionary<string, Dictionary<string, ValueTuple<Action<LineSegmentCollider>, Func<LineSegmentCollider>>>>();

		// Token: 0x04000A2F RID: 2607
		internal static LineSegmentCollider? _currentCollider = default(LineSegmentCollider?);

		// Token: 0x04000A30 RID: 2608
		private static readonly string[] DirectTransformAttributeCodes = new string[]
		{
			"groundStorageTransform",
			"guiTransform",
			"groundTransform",
			"tpHandTransform",
			"tpOffHandTransform",
			"onDisplayTransform",
			"onshelfTransform",
			"toolrackTransform",
			"onTongTransform",
			"onMetalTongTransform",
			"inForgeTransform",
			"infirepitTransform",
			"inTrapTransform",
			"onAntlerMountTransform",
			"onmoldrackTransform",
			"onOmokTransform",
			"onscrollrackTransform",
			"inGenericDisplayTransform",
			"inWeaponrackTransform",
			"inWallmountTransform",
			"inPistolstandTransform",
			"inViceTransform",
			"inCrossbowWallmountTransform"
		};

		// Token: 0x04000A31 RID: 2609
		private static readonly string[] TypedTransformAttributeCodes = new string[]
		{
			"onTongTransformByType",
			"onMetalTongTransformByType",
			"inForgeTransformByType"
		};

		// Token: 0x020002D0 RID: 720
		[Nullable(0)]
		private sealed class EditableTransform
		{
			// Token: 0x0600189E RID: 6302 RVA: 0x0006DA38 File Offset: 0x0006BC38
			public EditableTransform(ModelTransform transform, [Nullable(new byte[]
			{
				2,
				1
			})] Action<ModelTransform> apply = null, [Nullable(new byte[]
			{
				2,
				1,
				1
			})] Func<ModelTransform, DebugWindowManager.SourceSaveResult> saveToSource = null)
			{
				this.Transform = transform;
				this.Apply = apply;
				this.SaveToSource = saveToSource;
			}

			// Token: 0x17000550 RID: 1360
			// (get) Token: 0x0600189F RID: 6303 RVA: 0x0006DA55 File Offset: 0x0006BC55
			public ModelTransform Transform { get; }

			// Token: 0x17000551 RID: 1361
			// (get) Token: 0x060018A0 RID: 6304 RVA: 0x0006DA5D File Offset: 0x0006BC5D
			[Nullable(new byte[]
			{
				2,
				1
			})]
			public Action<ModelTransform> Apply { [return: Nullable(new byte[]
			{
				2,
				1
			})] get; }

			// Token: 0x17000552 RID: 1362
			// (get) Token: 0x060018A1 RID: 6305 RVA: 0x0006DA65 File Offset: 0x0006BC65
			[Nullable(new byte[]
			{
				2,
				1,
				1
			})]
			public Func<ModelTransform, DebugWindowManager.SourceSaveResult> SaveToSource { [return: Nullable(new byte[]
			{
				2,
				1,
				1
			})] get; }
		}

		// Token: 0x020002D1 RID: 721
		[Nullable(0)]
		private sealed class SourceSaveRequest
		{
			// Token: 0x060018A2 RID: 6306 RVA: 0x0006DA6D File Offset: 0x0006BC6D
			public SourceSaveRequest(string sourceFile, string oldText, string newText, string successStatus, Func<string> commit)
			{
				this.SourceFile = sourceFile;
				this.OldText = oldText;
				this.NewText = newText;
				this.SuccessStatus = successStatus;
				this._commit = commit;
			}

			// Token: 0x17000553 RID: 1363
			// (get) Token: 0x060018A3 RID: 6307 RVA: 0x0006DA9A File Offset: 0x0006BC9A
			public string SourceFile { get; }

			// Token: 0x17000554 RID: 1364
			// (get) Token: 0x060018A4 RID: 6308 RVA: 0x0006DAA2 File Offset: 0x0006BCA2
			public string OldText { get; }

			// Token: 0x17000555 RID: 1365
			// (get) Token: 0x060018A5 RID: 6309 RVA: 0x0006DAAA File Offset: 0x0006BCAA
			public string NewText { get; }

			// Token: 0x17000556 RID: 1366
			// (get) Token: 0x060018A6 RID: 6310 RVA: 0x0006DAB2 File Offset: 0x0006BCB2
			public string SuccessStatus { get; }

			// Token: 0x060018A7 RID: 6311 RVA: 0x0006DABC File Offset: 0x0006BCBC
			public string Commit()
			{
				string result = this._commit.Invoke();
				if (!string.IsNullOrWhiteSpace(result))
				{
					return result;
				}
				return this.SuccessStatus;
			}

			// Token: 0x04000E47 RID: 3655
			private readonly Func<string> _commit;
		}

		// Token: 0x020002D2 RID: 722
		[Nullable(0)]
		private sealed class SourceSaveResult
		{
			// Token: 0x060018A8 RID: 6312 RVA: 0x0006DAE5 File Offset: 0x0006BCE5
			private SourceSaveResult(string status, [Nullable(2)] DebugWindowManager.SourceSaveRequest request)
			{
				this.Status = status;
				this.Request = request;
			}

			// Token: 0x17000557 RID: 1367
			// (get) Token: 0x060018A9 RID: 6313 RVA: 0x0006DAFB File Offset: 0x0006BCFB
			public string Status { get; }

			// Token: 0x17000558 RID: 1368
			// (get) Token: 0x060018AA RID: 6314 RVA: 0x0006DB03 File Offset: 0x0006BD03
			[Nullable(2)]
			public DebugWindowManager.SourceSaveRequest Request { [NullableContext(2)] get; }

			// Token: 0x060018AB RID: 6315 RVA: 0x0006DB0B File Offset: 0x0006BD0B
			public static DebugWindowManager.SourceSaveResult Fail(string status)
			{
				return new DebugWindowManager.SourceSaveResult(status, null);
			}

			// Token: 0x060018AC RID: 6316 RVA: 0x0006DB14 File Offset: 0x0006BD14
			public static DebugWindowManager.SourceSaveResult Preview(DebugWindowManager.SourceSaveRequest request)
			{
				return new DebugWindowManager.SourceSaveResult("", request);
			}
		}
	}
}
