using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Concentus.Common
{
	// Token: 0x0200007A RID: 122
	internal static class Inlines
	{
		// Token: 0x06000341 RID: 833 RVA: 0x00024B21 File Offset: 0x00022D21
		[Conditional("DEBUG")]
		internal static void OpusAssert(bool condition, string message = "Unknown error")
		{
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00024B23 File Offset: 0x00022D23
		[MethodImpl(256)]
		internal static int MULT16_16SU(int a, int b)
		{
			return (int)((short)a * (short)((ushort)b));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00024B2A File Offset: 0x00022D2A
		[MethodImpl(256)]
		internal static int MULT16_16SU(short a, ushort b)
		{
			return (int)(a * (short)b);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00024B2F File Offset: 0x00022D2F
		[MethodImpl(256)]
		internal static int MULT16_16SU(int a, uint b)
		{
			return a * (int)b;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00024B34 File Offset: 0x00022D34
		[MethodImpl(256)]
		internal static int MULT16_32_Q16(short a, int b)
		{
			return Inlines.ADD32(Inlines.MULT16_16((int)a, Inlines.SHR(b, 16)), Inlines.SHR(Inlines.MULT16_16SU((int)a, b & 65535), 16));
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00024B5D File Offset: 0x00022D5D
		[MethodImpl(256)]
		internal static int MULT16_32_Q16(int a, int b)
		{
			return Inlines.ADD32(Inlines.MULT16_16(a, Inlines.SHR(b, 16)), Inlines.SHR(Inlines.MULT16_16SU(a, b & 65535), 16));
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00024B86 File Offset: 0x00022D86
		[MethodImpl(256)]
		internal static int MULT16_32_P16(short a, int b)
		{
			return Inlines.ADD32(Inlines.MULT16_16((int)a, Inlines.SHR(b, 16)), Inlines.PSHR(Inlines.MULT16_16SU((int)a, b & 65535), 16));
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00024BAF File Offset: 0x00022DAF
		[MethodImpl(256)]
		internal static int MULT16_32_P16(int a, int b)
		{
			return Inlines.ADD32(Inlines.MULT16_16(a, Inlines.SHR(b, 16)), Inlines.PSHR(Inlines.MULT16_16SU(a, b & 65535), 16));
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00024BD8 File Offset: 0x00022DD8
		[MethodImpl(256)]
		internal static int MULT16_32_Q15(short a, int b)
		{
			return ((int)a * (b >> 16) << 1) + ((int)a * (b & 65535) >> 15);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00024BEF File Offset: 0x00022DEF
		[MethodImpl(256)]
		internal static int MULT16_32_Q15(int a, int b)
		{
			return (a * (b >> 16) << 1) + (a * (b & 65535) >> 15);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00024C08 File Offset: 0x00022E08
		[MethodImpl(256)]
		internal static int MULT32_32_Q31(int a, int b)
		{
			return Inlines.ADD32(Inlines.ADD32(Inlines.SHL(Inlines.MULT16_16(Inlines.SHR(a, 16), Inlines.SHR(b, 16)), 1), Inlines.SHR(Inlines.MULT16_16SU(Inlines.SHR(a, 16), b & 65535), 15)), Inlines.SHR(Inlines.MULT16_16SU(Inlines.SHR(b, 16), a & 65535), 15));
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00024C70 File Offset: 0x00022E70
		[MethodImpl(256)]
		internal static short QCONST16(float x, int bits)
		{
			return (short)(0.5 + (double)(x * (float)(1 << bits)));
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00024C87 File Offset: 0x00022E87
		[MethodImpl(256)]
		internal static int QCONST32(float x, int bits)
		{
			return (int)(0.5 + (double)(x * (float)(1 << bits)));
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00024C9E File Offset: 0x00022E9E
		[MethodImpl(256)]
		internal static short NEG16(short x)
		{
			return 0 - x;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00024CA4 File Offset: 0x00022EA4
		[MethodImpl(256)]
		internal static int NEG16(int x)
		{
			return 0 - x;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00024CA9 File Offset: 0x00022EA9
		[MethodImpl(256)]
		internal static int NEG32(int x)
		{
			return 0 - x;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00024CAE File Offset: 0x00022EAE
		[MethodImpl(256)]
		internal static short EXTRACT16(int x)
		{
			return (short)x;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00024CB2 File Offset: 0x00022EB2
		[MethodImpl(256)]
		internal static int EXTEND32(short x)
		{
			return (int)x;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00024CB5 File Offset: 0x00022EB5
		[MethodImpl(256)]
		internal static int EXTEND32(int x)
		{
			return x;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00024CB8 File Offset: 0x00022EB8
		[MethodImpl(256)]
		internal static short SHR16(short a, int shift)
		{
			return (short)(a >> shift);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00024CC1 File Offset: 0x00022EC1
		[MethodImpl(256)]
		internal static int SHR16(int a, int shift)
		{
			return a >> shift;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00024CC9 File Offset: 0x00022EC9
		[MethodImpl(256)]
		internal static short SHL16(short a, int shift)
		{
			return (short)((ushort)a << shift);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00024CD3 File Offset: 0x00022ED3
		[MethodImpl(256)]
		internal static int SHL16(int a, int shift)
		{
			return a << shift;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00024CDB File Offset: 0x00022EDB
		[MethodImpl(256)]
		internal static int SHR32(int a, int shift)
		{
			return a >> shift;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00024CE3 File Offset: 0x00022EE3
		[MethodImpl(256)]
		internal static int SHL32(int a, int shift)
		{
			return a << shift;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00024CEB File Offset: 0x00022EEB
		[MethodImpl(256)]
		internal static int PSHR32(int a, int shift)
		{
			return Inlines.SHR32(a + (Inlines.EXTEND32(1) << shift >> 1), shift);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00024D02 File Offset: 0x00022F02
		[MethodImpl(256)]
		internal static short PSHR16(short a, int shift)
		{
			return Inlines.SHR16((short)((int)a + (1 << shift >> 1)), shift);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00024D15 File Offset: 0x00022F15
		[MethodImpl(256)]
		internal static int PSHR16(int a, int shift)
		{
			return Inlines.SHR32(a + (1 << shift >> 1), shift);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00024D27 File Offset: 0x00022F27
		[MethodImpl(256)]
		internal static int VSHR32(int a, int shift)
		{
			if (shift <= 0)
			{
				return Inlines.SHL32(a, -shift);
			}
			return Inlines.SHR32(a, shift);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00024D3D File Offset: 0x00022F3D
		[MethodImpl(256)]
		private static int SHR(int a, int shift)
		{
			return a >> shift;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00024D45 File Offset: 0x00022F45
		[MethodImpl(256)]
		private static int SHL(int a, int shift)
		{
			return Inlines.SHL32(a, shift);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00024D4E File Offset: 0x00022F4E
		[MethodImpl(256)]
		private static int SHR(short a, int shift)
		{
			return a >> shift;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00024D56 File Offset: 0x00022F56
		[MethodImpl(256)]
		private static int SHL(short a, int shift)
		{
			return Inlines.SHL32((int)a, shift);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00024D5F File Offset: 0x00022F5F
		[MethodImpl(256)]
		private static int PSHR(int a, int shift)
		{
			return Inlines.SHR(a + (Inlines.EXTEND32(1) << shift >> 1), shift);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00024D76 File Offset: 0x00022F76
		[MethodImpl(256)]
		internal static int SATURATE(int x, int a)
		{
			if (x > a)
			{
				return a;
			}
			if (x >= -a)
			{
				return x;
			}
			return -a;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00024D87 File Offset: 0x00022F87
		[MethodImpl(256)]
		internal static short SATURATE16(int x)
		{
			return Inlines.EXTRACT16((x > 32767) ? 32767 : ((x < -32768) ? -32768 : x));
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00024DAD File Offset: 0x00022FAD
		[MethodImpl(256)]
		internal static short ROUND16(short x, short a)
		{
			return Inlines.EXTRACT16(Inlines.PSHR32((int)x, (int)a));
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00024DBB File Offset: 0x00022FBB
		[MethodImpl(256)]
		internal static int ROUND16(int x, int a)
		{
			return Inlines.PSHR32(x, a);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00024DC4 File Offset: 0x00022FC4
		[MethodImpl(256)]
		internal static int PDIV32(int a, int b)
		{
			return a / b;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00024DC9 File Offset: 0x00022FC9
		[MethodImpl(256)]
		internal static short HALF16(short x)
		{
			return Inlines.SHR16(x, 1);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00024DD2 File Offset: 0x00022FD2
		[MethodImpl(256)]
		internal static int HALF16(int x)
		{
			return Inlines.SHR32(x, 1);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00024DDB File Offset: 0x00022FDB
		[MethodImpl(256)]
		internal static int HALF32(int x)
		{
			return Inlines.SHR32(x, 1);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00024DE4 File Offset: 0x00022FE4
		[MethodImpl(256)]
		internal static short ADD16(short a, short b)
		{
			return a + b;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00024DEA File Offset: 0x00022FEA
		[MethodImpl(256)]
		internal static int ADD16(int a, int b)
		{
			return a + b;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00024DEF File Offset: 0x00022FEF
		[MethodImpl(256)]
		internal static short SUB16(short a, short b)
		{
			return a - b;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00024DF5 File Offset: 0x00022FF5
		[MethodImpl(256)]
		internal static int SUB16(int a, int b)
		{
			return a - b;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00024DFA File Offset: 0x00022FFA
		[MethodImpl(256)]
		internal static int ADD32(int a, int b)
		{
			return a + b;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00024DFF File Offset: 0x00022FFF
		[MethodImpl(256)]
		internal static int SUB32(int a, int b)
		{
			return a - b;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00024E04 File Offset: 0x00023004
		[MethodImpl(256)]
		internal static short MULT16_16_16(short a, short b)
		{
			return a * b;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00024E0A File Offset: 0x0002300A
		[MethodImpl(256)]
		internal static int MULT16_16_16(int a, int b)
		{
			return a * b;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00024E0F File Offset: 0x0002300F
		[MethodImpl(256)]
		internal static int MULT16_16(int a, int b)
		{
			return a * b;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00024E14 File Offset: 0x00023014
		[MethodImpl(256)]
		internal static int MULT16_16(short a, short b)
		{
			return (int)(a * b);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00024E19 File Offset: 0x00023019
		[MethodImpl(256)]
		internal static int MAC16_16(short c, short a, short b)
		{
			return (int)(c + a * b);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00024E20 File Offset: 0x00023020
		[MethodImpl(256)]
		internal static int MAC16_16(int c, short a, short b)
		{
			return c + (int)(a * b);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00024E27 File Offset: 0x00023027
		[MethodImpl(256)]
		internal static int MAC16_16(int c, int a, int b)
		{
			return c + a * b;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00024E2E File Offset: 0x0002302E
		[MethodImpl(256)]
		internal static int MAC16_32_Q15(int c, short a, short b)
		{
			return Inlines.ADD32(c, Inlines.ADD32(Inlines.MULT16_16((int)a, Inlines.SHR(b, 15)), Inlines.SHR(Inlines.MULT16_16((int)a, (int)(b & short.MaxValue)), 15)));
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00024E5D File Offset: 0x0002305D
		[MethodImpl(256)]
		internal static int MAC16_32_Q15(int c, int a, int b)
		{
			return Inlines.ADD32(c, Inlines.ADD32(Inlines.MULT16_16(a, Inlines.SHR(b, 15)), Inlines.SHR(Inlines.MULT16_16(a, b & 32767), 15)));
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00024E8C File Offset: 0x0002308C
		[MethodImpl(256)]
		internal static int MAC16_32_Q16(int c, short a, short b)
		{
			return Inlines.ADD32(c, Inlines.ADD32(Inlines.MULT16_16((int)a, Inlines.SHR(b, 16)), Inlines.SHR(Inlines.MULT16_16SU((int)a, (int)b & 65535), 16)));
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00024EBB File Offset: 0x000230BB
		[MethodImpl(256)]
		internal static int MAC16_32_Q16(int c, int a, int b)
		{
			return Inlines.ADD32(c, Inlines.ADD32(Inlines.MULT16_16(a, Inlines.SHR(b, 16)), Inlines.SHR(Inlines.MULT16_16SU(a, b & 65535), 16)));
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00024EEA File Offset: 0x000230EA
		[MethodImpl(256)]
		internal static int MULT16_16_Q11_32(short a, short b)
		{
			return Inlines.SHR(Inlines.MULT16_16(a, b), 11);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00024EFA File Offset: 0x000230FA
		[MethodImpl(256)]
		internal static int MULT16_16_Q11_32(int a, int b)
		{
			return Inlines.SHR(Inlines.MULT16_16(a, b), 11);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00024F0A File Offset: 0x0002310A
		[MethodImpl(256)]
		internal static short MULT16_16_Q11(short a, short b)
		{
			return (short)Inlines.SHR(Inlines.MULT16_16(a, b), 11);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00024F1B File Offset: 0x0002311B
		[MethodImpl(256)]
		internal static int MULT16_16_Q11(int a, int b)
		{
			return Inlines.SHR(Inlines.MULT16_16(a, b), 11);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00024F2B File Offset: 0x0002312B
		[MethodImpl(256)]
		internal static short MULT16_16_Q13(short a, short b)
		{
			return (short)Inlines.SHR(Inlines.MULT16_16(a, b), 13);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00024F3C File Offset: 0x0002313C
		[MethodImpl(256)]
		internal static int MULT16_16_Q13(int a, int b)
		{
			return Inlines.SHR(Inlines.MULT16_16(a, b), 13);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00024F4C File Offset: 0x0002314C
		[MethodImpl(256)]
		internal static short MULT16_16_Q14(short a, short b)
		{
			return (short)Inlines.SHR(Inlines.MULT16_16(a, b), 14);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00024F5D File Offset: 0x0002315D
		[MethodImpl(256)]
		internal static int MULT16_16_Q14(int a, int b)
		{
			return Inlines.SHR(Inlines.MULT16_16(a, b), 14);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00024F6D File Offset: 0x0002316D
		[MethodImpl(256)]
		internal static short MULT16_16_Q15(short a, short b)
		{
			return (short)Inlines.SHR(Inlines.MULT16_16(a, b), 15);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00024F7E File Offset: 0x0002317E
		[MethodImpl(256)]
		internal static int MULT16_16_Q15(int a, int b)
		{
			return Inlines.SHR(Inlines.MULT16_16(a, b), 15);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00024F8E File Offset: 0x0002318E
		[MethodImpl(256)]
		internal static short MULT16_16_P13(short a, short b)
		{
			return (short)Inlines.SHR(Inlines.ADD32(4096, Inlines.MULT16_16(a, b)), 13);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00024FA9 File Offset: 0x000231A9
		[MethodImpl(256)]
		internal static int MULT16_16_P13(int a, int b)
		{
			return Inlines.SHR(Inlines.ADD32(4096, Inlines.MULT16_16(a, b)), 13);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00024FC3 File Offset: 0x000231C3
		[MethodImpl(256)]
		internal static short MULT16_16_P14(short a, short b)
		{
			return (short)Inlines.SHR(Inlines.ADD32(8192, Inlines.MULT16_16(a, b)), 14);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00024FDE File Offset: 0x000231DE
		[MethodImpl(256)]
		internal static int MULT16_16_P14(int a, int b)
		{
			return Inlines.SHR(Inlines.ADD32(8192, Inlines.MULT16_16(a, b)), 14);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00024FF8 File Offset: 0x000231F8
		[MethodImpl(256)]
		internal static short MULT16_16_P15(short a, short b)
		{
			return (short)Inlines.SHR(Inlines.ADD32(16384, Inlines.MULT16_16(a, b)), 15);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00025013 File Offset: 0x00023213
		[MethodImpl(256)]
		internal static int MULT16_16_P15(int a, int b)
		{
			return Inlines.SHR(Inlines.ADD32(16384, Inlines.MULT16_16(a, b)), 15);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0002502D File Offset: 0x0002322D
		[MethodImpl(256)]
		internal static short DIV32_16(int a, short b)
		{
			return (short)(a / (int)b);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00025033 File Offset: 0x00023233
		[MethodImpl(256)]
		internal static int DIV32_16(int a, int b)
		{
			return a / b;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00025038 File Offset: 0x00023238
		[MethodImpl(256)]
		internal static int DIV32(int a, int b)
		{
			return a / b;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0002503D File Offset: 0x0002323D
		[MethodImpl(256)]
		internal static short SAT16(int x)
		{
			return (x > 32767) ? short.MaxValue : ((x < -32768) ? short.MinValue : ((short)x));
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00025060 File Offset: 0x00023260
		[MethodImpl(256)]
		internal static short SIG2WORD16(int x)
		{
			x = Inlines.PSHR32(x, 12);
			x = Inlines.MAX32(x, -32768);
			x = Inlines.MIN32(x, 32767);
			return Inlines.EXTRACT16(x);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0002508C File Offset: 0x0002328C
		[MethodImpl(256)]
		internal static short MIN(short a, short b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00025095 File Offset: 0x00023295
		[MethodImpl(256)]
		internal static short MAX(short a, short b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0002509E File Offset: 0x0002329E
		[MethodImpl(256)]
		internal static short MIN16(short a, short b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000250A7 File Offset: 0x000232A7
		[MethodImpl(256)]
		internal static short MAX16(short a, short b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000250B0 File Offset: 0x000232B0
		[MethodImpl(256)]
		internal static int MIN16(int a, int b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000250B9 File Offset: 0x000232B9
		[MethodImpl(256)]
		internal static int MAX16(int a, int b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000250C2 File Offset: 0x000232C2
		[MethodImpl(256)]
		internal static float MIN16(float a, float b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000250CB File Offset: 0x000232CB
		[MethodImpl(256)]
		internal static float MAX16(float a, float b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000250D4 File Offset: 0x000232D4
		[MethodImpl(256)]
		internal static int MIN(int a, int b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000250DD File Offset: 0x000232DD
		[MethodImpl(256)]
		internal static int MAX(int a, int b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000250E6 File Offset: 0x000232E6
		[MethodImpl(256)]
		internal static int IMIN(int a, int b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000250EF File Offset: 0x000232EF
		[MethodImpl(256)]
		internal static uint IMIN(uint a, uint b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000250F8 File Offset: 0x000232F8
		[MethodImpl(256)]
		internal static int IMAX(int a, int b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00025101 File Offset: 0x00023301
		[MethodImpl(256)]
		internal static int MIN32(int a, int b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0002510A File Offset: 0x0002330A
		[MethodImpl(256)]
		internal static int MAX32(int a, int b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00025113 File Offset: 0x00023313
		[MethodImpl(256)]
		internal static float MIN32(float a, float b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0002511C File Offset: 0x0002331C
		[MethodImpl(256)]
		internal static float MAX32(float a, float b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00025125 File Offset: 0x00023325
		[MethodImpl(256)]
		internal static int ABS16(int x)
		{
			if (x >= 0)
			{
				return x;
			}
			return -x;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0002512F File Offset: 0x0002332F
		[MethodImpl(256)]
		internal static float ABS16(float x)
		{
			if (x >= 0f)
			{
				return x;
			}
			return -x;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0002513D File Offset: 0x0002333D
		[MethodImpl(256)]
		internal static short ABS16(short x)
		{
			return (x < 0) ? (-x) : x;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00025149 File Offset: 0x00023349
		[MethodImpl(256)]
		internal static int ABS32(int x)
		{
			if (x >= 0)
			{
				return x;
			}
			return -x;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00025153 File Offset: 0x00023353
		[MethodImpl(256)]
		internal static uint celt_udiv(uint n, uint d)
		{
			return n / d;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00025158 File Offset: 0x00023358
		[MethodImpl(256)]
		internal static int celt_udiv(int n, int d)
		{
			return n / d;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0002515D File Offset: 0x0002335D
		[MethodImpl(256)]
		internal static int celt_sudiv(int n, int d)
		{
			return n / d;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00025162 File Offset: 0x00023362
		[MethodImpl(256)]
		internal static int celt_div(int a, int b)
		{
			return Inlines.MULT32_32_Q31(a, Inlines.celt_rcp(b));
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00025170 File Offset: 0x00023370
		[MethodImpl(256)]
		internal static int celt_ilog2(int x)
		{
			return Inlines.EC_ILOG((uint)x) - 1;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0002517A File Offset: 0x0002337A
		[MethodImpl(256)]
		internal static int celt_zlog2(int x)
		{
			if (x > 0)
			{
				return Inlines.celt_ilog2(x);
			}
			return 0;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00025188 File Offset: 0x00023388
		[MethodImpl(256)]
		internal unsafe static int celt_maxabs16(Span<int> x, int x_ptr, int len)
		{
			int num = 0;
			int num2 = 0;
			for (int i = x_ptr; i < len + x_ptr; i++)
			{
				num = Inlines.MAX32(num, *x[i]);
				num2 = Inlines.MIN32(num2, *x[i]);
			}
			return Inlines.MAX32(Inlines.EXTEND32(num), -Inlines.EXTEND32(num2));
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000251DC File Offset: 0x000233DC
		[MethodImpl(256)]
		internal unsafe static int celt_maxabs32(Span<int> x, int len)
		{
			int a = 0;
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				a = Inlines.MAX32(a, *x[i]);
				num = Inlines.MIN32(num, *x[i]);
			}
			return Inlines.MAX32(a, 0 - num);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00025224 File Offset: 0x00023424
		[MethodImpl(256)]
		internal unsafe static int celt_maxabs32(Span<int> x, int x_ptr, int len)
		{
			int a = 0;
			int num = 0;
			for (int i = x_ptr; i < x_ptr + len; i++)
			{
				a = Inlines.MAX32(a, *x[i]);
				num = Inlines.MIN32(num, *x[i]);
			}
			return Inlines.MAX32(a, 0 - num);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0002526C File Offset: 0x0002346C
		[MethodImpl(256)]
		internal unsafe static short celt_maxabs32(Span<short> x, int x_ptr, int len)
		{
			short a = 0;
			short num = 0;
			for (int i = x_ptr; i < x_ptr + len; i++)
			{
				a = Inlines.MAX16(a, *x[i]);
				num = Inlines.MIN16(num, *x[i]);
			}
			return Inlines.MAX(a, 0 - num);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x000252B5 File Offset: 0x000234B5
		[MethodImpl(256)]
		internal static int FRAC_MUL16(int a, int b)
		{
			return 16384 + (short)a * (short)b >> 15;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000252C8 File Offset: 0x000234C8
		[MethodImpl(256)]
		internal static uint isqrt32(uint _val)
		{
			uint num = 0U;
			int num2 = Inlines.EC_ILOG(_val) - 1 >> 1;
			uint num3 = 1U << num2;
			do
			{
				uint num4 = (num << 1) + num3 << num2;
				if (num4 <= _val)
				{
					num += num3;
					_val -= num4;
				}
				num3 >>= 1;
				num2--;
			}
			while (num2 >= 0);
			return num;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00025310 File Offset: 0x00023510
		[MethodImpl(256)]
		internal static int celt_sqrt(int x)
		{
			if (x == 0)
			{
				return 0;
			}
			if (x >= 1073741824)
			{
				return 32767;
			}
			int num = (Inlines.celt_ilog2(x) >> 1) - 7;
			x = Inlines.VSHR32(x, 2 * num);
			short a = (short)(x - 32768);
			return Inlines.VSHR32((int)Inlines.ADD16(Inlines.sqrt_C[0], Inlines.MULT16_16_Q15(a, Inlines.ADD16(Inlines.sqrt_C[1], Inlines.MULT16_16_Q15(a, Inlines.ADD16(Inlines.sqrt_C[2], Inlines.MULT16_16_Q15(a, Inlines.ADD16(Inlines.sqrt_C[3], Inlines.MULT16_16_Q15(a, Inlines.sqrt_C[4])))))))), 7 - num);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x000253A8 File Offset: 0x000235A8
		[MethodImpl(256)]
		internal static int celt_rcp(int x)
		{
			int num = Inlines.celt_ilog2(x);
			int b = Inlines.VSHR32(x, num - 15) - 32768;
			int num2 = Inlines.ADD16(30840, Inlines.MULT16_16_Q15(-15420, b));
			num2 = Inlines.SUB16(num2, Inlines.MULT16_16_Q15(num2, Inlines.ADD16(Inlines.MULT16_16_Q15(num2, b), Inlines.ADD16(num2, -32768))));
			num2 = Inlines.SUB16(num2, Inlines.ADD16(1, Inlines.MULT16_16_Q15(num2, Inlines.ADD16(Inlines.MULT16_16_Q15(num2, b), Inlines.ADD16(num2, -32768)))));
			return Inlines.VSHR32(Inlines.EXTEND32(num2), num - 16);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00025440 File Offset: 0x00023640
		[MethodImpl(256)]
		internal static int celt_rsqrt_norm(int x)
		{
			int num = x - 32768;
			int num2 = Inlines.ADD16(23557, Inlines.MULT16_16_Q15(num, Inlines.ADD16(-13490, Inlines.MULT16_16_Q15(num, 6713))));
			int num3 = Inlines.MULT16_16_Q15(num2, num2);
			int a = Inlines.SHL16(Inlines.SUB16(Inlines.ADD16(Inlines.MULT16_16_Q15(num3, num), num3), 16384), 1);
			return Inlines.ADD16(num2, Inlines.MULT16_16_Q15(num2, Inlines.MULT16_16_Q15(a, Inlines.SUB16(Inlines.MULT16_16_Q15(a, 12288), 16384))));
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x000254C8 File Offset: 0x000236C8
		[MethodImpl(256)]
		internal static int frac_div32(int a, int b)
		{
			int shift = Inlines.celt_ilog2(b) - 29;
			a = Inlines.VSHR32(a, shift);
			b = Inlines.VSHR32(b, shift);
			int a2 = Inlines.ROUND16(Inlines.celt_rcp(Inlines.ROUND16(b, 16)), 3);
			int num = Inlines.MULT16_32_Q15(a2, a);
			int b2 = Inlines.PSHR32(a, 2) - Inlines.MULT32_32_Q31(num, b);
			num = Inlines.ADD32(num, Inlines.SHL32(Inlines.MULT16_32_Q15(a2, b2), 2));
			if (num >= 536870912)
			{
				return int.MaxValue;
			}
			if (num <= -536870912)
			{
				return -2147483647;
			}
			return Inlines.SHL32(num, 2);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00025554 File Offset: 0x00023754
		[MethodImpl(256)]
		internal static int celt_log2(int x)
		{
			if (x == 0)
			{
				return -32767;
			}
			int num = Inlines.celt_ilog2(x);
			int a = Inlines.VSHR32(x, num - 15) - 32768 - 16384;
			int a2 = Inlines.ADD16(-6793, Inlines.MULT16_16_Q15(a, Inlines.ADD16(15746, Inlines.MULT16_16_Q15(a, Inlines.ADD16(-5217, Inlines.MULT16_16_Q15(a, Inlines.ADD16(2545, Inlines.MULT16_16_Q15(a, -1401))))))));
			return (int)Inlines.SHL16((short)(num - 13), 10) + Inlines.SHR16(a2, 4);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x000255E4 File Offset: 0x000237E4
		[MethodImpl(256)]
		internal static int celt_exp2_frac(int x)
		{
			int num = Inlines.SHL16(x, 4);
			return Inlines.ADD16(16383, Inlines.MULT16_16_Q15(num, Inlines.ADD16(22804, Inlines.MULT16_16_Q15(num, Inlines.ADD16(14819, Inlines.MULT16_16_Q15(10204, num))))));
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00025630 File Offset: 0x00023830
		[MethodImpl(256)]
		internal static int celt_exp2(int x)
		{
			int num = Inlines.SHR16(x, 10);
			if (num > 14)
			{
				return 2130706432;
			}
			if (num < -15)
			{
				return 0;
			}
			return Inlines.VSHR32(Inlines.EXTEND32((int)((short)Inlines.celt_exp2_frac((int)((short)(x - (int)Inlines.SHL16((short)num, 10)))))), -num - 2);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00025678 File Offset: 0x00023878
		[MethodImpl(256)]
		internal static int celt_atan01(int x)
		{
			return Inlines.MULT16_16_P15(x, Inlines.ADD32(32767, Inlines.MULT16_16_P15(x, Inlines.ADD32(-21, Inlines.MULT16_16_P15(x, Inlines.ADD32(-11943, Inlines.MULT16_16_P15(4936, x)))))));
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000256B4 File Offset: 0x000238B4
		[MethodImpl(256)]
		internal static int celt_atan2p(int y, int x)
		{
			if (y < x)
			{
				int num = Inlines.celt_div(Inlines.SHL32(Inlines.EXTEND32(y), 15), x);
				if (num >= 32767)
				{
					num = 32767;
				}
				return Inlines.SHR32(Inlines.celt_atan01((int)Inlines.EXTRACT16(num)), 1);
			}
			int num2 = Inlines.celt_div(Inlines.SHL32(Inlines.EXTEND32(x), 15), y);
			if (num2 >= 32767)
			{
				num2 = 32767;
			}
			return 25736 - Inlines.SHR16(Inlines.celt_atan01((int)Inlines.EXTRACT16(num2)), 1);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00025734 File Offset: 0x00023934
		[MethodImpl(256)]
		internal static int celt_cos_norm(int x)
		{
			x &= 131071;
			if (x > Inlines.SHL32(Inlines.EXTEND32(1), 16))
			{
				x = Inlines.SUB32(Inlines.SHL32(Inlines.EXTEND32(1), 17), x);
			}
			if ((x & 32767) != 0)
			{
				if (x < Inlines.SHL32(Inlines.EXTEND32(1), 15))
				{
					return Inlines._celt_cos_pi_2((int)Inlines.EXTRACT16(x));
				}
				return Inlines.NEG32(Inlines._celt_cos_pi_2((int)Inlines.EXTRACT16(65536 - x)));
			}
			else
			{
				if ((x & 65535) != 0)
				{
					return 0;
				}
				if ((x & 131071) != 0)
				{
					return -32767;
				}
				return 32767;
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000257CC File Offset: 0x000239CC
		[MethodImpl(256)]
		internal static int _celt_cos_pi_2(int x)
		{
			int num = Inlines.MULT16_16_P15(x, x);
			return Inlines.ADD32(1, Inlines.MIN32(32766, Inlines.ADD32(Inlines.SUB16(32767, num), Inlines.MULT16_16_P15(num, Inlines.ADD32(-7651, Inlines.MULT16_16_P15(num, Inlines.ADD32(8277, Inlines.MULT16_16_P15(-626, num))))))));
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0002582C File Offset: 0x00023A2C
		[MethodImpl(256)]
		internal static short FLOAT2INT16(float x)
		{
			x *= 32768f;
			if (x < -32768f)
			{
				x = -32768f;
			}
			if (x > 32767f)
			{
				x = 32767f;
			}
			return (short)x;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00025857 File Offset: 0x00023A57
		[MethodImpl(256)]
		internal static int silk_ROR32(int a32, int rot)
		{
			return (int)Inlines.silk_ROR32((uint)a32, rot);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00025860 File Offset: 0x00023A60
		[MethodImpl(256)]
		internal static uint silk_ROR32(uint a32, int rot)
		{
			int num = 0 - rot;
			if (rot == 0)
			{
				return a32;
			}
			if (rot < 0)
			{
				return a32 << num | a32 >> 32 - num;
			}
			return a32 << 32 - rot | a32 >> rot;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0002589B File Offset: 0x00023A9B
		[MethodImpl(256)]
		internal static int silk_MUL(int a32, int b32)
		{
			return a32 * b32;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000258A0 File Offset: 0x00023AA0
		[MethodImpl(256)]
		internal static uint silk_MUL_uint(uint a32, uint b32)
		{
			return a32 * b32;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x000258A5 File Offset: 0x00023AA5
		[MethodImpl(256)]
		internal static int silk_MLA(int a32, int b32, int c32)
		{
			return Inlines.silk_ADD32(a32, b32 * c32);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000258B0 File Offset: 0x00023AB0
		[MethodImpl(256)]
		internal static int silk_MLA_uint(uint a32, uint b32, uint c32)
		{
			return (int)Inlines.silk_ADD32(a32, b32 * c32);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000258BB File Offset: 0x00023ABB
		[MethodImpl(256)]
		internal static int silk_SMULTT(int a32, int b32)
		{
			return (a32 >> 16) * (b32 >> 16);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000258C6 File Offset: 0x00023AC6
		[MethodImpl(256)]
		internal static int silk_SMLATT(int a32, int b32, int c32)
		{
			return Inlines.silk_ADD32(a32, (b32 >> 16) * (c32 >> 16));
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000258D7 File Offset: 0x00023AD7
		[MethodImpl(256)]
		internal static long silk_SMLALBB(long a64, short b16, short c16)
		{
			return Inlines.silk_ADD64(a64, (long)(b16 * c16));
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000258E3 File Offset: 0x00023AE3
		[MethodImpl(256)]
		internal static long silk_SMULL(int a32, int b32)
		{
			return (long)a32 * (long)b32;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000258EA File Offset: 0x00023AEA
		[MethodImpl(256)]
		internal static int silk_ADD32_ovflw(int a, int b)
		{
			return a + b;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000258EF File Offset: 0x00023AEF
		[MethodImpl(256)]
		internal static int silk_ADD32_ovflw(uint a, uint b)
		{
			return (int)(a + b);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000258F4 File Offset: 0x00023AF4
		[MethodImpl(256)]
		internal static int silk_SUB32_ovflw(int a, int b)
		{
			return a - b;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000258F9 File Offset: 0x00023AF9
		[MethodImpl(256)]
		internal static int silk_MLA_ovflw(int a32, int b32, int c32)
		{
			return Inlines.silk_ADD32_ovflw((uint)a32, (uint)(b32 * c32));
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00025904 File Offset: 0x00023B04
		[MethodImpl(256)]
		internal static int silk_SMLABB_ovflw(int a32, int b32, int c32)
		{
			return Inlines.silk_ADD32_ovflw(a32, (int)((short)b32 * (short)c32));
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00025911 File Offset: 0x00023B11
		[MethodImpl(256)]
		internal static int silk_SMULBB(int a32, int b32)
		{
			return (int)((short)a32 * (short)b32);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00025918 File Offset: 0x00023B18
		[MethodImpl(256)]
		internal static int silk_SMULWB(int a32, int b32)
		{
			return (int)((long)a32 * (long)((short)b32) >> 16);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00025924 File Offset: 0x00023B24
		[MethodImpl(256)]
		internal static int silk_SMLABB(int a32, int b32, int c32)
		{
			return a32 + (int)((short)b32 * (short)c32);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0002592D File Offset: 0x00023B2D
		[MethodImpl(256)]
		internal static int silk_DIV32_16(int a32, int b32)
		{
			return a32 / b32;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00025932 File Offset: 0x00023B32
		[MethodImpl(256)]
		internal static int silk_DIV32(int a32, int b32)
		{
			return a32 / b32;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00025937 File Offset: 0x00023B37
		[MethodImpl(256)]
		internal static short silk_ADD16(short a, short b)
		{
			return a + b;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0002593D File Offset: 0x00023B3D
		[MethodImpl(256)]
		internal static int silk_ADD32(int a, int b)
		{
			return a + b;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00025942 File Offset: 0x00023B42
		[MethodImpl(256)]
		internal static uint silk_ADD32(uint a, uint b)
		{
			return a + b;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00025947 File Offset: 0x00023B47
		[MethodImpl(256)]
		internal static long silk_ADD64(long a, long b)
		{
			return a + b;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0002594C File Offset: 0x00023B4C
		[MethodImpl(256)]
		internal static short silk_SUB16(short a, short b)
		{
			return a - b;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00025952 File Offset: 0x00023B52
		[MethodImpl(256)]
		internal static int silk_SUB32(int a, int b)
		{
			return a - b;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00025957 File Offset: 0x00023B57
		[MethodImpl(256)]
		internal static long silk_SUB64(long a, long b)
		{
			return a - b;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0002595C File Offset: 0x00023B5C
		[MethodImpl(256)]
		internal static int silk_SAT8(int a)
		{
			if (a > 255)
			{
				return 255;
			}
			if (a >= 0)
			{
				return a;
			}
			return 0;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00025973 File Offset: 0x00023B73
		[MethodImpl(256)]
		internal static int silk_SAT16(int a)
		{
			if (a > 32767)
			{
				return 32767;
			}
			if (a >= -32768)
			{
				return a;
			}
			return -32768;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00025992 File Offset: 0x00023B92
		[MethodImpl(256)]
		internal static int silk_SAT32(long a)
		{
			if (a > 2147483647L)
			{
				return int.MaxValue;
			}
			if (a >= -2147483648L)
			{
				return (int)a;
			}
			return int.MinValue;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000259B4 File Offset: 0x00023BB4
		[MethodImpl(256)]
		internal static short silk_ADD_SAT16(short a16, short b16)
		{
			return (short)Inlines.silk_SAT16(Inlines.silk_ADD32((int)a16, (int)b16));
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000259C3 File Offset: 0x00023BC3
		[MethodImpl(256)]
		internal static int silk_ADD_SAT32(int a32, int b32)
		{
			if ((a32 + b32 & -2147483648) != 0)
			{
				if (((long)(a32 | b32) & (long)((ulong)-2147483648)) != 0L)
				{
					return a32 + b32;
				}
				return int.MaxValue;
			}
			else
			{
				if (((long)(a32 & b32) & (long)((ulong)-2147483648)) == 0L)
				{
					return a32 + b32;
				}
				return int.MinValue;
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00025A00 File Offset: 0x00023C00
		[MethodImpl(256)]
		internal static long silk_ADD_SAT64(long a64, long b64)
		{
			if ((a64 + b64 & -9223372036854775808L) != 0L)
			{
				if (((a64 | b64) & -9223372036854775808L) != 0L)
				{
					return a64 + b64;
				}
				return long.MaxValue;
			}
			else
			{
				if ((a64 & b64 & -9223372036854775808L) == 0L)
				{
					return a64 + b64;
				}
				return long.MinValue;
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00025A55 File Offset: 0x00023C55
		[MethodImpl(256)]
		internal static short silk_SUB_SAT16(short a16, short b16)
		{
			return (short)Inlines.silk_SAT16(Inlines.silk_SUB32((int)a16, (int)b16));
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00025A64 File Offset: 0x00023C64
		[MethodImpl(256)]
		internal static int silk_SUB_SAT32(int a32, int b32)
		{
			if ((a32 - b32 & -2147483648) != 0)
			{
				if ((((long)a32 ^ (long)((ulong)-2147483648)) & (long)b32 & (long)((ulong)-2147483648)) == 0L)
				{
					return a32 - b32;
				}
				return int.MaxValue;
			}
			else
			{
				if (((long)a32 & ((long)b32 ^ (long)((ulong)-2147483648)) & (long)((ulong)-2147483648)) == 0L)
				{
					return a32 - b32;
				}
				return int.MinValue;
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00025ABC File Offset: 0x00023CBC
		[MethodImpl(256)]
		internal static long silk_SUB_SAT64(long a64, long b64)
		{
			if ((a64 - b64 & -9223372036854775808L) != 0L)
			{
				if (((a64 ^ -9223372036854775808L) & b64 & -9223372036854775808L) == 0L)
				{
					return a64 - b64;
				}
				return long.MaxValue;
			}
			else
			{
				if ((a64 & (b64 ^ -9223372036854775808L) & -9223372036854775808L) == 0L)
				{
					return a64 - b64;
				}
				return long.MinValue;
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00025B25 File Offset: 0x00023D25
		[MethodImpl(256)]
		internal static sbyte silk_ADD_POS_SAT8(sbyte a, sbyte b)
		{
			return (((int)(a + b) & 128) != 0) ? sbyte.MaxValue : (a + b);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00025B3A File Offset: 0x00023D3A
		[MethodImpl(256)]
		internal static short silk_ADD_POS_SAT16(short a, short b)
		{
			return (((int)(a + b) & 32768) != 0) ? short.MaxValue : (a + b);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00025B52 File Offset: 0x00023D52
		[MethodImpl(256)]
		internal static int silk_ADD_POS_SAT32(int a, int b)
		{
			if (((long)(a + b) & (long)((ulong)-2147483648)) == 0L)
			{
				return a + b;
			}
			return int.MaxValue;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00025B6A File Offset: 0x00023D6A
		[MethodImpl(256)]
		internal static long silk_ADD_POS_SAT64(long a, long b)
		{
			if ((a + b & -9223372036854775808L) == 0L)
			{
				return a + b;
			}
			return long.MaxValue;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00025B88 File Offset: 0x00023D88
		[MethodImpl(256)]
		internal static sbyte silk_LSHIFT8(sbyte a, int shift)
		{
			return (sbyte)(a << shift);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00025B91 File Offset: 0x00023D91
		[MethodImpl(256)]
		internal static short silk_LSHIFT16(short a, int shift)
		{
			return (short)(a << shift);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00025B9A File Offset: 0x00023D9A
		[MethodImpl(256)]
		internal static int silk_LSHIFT32(int a, int shift)
		{
			return a << shift;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00025BA2 File Offset: 0x00023DA2
		[MethodImpl(256)]
		internal static long silk_LSHIFT64(long a, int shift)
		{
			return a << shift;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00025BAA File Offset: 0x00023DAA
		[MethodImpl(256)]
		internal static int silk_LSHIFT(int a, int shift)
		{
			return a << shift;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00025BB2 File Offset: 0x00023DB2
		[MethodImpl(256)]
		internal static int silk_LSHIFT_ovflw(int a, int shift)
		{
			return a << shift;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00025BBA File Offset: 0x00023DBA
		[MethodImpl(256)]
		internal static uint silk_LSHIFT_uint(uint a, int shift)
		{
			return a << shift;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00025BC2 File Offset: 0x00023DC2
		[MethodImpl(256)]
		internal static int silk_LSHIFT_SAT32(int a, int shift)
		{
			return Inlines.silk_LSHIFT32(Inlines.silk_LIMIT(a, Inlines.silk_RSHIFT32(int.MinValue, shift), Inlines.silk_RSHIFT32(int.MaxValue, shift)), shift);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00025BE6 File Offset: 0x00023DE6
		[MethodImpl(256)]
		internal static sbyte silk_RSHIFT8(sbyte a, int shift)
		{
			return (sbyte)(a >> shift);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00025BEF File Offset: 0x00023DEF
		[MethodImpl(256)]
		internal static short silk_RSHIFT16(short a, int shift)
		{
			return (short)(a >> shift);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00025BF8 File Offset: 0x00023DF8
		[MethodImpl(256)]
		internal static int silk_RSHIFT32(int a, int shift)
		{
			return a >> shift;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00025C00 File Offset: 0x00023E00
		[MethodImpl(256)]
		internal static int silk_RSHIFT(int a, int shift)
		{
			return a >> shift;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00025C08 File Offset: 0x00023E08
		[MethodImpl(256)]
		internal static long silk_RSHIFT64(long a, int shift)
		{
			return a >> shift;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00025C10 File Offset: 0x00023E10
		[MethodImpl(256)]
		internal static uint silk_RSHIFT_uint(uint a, int shift)
		{
			return a >> shift;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00025C18 File Offset: 0x00023E18
		[MethodImpl(256)]
		internal static int silk_ADD_LSHIFT(int a, int b, int shift)
		{
			return a + (b << shift);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00025C22 File Offset: 0x00023E22
		[MethodImpl(256)]
		internal static int silk_ADD_LSHIFT32(int a, int b, int shift)
		{
			return a + (b << shift);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00025C2C File Offset: 0x00023E2C
		[MethodImpl(256)]
		internal static uint silk_ADD_LSHIFT_uint(uint a, uint b, int shift)
		{
			return a + (b << shift);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00025C36 File Offset: 0x00023E36
		[MethodImpl(256)]
		internal static int silk_ADD_RSHIFT(int a, int b, int shift)
		{
			return a + (b >> shift);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00025C40 File Offset: 0x00023E40
		[MethodImpl(256)]
		internal static int silk_ADD_RSHIFT32(int a, int b, int shift)
		{
			return a + (b >> shift);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00025C4A File Offset: 0x00023E4A
		[MethodImpl(256)]
		internal static uint silk_ADD_RSHIFT_uint(uint a, uint b, int shift)
		{
			return a + (b >> shift);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00025C54 File Offset: 0x00023E54
		[MethodImpl(256)]
		internal static int silk_SUB_LSHIFT32(int a, int b, int shift)
		{
			return a - (b << shift);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00025C5E File Offset: 0x00023E5E
		[MethodImpl(256)]
		internal static int silk_SUB_RSHIFT32(int a, int b, int shift)
		{
			return a - (b >> shift);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00025C68 File Offset: 0x00023E68
		[MethodImpl(256)]
		internal static int silk_RSHIFT_ROUND(int a, int shift)
		{
			if (shift != 1)
			{
				return (a >> shift - 1) + 1 >> 1;
			}
			return (a >> 1) + (a & 1);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00025C82 File Offset: 0x00023E82
		[MethodImpl(256)]
		internal static long silk_RSHIFT_ROUND64(long a, int shift)
		{
			if (shift != 1)
			{
				return (a >> shift - 1) + 1L >> 1;
			}
			return (a >> 1) + (a & 1L);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00025C9E File Offset: 0x00023E9E
		[MethodImpl(256)]
		internal static int silk_min(int a, int b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00025CA7 File Offset: 0x00023EA7
		[MethodImpl(256)]
		internal static int silk_max(int a, int b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00025CB0 File Offset: 0x00023EB0
		[MethodImpl(256)]
		internal static float silk_min(float a, float b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00025CB9 File Offset: 0x00023EB9
		[MethodImpl(256)]
		internal static float silk_max(float a, float b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00025CC2 File Offset: 0x00023EC2
		[MethodImpl(256)]
		internal static int SILK_CONST(float number, int scale)
		{
			return (int)((double)(number * (float)(1L << scale)) + 0.5);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00025CDA File Offset: 0x00023EDA
		[MethodImpl(256)]
		internal static int silk_min_int(int a, int b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00025CE3 File Offset: 0x00023EE3
		[MethodImpl(256)]
		internal static short silk_min_16(short a, short b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00025CEC File Offset: 0x00023EEC
		[MethodImpl(256)]
		internal static int silk_min_32(int a, int b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00025CF5 File Offset: 0x00023EF5
		[MethodImpl(256)]
		internal static long silk_min_64(long a, long b)
		{
			if (a >= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00025CFE File Offset: 0x00023EFE
		[MethodImpl(256)]
		internal static int silk_max_int(int a, int b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00025D07 File Offset: 0x00023F07
		[MethodImpl(256)]
		internal static short silk_max_16(short a, short b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00025D10 File Offset: 0x00023F10
		[MethodImpl(256)]
		internal static int silk_max_32(int a, int b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00025D19 File Offset: 0x00023F19
		[MethodImpl(256)]
		internal static long silk_max_64(long a, long b)
		{
			if (a <= b)
			{
				return b;
			}
			return a;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00025D22 File Offset: 0x00023F22
		[MethodImpl(256)]
		internal static float silk_LIMIT(float a, float limit1, float limit2)
		{
			if (limit1 <= limit2)
			{
				if (a > limit2)
				{
					return limit2;
				}
				if (a >= limit1)
				{
					return a;
				}
				return limit1;
			}
			else
			{
				if (a > limit1)
				{
					return limit1;
				}
				if (a >= limit2)
				{
					return a;
				}
				return limit2;
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00025D43 File Offset: 0x00023F43
		[MethodImpl(256)]
		internal static int silk_LIMIT(int a, int limit1, int limit2)
		{
			return Inlines.silk_LIMIT_32(a, limit1, limit2);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00025D4D File Offset: 0x00023F4D
		[MethodImpl(256)]
		internal static int silk_LIMIT_int(int a, int limit1, int limit2)
		{
			return Inlines.silk_LIMIT_32(a, limit1, limit2);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00025D57 File Offset: 0x00023F57
		[MethodImpl(256)]
		internal static short silk_LIMIT_16(short a, short limit1, short limit2)
		{
			if (limit1 <= limit2)
			{
				if (a > limit2)
				{
					return limit2;
				}
				if (a >= limit1)
				{
					return a;
				}
				return limit1;
			}
			else
			{
				if (a > limit1)
				{
					return limit1;
				}
				if (a >= limit2)
				{
					return a;
				}
				return limit2;
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00025D78 File Offset: 0x00023F78
		[MethodImpl(256)]
		internal static int silk_LIMIT_32(int a, int limit1, int limit2)
		{
			if (limit1 <= limit2)
			{
				if (a > limit2)
				{
					return limit2;
				}
				if (a >= limit1)
				{
					return a;
				}
				return limit1;
			}
			else
			{
				if (a > limit1)
				{
					return limit1;
				}
				if (a >= limit2)
				{
					return a;
				}
				return limit2;
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00025D99 File Offset: 0x00023F99
		[MethodImpl(256)]
		internal static int silk_abs(int a)
		{
			if (a <= 0)
			{
				return -a;
			}
			return a;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00025DA3 File Offset: 0x00023FA3
		[MethodImpl(256)]
		internal static int silk_abs_int16(int a)
		{
			return (a ^ a >> 15) - (a >> 15);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00025DB0 File Offset: 0x00023FB0
		[MethodImpl(256)]
		internal static int silk_abs_int32(int a)
		{
			return (a ^ a >> 31) - (a >> 31);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00025DBD File Offset: 0x00023FBD
		[MethodImpl(256)]
		internal static long silk_abs_int64(long a)
		{
			if (a <= 0L)
			{
				return -a;
			}
			return a;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00025DC8 File Offset: 0x00023FC8
		[MethodImpl(256)]
		internal static long silk_sign(int a)
		{
			return (a > 0) ? 1L : ((a < 0) ? -1L : 0L);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00025DDA File Offset: 0x00023FDA
		[MethodImpl(256)]
		internal static int silk_RAND(int seed)
		{
			return Inlines.silk_MLA_ovflw(907633515, seed, 196314165);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00025DEC File Offset: 0x00023FEC
		[MethodImpl(256)]
		internal static int silk_SMMUL(int a32, int b32)
		{
			return (int)Inlines.silk_RSHIFT64(Inlines.silk_SMULL(a32, b32), 32);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00025DFD File Offset: 0x00023FFD
		[MethodImpl(256)]
		internal static int silk_SMLAWT(int a32, int b32, int c32)
		{
			return a32 + (b32 >> 16) * (c32 >> 16) + ((b32 & 65535) * (c32 >> 16) >> 16);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00025E1C File Offset: 0x0002401C
		[MethodImpl(256)]
		internal static int silk_DIV32_varQ(int a32, int b32, int Qres)
		{
			int num = Inlines.silk_CLZ32(Inlines.silk_abs(a32)) - 1;
			int num2 = Inlines.silk_LSHIFT(a32, num);
			int num3 = Inlines.silk_CLZ32(Inlines.silk_abs(b32)) - 1;
			int num4 = Inlines.silk_LSHIFT(b32, num3);
			int num5 = Inlines.silk_DIV32_16(536870911, Inlines.silk_RSHIFT(num4, 16));
			int num6 = Inlines.silk_SMULWB(num2, num5);
			num2 = Inlines.silk_SUB32_ovflw(num2, Inlines.silk_LSHIFT_ovflw(Inlines.silk_SMMUL(num4, num6), 3));
			num6 = Inlines.silk_SMLAWB(num6, num2, num5);
			int num7 = 29 + num - num3 - Qres;
			if (num7 < 0)
			{
				return Inlines.silk_LSHIFT_SAT32(num6, -num7);
			}
			if (num7 < 32)
			{
				return Inlines.silk_RSHIFT(num6, num7);
			}
			return 0;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00025EC0 File Offset: 0x000240C0
		[MethodImpl(256)]
		internal static int silk_INVERSE32_varQ(int b32, int Qres)
		{
			int num = Inlines.silk_CLZ32(Inlines.silk_abs(b32)) - 1;
			int num2 = Inlines.silk_LSHIFT(b32, num);
			int num3 = Inlines.silk_DIV32_16(536870911, (int)((short)Inlines.silk_RSHIFT(num2, 16)));
			int num4 = Inlines.silk_LSHIFT(num3, 16);
			int b33 = Inlines.silk_LSHIFT(536870912 - Inlines.silk_SMULWB(num2, num3), 3);
			num4 = Inlines.silk_SMLAWW(num4, b33, num3);
			int num5 = 61 - num - Qres;
			if (num5 <= 0)
			{
				return Inlines.silk_LSHIFT_SAT32(num4, -num5);
			}
			if (num5 < 32)
			{
				return Inlines.silk_RSHIFT(num4, num5);
			}
			return 0;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00025F46 File Offset: 0x00024146
		[MethodImpl(256)]
		internal static int silk_SMLAWB(int a32, int b32, int c32)
		{
			return a32 + Inlines.silk_SMULWB(b32, c32);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00025F51 File Offset: 0x00024151
		[MethodImpl(256)]
		internal static int silk_SMULWT(int a32, int b32)
		{
			return (a32 >> 16) * (b32 >> 16) + ((a32 & 65535) * (b32 >> 16) >> 16);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00025F6C File Offset: 0x0002416C
		[MethodImpl(256)]
		internal static int silk_SMULBT(int a32, int b32)
		{
			return (int)((short)a32) * (b32 >> 16);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00025F75 File Offset: 0x00024175
		[MethodImpl(256)]
		internal static int silk_SMLABT(int a32, int b32, int c32)
		{
			return a32 + (int)((short)b32) * (c32 >> 16);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00025F80 File Offset: 0x00024180
		[MethodImpl(256)]
		internal static long silk_SMLAL(long a64, int b32, int c32)
		{
			return Inlines.silk_ADD64(a64, (long)b32 * (long)c32);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00025F8D File Offset: 0x0002418D
		[MethodImpl(256)]
		internal static void MatrixSet<T>(T[] Matrix_base_adr, int Matrix_ptr, int row, int column, int N, T value)
		{
			Matrix_base_adr[Matrix_ptr + row * N + column] = value;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00025F9F File Offset: 0x0002419F
		[MethodImpl(256)]
		internal static int MatrixGetPointer(int row, int column, int N)
		{
			return row * N + column;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00025FA6 File Offset: 0x000241A6
		[MethodImpl(256)]
		internal static T MatrixGet<T>(T[] Matrix_base_adr, int row, int column, int N)
		{
			return Matrix_base_adr[row * N + column];
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00025FB3 File Offset: 0x000241B3
		internal static T MatrixGet<T>(T[] Matrix_base_adr, int matrix_ptr, int row, int column, int N)
		{
			return Matrix_base_adr[matrix_ptr + row * N + column];
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00025FC3 File Offset: 0x000241C3
		[MethodImpl(256)]
		internal static void MatrixSet<T>(T[] Matrix_base_adr, int row, int column, int N, T value)
		{
			Matrix_base_adr[row * N + column] = value;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00025FD2 File Offset: 0x000241D2
		[MethodImpl(256)]
		internal static int silk_SMULWW(int a32, int b32)
		{
			return Inlines.silk_MLA(Inlines.silk_SMULWB(a32, b32), a32, Inlines.silk_RSHIFT_ROUND(b32, 16));
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00025FE9 File Offset: 0x000241E9
		[MethodImpl(256)]
		internal static int silk_SMLAWW(int a32, int b32, int c32)
		{
			return Inlines.silk_MLA(Inlines.silk_SMLAWB(a32, b32, c32), b32, Inlines.silk_RSHIFT_ROUND(c32, 16));
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00026004 File Offset: 0x00024204
		[MethodImpl(256)]
		internal static int silk_CLZ64(long input)
		{
			int num = (int)Inlines.silk_RSHIFT64(input, 32);
			if (num == 0)
			{
				return 32 + Inlines.silk_CLZ32((int)input);
			}
			return Inlines.silk_CLZ32(num);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0002602F File Offset: 0x0002422F
		[MethodImpl(256)]
		internal static int silk_CLZ32(int in32)
		{
			if (in32 != 0)
			{
				return 32 - Inlines.EC_ILOG((uint)in32);
			}
			return 32;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00026040 File Offset: 0x00024240
		[MethodImpl(256)]
		internal static void silk_CLZ_FRAC(int input, out int lz, out int frac_Q7)
		{
			int num = Inlines.silk_CLZ32(input);
			lz = num;
			frac_Q7 = (Inlines.silk_ROR32(input, 24 - num) & 127);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00026068 File Offset: 0x00024268
		[MethodImpl(256)]
		internal static int silk_SQRT_APPROX(int x)
		{
			if (x <= 0)
			{
				return 0;
			}
			int num;
			int b;
			Inlines.silk_CLZ_FRAC(x, out num, out b);
			int num2;
			if ((num & 1) != 0)
			{
				num2 = 32768;
			}
			else
			{
				num2 = 46214;
			}
			num2 >>= Inlines.silk_RSHIFT(num, 1);
			return Inlines.silk_SMLAWB(num2, num2, Inlines.silk_SMULBB(213, b));
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x000260B9 File Offset: 0x000242B9
		[MethodImpl(256)]
		internal static int MUL32_FRAC_Q(int a32, int b32, int Q)
		{
			return (int)Inlines.silk_RSHIFT_ROUND64(Inlines.silk_SMULL(a32, b32), Q);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x000260CC File Offset: 0x000242CC
		[MethodImpl(256)]
		internal static int silk_lin2log(int inLin)
		{
			int num;
			int num2;
			Inlines.silk_CLZ_FRAC(inLin, out num, out num2);
			return Inlines.silk_LSHIFT(31 - num, 7) + Inlines.silk_SMLAWB(num2, Inlines.silk_MUL(num2, 128 - num2), 179);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00026108 File Offset: 0x00024308
		[MethodImpl(256)]
		internal static int silk_log2lin(int inLog_Q7)
		{
			if (inLog_Q7 < 0)
			{
				return 0;
			}
			if (inLog_Q7 >= 3967)
			{
				return int.MaxValue;
			}
			int num = Inlines.silk_LSHIFT(1, Inlines.silk_RSHIFT(inLog_Q7, 7));
			int num2 = inLog_Q7 & 127;
			if (inLog_Q7 < 2048)
			{
				num = Inlines.silk_ADD_RSHIFT32(num, Inlines.silk_MUL(num, Inlines.silk_SMLAWB(num2, Inlines.silk_SMULBB(num2, 128 - num2), -174)), 7);
			}
			else
			{
				num = Inlines.silk_MLA(num, Inlines.silk_RSHIFT(num, 7), Inlines.silk_SMLAWB(num2, Inlines.silk_SMULBB(num2, 128 - num2), -174));
			}
			return num;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00026194 File Offset: 0x00024394
		[MethodImpl(256)]
		internal static void silk_interpolate(short[] xi, short[] x0, short[] x1, int ifact_Q2, int d)
		{
			for (int i = 0; i < d; i++)
			{
				xi[i] = (short)Inlines.silk_ADD_RSHIFT((int)x0[i], Inlines.silk_SMULBB((int)(x1[i] - x0[i]), ifact_Q2), 2);
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x000261C8 File Offset: 0x000243C8
		[MethodImpl(256)]
		internal static int silk_inner_prod_aligned_scale(short[] inVec1, short[] inVec2, int scale, int len)
		{
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				num = Inlines.silk_ADD_RSHIFT32(num, Inlines.silk_SMULBB((int)inVec1[i], (int)inVec2[i]), scale);
			}
			return num;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000261F8 File Offset: 0x000243F8
		[MethodImpl(256)]
		internal unsafe static void silk_scale_copy_vector16(Span<short> data_out, int data_out_ptr, Span<short> data_in, int data_in_ptr, int gain_Q16, int dataSize)
		{
			for (int i = 0; i < dataSize; i++)
			{
				*data_out[data_out_ptr + i] = (short)Inlines.silk_SMULWB(gain_Q16, (int)(*data_in[data_in_ptr + i]));
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00026230 File Offset: 0x00024430
		[MethodImpl(256)]
		internal static void silk_scale_vector32_Q26_lshift_18(int[] data1, int data1_ptr, int gain_Q26, int dataSize)
		{
			for (int i = data1_ptr; i < data1_ptr + dataSize; i++)
			{
				data1[i] = (int)Inlines.silk_RSHIFT64(Inlines.silk_SMULL(data1[i], gain_Q26), 8);
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00026260 File Offset: 0x00024460
		[MethodImpl(256)]
		internal static int silk_inner_prod(short[] inVec1, int inVec1_ptr, short[] inVec2, int inVec2_ptr, int len)
		{
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				num = Inlines.MAC16_16(num, inVec1[inVec1_ptr + i], inVec2[inVec2_ptr + i]);
			}
			return num;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00026290 File Offset: 0x00024490
		[MethodImpl(256)]
		internal static int silk_inner_prod_self(short[] inVec, int inVec_ptr, int len)
		{
			int num = 0;
			for (int i = inVec_ptr; i < inVec_ptr + len; i++)
			{
				num = Inlines.MAC16_16(num, inVec[i], inVec[i]);
			}
			return num;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000262BC File Offset: 0x000244BC
		[MethodImpl(256)]
		internal static long silk_inner_prod16_aligned_64(short[] inVec1, int inVec1_ptr, short[] inVec2, int inVec2_ptr, int len)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num = Inlines.silk_SMLALBB(num, inVec1[inVec1_ptr + i], inVec2[inVec2_ptr + i]);
			}
			return num;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000262EB File Offset: 0x000244EB
		[MethodImpl(256)]
		internal static uint EC_MINI(uint a, uint b)
		{
			return a + (b - a & ((b < a) ? uint.MaxValue : 0U));
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000262FC File Offset: 0x000244FC
		[MethodImpl(256)]
		internal static int EC_CLZ(uint x)
		{
			if (x == 0U)
			{
				return 0;
			}
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			uint num = x - (x >> 1 & 1431655765U);
			num = (num >> 2 & 858993459U) + (num & 858993459U);
			num = ((num >> 4) + num & 252645135U);
			num += num >> 8;
			num += num >> 16;
			num &= 63U;
			return (int)(1U - num);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00026374 File Offset: 0x00024574
		[MethodImpl(256)]
		internal static int EC_ILOG(uint x)
		{
			if (x == 0U)
			{
				return 1;
			}
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			uint num = x - (x >> 1 & 1431655765U);
			num = (num >> 2 & 858993459U) + (num & 858993459U);
			num = ((num >> 4) + num & 252645135U);
			num += num >> 8;
			num += num >> 16;
			return (int)(num & 63U);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000263E7 File Offset: 0x000245E7
		[MethodImpl(256)]
		internal static int abs(int a)
		{
			if (a < 0)
			{
				return 0 - a;
			}
			return a;
		}

		// Token: 0x0400040E RID: 1038
		private const MethodImplOptions INLINE_ATTR = 256;

		// Token: 0x0400040F RID: 1039
		private static readonly short[] sqrt_C = new short[]
		{
			23175,
			11561,
			-3011,
			1699,
			-664
		};

		// Token: 0x04000410 RID: 1040
		private const short log2_C0 = -6793;
	}
}
