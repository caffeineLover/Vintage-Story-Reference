using System;
using RPVoiceChat.GameContent.BlockEntityBehavior;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000E0 RID: 224
	public class BlockEntityCarillonBell : BlockEntity
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0002414F File Offset: 0x0002234F
		private BEBehaviorAnimatable VanillaAnimatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x00024157 File Offset: 0x00022357
		private BlockEntityAnimationUtil AnimUtil
		{
			get
			{
				BEBehaviorAnimatable vanillaAnimatable = this.VanillaAnimatable;
				if (vanillaAnimatable == null)
				{
					return null;
				}
				return vanillaAnimatable.animUtil;
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00024179 File Offset: 0x00022379
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this._shapePath = this.ResolveShapePath();
			if (api.Side == 2)
			{
				this.InitializeClientAnimator();
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000241A0 File Offset: 0x000223A0
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this._ringSequence = tree.GetInt("rpvc:ringSequence", 0);
			this._shapePath = this.ResolveShapePath();
			if (worldForResolving.Side != 2)
			{
				return;
			}
			this.InitializeClientAnimator();
			if (this._clientLastRingSequence < 0)
			{
				this._clientLastRingSequence = this._ringSequence;
				return;
			}
			if (this._ringSequence != this._clientLastRingSequence && this.UsesRopeShape())
			{
				this.PlaySingleShotAnimation("swing");
				this.PlaySingleShotAnimation("clapper");
			}
			this._clientLastRingSequence = this._ringSequence;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00024231 File Offset: 0x00022431
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetInt("rpvc:ringSequence", this._ringSequence);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0002424C File Offset: 0x0002244C
		public void OnRung()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return;
			}
			BEBehaviorSoundable behavior = base.GetBehavior<BEBehaviorSoundable>();
			if (behavior != null)
			{
				behavior.OnRung();
			}
			Block block = base.Block;
			string v;
			if (((block != null) ? block.Variant : null) != null && base.Block.Variant.TryGetValue("v", ref v) && string.Equals(v, "down", 5))
			{
				this._ringSequence++;
				this.MarkDirty(true, null);
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000242DC File Offset: 0x000224DC
		private void InitializeClientAnimator()
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil != null && animUtil.animator == null)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 2)
				{
					Block block = base.Block;
					if (((block != null) ? block.Code : null) != null && !string.IsNullOrWhiteSpace(this._shapePath))
					{
						AssetLocation assetLoc = new AssetLocation(base.Block.Code.Domain, "shapes/" + this._shapePath + ".json");
						Shape shape = Shape.TryGet(this.Api, assetLoc);
						if (((shape != null) ? shape.Animations : null) != null && shape.Animations.Length != 0)
						{
							shape.InitForAnimations(this.Api.Logger, this._shapePath, Array.Empty<string>());
						}
					}
					float rotYDeg = this.GetBlockSideRotY();
					animUtil.InitializeAnimator(this._shapePath, null, null, new Vec3f(0f, rotYDeg, 0f));
					return;
				}
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x000243D0 File Offset: 0x000225D0
		private void PlaySingleShotAnimation(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			if (animUtil.activeAnimationsByAnimCode.ContainsKey(animationCode))
			{
				animUtil.StopAnimation(animationCode);
			}
			animUtil.StartAnimation(new AnimationMetaData
			{
				Animation = animationCode,
				Code = animationCode
			});
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00024418 File Offset: 0x00022618
		private float GetBlockSideRotY()
		{
			Block block = base.Block;
			bool flag;
			string side;
			if (block == null)
			{
				flag = false;
			}
			else
			{
				RelaxedReadOnlyDictionary<string, string> variant = block.Variant;
				flag = ((variant != null) ? new bool?(variant.TryGetValue("side", ref side)) : default(bool?)).GetValueOrDefault();
			}
			float result;
			if (flag)
			{
				float num;
				if (!(side == "north"))
				{
					if (!(side == "east"))
					{
						if (!(side == "west"))
						{
							if (!(side == "south"))
							{
								num = 0f;
							}
							else
							{
								num = 180f;
							}
						}
						else
						{
							num = 90f;
						}
					}
					else
					{
						num = 270f;
					}
				}
				else
				{
					num = 0f;
				}
				result = num;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x000244C8 File Offset: 0x000226C8
		private bool UsesRopeShape()
		{
			Block block = base.Block;
			string v;
			return ((block != null) ? block.Variant : null) != null && base.Block.Variant.TryGetValue("v", ref v) && string.Equals(v, "down", 5);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00024510 File Offset: 0x00022710
		private string ResolveShapePath()
		{
			Block block = base.Block;
			string text;
			if (block == null)
			{
				text = null;
			}
			else
			{
				CompositeShape shape = block.Shape;
				if (shape == null)
				{
					text = null;
				}
				else
				{
					AssetLocation @base = shape.Base;
					text = ((@base != null) ? @base.Path : null);
				}
			}
			return text ?? "block/carillonbell/carillonbell_rope";
		}

		// Token: 0x04000333 RID: 819
		private const string SwingAnimationCode = "swing";

		// Token: 0x04000334 RID: 820
		private const string ClapperAnimationCode = "clapper";

		// Token: 0x04000335 RID: 821
		private string _shapePath;

		// Token: 0x04000336 RID: 822
		private int _ringSequence;

		// Token: 0x04000337 RID: 823
		private int _clientLastRingSequence = -1;
	}
}
