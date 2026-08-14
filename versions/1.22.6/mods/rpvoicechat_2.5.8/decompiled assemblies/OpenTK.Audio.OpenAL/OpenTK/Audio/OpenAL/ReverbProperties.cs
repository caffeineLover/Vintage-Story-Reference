using System;
using OpenTK.Mathematics;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x0200003A RID: 58
	public struct ReverbProperties
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00006FA4 File Offset: 0x000051A4
		public readonly float Density { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00006FAC File Offset: 0x000051AC
		public readonly float Diffusion { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00006FB4 File Offset: 0x000051B4
		public readonly float Gain { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00006FBC File Offset: 0x000051BC
		public readonly float GainHF { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00006FC4 File Offset: 0x000051C4
		public readonly float GainLF { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00006FCC File Offset: 0x000051CC
		public readonly float DecayTime { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00006FD4 File Offset: 0x000051D4
		public readonly float DecayHFRatio { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00006FDC File Offset: 0x000051DC
		public readonly float DecayLFRatio { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00006FE4 File Offset: 0x000051E4
		public readonly float ReflectionsGain { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00006FEC File Offset: 0x000051EC
		public readonly float ReflectionsDelay { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00006FF4 File Offset: 0x000051F4
		public readonly Vector3 ReflectionsPan { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00006FFC File Offset: 0x000051FC
		public readonly float LateReverbGain { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00007004 File Offset: 0x00005204
		public readonly float LateReverbDelay { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000EB RID: 235 RVA: 0x0000700C File Offset: 0x0000520C
		public readonly Vector3 LateReverbPan { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00007014 File Offset: 0x00005214
		public readonly float EchoTime { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000ED RID: 237 RVA: 0x0000701C File Offset: 0x0000521C
		public readonly float EchoDepth { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00007024 File Offset: 0x00005224
		public readonly float ModulationTime { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000EF RID: 239 RVA: 0x0000702C File Offset: 0x0000522C
		public readonly float ModulationDepth { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00007034 File Offset: 0x00005234
		public readonly float AirAbsorptionGainHF { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x0000703C File Offset: 0x0000523C
		public readonly float HFReference { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00007044 File Offset: 0x00005244
		public readonly float LFReference { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000704C File Offset: 0x0000524C
		public readonly float RoomRolloffFactor { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00007054 File Offset: 0x00005254
		public readonly int DecayHFLimit { get; }

		// Token: 0x060000F5 RID: 245 RVA: 0x0000705C File Offset: 0x0000525C
		public ReverbProperties(float density, float diffusion, float gain, float gainHF, float gainLF, float decayTime, float decayHFRatio, float decayLFRatio, float reflectionsGain, float reflectionsDelay, Vector3 reflectionsPan, float lateReverbGain, float lateReverbDelay, Vector3 lateReverbPan, float echoTime, float echoDepth, float modulationTime, float modulationDepth, float airAbsorptionGainHF, float hfReference, float lfReference, float roomRolloffFactor, int decayHFLimit)
		{
			this.Density = density;
			this.Diffusion = diffusion;
			this.Gain = gain;
			this.GainHF = gainHF;
			this.GainLF = gainLF;
			this.DecayTime = decayTime;
			this.DecayHFRatio = decayHFRatio;
			this.DecayLFRatio = decayLFRatio;
			this.ReflectionsGain = reflectionsGain;
			this.ReflectionsDelay = reflectionsDelay;
			this.ReflectionsPan = reflectionsPan;
			this.LateReverbGain = lateReverbGain;
			this.LateReverbDelay = lateReverbDelay;
			this.LateReverbPan = lateReverbPan;
			this.EchoTime = echoTime;
			this.EchoDepth = echoDepth;
			this.ModulationTime = modulationTime;
			this.ModulationDepth = modulationDepth;
			this.AirAbsorptionGainHF = airAbsorptionGainHF;
			this.HFReference = hfReference;
			this.LFReference = lfReference;
			this.RoomRolloffFactor = roomRolloffFactor;
			this.DecayHFLimit = decayHFLimit;
		}
	}
}
