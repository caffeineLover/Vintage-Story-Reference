using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace PetAI
{
	// Token: 0x02000013 RID: 19
	public class EntityBehaviorReceiveCommand : EntityBehavior
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00003B41 File Offset: 0x00001D41
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00003B5F File Offset: 0x00001D5F
		public string SimpleCommand
		{
			get
			{
				if (this.IsEntityObedient(new Command(EnumCommandType.SIMPLE, this._simpleCommand)))
				{
					return this._simpleCommand;
				}
				return null;
			}
			private set
			{
				this._simpleCommand = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00003B68 File Offset: 0x00001D68
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00003B9E File Offset: 0x00001D9E
		public string ComplexCommand
		{
			get
			{
				string commandName = this.entity.WatchedAttributes.GetString("activeCommand", null);
				if (this.IsEntityObedient(new Command(EnumCommandType.COMPLEX, commandName)))
				{
					return commandName;
				}
				return null;
			}
			private set
			{
				this.entity.WatchedAttributes.SetString("activeCommand", value);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003BB8 File Offset: 0x00001DB8
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00003C0C File Offset: 0x00001E0C
		public EnumAggressionLevel AggressionLevel
		{
			get
			{
				if (this.entity.GetBehavior<EntityBehaviorTameable>().DomesticationLevel == DomesticationLevel.WILD)
				{
					return EnumAggressionLevel.AGGRESSIVE;
				}
				string commandName = this.entity.WatchedAttributes.GetString("aggressionLevel", null);
				EnumAggressionLevel level;
				if (Enum.TryParse<EnumAggressionLevel>(commandName, ref level) && this.IsEntityObedient(new Command(EnumCommandType.AGGRESSIONLEVEL, commandName)))
				{
					return level;
				}
				return EnumAggressionLevel.NEUTRAL;
			}
			private set
			{
				this.entity.WatchedAttributes.SetString("aggressionLevel", value.ToString());
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003C30 File Offset: 0x00001E30
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00003C38 File Offset: 0x00001E38
		public Dictionary<Command, float> AvailableCommands { get; private set; } = new Dictionary<Command, float>();

		// Token: 0x06000054 RID: 84 RVA: 0x00003C41 File Offset: 0x00001E41
		public EntityBehaviorReceiveCommand(Entity entity) : base(entity)
		{
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003C58 File Offset: 0x00001E58
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			base.Initialize(properties, attributes);
			JsonObject jsonObject = attributes["availableCommands"];
			foreach (JsonObject jsonObject2 in ((jsonObject != null) ? jsonObject.AsArray() : null) ?? Array.Empty<JsonObject>())
			{
				string commandName = jsonObject2["commandName"].AsString(null);
				EnumCommandType type;
				Enum.TryParse<EnumCommandType>(jsonObject2["commandType"].AsString(null), ref type);
				float minObedience = jsonObject2["minObedience"].AsFloat(1f);
				this.AvailableCommands.Add(new Command(type, commandName), minObedience);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003CF4 File Offset: 0x00001EF4
		public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled)
		{
			base.OnInteract(byEntity, itemslot, hitPosition, mode, ref handled);
			EntityBehaviorTameable behavior = this.entity.GetBehavior<EntityBehaviorTameable>();
			if (behavior == null || behavior.DomesticationLevel > DomesticationLevel.WILD)
			{
				EntityPlayer player = byEntity as EntityPlayer;
				if (player != null)
				{
					string playerUID = player.PlayerUID;
					EntityBehaviorTameable behavior2 = this.entity.GetBehavior<EntityBehaviorTameable>();
					string text;
					if (behavior2 == null)
					{
						text = null;
					}
					else
					{
						IPlayer cachedOwner = behavior2.CachedOwner;
						text = ((cachedOwner != null) ? cachedOwner.PlayerUID : null);
					}
					if (playerUID == text && byEntity.Controls.Sneak && mode == 1 && this.entity.Api.Side == 2)
					{
						if (this.gui == null)
						{
							this.gui = new TaskSelectionGui(this.entity.Api as ICoreClientAPI, player, this.entity as EntityAgent);
						}
						else
						{
							this.gui.ComposeGui();
						}
						this.gui.TryOpen();
					}
				}
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003DD8 File Offset: 0x00001FD8
		public void SetCommand(Command command, EntityPlayer byPlayer)
		{
			if (command == null)
			{
				return;
			}
			if (byPlayer != null)
			{
				EntityBehaviorTameable behavior = this.entity.GetBehavior<EntityBehaviorTameable>();
				if (((behavior != null) ? behavior.CachedOwner : null) != null && !(this.entity.GetBehavior<EntityBehaviorTameable>().CachedOwner.PlayerUID == byPlayer.PlayerUID))
				{
					return;
				}
			}
			if (command.Type == EnumCommandType.COMPLEX)
			{
				this.ComplexCommand = command.CommandName;
				ITreeAttribute location = new TreeAttribute();
				location.SetDouble("x", this.entity.Pos.X);
				location.SetDouble("y", this.entity.Pos.Y);
				location.SetDouble("z", this.entity.Pos.Z);
				this.entity.WatchedAttributes.SetAttribute("staylocation", location);
			}
			if (command.Type == EnumCommandType.SIMPLE)
			{
				this.SimpleCommand = command.CommandName;
			}
			EnumAggressionLevel level;
			if (command.Type == EnumCommandType.AGGRESSIONLEVEL && Enum.TryParse<EnumAggressionLevel>(command.CommandName, ref level))
			{
				this.AggressionLevel = level;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003EDF File Offset: 0x000020DF
		public override string PropertyName()
		{
			return "receivecommand";
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003EE8 File Offset: 0x000020E8
		private bool IsEntityObedient(Command command)
		{
			return command == null || command.CommandName == null || !this.entity.HasBehavior<EntityBehaviorTameable>() || !this.AvailableCommands.ContainsKey(command) || this.entity.GetBehavior<EntityBehaviorTameable>().Obedience >= this.AvailableCommands[command];
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003F40 File Offset: 0x00002140
		public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player, ref EnumHandling handled)
		{
			if (this.entity.Alive)
			{
				EntityBehaviorTameable behavior = this.entity.GetBehavior<EntityBehaviorTameable>();
				if (((behavior != null) ? behavior.OwnerId : null) == player.PlayerUID)
				{
					return new WorldInteraction[]
					{
						new WorldInteraction
						{
							ActionLangCode = "petai:interact-command",
							HotKeyCode = "sneak",
							MouseButton = 2
						}
					};
				}
			}
			return Array.Empty<WorldInteraction>();
		}

		// Token: 0x04000034 RID: 52
		private string _simpleCommand;

		// Token: 0x04000035 RID: 53
		protected TaskSelectionGui gui;
	}
}
