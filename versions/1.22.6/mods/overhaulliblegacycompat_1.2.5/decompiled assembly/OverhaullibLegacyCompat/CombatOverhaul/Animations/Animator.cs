using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000173 RID: 371
	[NullableContext(1)]
	[Nullable(0)]
	public class Animator
	{
		// Token: 0x060012A9 RID: 4777 RVA: 0x0005A965 File Offset: 0x00058B65
		public Animator(Animation animation, [Nullable(2)] SoundsSynchronizerClient soundsManager, [Nullable(2)] ParticleEffectsManager particleEffectsManager, EntityPlayer player) : this(animation, soundsManager, particleEffectsManager, player, 1f)
		{
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0005A978 File Offset: 0x00058B78
		public Animator(Animation animation, [Nullable(2)] SoundsSynchronizerClient soundsManager, [Nullable(2)] ParticleEffectsManager particleEffectsManager, EntityPlayer player, float animationSpeed)
		{
			this._currentAnimation = animation;
			this._soundsManager = soundsManager;
			this._animationSpeed = animationSpeed;
			this._player = player;
			this._particleEffectsManager = particleEffectsManager;
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x0005A9DC File Offset: 0x00058BDC
		// (set) Token: 0x060012AC RID: 4780 RVA: 0x0005A9E4 File Offset: 0x00058BE4
		public bool FinishOverride { get; set; }

		// Token: 0x060012AD RID: 4781 RVA: 0x0005A9ED File Offset: 0x00058BED
		public void Play(Animation animation, TimeSpan duration)
		{
			this.Play(animation, (float)(animation.TotalDuration / duration));
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0005AA04 File Offset: 0x00058C04
		public void Play(Animation animation, float animationSpeed)
		{
			this._currentAnimation = animation;
			this._animationSpeed = animationSpeed;
			this._currentDuration = TimeSpan.Zero;
			this._previousAnimationFrame = this._lastFrame;
			this._unfiredCallbacks.Clear();
			this._unfiredCallbacks.AddRange(Enumerable.Select<CallbackFrame, string>(Enumerable.OrderBy<CallbackFrame, float>(animation.CallbackFrames, (CallbackFrame frame) => frame.DurationFraction), (CallbackFrame frame) => frame.Code));
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0005AA9C File Offset: 0x00058C9C
		public PlayerItemFrame Animate(TimeSpan delta, List<string> callbacks)
		{
			TimeSpan previousDuration = this._currentDuration * (double)this._animationSpeed;
			this._currentDuration += delta;
			TimeSpan adjustedDuration = this._currentDuration * (double)this._animationSpeed;
			if (this._soundsManager != null)
			{
				this._currentAnimation.PlaySounds(this._soundsManager, previousDuration, adjustedDuration, this._animationSpeed);
			}
			if (this._particleEffectsManager != null)
			{
				this._currentAnimation.SpawnParticles(this._player, this._particleEffectsManager, previousDuration, adjustedDuration);
			}
			callbacks.Clear();
			this._currentAnimation.GetCallbacks(previousDuration, adjustedDuration, callbacks);
			foreach (string callback in callbacks)
			{
				this._unfiredCallbacks.Remove(callback);
			}
			this._lastFrame = this._currentAnimation.Interpolate(this._previousAnimationFrame, adjustedDuration);
			return this._lastFrame;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0005AB9C File Offset: 0x00058D9C
		public bool Stopped()
		{
			return this._currentAnimation.TotalDuration <= this._currentDuration * (double)this._animationSpeed;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0005ABC0 File Offset: 0x00058DC0
		public bool Finished()
		{
			return this.FinishOverride || (this.Stopped() && !this._currentAnimation.Hold);
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0005ABE4 File Offset: 0x00058DE4
		public IEnumerable<string> GetUnfiredCallbacks()
		{
			return this._unfiredCallbacks;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0005ABEC File Offset: 0x00058DEC
		public void ClearUnfiredCallbacks()
		{
			this._unfiredCallbacks.Clear();
		}

		// Token: 0x040008BB RID: 2235
		private PlayerItemFrame _previousAnimationFrame = PlayerItemFrame.Zero;

		// Token: 0x040008BC RID: 2236
		private PlayerItemFrame _lastFrame = PlayerItemFrame.Zero;

		// Token: 0x040008BD RID: 2237
		private TimeSpan _currentDuration = TimeSpan.Zero;

		// Token: 0x040008BE RID: 2238
		private float _animationSpeed;

		// Token: 0x040008BF RID: 2239
		private Animation _currentAnimation;

		// Token: 0x040008C0 RID: 2240
		[Nullable(2)]
		private readonly SoundsSynchronizerClient _soundsManager;

		// Token: 0x040008C1 RID: 2241
		[Nullable(2)]
		private readonly ParticleEffectsManager _particleEffectsManager;

		// Token: 0x040008C2 RID: 2242
		private readonly EntityPlayer _player;

		// Token: 0x040008C3 RID: 2243
		private readonly List<string> _unfiredCallbacks = new List<string>();
	}
}
