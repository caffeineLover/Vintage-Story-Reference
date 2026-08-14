using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200002F RID: 47
	public class BehaviorInTreeHollowTransform : CollectibleBehavior
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0001A7E3 File Offset: 0x000189E3
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0001A7EB File Offset: 0x000189EB
		public ModelTransform Transform { get; set; } = ModelTransform.NoTransform;

		// Token: 0x06000238 RID: 568 RVA: 0x0001A7F4 File Offset: 0x000189F4
		public BehaviorInTreeHollowTransform(CollectibleObject collectibleObject) : base(collectibleObject)
		{
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0001A808 File Offset: 0x00018A08
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
