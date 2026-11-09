using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Elements
{
	// Token: 0x020000C8 RID: 200
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class DynamicImage : Element, IStateful, IDisposable
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0000EEFF File Offset: 0x0000D0FF
		// (set) Token: 0x0600060F RID: 1551 RVA: 0x0000EF07 File Offset: 0x0000D107
		internal int? TargetDpi { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0000EF10 File Offset: 0x0000D110
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x0000EF18 File Offset: 0x0000D118
		internal ImageCompressionQuality? CompressionQuality { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0000EF21 File Offset: 0x0000D121
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x0000EF29 File Offset: 0x0000D129
		internal bool UseOriginalImage { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0000EF32 File Offset: 0x0000D132
		// (set) Token: 0x06000615 RID: 1557 RVA: 0x0000EF3A File Offset: 0x0000D13A
		public GenerateDynamicImageDelegate Source { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0000EF43 File Offset: 0x0000D143
		[TupleElementNames(new string[]
		{
			"Size",
			"Image"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			2
		})]
		private List<ValueTuple<Size, SkImage>> Cache { [return: TupleElementNames(new string[]
		{
			"Size",
			"Image"
		})] [return: Nullable(new byte[]
		{
			1,
			0,
			2
		})] get; } = new List<ValueTuple<Size, SkImage>>(1);

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0000EF4B File Offset: 0x0000D14B
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x0000EF53 File Offset: 0x0000D153
		private float GenerationTime { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x0000EF5C File Offset: 0x0000D15C
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x0000EF64 File Offset: 0x0000D164
		private int DrawnImageSize { get; set; }

		// Token: 0x0600061B RID: 1563 RVA: 0x0000EF70 File Offset: 0x0000D170
		~DynamicImage()
		{
			this.WarnThatFinalizerIsReached<DynamicImage>();
			this.Dispose();
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
		public void Dispose()
		{
			foreach (ValueTuple<Size, SkImage> valueTuple in this.Cache)
			{
				SkImage item = valueTuple.Item2;
				if (item != null)
				{
					item.Dispose();
				}
			}
			this.Cache.Clear();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0000F010 File Offset: 0x0000D210
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			if (availableSpace.IsNegative())
			{
				return SpacePlan.Wrap("The available space is negative.");
			}
			return SpacePlan.FullRender(availableSpace);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0000F03C File Offset: 0x0000D23C
		internal override void Draw(Size availableSpace)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			SkImage skImage = Enumerable.FirstOrDefault<ValueTuple<Size, SkImage>>(this.Cache, ([TupleElementNames(new string[]
			{
				"Size",
				"Image"
			})] ValueTuple<Size, SkImage> x) => Size.Equal(x.Item1, availableSpace)).Item2;
			if (skImage == null)
			{
				skImage = this.GetImage(availableSpace);
				this.Cache.Add(new ValueTuple<Size, SkImage>(availableSpace, skImage));
			}
			if (skImage != null)
			{
				base.Canvas.DrawImage(skImage, availableSpace);
			}
			this.GenerationTime += (float)stopwatch.Elapsed.TotalMilliseconds;
			this.DrawnImageSize += ((skImage != null) ? skImage.EncodedDataSize : 0);
			this.IsRendered = true;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0000F0F4 File Offset: 0x0000D2F4
		private SkImage GetImage(Size availableSpace)
		{
			int valueOrDefault = this.TargetDpi.GetValueOrDefault(72);
			GenerateDynamicImageDelegatePayload payload = new GenerateDynamicImageDelegatePayload
			{
				AvailableSpace = availableSpace,
				ImageSize = DynamicImage.GetTargetResolution(availableSpace, valueOrDefault),
				Dpi = valueOrDefault
			};
			GenerateDynamicImageDelegate source = this.Source;
			byte[] array = (source != null) ? source(payload) : null;
			if (array == null)
			{
				return null;
			}
			SkImage result;
			using (SkData skData = SkData.FromBinary(array))
			{
				SkImage skImage = SkImage.FromData(skData);
				if (this.UseOriginalImage)
				{
					result = skImage;
				}
				else
				{
					SkImage skImage2 = skImage.CompressImage(this.CompressionQuality.Value);
					if (skImage.EncodedDataSize > skImage2.EncodedDataSize)
					{
						skImage.Dispose();
						result = skImage2;
					}
					else
					{
						skImage2.Dispose();
						result = skImage;
					}
				}
			}
			return result;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0000F1C8 File Offset: 0x0000D3C8
		private static ImageSize GetTargetResolution(Size availableSize, int targetDpi)
		{
			float num = (float)targetDpi / 72f;
			return new ImageSize((int)(availableSize.Width * num), (int)(availableSize.Height * num));
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x0000F1F5 File Offset: 0x0000D3F5
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x0000F1FD File Offset: 0x0000D3FD
		private bool IsRendered { get; set; }

		// Token: 0x06000623 RID: 1571 RVA: 0x0000F206 File Offset: 0x0000D406
		public void ResetState(bool hardReset = false)
		{
			this.IsRendered = false;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0000F20F File Offset: 0x0000D40F
		[NullableContext(1)]
		public object GetState()
		{
			return this.IsRendered;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0000F21C File Offset: 0x0000D41C
		[NullableContext(1)]
		public void SetState(object state)
		{
			this.IsRendered = (bool)state;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0000F22C File Offset: 0x0000D42C
		internal override string GetCompanionHint()
		{
			int num = Math.Max(1, this.DrawnImageSize / 1024);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(19, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral("KB, generated in ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.GenerationTime, "0.00");
			defaultInterpolatedStringHandler.AppendLiteral("ms");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
