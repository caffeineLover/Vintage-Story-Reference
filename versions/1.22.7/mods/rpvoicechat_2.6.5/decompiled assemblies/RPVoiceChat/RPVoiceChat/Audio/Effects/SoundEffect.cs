using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x02000111 RID: 273
	public abstract class SoundEffect
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x000373A1 File Offset: 0x000355A1
		public string Name { get; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x000373A9 File Offset: 0x000355A9
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x000373B1 File Offset: 0x000355B1
		public bool IsEnabled { get; set; }

		// Token: 0x06000A03 RID: 2563 RVA: 0x000373BA File Offset: 0x000355BA
		protected SoundEffect(string name, int source)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
			this.source = source;
			this.nullEffect = ALC.EFX.GenEffect();
			this.effect = this.GenerateEffect();
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x000373F8 File Offset: 0x000355F8
		public virtual void Apply()
		{
			ALDevice device = ALC.GetContextsDevice(ALC.GetCurrentContext());
			if (device == IntPtr.Zero || !ALC.IsExtensionPresent(device, "ALC_EXT_EFX"))
			{
				Logger.client.Debug("[SoundEffect] EFX not supported, effect ignored.");
				return;
			}
			if (this.IsEnabled)
			{
				return;
			}
			ALC.EFX.Source(this.source, 131078, this.slot, 0, 0);
			this.IsEnabled = true;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00037462 File Offset: 0x00035662
		public virtual void Clear()
		{
			if (!this.IsEnabled)
			{
				return;
			}
			ALC.EFX.Source(this.source, 131078, 0, 0, 0);
			this.IsEnabled = false;
		}

		// Token: 0x06000A06 RID: 2566
		protected abstract int GenerateEffect();

		// Token: 0x06000A07 RID: 2567 RVA: 0x00037488 File Offset: 0x00035688
		public static SoundEffect Create(string name, int source)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			if (source <= 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(62, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[RPVoiceChat] Cannot create SoundEffect \"");
				defaultInterpolatedStringHandler.AppendFormatted(name);
				defaultInterpolatedStringHandler.AppendLiteral("\": invalid source ID ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(source);
				Console.WriteLine(defaultInterpolatedStringHandler.ToStringAndClear());
				return null;
			}
			string key = name.ToLowerInvariant();
			Func<int, SoundEffect> factory;
			if (SoundEffect.registry.TryGetValue(key, ref factory))
			{
				try
				{
					return factory.Invoke(source);
				}
				catch (Exception e)
				{
					Console.WriteLine("[RPVoiceChat] Error creating SoundEffect \"" + name + "\": " + e.Message);
					return null;
				}
			}
			Console.WriteLine("[RPVoiceChat] SoundEffect \"" + name + "\" not found in registry.");
			return null;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00037550 File Offset: 0x00035750
		public static void Register(string name, Func<int, SoundEffect> factory)
		{
			if (string.IsNullOrWhiteSpace(name) || factory == null)
			{
				return;
			}
			string key = name.ToLowerInvariant();
			if (SoundEffect.registry.ContainsKey(key))
			{
				Console.WriteLine("[RPVoiceChat] Warning: SoundEffect \"" + name + "\" is already registered. It will be overwritten.");
			}
			SoundEffect.registry[key] = factory;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x000375A0 File Offset: 0x000357A0
		// Note: this type is marked as 'beforefieldinit'.
		static SoundEffect()
		{
			Dictionary<string, Func<int, SoundEffect>> dictionary = new Dictionary<string, Func<int, SoundEffect>>();
			dictionary.Add("cheapmic", (int source) => new CheapMicEffect(source));
			dictionary.Add("reverb", (int source) => new ReverbEffect(source));
			dictionary.Add("intoxicated", (int source) => new IntoxicatedEffect(source));
			SoundEffect.registry = dictionary;
		}

		// Token: 0x04000488 RID: 1160
		protected int source;

		// Token: 0x04000489 RID: 1161
		protected int effect;

		// Token: 0x0400048A RID: 1162
		protected int slot;

		// Token: 0x0400048B RID: 1163
		private int nullEffect;

		// Token: 0x0400048D RID: 1165
		private static readonly Dictionary<string, Func<int, SoundEffect>> registry;
	}
}
