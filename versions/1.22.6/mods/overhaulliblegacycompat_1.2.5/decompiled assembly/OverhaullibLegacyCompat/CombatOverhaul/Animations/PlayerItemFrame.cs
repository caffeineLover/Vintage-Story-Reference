using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Integration.Transpilers;
using OpenTK.Mathematics;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200017C RID: 380
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct PlayerItemFrame
	{
		// Token: 0x060012D2 RID: 4818 RVA: 0x0005BDC5 File Offset: 0x00059FC5
		public PlayerItemFrame(PlayerFrame player, ItemFrame? item)
		{
			this.Player = player;
			this.Item = item;
			this.DetachedAnchor = player.DetachedAnchor;
			this.SwitchArms = player.SwitchArms;
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0005BDF0 File Offset: 0x00059FF0
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

		// Token: 0x060012D4 RID: 4820 RVA: 0x0005BE38 File Offset: 0x0005A038
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

		// Token: 0x060012D5 RID: 4821 RVA: 0x0005BE80 File Offset: 0x0005A080
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

		// Token: 0x04000926 RID: 2342
		public readonly PlayerFrame Player;

		// Token: 0x04000927 RID: 2343
		public readonly ItemFrame? Item;

		// Token: 0x04000928 RID: 2344
		public readonly bool DetachedAnchor;

		// Token: 0x04000929 RID: 2345
		public readonly bool SwitchArms;

		// Token: 0x0400092A RID: 2346
		public static readonly PlayerItemFrame Zero = new PlayerItemFrame(PlayerFrame.Zero, default(ItemFrame?));

		// Token: 0x0400092B RID: 2347
		public static readonly PlayerItemFrame Empty = new PlayerItemFrame(PlayerFrame.Empty, default(ItemFrame?));

		// Token: 0x0400092C RID: 2348
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
