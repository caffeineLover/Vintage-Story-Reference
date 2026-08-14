using System;
using RPVoiceChat.GameContent.BlockEntityBehavior;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000AE RID: 174
	public class BlockEntityCarillonBell : BlockEntity
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001CB23 File Offset: 0x0001AD23
		private BEBehaviorAnimatable VanillaAnimatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0001CB2B File Offset: 0x0001AD2B
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

		// Token: 0x06000550 RID: 1360 RVA: 0x0001CB4D File Offset: 0x0001AD4D
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this._shapePath = this.ResolveShapePath();
			if (api.Side == 2)
			{
				this.InitializeClientAnimator();
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001CB74 File Offset: 0x0001AD74
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

		// Token: 0x06000552 RID: 1362 RVA: 0x0001CC05 File Offset: 0x0001AE05
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetInt("rpvc:ringSequence", this._ringSequence);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001CC20 File Offset: 0x0001AE20
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

		// Token: 0x06000554 RID: 1364 RVA: 0x0001CCB0 File Offset: 0x0001AEB0
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

		// Token: 0x06000555 RID: 1365 RVA: 0x0001CDA4 File Offset: 0x0001AFA4
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

		// Token: 0x06000556 RID: 1366 RVA: 0x0001CDEC File Offset: 0x0001AFEC
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

		// Token: 0x06000557 RID: 1367 RVA: 0x0001CE9C File Offset: 0x0001B09C
		private bool UsesRopeShape()
		{
			Block block = base.Block;
			string v;
			return ((block != null) ? block.Variant : null) != null && base.Block.Variant.TryGetValue("v", ref v) && string.Equals(v, "down", 5);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001CEE4 File Offset: 0x0001B0E4
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

		// Token: 0x04000286 RID: 646
		private const string SwingAnimationCode = "swing";

		// Token: 0x04000287 RID: 647
		private const string ClapperAnimationCode = "clapper";

		// Token: 0x04000288 RID: 648
		private string _shapePath;

		// Token: 0x04000289 RID: 649
		private int _ringSequence;

		// Token: 0x0400028A RID: 650
		private int _clientLastRingSequence = -1;
	}
}
