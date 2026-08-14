using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Vintagestory.API.MathTools;

namespace VSImGui
{
	// Token: 0x02000011 RID: 17
	[NullableContext(1)]
	[Nullable(0)]
	[JsonConverter(typeof(ValueConverter<Value2>))]
	public struct Value2 : IFloatArrayValue
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000ADA7 File Offset: 0x00008FA7
		// (set) Token: 0x06000194 RID: 404 RVA: 0x0000ADAF File Offset: 0x00008FAF
		public Vector2 Value { readonly get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000ADB8 File Offset: 0x00008FB8
		public float X
		{
			get
			{
				return this.Value.X;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000ADC5 File Offset: 0x00008FC5
		public float Y
		{
			get
			{
				return this.Value.Y;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000ADD2 File Offset: 0x00008FD2
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000ADEC File Offset: 0x00008FEC
		public float[] Array
		{
			get
			{
				return new float[]
				{
					this.X,
					this.Y
				};
			}
			set
			{
				this.Value = new Vector2(value);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000ADFF File Offset: 0x00008FFF
		public Value2(Vector2 value)
		{
			this.Value = value;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000AE08 File Offset: 0x00009008
		[NullableContext(0)]
		public Value2([TupleElementNames(new string[]
		{
			"first",
			"second"
		})] ValueTuple<float, float> values)
		{
			this.Value = new Vector2(values.Item1, values.Item2);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000AE21 File Offset: 0x00009021
		public Value2(Vec2f value)
		{
			this.Value = new Vector2(value.X, value.Y);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000AE3A File Offset: 0x0000903A
		public Value2(float first, float second)
		{
			this.Value = new Vector2(first, second);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000AE49 File Offset: 0x00009049
		public static implicit operator Value2(Vector2 value)
		{
			return new Value2(value);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000AE51 File Offset: 0x00009051
		[NullableContext(0)]
		public static implicit operator Value2([TupleElementNames(new string[]
		{
			"first",
			"second"
		})] ValueTuple<float, float> values)
		{
			return new Value2(values);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000AE59 File Offset: 0x00009059
		public static implicit operator Value2(Vec2f value)
		{
			return new Value2(value);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000AE61 File Offset: 0x00009061
		public static implicit operator Vector2(Value2 value)
		{
			return new Vector2(value.X, value.Y);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000AE76 File Offset: 0x00009076
		public override string ToString()
		{
			return this.Array.ToString();
		}
	}
}
