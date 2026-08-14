using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x0200016A RID: 362
	public sealed class IndexedColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x0600083A RID: 2106 RVA: 0x000264F0 File Offset: 0x000246F0
		internal static ColorSpaceDetails Stencil(ColorSpaceDetails colorSpaceDetails, double[] decode)
		{
			bool flag = decode.Length >= 2 && decode[0] == 1.0 && decode[1] == 0.0;
			byte hiVal = 1;
			byte[] colorTable;
			if (!flag)
			{
				(colorTable = new byte[2])[1] = byte.MaxValue;
			}
			else
			{
				(colorTable = new byte[2])[0] = byte.MaxValue;
			}
			return new IndexedColorSpaceDetails(colorSpaceDetails, hiVal, colorTable);
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x0002654B File Offset: 0x0002474B
		public override int NumberOfColorComponents
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x0002654E File Offset: 0x0002474E
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.BaseColorSpace.BaseNumberOfColorComponents;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0002655B File Offset: 0x0002475B
		public ColorSpaceDetails BaseColorSpace { get; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x00026563 File Offset: 0x00024763
		public byte HiVal { get; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0002656B File Offset: 0x0002476B
		public IReadOnlyList<byte> ColorTable { get; }

		// Token: 0x06000840 RID: 2112 RVA: 0x00026574 File Offset: 0x00024774
		public IndexedColorSpaceDetails(ColorSpaceDetails baseColorSpaceDetails, byte hiVal, IReadOnlyList<byte> colorTable) : base(ColorSpace.Indexed)
		{
			if (baseColorSpaceDetails == null)
			{
				throw new ArgumentNullException("baseColorSpaceDetails");
			}
			this.BaseColorSpace = baseColorSpaceDetails;
			this.HiVal = hiVal;
			this.ColorTable = colorTable;
			base.BaseType = baseColorSpaceDetails.Type;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000265C4 File Offset: 0x000247C4
		internal override double[] Process(params double[] values)
		{
			byte[] array = this.UnwrapIndexedColorSpaceBytes(new byte[]
			{
				(byte)values[0]
			});
			return this.BaseColorSpace.Process(Enumerable.ToArray<double>(Enumerable.Select<byte, double>(array, (byte b) => (double)b / 255.0)));
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0002661C File Offset: 0x0002481C
		public override IColor GetColor(params double[] values)
		{
			if (values == null || values.Length != this.NumberOfColorComponents)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(45, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid number of imputs, expecting ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.NumberOfColorComponents);
				defaultInterpolatedStringHandler.AppendLiteral(" but got ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(values.Length);
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "values");
			}
			return this.cache.GetOrAdd(values[0], delegate(double v)
			{
				byte[] array = this.UnwrapIndexedColorSpaceBytes(new byte[]
				{
					(byte)v
				});
				return this.BaseColorSpace.GetColor(Enumerable.ToArray<double>(Enumerable.Select<byte, double>(array, (byte b) => (double)b / 255.0)));
			});
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0002669C File Offset: 0x0002489C
		internal byte[] UnwrapIndexedColorSpaceBytes(IReadOnlyList<byte> input)
		{
			int num = 1;
			Func<byte, IEnumerable<byte>> func = null;
			switch (base.BaseType)
			{
			case ColorSpace.DeviceGray:
			case ColorSpace.CalGray:
			case ColorSpace.Separation:
				func = ((byte x) => new byte[]
				{
					this.ColorTable[(int)x]
				});
				num = 1;
				break;
			case ColorSpace.DeviceRGB:
			case ColorSpace.CalRGB:
			case ColorSpace.Lab:
				func = delegate(byte x)
				{
					byte[] array2 = new byte[3];
					for (int i = 0; i < 3; i++)
					{
						array2[i] = this.ColorTable[(int)(x * 3) + i];
					}
					return array2;
				};
				num = 3;
				break;
			case ColorSpace.DeviceCMYK:
				func = delegate(byte x)
				{
					byte[] array2 = new byte[4];
					for (int i = 0; i < 4; i++)
					{
						array2[i] = this.ColorTable[(int)(x * 4) + i];
					}
					return array2;
				};
				num = 4;
				break;
			case ColorSpace.ICCBased:
			case ColorSpace.DeviceN:
				func = delegate(byte x)
				{
					byte[] array2 = new byte[this.BaseColorSpace.NumberOfColorComponents];
					for (int i = 0; i < this.BaseColorSpace.NumberOfColorComponents; i++)
					{
						array2[i] = this.ColorTable[(int)x * this.BaseColorSpace.NumberOfColorComponents + i];
					}
					return array2;
				};
				num = this.BaseColorSpace.NumberOfColorComponents;
				break;
			}
			if (func != null)
			{
				byte[] array = new byte[input.Count * num];
				int num2 = 0;
				foreach (byte b in input)
				{
					foreach (byte b2 in func.Invoke(b))
					{
						array[num2++] = b2;
					}
				}
				return array;
			}
			return Enumerable.ToArray<byte>(input);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x000267D4 File Offset: 0x000249D4
		public override IColor GetInitializeColor()
		{
			return this.GetColor(new double[1]);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x000267E4 File Offset: 0x000249E4
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded)
		{
			byte[] decoded2 = this.UnwrapIndexedColorSpaceBytes(decoded);
			return this.BaseColorSpace.Transform(decoded2);
		}

		// Token: 0x040003F1 RID: 1009
		private readonly ConcurrentDictionary<double, IColor> cache = new ConcurrentDictionary<double, IColor>();
	}
}
