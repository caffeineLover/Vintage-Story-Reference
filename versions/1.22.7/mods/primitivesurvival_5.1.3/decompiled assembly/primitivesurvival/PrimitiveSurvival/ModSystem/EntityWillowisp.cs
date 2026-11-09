using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200003F RID: 63
	public class EntityWillowisp : EntityGenericGlowingAgent
	{
		// Token: 0x0600027B RID: 635 RVA: 0x0001D0F1 File Offset: 0x0001B2F1
		static EntityWillowisp()
		{
			AiTaskRegistry.Register<AiTaskButterflyWander>("butterflywander");
			AiTaskRegistry.Register<AiTaskButterflyRest>("butterflyrest");
			AiTaskRegistry.Register<AiTaskButterflyChase>("butterflychase");
			AiTaskRegistry.Register<AiTaskButterflyFlee>("butterflyflee");
			AiTaskRegistry.Register<AiTaskButterflyFeedOnFlowers>("butterflyfeedonflowers");
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0001D125 File Offset: 0x0001B325
		public override bool IsInteractable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0001D128 File Offset: 0x0001B328
		public override void Initialize(EntityProperties properties, ICoreAPI api, long inChunkIndex3d)
		{
			base.Initialize(properties, api, inChunkIndex3d);
			if (api.Side == 2)
			{
				this.WatchedAttributes.RegisterModifiedListener("windWaveIntensity", delegate()
				{
					(base.Properties.Client.Renderer as EntityShapeRenderer).WindWaveIntensity = this.WatchedAttributes.GetDouble("windWaveIntensity", 0.0);
				});
			}
			api.ModLoader.GetModSystem<WeatherSystemBase>(true);
			api.ModLoader.GetModSystem<RoomRegistry>(true);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0001D180 File Offset: 0x0001B380
		public override void OnGameTick(float dt)
		{
			if (this.World.Side == 1)
			{
				base.OnGameTick(dt);
				return;
			}
			if (!this.AnimManager.ActiveAnimationsByAnimCode.ContainsKey("feed") && !this.AnimManager.ActiveAnimationsByAnimCode.ContainsKey("rest"))
			{
				if (base.Pos.Y < base.Pos.Y - 0.25 && !base.Collided)
				{
					this.SetAnimation("glide", 1f);
				}
				else
				{
					this.SetAnimation("fly", 2f);
				}
			}
			base.OnGameTick(dt);
			int num = this.cnt;
			this.cnt = num + 1;
			if (num > 30)
			{
				float affectedness = this.World.BlockAccessor.GetLightLevel(base.Pos.XYZ.AsBlockPos, 1) < 14;
				this.windMotion = this.Api.ModLoader.GetModSystem<WeatherSystemBase>(true).WeatherDataSlowAccess.GetWindSpeed(base.Pos.XYZ * affectedness);
				this.cnt = 0;
			}
			if (this.AnimManager.ActiveAnimationsByAnimCode.ContainsKey("fly"))
			{
				base.Pos.X += Math.Max(0.0, (this.windMotion - 0.2) / 20.0);
			}
			if (base.Pos.SquareDistanceTo(base.Pos.XYZ) > 0.01)
			{
				float desiredYaw = (float)Math.Atan2(base.Pos.X - base.Pos.X, base.Pos.Z - base.Pos.Z);
				float yawDist = GameMath.AngleRadDistance(base.Pos.Yaw, desiredYaw);
				base.Pos.Yaw += GameMath.Clamp(yawDist, -35f * dt, 35f * dt);
				base.Pos.Yaw %= 6.2831855f;
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0001D394 File Offset: 0x0001B594
		private void SetAnimation(string animCode, float speed)
		{
			AnimationMetaData animMeta;
			if (!this.AnimManager.ActiveAnimationsByAnimCode.TryGetValue(animCode, ref animMeta))
			{
				animMeta = new AnimationMetaData
				{
					Code = animCode,
					Animation = animCode,
					AnimationSpeed = speed
				};
				this.AnimManager.ActiveAnimationsByAnimCode.Clear();
				this.AnimManager.ActiveAnimationsByAnimCode[animMeta.Animation] = animMeta;
				return;
			}
			animMeta.AnimationSpeed = speed;
			this.UpdateDebugAttributes();
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0001D408 File Offset: 0x0001B608
		public override void OnReceivedServerAnimations(int[] activeAnimations, int activeAnimationsCount, float[] activeAnimationSpeeds)
		{
			if (activeAnimationsCount == 0)
			{
				this.AnimManager.ActiveAnimationsByAnimCode.Clear();
				this.AnimManager.StartAnimation("fly");
			}
			string active = "";
			bool found = false;
			for (int i = 0; i < activeAnimationsCount; i++)
			{
				int crc32 = activeAnimations[i];
				for (int j = 0; j < base.Properties.Client.LoadedShape.Animations.Length; j++)
				{
					Animation anim = base.Properties.Client.LoadedShape.Animations[j];
					int mask = int.MaxValue;
					if (((ulong)anim.CodeCrc32 & (ulong)((long)mask)) == (ulong)((long)(crc32 & mask)))
					{
						if (this.AnimManager.ActiveAnimationsByAnimCode.ContainsKey(anim.Code))
						{
							break;
						}
						if (!(anim.Code == "glide") && !(anim.Code == "fly"))
						{
							string code = anim.Code ?? anim.Name.ToLowerInvariant();
							active = active + ", " + code;
							AnimationMetaData animmeta;
							base.Properties.Client.AnimationsByMetaCode.TryGetValue(code, ref animmeta);
							if (animmeta == null)
							{
								animmeta = new AnimationMetaData
								{
									Code = code,
									Animation = code,
									CodeCrc32 = anim.CodeCrc32
								};
							}
							animmeta.AnimationSpeed = activeAnimationSpeeds[i];
							this.AnimManager.ActiveAnimationsByAnimCode[anim.Code] = animmeta;
							found = true;
						}
					}
				}
			}
			if (!found)
			{
				(base.Properties.Client.Renderer as EntityShapeRenderer).AddRenderFlags = 0;
				return;
			}
			this.AnimManager.StopAnimation("fly");
			this.AnimManager.StopAnimation("glide");
			if (this.FeetInLiquid)
			{
				(base.Properties.Client.Renderer as EntityShapeRenderer).AddRenderFlags |= 4096;
				return;
			}
			(base.Properties.Client.Renderer as EntityShapeRenderer).AddRenderFlags &= -4097;
		}

		// Token: 0x04000119 RID: 281
		public double windMotion;

		// Token: 0x0400011A RID: 282
		private int cnt;
	}
}
