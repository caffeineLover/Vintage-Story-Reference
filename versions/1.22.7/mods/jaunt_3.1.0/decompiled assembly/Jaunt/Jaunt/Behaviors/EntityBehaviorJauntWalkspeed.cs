using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Jaunt.Behaviors
{
	// Token: 0x02000016 RID: 22
	public class EntityBehaviorJauntWalkspeed : EntityBehavior
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x0000621F File Offset: 0x0000441F
		public EntityBehaviorJauntWalkspeed(Entity entity) : base(entity)
		{
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006233 File Offset: 0x00004433
		public override string PropertyName()
		{
			return "jaunt:walkspeed";
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000623A File Offset: 0x0000443A
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			base.Initialize(properties, attributes);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006244 File Offset: 0x00004444
		public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled)
		{
			base.OnInteract(byEntity, itemslot, hitPosition, mode, ref handled);
			this.CheckAttachments();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000625C File Offset: 0x0000445C
		private Task CheckAttachments()
		{
			EntityBehaviorJauntWalkspeed.<CheckAttachments>d__6 <CheckAttachments>d__;
			<CheckAttachments>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CheckAttachments>d__.<>4__this = this;
			<CheckAttachments>d__.<>1__state = -1;
			<CheckAttachments>d__.<>t__builder.Start<EntityBehaviorJauntWalkspeed.<CheckAttachments>d__6>(ref <CheckAttachments>d__);
			return <CheckAttachments>d__.<>t__builder.Task;
		}

		// Token: 0x04000069 RID: 105
		public float ExtraSpeedModifier = 1f;

		// Token: 0x0400006A RID: 106
		public float speedmodifier;
	}
}
