using System;
using System.Runtime.CompilerServices;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio
{
	// Token: 0x02000102 RID: 258
	public static class OALW
	{
		// Token: 0x06000962 RID: 2402 RVA: 0x00033FC0 File Offset: 0x000321C0
		public static void Source(int source, ALSourceb property, bool value)
		{
			AL.Source(source, property, value);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error setting source ");
			defaultInterpolatedStringHandler.AppendFormatted<ALSourceb>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00034000 File Offset: 0x00032200
		public static void Source(int source, ALSourcef property, float value)
		{
			AL.Source(source, property, value);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error setting source ");
			defaultInterpolatedStringHandler.AppendFormatted<ALSourcef>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00034040 File Offset: 0x00032240
		public static void Source(int source, ALSource3f property, float value1, float value2, float value3)
		{
			AL.Source(source, property, value1, value2, value3);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error setting source ");
			defaultInterpolatedStringHandler.AppendFormatted<ALSource3f>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00034084 File Offset: 0x00032284
		public static void Source(int source, ALSourcei property, int value)
		{
			AL.Source(source, property, value);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error setting source ");
			defaultInterpolatedStringHandler.AppendFormatted<ALSourcei>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x000340C4 File Offset: 0x000322C4
		public static void Listener(ALListenerf property, float value)
		{
			AL.Listener(property, value);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(23, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error setting listener ");
			defaultInterpolatedStringHandler.AppendFormatted<ALListenerf>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00034103 File Offset: 0x00032303
		public static int GenSource()
		{
			int result = AL.GenSource();
			OALW.CheckError("Error generating source", 0);
			return result;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00034118 File Offset: 0x00032318
		public static void GetSource(int source, ALGetSourcei property, out int value)
		{
			AL.GetSource(source, property, ref value);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error getting source ");
			defaultInterpolatedStringHandler.AppendFormatted<ALGetSourcei>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 40963);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0003415C File Offset: 0x0003235C
		public static ALSourceState GetSourceState(int source)
		{
			int state;
			OALW.GetSource(source, 4112, out state);
			OALW.CheckError("Error getting source state", 40963);
			return state;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00034186 File Offset: 0x00032386
		public static void SourceQueueBuffer(int source, int buffer)
		{
			AL.SourceQueueBuffer(source, buffer);
			OALW.CheckError("Error SourceQueueBuffer", 0);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0003419C File Offset: 0x0003239C
		public static int SourceUnqueueBuffer(int source)
		{
			int buffer = AL.SourceUnqueueBuffer(source);
			OALW.CheckError("Error SourceUnqueueBuffer", new ALError[]
			{
				40963,
				40961
			});
			return buffer;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x000341D1 File Offset: 0x000323D1
		public static void SourcePlay(int source)
		{
			AL.SourcePlay(source);
			OALW.CheckError("Error playing source", 0);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x000341E4 File Offset: 0x000323E4
		public static void SourceStop(int source)
		{
			AL.SourceStop(source);
			OALW.CheckError("Error stop playing source", new ALError[]
			{
				40963,
				40961
			});
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0003420C File Offset: 0x0003240C
		public static void DeleteSource(int source)
		{
			try
			{
				AL.DeleteSource(source);
			}
			catch
			{
			}
			OALW.CheckError("Error deleting source", 40963);
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00034244 File Offset: 0x00032444
		public static int[] GenBuffers(int bufferCount)
		{
			int[] result = AL.GenBuffers(bufferCount);
			OALW.CheckError("Error gen buffers", 0);
			return result;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00034257 File Offset: 0x00032457
		public static void BufferData(int currentBuffer, ALFormat format, byte[] audio, int frequency)
		{
			AL.BufferData<byte>(currentBuffer, format, audio, frequency);
			OALW.CheckError("Error buffer data", 0);
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0003426D File Offset: 0x0003246D
		public static void DeleteBuffers(int[] buffers)
		{
			AL.DeleteBuffers(buffers);
			OALW.CheckError("Error deleting buffers", 0);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00034280 File Offset: 0x00032480
		public static void ClearError()
		{
			AL.GetError();
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00034288 File Offset: 0x00032488
		public static void CheckError(string value, ALError ignoredErrors = 0)
		{
			ALError error = AL.GetError();
			if (error == null)
			{
				return;
			}
			if (error == ignoredErrors)
			{
				return;
			}
			Logger.client.Debug("{0} {1}", new object[]
			{
				value,
				AL.GetErrorString(error)
			});
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x000342C8 File Offset: 0x000324C8
		public static void CheckError(string value, params ALError[] ignoredErrors)
		{
			ALError error = AL.GetError();
			if (error == null)
			{
				return;
			}
			foreach (ALError ignoredError in ignoredErrors)
			{
				if (error == ignoredError)
				{
					return;
				}
			}
			Logger.client.Debug("{0} {1}", new object[]
			{
				value,
				AL.GetErrorString(error)
			});
		}
	}
}
