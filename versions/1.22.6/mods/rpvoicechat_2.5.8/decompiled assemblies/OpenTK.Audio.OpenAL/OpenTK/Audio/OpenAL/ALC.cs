using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x02000007 RID: 7
	public class ALC : ALBase
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002169 File Offset: 0x00000369
		static ALC()
		{
			ALBase.RegisterOpenALResolver();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002170 File Offset: 0x00000370
		private ALC()
		{
		}

		// Token: 0x06000015 RID: 21
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCreateContext", ExactSpelling = true)]
		public unsafe static extern ALContext CreateContext([In] ALDevice device, [In] int* attributeList);

		// Token: 0x06000016 RID: 22
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCreateContext", ExactSpelling = true)]
		public static extern ALContext CreateContext([In] ALDevice device, [In] ref int attributeList);

		// Token: 0x06000017 RID: 23
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCreateContext", ExactSpelling = true)]
		public static extern ALContext CreateContext([In] ALDevice device, [In] int[] attributeList);

		// Token: 0x06000018 RID: 24 RVA: 0x00002178 File Offset: 0x00000378
		public static ALContext CreateContext(ALDevice device, Span<int> attributeList)
		{
			return ALC.CreateContext(device, attributeList[0]);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002188 File Offset: 0x00000388
		public static ALContext CreateContext(ALDevice device, ALContextAttributes attributes)
		{
			return ALC.CreateContext(device, attributes.CreateAttributeArray());
		}

		// Token: 0x0600001A RID: 26
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcMakeContextCurrent", ExactSpelling = true)]
		public static extern bool MakeContextCurrent(ALContext context);

		// Token: 0x0600001B RID: 27
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcProcessContext", ExactSpelling = true)]
		public static extern void ProcessContext(ALContext context);

		// Token: 0x0600001C RID: 28
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcSuspendContext", ExactSpelling = true)]
		public static extern void SuspendContext(ALContext context);

		// Token: 0x0600001D RID: 29
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcDestroyContext", ExactSpelling = true)]
		public static extern void DestroyContext(ALContext context);

		// Token: 0x0600001E RID: 30
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcGetCurrentContext", ExactSpelling = true)]
		public static extern ALContext GetCurrentContext();

		// Token: 0x0600001F RID: 31
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcGetContextsDevice", ExactSpelling = true)]
		public static extern ALDevice GetContextsDevice(ALContext context);

		// Token: 0x06000020 RID: 32
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcOpenDevice", ExactSpelling = true)]
		public static extern ALDevice OpenDevice([In] string devicename);

		// Token: 0x06000021 RID: 33
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCloseDevice", ExactSpelling = true)]
		public static extern bool CloseDevice([In] ALDevice device);

		// Token: 0x06000022 RID: 34
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcGetError", ExactSpelling = true)]
		public static extern AlcError GetError([In] ALDevice device);

		// Token: 0x06000023 RID: 35
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcIsExtensionPresent", ExactSpelling = true)]
		public static extern bool IsExtensionPresent([In] ALDevice device, [In] string extname);

		// Token: 0x06000024 RID: 36
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcIsExtensionPresent", ExactSpelling = true)]
		public static extern bool IsExtensionPresent([In] ALCaptureDevice device, [In] string extname);

		// Token: 0x06000025 RID: 37
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcGetProcAddress", ExactSpelling = true)]
		public static extern IntPtr GetProcAddress([In] ALDevice device, [In] string funcname);

		// Token: 0x06000026 RID: 38
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcGetEnumValue", ExactSpelling = true)]
		public static extern int GetEnumValue([In] ALDevice device, [In] string enumname);

		// Token: 0x06000027 RID: 39
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcGetString", ExactSpelling = true)]
		public unsafe static extern byte* GetStringPtr([In] ALDevice device, AlcGetString param);

		// Token: 0x06000028 RID: 40
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcGetString", ExactSpelling = true)]
		[return: MarshalAs(44, MarshalTypeRef = OpenTK.Audio.OpenAL.Native.ConstCharPtrMarshaler)]
		public static extern string GetString([In] ALDevice device, AlcGetString param);

		// Token: 0x06000029 RID: 41 RVA: 0x00002196 File Offset: 0x00000396
		public static List<string> GetString(ALDevice device, AlcGetStringList param)
		{
			return ALC.ALStringListToList(ALC.GetStringPtr(device, (AlcGetString)param));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000021A4 File Offset: 0x000003A4
		public static List<string> GetString(AlcGetStringList param)
		{
			return ALC.GetString(ALDevice.Null, param);
		}

		// Token: 0x0600002B RID: 43
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcGetIntegerv", ExactSpelling = true)]
		public unsafe static extern void GetInteger(ALDevice device, AlcGetInteger param, int size, int* data);

		// Token: 0x0600002C RID: 44
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcGetIntegerv", ExactSpelling = true)]
		public static extern void GetInteger(ALDevice device, AlcGetInteger param, int size, int[] data);

		// Token: 0x0600002D RID: 45
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcGetIntegerv", ExactSpelling = true)]
		public static extern void GetInteger(ALDevice device, AlcGetInteger param, int size, out int data);

		// Token: 0x0600002E RID: 46 RVA: 0x000021B1 File Offset: 0x000003B1
		public static void GetInteger(ALDevice device, AlcGetInteger param, out int data)
		{
			ALC.GetInteger(device, param, 1, out data);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000021BC File Offset: 0x000003BC
		public static int GetInteger(ALDevice device, AlcGetInteger param)
		{
			int result;
			ALC.GetInteger(device, param, 1, out result);
			return result;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000021D4 File Offset: 0x000003D4
		public static int[] GetAttributeArray(ALDevice device)
		{
			int num;
			ALC.GetInteger(device, AlcGetInteger.AttributesSize, 1, out num);
			int[] array = new int[num];
			ALC.GetInteger(device, AlcGetInteger.AllAttributes, num, array);
			return array;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002204 File Offset: 0x00000404
		public static ALContextAttributes GetContextAttributes(ALDevice device)
		{
			int num;
			ALC.GetInteger(device, AlcGetInteger.AttributesSize, 1, out num);
			int[] array = new int[num];
			ALC.GetInteger(device, AlcGetInteger.AllAttributes, num, array);
			return ALContextAttributes.FromArray(array);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002239 File Offset: 0x00000439
		public static bool IsCaptureExtensionPresent(ALDevice device)
		{
			return ALC.IsExtensionPresent(device, "ALC_EXT_CAPTURE");
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002246 File Offset: 0x00000446
		public static bool IsCaptureExtensionPresent(ALCaptureDevice device)
		{
			return ALC.IsExtensionPresent(device, "ALC_EXT_CAPTURE");
		}

		// Token: 0x06000034 RID: 52
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcCaptureOpenDevice", ExactSpelling = true)]
		public static extern ALCaptureDevice CaptureOpenDevice(string devicename, uint frequency, ALFormat format, int buffersize);

		// Token: 0x06000035 RID: 53
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcCaptureOpenDevice", ExactSpelling = true)]
		public static extern ALCaptureDevice CaptureOpenDevice(string devicename, int frequency, ALFormat format, int buffersize);

		// Token: 0x06000036 RID: 54
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCaptureCloseDevice", ExactSpelling = true)]
		public static extern bool CaptureCloseDevice([In] ALCaptureDevice device);

		// Token: 0x06000037 RID: 55
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCaptureStart", ExactSpelling = true)]
		public static extern void CaptureStart([In] ALCaptureDevice device);

		// Token: 0x06000038 RID: 56
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCaptureStop", ExactSpelling = true)]
		public static extern void CaptureStop([In] ALCaptureDevice device);

		// Token: 0x06000039 RID: 57
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCaptureSamples", ExactSpelling = true)]
		public static extern void CaptureSamples(ALCaptureDevice device, IntPtr buffer, int samples);

		// Token: 0x0600003A RID: 58
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCaptureSamples", ExactSpelling = true)]
		public unsafe static extern void CaptureSamples(ALCaptureDevice device, void* buffer, int samples);

		// Token: 0x0600003B RID: 59
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCaptureSamples", ExactSpelling = true)]
		public static extern void CaptureSamples(ALCaptureDevice device, ref byte buffer, int samples);

		// Token: 0x0600003C RID: 60
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcCaptureSamples", ExactSpelling = true)]
		public static extern void CaptureSamples(ALCaptureDevice device, ref short buffer, int samples);

		// Token: 0x0600003D RID: 61 RVA: 0x00002254 File Offset: 0x00000454
		public unsafe static void CaptureSamples<[IsUnmanaged] T>(ALCaptureDevice device, ref T buffer, int samples) where T : struct, ValueType
		{
			fixed (T* ptr = &buffer)
			{
				T* buffer2 = ptr;
				ALC.CaptureSamples(device, (void*)buffer2, samples);
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002271 File Offset: 0x00000471
		public static void CaptureSamples<[IsUnmanaged] T>(ALCaptureDevice device, T[] buffer, int samples) where T : struct, ValueType
		{
			ALC.CaptureSamples<T>(device, ref buffer[0], samples);
		}

		// Token: 0x0600003F RID: 63
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcGetIntegerv", ExactSpelling = true)]
		public unsafe static extern void GetInteger(ALCaptureDevice device, AlcGetInteger param, int size, int* data);

		// Token: 0x06000040 RID: 64
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcGetIntegerv", ExactSpelling = true)]
		public static extern void GetInteger(ALCaptureDevice device, AlcGetInteger param, int size, int[] data);

		// Token: 0x06000041 RID: 65
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alcGetIntegerv", ExactSpelling = true)]
		public static extern void GetInteger(ALCaptureDevice device, AlcGetInteger param, int size, out int data);

		// Token: 0x06000042 RID: 66 RVA: 0x00002284 File Offset: 0x00000484
		public static int GetInteger(ALCaptureDevice device, AlcGetInteger param)
		{
			int result;
			ALC.GetInteger(device, param, 1, out result);
			return result;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000229C File Offset: 0x0000049C
		public static bool IsEnumerationExtensionPresent(ALDevice device)
		{
			return ALC.IsExtensionPresent(device, "ALC_ENUMERATION_EXT");
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000022A9 File Offset: 0x000004A9
		public static bool IsEnumerationExtensionPresent(ALCaptureDevice device)
		{
			return ALC.IsExtensionPresent(device, "ALC_ENUMERATION_EXT");
		}

		// Token: 0x06000045 RID: 69
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcGetString", ExactSpelling = true)]
		[return: MarshalAs(44, MarshalTypeRef = OpenTK.Audio.OpenAL.Native.ConstCharPtrMarshaler)]
		public static extern string GetString(ALDevice device, GetEnumerationString param);

		// Token: 0x06000046 RID: 70
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alcGetString", ExactSpelling = true)]
		public unsafe static extern byte* GetStringListPtr(ALDevice device, GetEnumerationStringList param);

		// Token: 0x06000047 RID: 71 RVA: 0x000022B6 File Offset: 0x000004B6
		public static IEnumerable<string> GetStringList(GetEnumerationStringList param)
		{
			return ALC.ALStringListToList(ALC.GetStringListPtr(ALDevice.Null, param));
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000022C8 File Offset: 0x000004C8
		internal unsafe static List<string> ALStringListToList(byte* alList)
		{
			if (alList == null)
			{
				return new List<string>();
			}
			List<string> list = new List<string>();
			byte* ptr = alList;
			for (;;)
			{
				string text = Marshal.PtrToStringAnsi(new IntPtr((void*)ptr));
				if (string.IsNullOrEmpty(text))
				{
					break;
				}
				list.Add(text);
				ptr += text.Length + 1;
			}
			return list;
		}

		// Token: 0x04000008 RID: 8
		internal const string Lib = "AL";

		// Token: 0x04000009 RID: 9
		internal const CallingConvention AlcCallingConv = 2;

		// Token: 0x0200004B RID: 75
		public class EFX : ALBase
		{
			// Token: 0x0600010D RID: 269 RVA: 0x00007448 File Offset: 0x00005648
			static EFX()
			{
				ALBase.RegisterOpenALResolver();
			}

			// Token: 0x0600010E RID: 270 RVA: 0x00007838 File Offset: 0x00005A38
			private EFX()
			{
			}

			// Token: 0x0600010F RID: 271 RVA: 0x00007840 File Offset: 0x00005A40
			public static bool IsExtensionPresent(ALDevice device)
			{
				return ALC.IsExtensionPresent(device, "ALC_EXT_EFX");
			}

			// Token: 0x06000110 RID: 272
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alcGetIntegerv", ExactSpelling = true)]
			public unsafe static extern void GetInteger(ALDevice device, EFXContextInteger param, int size, int* data);

			// Token: 0x06000111 RID: 273
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alcGetIntegerv", ExactSpelling = true)]
			public static extern void GetInteger(ALDevice device, EFXContextInteger param, int size, ref int data);

			// Token: 0x06000112 RID: 274
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alcGetIntegerv", ExactSpelling = true)]
			public static extern void GetInteger(ALDevice device, EFXContextInteger param, int size, int[] data);

			// Token: 0x06000113 RID: 275 RVA: 0x0000784D File Offset: 0x00005A4D
			public static void GetInteger(ALDevice device, EFXContextInteger param, int[] data)
			{
				ALC.EFX.GetInteger(device, param, data.Length, data);
			}

			// Token: 0x06000114 RID: 276 RVA: 0x0000785C File Offset: 0x00005A5C
			public int GetEFXMajorVersion(ALDevice device)
			{
				int result = 0;
				ALC.EFX.GetInteger(device, EFXContextInteger.EFXMajorVersion, 1, ref result);
				return result;
			}

			// Token: 0x06000115 RID: 277 RVA: 0x0000787C File Offset: 0x00005A7C
			public int GetEFXMinorVersion(ALDevice device)
			{
				int result = 0;
				ALC.EFX.GetInteger(device, EFXContextInteger.EFXMinorVersion, 1, ref result);
				return result;
			}

			// Token: 0x06000116 RID: 278 RVA: 0x0000789A File Offset: 0x00005A9A
			public Version GetEFXVersion(ALDevice device)
			{
				return new Version(this.GetEFXMajorVersion(device), this.GetEFXMinorVersion(device));
			}

			// Token: 0x06000117 RID: 279 RVA: 0x000078AF File Offset: 0x00005AAF
			public unsafe static void GenAuxiliaryEffectSlots(int count, int* slots)
			{
				ALC.EFX._GenAuxiliaryEffectSlotsPtr(count, slots);
			}

			// Token: 0x06000118 RID: 280 RVA: 0x000078BD File Offset: 0x00005ABD
			public static void GenAuxiliaryEffectSlots(int count, ref int slots)
			{
				ALC.EFX._GenAuxiliaryEffectSlotsRef(count, ref slots);
			}

			// Token: 0x06000119 RID: 281 RVA: 0x000078CB File Offset: 0x00005ACB
			public static void GenAuxiliaryEffectSlots(int count, int[] slots)
			{
				ALC.EFX._GenAuxiliaryEffectSlotsArray(count, slots);
			}

			// Token: 0x0600011A RID: 282 RVA: 0x000078D9 File Offset: 0x00005AD9
			public unsafe static void DeleteAuxiliaryEffectSlots(int count, int* slots)
			{
				ALC.EFX._DeleteAuxiliaryEffectSlotsPtr(count, slots);
			}

			// Token: 0x0600011B RID: 283 RVA: 0x000078E7 File Offset: 0x00005AE7
			public static void DeleteAuxiliaryEffectSlots(int count, ref int slots)
			{
				ALC.EFX._DeleteAuxiliaryEffectSlotsRef(count, ref slots);
			}

			// Token: 0x0600011C RID: 284 RVA: 0x000078F5 File Offset: 0x00005AF5
			public static void DeleteAuxiliaryEffectSlots(int count, int[] slots)
			{
				ALC.EFX._DeleteAuxiliaryEffectSlotsArray(count, slots);
			}

			// Token: 0x0600011D RID: 285 RVA: 0x00007903 File Offset: 0x00005B03
			public static bool IsAuxiliaryEffectSlot(int slot)
			{
				return ALC.EFX._IsAuxiliaryEffectSlot(slot);
			}

			// Token: 0x0600011E RID: 286 RVA: 0x00007910 File Offset: 0x00005B10
			public static void AuxiliaryEffectSlot(int slot, EffectSlotInteger param, int value)
			{
				ALC.EFX._AuxiliaryEffectSloti(slot, param, value);
			}

			// Token: 0x0600011F RID: 287 RVA: 0x0000791F File Offset: 0x00005B1F
			public static void AuxiliaryEffectSlot(int slot, EffectSlotBoolean param, bool value)
			{
				ALC.EFX._AuxiliaryEffectSlotb(slot, param, (value > false) ? 1 : 0);
			}

			// Token: 0x06000120 RID: 288 RVA: 0x00007931 File Offset: 0x00005B31
			public static void AuxiliaryEffectSlot(int slot, EffectSlotFloat param, float value)
			{
				ALC.EFX._AuxiliaryEffectSlotf(slot, param, value);
			}

			// Token: 0x06000121 RID: 289 RVA: 0x00007940 File Offset: 0x00005B40
			public unsafe static void GetAuxiliaryEffectSlot(int slot, EffectSlotInteger param, int* value)
			{
				ALC.EFX._GetAuxiliaryEffectSlotiPtr(slot, param, value);
			}

			// Token: 0x06000122 RID: 290 RVA: 0x0000794F File Offset: 0x00005B4F
			public static void GetAuxiliaryEffectSlot(int slot, EffectSlotInteger param, out int value)
			{
				ALC.EFX._GetAuxiliaryEffectSlotiRef(slot, param, out value);
			}

			// Token: 0x06000123 RID: 291 RVA: 0x0000795E File Offset: 0x00005B5E
			public unsafe static void GetAuxiliaryEffectSlot(int slot, EffectSlotFloat param, float* value)
			{
				ALC.EFX._GetAuxiliaryEffectSlotfPtr(slot, param, value);
			}

			// Token: 0x06000124 RID: 292 RVA: 0x0000796D File Offset: 0x00005B6D
			public static void GetAuxiliaryEffectSlot(int slot, EffectSlotFloat param, out float value)
			{
				ALC.EFX._GetAuxiliaryEffectSlotfRef(slot, param, out value);
			}

			// Token: 0x06000125 RID: 293 RVA: 0x0000797C File Offset: 0x00005B7C
			public unsafe static void GenEffects(int count, int* effects)
			{
				ALC.EFX._GenEffectsPtr(count, effects);
			}

			// Token: 0x06000126 RID: 294 RVA: 0x0000798A File Offset: 0x00005B8A
			public static void GenEffects(int count, ref int effects)
			{
				ALC.EFX._GenEffectsRef(count, ref effects);
			}

			// Token: 0x06000127 RID: 295 RVA: 0x00007998 File Offset: 0x00005B98
			public static void GenEffects(int count, int[] effects)
			{
				ALC.EFX._GenEffectsArray(count, effects);
			}

			// Token: 0x06000128 RID: 296 RVA: 0x000079A6 File Offset: 0x00005BA6
			public unsafe static void DeleteEffects(int count, int* effects)
			{
				ALC.EFX._DeleteEffectsPtr(count, effects);
			}

			// Token: 0x06000129 RID: 297 RVA: 0x000079B4 File Offset: 0x00005BB4
			public static void DeleteEffects(int count, ref int effects)
			{
				ALC.EFX._DeleteEffectsRef(count, ref effects);
			}

			// Token: 0x0600012A RID: 298 RVA: 0x000079C2 File Offset: 0x00005BC2
			public static void DeleteEffects(int count, int[] effects)
			{
				ALC.EFX._DeleteEffectsArray(count, effects);
			}

			// Token: 0x0600012B RID: 299 RVA: 0x000079D0 File Offset: 0x00005BD0
			public static bool IsEffect(int effect)
			{
				return ALC.EFX._IsEffect(effect);
			}

			// Token: 0x0600012C RID: 300 RVA: 0x000079DD File Offset: 0x00005BDD
			public static void Effect(int effect, EffectInteger param, int value)
			{
				ALC.EFX._Effecti(effect, param, value);
			}

			// Token: 0x0600012D RID: 301 RVA: 0x000079EC File Offset: 0x00005BEC
			public static void Effect(int effect, EffectFloat param, float value)
			{
				ALC.EFX._Effectf(effect, param, value);
			}

			// Token: 0x0600012E RID: 302 RVA: 0x000079FB File Offset: 0x00005BFB
			public unsafe static void Effect(int effect, EffectVector3 param, float* value)
			{
				ALC.EFX._EffectfvPtr(effect, param, value);
			}

			// Token: 0x0600012F RID: 303 RVA: 0x00007A0A File Offset: 0x00005C0A
			public static void Effect(int effect, EffectVector3 param, ref float value)
			{
				ALC.EFX._EffectfvRef(effect, param, ref value);
			}

			// Token: 0x06000130 RID: 304 RVA: 0x00007A19 File Offset: 0x00005C19
			public static void Effect(int effect, EffectVector3 param, float[] value)
			{
				ALC.EFX._EffectfvArray(effect, param, value);
			}

			// Token: 0x06000131 RID: 305 RVA: 0x00007A28 File Offset: 0x00005C28
			public unsafe static void GetEffect(int effect, EffectInteger param, int* value)
			{
				ALC.EFX._GetEffectiPtr(effect, param, value);
			}

			// Token: 0x06000132 RID: 306 RVA: 0x00007A37 File Offset: 0x00005C37
			public static void GetEffect(int effect, EffectInteger param, out int value)
			{
				ALC.EFX._GetEffectiRef(effect, param, out value);
			}

			// Token: 0x06000133 RID: 307 RVA: 0x00007A46 File Offset: 0x00005C46
			public unsafe static void GetEffect(int effect, EffectFloat param, float* value)
			{
				ALC.EFX._GetEffectfPtr(effect, param, value);
			}

			// Token: 0x06000134 RID: 308 RVA: 0x00007A55 File Offset: 0x00005C55
			public static void GetEffect(int effect, EffectFloat param, out float value)
			{
				ALC.EFX._GetEffectfRef(effect, param, out value);
			}

			// Token: 0x06000135 RID: 309 RVA: 0x00007A64 File Offset: 0x00005C64
			public unsafe static void GetEffect(int effect, EffectVector3 param, float* value)
			{
				ALC.EFX._GetEffectfvPtr(effect, param, value);
			}

			// Token: 0x06000136 RID: 310 RVA: 0x00007A73 File Offset: 0x00005C73
			public static void GetEffect(int effect, EffectVector3 param, out float value)
			{
				ALC.EFX._GetEffectfvRef(effect, param, out value);
			}

			// Token: 0x06000137 RID: 311 RVA: 0x00007A82 File Offset: 0x00005C82
			public unsafe static void GenFilters(int count, int* filters)
			{
				ALC.EFX._GenFiltersPtr(count, filters);
			}

			// Token: 0x06000138 RID: 312 RVA: 0x00007A90 File Offset: 0x00005C90
			public static void GenFilters(int count, ref int filters)
			{
				ALC.EFX._GenFiltersRef(count, ref filters);
			}

			// Token: 0x06000139 RID: 313 RVA: 0x00007A9E File Offset: 0x00005C9E
			public static void GenFilters(int count, int[] filters)
			{
				ALC.EFX._GenFiltersArray(count, filters);
			}

			// Token: 0x0600013A RID: 314 RVA: 0x00007AAC File Offset: 0x00005CAC
			public unsafe static void DeleteFilters(int count, int* filters)
			{
				ALC.EFX._DeleteFiltersPtr(count, filters);
			}

			// Token: 0x0600013B RID: 315 RVA: 0x00007ABA File Offset: 0x00005CBA
			public static void DeleteFilters(int count, ref int filters)
			{
				ALC.EFX._DeleteFiltersRef(count, ref filters);
			}

			// Token: 0x0600013C RID: 316 RVA: 0x00007AC8 File Offset: 0x00005CC8
			public static void DeleteFilters(int count, int[] filters)
			{
				ALC.EFX._DeleteFiltersArray(count, filters);
			}

			// Token: 0x0600013D RID: 317 RVA: 0x00007AD6 File Offset: 0x00005CD6
			public static bool IsFilter(int filter)
			{
				return ALC.EFX._IsFilter(filter);
			}

			// Token: 0x0600013E RID: 318 RVA: 0x00007AE3 File Offset: 0x00005CE3
			public static void Filter(int filter, FilterInteger param, int value)
			{
				ALC.EFX._Filteri(filter, param, value);
			}

			// Token: 0x0600013F RID: 319 RVA: 0x00007AF2 File Offset: 0x00005CF2
			public static void Filter(int filter, FilterFloat param, float value)
			{
				ALC.EFX._Filterf(filter, param, value);
			}

			// Token: 0x06000140 RID: 320 RVA: 0x00007B01 File Offset: 0x00005D01
			public unsafe static void GetFilter(int filter, FilterInteger param, int* value)
			{
				ALC.EFX._GetFilteriPtr(filter, param, value);
			}

			// Token: 0x06000141 RID: 321 RVA: 0x00007B10 File Offset: 0x00005D10
			public static void GetFilter(int filter, FilterInteger param, out int value)
			{
				ALC.EFX._GetFilteriRef(filter, param, out value);
			}

			// Token: 0x06000142 RID: 322 RVA: 0x00007B1F File Offset: 0x00005D1F
			public unsafe static void GetFilter(int filter, FilterFloat param, float* value)
			{
				ALC.EFX._GetFilterfPtr(filter, param, value);
			}

			// Token: 0x06000143 RID: 323 RVA: 0x00007B2E File Offset: 0x00005D2E
			public static void GetFilter(int filter, FilterFloat param, out float value)
			{
				ALC.EFX._GetFilterfRef(filter, param, out value);
			}

			// Token: 0x06000144 RID: 324 RVA: 0x00007B3D File Offset: 0x00005D3D
			public static void Source(int source, EFXSourceInteger param, int value)
			{
				ALC.EFX._Sourcei(source, param, value);
			}

			// Token: 0x06000145 RID: 325 RVA: 0x00007B4C File Offset: 0x00005D4C
			public static void Source(int source, EFXSourceFloat param, float value)
			{
				ALC.EFX._Source(source, param, value);
			}

			// Token: 0x06000146 RID: 326 RVA: 0x00007B5B File Offset: 0x00005D5B
			public static void Source(int source, EFXSourceBoolean param, bool value)
			{
				ALC.EFX._Sourceb(source, param, value);
			}

			// Token: 0x06000147 RID: 327 RVA: 0x00007B6A File Offset: 0x00005D6A
			public unsafe static void Source(int source, EFXSourceInteger3 param, int* value)
			{
				ALC.EFX._SourceivPtr(source, param, value);
			}

			// Token: 0x06000148 RID: 328 RVA: 0x00007B79 File Offset: 0x00005D79
			public static void Source(int source, EFXSourceInteger3 param, ref int value)
			{
				ALC.EFX._SourceivRef(source, param, ref value);
			}

			// Token: 0x06000149 RID: 329 RVA: 0x00007B88 File Offset: 0x00005D88
			public static void Source(int source, EFXSourceInteger3 param, int[] value)
			{
				ALC.EFX._SourceivArray(source, param, value);
			}

			// Token: 0x0600014A RID: 330 RVA: 0x00007B97 File Offset: 0x00005D97
			public static void Source(int source, EFXSourceInteger3 param, int value1, int value2, int value3)
			{
				ALC.EFX._Source3i(source, param, value1, value2, value3);
			}

			// Token: 0x0600014B RID: 331 RVA: 0x00007BA9 File Offset: 0x00005DA9
			public unsafe static void GetSource(int source, EFXSourceInteger param, int* value)
			{
				ALC.EFX._GetSourceiPtr(source, param, value);
			}

			// Token: 0x0600014C RID: 332 RVA: 0x00007BB8 File Offset: 0x00005DB8
			public static void GetSource(int source, EFXSourceInteger param, out int value)
			{
				ALC.EFX._GetSourceiRef(source, param, out value);
			}

			// Token: 0x0600014D RID: 333 RVA: 0x00007BC7 File Offset: 0x00005DC7
			public unsafe static void GetSource(int source, EFXSourceFloat param, float* value)
			{
				ALC.EFX._GetSourcefPtr(source, param, value);
			}

			// Token: 0x0600014E RID: 334 RVA: 0x00007BD6 File Offset: 0x00005DD6
			public static void GetSource(int source, EFXSourceFloat param, out float value)
			{
				ALC.EFX._GetSourcefRef(source, param, out value);
			}

			// Token: 0x0600014F RID: 335 RVA: 0x00007BE5 File Offset: 0x00005DE5
			public unsafe static void GetSource(int source, EFXSourceBoolean param, bool* value)
			{
				ALC.EFX._GetSourcebPtr(source, param, value);
			}

			// Token: 0x06000150 RID: 336 RVA: 0x00007BF4 File Offset: 0x00005DF4
			public static void GetSource(int source, EFXSourceBoolean param, out bool value)
			{
				ALC.EFX._GetSourcebRef(source, param, out value);
			}

			// Token: 0x06000151 RID: 337 RVA: 0x00007C03 File Offset: 0x00005E03
			public unsafe static void GetSource(int source, EFXSourceInteger3 param, int* value)
			{
				ALC.EFX._GetSourceivPtr(source, param, value);
			}

			// Token: 0x06000152 RID: 338 RVA: 0x00007C12 File Offset: 0x00005E12
			public static void GetSource(int source, EFXSourceInteger3 param, ref int value)
			{
				ALC.EFX._GetSourceivRef(source, param, ref value);
			}

			// Token: 0x06000153 RID: 339 RVA: 0x00007C21 File Offset: 0x00005E21
			public static void GetSource(int source, EFXSourceInteger3 param, int[] value)
			{
				ALC.EFX._GetSourceivArray(source, param, value);
			}

			// Token: 0x06000154 RID: 340 RVA: 0x00007C30 File Offset: 0x00005E30
			public unsafe static void GetSource(int source, EFXSourceInteger3 param, int* value1, int* value2, int* value3)
			{
				ALC.EFX._GetSource3iPtr(source, param, value1, value2, value3);
			}

			// Token: 0x06000155 RID: 341 RVA: 0x00007C42 File Offset: 0x00005E42
			public static void GetSource(int source, EFXSourceInteger3 param, out int value1, out int value2, out int value3)
			{
				ALC.EFX._GetSource3iRef(source, param, out value1, out value2, out value3);
			}

			// Token: 0x06000156 RID: 342 RVA: 0x00007C54 File Offset: 0x00005E54
			public static void Listener(int listener, EFXListenerFloat param, float value)
			{
				ALC.EFX._Listenerf(listener, param, value);
			}

			// Token: 0x06000157 RID: 343 RVA: 0x00007C63 File Offset: 0x00005E63
			public unsafe static void GetListener(int listener, EFXListenerFloat param, float* value)
			{
				ALC.EFX._GetListenerfPtr(listener, param, value);
			}

			// Token: 0x06000158 RID: 344 RVA: 0x00007C72 File Offset: 0x00005E72
			public static void GetListener(int listener, EFXListenerFloat param, out float value)
			{
				ALC.EFX._GetListenerfRef(listener, param, out value);
			}

			// Token: 0x06000159 RID: 345 RVA: 0x00007C81 File Offset: 0x00005E81
			public static void GenAuxiliaryEffectSlots(int[] slots)
			{
				ALC.EFX.GenAuxiliaryEffectSlots(slots.Length, slots);
			}

			// Token: 0x0600015A RID: 346 RVA: 0x00007C8C File Offset: 0x00005E8C
			public static int[] GenAuxiliaryEffectSlots(int count)
			{
				int[] array = new int[count];
				ALC.EFX.GenAuxiliaryEffectSlots(count, array);
				return array;
			}

			// Token: 0x0600015B RID: 347 RVA: 0x00007CA8 File Offset: 0x00005EA8
			public static int GenAuxiliaryEffectSlot()
			{
				int result = 0;
				ALC.EFX.GenAuxiliaryEffectSlots(1, ref result);
				return result;
			}

			// Token: 0x0600015C RID: 348 RVA: 0x00007CC0 File Offset: 0x00005EC0
			public static void GenAuxiliaryEffectSlot(out int slot)
			{
				int num = 0;
				ALC.EFX.GenAuxiliaryEffectSlots(1, ref num);
				slot = num;
			}

			// Token: 0x0600015D RID: 349 RVA: 0x00007CDA File Offset: 0x00005EDA
			public static void DeleteAuxiliaryEffectSlots(int[] slots)
			{
				ALC.EFX.DeleteAuxiliaryEffectSlots(slots.Length, slots);
			}

			// Token: 0x0600015E RID: 350 RVA: 0x00007CE5 File Offset: 0x00005EE5
			public static void DeleteAuxiliaryEffectSlot(int slot)
			{
				ALC.EFX.DeleteAuxiliaryEffectSlots(1, ref slot);
			}

			// Token: 0x0600015F RID: 351 RVA: 0x00007CF0 File Offset: 0x00005EF0
			public static int GetAuxiliaryEffectSlot(int slot, EffectSlotInteger param)
			{
				int result;
				ALC.EFX.GetAuxiliaryEffectSlot(slot, param, out result);
				return result;
			}

			// Token: 0x06000160 RID: 352 RVA: 0x00007D08 File Offset: 0x00005F08
			public static float GetAuxiliaryEffectSlot(int slot, EffectSlotFloat param)
			{
				float result;
				ALC.EFX.GetAuxiliaryEffectSlot(slot, param, out result);
				return result;
			}

			// Token: 0x06000161 RID: 353 RVA: 0x00007D1F File Offset: 0x00005F1F
			public static void GenEffects(int[] effects)
			{
				ALC.EFX.GenEffects(effects.Length, effects);
			}

			// Token: 0x06000162 RID: 354 RVA: 0x00007D2C File Offset: 0x00005F2C
			public static int[] GenEffects(int count)
			{
				int[] array = new int[count];
				ALC.EFX.GenEffects(count, array);
				return array;
			}

			// Token: 0x06000163 RID: 355 RVA: 0x00007D48 File Offset: 0x00005F48
			public static int GenEffect()
			{
				int result = 0;
				ALC.EFX.GenEffects(1, ref result);
				return result;
			}

			// Token: 0x06000164 RID: 356 RVA: 0x00007D60 File Offset: 0x00005F60
			public static void GenEffect(out int effect)
			{
				int num = 0;
				ALC.EFX.GenEffects(1, ref num);
				effect = num;
			}

			// Token: 0x06000165 RID: 357 RVA: 0x00007D7A File Offset: 0x00005F7A
			public static void DeleteEffects(int[] effects)
			{
				ALC.EFX.DeleteEffects(effects.Length, effects);
			}

			// Token: 0x06000166 RID: 358 RVA: 0x00007D85 File Offset: 0x00005F85
			public static void DeleteEffect(int effect)
			{
				ALC.EFX.DeleteEffects(1, ref effect);
			}

			// Token: 0x06000167 RID: 359 RVA: 0x00007D8F File Offset: 0x00005F8F
			public static void Effect(int effect, EffectVector3 param, ref Vector3 value)
			{
				ALC.EFX.Effect(effect, param, ref value.X);
			}

			// Token: 0x06000168 RID: 360 RVA: 0x00007DA0 File Offset: 0x00005FA0
			public static float GetEffect(int effect, EffectFloat param)
			{
				float result;
				ALC.EFX.GetEffect(effect, param, out result);
				return result;
			}

			// Token: 0x06000169 RID: 361 RVA: 0x00007DB8 File Offset: 0x00005FB8
			public static int GetEffect(int effect, EffectInteger param)
			{
				int result;
				ALC.EFX.GetEffect(effect, param, out result);
				return result;
			}

			// Token: 0x0600016A RID: 362 RVA: 0x00007DCF File Offset: 0x00005FCF
			public static void GetEffect(int effect, EffectVector3 param, out Vector3 value)
			{
				value.Y = 0f;
				value.Z = 0f;
				ALC.EFX.GetEffect(effect, param, out value.X);
			}

			// Token: 0x0600016B RID: 363 RVA: 0x00007DF4 File Offset: 0x00005FF4
			public static Vector3 GetEffect(int effect, EffectVector3 param)
			{
				Vector3 result;
				ALC.EFX.GetEffect(effect, param, out result);
				return result;
			}

			// Token: 0x0600016C RID: 364 RVA: 0x00007E0B File Offset: 0x0000600B
			public static void GenFilters(int[] filters)
			{
				ALC.EFX.GenFilters(filters.Length, filters);
			}

			// Token: 0x0600016D RID: 365 RVA: 0x00007E18 File Offset: 0x00006018
			public static int[] GenFilters(int count)
			{
				int[] array = new int[count];
				ALC.EFX.GenFilters(count, array);
				return array;
			}

			// Token: 0x0600016E RID: 366 RVA: 0x00007E34 File Offset: 0x00006034
			public static int GenFilter()
			{
				int result = 0;
				ALC.EFX.GenFilters(1, ref result);
				return result;
			}

			// Token: 0x0600016F RID: 367 RVA: 0x00007E4C File Offset: 0x0000604C
			public static void GenFilter(out int filter)
			{
				int num = 0;
				ALC.EFX.GenFilters(1, ref num);
				filter = num;
			}

			// Token: 0x06000170 RID: 368 RVA: 0x00007E66 File Offset: 0x00006066
			public static void DeleteFilters(int[] filters)
			{
				ALC.EFX.DeleteFilters(filters.Length, filters);
			}

			// Token: 0x06000171 RID: 369 RVA: 0x00007E71 File Offset: 0x00006071
			public static void DeleteFilter(int filter)
			{
				ALC.EFX.DeleteFilters(1, ref filter);
			}

			// Token: 0x06000172 RID: 370 RVA: 0x00007E7C File Offset: 0x0000607C
			public static int GetFilter(int filter, FilterInteger param)
			{
				int result;
				ALC.EFX.GetFilter(filter, param, out result);
				return result;
			}

			// Token: 0x06000173 RID: 371 RVA: 0x00007E94 File Offset: 0x00006094
			public static float GetFilter(int filter, FilterFloat param)
			{
				float result;
				ALC.EFX.GetFilter(filter, param, out result);
				return result;
			}

			// Token: 0x06000174 RID: 372 RVA: 0x00007EAC File Offset: 0x000060AC
			public static int GetSource(int source, EFXSourceInteger param)
			{
				int result;
				ALC.EFX.GetSource(source, param, out result);
				return result;
			}

			// Token: 0x06000175 RID: 373 RVA: 0x00007EC4 File Offset: 0x000060C4
			public static float GetSource(int source, EFXSourceFloat param)
			{
				float result;
				ALC.EFX.GetSource(source, param, out result);
				return result;
			}

			// Token: 0x06000176 RID: 374 RVA: 0x00007EDC File Offset: 0x000060DC
			public static bool GetSource(int source, EFXSourceBoolean param)
			{
				bool result;
				ALC.EFX.GetSource(source, param, out result);
				return result;
			}

			// Token: 0x06000177 RID: 375 RVA: 0x00007EF4 File Offset: 0x000060F4
			public static float GetListener(int listener, EFXListenerFloat param)
			{
				float result;
				ALC.EFX.GetListener(listener, param, out result);
				return result;
			}

			// Token: 0x04000231 RID: 561
			public const string ExtensionName = "ALC_EXT_EFX";

			// Token: 0x04000232 RID: 562
			private static readonly ALC.EFX.GenAuxiliaryEffectSlotsPtrDelegate _GenAuxiliaryEffectSlotsPtr = ALBase.LoadDelegate<ALC.EFX.GenAuxiliaryEffectSlotsPtrDelegate>("alGenAuxiliaryEffectSlots");

			// Token: 0x04000233 RID: 563
			private static readonly ALC.EFX.GenAuxiliaryEffectSlotsRefDelegate _GenAuxiliaryEffectSlotsRef = ALBase.LoadDelegate<ALC.EFX.GenAuxiliaryEffectSlotsRefDelegate>("alGenAuxiliaryEffectSlots");

			// Token: 0x04000234 RID: 564
			private static readonly ALC.EFX.GenAuxiliaryEffectSlotsArrayDelegate _GenAuxiliaryEffectSlotsArray = ALBase.LoadDelegate<ALC.EFX.GenAuxiliaryEffectSlotsArrayDelegate>("alGenAuxiliaryEffectSlots");

			// Token: 0x04000235 RID: 565
			private static readonly ALC.EFX.DeleteAuxiliaryEffectSlotsPtrDelegate _DeleteAuxiliaryEffectSlotsPtr = ALBase.LoadDelegate<ALC.EFX.DeleteAuxiliaryEffectSlotsPtrDelegate>("alDeleteAuxiliaryEffectSlots");

			// Token: 0x04000236 RID: 566
			private static readonly ALC.EFX.DeleteAuxiliaryEffectSlotsRefDelegate _DeleteAuxiliaryEffectSlotsRef = ALBase.LoadDelegate<ALC.EFX.DeleteAuxiliaryEffectSlotsRefDelegate>("alDeleteAuxiliaryEffectSlots");

			// Token: 0x04000237 RID: 567
			private static readonly ALC.EFX.DeleteAuxiliaryEffectSlotsArrayDelegate _DeleteAuxiliaryEffectSlotsArray = ALBase.LoadDelegate<ALC.EFX.DeleteAuxiliaryEffectSlotsArrayDelegate>("alDeleteAuxiliaryEffectSlots");

			// Token: 0x04000238 RID: 568
			private static readonly ALC.EFX.IsAuxiliaryEffectSlotDelegate _IsAuxiliaryEffectSlot = ALBase.LoadDelegate<ALC.EFX.IsAuxiliaryEffectSlotDelegate>("alIsAuxiliaryEffectSlot");

			// Token: 0x04000239 RID: 569
			private static readonly ALC.EFX.AuxiliaryEffectSlotiDelegate _AuxiliaryEffectSloti = ALBase.LoadDelegate<ALC.EFX.AuxiliaryEffectSlotiDelegate>("alAuxiliaryEffectSloti");

			// Token: 0x0400023A RID: 570
			private static readonly ALC.EFX.AuxiliaryEffectSlotbDelegate _AuxiliaryEffectSlotb = ALBase.LoadDelegate<ALC.EFX.AuxiliaryEffectSlotbDelegate>("alAuxiliaryEffectSloti");

			// Token: 0x0400023B RID: 571
			private static readonly ALC.EFX.AuxiliaryEffectSlotfDelegate _AuxiliaryEffectSlotf = ALBase.LoadDelegate<ALC.EFX.AuxiliaryEffectSlotfDelegate>("alAuxiliaryEffectSlotf");

			// Token: 0x0400023C RID: 572
			private static readonly ALC.EFX.GetAuxiliaryEffectSlotiPtrDelegate _GetAuxiliaryEffectSlotiPtr = ALBase.LoadDelegate<ALC.EFX.GetAuxiliaryEffectSlotiPtrDelegate>("alGetAuxiliaryEffectSloti");

			// Token: 0x0400023D RID: 573
			private static readonly ALC.EFX.GetAuxiliaryEffectSlotiRefDelegate _GetAuxiliaryEffectSlotiRef = ALBase.LoadDelegate<ALC.EFX.GetAuxiliaryEffectSlotiRefDelegate>("alGetAuxiliaryEffectSloti");

			// Token: 0x0400023E RID: 574
			private static readonly ALC.EFX.GetAuxiliaryEffectSlotfPtrDelegate _GetAuxiliaryEffectSlotfPtr = ALBase.LoadDelegate<ALC.EFX.GetAuxiliaryEffectSlotfPtrDelegate>("alGetAuxiliaryEffectSlotf");

			// Token: 0x0400023F RID: 575
			private static readonly ALC.EFX.GetAuxiliaryEffectSlotfRefDelegate _GetAuxiliaryEffectSlotfRef = ALBase.LoadDelegate<ALC.EFX.GetAuxiliaryEffectSlotfRefDelegate>("alGetAuxiliaryEffectSlotf");

			// Token: 0x04000240 RID: 576
			private static readonly ALC.EFX.GenEffectsPtrDelegate _GenEffectsPtr = ALBase.LoadDelegate<ALC.EFX.GenEffectsPtrDelegate>("alGenEffects");

			// Token: 0x04000241 RID: 577
			private static readonly ALC.EFX.GenEffectsRefDelegate _GenEffectsRef = ALBase.LoadDelegate<ALC.EFX.GenEffectsRefDelegate>("alGenEffects");

			// Token: 0x04000242 RID: 578
			private static readonly ALC.EFX.GenEffectsArrayDelegate _GenEffectsArray = ALBase.LoadDelegate<ALC.EFX.GenEffectsArrayDelegate>("alGenEffects");

			// Token: 0x04000243 RID: 579
			private static readonly ALC.EFX.DeleteEffectsPtrDelegate _DeleteEffectsPtr = ALBase.LoadDelegate<ALC.EFX.DeleteEffectsPtrDelegate>("alDeleteEffects");

			// Token: 0x04000244 RID: 580
			private static readonly ALC.EFX.DeleteEffectsRefDelegate _DeleteEffectsRef = ALBase.LoadDelegate<ALC.EFX.DeleteEffectsRefDelegate>("alDeleteEffects");

			// Token: 0x04000245 RID: 581
			private static readonly ALC.EFX.DeleteEffectsArrayDelegate _DeleteEffectsArray = ALBase.LoadDelegate<ALC.EFX.DeleteEffectsArrayDelegate>("alDeleteEffects");

			// Token: 0x04000246 RID: 582
			private static readonly ALC.EFX.IsEffectDelegate _IsEffect = ALBase.LoadDelegate<ALC.EFX.IsEffectDelegate>("alIsEffect");

			// Token: 0x04000247 RID: 583
			private static readonly ALC.EFX.EffectiDelegate _Effecti = ALBase.LoadDelegate<ALC.EFX.EffectiDelegate>("alEffecti");

			// Token: 0x04000248 RID: 584
			private static readonly ALC.EFX.EffectfDelegate _Effectf = ALBase.LoadDelegate<ALC.EFX.EffectfDelegate>("alEffectf");

			// Token: 0x04000249 RID: 585
			private static readonly ALC.EFX.EffectfvPtrDelegate _EffectfvPtr = ALBase.LoadDelegate<ALC.EFX.EffectfvPtrDelegate>("alEffectfv");

			// Token: 0x0400024A RID: 586
			private static readonly ALC.EFX.EffectfvRefDelegate _EffectfvRef = ALBase.LoadDelegate<ALC.EFX.EffectfvRefDelegate>("alEffectfv");

			// Token: 0x0400024B RID: 587
			private static readonly ALC.EFX.EffectfvArrayDelegate _EffectfvArray = ALBase.LoadDelegate<ALC.EFX.EffectfvArrayDelegate>("alEffectfv");

			// Token: 0x0400024C RID: 588
			private static readonly ALC.EFX.GetEffectiPtrDelegate _GetEffectiPtr = ALBase.LoadDelegate<ALC.EFX.GetEffectiPtrDelegate>("alGetEffecti");

			// Token: 0x0400024D RID: 589
			private static readonly ALC.EFX.GetEffectiRefDelegate _GetEffectiRef = ALBase.LoadDelegate<ALC.EFX.GetEffectiRefDelegate>("alGetEffecti");

			// Token: 0x0400024E RID: 590
			private static readonly ALC.EFX.GetEffectfPtrDelegate _GetEffectfPtr = ALBase.LoadDelegate<ALC.EFX.GetEffectfPtrDelegate>("alGetEffectf");

			// Token: 0x0400024F RID: 591
			private static readonly ALC.EFX.GetEffectfRefDelegate _GetEffectfRef = ALBase.LoadDelegate<ALC.EFX.GetEffectfRefDelegate>("alGetEffectf");

			// Token: 0x04000250 RID: 592
			private static readonly ALC.EFX.GetEffectfvPtrDelegate _GetEffectfvPtr = ALBase.LoadDelegate<ALC.EFX.GetEffectfvPtrDelegate>("alGetEffectfv");

			// Token: 0x04000251 RID: 593
			private static readonly ALC.EFX.GetEffectfvRefDelegate _GetEffectfvRef = ALBase.LoadDelegate<ALC.EFX.GetEffectfvRefDelegate>("alGetEffectfv");

			// Token: 0x04000252 RID: 594
			private static readonly ALC.EFX.GenFiltersPtrDelegate _GenFiltersPtr = ALBase.LoadDelegate<ALC.EFX.GenFiltersPtrDelegate>("alGenFilters");

			// Token: 0x04000253 RID: 595
			private static readonly ALC.EFX.GenFiltersRefDelegate _GenFiltersRef = ALBase.LoadDelegate<ALC.EFX.GenFiltersRefDelegate>("alGenFilters");

			// Token: 0x04000254 RID: 596
			private static readonly ALC.EFX.GenFiltersArrayDelegate _GenFiltersArray = ALBase.LoadDelegate<ALC.EFX.GenFiltersArrayDelegate>("alGenFilters");

			// Token: 0x04000255 RID: 597
			private static readonly ALC.EFX.DeleteFiltersPtrDelegate _DeleteFiltersPtr = ALBase.LoadDelegate<ALC.EFX.DeleteFiltersPtrDelegate>("alDeleteFilters");

			// Token: 0x04000256 RID: 598
			private static readonly ALC.EFX.DeleteFiltersRefDelegate _DeleteFiltersRef = ALBase.LoadDelegate<ALC.EFX.DeleteFiltersRefDelegate>("alDeleteFilters");

			// Token: 0x04000257 RID: 599
			private static readonly ALC.EFX.DeleteFiltersArrayDelegate _DeleteFiltersArray = ALBase.LoadDelegate<ALC.EFX.DeleteFiltersArrayDelegate>("alDeleteFilters");

			// Token: 0x04000258 RID: 600
			private static readonly ALC.EFX.IsFilterDelegate _IsFilter = ALBase.LoadDelegate<ALC.EFX.IsFilterDelegate>("alIsFilter");

			// Token: 0x04000259 RID: 601
			private static readonly ALC.EFX.FilteriDelegate _Filteri = ALBase.LoadDelegate<ALC.EFX.FilteriDelegate>("alFilteri");

			// Token: 0x0400025A RID: 602
			private static readonly ALC.EFX.FilterfDelegate _Filterf = ALBase.LoadDelegate<ALC.EFX.FilterfDelegate>("alFilterf");

			// Token: 0x0400025B RID: 603
			private static readonly ALC.EFX.GetFilteriPtrDelegate _GetFilteriPtr = ALBase.LoadDelegate<ALC.EFX.GetFilteriPtrDelegate>("alGetFilteri");

			// Token: 0x0400025C RID: 604
			private static readonly ALC.EFX.GetFilteriRefDelegate _GetFilteriRef = ALBase.LoadDelegate<ALC.EFX.GetFilteriRefDelegate>("alGetFilteri");

			// Token: 0x0400025D RID: 605
			private static readonly ALC.EFX.GetFilterfPtrDelegate _GetFilterfPtr = ALBase.LoadDelegate<ALC.EFX.GetFilterfPtrDelegate>("alGetFilterf");

			// Token: 0x0400025E RID: 606
			private static readonly ALC.EFX.GetFilterfRefDelegate _GetFilterfRef = ALBase.LoadDelegate<ALC.EFX.GetFilterfRefDelegate>("alGetFilterf");

			// Token: 0x0400025F RID: 607
			private static readonly ALC.EFX.SourceiDelegate _Sourcei = ALBase.LoadDelegate<ALC.EFX.SourceiDelegate>("alSourcei");

			// Token: 0x04000260 RID: 608
			private static readonly ALC.EFX.SourceDelegate _Source = ALBase.LoadDelegate<ALC.EFX.SourceDelegate>("alSourcei");

			// Token: 0x04000261 RID: 609
			private static readonly ALC.EFX.SourcebDelegate _Sourceb = ALBase.LoadDelegate<ALC.EFX.SourcebDelegate>("alSourcei");

			// Token: 0x04000262 RID: 610
			private static readonly ALC.EFX.SourceivPtrDelegate _SourceivPtr = ALBase.LoadDelegate<ALC.EFX.SourceivPtrDelegate>("alSourceiv");

			// Token: 0x04000263 RID: 611
			private static readonly ALC.EFX.SourceivRefDelegate _SourceivRef = ALBase.LoadDelegate<ALC.EFX.SourceivRefDelegate>("alSourceiv");

			// Token: 0x04000264 RID: 612
			private static readonly ALC.EFX.SourceivArrayDelegate _SourceivArray = ALBase.LoadDelegate<ALC.EFX.SourceivArrayDelegate>("alSourceiv");

			// Token: 0x04000265 RID: 613
			private static readonly ALC.EFX.Source3iDelegate _Source3i = ALBase.LoadDelegate<ALC.EFX.Source3iDelegate>("alSource3i");

			// Token: 0x04000266 RID: 614
			private static readonly ALC.EFX.GetSourceiPtrDelegate _GetSourceiPtr = ALBase.LoadDelegate<ALC.EFX.GetSourceiPtrDelegate>("alGetSourcei");

			// Token: 0x04000267 RID: 615
			private static readonly ALC.EFX.GetSourceiRefDelegate _GetSourceiRef = ALBase.LoadDelegate<ALC.EFX.GetSourceiRefDelegate>("alGetSourcei");

			// Token: 0x04000268 RID: 616
			private static readonly ALC.EFX.GetSourcefPtrDelegate _GetSourcefPtr = ALBase.LoadDelegate<ALC.EFX.GetSourcefPtrDelegate>("alGetSourcef");

			// Token: 0x04000269 RID: 617
			private static readonly ALC.EFX.GetSourcefRefDelegate _GetSourcefRef = ALBase.LoadDelegate<ALC.EFX.GetSourcefRefDelegate>("alGetSourcef");

			// Token: 0x0400026A RID: 618
			private static readonly ALC.EFX.GetSourcebPtrDelegate _GetSourcebPtr = ALBase.LoadDelegate<ALC.EFX.GetSourcebPtrDelegate>("alGetSourcei");

			// Token: 0x0400026B RID: 619
			private static readonly ALC.EFX.GetSourcebRefDelegate _GetSourcebRef = ALBase.LoadDelegate<ALC.EFX.GetSourcebRefDelegate>("alGetSourcei");

			// Token: 0x0400026C RID: 620
			private static readonly ALC.EFX.GetSourceivPtrDelegate _GetSourceivPtr = ALBase.LoadDelegate<ALC.EFX.GetSourceivPtrDelegate>("alGetSourceiv");

			// Token: 0x0400026D RID: 621
			private static readonly ALC.EFX.GetSourceivRefDelegate _GetSourceivRef = ALBase.LoadDelegate<ALC.EFX.GetSourceivRefDelegate>("alGetSourceiv");

			// Token: 0x0400026E RID: 622
			private static readonly ALC.EFX.GetSourceivArrayDelegate _GetSourceivArray = ALBase.LoadDelegate<ALC.EFX.GetSourceivArrayDelegate>("alGetSourceiv");

			// Token: 0x0400026F RID: 623
			private static readonly ALC.EFX.GetSource3iPtrDelegate _GetSource3iPtr = ALBase.LoadDelegate<ALC.EFX.GetSource3iPtrDelegate>("alGetSource3i");

			// Token: 0x04000270 RID: 624
			private static readonly ALC.EFX.GetSource3iRefDelegate _GetSource3iRef = ALBase.LoadDelegate<ALC.EFX.GetSource3iRefDelegate>("alGetSource3i");

			// Token: 0x04000271 RID: 625
			private static readonly ALC.EFX.ListenerfDelegate _Listenerf = ALBase.LoadDelegate<ALC.EFX.ListenerfDelegate>("alListenerf");

			// Token: 0x04000272 RID: 626
			private static readonly ALC.EFX.GetListenerfPtrDelegate _GetListenerfPtr = ALBase.LoadDelegate<ALC.EFX.GetListenerfPtrDelegate>("alGetListenerf");

			// Token: 0x04000273 RID: 627
			private static readonly ALC.EFX.GetListenerfRefDelegate _GetListenerfRef = ALBase.LoadDelegate<ALC.EFX.GetListenerfRefDelegate>("alGetListenerf");

			// Token: 0x02000056 RID: 86
			// (Invoke) Token: 0x060001C2 RID: 450
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GenAuxiliaryEffectSlotsPtrDelegate(int count, int* slots);

			// Token: 0x02000057 RID: 87
			// (Invoke) Token: 0x060001C6 RID: 454
			[UnmanagedFunctionPointer(2)]
			private delegate void GenAuxiliaryEffectSlotsRefDelegate(int count, ref int slots);

			// Token: 0x02000058 RID: 88
			// (Invoke) Token: 0x060001CA RID: 458
			[UnmanagedFunctionPointer(2)]
			private delegate void GenAuxiliaryEffectSlotsArrayDelegate(int count, int[] slots);

			// Token: 0x02000059 RID: 89
			// (Invoke) Token: 0x060001CE RID: 462
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void DeleteAuxiliaryEffectSlotsPtrDelegate(int count, int* slots);

			// Token: 0x0200005A RID: 90
			// (Invoke) Token: 0x060001D2 RID: 466
			[UnmanagedFunctionPointer(2)]
			private delegate void DeleteAuxiliaryEffectSlotsRefDelegate(int count, ref int slots);

			// Token: 0x0200005B RID: 91
			// (Invoke) Token: 0x060001D6 RID: 470
			[UnmanagedFunctionPointer(2)]
			private delegate void DeleteAuxiliaryEffectSlotsArrayDelegate(int count, int[] slots);

			// Token: 0x0200005C RID: 92
			// (Invoke) Token: 0x060001DA RID: 474
			[UnmanagedFunctionPointer(2)]
			private delegate bool IsAuxiliaryEffectSlotDelegate(int slot);

			// Token: 0x0200005D RID: 93
			// (Invoke) Token: 0x060001DE RID: 478
			[UnmanagedFunctionPointer(2)]
			private delegate void AuxiliaryEffectSlotiDelegate(int slot, EffectSlotInteger param, int value);

			// Token: 0x0200005E RID: 94
			// (Invoke) Token: 0x060001E2 RID: 482
			[UnmanagedFunctionPointer(2)]
			private delegate void AuxiliaryEffectSlotbDelegate(int slot, EffectSlotBoolean param, int value);

			// Token: 0x0200005F RID: 95
			// (Invoke) Token: 0x060001E6 RID: 486
			[UnmanagedFunctionPointer(2)]
			private delegate void AuxiliaryEffectSlotfDelegate(int slot, EffectSlotFloat param, float value);

			// Token: 0x02000060 RID: 96
			// (Invoke) Token: 0x060001EA RID: 490
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetAuxiliaryEffectSlotiPtrDelegate(int slot, EffectSlotInteger param, int* value);

			// Token: 0x02000061 RID: 97
			// (Invoke) Token: 0x060001EE RID: 494
			[UnmanagedFunctionPointer(2)]
			private delegate void GetAuxiliaryEffectSlotiRefDelegate(int slot, EffectSlotInteger param, out int value);

			// Token: 0x02000062 RID: 98
			// (Invoke) Token: 0x060001F2 RID: 498
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetAuxiliaryEffectSlotfPtrDelegate(int slot, EffectSlotFloat param, float* value);

			// Token: 0x02000063 RID: 99
			// (Invoke) Token: 0x060001F6 RID: 502
			[UnmanagedFunctionPointer(2)]
			private delegate void GetAuxiliaryEffectSlotfRefDelegate(int slot, EffectSlotFloat param, out float value);

			// Token: 0x02000064 RID: 100
			// (Invoke) Token: 0x060001FA RID: 506
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GenEffectsPtrDelegate(int count, int* effects);

			// Token: 0x02000065 RID: 101
			// (Invoke) Token: 0x060001FE RID: 510
			[UnmanagedFunctionPointer(2)]
			private delegate void GenEffectsRefDelegate(int count, ref int effects);

			// Token: 0x02000066 RID: 102
			// (Invoke) Token: 0x06000202 RID: 514
			[UnmanagedFunctionPointer(2)]
			private delegate void GenEffectsArrayDelegate(int count, int[] effects);

			// Token: 0x02000067 RID: 103
			// (Invoke) Token: 0x06000206 RID: 518
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void DeleteEffectsPtrDelegate(int count, int* effects);

			// Token: 0x02000068 RID: 104
			// (Invoke) Token: 0x0600020A RID: 522
			[UnmanagedFunctionPointer(2)]
			private delegate void DeleteEffectsRefDelegate(int count, ref int effects);

			// Token: 0x02000069 RID: 105
			// (Invoke) Token: 0x0600020E RID: 526
			[UnmanagedFunctionPointer(2)]
			private delegate void DeleteEffectsArrayDelegate(int count, int[] effects);

			// Token: 0x0200006A RID: 106
			// (Invoke) Token: 0x06000212 RID: 530
			[UnmanagedFunctionPointer(2)]
			private delegate bool IsEffectDelegate(int effect);

			// Token: 0x0200006B RID: 107
			// (Invoke) Token: 0x06000216 RID: 534
			[UnmanagedFunctionPointer(2)]
			private delegate void EffectiDelegate(int effect, EffectInteger param, int value);

			// Token: 0x0200006C RID: 108
			// (Invoke) Token: 0x0600021A RID: 538
			[UnmanagedFunctionPointer(2)]
			private delegate void EffectfDelegate(int effect, EffectFloat param, float value);

			// Token: 0x0200006D RID: 109
			// (Invoke) Token: 0x0600021E RID: 542
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void EffectfvPtrDelegate(int effect, EffectVector3 param, float* value);

			// Token: 0x0200006E RID: 110
			// (Invoke) Token: 0x06000222 RID: 546
			[UnmanagedFunctionPointer(2)]
			private delegate void EffectfvRefDelegate(int effect, EffectVector3 param, ref float value);

			// Token: 0x0200006F RID: 111
			// (Invoke) Token: 0x06000226 RID: 550
			[UnmanagedFunctionPointer(2)]
			private delegate void EffectfvArrayDelegate(int effect, EffectVector3 param, float[] value);

			// Token: 0x02000070 RID: 112
			// (Invoke) Token: 0x0600022A RID: 554
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetEffectiPtrDelegate(int effect, EffectInteger param, int* value);

			// Token: 0x02000071 RID: 113
			// (Invoke) Token: 0x0600022E RID: 558
			[UnmanagedFunctionPointer(2)]
			private delegate void GetEffectiRefDelegate(int effect, EffectInteger param, out int value);

			// Token: 0x02000072 RID: 114
			// (Invoke) Token: 0x06000232 RID: 562
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetEffectfPtrDelegate(int effect, EffectFloat param, float* value);

			// Token: 0x02000073 RID: 115
			// (Invoke) Token: 0x06000236 RID: 566
			[UnmanagedFunctionPointer(2)]
			private delegate void GetEffectfRefDelegate(int effect, EffectFloat param, out float value);

			// Token: 0x02000074 RID: 116
			// (Invoke) Token: 0x0600023A RID: 570
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetEffectfvPtrDelegate(int effect, EffectVector3 param, float* value);

			// Token: 0x02000075 RID: 117
			// (Invoke) Token: 0x0600023E RID: 574
			[UnmanagedFunctionPointer(2)]
			private delegate void GetEffectfvRefDelegate(int effect, EffectVector3 param, out float value);

			// Token: 0x02000076 RID: 118
			// (Invoke) Token: 0x06000242 RID: 578
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GenFiltersPtrDelegate(int count, int* filters);

			// Token: 0x02000077 RID: 119
			// (Invoke) Token: 0x06000246 RID: 582
			[UnmanagedFunctionPointer(2)]
			private delegate void GenFiltersRefDelegate(int count, ref int filters);

			// Token: 0x02000078 RID: 120
			// (Invoke) Token: 0x0600024A RID: 586
			[UnmanagedFunctionPointer(2)]
			private delegate void GenFiltersArrayDelegate(int count, int[] filters);

			// Token: 0x02000079 RID: 121
			// (Invoke) Token: 0x0600024E RID: 590
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void DeleteFiltersPtrDelegate(int count, int* filters);

			// Token: 0x0200007A RID: 122
			// (Invoke) Token: 0x06000252 RID: 594
			[UnmanagedFunctionPointer(2)]
			private delegate void DeleteFiltersRefDelegate(int count, ref int filters);

			// Token: 0x0200007B RID: 123
			// (Invoke) Token: 0x06000256 RID: 598
			[UnmanagedFunctionPointer(2)]
			private delegate void DeleteFiltersArrayDelegate(int count, int[] filters);

			// Token: 0x0200007C RID: 124
			// (Invoke) Token: 0x0600025A RID: 602
			[UnmanagedFunctionPointer(2)]
			private delegate bool IsFilterDelegate(int filter);

			// Token: 0x0200007D RID: 125
			// (Invoke) Token: 0x0600025E RID: 606
			[UnmanagedFunctionPointer(2)]
			private delegate void FilteriDelegate(int filter, FilterInteger param, int value);

			// Token: 0x0200007E RID: 126
			// (Invoke) Token: 0x06000262 RID: 610
			[UnmanagedFunctionPointer(2)]
			private delegate void FilterfDelegate(int filter, FilterFloat param, float value);

			// Token: 0x0200007F RID: 127
			// (Invoke) Token: 0x06000266 RID: 614
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetFilteriPtrDelegate(int filter, FilterInteger param, int* value);

			// Token: 0x02000080 RID: 128
			// (Invoke) Token: 0x0600026A RID: 618
			[UnmanagedFunctionPointer(2)]
			private delegate void GetFilteriRefDelegate(int filter, FilterInteger param, out int value);

			// Token: 0x02000081 RID: 129
			// (Invoke) Token: 0x0600026E RID: 622
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetFilterfPtrDelegate(int filter, FilterFloat param, float* value);

			// Token: 0x02000082 RID: 130
			// (Invoke) Token: 0x06000272 RID: 626
			[UnmanagedFunctionPointer(2)]
			private delegate void GetFilterfRefDelegate(int filter, FilterFloat param, out float value);

			// Token: 0x02000083 RID: 131
			// (Invoke) Token: 0x06000276 RID: 630
			[UnmanagedFunctionPointer(2)]
			private delegate void SourceiDelegate(int source, EFXSourceInteger param, int value);

			// Token: 0x02000084 RID: 132
			// (Invoke) Token: 0x0600027A RID: 634
			[UnmanagedFunctionPointer(2)]
			private delegate void SourceDelegate(int source, EFXSourceFloat param, float value);

			// Token: 0x02000085 RID: 133
			// (Invoke) Token: 0x0600027E RID: 638
			[UnmanagedFunctionPointer(2)]
			private delegate void SourcebDelegate(int source, EFXSourceBoolean param, bool value);

			// Token: 0x02000086 RID: 134
			// (Invoke) Token: 0x06000282 RID: 642
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void SourceivPtrDelegate(int source, EFXSourceInteger3 param, int* value);

			// Token: 0x02000087 RID: 135
			// (Invoke) Token: 0x06000286 RID: 646
			[UnmanagedFunctionPointer(2)]
			private delegate void SourceivRefDelegate(int source, EFXSourceInteger3 param, ref int value);

			// Token: 0x02000088 RID: 136
			// (Invoke) Token: 0x0600028A RID: 650
			[UnmanagedFunctionPointer(2)]
			private delegate void SourceivArrayDelegate(int source, EFXSourceInteger3 param, int[] value);

			// Token: 0x02000089 RID: 137
			// (Invoke) Token: 0x0600028E RID: 654
			[UnmanagedFunctionPointer(2)]
			private delegate void Source3iDelegate(int source, EFXSourceInteger3 param, int value1, int value2, int value3);

			// Token: 0x0200008A RID: 138
			// (Invoke) Token: 0x06000292 RID: 658
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetSourceiPtrDelegate(int source, EFXSourceInteger param, int* value);

			// Token: 0x0200008B RID: 139
			// (Invoke) Token: 0x06000296 RID: 662
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourceiRefDelegate(int source, EFXSourceInteger param, out int value);

			// Token: 0x0200008C RID: 140
			// (Invoke) Token: 0x0600029A RID: 666
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetSourcefPtrDelegate(int source, EFXSourceFloat param, float* value);

			// Token: 0x0200008D RID: 141
			// (Invoke) Token: 0x0600029E RID: 670
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourcefRefDelegate(int source, EFXSourceFloat param, out float value);

			// Token: 0x0200008E RID: 142
			// (Invoke) Token: 0x060002A2 RID: 674
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetSourcebPtrDelegate(int source, EFXSourceBoolean param, bool* value);

			// Token: 0x0200008F RID: 143
			// (Invoke) Token: 0x060002A6 RID: 678
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourcebRefDelegate(int source, EFXSourceBoolean param, out bool value);

			// Token: 0x02000090 RID: 144
			// (Invoke) Token: 0x060002AA RID: 682
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetSourceivPtrDelegate(int source, EFXSourceInteger3 param, int* value);

			// Token: 0x02000091 RID: 145
			// (Invoke) Token: 0x060002AE RID: 686
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourceivRefDelegate(int source, EFXSourceInteger3 param, ref int value);

			// Token: 0x02000092 RID: 146
			// (Invoke) Token: 0x060002B2 RID: 690
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourceivArrayDelegate(int source, EFXSourceInteger3 param, int[] value);

			// Token: 0x02000093 RID: 147
			// (Invoke) Token: 0x060002B6 RID: 694
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetSource3iPtrDelegate(int source, EFXSourceInteger3 param, int* value1, int* value2, int* value3);

			// Token: 0x02000094 RID: 148
			// (Invoke) Token: 0x060002BA RID: 698
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSource3iRefDelegate(int source, EFXSourceInteger3 param, out int value1, out int value2, out int value3);

			// Token: 0x02000095 RID: 149
			// (Invoke) Token: 0x060002BE RID: 702
			[UnmanagedFunctionPointer(2)]
			private delegate void ListenerfDelegate(int listener, EFXListenerFloat param, float value);

			// Token: 0x02000096 RID: 150
			// (Invoke) Token: 0x060002C2 RID: 706
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetListenerfPtrDelegate(int listener, EFXListenerFloat param, float* value);

			// Token: 0x02000097 RID: 151
			// (Invoke) Token: 0x060002C6 RID: 710
			[UnmanagedFunctionPointer(2)]
			private delegate void GetListenerfRefDelegate(int listener, EFXListenerFloat param, out float value);
		}

		// Token: 0x0200004C RID: 76
		public class EnumerateAll : ALBase
		{
			// Token: 0x06000178 RID: 376 RVA: 0x00007F0B File Offset: 0x0000610B
			static EnumerateAll()
			{
				ALBase.RegisterOpenALResolver();
			}

			// Token: 0x06000179 RID: 377 RVA: 0x00007F12 File Offset: 0x00006112
			private EnumerateAll()
			{
			}

			// Token: 0x0600017A RID: 378 RVA: 0x00007F1A File Offset: 0x0000611A
			public static bool IsExtensionPresent()
			{
				return ALC.IsExtensionPresent(ALDevice.Null, "ALC_ENUMERATE_ALL_EXT");
			}

			// Token: 0x0600017B RID: 379 RVA: 0x00007F2B File Offset: 0x0000612B
			public static bool IsExtensionPresent(ALDevice device)
			{
				return ALC.IsExtensionPresent(device, "ALC_ENUMERATE_ALL_EXT");
			}

			// Token: 0x0600017C RID: 380
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alcGetString", ExactSpelling = true)]
			public static extern string GetString(ALDevice device, GetEnumerateAllContextString param);

			// Token: 0x0600017D RID: 381
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alcGetString", ExactSpelling = true)]
			public unsafe static extern byte* GetStringList(ALDevice device, GetEnumerateAllContextStringList param);

			// Token: 0x0600017E RID: 382 RVA: 0x00007F38 File Offset: 0x00006138
			public static IEnumerable<string> GetStringList(GetEnumerateAllContextStringList param)
			{
				return ALC.ALStringListToList(ALC.EnumerateAll.GetStringList(ALDevice.Null, param));
			}

			// Token: 0x04000274 RID: 628
			public const string ExtensionName = "ALC_ENUMERATE_ALL_EXT";
		}

		// Token: 0x0200004D RID: 77
		public class DeviceClock : ALBase
		{
			// Token: 0x0600017F RID: 383 RVA: 0x00007F4C File Offset: 0x0000614C
			static DeviceClock()
			{
				ALBase.RegisterOpenALResolver();
			}

			// Token: 0x06000180 RID: 384 RVA: 0x00007FE5 File Offset: 0x000061E5
			private DeviceClock()
			{
			}

			// Token: 0x06000181 RID: 385 RVA: 0x00007FED File Offset: 0x000061ED
			public static bool IsExtensionPresent(ALDevice device)
			{
				return ALC.IsExtensionPresent(device, "ALC_SOFT_device_clock");
			}

			// Token: 0x06000182 RID: 386 RVA: 0x00007FFA File Offset: 0x000061FA
			public unsafe static void GetInteger(ALDevice device, GetInteger64 param, int size, long* values)
			{
				ALC.DeviceClock._GetIntegerPtr(device, param, size, values);
			}

			// Token: 0x06000183 RID: 387 RVA: 0x0000800A File Offset: 0x0000620A
			private static void GetInteger(ALDevice device, GetInteger64 param, int size, ref long values)
			{
				ALC.DeviceClock._GetIntegerRef(device, param, size, ref values);
			}

			// Token: 0x06000184 RID: 388 RVA: 0x0000801A File Offset: 0x0000621A
			public static void GetInteger(ALDevice device, GetInteger64 param, int size, long[] values)
			{
				ALC.DeviceClock._GetIntegerArray(device, param, size, values);
			}

			// Token: 0x06000185 RID: 389 RVA: 0x0000802A File Offset: 0x0000622A
			public unsafe static void GetSource(int source, SourceInteger64 param, long* values)
			{
				ALC.DeviceClock._GetSourcei64vPtr(source, param, values);
			}

			// Token: 0x06000186 RID: 390 RVA: 0x00008039 File Offset: 0x00006239
			private static void GetSource(int source, SourceInteger64 param, ref long values)
			{
				ALC.DeviceClock._GetSourcei64vRef(source, param, ref values);
			}

			// Token: 0x06000187 RID: 391 RVA: 0x00008048 File Offset: 0x00006248
			public static void GetSource(int source, SourceInteger64 param, long[] values)
			{
				ALC.DeviceClock._GetSourcei64vArray(source, param, values);
			}

			// Token: 0x06000188 RID: 392 RVA: 0x00008057 File Offset: 0x00006257
			public unsafe static void GetSource(int source, SourceDouble param, double* values)
			{
				ALC.DeviceClock._GetSourcedvPtr(source, param, values);
			}

			// Token: 0x06000189 RID: 393 RVA: 0x00008066 File Offset: 0x00006266
			private static void GetSource(int source, SourceDouble param, ref double values)
			{
				ALC.DeviceClock._GetSourcedvRef(source, param, ref values);
			}

			// Token: 0x0600018A RID: 394 RVA: 0x00008075 File Offset: 0x00006275
			public static void GetSource(int source, SourceDouble param, double[] values)
			{
				ALC.DeviceClock._GetSourcedvArray(source, param, values);
			}

			// Token: 0x0600018B RID: 395 RVA: 0x00008084 File Offset: 0x00006284
			public static void GetInteger(ALDevice device, GetInteger64 param, long[] values)
			{
				ALC.DeviceClock.GetInteger(device, param, values.Length, values);
			}

			// Token: 0x0600018C RID: 396 RVA: 0x00008091 File Offset: 0x00006291
			public static void GetInteger(ALDevice device, GetInteger64 param, Span<long> values)
			{
				ALC.DeviceClock.GetInteger(device, param, values.Length, values[0]);
			}

			// Token: 0x0600018D RID: 397 RVA: 0x000080A9 File Offset: 0x000062A9
			public static void GetSource(int source, SourceInteger64 param, Span<long> values)
			{
				ALC.DeviceClock.GetSource(source, param, values[0]);
			}

			// Token: 0x0600018E RID: 398 RVA: 0x000080BC File Offset: 0x000062BC
			public unsafe static void GetSource(int source, SourceInteger64 param, out int value1, out int value2, out long value3)
			{
				int* ptr = stackalloc int[(UIntPtr)16];
				ALC.DeviceClock.GetSource(source, param, (long*)ptr);
				value1 = *ptr;
				value2 = ptr[1];
				value3 = *(long*)(ptr + (IntPtr)2 * 8 / 4);
			}

			// Token: 0x0600018F RID: 399 RVA: 0x000080EB File Offset: 0x000062EB
			public static void GetSource(int source, SourceDouble param, Span<double> values)
			{
				ALC.DeviceClock.GetSource(source, param, values[0]);
			}

			// Token: 0x06000190 RID: 400 RVA: 0x000080FC File Offset: 0x000062FC
			public unsafe static void GetSource(int source, SourceDouble param, out double value1, out double value2)
			{
				Span<double> values = new Span<double>(stackalloc byte[(UIntPtr)16], 2);
				ALC.DeviceClock.GetSource(source, param, values);
				value1 = *values[0];
				value2 = *values[1];
			}

			// Token: 0x04000275 RID: 629
			public const string ExtensionName = "ALC_SOFT_device_clock";

			// Token: 0x04000276 RID: 630
			private static readonly ALC.DeviceClock.GetIntegerPtrDelegate _GetIntegerPtr = ALBase.LoadDelegate<ALC.DeviceClock.GetIntegerPtrDelegate>("alcGetInteger64vSOFT");

			// Token: 0x04000277 RID: 631
			private static readonly ALC.DeviceClock.GetIntegerRefDelegate _GetIntegerRef = ALBase.LoadDelegate<ALC.DeviceClock.GetIntegerRefDelegate>("alcGetInteger64vSOFT");

			// Token: 0x04000278 RID: 632
			private static readonly ALC.DeviceClock.GetIntegerArrayDelegate _GetIntegerArray = ALBase.LoadDelegate<ALC.DeviceClock.GetIntegerArrayDelegate>("alcGetInteger64vSOFT");

			// Token: 0x04000279 RID: 633
			private static readonly ALC.DeviceClock.GetSourcei64vPtrDelegate _GetSourcei64vPtr = ALBase.LoadDelegate<ALC.DeviceClock.GetSourcei64vPtrDelegate>("alGetSourcei64vSOFT");

			// Token: 0x0400027A RID: 634
			private static readonly ALC.DeviceClock.GetSourcei64vRefDelegate _GetSourcei64vRef = ALBase.LoadDelegate<ALC.DeviceClock.GetSourcei64vRefDelegate>("alGetSourcei64vSOFT");

			// Token: 0x0400027B RID: 635
			private static readonly ALC.DeviceClock.GetSourcei64vArrayDelegate _GetSourcei64vArray = ALBase.LoadDelegate<ALC.DeviceClock.GetSourcei64vArrayDelegate>("alGetSourcei64vSOFT");

			// Token: 0x0400027C RID: 636
			private static readonly ALC.DeviceClock.GetSourcedvPtrDelegate _GetSourcedvPtr = ALBase.LoadDelegate<ALC.DeviceClock.GetSourcedvPtrDelegate>("alGetSourcedvSOFT");

			// Token: 0x0400027D RID: 637
			private static readonly ALC.DeviceClock.GetSourcedvRefDelegate _GetSourcedvRef = ALBase.LoadDelegate<ALC.DeviceClock.GetSourcedvRefDelegate>("alGetSourcedvSOFT");

			// Token: 0x0400027E RID: 638
			private static readonly ALC.DeviceClock.GetSourcedvArrayDelegate _GetSourcedvArray = ALBase.LoadDelegate<ALC.DeviceClock.GetSourcedvArrayDelegate>("alGetSourcedvSOFT");

			// Token: 0x02000098 RID: 152
			// (Invoke) Token: 0x060002CA RID: 714
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetIntegerPtrDelegate(ALDevice device, GetInteger64 param, int size, long* values);

			// Token: 0x02000099 RID: 153
			// (Invoke) Token: 0x060002CE RID: 718
			[UnmanagedFunctionPointer(2)]
			private delegate void GetIntegerRefDelegate(ALDevice device, GetInteger64 param, int size, ref long values);

			// Token: 0x0200009A RID: 154
			// (Invoke) Token: 0x060002D2 RID: 722
			[UnmanagedFunctionPointer(2)]
			private delegate void GetIntegerArrayDelegate(ALDevice device, GetInteger64 param, int size, long[] values);

			// Token: 0x0200009B RID: 155
			// (Invoke) Token: 0x060002D6 RID: 726
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetSourcei64vPtrDelegate(int source, SourceInteger64 param, long* values);

			// Token: 0x0200009C RID: 156
			// (Invoke) Token: 0x060002DA RID: 730
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourcei64vRefDelegate(int source, SourceInteger64 param, ref long values);

			// Token: 0x0200009D RID: 157
			// (Invoke) Token: 0x060002DE RID: 734
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourcei64vArrayDelegate(int source, SourceInteger64 param, long[] values);

			// Token: 0x0200009E RID: 158
			// (Invoke) Token: 0x060002E2 RID: 738
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetSourcedvPtrDelegate(int source, SourceDouble param, double* values);

			// Token: 0x0200009F RID: 159
			// (Invoke) Token: 0x060002E6 RID: 742
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourcedvRefDelegate(int source, SourceDouble param, ref double values);

			// Token: 0x020000A0 RID: 160
			// (Invoke) Token: 0x060002EA RID: 746
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourcedvArrayDelegate(int source, SourceDouble param, double[] values);
		}

		// Token: 0x0200004E RID: 78
		public class Loopback
		{
			// Token: 0x06000191 RID: 401 RVA: 0x00008134 File Offset: 0x00006334
			static Loopback()
			{
				ALBase.RegisterOpenALResolver();
			}

			// Token: 0x06000192 RID: 402 RVA: 0x00008191 File Offset: 0x00006391
			private Loopback()
			{
			}

			// Token: 0x06000193 RID: 403 RVA: 0x00008199 File Offset: 0x00006399
			public static bool IsExtensionPresent(ALDevice device)
			{
				return ALC.IsExtensionPresent(device, "ALC_SOFT_loopback");
			}

			// Token: 0x06000194 RID: 404 RVA: 0x000081A6 File Offset: 0x000063A6
			public static ALDevice LoopbackOpenDeviceSOFT(string device)
			{
				return ALC.Loopback._LoopbackOpenDeviceSOFT(device);
			}

			// Token: 0x06000195 RID: 405 RVA: 0x000081B3 File Offset: 0x000063B3
			public static bool IsRenderFormatSupportedSOFT(ALDevice device, int frequency, Channels channels, RenderFormat type)
			{
				return ALC.Loopback._IsRenderFormatSupportedSOFT(device, frequency, channels, type);
			}

			// Token: 0x06000196 RID: 406 RVA: 0x000081C3 File Offset: 0x000063C3
			public unsafe static void RenderSamplesSOFT(ALDevice device, byte* buffer, int samples)
			{
				ALC.Loopback._RenderSamplesSOFTPtr(device, buffer, samples);
			}

			// Token: 0x06000197 RID: 407 RVA: 0x000081D2 File Offset: 0x000063D2
			public static void RenderSamplesSOFT(ALDevice device, ref byte buffer, int samples)
			{
				ALC.Loopback._RenderSamplesSOFTRef(device, ref buffer, samples);
			}

			// Token: 0x06000198 RID: 408 RVA: 0x000081E1 File Offset: 0x000063E1
			public static void RenderSamplesSOFT(ALDevice device, byte[] buffer, int samples)
			{
				ALC.Loopback._RenderSamplesSOFTArray(device, buffer, samples);
			}

			// Token: 0x06000199 RID: 409 RVA: 0x000081F0 File Offset: 0x000063F0
			public unsafe static void RenderSamplesSOFT<[IsUnmanaged] TBuffer>(ALDevice device, TBuffer[] buffer, int samples) where TBuffer : struct, ValueType
			{
				fixed (TBuffer[] array = buffer)
				{
					TBuffer* buffer2;
					if (buffer == null || array.Length == 0)
					{
						buffer2 = null;
					}
					else
					{
						buffer2 = &array[0];
					}
					ALC.Loopback.RenderSamplesSOFT(device, (byte*)buffer2, samples);
				}
			}

			// Token: 0x0400027F RID: 639
			public const string ExtensionName = "ALC_SOFT_loopback";

			// Token: 0x04000280 RID: 640
			private static readonly ALC.Loopback.LoopbackOpenDeviceSOFTDelegate _LoopbackOpenDeviceSOFT = ALBase.LoadDelegate<ALC.Loopback.LoopbackOpenDeviceSOFTDelegate>("alcLoopbackOpenDeviceSOFT");

			// Token: 0x04000281 RID: 641
			private static readonly ALC.Loopback.IsRenderFormatSupportedSOFTDelegate _IsRenderFormatSupportedSOFT = ALBase.LoadDelegate<ALC.Loopback.IsRenderFormatSupportedSOFTDelegate>("alcIsRenderFormatSupportedSOFT");

			// Token: 0x04000282 RID: 642
			private static readonly ALC.Loopback.RenderSamplesSOFTPtrDelegate _RenderSamplesSOFTPtr = ALBase.LoadDelegate<ALC.Loopback.RenderSamplesSOFTPtrDelegate>("alcLoopbackOpenDeviceSOFT");

			// Token: 0x04000283 RID: 643
			private static readonly ALC.Loopback.RenderSamplesSOFTRefDelegate _RenderSamplesSOFTRef = ALBase.LoadDelegate<ALC.Loopback.RenderSamplesSOFTRefDelegate>("alcLoopbackOpenDeviceSOFT");

			// Token: 0x04000284 RID: 644
			private static readonly ALC.Loopback.RenderSamplesSOFTArrayDelegate _RenderSamplesSOFTArray = ALBase.LoadDelegate<ALC.Loopback.RenderSamplesSOFTArrayDelegate>("alcLoopbackOpenDeviceSOFT");

			// Token: 0x020000A1 RID: 161
			// (Invoke) Token: 0x060002EE RID: 750
			[UnmanagedFunctionPointer(2, CharSet = 2)]
			private delegate ALDevice LoopbackOpenDeviceSOFTDelegate(string device);

			// Token: 0x020000A2 RID: 162
			// (Invoke) Token: 0x060002F2 RID: 754
			[UnmanagedFunctionPointer(2)]
			private delegate bool IsRenderFormatSupportedSOFTDelegate(ALDevice device, int frequency, Channels channels, RenderFormat type);

			// Token: 0x020000A3 RID: 163
			// (Invoke) Token: 0x060002F6 RID: 758
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void RenderSamplesSOFTPtrDelegate(ALDevice device, byte* buffer, int samples);

			// Token: 0x020000A4 RID: 164
			// (Invoke) Token: 0x060002FA RID: 762
			[UnmanagedFunctionPointer(2)]
			private delegate void RenderSamplesSOFTRefDelegate(ALDevice device, ref byte buffer, int samples);

			// Token: 0x020000A5 RID: 165
			// (Invoke) Token: 0x060002FE RID: 766
			[UnmanagedFunctionPointer(2)]
			private delegate void RenderSamplesSOFTArrayDelegate(ALDevice device, byte[] buffer, int samples);
		}
	}
}
