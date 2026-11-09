using System;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace Jaunt.Behaviors
{
	// Token: 0x0200000E RID: 14
	public class EntityBehaviorJauntAiControl : EntityBehavior, IMountableListener
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00003FCE File Offset: 0x000021CE
		public EntityBehaviorJauntAiControl(Entity entity) : base(entity)
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003FE5 File Offset: 0x000021E5
		public override string PropertyName()
		{
			return "jaunt:aicontrol";
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003FEC File Offset: 0x000021EC
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			this.hitchingEntityCode = (attributes["hitchingEntityCode"].AsString(null) ?? "hitchingpost");
			this.canWanderIndoors = attributes["wanderIndoors"].AsBool(false);
			base.Initialize(properties, attributes);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004038 File Offset: 0x00002238
		public override void AfterInitialized(bool onFirstSpawn)
		{
			base.AfterInitialized(onFirstSpawn);
			if (this.entity.World.Side == 1)
			{
				this.entity.GetBehavior<EntityBehaviorTaskAI>().TaskManager.OnShouldExecuteTask += new ActionBoolReturn<IAiTask>(this.TaskManager_OnShouldExecuteTask);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004078 File Offset: 0x00002278
		public override void OnGameTick(float dt)
		{
			this.checkSkyTimer += dt;
			float cssi = this.canseesky ? 5f : 30f;
			if (this.checkSkyTimer >= cssi)
			{
				this.checkSkyTimer -= cssi;
				this.CheckSky();
			}
			this.checkPostTimer += dt;
			float hpnt = this.hitchingPostNear ? 30f : 5f;
			if (this.checkPostTimer >= hpnt)
			{
				this.checkPostTimer -= hpnt;
				this.CheckPost();
			}
			base.OnGameTick(dt);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000410C File Offset: 0x0000230C
		private void CheckSky()
		{
			this.canseesky = (this.entity.Pos.Y >= (double)this.entity.World.BlockAccessor.GetRainMapHeightAt((int)this.entity.Pos.X, (int)this.entity.Pos.Z));
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000416C File Offset: 0x0000236C
		private void CheckPost()
		{
			bool flag = this.isnear;
			this.hitchingPostNear = Enumerable.Any<Entity>(this.entity.World.GetEntitiesAround(this.entity.Pos.XYZ, 10f, 10f, null), delegate(Entity e)
			{
				AssetLocation code = e.Code;
				return ((code != null) ? code.FirstCodePart() : null) == this.hitchingEntityCode;
			});
			this.isnear = this.hitchingPostNear;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000041D0 File Offset: 0x000023D0
		public void MountEasterEgg(EntityAgent entityAgent)
		{
			ItemSlot rightHandItemSlot = entityAgent.RightHandItemSlot;
			string text;
			if (rightHandItemSlot == null)
			{
				text = null;
			}
			else
			{
				ItemStack itemstack = rightHandItemSlot.Itemstack;
				if (itemstack == null)
				{
					text = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					if (collectible == null)
					{
						text = null;
					}
					else
					{
						AssetLocation code = collectible.Code;
						text = ((code != null) ? code.FirstCodePart() : null);
					}
				}
			}
			string tool = text;
			if (tool == null)
			{
				return;
			}
			if (tool == "joustinglance")
			{
				this.entity.StartAnimation("jump");
				return;
			}
			if (tool == "lantern")
			{
				this.entity.StartAnimation("jump");
				return;
			}
			if (!(tool == "flute"))
			{
				return;
			}
			this.entity.StartAnimation("Eat");
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004272 File Offset: 0x00002472
		void IMountableListener.DidMount(EntityAgent entityAgent)
		{
			this.StopAiTasks();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000427A File Offset: 0x0000247A
		void IMountableListener.DidUnmount(EntityAgent entityAgent)
		{
			this.CheckSky();
			this.CheckPost();
			this.StopAiTasks();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004290 File Offset: 0x00002490
		private bool TaskManager_OnShouldExecuteTask(IAiTask t)
		{
			if (t is AiTaskWander)
			{
				if (this.hitchingPostNear)
				{
					return false;
				}
				if (!this.canseesky && !this.canWanderIndoors)
				{
					return false;
				}
				if (this.RecentlyDismounted(24))
				{
					return false;
				}
			}
			if (t is AiTaskSeekFoodAndEat)
			{
				if (this.hitchingPostNear)
				{
					return false;
				}
				if (this.RecentlyDismounted(4))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000042EB File Offset: 0x000024EB
		public bool RecentlyDismounted(int hours)
		{
			return this.entity.World.Calendar.TotalHours - this.entity.WatchedAttributes.GetDouble("lastDismountTotalHours", 0.0) < (double)hours;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004328 File Offset: 0x00002528
		private void StopAiTasks()
		{
			EntityBehaviorTaskAI behavior = this.entity.GetBehavior<EntityBehaviorTaskAI>();
			AiTaskManager tm = (behavior != null) ? behavior.TaskManager : null;
			if (tm == null)
			{
				return;
			}
			tm.StopTasks();
		}

		// Token: 0x04000049 RID: 73
		private bool canWanderIndoors;

		// Token: 0x0400004A RID: 74
		public bool hitchingPostNear = true;

		// Token: 0x0400004B RID: 75
		public bool canseesky = true;

		// Token: 0x0400004C RID: 76
		public float checkPostTimer;

		// Token: 0x0400004D RID: 77
		public float checkSkyTimer;

		// Token: 0x0400004E RID: 78
		private string hitchingEntityCode;

		// Token: 0x0400004F RID: 79
		private bool isnear;
	}
}
