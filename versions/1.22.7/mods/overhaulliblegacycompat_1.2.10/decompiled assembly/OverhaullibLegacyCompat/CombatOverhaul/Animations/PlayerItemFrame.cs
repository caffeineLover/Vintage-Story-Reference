using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Integration.Transpilers;
using OpenTK.Mathematics;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000189 RID: 393
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct PlayerItemFrame
	{
		// Token: 0x06001349 RID: 4937 RVA: 0x0005D819 File Offset: 0x0005BA19
		public PlayerItemFrame(PlayerFrame player, ItemFrame? item)
		{
			this.Player = player;
			this.Item = item;
			this.DetachedAnchor = player.DetachedAnchor;
			this.SwitchArms = player.SwitchArms;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0005D844 File Offset: 0x0005BA44
		public void Apply(ElementPose pose, EnumAnimatedElement element, Vector3 eyePosition, float eyeHeight, float cameraPitch = 0f, bool applyCameraPitch = false, bool overrideTorso = true)
		{
			if (element != EnumAnimatedElement.Unknown)
			{
				this.Player.Apply(pose, element, eyePosition, eyeHeight, cameraPitch, applyCameraPitch, overrideTorso);
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			this.Item.GetValueOrDefault().Apply(pose);
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0005D88C File Offset: 0x0005BA8C
		public void Apply(ExtendedElementPose pose, EnumAnimatedElement element, Vector3 eyePosition, float eyeHeight, float cameraPitch = 0f, bool applyCameraPitch = false, bool overrideTorso = true)
		{
			if (element != EnumAnimatedElement.Unknown)
			{
				this.Player.Apply(pose, element, eyePosition, eyeHeight, cameraPitch, applyCameraPitch, overrideTorso);
				return;
			}
			if (this.Item == null)
			{
				return;
			}
			this.Item.GetValueOrDefault().Apply(pose);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0005D8D4 File Offset: 0x0005BAD4
		public static PlayerItemFrame Compose([TupleElementNames(new string[]
		{
			"element",
			"weight"
		})] [Nullable(new byte[]
		{
			1,
			0
		})] IEnumerable<ValueTuple<PlayerItemFrame, float>> frames)
		{
			IReadOnlyList<ValueTuple<PlayerItemFrame, float>> readOnlyList = (frames as IReadOnlyList<ValueTuple<PlayerItemFrame, float>>) ?? Enumerable.ToArray<ValueTuple<PlayerItemFrame, float>>(frames);
			PlayerFrame player = PlayerFrame.Compose(readOnlyList);
			List<ValueTuple<ItemFrame, float>> list;
			if ((list = PlayerItemFrame._itemFrameComposeScratch) == null)
			{
				list = (PlayerItemFrame._itemFrameComposeScratch = new List<ValueTuple<ItemFrame, float>>());
			}
			List<ValueTuple<ItemFrame, float>> itemFrames = list;
			itemFrames.Clear();
			foreach (ValueTuple<PlayerItemFrame, float> valueTuple in readOnlyList)
			{
				PlayerItemFrame element = valueTuple.Item1;
				float weight = valueTuple.Item2;
				if (element.Item != null)
				{
					itemFrames.Add(new ValueTuple<ItemFrame, float>(element.Item.Value, weight));
				}
			}
			ItemFrame item = ItemFrame.Compose(itemFrames);
			return new PlayerItemFrame(player, new ItemFrame?(item));
		}

		// Token: 0x0400094E RID: 2382
		public readonly PlayerFrame Player;

		// Token: 0x0400094F RID: 2383
		public readonly ItemFrame? Item;

		// Token: 0x04000950 RID: 2384
		public readonly bool DetachedAnchor;

		// Token: 0x04000951 RID: 2385
		public readonly bool SwitchArms;

		// Token: 0x04000952 RID: 2386
		public static readonly PlayerItemFrame Zero = new PlayerItemFrame(PlayerFrame.Zero, default(ItemFrame?));

		// Token: 0x04000953 RID: 2387
		public static readonly PlayerItemFrame Empty = new PlayerItemFrame(PlayerFrame.Empty, default(ItemFrame?));

		// Token: 0x04000954 RID: 2388
		[TupleElementNames(new string[]
		{
			"element",
			"weight"
		})]
		[Nullable(new byte[]
		{
			2,
			0
		})]
		[ThreadStatic]
		private static List<ValueTuple<ItemFrame, float>> _itemFrameComposeScratch;
	}
}
