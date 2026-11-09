using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace PetAI
{
	// Token: 0x0200000D RID: 13
	public class EntityBehaviorGiveCommand : EntityBehavior
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00003768 File Offset: 0x00001968
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000037BE File Offset: 0x000019BE
		public Entity Victim
		{
			get
			{
				if (this._victim == null || !this._victim.Alive || !this._victim.IsInteractable || this._victim.EntityId == this.entity.EntityId)
				{
					this._victim = null;
					return null;
				}
				return this._victim;
			}
			set
			{
				this._victim = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000037C8 File Offset: 0x000019C8
		// (set) Token: 0x06000038 RID: 56 RVA: 0x0000381E File Offset: 0x00001A1E
		public Entity Attacker
		{
			get
			{
				if (this._attacker == null || !this._attacker.Alive || !this._attacker.IsInteractable || this._attacker.EntityId == this.entity.EntityId)
				{
					this._attacker = null;
					return null;
				}
				return this._attacker;
			}
			set
			{
				this._attacker = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00003828 File Offset: 0x00001A28
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00003888 File Offset: 0x00001A88
		public Command ActiveCommand
		{
			get
			{
				ITreeAttribute treeAttribute = this.entity.WatchedAttributes.GetTreeAttribute("activeCommand");
				string commandName = (treeAttribute != null) ? treeAttribute.GetString("commandName", null) : null;
				EnumCommandType type;
				if (Enum.TryParse<EnumCommandType>((treeAttribute != null) ? treeAttribute.GetString("type", null) : null, ref type) && commandName != null)
				{
					return new Command(type, commandName);
				}
				return null;
			}
			set
			{
				ITreeAttribute command = new TreeAttribute();
				command.SetString("commandName", value.CommandName);
				command.SetString("type", value.Type.ToString());
				this.entity.WatchedAttributes.SetAttribute("activeCommand", command);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000038E1 File Offset: 0x00001AE1
		public EntityBehaviorGiveCommand(Entity entity) : base(entity)
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000038EA File Offset: 0x00001AEA
		public override void DidAttack(DamageSource source, EntityAgent targetEntity, ref EnumHandling handled)
		{
			base.DidAttack(source, targetEntity, ref handled);
			this.Victim = targetEntity;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000038FC File Offset: 0x00001AFC
		public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
		{
			base.OnEntityReceiveDamage(damageSource, ref damage);
			this.Attacker = damageSource.GetCauseEntity();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003912 File Offset: 0x00001B12
		public override string PropertyName()
		{
			return "givecommand";
		}

		// Token: 0x04000025 RID: 37
		private Entity _victim;

		// Token: 0x04000026 RID: 38
		private Entity _attacker;
	}
}
