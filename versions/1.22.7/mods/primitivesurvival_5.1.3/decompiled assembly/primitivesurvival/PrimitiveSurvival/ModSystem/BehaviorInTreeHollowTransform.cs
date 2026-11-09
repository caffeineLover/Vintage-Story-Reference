using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200002F RID: 47
	public class BehaviorInTreeHollowTransform : CollectibleBehavior
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0001A7AF File Offset: 0x000189AF
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0001A7B7 File Offset: 0x000189B7
		public ModelTransform Transform { get; set; } = ModelTransform.NoTransform;

		// Token: 0x06000238 RID: 568 RVA: 0x0001A7C0 File Offset: 0x000189C0
		public BehaviorInTreeHollowTransform(CollectibleObject collectibleObject) : base(collectibleObject)
		{
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0001A7D4 File Offset: 0x000189D4
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			ModelTransform transform = properties.AsObject<ModelTransform>(null);
			if (transform != null)
			{
				this.Transform = transform;
			}
		}
	}
}
