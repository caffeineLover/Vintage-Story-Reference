using System;
using System.Runtime.CompilerServices;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000C9 RID: 201
	public static class OALW
	{
		// Token: 0x06000742 RID: 1858 RVA: 0x0002896C File Offset: 0x00026B6C
		public static void Source(int source, ALSourceb property, bool value)
		{
			AL.Source(source, property, value);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error setting source ");
			defaultInterpolatedStringHandler.AppendFormatted<ALSourceb>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x000289AC File Offset: 0x00026BAC
		public static void Source(int source, ALSourcef property, float value)
		{
			AL.Source(source, property, value);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error setting source ");
			defaultInterpolatedStringHandler.AppendFormatted<ALSourcef>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000289EC File Offset: 0x00026BEC
		public static void Source(int source, ALSource3f property, float value1, float value2, float value3)
		{
			AL.Source(source, property, value1, value2, value3);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error setting source ");
			defaultInterpolatedStringHandler.AppendFormatted<ALSource3f>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00028A30 File Offset: 0x00026C30
		public static void Source(int source, ALSourcei property, int value)
		{
			AL.Source(source, property, value);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error setting source ");
			defaultInterpolatedStringHandler.AppendFormatted<ALSourcei>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00028A70 File Offset: 0x00026C70
		public static void Listener(ALListenerf property, float value)
		{
			AL.Listener(property, value);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(23, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error setting listener ");
			defaultInterpolatedStringHandler.AppendFormatted<ALListenerf>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00028AAF File Offset: 0x00026CAF
		public static int GenSource()
		{
			int result = AL.GenSource();
			OALW.CheckError("Error generating source", 0);
			return result;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00028AC4 File Offset: 0x00026CC4
		public static void GetSource(int source, ALGetSourcei property, out int value)
		{
			AL.GetSource(source, property, ref value);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Error getting source ");
			defaultInterpolatedStringHandler.AppendFormatted<ALGetSourcei>(property);
			OALW.CheckError(defaultInterpolatedStringHandler.ToStringAndClear(), 40963);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00028B08 File Offset: 0x00026D08
		public static ALSourceState GetSourceState(int source)
		{
			int state;
			OALW.GetSource(source, 4112, out state);
			OALW.CheckError("Error getting source state", 40963);
			return state;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00028B32 File Offset: 0x00026D32
		public static void SourceQueueBuffer(int source, int buffer)
		{
			AL.SourceQueueBuffer(source, buffer);
			OALW.CheckError("Error SourceQueueBuffer", 0);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00028B46 File Offset: 0x00026D46
		public static int SourceUnqueueBuffer(int source)
		{
			int result = AL.SourceUnqueueBuffer(source);
			OALW.CheckError("Error SourceUnqueueBuffer", 40963);
			return result;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00028B5D File Offset: 0x00026D5D
		public static void SourcePlay(int source)
		{
			AL.SourcePlay(source);
			OALW.CheckError("Error playing source", 0);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00028B70 File Offset: 0x00026D70
		public static void SourceStop(int source)
		{
			AL.SourceStop(source);
			OALW.CheckError("Error stop playing source", 40963);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00028B88 File Offset: 0x00026D88
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

		// Token: 0x0600074F RID: 1871 RVA: 0x00028BC0 File Offset: 0x00026DC0
		public static int[] GenBuffers(int bufferCount)
		{
			int[] result = AL.GenBuffers(bufferCount);
			OALW.CheckError("Error gen buffers", 0);
			return result;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00028BD3 File Offset: 0x00026DD3
		public static void BufferData(int currentBuffer, ALFormat format, byte[] audio, int frequency)
		{
			AL.BufferData<byte>(currentBuffer, format, audio, frequency);
			OALW.CheckError("Error buffer data", 0);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00028BE9 File Offset: 0x00026DE9
		public static void DeleteBuffers(int[] buffers)
		{
			AL.DeleteBuffers(buffers);
			OALW.CheckError("Error deleting buffers", 0);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00028BFC File Offset: 0x00026DFC
		public static void ClearError()
		{
			AL.GetError();
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00028C04 File Offset: 0x00026E04
		public static void CheckError(string Value, ALError ignoredErrors = 0)
		{
			ALError error = AL.GetError();
			if (error == null)
			{
				return;
			}
			if (ignoredErrors == error)
			{
				return;
			}
			Logger.client.Debug("{0} {1}", new object[]
			{
				Value,
				AL.GetErrorString(error)
			});
		}
	}
}
