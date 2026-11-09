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
	// Token: 0x020001B3 RID: 435
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DebugWindowManager : IDisposable
	{
		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x00066DEA File Offset: 0x00064FEA
		// (set) Token: 0x060014C0 RID: 5312 RVA: 0x00066DF1 File Offset: 0x00064FF1
		public static bool PlayAnimationsInThirdPerson { get; set; } = false;

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x00066DF9 File Offset: 0x00064FF9
		// (set) Token: 0x060014C2 RID: 5314 RVA: 0x00066E00 File Offset: 0x00065000
		public static bool RenderDebugColliders { get; set; } = false;

		// Token: 0x060014C3 RID: 5315 RVA: 0x00066E08 File Offset: 0x00065008
		public DebugWindowManager(ICoreClientAPI api, ParticleEffectsManager particleEffectsManager, AnimationsManager animationsManager)
		{
			this._api = api;
			this._particleEffectsManager = particleEffectsManager;
			this._animationsManager = animationsManager;
			DebugWindowManager._instance = this;
			DebugWindowManager._colliders.Clear();
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00066EDB File Offset: 0x000650DB
		public void Load(ICoreClientAPI api)
		{
			this._behavior = api.World.Player.Entity.GetBehavior<FirstPersonAnimationsBehavior>();
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00066EF8 File Offset: 0x000650F8
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

		// Token: 0x060014C6 RID: 5318 RVA: 0x00066F34 File Offset: 0x00065134
		public static void RegisterTransformByCode(ModelTransform transform, string code)
		{
			DebugWindowManager._instance.RegisterTransform(transform, code);
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00066F42 File Offset: 0x00065142
		public void RegisterTransform(ModelTransform transform, string code)
		{
			this._transforms[code] = new DebugWindowManager.EditableTransform(transform, null, null);
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00066F58 File Offset: 0x00065158
		private void RegisterTransform(ModelTransform transform, string code, Action<ModelTransform> apply, [Nullable(new byte[]
		{
			2,
			1,
			1
		})] Func<ModelTransform, DebugWindowManager.SourceSaveResult> saveToSource = null)
		{
			this._transforms[code] = new DebugWindowManager.EditableTransform(transform, apply, saveToSource);
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00066F70 File Offset: 0x00065170
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

		// Token: 0x060014CA RID: 5322 RVA: 0x0006702C File Offset: 0x0006522C
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

		// Token: 0x060014CB RID: 5323 RVA: 0x0006707C File Offset: 0x0006527C
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

		// Token: 0x060014CC RID: 5324 RVA: 0x0006713C File Offset: 0x0006533C
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

		// Token: 0x060014CD RID: 5325 RVA: 0x0006728C File Offset: 0x0006548C
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

		// Token: 0x060014CE RID: 5326 RVA: 0x000672DC File Offset: 0x000654DC
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

		// Token: 0x060014CF RID: 5327 RVA: 0x00067348 File Offset: 0x00065548
		private static JToken TransformToToken(ModelTransform transform)
		{
			return JToken.Parse(JsonUtil.ToPrettyString<ModelTransform>(transform));
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00067358 File Offset: 0x00065558
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

		// Token: 0x060014D1 RID: 5329 RVA: 0x00067518 File Offset: 0x00065718
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

		// Token: 0x060014D2 RID: 5330 RVA: 0x000676C4 File Offset: 0x000658C4
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

		// Token: 0x060014D3 RID: 5331 RVA: 0x000678E4 File Offset: 0x00065AE4
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

		// Token: 0x060014D4 RID: 5332 RVA: 0x0006795C File Offset: 0x00065B5C
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

		// Token: 0x060014D5 RID: 5333 RVA: 0x00067A08 File Offset: 0x00065C08
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

		// Token: 0x060014D6 RID: 5334 RVA: 0x00067A90 File Offset: 0x00065C90
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

		// Token: 0x060014D7 RID: 5335 RVA: 0x00067B0C File Offset: 0x00065D0C
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

		// Token: 0x060014D8 RID: 5336 RVA: 0x00067B64 File Offset: 0x00065D64
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

		// Token: 0x060014D9 RID: 5337 RVA: 0x00067BDC File Offset: 0x00065DDC
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

		// Token: 0x060014DA RID: 5338 RVA: 0x00067C41 File Offset: 0x00065E41
		public static void RegisterCollider(string item, string type, Action<LineSegmentCollider> setter, Func<LineSegmentCollider> getter)
		{
			if (!DebugWindowManager._colliders.ContainsKey(item))
			{
				DebugWindowManager._colliders.Add(item, new Dictionary<string, ValueTuple<Action<LineSegmentCollider>, Func<LineSegmentCollider>>>());
			}
			DebugWindowManager._colliders[item].Add(type, new ValueTuple<Action<LineSegmentCollider>, Func<LineSegmentCollider>>(setter, getter));
		}

		// Token: 0x04000A3D RID: 2621
		private bool _showAnimationEditor;

		// Token: 0x04000A3E RID: 2622
		private int _selectedAnimationIndex;

		// Token: 0x04000A3F RID: 2623
		private int _selectedAnimationIndexFiltered;

		// Token: 0x04000A40 RID: 2624
		[Nullable(2)]
		private FirstPersonAnimationsBehavior _behavior;

		// Token: 0x04000A41 RID: 2625
		private readonly ICoreClientAPI _api;

		// Token: 0x04000A42 RID: 2626
		private string _itemAnimation = "";

		// Token: 0x04000A43 RID: 2627
		private string _animationKey = "";

		// Token: 0x04000A44 RID: 2628
		private readonly FieldInfo _mainCameraInfo = typeof(ClientMain).GetField("MainCamera", 36);

		// Token: 0x04000A45 RID: 2629
		private readonly FieldInfo _cameraFov = typeof(Camera).GetField("Fov", 36);

		// Token: 0x04000A46 RID: 2630
		private string _playerAnimationKey = "";

		// Token: 0x04000A47 RID: 2631
		private float _animationSpeed = 1f;

		// Token: 0x04000A48 RID: 2632
		private ParticleEffectsManager _particleEffectsManager;

		// Token: 0x04000A49 RID: 2633
		private readonly AnimationsManager _animationsManager;

		// Token: 0x04000A4A RID: 2634
		private AnimationJson _animationBuffer;

		// Token: 0x04000A4B RID: 2635
		internal static DebugWindowManager _instance;

		// Token: 0x04000A4C RID: 2636
		private bool _disposed;

		// Token: 0x04000A4D RID: 2637
		private string _animationsFilter = "";

		// Token: 0x04000A4E RID: 2638
		private string _filter = "";

		// Token: 0x04000A4F RID: 2639
		private string _collidersItemsFilter = "";

		// Token: 0x04000A50 RID: 2640
		private int _transformIndex;

		// Token: 0x04000A51 RID: 2641
		private int _colliderItemIndex;

		// Token: 0x04000A52 RID: 2642
		private int _colliderIndex;

		// Token: 0x04000A53 RID: 2643
		private int _heldTransformAttributeIndex;

		// Token: 0x04000A54 RID: 2644
		private string _transformSaveStatus = "";

		// Token: 0x04000A55 RID: 2645
		private readonly Dictionary<string, DebugWindowManager.EditableTransform> _transforms = new Dictionary<string, DebugWindowManager.EditableTransform>();

		// Token: 0x04000A56 RID: 2646
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

		// Token: 0x04000A57 RID: 2647
		internal static LineSegmentCollider? _currentCollider = default(LineSegmentCollider?);

		// Token: 0x04000A58 RID: 2648
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

		// Token: 0x04000A59 RID: 2649
		private static readonly string[] TypedTransformAttributeCodes = new string[]
		{
			"onTongTransformByType",
			"onMetalTongTransformByType",
			"inForgeTransformByType"
		};

		// Token: 0x020002E3 RID: 739
		[Nullable(0)]
		private sealed class EditableTransform
		{
			// Token: 0x06001943 RID: 6467 RVA: 0x0006FFD4 File Offset: 0x0006E1D4
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

			// Token: 0x1700057A RID: 1402
			// (get) Token: 0x06001944 RID: 6468 RVA: 0x0006FFF1 File Offset: 0x0006E1F1
			public ModelTransform Transform { get; }

			// Token: 0x1700057B RID: 1403
			// (get) Token: 0x06001945 RID: 6469 RVA: 0x0006FFF9 File Offset: 0x0006E1F9
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

			// Token: 0x1700057C RID: 1404
			// (get) Token: 0x06001946 RID: 6470 RVA: 0x00070001 File Offset: 0x0006E201
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

		// Token: 0x020002E4 RID: 740
		[Nullable(0)]
		private sealed class SourceSaveRequest
		{
			// Token: 0x06001947 RID: 6471 RVA: 0x00070009 File Offset: 0x0006E209
			public SourceSaveRequest(string sourceFile, string oldText, string newText, string successStatus, Func<string> commit)
			{
				this.SourceFile = sourceFile;
				this.OldText = oldText;
				this.NewText = newText;
				this.SuccessStatus = successStatus;
				this._commit = commit;
			}

			// Token: 0x1700057D RID: 1405
			// (get) Token: 0x06001948 RID: 6472 RVA: 0x00070036 File Offset: 0x0006E236
			public string SourceFile { get; }

			// Token: 0x1700057E RID: 1406
			// (get) Token: 0x06001949 RID: 6473 RVA: 0x0007003E File Offset: 0x0006E23E
			public string OldText { get; }

			// Token: 0x1700057F RID: 1407
			// (get) Token: 0x0600194A RID: 6474 RVA: 0x00070046 File Offset: 0x0006E246
			public string NewText { get; }

			// Token: 0x17000580 RID: 1408
			// (get) Token: 0x0600194B RID: 6475 RVA: 0x0007004E File Offset: 0x0006E24E
			public string SuccessStatus { get; }

			// Token: 0x0600194C RID: 6476 RVA: 0x00070058 File Offset: 0x0006E258
			public string Commit()
			{
				string result = this._commit.Invoke();
				if (!string.IsNullOrWhiteSpace(result))
				{
					return result;
				}
				return this.SuccessStatus;
			}

			// Token: 0x04000E93 RID: 3731
			private readonly Func<string> _commit;
		}

		// Token: 0x020002E5 RID: 741
		[Nullable(0)]
		private sealed class SourceSaveResult
		{
			// Token: 0x0600194D RID: 6477 RVA: 0x00070081 File Offset: 0x0006E281
			private SourceSaveResult(string status, [Nullable(2)] DebugWindowManager.SourceSaveRequest request)
			{
				this.Status = status;
				this.Request = request;
			}

			// Token: 0x17000581 RID: 1409
			// (get) Token: 0x0600194E RID: 6478 RVA: 0x00070097 File Offset: 0x0006E297
			public string Status { get; }

			// Token: 0x17000582 RID: 1410
			// (get) Token: 0x0600194F RID: 6479 RVA: 0x0007009F File Offset: 0x0006E29F
			[Nullable(2)]
			public DebugWindowManager.SourceSaveRequest Request { [NullableContext(2)] get; }

			// Token: 0x06001950 RID: 6480 RVA: 0x000700A7 File Offset: 0x0006E2A7
			public static DebugWindowManager.SourceSaveResult Fail(string status)
			{
				return new DebugWindowManager.SourceSaveResult(status, null);
			}

			// Token: 0x06001951 RID: 6481 RVA: 0x000700B0 File Offset: 0x0006E2B0
			public static DebugWindowManager.SourceSaveResult Preview(DebugWindowManager.SourceSaveRequest request)
			{
				return new DebugWindowManager.SourceSaveResult("", request);
			}
		}
	}
}
