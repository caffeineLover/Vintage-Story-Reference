using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Compatibility;
using CombatOverhaul.Integration;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace CombatOverhaul.Colliders
{
	// Token: 0x0200015D RID: 349
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CollidersEntityBehavior : EntityBehavior
	{
		// Token: 0x0600117B RID: 4475 RVA: 0x00053AA8 File Offset: 0x00051CA8
		public CollidersEntityBehavior(Entity entity)
		{
			Dictionary<ColliderTypes, int> dictionary = new Dictionary<ColliderTypes, int>();
			dictionary.Add(ColliderTypes.Torso, -1);
			dictionary.Add(ColliderTypes.Head, ColorUtil.ColorFromRgba(255, 0, 0, 255));
			dictionary.Add(ColliderTypes.Arm, ColorUtil.ColorFromRgba(0, 255, 0, 255));
			dictionary.Add(ColliderTypes.Leg, ColorUtil.ColorFromRgba(0, 0, 255, 255));
			dictionary.Add(ColliderTypes.Critical, ColorUtil.ColorFromRgba(255, 255, 0, 255));
			dictionary.Add(ColliderTypes.Resistant, ColorUtil.ColorFromRgba(255, 0, 255, 255));
			this._colliderColors = dictionary;
			this._defaultConfig = new CollidersConfig();
			this._lastTransformedColliderCount = -1;
			this._lastTransformationMatricesLength = -1;
			this._lastEntityPosX = double.NaN;
			this._lastEntityPosY = double.NaN;
			this._lastEntityPosZ = double.NaN;
			this._lastEntityYaw = double.NaN;
			this._lastEntityPitch = double.NaN;
			this._lastEntityRoll = double.NaN;
			this._lastColliderJointIds = Array.Empty<int>();
			this._lastColliderJointMatrices = Array.Empty<float>();
			base..ctor(entity);
			this._settings = entity.Api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).Settings;
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x00053C32 File Offset: 0x00051E32
		// (set) Token: 0x0600117D RID: 4477 RVA: 0x00053C3A File Offset: 0x00051E3A
		public CuboidAABBCollider BoundingBox { get; private set; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x00053C43 File Offset: 0x00051E43
		// (set) Token: 0x0600117F RID: 4479 RVA: 0x00053C4B File Offset: 0x00051E4B
		public bool HasOBBCollider { get; private set; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x00053C54 File Offset: 0x00051E54
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x00053C5C File Offset: 0x00051E5C
		public bool UnprocessedElementsLeft { get; set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x00053C65 File Offset: 0x00051E65
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x00053C6D File Offset: 0x00051E6D
		public bool UnprocessedElementsLeftCustom { get; set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x00053C76 File Offset: 0x00051E76
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x00053C7E File Offset: 0x00051E7E
		public HashSet<string> ShapeElementsToProcess { get; private set; } = new HashSet<string>();

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x00053C87 File Offset: 0x00051E87
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x00053C8F File Offset: 0x00051E8F
		public Dictionary<string, ColliderTypes> CollidersTypes { get; private set; } = new Dictionary<string, ColliderTypes>();

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x00053C98 File Offset: 0x00051E98
		// (set) Token: 0x06001189 RID: 4489 RVA: 0x00053CA0 File Offset: 0x00051EA0
		public Dictionary<string, ShapeElementCollider> Colliders { get; private set; } = new Dictionary<string, ShapeElementCollider>();

		// Token: 0x0600118A RID: 4490 RVA: 0x00053CA9 File Offset: 0x00051EA9
		public override string PropertyName()
		{
			return "CombatOverhaul:EntityColliders";
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x00053CB0 File Offset: 0x00051EB0
		// (set) Token: 0x0600118C RID: 4492 RVA: 0x00053CB8 File Offset: 0x00051EB8
		[Nullable(2)]
		internal ClientAnimator Animator { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x00053CC1 File Offset: 0x00051EC1
		// (set) Token: 0x0600118E RID: 4494 RVA: 0x00053CC8 File Offset: 0x00051EC8
		public static bool RenderColliders { get; set; }

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x00053CD0 File Offset: 0x00051ED0
		// (set) Token: 0x06001190 RID: 4496 RVA: 0x00053CD8 File Offset: 0x00051ED8
		public float DefaultPenetrationResistance { get; set; } = 5f;

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x00053CE1 File Offset: 0x00051EE1
		// (set) Token: 0x06001192 RID: 4498 RVA: 0x00053CE9 File Offset: 0x00051EE9
		public Dictionary<string, float> PenetrationResistances { get; set; } = new Dictionary<string, float>();

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001193 RID: 4499 RVA: 0x00053CF2 File Offset: 0x00051EF2
		// (set) Token: 0x06001194 RID: 4500 RVA: 0x00053CFA File Offset: 0x00051EFA
		public bool ResistantCollidersStopProjectiles { get; set; } = true;

		// Token: 0x06001195 RID: 4501 RVA: 0x00053D04 File Offset: 0x00051F04
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			try
			{
				if (!attributes.KeyExists("elements"))
				{
					ICoreAPI api = this.entity.Api;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(86, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Error on parsing behavior properties for entity: ");
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.entity.Code);
					defaultInterpolatedStringHandler.AppendLiteral(". 'elements' attribute was not found.");
					LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					this._defaultConfig = attributes.AsObject<CollidersConfig>(null);
					this.ApplyConfig(this._defaultConfig);
					this._timeSinceLastUpdate = (float)this.entity.Api.World.Rand.NextDouble() * 0.033333335f;
				}
			}
			catch (Exception exception)
			{
				ICoreAPI api2 = this.entity.Api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(62, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Error on parsing behavior properties for entity: ");
				defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>(this.entity.Code);
				defaultInterpolatedStringHandler2.AppendLiteral(". Exception:\n");
				defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api2, this, defaultInterpolatedStringHandler2.ToStringAndClear());
				this.UnprocessedElementsLeft = false;
				this.HasOBBCollider = false;
			}
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00053E28 File Offset: 0x00052028
		public override void AfterInitialized(bool onFirstSpawn)
		{
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00053E2A File Offset: 0x0005202A
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			PlayerModelSkinSubscription subscribedSkinBehavior = this._subscribedSkinBehavior;
			if (subscribedSkinBehavior != null)
			{
				subscribedSkinBehavior.Dispose();
			}
			this._subscribedSkinBehavior = null;
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00053E4C File Offset: 0x0005204C
		public override void OnGameTick(float deltaTime)
		{
			ICoreClientAPI clientApi = this.entity.Api as ICoreClientAPI;
			if (clientApi == null)
			{
				return;
			}
			if (!this._subscribed)
			{
				if (clientApi.ModLoader.IsModEnabled("playermodellib"))
				{
					this.SubscribeOnModelChange();
				}
				this._subscribed = true;
			}
			this._timeSinceLastUpdate += deltaTime;
			if (this._timeSinceLastUpdate < 0.033333335f)
			{
				return;
			}
			this._timeSinceLastUpdate = 0f;
			if (!this.HasOBBCollider || !this.entity.Alive)
			{
				return;
			}
			bool needsElementProcessing = this.UnprocessedElementsLeft || this.UnprocessedElementsLeftCustom;
			if (!this.entity.IsRendered && !needsElementProcessing)
			{
				return;
			}
			IAnimationManager animManager = this.entity.AnimManager;
			ClientAnimator animator = ((animManager != null) ? animManager.Animator : null) as ClientAnimator;
			if (animator == null)
			{
				return;
			}
			this.Animator = animator;
			if (this.UnprocessedElementsLeftCustom)
			{
				this.ProcessCollidersForCustomModel(animator);
			}
			else if (this.UnprocessedElementsLeft)
			{
				this.ProcessConfiguredColliderElements(animator);
			}
			if (this.entity.IsRendered && this.ShouldRecalculateColliders(animator))
			{
				this.RecalculateColliders(animator, clientApi);
			}
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00053F5C File Offset: 0x0005215C
		public void Render(ICoreClientAPI api, EntityAgent entityPlayer, EntityShapeRenderer renderer, int color = -1)
		{
			ICoreClientAPI coreClientAPI = this.entity.Api as ICoreClientAPI;
			bool flag;
			if (coreClientAPI != null)
			{
				IClientWorldAccessor world = coreClientAPI.World;
				if (world != null)
				{
					IClientPlayer player = world.Player;
					if (player != null)
					{
						flag = (player.CameraMode == 0);
						goto IL_37;
					}
				}
			}
			flag = false;
			IL_37:
			bool firstPerson = flag;
			if (api.World.Player.Entity.EntityId == entityPlayer.EntityId && firstPerson)
			{
				return;
			}
			if (!this.HasOBBCollider || !this.entity.Alive)
			{
				return;
			}
			string text;
			ShapeElementCollider shapeElementCollider;
			if (!this._renderersAssigned)
			{
				foreach (KeyValuePair<string, ShapeElementCollider> keyValuePair in this.Colliders)
				{
					keyValuePair.Deconstruct(ref text, ref shapeElementCollider);
					ShapeElementCollider collider = shapeElementCollider;
					if (!collider.HasRenderer)
					{
						shapeElementCollider = collider;
						if (shapeElementCollider.Renderer == null)
						{
							shapeElementCollider.Renderer = renderer;
						}
						collider.HasRenderer = true;
					}
				}
				this._renderersAssigned = (this.Colliders.Count > 0);
			}
			if (!CollidersEntityBehavior.RenderColliders)
			{
				return;
			}
			IShaderProgram currentShader = api.Render.CurrentActiveShader;
			if (currentShader != null)
			{
				currentShader.Stop();
			}
			foreach (KeyValuePair<string, ShapeElementCollider> keyValuePair in this.Colliders)
			{
				keyValuePair.Deconstruct(ref text, ref shapeElementCollider);
				string id = text;
				ShapeElementCollider collider2 = shapeElementCollider;
				ColliderTypes value;
				if (this.CollidersTypes.TryGetValue(id, ref value))
				{
					collider2.Render(api, entityPlayer, this._colliderColors[value]);
				}
			}
			if (currentShader != null)
			{
				currentShader.Use();
			}
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0005410C File Offset: 0x0005230C
		public bool CollideAABB(Vector3d thisTickOrigin, Vector3d previousTickOrigin, float radius, float penetrationDistance, [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out List<ValueTuple<string, double, Vector3d>> intersections)
		{
			intersections = new List<ValueTuple<string, double, Vector3d>>();
			CuboidAABBCollider AABBCollider = new CuboidAABBCollider(this.entity);
			double effectiveRadius = (double)(radius + Math.Max(0f, penetrationDistance));
			Vector3d intersection;
			if (!AABBCollider.Collide(thisTickOrigin, previousTickOrigin, effectiveRadius, out intersection))
			{
				return false;
			}
			intersections.Add(new ValueTuple<string, double, Vector3d>("", Vector3d.Distance(previousTickOrigin, intersection), intersection));
			return true;
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0005416C File Offset: 0x0005236C
		public bool Collide(Vector3d segmentStart, Vector3d segmentDirection, out string collider, out double parameter, out Vector3d intersection)
		{
			parameter = 3.4028234663852886E+38;
			bool foundIntersection = false;
			collider = "";
			intersection = Vector3d.Zero;
			if (!this.HasUsableObbColliders())
			{
				CuboidAABBCollider AABBCollider = new CuboidAABBCollider(this.entity);
				bool result = AABBCollider.Collide(segmentStart, segmentDirection, out parameter);
				intersection = segmentStart + parameter * segmentDirection;
				return result;
			}
			double num;
			if (!this.BoundingBox.Collide(segmentStart, segmentDirection, out num))
			{
				return false;
			}
			foreach (KeyValuePair<string, ShapeElementCollider> keyValuePair in this.Colliders)
			{
				string text;
				ShapeElementCollider shapeElementCollider;
				keyValuePair.Deconstruct(ref text, ref shapeElementCollider);
				string key = text;
				double currentParameter;
				Vector3d currentIntersection;
				if (shapeElementCollider.Collide(segmentStart, segmentDirection, out currentParameter, out currentIntersection) && currentParameter < parameter)
				{
					parameter = currentParameter;
					collider = key;
					intersection = currentIntersection;
					foundIntersection = true;
				}
			}
			return foundIntersection;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00054264 File Offset: 0x00052464
		public bool Collide(Vector3d thisTickOrigin, Vector3d previousTickOrigin, float radius, float penetrationDistance, [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out List<ValueTuple<string, double, Vector3d>> intersections)
		{
			intersections = new List<ValueTuple<string, double, Vector3d>>();
			bool foundIntersection = false;
			if (!this.HasUsableObbColliders())
			{
				CuboidAABBCollider AABBCollider = new CuboidAABBCollider(this.entity);
				Vector3d intersection;
				return AABBCollider.Collide(thisTickOrigin, previousTickOrigin, (double)radius, out intersection);
			}
			Vector3d vector3d;
			if (!this.BoundingBox.Collide(thisTickOrigin, previousTickOrigin, (double)radius, out vector3d))
			{
				return false;
			}
			Vector3d firstIntersection = previousTickOrigin;
			double lowestParameter = 1.0;
			string text;
			ShapeElementCollider shapeElementCollider;
			foreach (KeyValuePair<string, ShapeElementCollider> keyValuePair in this.Colliders)
			{
				keyValuePair.Deconstruct(ref text, ref shapeElementCollider);
				double num;
				Vector3d segmentClosestPoint;
				if (shapeElementCollider.Collide(thisTickOrigin, previousTickOrigin, (double)radius, out num, out vector3d, out segmentClosestPoint))
				{
					double length = (segmentClosestPoint - previousTickOrigin).Length;
					vector3d = thisTickOrigin - previousTickOrigin;
					double parameter = length / vector3d.Length;
					if (lowestParameter >= parameter)
					{
						firstIntersection = segmentClosestPoint;
						lowestParameter = parameter;
					}
					foundIntersection = true;
				}
			}
			if (foundIntersection)
			{
				Vector3d thisTickOriginAdjustedForPenetration = firstIntersection + Vector3d.Normalize(thisTickOrigin - previousTickOrigin) * (double)penetrationDistance;
				foundIntersection = false;
				foreach (KeyValuePair<string, ShapeElementCollider> keyValuePair in this.Colliders)
				{
					keyValuePair.Deconstruct(ref text, ref shapeElementCollider);
					string key = text;
					double currentDistance;
					Vector3d currentIntersection;
					Vector3d segmentClosestPoint2;
					if (shapeElementCollider.Collide(thisTickOriginAdjustedForPenetration, previousTickOrigin, (double)radius, out currentDistance, out currentIntersection, out segmentClosestPoint2))
					{
						double num2 = (segmentClosestPoint2 - previousTickOrigin).Length + currentDistance;
						vector3d = thisTickOrigin - previousTickOrigin;
						double parameter2 = num2 / vector3d.Length;
						intersections.Add(new ValueTuple<string, double, Vector3d>(key, parameter2, currentIntersection));
						foundIntersection = true;
					}
				}
			}
			intersections.Sort(([Nullable(new byte[]
			{
				0,
				1
			})] ValueTuple<string, double, Vector3d> first, [Nullable(new byte[]
			{
				0,
				1
			})] ValueTuple<string, double, Vector3d> second) => first.Item2.CompareTo(second.Item2));
			return foundIntersection;
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0005443C File Offset: 0x0005263C
		public bool Collide(Vector3d thisTickOrigin, Vector3d previousTickOrigin, float radius, [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out List<ValueTuple<string, double, Vector3d>> intersections)
		{
			intersections = new List<ValueTuple<string, double, Vector3d>>();
			bool foundIntersection = false;
			if (!this.HasUsableObbColliders())
			{
				CuboidAABBCollider AABBCollider = new CuboidAABBCollider(this.entity);
				Vector3d intersection;
				return AABBCollider.Collide(thisTickOrigin, previousTickOrigin, (double)radius, out intersection);
			}
			Vector3d vector3d;
			if (!this.BoundingBox.Collide(thisTickOrigin, previousTickOrigin, (double)radius, out vector3d))
			{
				return false;
			}
			double lowestParameter = 1.0;
			string text;
			ShapeElementCollider shapeElementCollider;
			foreach (KeyValuePair<string, ShapeElementCollider> keyValuePair in this.Colliders)
			{
				keyValuePair.Deconstruct(ref text, ref shapeElementCollider);
				double num;
				Vector3d segmentClosestPoint;
				if (shapeElementCollider.Collide(thisTickOrigin, previousTickOrigin, (double)radius, out num, out vector3d, out segmentClosestPoint))
				{
					double length = (segmentClosestPoint - previousTickOrigin).Length;
					vector3d = thisTickOrigin - previousTickOrigin;
					double parameter = length / vector3d.Length;
					if (lowestParameter >= parameter)
					{
						lowestParameter = parameter;
					}
					foundIntersection = true;
				}
			}
			if (foundIntersection)
			{
				foundIntersection = false;
				foreach (KeyValuePair<string, ShapeElementCollider> keyValuePair in this.Colliders)
				{
					keyValuePair.Deconstruct(ref text, ref shapeElementCollider);
					string key = text;
					double currentDistance;
					Vector3d currentIntersection;
					Vector3d segmentClosestPoint2;
					if (shapeElementCollider.Collide(thisTickOrigin, previousTickOrigin, (double)radius, out currentDistance, out currentIntersection, out segmentClosestPoint2))
					{
						Vector3d segmentPoint = segmentClosestPoint2 - previousTickOrigin;
						double num2 = 1.0;
						double num3 = segmentPoint.Length + currentDistance;
						vector3d = thisTickOrigin - previousTickOrigin;
						double parameter2 = GameMath.Clamp(num2 - num3 / vector3d.Length, 0.0, 1.0);
						intersections.Add(new ValueTuple<string, double, Vector3d>(key, parameter2, currentIntersection));
						foundIntersection = true;
					}
				}
			}
			intersections.Sort(([Nullable(new byte[]
			{
				0,
				1
			})] ValueTuple<string, double, Vector3d> first, [Nullable(new byte[]
			{
				0,
				1
			})] ValueTuple<string, double, Vector3d> second) => first.Item2.CompareTo(second.Item2));
			return foundIntersection;
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00054618 File Offset: 0x00052818
		public bool Collide(Vector3d thisTickOrigin, Vector3d previousTickOrigin, float radius, out string collider, out double parameter, out Vector3d intersection)
		{
			collider = "";
			parameter = 0.0;
			intersection = Vector3d.Zero;
			if (!this.HasUsableObbColliders())
			{
				CuboidAABBCollider AABBCollider = new CuboidAABBCollider(this.entity);
				return AABBCollider.Collide(thisTickOrigin, previousTickOrigin, (double)radius, out intersection);
			}
			Vector3d vector3d;
			if (!this.BoundingBox.Collide(thisTickOrigin, previousTickOrigin, (double)radius, out vector3d))
			{
				return false;
			}
			foreach (KeyValuePair<string, ShapeElementCollider> keyValuePair in this.Colliders)
			{
				string text;
				ShapeElementCollider shapeElementCollider;
				keyValuePair.Deconstruct(ref text, ref shapeElementCollider);
				string key = text;
				double currentDistance;
				Vector3d currentIntersection;
				Vector3d segmentClosestPoint;
				if (shapeElementCollider.Collide(thisTickOrigin, previousTickOrigin, (double)radius, out currentDistance, out currentIntersection, out segmentClosestPoint))
				{
					intersection = segmentClosestPoint - previousTickOrigin;
					double num = intersection.Length + currentDistance;
					vector3d = thisTickOrigin - previousTickOrigin;
					parameter = num / vector3d.Length;
					collider = key;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0005471C File Offset: 0x0005291C
		public bool Collide(Vector3d thisTickStart, Vector3d previousTickStart, Vector3d thisTickDirection, Vector3d previousTickDirection, int subdivisions, out string collider, out double parameter, out Vector3d intersection)
		{
			Vector3d startTail = previousTickStart + previousTickDirection;
			Vector3d directionHead = thisTickStart - previousTickStart;
			Vector3d directionTail = thisTickStart + thisTickDirection - startTail;
			for (int subdivision = 0; subdivision < subdivisions; subdivision++)
			{
				float subdivisionParameter = (float)subdivision / (float)subdivisions;
				Vector3d head = previousTickStart + directionHead * (double)subdivisionParameter;
				Vector3d tail = startTail + directionTail * (double)subdivisionParameter;
				if (this.Collide(head, tail - head, out collider, out parameter, out intersection))
				{
					return true;
				}
			}
			collider = "";
			parameter = 0.0;
			intersection = Vector3d.Zero;
			return false;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x000547C4 File Offset: 0x000529C4
		public bool Collide(Vector3d thisTickStart, Vector3d previousTickStart, Vector3d thisTickDirection, Vector3d previousTickDirection, float radius, out string collider, out double parameter, out Vector3d intersection)
		{
			collider = "";
			parameter = 0.0;
			intersection = Vector3d.Zero;
			Vector3d startTail = previousTickStart + previousTickDirection;
			Vector3d directionHead = thisTickStart - previousTickStart;
			Vector3d directionTail = thisTickStart + thisTickDirection - startTail;
			double maxSweepDistance = Math.Max((thisTickStart - previousTickStart).Length, (thisTickStart + thisTickDirection - previousTickStart - previousTickDirection).Length);
			double preferredStep = Math.Max(0.045, (double)radius * 0.35);
			int subdivisions = Math.Max(10, (int)Math.Ceiling(maxSweepDistance / preferredStep));
			List<ValueTuple<string, double, Vector3d>> intersections = new List<ValueTuple<string, double, Vector3d>>();
			int subdivision = 0;
			while (subdivision < subdivisions)
			{
				float subdivisionParameter = (float)subdivision / (float)subdivisions;
				Vector3d head = previousTickStart + directionHead * (double)subdivisionParameter;
				Vector3d tail = startTail + directionTail * (double)subdivisionParameter;
				List<ValueTuple<string, double, Vector3d>> currentIntersections;
				if (this.Collide(head, tail, radius, out currentIntersections))
				{
					intersections.AddRange(currentIntersections);
					if (this._settings.DebugWeaponTrailParticles)
					{
						Vec3d pos7 = new Vec3d(head.X, head.Y, head.Z);
						this.entity.Api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(125, 125, 255, 128), pos7, pos7, new Vec3f(), new Vec3f(), 0.5f, 0f, 1f * radius * 16f * this._settings.DebugWeaponTrailParticlesSize, 1, null);
						Vec3d pos8 = new Vec3d(tail.X, tail.Y, tail.Z);
						this.entity.Api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(125, 125, 255, 128), pos8, pos8, new Vec3f(), new Vec3f(), 0.5f, 0f, 1f * radius * 16f * this._settings.DebugWeaponTrailParticlesSize, 1, null);
						float c = 8f;
						int i = 0;
						while ((float)i < c)
						{
							Vec3d pos9 = pos7 + (float)i / c * (pos8 - pos7);
							this.entity.Api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(64, 64, 128, 128), pos9, pos9, new Vec3f(), new Vec3f(), 0.5f, 0f, radius * 16f * this._settings.DebugWeaponTrailParticlesSize, 1, null);
							i++;
						}
						break;
					}
					break;
				}
				else
				{
					if (this._settings.DebugWeaponTrailParticles)
					{
						Vec3d pos10 = new Vec3d(head.X, head.Y, head.Z);
						this.entity.Api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(125, 125, 125, 128), pos10, pos10, new Vec3f(), new Vec3f(), 0.5f, 0f, 1f * radius * 16f * this._settings.DebugWeaponTrailParticlesSize, 1, null);
						Vec3d pos11 = new Vec3d(tail.X, tail.Y, tail.Z);
						this.entity.Api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(125, 125, 125, 128), pos11, pos11, new Vec3f(), new Vec3f(), 0.5f, 0f, 1f * radius * 16f * this._settings.DebugWeaponTrailParticlesSize, 1, null);
						float c2 = 8f;
						int j = 0;
						while ((float)j < c2)
						{
							Vec3d pos12 = pos10 + (float)j / c2 * (pos11 - pos10);
							this.entity.Api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(64, 64, 64, 128), pos12, pos12, new Vec3f(), new Vec3f(), 0.5f, 0f, radius * 16f * this._settings.DebugWeaponTrailParticlesSize, 1, null);
							j++;
						}
					}
					subdivision++;
				}
			}
			if (intersections.Count != 0)
			{
				intersections.Sort(([Nullable(new byte[]
				{
					0,
					1
				})] ValueTuple<string, double, Vector3d> first, [Nullable(new byte[]
				{
					0,
					1
				})] ValueTuple<string, double, Vector3d> second) => first.Item2.CompareTo(second.Item2));
				double smallestParameter = 1.0;
				foreach (ValueTuple<string, double, Vector3d> valueTuple in intersections)
				{
					string firstCollider = valueTuple.Item1;
					double firstParameter = valueTuple.Item2;
					Vector3d firstIntersection = valueTuple.Item3;
					if (smallestParameter >= firstParameter)
					{
						smallestParameter = firstParameter;
						collider = firstCollider;
						parameter = firstParameter;
						intersection = firstIntersection;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00054CC4 File Offset: 0x00052EC4
		private bool HasUsableObbColliders()
		{
			return this.HasOBBCollider && this.Colliders.Count > 0;
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060011A2 RID: 4514 RVA: 0x00054CDE File Offset: 0x00052EDE
		public bool HasUsableDetailedColliders
		{
			get
			{
				return this.HasUsableObbColliders();
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060011A3 RID: 4515 RVA: 0x00054CE6 File Offset: 0x00052EE6
		[Nullable(2)]
		private ICoreAPI Api
		{
			[NullableContext(2)]
			get
			{
				Entity entity = this.entity;
				if (entity == null)
				{
					return null;
				}
				return entity.Api;
			}
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x00054CFC File Offset: 0x00052EFC
		private void SetColliderElement(ShapeElement element)
		{
			if (element == null || element.Name == null || element.From == null || element.To == null)
			{
				return;
			}
			if (this.UnprocessedElementsLeft && this.ShapeElementsToProcess.Contains(element.Name))
			{
				this.Colliders[element.Name] = new ShapeElementCollider(element);
				this.ShapeElementsToProcess.Remove(element.Name);
				this.UnprocessedElementsLeft = (this.ShapeElementsToProcess.Count > 0);
				this._renderersAssigned = false;
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00054D84 File Offset: 0x00052F84
		private void AddPoseShapeElements(ElementPose pose)
		{
			this.SetColliderElement(pose.ForElement);
			foreach (ElementPose childPose in pose.ChildElementPoses)
			{
				this.AddPoseShapeElements(childPose);
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00054DE4 File Offset: 0x00052FE4
		private void RecalculateColliders(ClientAnimator animator, ICoreClientAPI clientApi)
		{
			foreach (KeyValuePair<string, ShapeElementCollider> keyValuePair in this.Colliders)
			{
				string text;
				ShapeElementCollider shapeElementCollider;
				keyValuePair.Deconstruct(ref text, ref shapeElementCollider);
				shapeElementCollider.Transform(animator.TransformationMatrices, clientApi);
			}
			this.CalculateBoundingBox();
			this._collidersTransformed = true;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00054E58 File Offset: 0x00053058
		private bool ShouldRecalculateColliders(ClientAnimator animator)
		{
			if (!this._collidersTransformed || !this._colliderTransformSnapshotValid)
			{
				this.CaptureColliderTransformState(animator);
				return true;
			}
			if (!this.ColliderTransformStateChanged(animator))
			{
				return false;
			}
			this.CaptureColliderTransformState(animator);
			return true;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00054E88 File Offset: 0x00053088
		private bool ColliderTransformStateChanged(ClientAnimator animator)
		{
			if (this.Colliders.Count != this._lastTransformedColliderCount)
			{
				return true;
			}
			EntityPos position = this.entity.Pos;
			if (position.X != this._lastEntityPosX || position.Y != this._lastEntityPosY || position.Z != this._lastEntityPosZ || (double)position.Yaw != this._lastEntityYaw || (double)position.Pitch != this._lastEntityPitch || (double)position.Roll != this._lastEntityRoll)
			{
				return true;
			}
			float[] transformMatrices = animator.TransformationMatrices;
			if (transformMatrices.Length != this._lastTransformationMatricesLength)
			{
				return true;
			}
			if (this._lastColliderJointIds.Length != this.Colliders.Count || this._lastColliderJointMatrices.Length != this.Colliders.Count * 16)
			{
				return true;
			}
			int colliderIndex = 0;
			foreach (ShapeElementCollider collider in this.Colliders.Values)
			{
				if (colliderIndex >= this._lastColliderJointIds.Length || this._lastColliderJointIds[colliderIndex] != collider.JointId)
				{
					return true;
				}
				int offset = collider.JointId * 16;
				int snapshotOffset = colliderIndex * 16;
				for (int index = 0; index < 16; index++)
				{
					if (BitConverter.SingleToInt32Bits((offset + index < transformMatrices.Length) ? transformMatrices[offset + index] : 0f) != BitConverter.SingleToInt32Bits(this._lastColliderJointMatrices[snapshotOffset + index]))
					{
						return true;
					}
				}
				colliderIndex++;
			}
			return false;
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00055020 File Offset: 0x00053220
		private void CaptureColliderTransformState(ClientAnimator animator)
		{
			EntityPos position = this.entity.Pos;
			this._lastEntityPosX = position.X;
			this._lastEntityPosY = position.Y;
			this._lastEntityPosZ = position.Z;
			this._lastEntityYaw = (double)position.Yaw;
			this._lastEntityPitch = (double)position.Pitch;
			this._lastEntityRoll = (double)position.Roll;
			this._lastTransformedColliderCount = this.Colliders.Count;
			float[] transformMatrices = animator.TransformationMatrices;
			this._lastTransformationMatricesLength = transformMatrices.Length;
			int requiredMatrixValues = this.Colliders.Count * 16;
			if (this._lastColliderJointIds.Length != this.Colliders.Count)
			{
				this._lastColliderJointIds = new int[this.Colliders.Count];
			}
			if (this._lastColliderJointMatrices.Length != requiredMatrixValues)
			{
				this._lastColliderJointMatrices = new float[requiredMatrixValues];
			}
			int colliderIndex = 0;
			foreach (ShapeElementCollider collider in this.Colliders.Values)
			{
				this._lastColliderJointIds[colliderIndex] = collider.JointId;
				int offset = collider.JointId * 16;
				int snapshotOffset = colliderIndex * 16;
				for (int index = 0; index < 16; index++)
				{
					this._lastColliderJointMatrices[snapshotOffset + index] = ((offset + index < transformMatrices.Length) ? transformMatrices[offset + index] : 0f);
				}
				colliderIndex++;
			}
			this._colliderTransformSnapshotValid = true;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x000551A0 File Offset: 0x000533A0
		private void CalculateBoundingBox()
		{
			if (this.Colliders.Count == 0)
			{
				this.BoundingBox = new CuboidAABBCollider(this.entity);
				return;
			}
			Vector3d min;
			min..ctor(3.4028234663852886E+38, 3.4028234663852886E+38, 3.4028234663852886E+38);
			Vector3d max;
			max..ctor(-3.4028234663852886E+38, -3.4028234663852886E+38, -3.4028234663852886E+38);
			foreach (ShapeElementCollider collider in this.Colliders.Values)
			{
				for (int vertex = 0; vertex < 8; vertex++)
				{
					Vector4d inworldVertex = collider.InworldVertices[vertex];
					min.X = Math.Min(min.X, inworldVertex.X);
					min.Y = Math.Min(min.Y, inworldVertex.Y);
					min.Z = Math.Min(min.Z, inworldVertex.Z);
					max.X = Math.Max(max.X, inworldVertex.X);
					max.Y = Math.Max(max.Y, inworldVertex.Y);
					max.Z = Math.Max(max.Z, inworldVertex.Z);
				}
			}
			this.BoundingBox = new CuboidAABBCollider(min, max);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x00055324 File Offset: 0x00053524
		private void ReloadCollidersForCustomModel(string modelCode)
		{
			ICoreAPI api = this.Api;
			PlayerModelLibCompatibilitySystem system = (api != null) ? api.ModLoader.GetModSystem<PlayerModelLibCompatibilitySystem>(true) : null;
			if (system == null)
			{
				return;
			}
			PlayerModelConfig customModelConfig;
			if (!system.CustomModelConfigs.TryGetValue(modelCode, ref customModelConfig) || customModelConfig.Colliders == null)
			{
				this.ApplyConfig(this._defaultConfig);
				return;
			}
			this.CollidersTypes.Clear();
			this.ShapeElementsToProcess.Clear();
			this.Colliders.Clear();
			this._renderersAssigned = false;
			this.ResetColliderResolutionState();
			this.ResetColliderTransformState();
			ColliderTypesJson types = customModelConfig.Colliders.Elements;
			foreach (string collider in types.Torso)
			{
				this.CollidersTypes.Add(collider, ColliderTypes.Torso);
				this.ShapeElementsToProcess.Add(collider);
			}
			foreach (string collider2 in types.Head)
			{
				this.CollidersTypes.Add(collider2, ColliderTypes.Head);
				this.ShapeElementsToProcess.Add(collider2);
			}
			foreach (string collider3 in types.Arm)
			{
				this.CollidersTypes.Add(collider3, ColliderTypes.Arm);
				this.ShapeElementsToProcess.Add(collider3);
			}
			foreach (string collider4 in types.Leg)
			{
				this.CollidersTypes.Add(collider4, ColliderTypes.Leg);
				this.ShapeElementsToProcess.Add(collider4);
			}
			foreach (string collider5 in types.Critical)
			{
				this.CollidersTypes.Add(collider5, ColliderTypes.Critical);
				this.ShapeElementsToProcess.Add(collider5);
			}
			foreach (string collider6 in types.Resistant)
			{
				this.CollidersTypes.Add(collider6, ColliderTypes.Resistant);
				this.ShapeElementsToProcess.Add(collider6);
			}
			this.DefaultPenetrationResistance = customModelConfig.Colliders.DefaultPenetrationResistance;
			this.PenetrationResistances = customModelConfig.Colliders.PenetrationResistances;
			this.ResistantCollidersStopProjectiles = customModelConfig.Colliders.ResistantCollidersStopProjectiles;
			this.UnprocessedElementsLeftCustom = true;
			this.HasOBBCollider = true;
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00055558 File Offset: 0x00053758
		private void ApplyConfig(CollidersConfig config)
		{
			this.CollidersTypes.Clear();
			this.ShapeElementsToProcess.Clear();
			this.Colliders.Clear();
			this._renderersAssigned = false;
			this.ResetColliderResolutionState();
			this.ResetColliderTransformState();
			ColliderTypesJson types = config.Elements;
			foreach (string collider in types.Torso)
			{
				this.CollidersTypes.Add(collider, ColliderTypes.Torso);
				this.ShapeElementsToProcess.Add(collider);
			}
			foreach (string collider2 in types.Head)
			{
				this.CollidersTypes.Add(collider2, ColliderTypes.Head);
				this.ShapeElementsToProcess.Add(collider2);
			}
			foreach (string collider3 in types.Arm)
			{
				this.CollidersTypes.Add(collider3, ColliderTypes.Arm);
				this.ShapeElementsToProcess.Add(collider3);
			}
			foreach (string collider4 in types.Leg)
			{
				this.CollidersTypes.Add(collider4, ColliderTypes.Leg);
				this.ShapeElementsToProcess.Add(collider4);
			}
			foreach (string collider5 in types.Critical)
			{
				this.CollidersTypes.Add(collider5, ColliderTypes.Critical);
				this.ShapeElementsToProcess.Add(collider5);
			}
			foreach (string collider6 in types.Resistant)
			{
				this.CollidersTypes.Add(collider6, ColliderTypes.Resistant);
				this.ShapeElementsToProcess.Add(collider6);
			}
			this.DefaultPenetrationResistance = config.DefaultPenetrationResistance;
			this.PenetrationResistances = config.PenetrationResistances;
			this.ResistantCollidersStopProjectiles = config.ResistantCollidersStopProjectiles;
			this.UnprocessedElementsLeft = (this.ShapeElementsToProcess.Count > 0);
			this.HasOBBCollider = true;
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00055720 File Offset: 0x00053920
		private void SubscribeOnModelChange()
		{
			this._subscribedSkinBehavior = PlayerModelSkinSubscription.Subscribe(this.entity, new Action<string>(this.ReloadCollidersForCustomModel), true);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00055740 File Offset: 0x00053940
		private void ProcessCollidersForCustomModel(ClientAnimator animator)
		{
			if (!this.UnprocessedElementsLeftCustom)
			{
				return;
			}
			this.UnprocessedElementsLeft = (this.ShapeElementsToProcess.Count > 0);
			this.Colliders.Clear();
			this._renderersAssigned = false;
			this.ResetColliderResolutionState();
			this.ResetColliderTransformState();
			this.ProcessConfiguredColliderElements(animator);
			this.UnprocessedElementsLeftCustom = false;
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00055798 File Offset: 0x00053998
		private void ProcessConfiguredColliderElements(ClientAnimator animator)
		{
			if (!this.UnprocessedElementsLeft)
			{
				return;
			}
			this._colliderElementResolutionAttempts++;
			try
			{
				foreach (ElementPose pose in animator.RootPoses)
				{
					this.AddPoseShapeElements(pose);
				}
				if (this.ShapeElementsToProcess.Count != 0 && !this._reportedMissingColliders)
				{
					this._reportedMissingColliders = true;
				}
			}
			catch (Exception exception)
			{
				if (!this._reportedColliderCreationError)
				{
					ICoreAPI api = this.entity.Api;
					Type typeFromHandle = typeof(HarmonyPatches);
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(37, 2);
					defaultInterpolatedStringHandler.AppendLiteral("(");
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.entity.Code);
					defaultInterpolatedStringHandler.AppendLiteral(") Error during creating colliders: \n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					LoggerUtil.Error(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this._reportedColliderCreationError = true;
			}
			if (this.UnprocessedElementsLeft && this._colliderElementResolutionAttempts >= 10)
			{
				this.UnprocessedElementsLeft = false;
			}
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000558B4 File Offset: 0x00053AB4
		private void ResetColliderResolutionState()
		{
			this._colliderElementResolutionAttempts = 0;
			this._reportedMissingColliders = false;
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x000558C4 File Offset: 0x00053AC4
		private void ResetColliderTransformState()
		{
			this._collidersTransformed = false;
			this._colliderTransformSnapshotValid = false;
			this._lastTransformedColliderCount = -1;
			this._lastTransformationMatricesLength = -1;
		}

		// Token: 0x04000842 RID: 2114
		public const string PlayerModelLibId = "playermodellib";

		// Token: 0x04000843 RID: 2115
		private readonly Dictionary<ColliderTypes, int> _colliderColors;

		// Token: 0x04000844 RID: 2116
		private bool _reportedMissingColliders;

		// Token: 0x04000845 RID: 2117
		private bool _reportedColliderCreationError;

		// Token: 0x04000846 RID: 2118
		private CollidersConfig _defaultConfig;

		// Token: 0x04000847 RID: 2119
		private const int _updateFps = 30;

		// Token: 0x04000848 RID: 2120
		private const float _updateTimeSec = 0.033333335f;

		// Token: 0x04000849 RID: 2121
		private const int _maxColliderElementResolutionAttempts = 10;

		// Token: 0x0400084A RID: 2122
		private float _timeSinceLastUpdate;

		// Token: 0x0400084B RID: 2123
		private readonly Settings _settings;

		// Token: 0x0400084C RID: 2124
		private bool _subscribed;

		// Token: 0x0400084D RID: 2125
		[Nullable(2)]
		private PlayerModelSkinSubscription _subscribedSkinBehavior;

		// Token: 0x0400084E RID: 2126
		private bool _collidersTransformed;

		// Token: 0x0400084F RID: 2127
		private bool _renderersAssigned;

		// Token: 0x04000850 RID: 2128
		private int _colliderElementResolutionAttempts;

		// Token: 0x04000851 RID: 2129
		private bool _colliderTransformSnapshotValid;

		// Token: 0x04000852 RID: 2130
		private int _lastTransformedColliderCount;

		// Token: 0x04000853 RID: 2131
		private int _lastTransformationMatricesLength;

		// Token: 0x04000854 RID: 2132
		private double _lastEntityPosX;

		// Token: 0x04000855 RID: 2133
		private double _lastEntityPosY;

		// Token: 0x04000856 RID: 2134
		private double _lastEntityPosZ;

		// Token: 0x04000857 RID: 2135
		private double _lastEntityYaw;

		// Token: 0x04000858 RID: 2136
		private double _lastEntityPitch;

		// Token: 0x04000859 RID: 2137
		private double _lastEntityRoll;

		// Token: 0x0400085A RID: 2138
		private int[] _lastColliderJointIds;

		// Token: 0x0400085B RID: 2139
		private float[] _lastColliderJointMatrices;
	}
}
