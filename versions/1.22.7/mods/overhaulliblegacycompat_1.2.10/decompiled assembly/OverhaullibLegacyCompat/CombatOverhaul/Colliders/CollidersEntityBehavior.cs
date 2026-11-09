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
	// Token: 0x02000169 RID: 361
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CollidersEntityBehavior : EntityBehavior
	{
		// Token: 0x060011EF RID: 4591 RVA: 0x000553B0 File Offset: 0x000535B0
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

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x0005553A File Offset: 0x0005373A
		// (set) Token: 0x060011F1 RID: 4593 RVA: 0x00055542 File Offset: 0x00053742
		public CuboidAABBCollider BoundingBox { get; private set; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x0005554B File Offset: 0x0005374B
		// (set) Token: 0x060011F3 RID: 4595 RVA: 0x00055553 File Offset: 0x00053753
		public bool HasOBBCollider { get; private set; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x0005555C File Offset: 0x0005375C
		// (set) Token: 0x060011F5 RID: 4597 RVA: 0x00055564 File Offset: 0x00053764
		public bool UnprocessedElementsLeft { get; set; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x0005556D File Offset: 0x0005376D
		// (set) Token: 0x060011F7 RID: 4599 RVA: 0x00055575 File Offset: 0x00053775
		public bool UnprocessedElementsLeftCustom { get; set; }

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x0005557E File Offset: 0x0005377E
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x00055586 File Offset: 0x00053786
		public HashSet<string> ShapeElementsToProcess { get; private set; } = new HashSet<string>();

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x0005558F File Offset: 0x0005378F
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x00055597 File Offset: 0x00053797
		public Dictionary<string, ColliderTypes> CollidersTypes { get; private set; } = new Dictionary<string, ColliderTypes>();

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x000555A0 File Offset: 0x000537A0
		// (set) Token: 0x060011FD RID: 4605 RVA: 0x000555A8 File Offset: 0x000537A8
		public Dictionary<string, ShapeElementCollider> Colliders { get; private set; } = new Dictionary<string, ShapeElementCollider>();

		// Token: 0x060011FE RID: 4606 RVA: 0x000555B1 File Offset: 0x000537B1
		public override string PropertyName()
		{
			return "CombatOverhaul:EntityColliders";
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060011FF RID: 4607 RVA: 0x000555B8 File Offset: 0x000537B8
		// (set) Token: 0x06001200 RID: 4608 RVA: 0x000555C0 File Offset: 0x000537C0
		[Nullable(2)]
		internal ClientAnimator Animator { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001201 RID: 4609 RVA: 0x000555C9 File Offset: 0x000537C9
		// (set) Token: 0x06001202 RID: 4610 RVA: 0x000555D0 File Offset: 0x000537D0
		public static bool RenderColliders { get; set; }

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x000555D8 File Offset: 0x000537D8
		// (set) Token: 0x06001204 RID: 4612 RVA: 0x000555E0 File Offset: 0x000537E0
		public float DefaultPenetrationResistance { get; set; } = 5f;

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x000555E9 File Offset: 0x000537E9
		// (set) Token: 0x06001206 RID: 4614 RVA: 0x000555F1 File Offset: 0x000537F1
		public Dictionary<string, float> PenetrationResistances { get; set; } = new Dictionary<string, float>();

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x000555FA File Offset: 0x000537FA
		// (set) Token: 0x06001208 RID: 4616 RVA: 0x00055602 File Offset: 0x00053802
		public bool ResistantCollidersStopProjectiles { get; set; } = true;

		// Token: 0x06001209 RID: 4617 RVA: 0x0005560C File Offset: 0x0005380C
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

		// Token: 0x0600120A RID: 4618 RVA: 0x00055730 File Offset: 0x00053930
		public override void AfterInitialized(bool onFirstSpawn)
		{
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00055732 File Offset: 0x00053932
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

		// Token: 0x0600120C RID: 4620 RVA: 0x00055754 File Offset: 0x00053954
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

		// Token: 0x0600120D RID: 4621 RVA: 0x00055864 File Offset: 0x00053A64
		[NullableContext(2)]
		public void Render(ICoreClientAPI api, Entity renderedEntity, [Nullable(1)] EntityShapeRenderer renderer, int color = -1)
		{
			if (api == null || renderedEntity == null)
			{
				return;
			}
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
						goto IL_3E;
					}
				}
			}
			flag = false;
			IL_3E:
			bool firstPerson = flag;
			if (api.World.Player.Entity.EntityId == renderedEntity.EntityId && firstPerson)
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
					collider2.Render(api, api.World.Player.Entity, this._colliderColors[value]);
				}
			}
			if (currentShader != null)
			{
				currentShader.Use();
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00055A2C File Offset: 0x00053C2C
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

		// Token: 0x0600120F RID: 4623 RVA: 0x00055A8C File Offset: 0x00053C8C
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

		// Token: 0x06001210 RID: 4624 RVA: 0x00055B84 File Offset: 0x00053D84
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

		// Token: 0x06001211 RID: 4625 RVA: 0x00055D5C File Offset: 0x00053F5C
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

		// Token: 0x06001212 RID: 4626 RVA: 0x00055F38 File Offset: 0x00054138
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

		// Token: 0x06001213 RID: 4627 RVA: 0x0005603C File Offset: 0x0005423C
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

		// Token: 0x06001214 RID: 4628 RVA: 0x000560E4 File Offset: 0x000542E4
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

		// Token: 0x06001215 RID: 4629 RVA: 0x000565E4 File Offset: 0x000547E4
		private bool HasUsableObbColliders()
		{
			return this.HasOBBCollider && this.Colliders.Count > 0;
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x000565FE File Offset: 0x000547FE
		public bool HasUsableDetailedColliders
		{
			get
			{
				return this.HasUsableObbColliders();
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x00056606 File Offset: 0x00054806
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

		// Token: 0x06001218 RID: 4632 RVA: 0x0005661C File Offset: 0x0005481C
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

		// Token: 0x06001219 RID: 4633 RVA: 0x000566A4 File Offset: 0x000548A4
		private void AddPoseShapeElements(ElementPose pose)
		{
			this.SetColliderElement(pose.ForElement);
			foreach (ElementPose childPose in pose.ChildElementPoses)
			{
				this.AddPoseShapeElements(childPose);
			}
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00056704 File Offset: 0x00054904
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

		// Token: 0x0600121B RID: 4635 RVA: 0x00056778 File Offset: 0x00054978
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

		// Token: 0x0600121C RID: 4636 RVA: 0x000567A8 File Offset: 0x000549A8
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

		// Token: 0x0600121D RID: 4637 RVA: 0x00056940 File Offset: 0x00054B40
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

		// Token: 0x0600121E RID: 4638 RVA: 0x00056AC0 File Offset: 0x00054CC0
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

		// Token: 0x0600121F RID: 4639 RVA: 0x00056C44 File Offset: 0x00054E44
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

		// Token: 0x06001220 RID: 4640 RVA: 0x00056E78 File Offset: 0x00055078
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

		// Token: 0x06001221 RID: 4641 RVA: 0x00057040 File Offset: 0x00055240
		private void SubscribeOnModelChange()
		{
			this._subscribedSkinBehavior = PlayerModelSkinSubscription.Subscribe(this.entity, new Action<string>(this.ReloadCollidersForCustomModel), true);
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00057060 File Offset: 0x00055260
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

		// Token: 0x06001223 RID: 4643 RVA: 0x000570B8 File Offset: 0x000552B8
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

		// Token: 0x06001224 RID: 4644 RVA: 0x000571D4 File Offset: 0x000553D4
		private void ResetColliderResolutionState()
		{
			this._colliderElementResolutionAttempts = 0;
			this._reportedMissingColliders = false;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x000571E4 File Offset: 0x000553E4
		private void ResetColliderTransformState()
		{
			this._collidersTransformed = false;
			this._colliderTransformSnapshotValid = false;
			this._lastTransformedColliderCount = -1;
			this._lastTransformationMatricesLength = -1;
		}

		// Token: 0x04000868 RID: 2152
		public const string PlayerModelLibId = "playermodellib";

		// Token: 0x04000869 RID: 2153
		private readonly Dictionary<ColliderTypes, int> _colliderColors;

		// Token: 0x0400086A RID: 2154
		private bool _reportedMissingColliders;

		// Token: 0x0400086B RID: 2155
		private bool _reportedColliderCreationError;

		// Token: 0x0400086C RID: 2156
		private CollidersConfig _defaultConfig;

		// Token: 0x0400086D RID: 2157
		private const int _updateFps = 30;

		// Token: 0x0400086E RID: 2158
		private const float _updateTimeSec = 0.033333335f;

		// Token: 0x0400086F RID: 2159
		private const int _maxColliderElementResolutionAttempts = 10;

		// Token: 0x04000870 RID: 2160
		private float _timeSinceLastUpdate;

		// Token: 0x04000871 RID: 2161
		private readonly Settings _settings;

		// Token: 0x04000872 RID: 2162
		private bool _subscribed;

		// Token: 0x04000873 RID: 2163
		[Nullable(2)]
		private PlayerModelSkinSubscription _subscribedSkinBehavior;

		// Token: 0x04000874 RID: 2164
		private bool _collidersTransformed;

		// Token: 0x04000875 RID: 2165
		private bool _renderersAssigned;

		// Token: 0x04000876 RID: 2166
		private int _colliderElementResolutionAttempts;

		// Token: 0x04000877 RID: 2167
		private bool _colliderTransformSnapshotValid;

		// Token: 0x04000878 RID: 2168
		private int _lastTransformedColliderCount;

		// Token: 0x04000879 RID: 2169
		private int _lastTransformationMatricesLength;

		// Token: 0x0400087A RID: 2170
		private double _lastEntityPosX;

		// Token: 0x0400087B RID: 2171
		private double _lastEntityPosY;

		// Token: 0x0400087C RID: 2172
		private double _lastEntityPosZ;

		// Token: 0x0400087D RID: 2173
		private double _lastEntityYaw;

		// Token: 0x0400087E RID: 2174
		private double _lastEntityPitch;

		// Token: 0x0400087F RID: 2175
		private double _lastEntityRoll;

		// Token: 0x04000880 RID: 2176
		private int[] _lastColliderJointIds;

		// Token: 0x04000881 RID: 2177
		private float[] _lastColliderJointMatrices;
	}
}
