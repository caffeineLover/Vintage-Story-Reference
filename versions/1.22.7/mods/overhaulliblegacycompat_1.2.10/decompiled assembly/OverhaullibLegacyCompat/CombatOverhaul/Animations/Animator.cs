using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000180 RID: 384
	[NullableContext(1)]
	[Nullable(0)]
	public class Animator
	{
		// Token: 0x06001320 RID: 4896 RVA: 0x0005C3BA File Offset: 0x0005A5BA
		public Animator(Animation animation, [Nullable(2)] SoundsSynchronizerClient soundsManager, [Nullable(2)] ParticleEffectsManager particleEffectsManager, EntityPlayer player) : this(animation, soundsManager, particleEffectsManager, player, 1f)
		{
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0005C3CC File Offset: 0x0005A5CC
		public Animator(Animation animation, [Nullable(2)] SoundsSynchronizerClient soundsManager, [Nullable(2)] ParticleEffectsManager particleEffectsManager, EntityPlayer player, float animationSpeed)
		{
			this._currentAnimation = animation;
			this._soundsManager = soundsManager;
			this._animationSpeed = animationSpeed;
			this._player = player;
			this._particleEffectsManager = particleEffectsManager;
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001322 RID: 4898 RVA: 0x0005C430 File Offset: 0x0005A630
		// (set) Token: 0x06001323 RID: 4899 RVA: 0x0005C438 File Offset: 0x0005A638
		public bool FinishOverride { get; set; }

		// Token: 0x06001324 RID: 4900 RVA: 0x0005C441 File Offset: 0x0005A641
		public void Play(Animation animation, TimeSpan duration)
		{
			this.Play(animation, (float)(animation.TotalDuration / duration));
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0005C458 File Offset: 0x0005A658
		public void Play(Animation animation, float animationSpeed)
		{
			this._currentAnimation = animation;
			this._animationSpeed = animationSpeed;
			this._currentDuration = TimeSpan.Zero;
			this._previousAnimationFrame = this._lastFrame;
			this._unfiredCallbacks.Clear();
			this._unfiredCallbacks.AddRange(Enumerable.Select<CallbackFrame, string>(Enumerable.OrderBy<CallbackFrame, float>(animation.CallbackFrames, (CallbackFrame frame) => frame.DurationFraction), (CallbackFrame frame) => frame.Code));
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0005C4F0 File Offset: 0x0005A6F0
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

		// Token: 0x06001327 RID: 4903 RVA: 0x0005C5F0 File Offset: 0x0005A7F0
		public bool Stopped()
		{
			return this._currentAnimation.TotalDuration <= this._currentDuration * (double)this._animationSpeed;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0005C614 File Offset: 0x0005A814
		public bool Finished()
		{
			return this.FinishOverride || (this.Stopped() && !this._currentAnimation.Hold);
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0005C638 File Offset: 0x0005A838
		public IEnumerable<string> GetUnfiredCallbacks()
		{
			return this._unfiredCallbacks;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0005C640 File Offset: 0x0005A840
		public void ClearUnfiredCallbacks()
		{
			this._unfiredCallbacks.Clear();
		}

		// Token: 0x040008E3 RID: 2275
		private PlayerItemFrame _previousAnimationFrame = PlayerItemFrame.Zero;

		// Token: 0x040008E4 RID: 2276
		private PlayerItemFrame _lastFrame = PlayerItemFrame.Zero;

		// Token: 0x040008E5 RID: 2277
		private TimeSpan _currentDuration = TimeSpan.Zero;

		// Token: 0x040008E6 RID: 2278
		private float _animationSpeed;

		// Token: 0x040008E7 RID: 2279
		private Animation _currentAnimation;

		// Token: 0x040008E8 RID: 2280
		[Nullable(2)]
		private readonly SoundsSynchronizerClient _soundsManager;

		// Token: 0x040008E9 RID: 2281
		[Nullable(2)]
		private readonly ParticleEffectsManager _particleEffectsManager;

		// Token: 0x040008EA RID: 2282
		private readonly EntityPlayer _player;

		// Token: 0x040008EB RID: 2283
		private readonly List<string> _unfiredCallbacks = new List<string>();
	}
}
