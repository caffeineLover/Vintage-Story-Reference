using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x02000011 RID: 17
	public class AL : ALBase
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00002789 File Offset: 0x00000989
		static AL()
		{
			ALBase.RegisterOpenALResolver();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002790 File Offset: 0x00000990
		private AL()
		{
		}

		// Token: 0x06000067 RID: 103
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alEnable", ExactSpelling = true)]
		public static extern void Enable(ALCapability capability);

		// Token: 0x06000068 RID: 104
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alDisable", ExactSpelling = true)]
		public static extern void Disable(ALCapability capability);

		// Token: 0x06000069 RID: 105
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alIsEnabled", ExactSpelling = true)]
		public static extern bool IsEnabled(ALCapability capability);

		// Token: 0x0600006A RID: 106
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alGetString", ExactSpelling = true)]
		[return: MarshalAs(44, MarshalTypeRef = OpenTK.Audio.OpenAL.Native.ConstCharPtrMarshaler)]
		public static extern string Get(ALGetString param);

		// Token: 0x0600006B RID: 107 RVA: 0x00002798 File Offset: 0x00000998
		public static string GetErrorString(ALError param)
		{
			return AL.Get((ALGetString)param);
		}

		// Token: 0x0600006C RID: 108
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetInteger", ExactSpelling = true)]
		public static extern int Get(ALGetInteger param);

		// Token: 0x0600006D RID: 109
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetFloat", ExactSpelling = true)]
		public static extern float Get(ALGetFloat param);

		// Token: 0x0600006E RID: 110
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetError", ExactSpelling = true)]
		public static extern ALError GetError();

		// Token: 0x0600006F RID: 111
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alIsExtensionPresent", ExactSpelling = true)]
		public static extern bool IsExtensionPresent([In] string extname);

		// Token: 0x06000070 RID: 112
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alGetProcAddress", ExactSpelling = true)]
		public static extern IntPtr GetProcAddress([In] string fname);

		// Token: 0x06000071 RID: 113
		[DllImport("AL", CallingConvention = 2, CharSet = 2, EntryPoint = "alGetEnumValue", ExactSpelling = true)]
		public static extern int GetEnumValue([In] string ename);

		// Token: 0x06000072 RID: 114
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alListenerf", ExactSpelling = true)]
		public static extern void Listener(ALListenerf param, float value);

		// Token: 0x06000073 RID: 115
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alListener3f", ExactSpelling = true)]
		public static extern void Listener(ALListener3f param, float value1, float value2, float value3);

		// Token: 0x06000074 RID: 116 RVA: 0x000027A0 File Offset: 0x000009A0
		public static void Listener(ALListener3f param, ref Vector3 values)
		{
			AL.Listener(param, values.X, values.Y, values.Z);
		}

		// Token: 0x06000075 RID: 117
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alListenerfv", ExactSpelling = true)]
		public unsafe static extern void Listener(ALListenerfv param, float* values);

		// Token: 0x06000076 RID: 118
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alListenerfv", ExactSpelling = true)]
		public static extern void Listener(ALListenerfv param, ref float values);

		// Token: 0x06000077 RID: 119
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alListenerfv", ExactSpelling = true)]
		public static extern void Listener(ALListenerfv param, float[] values);

		// Token: 0x06000078 RID: 120 RVA: 0x000027BC File Offset: 0x000009BC
		public unsafe static void Listener(ALListenerfv param, ref Vector3 at, ref Vector3 up)
		{
			Span<float> span = new Span<float>(stackalloc byte[(UIntPtr)24], 6);
			*span[0] = at.X;
			*span[1] = at.Y;
			*span[2] = at.Z;
			*span[3] = up.X;
			*span[4] = up.Y;
			*span[5] = up.Z;
			AL.Listener(param, span[0]);
		}

		// Token: 0x06000079 RID: 121
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetListenerf", ExactSpelling = true)]
		public static extern void GetListener(ALListenerf param, out float value);

		// Token: 0x0600007A RID: 122 RVA: 0x00002840 File Offset: 0x00000A40
		public static float GetListener(ALListenerf param)
		{
			float result;
			AL.GetListener(param, out result);
			return result;
		}

		// Token: 0x0600007B RID: 123
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetListener3f", ExactSpelling = true)]
		public static extern void GetListener(ALListener3f param, out float value1, out float value2, out float value3);

		// Token: 0x0600007C RID: 124 RVA: 0x00002856 File Offset: 0x00000A56
		public static void GetListener(ALListener3f param, out Vector3 values)
		{
			AL.GetListener(param, out values.X, out values.Y, out values.Z);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002870 File Offset: 0x00000A70
		public static Vector3 GetListener(ALListener3f param)
		{
			Vector3 result;
			AL.GetListener(param, out result);
			return result;
		}

		// Token: 0x0600007E RID: 126
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetListenerfv", ExactSpelling = true)]
		public unsafe static extern void GetListener(ALListenerfv param, float* values);

		// Token: 0x0600007F RID: 127
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetListenerfv", ExactSpelling = true)]
		public static extern void GetListener(ALListenerfv param, ref float values);

		// Token: 0x06000080 RID: 128
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetListenerfv", ExactSpelling = true)]
		public static extern void GetListener(ALListenerfv param, [In] float[] values);

		// Token: 0x06000081 RID: 129 RVA: 0x00002888 File Offset: 0x00000A88
		public unsafe static void GetListener(ALListenerfv param, out Vector3 at, out Vector3 up)
		{
			Span<float> span = new Span<float>(stackalloc byte[(UIntPtr)24], 6);
			AL.GetListener(param, span[0]);
			at.X = *span[0];
			at.Y = *span[1];
			at.Z = *span[2];
			up.X = *span[3];
			up.Y = *span[4];
			up.Z = *span[5];
		}

		// Token: 0x06000082 RID: 130
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGenSources", ExactSpelling = true)]
		public unsafe static extern void GenSources(int n, [In] int* sources);

		// Token: 0x06000083 RID: 131
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGenSources", ExactSpelling = true)]
		public static extern void GenSources(int n, ref int sources);

		// Token: 0x06000084 RID: 132
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGenSources", ExactSpelling = true)]
		public static extern void GenSources(int n, int[] sources);

		// Token: 0x06000085 RID: 133 RVA: 0x00002909 File Offset: 0x00000B09
		public static void GenSources(int[] sources)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			AL.GenSources(sources.Length, sources);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002922 File Offset: 0x00000B22
		public static void GenSources(Span<int> sources)
		{
			AL.GenSources(sources.Length, sources[0]);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002938 File Offset: 0x00000B38
		public static int[] GenSources(int n)
		{
			int[] array = new int[n];
			AL.GenSources(n, array);
			return array;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002954 File Offset: 0x00000B54
		public static int GenSource()
		{
			int result = 0;
			AL.GenSources(1, ref result);
			return result;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000296C File Offset: 0x00000B6C
		public static void GenSource(out int source)
		{
			int num = 0;
			AL.GenSources(1, ref num);
			source = num;
		}

		// Token: 0x0600008A RID: 138
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alDeleteSources", ExactSpelling = true)]
		public unsafe static extern void DeleteSources(int n, [In] int* sources);

		// Token: 0x0600008B RID: 139
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alDeleteSources", ExactSpelling = true)]
		public static extern void DeleteSources(int n, ref int sources);

		// Token: 0x0600008C RID: 140 RVA: 0x00002986 File Offset: 0x00000B86
		public static void DeleteSources(int[] sources)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			AL.DeleteSources(sources.Length, ref sources[0]);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000029A5 File Offset: 0x00000BA5
		public static void DeleteSources(ReadOnlySpan<int> sources)
		{
			AL.DeleteSources(sources.Length, MemoryMarshal.GetReference<int>(sources));
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000029B9 File Offset: 0x00000BB9
		public static void DeleteSource(int source)
		{
			AL.DeleteSources(1, ref source);
		}

		// Token: 0x0600008F RID: 143
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alIsSource", ExactSpelling = true)]
		public static extern bool IsSource(int sid);

		// Token: 0x06000090 RID: 144
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcef", ExactSpelling = true)]
		public static extern void Source(int sid, ALSourcef param, float value);

		// Token: 0x06000091 RID: 145
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSource3f", ExactSpelling = true)]
		public static extern void Source(int sid, ALSource3f param, float value1, float value2, float value3);

		// Token: 0x06000092 RID: 146 RVA: 0x000029C3 File Offset: 0x00000BC3
		public static void Source(int sid, ALSource3f param, ref Vector3 values)
		{
			AL.Source(sid, param, values.X, values.Y, values.Z);
		}

		// Token: 0x06000093 RID: 147
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcei", ExactSpelling = true)]
		public static extern void Source(int sid, ALSourcei param, int value);

		// Token: 0x06000094 RID: 148
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcei", ExactSpelling = true)]
		public static extern void Source(int sid, ALSourceb param, bool value);

		// Token: 0x06000095 RID: 149
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSource3i", ExactSpelling = true)]
		public static extern void Source(int sid, ALSource3i param, int value1, int value2, int value3);

		// Token: 0x06000096 RID: 150
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetSourcef", ExactSpelling = true)]
		public static extern void GetSource(int sid, ALSourcef param, out float value);

		// Token: 0x06000097 RID: 151 RVA: 0x000029E0 File Offset: 0x00000BE0
		public static float GetSource(int sid, ALSourcef param)
		{
			float result;
			AL.GetSource(sid, param, out result);
			return result;
		}

		// Token: 0x06000098 RID: 152
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetSource3f", ExactSpelling = true)]
		public static extern void GetSource(int sid, ALSource3f param, out float value1, out float value2, out float value3);

		// Token: 0x06000099 RID: 153 RVA: 0x000029F7 File Offset: 0x00000BF7
		public static void GetSource(int sid, ALSource3f param, out Vector3 values)
		{
			AL.GetSource(sid, param, out values.X, out values.Y, out values.Z);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002A14 File Offset: 0x00000C14
		public static Vector3 GetSource(int sid, ALSource3f param)
		{
			Vector3 result;
			AL.GetSource(sid, param, out result.X, out result.Y, out result.Z);
			return result;
		}

		// Token: 0x0600009B RID: 155
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetSource3i", ExactSpelling = true)]
		public static extern void GetSource(int sid, ALSource3i param, out int value1, out int value2, out int value3);

		// Token: 0x0600009C RID: 156 RVA: 0x00002A3E File Offset: 0x00000C3E
		public static void GetSource(int sid, ALSource3i param, out Vector3i values)
		{
			AL.GetSource(sid, param, out values.X, out values.Y, out values.Z);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00002A5C File Offset: 0x00000C5C
		public static Vector3i GetSource(int sid, ALSource3i param)
		{
			Vector3i result;
			AL.GetSource(sid, param, out result.X, out result.Y, out result.Z);
			return result;
		}

		// Token: 0x0600009E RID: 158
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetSourcei", ExactSpelling = true)]
		public static extern void GetSource(int sid, ALGetSourcei param, out int value);

		// Token: 0x0600009F RID: 159 RVA: 0x00002A88 File Offset: 0x00000C88
		public static int GetSource(int sid, ALGetSourcei param)
		{
			int result;
			AL.GetSource(sid, param, out result);
			return result;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002AA0 File Offset: 0x00000CA0
		public static void GetSource(int sid, ALSourceb param, out bool value)
		{
			int num;
			AL.GetSource(sid, (ALGetSourcei)param, out num);
			value = (num != 0);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00002ABC File Offset: 0x00000CBC
		public static bool GetSource(int sid, ALSourceb param)
		{
			int num;
			AL.GetSource(sid, (ALGetSourcei)param, out num);
			return num != 0;
		}

		// Token: 0x060000A2 RID: 162
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcePlayv", ExactSpelling = true)]
		public unsafe static extern void SourcePlay(int ns, [In] int* sids);

		// Token: 0x060000A3 RID: 163
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcePlayv", ExactSpelling = true)]
		public static extern void SourcePlay(int ns, [In] ref int sids);

		// Token: 0x060000A4 RID: 164
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcePlayv", ExactSpelling = true)]
		public static extern void SourcePlay(int ns, [In] int[] sids);

		// Token: 0x060000A5 RID: 165 RVA: 0x00002AD6 File Offset: 0x00000CD6
		public static void SourcePlay(ReadOnlySpan<int> sources)
		{
			AL.SourcePlay(sources.Length, MemoryMarshal.GetReference<int>(sources));
		}

		// Token: 0x060000A6 RID: 166
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceStopv", ExactSpelling = true)]
		public unsafe static extern void SourceStop(int ns, [In] int* sids);

		// Token: 0x060000A7 RID: 167
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceStopv", ExactSpelling = true)]
		public static extern void SourceStop(int ns, ref int sids);

		// Token: 0x060000A8 RID: 168
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceStopv", ExactSpelling = true)]
		public static extern void SourceStop(int ns, int[] sids);

		// Token: 0x060000A9 RID: 169 RVA: 0x00002AEA File Offset: 0x00000CEA
		public static void SourceStop(ReadOnlySpan<int> sources)
		{
			AL.SourceStop(sources.Length, MemoryMarshal.GetReference<int>(sources));
		}

		// Token: 0x060000AA RID: 170
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceRewindv", ExactSpelling = true)]
		public unsafe static extern void SourceRewind(int ns, [In] int* sids);

		// Token: 0x060000AB RID: 171
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceRewindv", ExactSpelling = true)]
		public static extern void SourceRewind(int ns, ref int sids);

		// Token: 0x060000AC RID: 172
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceRewindv", ExactSpelling = true)]
		public static extern void SourceRewind(int ns, int[] sids);

		// Token: 0x060000AD RID: 173 RVA: 0x00002AFE File Offset: 0x00000CFE
		public static void SourceRewind(ReadOnlySpan<int> sources)
		{
			AL.SourceRewind(sources.Length, MemoryMarshal.GetReference<int>(sources));
		}

		// Token: 0x060000AE RID: 174
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcePausev", ExactSpelling = true)]
		public unsafe static extern void SourcePause(int ns, [In] int* sids);

		// Token: 0x060000AF RID: 175
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcePausev", ExactSpelling = true)]
		public static extern void SourcePause(int ns, ref int sids);

		// Token: 0x060000B0 RID: 176
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcePausev", ExactSpelling = true)]
		public static extern void SourcePause(int ns, int[] sids);

		// Token: 0x060000B1 RID: 177 RVA: 0x00002B12 File Offset: 0x00000D12
		public static void SourcePause(ReadOnlySpan<int> sources)
		{
			AL.SourcePause(sources.Length, MemoryMarshal.GetReference<int>(sources));
		}

		// Token: 0x060000B2 RID: 178
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcePlay", ExactSpelling = true)]
		public static extern void SourcePlay(int sid);

		// Token: 0x060000B3 RID: 179
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceStop", ExactSpelling = true)]
		public static extern void SourceStop(int sid);

		// Token: 0x060000B4 RID: 180
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceRewind", ExactSpelling = true)]
		public static extern void SourceRewind(int sid);

		// Token: 0x060000B5 RID: 181
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourcePause", ExactSpelling = true)]
		public static extern void SourcePause(int sid);

		// Token: 0x060000B6 RID: 182
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceQueueBuffers", ExactSpelling = true)]
		public unsafe static extern void SourceQueueBuffers(int sid, int numEntries, [In] int* bids);

		// Token: 0x060000B7 RID: 183
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceQueueBuffers", ExactSpelling = true)]
		public static extern void SourceQueueBuffers(int sid, int numEntries, int[] bids);

		// Token: 0x060000B8 RID: 184
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceQueueBuffers", ExactSpelling = true)]
		public static extern void SourceQueueBuffers(int sid, int numEntries, ref int bids);

		// Token: 0x060000B9 RID: 185 RVA: 0x00002B26 File Offset: 0x00000D26
		public static void SourceQueueBuffers(int sid, ReadOnlySpan<int> buffers)
		{
			AL.SourceQueueBuffers(sid, buffers.Length, MemoryMarshal.GetReference<int>(buffers));
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00002B3B File Offset: 0x00000D3B
		public static void SourceQueueBuffer(int source, int buffer)
		{
			AL.SourceQueueBuffers(source, 1, ref buffer);
		}

		// Token: 0x060000BB RID: 187
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceUnqueueBuffers", ExactSpelling = true)]
		public unsafe static extern void SourceUnqueueBuffers(int sid, int numEntries, int* bids);

		// Token: 0x060000BC RID: 188
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceUnqueueBuffers", ExactSpelling = true)]
		public static extern void SourceUnqueueBuffers(int sid, int numEntries, int[] bids);

		// Token: 0x060000BD RID: 189
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSourceUnqueueBuffers", ExactSpelling = true)]
		public static extern void SourceUnqueueBuffers(int sid, int numEntries, ref int bids);

		// Token: 0x060000BE RID: 190 RVA: 0x00002B46 File Offset: 0x00000D46
		public static void SourceUnqueueBuffers(int sid, int[] bids)
		{
			AL.SourceUnqueueBuffers(sid, bids.Length, bids);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00002B52 File Offset: 0x00000D52
		public static void SourceUnqueueBuffers(int sid, Span<int> bids)
		{
			AL.SourceUnqueueBuffers(sid, bids.Length, bids[0]);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00002B6C File Offset: 0x00000D6C
		public static int SourceUnqueueBuffer(int sid)
		{
			int result = 0;
			AL.SourceUnqueueBuffers(sid, 1, ref result);
			return result;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00002B88 File Offset: 0x00000D88
		public static int[] SourceUnqueueBuffers(int sid, int numEntries)
		{
			if (numEntries <= 0)
			{
				throw new ArgumentOutOfRangeException("numEntries", "Must be greater than zero.");
			}
			int[] array = new int[numEntries];
			AL.SourceUnqueueBuffers(sid, numEntries, array);
			return array;
		}

		// Token: 0x060000C2 RID: 194
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGenBuffers", ExactSpelling = true)]
		public unsafe static extern void GenBuffers(int n, [Out] int* buffers);

		// Token: 0x060000C3 RID: 195
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGenBuffers", ExactSpelling = true)]
		public static extern void GenBuffers(int n, ref int buffers);

		// Token: 0x060000C4 RID: 196
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGenBuffers", ExactSpelling = true)]
		public static extern void GenBuffers(int n, [Out] int[] buffers);

		// Token: 0x060000C5 RID: 197 RVA: 0x00002BB9 File Offset: 0x00000DB9
		public static void GenBuffers(Span<int> buffers)
		{
			AL.GenBuffers(buffers.Length, buffers[0]);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public static int[] GenBuffers(int n)
		{
			int[] array = new int[n];
			AL.GenBuffers(array.Length, array);
			return array;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00002BF0 File Offset: 0x00000DF0
		public static int GenBuffer()
		{
			int result = 0;
			AL.GenBuffers(1, ref result);
			return result;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00002C08 File Offset: 0x00000E08
		public static void GenBuffer(out int buffer)
		{
			int num = 0;
			AL.GenBuffers(1, ref num);
			buffer = num;
		}

		// Token: 0x060000C9 RID: 201
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alDeleteBuffers", ExactSpelling = true)]
		public unsafe static extern void DeleteBuffers(int n, [In] int* buffers);

		// Token: 0x060000CA RID: 202
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alDeleteBuffers", ExactSpelling = true)]
		public static extern void DeleteBuffers(int n, [In] ref int buffers);

		// Token: 0x060000CB RID: 203
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alDeleteBuffers", ExactSpelling = true)]
		public static extern void DeleteBuffers(int n, [In] int[] buffers);

		// Token: 0x060000CC RID: 204 RVA: 0x00002C22 File Offset: 0x00000E22
		public static void DeleteBuffers(int[] buffers)
		{
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			AL.DeleteBuffers(buffers.Length, buffers);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00002C3B File Offset: 0x00000E3B
		public static void DeleteBuffers(ReadOnlySpan<int> buffers)
		{
			AL.DeleteBuffers(buffers.Length, MemoryMarshal.GetReference<int>(buffers));
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00002C4F File Offset: 0x00000E4F
		public static void DeleteBuffer(int buffer)
		{
			AL.DeleteBuffers(1, ref buffer);
		}

		// Token: 0x060000CF RID: 207
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alIsBuffer", ExactSpelling = true)]
		public static extern bool IsBuffer(int bid);

		// Token: 0x060000D0 RID: 208
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alBufferData", ExactSpelling = true)]
		public static extern void BufferData(int bid, ALFormat format, IntPtr buffer, int size, int freq);

		// Token: 0x060000D1 RID: 209
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alBufferData", ExactSpelling = true)]
		public unsafe static extern void BufferData(int bid, ALFormat format, void* buffer, int size, int freq);

		// Token: 0x060000D2 RID: 210
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alBufferData", ExactSpelling = true)]
		public static extern void BufferData(int bid, ALFormat format, ref byte buffer, int size, int freq);

		// Token: 0x060000D3 RID: 211
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alBufferData", ExactSpelling = true)]
		public static extern void BufferData(int bid, ALFormat format, ref short buffer, int bytes, int freq);

		// Token: 0x060000D4 RID: 212 RVA: 0x00002C5C File Offset: 0x00000E5C
		public unsafe static void BufferData<[IsUnmanaged] TBuffer>(int bid, ALFormat format, TBuffer[] buffer, int freq) where TBuffer : struct, ValueType
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
				AL.BufferData(bid, format, (void*)buffer2, buffer.Length * sizeof(TBuffer), freq);
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00002C98 File Offset: 0x00000E98
		public unsafe static void BufferData<[IsUnmanaged] TBuffer>(int bid, ALFormat format, ReadOnlySpan<TBuffer> buffer, int freq) where TBuffer : struct, ValueType
		{
			fixed (TBuffer* pinnableReference = buffer.GetPinnableReference())
			{
				TBuffer* buffer2 = pinnableReference;
				AL.BufferData(bid, format, (void*)buffer2, buffer.Length * sizeof(TBuffer), freq);
			}
		}

		// Token: 0x060000D6 RID: 214
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alGetBufferi", ExactSpelling = true)]
		public static extern void GetBuffer(int bid, ALGetBufferi param, out int value);

		// Token: 0x060000D7 RID: 215 RVA: 0x00002CCC File Offset: 0x00000ECC
		public static int GetBuffer(int bid, ALGetBufferi param)
		{
			int result;
			AL.GetBuffer(bid, param, out result);
			return result;
		}

		// Token: 0x060000D8 RID: 216
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alDopplerFactor", ExactSpelling = true)]
		public static extern void DopplerFactor(float value);

		// Token: 0x060000D9 RID: 217
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alDopplerVelocity", ExactSpelling = true)]
		public static extern void DopplerVelocity(float value);

		// Token: 0x060000DA RID: 218
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alSpeedOfSound", ExactSpelling = true)]
		public static extern void SpeedOfSound(float value);

		// Token: 0x060000DB RID: 219
		[DllImport("AL", CallingConvention = 2, EntryPoint = "alDistanceModel", ExactSpelling = true)]
		public static extern void DistanceModel(ALDistanceModel distancemodel);

		// Token: 0x060000DC RID: 220 RVA: 0x00002CE3 File Offset: 0x00000EE3
		public static ALDistanceModel GetDistanceModel()
		{
			return (ALDistanceModel)AL.Get(ALGetInteger.DistanceModel);
		}

		// Token: 0x0400003F RID: 63
		internal const string Lib = "AL";

		// Token: 0x04000040 RID: 64
		internal const CallingConvention ALCallingConvention = 2;

		// Token: 0x02000051 RID: 81
		public class EXTDouble : ALBase
		{
			// Token: 0x0600019A RID: 410 RVA: 0x0000821F File Offset: 0x0000641F
			static EXTDouble()
			{
				ALBase.RegisterOpenALResolver();
			}

			// Token: 0x0600019B RID: 411 RVA: 0x00008226 File Offset: 0x00006426
			private EXTDouble()
			{
			}

			// Token: 0x0600019C RID: 412 RVA: 0x0000822E File Offset: 0x0000642E
			public static bool IsExtensionPresent()
			{
				return AL.IsExtensionPresent("AL_EXT_double");
			}

			// Token: 0x0600019D RID: 413
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alBufferData", ExactSpelling = true)]
			public unsafe static extern void BufferData(int bid, DoubleBufferFormat format, double* buffer, int bytes, int freq);

			// Token: 0x0600019E RID: 414
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alBufferData", ExactSpelling = true)]
			public static extern void BufferData(int bid, DoubleBufferFormat format, ref double buffer, int bytes, int freq);

			// Token: 0x0600019F RID: 415 RVA: 0x0000823A File Offset: 0x0000643A
			public static void BufferData(int bid, DoubleBufferFormat format, double[] buffer, int freq)
			{
				AL.EXTDouble.BufferData(bid, format, ref buffer[0], buffer.Length * 8, freq);
			}

			// Token: 0x060001A0 RID: 416 RVA: 0x00008250 File Offset: 0x00006450
			public static void BufferData(int bid, DoubleBufferFormat format, Span<double> buffer, int freq)
			{
				AL.EXTDouble.BufferData(bid, format, buffer[0], buffer.Length * 8, freq);
			}

			// Token: 0x04000289 RID: 649
			public const string ExtensionName = "AL_EXT_double";
		}

		// Token: 0x02000052 RID: 82
		public class EXTFloat32 : ALBase
		{
			// Token: 0x060001A1 RID: 417 RVA: 0x0000826B File Offset: 0x0000646B
			static EXTFloat32()
			{
				ALBase.RegisterOpenALResolver();
			}

			// Token: 0x060001A2 RID: 418 RVA: 0x00008272 File Offset: 0x00006472
			private EXTFloat32()
			{
			}

			// Token: 0x060001A3 RID: 419 RVA: 0x0000827A File Offset: 0x0000647A
			public static bool IsExtensionPresent()
			{
				return AL.IsExtensionPresent("AL_EXT_float32");
			}

			// Token: 0x060001A4 RID: 420
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alBufferData", ExactSpelling = true)]
			public unsafe static extern void BufferData(int bid, FloatBufferFormat format, float* buffer, int bytes, int freq);

			// Token: 0x060001A5 RID: 421
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alBufferData", ExactSpelling = true)]
			public static extern void BufferData(int bid, FloatBufferFormat format, ref float buffer, int bytes, int freq);

			// Token: 0x060001A6 RID: 422 RVA: 0x00008286 File Offset: 0x00006486
			public static void BufferData(int bid, FloatBufferFormat format, float[] buffer, int freq)
			{
				AL.EXTFloat32.BufferData(bid, format, ref buffer[0], buffer.Length * 4, freq);
			}

			// Token: 0x060001A7 RID: 423 RVA: 0x0000829C File Offset: 0x0000649C
			public static void BufferData(int bid, FloatBufferFormat format, Span<float> buffer, int freq)
			{
				AL.EXTFloat32.BufferData(bid, format, buffer[0], buffer.Length * 4, freq);
			}

			// Token: 0x0400028A RID: 650
			public const string ExtensionName = "AL_EXT_float32";
		}

		// Token: 0x02000053 RID: 83
		public class LoopPoints : ALBase
		{
			// Token: 0x060001A8 RID: 424 RVA: 0x000082B7 File Offset: 0x000064B7
			static LoopPoints()
			{
				ALBase.RegisterOpenALResolver();
			}

			// Token: 0x060001A9 RID: 425 RVA: 0x000082BE File Offset: 0x000064BE
			private LoopPoints()
			{
			}

			// Token: 0x060001AA RID: 426 RVA: 0x000082C6 File Offset: 0x000064C6
			public static bool IsExtensionPresent()
			{
				return AL.IsExtensionPresent("AL_SOFT_loop_points");
			}

			// Token: 0x060001AB RID: 427
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alBufferiv", ExactSpelling = true)]
			public unsafe static extern void Buffer(int buffer, BufferLoopPoint param, int* values);

			// Token: 0x060001AC RID: 428
			[DllImport("AL", CallingConvention = 2, EntryPoint = "alBufferiv", ExactSpelling = true)]
			public static extern void Buffer(int buffer, BufferLoopPoint param, ref int values);

			// Token: 0x060001AD RID: 429 RVA: 0x000082D2 File Offset: 0x000064D2
			public static void Buffer(int buffer, BufferLoopPoint param, ReadOnlySpan<int> values)
			{
				AL.LoopPoints.Buffer(buffer, param, MemoryMarshal.GetReference<int>(values));
			}

			// Token: 0x060001AE RID: 430 RVA: 0x000082E4 File Offset: 0x000064E4
			public unsafe static void Buffer(int buffer, BufferLoopPoint param, int start, int end)
			{
				Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)8], 2);
				*span[0] = start;
				*span[1] = end;
				AL.LoopPoints.Buffer(buffer, param, span);
			}

			// Token: 0x0400028B RID: 651
			public const string ExtensionName = "AL_SOFT_loop_points";
		}

		// Token: 0x02000054 RID: 84
		public class SourceLatency : ALBase
		{
			// Token: 0x060001AF RID: 431 RVA: 0x00008320 File Offset: 0x00006520
			static SourceLatency()
			{
				ALBase.RegisterOpenALResolver();
			}

			// Token: 0x060001B0 RID: 432 RVA: 0x0000838C File Offset: 0x0000658C
			private SourceLatency()
			{
			}

			// Token: 0x060001B1 RID: 433 RVA: 0x00008394 File Offset: 0x00006594
			public static bool IsExtensionPresent()
			{
				return AL.IsExtensionPresent("AL_SOFT_source_latency");
			}

			// Token: 0x060001B2 RID: 434 RVA: 0x000083A0 File Offset: 0x000065A0
			public unsafe static void GetSource(int source, SourceLatencyVector2i param, long* values)
			{
				AL.SourceLatency._GetSourcei64vPtr(source, param, values);
			}

			// Token: 0x060001B3 RID: 435 RVA: 0x000083AF File Offset: 0x000065AF
			private static void GetSource(int source, SourceLatencyVector2i param, out long values)
			{
				AL.SourceLatency._GetSourcei64vRef(source, param, out values);
			}

			// Token: 0x060001B4 RID: 436 RVA: 0x000083BE File Offset: 0x000065BE
			public static void GetSource(int source, SourceLatencyVector2i param, long[] values)
			{
				AL.SourceLatency._GetSourcei64vArray(source, param, values);
			}

			// Token: 0x060001B5 RID: 437 RVA: 0x000083CD File Offset: 0x000065CD
			public unsafe static void GetSource(int source, SourceLatencyVector2d param, double* values)
			{
				AL.SourceLatency._GetSourcedvPtr(source, param, values);
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x000083DC File Offset: 0x000065DC
			private static void GetSource(int source, SourceLatencyVector2d param, out double values)
			{
				AL.SourceLatency._GetSourcedvRef(source, param, out values);
			}

			// Token: 0x060001B7 RID: 439 RVA: 0x000083EB File Offset: 0x000065EB
			public static void GetSource(int source, SourceLatencyVector2d param, double[] values)
			{
				AL.SourceLatency._GetSourcedvArray(source, param, values);
			}

			// Token: 0x060001B8 RID: 440 RVA: 0x000083FC File Offset: 0x000065FC
			public unsafe static void GetSource(int source, SourceLatencyVector2i param, out long value1, out long value2)
			{
				long* ptr = stackalloc long[(UIntPtr)16];
				AL.SourceLatency.GetSource(source, param, ptr);
				value1 = *ptr;
				value2 = ptr[1];
			}

			// Token: 0x060001B9 RID: 441 RVA: 0x00008421 File Offset: 0x00006621
			public static void GetSource(int source, SourceLatencyVector2i param, Span<long> values)
			{
				AL.SourceLatency.GetSource(source, param, values[0]);
			}

			// Token: 0x060001BA RID: 442 RVA: 0x00008434 File Offset: 0x00006634
			public unsafe static void GetSource(int source, SourceLatencyVector2i param, out int value1, out int value2, out long value3)
			{
				int* ptr = stackalloc int[(UIntPtr)16];
				AL.SourceLatency.GetSource(source, param, (long*)ptr);
				value1 = *ptr;
				value2 = ptr[1];
				value3 = *(long*)(ptr + (IntPtr)2 * 8 / 4);
			}

			// Token: 0x060001BB RID: 443 RVA: 0x00008464 File Offset: 0x00006664
			public unsafe static void GetSource(int source, SourceLatencyVector2d param, out double value1, out double value2)
			{
				double* ptr = stackalloc double[(UIntPtr)16];
				AL.SourceLatency.GetSource(source, param, ptr);
				value1 = *ptr;
				value2 = ptr[1];
			}

			// Token: 0x060001BC RID: 444 RVA: 0x00008489 File Offset: 0x00006689
			public static void GetSource(int source, SourceLatencyVector2d param, Span<double> values)
			{
				AL.SourceLatency.GetSource(source, param, values[0]);
			}

			// Token: 0x060001BD RID: 445 RVA: 0x0000849A File Offset: 0x0000669A
			public static void GetSource(int source, SourceLatencyVector2d param, out Vector2d values)
			{
				values.Y = 0.0;
				AL.SourceLatency.GetSource(source, param, out values.X);
			}

			// Token: 0x0400028C RID: 652
			public const string ExtensionName = "AL_SOFT_source_latency";

			// Token: 0x0400028D RID: 653
			private static readonly AL.SourceLatency.GetSourcei64vPtrDelegate _GetSourcei64vPtr = ALBase.LoadDelegate<AL.SourceLatency.GetSourcei64vPtrDelegate>("alGetSourcei64vSOFT");

			// Token: 0x0400028E RID: 654
			private static readonly AL.SourceLatency.GetSourcei64vRefDelegate _GetSourcei64vRef = ALBase.LoadDelegate<AL.SourceLatency.GetSourcei64vRefDelegate>("alGetSourcei64vSOFT");

			// Token: 0x0400028F RID: 655
			private static readonly AL.SourceLatency.GetSourcei64vArrayDelegate _GetSourcei64vArray = ALBase.LoadDelegate<AL.SourceLatency.GetSourcei64vArrayDelegate>("alGetSourcei64vSOFT");

			// Token: 0x04000290 RID: 656
			private static readonly AL.SourceLatency.GetSourcedvPtrDelegate _GetSourcedvPtr = ALBase.LoadDelegate<AL.SourceLatency.GetSourcedvPtrDelegate>("alGetSourcedvSOFT");

			// Token: 0x04000291 RID: 657
			private static readonly AL.SourceLatency.GetSourcedvRefDelegate _GetSourcedvRef = ALBase.LoadDelegate<AL.SourceLatency.GetSourcedvRefDelegate>("alGetSourcedvSOFT");

			// Token: 0x04000292 RID: 658
			private static readonly AL.SourceLatency.GetSourcedvArrayDelegate _GetSourcedvArray = ALBase.LoadDelegate<AL.SourceLatency.GetSourcedvArrayDelegate>("alGetSourcedvSOFT");

			// Token: 0x020000A6 RID: 166
			// (Invoke) Token: 0x06000302 RID: 770
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetSourcei64vPtrDelegate(int source, SourceLatencyVector2i param, long* values);

			// Token: 0x020000A7 RID: 167
			// (Invoke) Token: 0x06000306 RID: 774
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourcei64vRefDelegate(int source, SourceLatencyVector2i param, out long values);

			// Token: 0x020000A8 RID: 168
			// (Invoke) Token: 0x0600030A RID: 778
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourcei64vArrayDelegate(int source, SourceLatencyVector2i param, long[] values);

			// Token: 0x020000A9 RID: 169
			// (Invoke) Token: 0x0600030E RID: 782
			[UnmanagedFunctionPointer(2)]
			private unsafe delegate void GetSourcedvPtrDelegate(int source, SourceLatencyVector2d param, double* values);

			// Token: 0x020000AA RID: 170
			// (Invoke) Token: 0x06000312 RID: 786
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourcedvRefDelegate(int source, SourceLatencyVector2d param, out double values);

			// Token: 0x020000AB RID: 171
			// (Invoke) Token: 0x06000316 RID: 790
			[UnmanagedFunctionPointer(2)]
			private delegate void GetSourcedvArrayDelegate(int source, SourceLatencyVector2d param, double[] values);
		}
	}
}
