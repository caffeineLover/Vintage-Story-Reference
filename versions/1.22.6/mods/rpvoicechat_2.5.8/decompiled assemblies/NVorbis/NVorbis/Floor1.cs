using System;
using System.Collections.Generic;
using System.IO;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x02000007 RID: 7
	internal class Floor1 : IFloor
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00003104 File Offset: 0x00001304
		public void Init(IPacket packet, int channels, int block0Size, int block1Size, ICodebook[] codebooks)
		{
			int num = -1;
			this._partitionClass = new int[(int)packet.ReadBits(5)];
			for (int i = 0; i < this._partitionClass.Length; i++)
			{
				this._partitionClass[i] = (int)packet.ReadBits(4);
				if (this._partitionClass[i] > num)
				{
					num = this._partitionClass[i];
				}
			}
			this._classDimensions = new int[++num];
			this._classSubclasses = new int[num];
			this._classMasterbooks = new ICodebook[num];
			this._classMasterBookIndex = new int[num];
			this._subclassBooks = new ICodebook[num][];
			this._subclassBookIndex = new int[num][];
			for (int j = 0; j < num; j++)
			{
				this._classDimensions[j] = (int)packet.ReadBits(3) + 1;
				this._classSubclasses[j] = (int)packet.ReadBits(2);
				if (this._classSubclasses[j] > 0)
				{
					this._classMasterBookIndex[j] = (int)packet.ReadBits(8);
					this._classMasterbooks[j] = codebooks[this._classMasterBookIndex[j]];
				}
				this._subclassBooks[j] = new ICodebook[1 << this._classSubclasses[j]];
				this._subclassBookIndex[j] = new int[this._subclassBooks[j].Length];
				for (int k = 0; k < this._subclassBooks[j].Length; k++)
				{
					int num2 = (int)packet.ReadBits(8) - 1;
					if (num2 >= 0)
					{
						this._subclassBooks[j][k] = codebooks[num2];
					}
					this._subclassBookIndex[j][k] = num2;
				}
			}
			this._multiplier = (int)packet.ReadBits(2);
			this._range = Floor1._rangeLookup[this._multiplier];
			this._yBits = Floor1._yBitsLookup[this._multiplier];
			this._multiplier++;
			int num3 = (int)packet.ReadBits(4);
			List<int> list = new List<int>();
			list.Add(0);
			list.Add(1 << num3);
			for (int l = 0; l < this._partitionClass.Length; l++)
			{
				int num4 = this._partitionClass[l];
				for (int m = 0; m < this._classDimensions[num4]; m++)
				{
					list.Add((int)packet.ReadBits(num3));
				}
			}
			this._xList = list.ToArray();
			this._lNeigh = new int[list.Count];
			this._hNeigh = new int[list.Count];
			this._sortIdx = new int[list.Count];
			this._sortIdx[0] = 0;
			this._sortIdx[1] = 1;
			for (int n = 2; n < this._lNeigh.Length; n++)
			{
				this._lNeigh[n] = 0;
				this._hNeigh[n] = 1;
				this._sortIdx[n] = n;
				for (int num5 = 2; num5 < n; num5++)
				{
					int num6 = this._xList[num5];
					if (num6 < this._xList[n])
					{
						if (num6 > this._xList[this._lNeigh[n]])
						{
							this._lNeigh[n] = num5;
						}
					}
					else if (num6 < this._xList[this._hNeigh[n]])
					{
						this._hNeigh[n] = num5;
					}
				}
			}
			for (int num7 = 0; num7 < this._sortIdx.Length - 1; num7++)
			{
				for (int num8 = num7 + 1; num8 < this._sortIdx.Length; num8++)
				{
					if (this._xList[num7] == this._xList[num8])
					{
						throw new InvalidDataException();
					}
					if (this._xList[this._sortIdx[num7]] > this._xList[this._sortIdx[num8]])
					{
						int num9 = this._sortIdx[num7];
						this._sortIdx[num7] = this._sortIdx[num8];
						this._sortIdx[num8] = num9;
					}
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000034D4 File Offset: 0x000016D4
		public IFloorData Unpack(IPacket packet, int blockSize, int channel)
		{
			Floor1.Data data = new Floor1.Data();
			if (packet.ReadBit())
			{
				int num = 2;
				data.Posts[0] = (int)packet.ReadBits(this._yBits);
				data.Posts[1] = (int)packet.ReadBits(this._yBits);
				for (int i = 0; i < this._partitionClass.Length; i++)
				{
					int num2 = this._partitionClass[i];
					int num3 = this._classDimensions[num2];
					int num4 = this._classSubclasses[num2];
					int num5 = (1 << num4) - 1;
					uint num6 = 0U;
					if (num4 > 0 && (num6 = (uint)this._classMasterbooks[num2].DecodeScalar(packet)) == 4294967295U)
					{
						num = 0;
						break;
					}
					for (int j = 0; j < num3; j++)
					{
						ICodebook codebook = this._subclassBooks[num2][(int)(checked((IntPtr)(unchecked((ulong)num6 & (ulong)((long)num5)))))];
						num6 >>= num4;
						if (codebook != null && (data.Posts[num] = codebook.DecodeScalar(packet)) == -1)
						{
							num = 0;
							i = this._partitionClass.Length;
							break;
						}
						num++;
					}
				}
				data.PostCount = num;
			}
			return data;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000035E4 File Offset: 0x000017E4
		public void Apply(IFloorData floorData, int blockSize, float[] residue)
		{
			Floor1.Data data = floorData as Floor1.Data;
			if (data == null)
			{
				throw new ArgumentException("Incorrect packet data!", "packetData");
			}
			int num = blockSize / 2;
			if (data.PostCount > 0)
			{
				bool[] array = this.UnwrapPosts(data);
				int num2 = 0;
				int num3 = data.Posts[0] * this._multiplier;
				for (int i = 1; i < data.PostCount; i++)
				{
					int num4 = this._sortIdx[i];
					if (array[num4])
					{
						int num5 = this._xList[num4];
						int num6 = data.Posts[num4] * this._multiplier;
						if (num2 < num)
						{
							this.RenderLineMulti(num2, num3, Math.Min(num5, num), num6, residue);
						}
						num2 = num5;
						num3 = num6;
					}
					if (num2 >= num)
					{
						break;
					}
				}
				if (num2 < num)
				{
					this.RenderLineMulti(num2, num3, num, num3, residue);
					return;
				}
			}
			else
			{
				Array.Clear(residue, 0, num);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000036B8 File Offset: 0x000018B8
		private bool[] UnwrapPosts(Floor1.Data data)
		{
			bool[] array = new bool[64];
			array[0] = true;
			array[1] = true;
			int[] array2 = new int[64];
			array2[0] = data.Posts[0];
			array2[1] = data.Posts[1];
			for (int i = 2; i < data.PostCount; i++)
			{
				int num = this._lNeigh[i];
				int num2 = this._hNeigh[i];
				int num3 = this.RenderPoint(this._xList[num], array2[num], this._xList[num2], array2[num2], this._xList[i]);
				int num4 = data.Posts[i];
				int num5 = this._range - num3;
				int num6 = num3;
				int num7;
				if (num5 < num6)
				{
					num7 = num5 * 2;
				}
				else
				{
					num7 = num6 * 2;
				}
				if (num4 != 0)
				{
					array[num] = true;
					array[num2] = true;
					array[i] = true;
					if (num4 >= num7)
					{
						if (num5 > num6)
						{
							array2[i] = num4 - num6 + num3;
						}
						else
						{
							array2[i] = num3 - num4 + num5 - 1;
						}
					}
					else if (num4 % 2 == 1)
					{
						array2[i] = num3 - (num4 + 1) / 2;
					}
					else
					{
						array2[i] = num3 + num4 / 2;
					}
				}
				else
				{
					array[i] = false;
					array2[i] = num3;
				}
			}
			for (int j = 0; j < data.PostCount; j++)
			{
				data.Posts[j] = array2[j];
			}
			return array;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000037F8 File Offset: 0x000019F8
		private int RenderPoint(int x0, int y0, int x1, int y1, int X)
		{
			int num = y1 - y0;
			int num2 = x1 - x0;
			int num3 = Math.Abs(num) * (X - x0) / num2;
			if (num < 0)
			{
				return y0 - num3;
			}
			return y0 + num3;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003828 File Offset: 0x00001A28
		private void RenderLineMulti(int x0, int y0, int x1, int y1, float[] v)
		{
			int num = y1 - y0;
			int num2 = x1 - x0;
			int num3 = Math.Abs(num);
			int num4 = 1 - (num >> 31 & 1) * 2;
			int num5 = num / num2;
			int num6 = x0;
			int num7 = y0;
			int num8 = -num2;
			v[x0] *= Floor1.inverse_dB_table[y0];
			num3 -= Math.Abs(num5) * num2;
			while (++num6 < x1)
			{
				num7 += num5;
				num8 += num3;
				if (num8 >= 0)
				{
					num8 -= num2;
					num7 += num4;
				}
				v[num6] *= Floor1.inverse_dB_table[num7];
			}
		}

		// Token: 0x0400001B RID: 27
		private int[] _partitionClass;

		// Token: 0x0400001C RID: 28
		private int[] _classDimensions;

		// Token: 0x0400001D RID: 29
		private int[] _classSubclasses;

		// Token: 0x0400001E RID: 30
		private int[] _xList;

		// Token: 0x0400001F RID: 31
		private int[] _classMasterBookIndex;

		// Token: 0x04000020 RID: 32
		private int[] _hNeigh;

		// Token: 0x04000021 RID: 33
		private int[] _lNeigh;

		// Token: 0x04000022 RID: 34
		private int[] _sortIdx;

		// Token: 0x04000023 RID: 35
		private int _multiplier;

		// Token: 0x04000024 RID: 36
		private int _range;

		// Token: 0x04000025 RID: 37
		private int _yBits;

		// Token: 0x04000026 RID: 38
		private ICodebook[] _classMasterbooks;

		// Token: 0x04000027 RID: 39
		private ICodebook[][] _subclassBooks;

		// Token: 0x04000028 RID: 40
		private int[][] _subclassBookIndex;

		// Token: 0x04000029 RID: 41
		private static readonly int[] _rangeLookup = new int[]
		{
			256,
			128,
			86,
			64
		};

		// Token: 0x0400002A RID: 42
		private static readonly int[] _yBitsLookup = new int[]
		{
			8,
			7,
			7,
			6
		};

		// Token: 0x0400002B RID: 43
		private static readonly float[] inverse_dB_table = new float[]
		{
			1.0649863E-07f,
			1.1341951E-07f,
			1.2079015E-07f,
			1.2863978E-07f,
			1.369995E-07f,
			1.459025E-07f,
			1.5538409E-07f,
			1.6548181E-07f,
			1.7623574E-07f,
			1.8768856E-07f,
			1.998856E-07f,
			2.128753E-07f,
			2.2670913E-07f,
			2.4144197E-07f,
			2.5713223E-07f,
			2.7384212E-07f,
			2.9163792E-07f,
			3.1059022E-07f,
			3.307741E-07f,
			3.5226967E-07f,
			3.7516213E-07f,
			3.995423E-07f,
			4.255068E-07f,
			4.5315863E-07f,
			4.8260745E-07f,
			5.1397E-07f,
			5.4737063E-07f,
			5.829419E-07f,
			6.208247E-07f,
			6.611694E-07f,
			7.041359E-07f,
			7.4989464E-07f,
			7.98627E-07f,
			8.505263E-07f,
			9.057983E-07f,
			9.646621E-07f,
			1.0273513E-06f,
			1.0941144E-06f,
			1.1652161E-06f,
			1.2409384E-06f,
			1.3215816E-06f,
			1.4074654E-06f,
			1.4989305E-06f,
			1.5963394E-06f,
			1.7000785E-06f,
			1.8105592E-06f,
			1.9282195E-06f,
			2.053526E-06f,
			2.1869757E-06f,
			2.3290977E-06f,
			2.4804558E-06f,
			2.6416496E-06f,
			2.813319E-06f,
			2.9961443E-06f,
			3.1908505E-06f,
			3.39821E-06f,
			3.619045E-06f,
			3.8542307E-06f,
			4.1047006E-06f,
			4.371447E-06f,
			4.6555283E-06f,
			4.958071E-06f,
			5.280274E-06f,
			5.623416E-06f,
			5.988857E-06f,
			6.3780467E-06f,
			6.7925284E-06f,
			7.2339453E-06f,
			7.704048E-06f,
			8.2047E-06f,
			8.737888E-06f,
			9.305725E-06f,
			9.910464E-06f,
			1.0554501E-05f,
			1.1240392E-05f,
			1.1970856E-05f,
			1.2748789E-05f,
			1.3577278E-05f,
			1.4459606E-05f,
			1.5399271E-05f,
			1.6400005E-05f,
			1.7465769E-05f,
			1.8600793E-05f,
			1.9809577E-05f,
			2.1096914E-05f,
			2.2467912E-05f,
			2.3928002E-05f,
			2.5482977E-05f,
			2.7139005E-05f,
			2.890265E-05f,
			3.078091E-05f,
			3.2781227E-05f,
			3.4911533E-05f,
			3.718028E-05f,
			3.9596467E-05f,
			4.2169668E-05f,
			4.491009E-05f,
			4.7828602E-05f,
			5.0936775E-05f,
			5.424693E-05f,
			5.7772202E-05f,
			6.152657E-05f,
			6.552491E-05f,
			6.9783084E-05f,
			7.4317984E-05f,
			7.914758E-05f,
			8.429104E-05f,
			8.976875E-05f,
			9.560242E-05f,
			0.00010181521f,
			0.00010843174f,
			0.00011547824f,
			0.00012298267f,
			0.00013097477f,
			0.00013948625f,
			0.00014855085f,
			0.00015820454f,
			0.00016848555f,
			0.00017943469f,
			0.00019109536f,
			0.00020351382f,
			0.0002167393f,
			0.00023082423f,
			0.00024582449f,
			0.00026179955f,
			0.00027881275f,
			0.00029693157f,
			0.00031622787f,
			0.00033677815f,
			0.00035866388f,
			0.00038197188f,
			0.00040679457f,
			0.00043323037f,
			0.0004613841f,
			0.0004913675f,
			0.00052329927f,
			0.0005573062f,
			0.0005935231f,
			0.0006320936f,
			0.0006731706f,
			0.000716917f,
			0.0007635063f,
			0.00081312325f,
			0.00086596457f,
			0.00092223985f,
			0.0009821722f,
			0.0010459992f,
			0.0011139743f,
			0.0011863665f,
			0.0012634633f,
			0.0013455702f,
			0.0014330129f,
			0.0015261382f,
			0.0016253153f,
			0.0017309374f,
			0.0018434235f,
			0.0019632196f,
			0.0020908006f,
			0.0022266726f,
			0.0023713743f,
			0.0025254795f,
			0.0026895993f,
			0.0028643848f,
			0.0030505287f,
			0.003248769f,
			0.0034598925f,
			0.0036847359f,
			0.0039241905f,
			0.0041792067f,
			0.004450795f,
			0.004740033f,
			0.005048067f,
			0.0053761187f,
			0.005725489f,
			0.0060975635f,
			0.0064938175f,
			0.0069158226f,
			0.0073652514f,
			0.007843887f,
			0.008353627f,
			0.008896492f,
			0.009474637f,
			0.010090352f,
			0.01074608f,
			0.011444421f,
			0.012188144f,
			0.012980198f,
			0.013823725f,
			0.014722068f,
			0.015678791f,
			0.016697686f,
			0.017782796f,
			0.018938422f,
			0.020169148f,
			0.021479854f,
			0.022875736f,
			0.02436233f,
			0.025945531f,
			0.027631618f,
			0.029427277f,
			0.031339627f,
			0.03337625f,
			0.035545226f,
			0.037855156f,
			0.0403152f,
			0.042935107f,
			0.045725275f,
			0.048696756f,
			0.05186135f,
			0.05523159f,
			0.05882085f,
			0.062643364f,
			0.06671428f,
			0.07104975f,
			0.075666964f,
			0.08058423f,
			0.08582105f,
			0.09139818f,
			0.097337745f,
			0.1036633f,
			0.11039993f,
			0.11757434f,
			0.12521498f,
			0.13335215f,
			0.14201812f,
			0.15124726f,
			0.16107617f,
			0.1715438f,
			0.18269168f,
			0.19456401f,
			0.20720787f,
			0.22067343f,
			0.23501402f,
			0.25028655f,
			0.26655158f,
			0.28387362f,
			0.3023213f,
			0.32196787f,
			0.34289113f,
			0.36517414f,
			0.3889052f,
			0.41417846f,
			0.44109413f,
			0.4697589f,
			0.50028646f,
			0.53279793f,
			0.5674221f,
			0.6042964f,
			0.64356697f,
			0.6853896f,
			0.72993004f,
			0.777365f,
			0.8278826f,
			0.88168305f,
			0.9389798f,
			1f
		};

		// Token: 0x02000040 RID: 64
		private class Data : IFloorData
		{
			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x06000258 RID: 600 RVA: 0x0000875B File Offset: 0x0000695B
			public bool ExecuteChannel
			{
				get
				{
					return (this.ForceEnergy || this.PostCount > 0) && !this.ForceNoEnergy;
				}
			}

			// Token: 0x170000E5 RID: 229
			// (get) Token: 0x06000259 RID: 601 RVA: 0x00008779 File Offset: 0x00006979
			// (set) Token: 0x0600025A RID: 602 RVA: 0x00008781 File Offset: 0x00006981
			public bool ForceEnergy { get; set; }

			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x0600025B RID: 603 RVA: 0x0000878A File Offset: 0x0000698A
			// (set) Token: 0x0600025C RID: 604 RVA: 0x00008792 File Offset: 0x00006992
			public bool ForceNoEnergy { get; set; }

			// Token: 0x040000F1 RID: 241
			internal int[] Posts = new int[64];

			// Token: 0x040000F2 RID: 242
			internal int PostCount;
		}
	}
}
