using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x0200002C RID: 44
	[Serializable]
	public struct Vector4h : ISerializable, IEquatable<Vector4h>, IFormattable
	{
		// Token: 0x06000D57 RID: 3415 RVA: 0x00030AAB File Offset: 0x0002ECAB
		public Vector4h(Half value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
			this.W = value;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00030AC9 File Offset: 0x0002ECC9
		public Vector4h(float value)
		{
			this.X = new Half(value);
			this.Y = new Half(value);
			this.Z = new Half(value);
			this.W = new Half(value);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00030AFB File Offset: 0x0002ECFB
		public Vector4h(Half x, Half y, Half z, Half w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00030B1A File Offset: 0x0002ED1A
		public Vector4h(float x, float y, float z, float w)
		{
			this.X = new Half(x);
			this.Y = new Half(y);
			this.Z = new Half(z);
			this.W = new Half(w);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00030B4D File Offset: 0x0002ED4D
		public Vector4h(float x, float y, float z, float w, bool throwOnError)
		{
			this.X = new Half(x, throwOnError);
			this.Y = new Half(y, throwOnError);
			this.Z = new Half(z, throwOnError);
			this.W = new Half(w, throwOnError);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00030B88 File Offset: 0x0002ED88
		public Vector4h(Vector4 v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
			this.Z = new Half(v.Z);
			this.W = new Half(v.W);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00030BDC File Offset: 0x0002EDDC
		public Vector4h(Vector4 v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
			this.Z = new Half(v.Z, throwOnError);
			this.W = new Half(v.W, throwOnError);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00030C34 File Offset: 0x0002EE34
		public Vector4h(in Vector4 v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
			this.Z = new Half(v.Z);
			this.W = new Half(v.W);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00030C88 File Offset: 0x0002EE88
		public Vector4h(in Vector4 v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
			this.Z = new Half(v.Z, throwOnError);
			this.W = new Half(v.W, throwOnError);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00030CE0 File Offset: 0x0002EEE0
		public Vector4h(Vector4d v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
			this.Z = new Half(v.Z);
			this.W = new Half(v.W);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00030D34 File Offset: 0x0002EF34
		public Vector4h(Vector4d v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
			this.Z = new Half(v.Z, throwOnError);
			this.W = new Half(v.W, throwOnError);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00030D8C File Offset: 0x0002EF8C
		public Vector4h(in Vector4d v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
			this.Z = new Half(v.Z);
			this.W = new Half(v.W);
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00030DE0 File Offset: 0x0002EFE0
		public Vector4h(in Vector4d v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
			this.Z = new Half(v.Z, throwOnError);
			this.W = new Half(v.W, throwOnError);
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00030E35 File Offset: 0x0002F035
		// (set) Token: 0x06000D65 RID: 3429 RVA: 0x00030E42 File Offset: 0x0002F042
		[XmlIgnore]
		public unsafe Vector2h Xy
		{
			get
			{
				return *Unsafe.As<Vector4h, Vector2h>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00030E5C File Offset: 0x0002F05C
		// (set) Token: 0x06000D67 RID: 3431 RVA: 0x00030E6F File Offset: 0x0002F06F
		[XmlIgnore]
		public Vector2h Xz
		{
			readonly get
			{
				return new Vector2h(this.X, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x00030E89 File Offset: 0x0002F089
		// (set) Token: 0x06000D69 RID: 3433 RVA: 0x00030E9C File Offset: 0x0002F09C
		[XmlIgnore]
		public Vector2h Xw
		{
			readonly get
			{
				return new Vector2h(this.X, this.W);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00030EB6 File Offset: 0x0002F0B6
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x00030EC9 File Offset: 0x0002F0C9
		[XmlIgnore]
		public Vector2h Yx
		{
			readonly get
			{
				return new Vector2h(this.Y, this.X);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00030EE3 File Offset: 0x0002F0E3
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x00030EF6 File Offset: 0x0002F0F6
		[XmlIgnore]
		public Vector2h Yz
		{
			readonly get
			{
				return new Vector2h(this.Y, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00030F10 File Offset: 0x0002F110
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x00030F23 File Offset: 0x0002F123
		[XmlIgnore]
		public Vector2h Yw
		{
			readonly get
			{
				return new Vector2h(this.Y, this.W);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x00030F3D File Offset: 0x0002F13D
		// (set) Token: 0x06000D71 RID: 3441 RVA: 0x00030F50 File Offset: 0x0002F150
		[XmlIgnore]
		public Vector2h Zx
		{
			readonly get
			{
				return new Vector2h(this.Z, this.X);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x00030F6A File Offset: 0x0002F16A
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x00030F7D File Offset: 0x0002F17D
		[XmlIgnore]
		public Vector2h Zy
		{
			readonly get
			{
				return new Vector2h(this.Z, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x00030F97 File Offset: 0x0002F197
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x00030FAA File Offset: 0x0002F1AA
		[XmlIgnore]
		public Vector2h Zw
		{
			readonly get
			{
				return new Vector2h(this.Z, this.W);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x00030FC4 File Offset: 0x0002F1C4
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x00030FD7 File Offset: 0x0002F1D7
		[XmlIgnore]
		public Vector2h Wx
		{
			readonly get
			{
				return new Vector2h(this.W, this.X);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x00030FF1 File Offset: 0x0002F1F1
		// (set) Token: 0x06000D79 RID: 3449 RVA: 0x00031004 File Offset: 0x0002F204
		[XmlIgnore]
		public Vector2h Wy
		{
			readonly get
			{
				return new Vector2h(this.W, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x0003101E File Offset: 0x0002F21E
		// (set) Token: 0x06000D7B RID: 3451 RVA: 0x00031031 File Offset: 0x0002F231
		[XmlIgnore]
		public Vector2h Wz
		{
			readonly get
			{
				return new Vector2h(this.W, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0003104B File Offset: 0x0002F24B
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x00031058 File Offset: 0x0002F258
		[XmlIgnore]
		public unsafe Vector3h Xyz
		{
			get
			{
				return *Unsafe.As<Vector4h, Vector3h>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x0003107E File Offset: 0x0002F27E
		// (set) Token: 0x06000D7F RID: 3455 RVA: 0x00031097 File Offset: 0x0002F297
		[XmlIgnore]
		public Vector3h Xyw
		{
			readonly get
			{
				return new Vector3h(this.X, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x000310BD File Offset: 0x0002F2BD
		// (set) Token: 0x06000D81 RID: 3457 RVA: 0x000310D6 File Offset: 0x0002F2D6
		[XmlIgnore]
		public Vector3h Xzy
		{
			readonly get
			{
				return new Vector3h(this.X, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x000310FC File Offset: 0x0002F2FC
		// (set) Token: 0x06000D83 RID: 3459 RVA: 0x00031115 File Offset: 0x0002F315
		[XmlIgnore]
		public Vector3h Xzw
		{
			readonly get
			{
				return new Vector3h(this.X, this.Z, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x0003113B File Offset: 0x0002F33B
		// (set) Token: 0x06000D85 RID: 3461 RVA: 0x00031154 File Offset: 0x0002F354
		[XmlIgnore]
		public Vector3h Xwy
		{
			readonly get
			{
				return new Vector3h(this.X, this.W, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x0003117A File Offset: 0x0002F37A
		// (set) Token: 0x06000D87 RID: 3463 RVA: 0x00031193 File Offset: 0x0002F393
		[XmlIgnore]
		public Vector3h Xwz
		{
			readonly get
			{
				return new Vector3h(this.X, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x000311B9 File Offset: 0x0002F3B9
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x000311D2 File Offset: 0x0002F3D2
		[XmlIgnore]
		public Vector3h Yxz
		{
			readonly get
			{
				return new Vector3h(this.Y, this.X, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x000311F8 File Offset: 0x0002F3F8
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x00031211 File Offset: 0x0002F411
		[XmlIgnore]
		public Vector3h Yxw
		{
			readonly get
			{
				return new Vector3h(this.Y, this.X, this.W);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00031237 File Offset: 0x0002F437
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x00031250 File Offset: 0x0002F450
		[XmlIgnore]
		public Vector3h Yzx
		{
			readonly get
			{
				return new Vector3h(this.Y, this.Z, this.X);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00031276 File Offset: 0x0002F476
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x0003128F File Offset: 0x0002F48F
		[XmlIgnore]
		public Vector3h Yzw
		{
			readonly get
			{
				return new Vector3h(this.Y, this.Z, this.W);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x000312B5 File Offset: 0x0002F4B5
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x000312CE File Offset: 0x0002F4CE
		[XmlIgnore]
		public Vector3h Ywx
		{
			readonly get
			{
				return new Vector3h(this.Y, this.W, this.X);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x000312F4 File Offset: 0x0002F4F4
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x0003130D File Offset: 0x0002F50D
		[XmlIgnore]
		public Vector3h Ywz
		{
			readonly get
			{
				return new Vector3h(this.Y, this.W, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x00031333 File Offset: 0x0002F533
		// (set) Token: 0x06000D95 RID: 3477 RVA: 0x0003134C File Offset: 0x0002F54C
		[XmlIgnore]
		public Vector3h Zxy
		{
			readonly get
			{
				return new Vector3h(this.Z, this.X, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x00031372 File Offset: 0x0002F572
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x0003138B File Offset: 0x0002F58B
		[XmlIgnore]
		public Vector3h Zxw
		{
			readonly get
			{
				return new Vector3h(this.Z, this.X, this.W);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x000313B1 File Offset: 0x0002F5B1
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x000313CA File Offset: 0x0002F5CA
		[XmlIgnore]
		public Vector3h Zyx
		{
			readonly get
			{
				return new Vector3h(this.Z, this.Y, this.X);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x000313F0 File Offset: 0x0002F5F0
		// (set) Token: 0x06000D9B RID: 3483 RVA: 0x00031409 File Offset: 0x0002F609
		[XmlIgnore]
		public Vector3h Zyw
		{
			readonly get
			{
				return new Vector3h(this.Z, this.Y, this.W);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x0003142F File Offset: 0x0002F62F
		// (set) Token: 0x06000D9D RID: 3485 RVA: 0x00031448 File Offset: 0x0002F648
		[XmlIgnore]
		public Vector3h Zwx
		{
			readonly get
			{
				return new Vector3h(this.Z, this.W, this.X);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x0003146E File Offset: 0x0002F66E
		// (set) Token: 0x06000D9F RID: 3487 RVA: 0x00031487 File Offset: 0x0002F687
		[XmlIgnore]
		public Vector3h Zwy
		{
			readonly get
			{
				return new Vector3h(this.Z, this.W, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x000314AD File Offset: 0x0002F6AD
		// (set) Token: 0x06000DA1 RID: 3489 RVA: 0x000314C6 File Offset: 0x0002F6C6
		[XmlIgnore]
		public Vector3h Wxy
		{
			readonly get
			{
				return new Vector3h(this.W, this.X, this.Y);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x000314EC File Offset: 0x0002F6EC
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x00031505 File Offset: 0x0002F705
		[XmlIgnore]
		public Vector3h Wxz
		{
			readonly get
			{
				return new Vector3h(this.W, this.X, this.Z);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0003152B File Offset: 0x0002F72B
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x00031544 File Offset: 0x0002F744
		[XmlIgnore]
		public Vector3h Wyx
		{
			readonly get
			{
				return new Vector3h(this.W, this.Y, this.X);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x0003156A File Offset: 0x0002F76A
		// (set) Token: 0x06000DA7 RID: 3495 RVA: 0x00031583 File Offset: 0x0002F783
		[XmlIgnore]
		public Vector3h Wyz
		{
			readonly get
			{
				return new Vector3h(this.W, this.Y, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x000315A9 File Offset: 0x0002F7A9
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x000315C2 File Offset: 0x0002F7C2
		[XmlIgnore]
		public Vector3h Wzx
		{
			readonly get
			{
				return new Vector3h(this.W, this.Z, this.X);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x000315E8 File Offset: 0x0002F7E8
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x00031601 File Offset: 0x0002F801
		[XmlIgnore]
		public Vector3h Wzy
		{
			readonly get
			{
				return new Vector3h(this.W, this.Z, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x00031627 File Offset: 0x0002F827
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x00031646 File Offset: 0x0002F846
		[XmlIgnore]
		public Vector4h Xywz
		{
			readonly get
			{
				return new Vector4h(this.X, this.Y, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x00031678 File Offset: 0x0002F878
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x00031697 File Offset: 0x0002F897
		[XmlIgnore]
		public Vector4h Xzyw
		{
			readonly get
			{
				return new Vector4h(this.X, this.Z, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x000316C9 File Offset: 0x0002F8C9
		// (set) Token: 0x06000DB1 RID: 3505 RVA: 0x000316E8 File Offset: 0x0002F8E8
		[XmlIgnore]
		public Vector4h Xzwy
		{
			readonly get
			{
				return new Vector4h(this.X, this.Z, this.W, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.W = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0003171A File Offset: 0x0002F91A
		// (set) Token: 0x06000DB3 RID: 3507 RVA: 0x00031739 File Offset: 0x0002F939
		[XmlIgnore]
		public Vector4h Xwyz
		{
			readonly get
			{
				return new Vector4h(this.X, this.W, this.Y, this.Z);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Y = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x0003176B File Offset: 0x0002F96B
		// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x0003178A File Offset: 0x0002F98A
		[XmlIgnore]
		public Vector4h Xwzy
		{
			readonly get
			{
				return new Vector4h(this.X, this.W, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x000317BC File Offset: 0x0002F9BC
		// (set) Token: 0x06000DB7 RID: 3511 RVA: 0x000317DB File Offset: 0x0002F9DB
		[XmlIgnore]
		public Vector4h Yxzw
		{
			readonly get
			{
				return new Vector4h(this.Y, this.X, this.Z, this.W);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.Z = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x0003180D File Offset: 0x0002FA0D
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x0003182C File Offset: 0x0002FA2C
		[XmlIgnore]
		public Vector4h Yxwz
		{
			readonly get
			{
				return new Vector4h(this.Y, this.X, this.W, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0003185E File Offset: 0x0002FA5E
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x0003187D File Offset: 0x0002FA7D
		[XmlIgnore]
		public Vector4h Yyzw
		{
			readonly get
			{
				return new Vector4h(this.Y, this.Y, this.Z, this.W);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x000318AF File Offset: 0x0002FAAF
		// (set) Token: 0x06000DBD RID: 3517 RVA: 0x000318CE File Offset: 0x0002FACE
		[XmlIgnore]
		public Vector4h Yywz
		{
			readonly get
			{
				return new Vector4h(this.Y, this.Y, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00031900 File Offset: 0x0002FB00
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x0003191F File Offset: 0x0002FB1F
		[XmlIgnore]
		public Vector4h Yzxw
		{
			readonly get
			{
				return new Vector4h(this.Y, this.Z, this.X, this.W);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.X = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00031951 File Offset: 0x0002FB51
		// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x00031970 File Offset: 0x0002FB70
		[XmlIgnore]
		public Vector4h Yzwx
		{
			readonly get
			{
				return new Vector4h(this.Y, this.Z, this.W, this.X);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.W = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x000319A2 File Offset: 0x0002FBA2
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x000319C1 File Offset: 0x0002FBC1
		[XmlIgnore]
		public Vector4h Ywxz
		{
			readonly get
			{
				return new Vector4h(this.Y, this.W, this.X, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.X = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x000319F3 File Offset: 0x0002FBF3
		// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x00031A12 File Offset: 0x0002FC12
		[XmlIgnore]
		public Vector4h Ywzx
		{
			readonly get
			{
				return new Vector4h(this.Y, this.W, this.Z, this.X);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00031A44 File Offset: 0x0002FC44
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x00031A63 File Offset: 0x0002FC63
		[XmlIgnore]
		public Vector4h Zxyw
		{
			readonly get
			{
				return new Vector4h(this.Z, this.X, this.Y, this.W);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00031A95 File Offset: 0x0002FC95
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x00031AB4 File Offset: 0x0002FCB4
		[XmlIgnore]
		public Vector4h Zxwy
		{
			readonly get
			{
				return new Vector4h(this.Z, this.X, this.W, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.W = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x00031AE6 File Offset: 0x0002FCE6
		// (set) Token: 0x06000DCB RID: 3531 RVA: 0x00031B05 File Offset: 0x0002FD05
		[XmlIgnore]
		public Vector4h Zyxw
		{
			readonly get
			{
				return new Vector4h(this.Z, this.Y, this.X, this.W);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.X = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x00031B37 File Offset: 0x0002FD37
		// (set) Token: 0x06000DCD RID: 3533 RVA: 0x00031B56 File Offset: 0x0002FD56
		[XmlIgnore]
		public Vector4h Zywx
		{
			readonly get
			{
				return new Vector4h(this.Z, this.Y, this.W, this.X);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x00031B88 File Offset: 0x0002FD88
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x00031BA7 File Offset: 0x0002FDA7
		[XmlIgnore]
		public Vector4h Zwxy
		{
			readonly get
			{
				return new Vector4h(this.Z, this.W, this.X, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.X = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00031BD9 File Offset: 0x0002FDD9
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x00031BF8 File Offset: 0x0002FDF8
		[XmlIgnore]
		public Vector4h Zwyx
		{
			readonly get
			{
				return new Vector4h(this.Z, this.W, this.Y, this.X);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.Y = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x00031C2A File Offset: 0x0002FE2A
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x00031C49 File Offset: 0x0002FE49
		[XmlIgnore]
		public Vector4h Zwzy
		{
			readonly get
			{
				return new Vector4h(this.Z, this.W, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x00031C7B File Offset: 0x0002FE7B
		// (set) Token: 0x06000DD5 RID: 3541 RVA: 0x00031C9A File Offset: 0x0002FE9A
		[XmlIgnore]
		public Vector4h Wxyz
		{
			readonly get
			{
				return new Vector4h(this.W, this.X, this.Y, this.Z);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Y = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x00031CCC File Offset: 0x0002FECC
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x00031CEB File Offset: 0x0002FEEB
		[XmlIgnore]
		public Vector4h Wxzy
		{
			readonly get
			{
				return new Vector4h(this.W, this.X, this.Z, this.Y);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x00031D1D File Offset: 0x0002FF1D
		// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x00031D3C File Offset: 0x0002FF3C
		[XmlIgnore]
		public Vector4h Wyxz
		{
			readonly get
			{
				return new Vector4h(this.W, this.Y, this.X, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.X = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x00031D6E File Offset: 0x0002FF6E
		// (set) Token: 0x06000DDB RID: 3547 RVA: 0x00031D8D File Offset: 0x0002FF8D
		[XmlIgnore]
		public Vector4h Wyzx
		{
			readonly get
			{
				return new Vector4h(this.W, this.Y, this.Z, this.X);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x00031DBF File Offset: 0x0002FFBF
		// (set) Token: 0x06000DDD RID: 3549 RVA: 0x00031DDE File Offset: 0x0002FFDE
		[XmlIgnore]
		public Vector4h Wzxy
		{
			readonly get
			{
				return new Vector4h(this.W, this.Z, this.X, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.X = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000DDE RID: 3550 RVA: 0x00031E10 File Offset: 0x00030010
		// (set) Token: 0x06000DDF RID: 3551 RVA: 0x00031E2F File Offset: 0x0003002F
		[XmlIgnore]
		public Vector4h Wzyx
		{
			readonly get
			{
				return new Vector4h(this.W, this.Z, this.Y, this.X);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x00031E61 File Offset: 0x00030061
		// (set) Token: 0x06000DE1 RID: 3553 RVA: 0x00031E80 File Offset: 0x00030080
		[XmlIgnore]
		public Vector4h Wzyw
		{
			readonly get
			{
				return new Vector4h(this.W, this.Z, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00031EB2 File Offset: 0x000300B2
		public readonly Vector4 ToVector4()
		{
			return new Vector4(this.X, this.Y, this.Z, this.W);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00031EE5 File Offset: 0x000300E5
		public readonly Vector4d ToVector4d()
		{
			return new Vector4d(this.X, this.Y, this.Z, this.W);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00031F18 File Offset: 0x00030118
		public static explicit operator Vector4h(Vector4 v4f)
		{
			return new Vector4h(v4f);
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00031F20 File Offset: 0x00030120
		public static explicit operator Vector4h(Vector4d v4d)
		{
			return new Vector4h(v4d);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00031F28 File Offset: 0x00030128
		public static implicit operator Vector4(Vector4h vec)
		{
			return new Vector4(vec.X, vec.Y, vec.Z, vec.W);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00031F5B File Offset: 0x0003015B
		public static implicit operator Vector4d(Vector4h vec)
		{
			return new Vector4d(vec.X, vec.Y, vec.Z, vec.W);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00031F8E File Offset: 0x0003018E
		public static explicit operator Vector4i(Vector4h vec)
		{
			return new Vector4i((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00031FC9 File Offset: 0x000301C9
		public static implicit operator Vector4h([TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z",
			"W"
		})] ValueTuple<Half, Half, Half, Half> values)
		{
			return new Vector4h(values.Item1, values.Item2, values.Item3, values.Item4);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00031FE8 File Offset: 0x000301E8
		public static bool operator ==(Vector4h left, Vector4h right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00031FF2 File Offset: 0x000301F2
		public static bool operator !=(Vector4h left, Vector4h right)
		{
			return !(left == right);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00032000 File Offset: 0x00030200
		public Vector4h(SerializationInfo info, StreamingContext context)
		{
			this.X = (Half)info.GetValue("X", typeof(Half));
			this.Y = (Half)info.GetValue("Y", typeof(Half));
			this.Z = (Half)info.GetValue("Z", typeof(Half));
			this.W = (Half)info.GetValue("W", typeof(Half));
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00032090 File Offset: 0x00030290
		public readonly void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("X", this.X);
			info.AddValue("Y", this.Y);
			info.AddValue("Z", this.Z);
			info.AddValue("W", this.W);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x000320F5 File Offset: 0x000302F5
		public void FromBinaryStream(BinaryReader bin)
		{
			this.X.FromBinaryStream(bin);
			this.Y.FromBinaryStream(bin);
			this.Z.FromBinaryStream(bin);
			this.W.FromBinaryStream(bin);
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00032127 File Offset: 0x00030327
		public void ToBinaryStream(BinaryWriter bin)
		{
			this.X.ToBinaryStream(bin);
			this.Y.ToBinaryStream(bin);
			this.Z.ToBinaryStream(bin);
			this.W.ToBinaryStream(bin);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00032159 File Offset: 0x00030359
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00032163 File Offset: 0x00030363
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0003216D File Offset: 0x0003036D
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00032178 File Offset: 0x00030378
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("({0}{4} {1}{4} {2}{4} {3})", new object[]
			{
				this.X.ToString(format, formatProvider),
				this.Y.ToString(format, formatProvider),
				this.Z.ToString(format, formatProvider),
				this.W.ToString(format, formatProvider),
				MathHelper.GetListSeparator(formatProvider)
			});
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x000321DE File Offset: 0x000303DE
		public override bool Equals(object obj)
		{
			return obj is Vector4h && this.Equals((Vector4h)obj);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x000321F8 File Offset: 0x000303F8
		public bool Equals(Vector4h other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y) && this.Z.Equals(other.Z) && this.W.Equals(other.W);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00032251 File Offset: 0x00030451
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Half, Half, Half, Half>(this.X, this.Y, this.Z, this.W);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00032270 File Offset: 0x00030470
		public static byte[] GetBytes(Vector4h h)
		{
			byte[] array = new byte[Vector4h.SizeInBytes];
			byte[] bytes = Half.GetBytes(h.X);
			array[0] = bytes[0];
			array[1] = bytes[1];
			bytes = Half.GetBytes(h.Y);
			array[2] = bytes[0];
			array[3] = bytes[1];
			bytes = Half.GetBytes(h.Z);
			array[4] = bytes[0];
			array[5] = bytes[1];
			bytes = Half.GetBytes(h.W);
			array[6] = bytes[0];
			array[7] = bytes[1];
			return array;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x000322E7 File Offset: 0x000304E7
		public static Vector4h FromBytes(byte[] value, int startIndex)
		{
			return new Vector4h(Half.FromBytes(value, startIndex), Half.FromBytes(value, startIndex + 2), Half.FromBytes(value, startIndex + 4), Half.FromBytes(value, startIndex + 6));
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00032310 File Offset: 0x00030510
		public readonly void Deconstruct(out Half x, out Half y, out Half z, out Half w)
		{
			x = this.X;
			y = this.Y;
			z = this.Z;
			w = this.W;
		}

		// Token: 0x040000DF RID: 223
		public Half X;

		// Token: 0x040000E0 RID: 224
		public Half Y;

		// Token: 0x040000E1 RID: 225
		public Half Z;

		// Token: 0x040000E2 RID: 226
		public Half W;

		// Token: 0x040000E3 RID: 227
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector4h>();
	}
}
