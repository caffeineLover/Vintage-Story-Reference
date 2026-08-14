using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x020000D6 RID: 214
	public abstract class SoundEffect
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0002ABD1 File Offset: 0x00028DD1
		public string Name { get; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0002ABD9 File Offset: 0x00028DD9
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0002ABE1 File Offset: 0x00028DE1
		public bool IsEnabled { get; set; }

		// Token: 0x060007B6 RID: 1974 RVA: 0x0002ABEA File Offset: 0x00028DEA
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

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002AC28 File Offset: 0x00028E28
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

		// Token: 0x060007B8 RID: 1976 RVA: 0x0002AC92 File Offset: 0x00028E92
		public virtual void Clear()
		{
			if (!this.IsEnabled)
			{
				return;
			}
			ALC.EFX.Source(this.source, 131078, 0, 0, 0);
			this.IsEnabled = false;
		}

		// Token: 0x060007B9 RID: 1977
		protected abstract int GenerateEffect();

		// Token: 0x060007BA RID: 1978 RVA: 0x0002ACB8 File Offset: 0x00028EB8
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

		// Token: 0x060007BB RID: 1979 RVA: 0x0002AD80 File Offset: 0x00028F80
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

		// Token: 0x060007BC RID: 1980 RVA: 0x0002ADD0 File Offset: 0x00028FD0
		// Note: this type is marked as 'beforefieldinit'.
		static SoundEffect()
		{
			Dictionary<string, Func<int, SoundEffect>> dictionary = new Dictionary<string, Func<int, SoundEffect>>();
			dictionary.Add("cheapmic", (int source) => new CheapMicEffect(source));
			dictionary.Add("reverb", (int source) => new ReverbEffect(source));
			dictionary.Add("intoxicated", (int source) => new IntoxicatedEffect(source));
			SoundEffect.registry = dictionary;
		}

		// Token: 0x0400038E RID: 910
		protected int source;

		// Token: 0x0400038F RID: 911
		protected int effect;

		// Token: 0x04000390 RID: 912
		protected int slot;

		// Token: 0x04000391 RID: 913
		private int nullEffect;

		// Token: 0x04000393 RID: 915
		private static readonly Dictionary<string, Func<int, SoundEffect>> registry;
	}
}
