using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Vintagestory.API.MathTools;

namespace VSImGui
{
	// Token: 0x02000012 RID: 18
	[NullableContext(1)]
	[Nullable(0)]
	[JsonConverter(typeof(ValueConverter<Value3>))]
	public struct Value3 : IFloatArrayValue
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000AE83 File Offset: 0x00009083
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0000AE8B File Offset: 0x0000908B
		public Vector3 Value { readonly get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000AE94 File Offset: 0x00009094
		public float X
		{
			get
			{
				return this.Value.X;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000AEA1 File Offset: 0x000090A1
		public float Y
		{
			get
			{
				return this.Value.Y;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000AEAE File Offset: 0x000090AE
		public float Z
		{
			get
			{
				return this.Value.Z;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000AEBB File Offset: 0x000090BB
		public float R
		{
			get
			{
				return this.Value.X;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000AEC8 File Offset: 0x000090C8
		public float G
		{
			get
			{
				return this.Value.Y;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000AED5 File Offset: 0x000090D5
		public float B
		{
			get
			{
				return this.Value.Z;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000AEE2 File Offset: 0x000090E2
		public Value2 XY
		{
			get
			{
				return new Value2(this.X, this.Y);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000AEF5 File Offset: 0x000090F5
		// (set) Token: 0x060001AC RID: 428 RVA: 0x0000AF18 File Offset: 0x00009118
		public float[] Array
		{
			get
			{
				return new float[]
				{
					this.X,
					this.Y,
					this.Z
				};
			}
			set
			{
				this.Value = new Vector3(value);
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000AF2B File Offset: 0x0000912B
		public Value3(Vector3 value)
		{
			this.Value = value;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000AF34 File Offset: 0x00009134
		[NullableContext(0)]
		public Value3([TupleElementNames(new string[]
		{
			"first",
			"second",
			"third"
		})] ValueTuple<float, float, float> values)
		{
			this.Value = new Vector3(values.Item1, values.Item2, values.Item3);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000AF53 File Offset: 0x00009153
		public Value3(Vec3f value)
		{
			this.Value = new Vector3(value.X, value.Y, value.Z);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000AF72 File Offset: 0x00009172
		public Value3(float first, float second, float third)
		{
			this.Value = new Vector3(first, second, third);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000AF82 File Offset: 0x00009182
		public static implicit operator Value3(Vector3 value)
		{
			return new Value3(value);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000AF8A File Offset: 0x0000918A
		[NullableContext(0)]
		public static implicit operator Value3([TupleElementNames(new string[]
		{
			"first",
			"second",
			"third"
		})] ValueTuple<float, float, float> values)
		{
			return new Value3(values);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000AF92 File Offset: 0x00009192
		public static implicit operator Value3(Vec3f value)
		{
			return new Value3(value);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000AF9A File Offset: 0x0000919A
		public static implicit operator Vector3(Value3 value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000AFB6 File Offset: 0x000091B6
		public override string ToString()
		{
			return this.Array.ToString();
		}
	}
}
