using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000033 RID: 51
	public class EntityBehaviorCarryable : EntityBehavior
	{
		// Token: 0x06000247 RID: 583 RVA: 0x0001B6AC File Offset: 0x000198AC
		public EntityBehaviorCarryable(Entity entity) : base(entity)
		{
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0001B6B5 File Offset: 0x000198B5
		public override void Initialize(EntityProperties properties, JsonObject typeAttributes)
		{
			base.Initialize(properties, typeAttributes);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0001B6BF File Offset: 0x000198BF
		public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player, ref EnumHandling handled)
		{
			this.interactions = ObjectCacheUtil.GetOrCreate<WorldInteraction[]>(world.Api, "carryableEntityInteractions", () => new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-behavior-rightclickpickup",
					MouseButton = 2
				}
			});
			return this.interactions;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0001B6FC File Offset: 0x000198FC
		public override void GetInfoText(StringBuilder infotext)
		{
			infotext.AppendLine(Lang.Get("primitivesurvival:creature-carryable", Array.Empty<object>()));
			base.GetInfoText(infotext);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0001B71B File Offset: 0x0001991B
		public override string PropertyName()
		{
			return "carryable";
		}

		// Token: 0x04000102 RID: 258
		private WorldInteraction[] interactions;
	}
}
