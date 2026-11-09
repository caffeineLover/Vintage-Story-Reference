using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Vintagestory.API.MathTools;

namespace VSImGui
{
	// Token: 0x02000013 RID: 19
	[JsonConverter(typeof(ValueConverter<Value4>))]
	public struct Value4 : IFloatArrayValue
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000AFC3 File Offset: 0x000091C3
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x0000AFCB File Offset: 0x000091CB
		public Vector4 Value { readonly get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000AFD4 File Offset: 0x000091D4
		public float X
		{
			get
			{
				return this.Value.X;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000AFE1 File Offset: 0x000091E1
		public float Y
		{
			get
			{
				return this.Value.Y;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000AFEE File Offset: 0x000091EE
		public float Z
		{
			get
			{
				return this.Value.Z;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000AFFB File Offset: 0x000091FB
		public float W
		{
			get
			{
				return this.Value.W;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000B008 File Offset: 0x00009208
		public float R
		{
			get
			{
				return this.Value.X;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000B015 File Offset: 0x00009215
		public float G
		{
			get
			{
				return this.Value.Y;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000B022 File Offset: 0x00009222
		public float B
		{
			get
			{
				return this.Value.Z;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000B02F File Offset: 0x0000922F
		public float A
		{
			get
			{
				return this.Value.W;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000B03C File Offset: 0x0000923C
		public Value3 RGB
		{
			get
			{
				return new Value3(this.R, this.G, this.B);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000B055 File Offset: 0x00009255
		public Value3 XYZ
		{
			get
			{
				return new Value3(this.X, this.Y, this.Z);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000B06E File Offset: 0x0000926E
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x0000B09A File Offset: 0x0000929A
		[Nullable(1)]
		[JsonProperty]
		public float[] Array
		{
			[NullableContext(1)]
			get
			{
				return new float[]
				{
					this.X,
					this.Y,
					this.Z,
					this.W
				};
			}
			[NullableContext(1)]
			set
			{
				this.Value = new Vector4(value);
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000B0AD File Offset: 0x000092AD
		public Value4(Vector4 value)
		{
			this.Value = value;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000B0B6 File Offset: 0x000092B6
		public Value4([TupleElementNames(new string[]
		{
			"first",
			"second",
			"third",
			"fourth"
		})] ValueTuple<float, float, float, float> values)
		{
			this.Value = new Vector4(values.Item1, values.Item2, values.Item3, values.Item4);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000B0DB File Offset: 0x000092DB
		[NullableContext(1)]
		public Value4(Vec4f value)
		{
			this.Value = new Vector4(value.X, value.Y, value.Z, value.W);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000B100 File Offset: 0x00009300
		public Value4(float first, float second, float third, float fourth)
		{
			this.Value = new Vector4(first, second, third, fourth);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000B112 File Offset: 0x00009312
		public Value4([TupleElementNames(new string[]
		{
			"RGB",
			"alpha"
		})] ValueTuple<Value3, float> values)
		{
			this.Value = new Vector4(values.Item1.X, values.Item1.Y, values.Item1.Z, values.Item2);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000B149 File Offset: 0x00009349
		public static implicit operator Value4(Vector4 value)
		{
			return new Value4(value);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000B151 File Offset: 0x00009351
		public static implicit operator Value4([TupleElementNames(new string[]
		{
			"first",
			"second",
			"third",
			"fourth"
		})] ValueTuple<float, float, float, float> values)
		{
			return new Value4(values);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000B159 File Offset: 0x00009359
		public static implicit operator Value4([TupleElementNames(new string[]
		{
			"RGB",
			"alpha"
		})] ValueTuple<Value3, float> values)
		{
			return new Value4(values);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000B161 File Offset: 0x00009361
		[NullableContext(1)]
		public static implicit operator Value4(Vec4f value)
		{
			return new Value4(value);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000B169 File Offset: 0x00009369
		public static implicit operator Vector4(Value4 value)
		{
			return new Vector4(value.X, value.Y, value.Z, value.W);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000B18C File Offset: 0x0000938C
		[NullableContext(1)]
		public override string ToString()
		{
			return this.Array.ToString();
		}
	}
}
