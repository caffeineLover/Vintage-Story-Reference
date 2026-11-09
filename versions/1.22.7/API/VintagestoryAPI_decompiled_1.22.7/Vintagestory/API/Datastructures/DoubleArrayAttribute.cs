using System;
using System.IO;
using System.Text;
using Vintagestory.API.Config;

namespace Vintagestory.API.Datastructures;

public class DoubleArrayAttribute : ArrayAttribute<double>, IAttribute
{
	public DoubleArrayAttribute()
	{
	}

	public DoubleArrayAttribute(double[] value)
	{
		base.value = value;
	}

	public void ToBytes(BinaryWriter stream)
	{
		double[] values = value;
		stream.Write(values.Length);
		for (int i = 0; i < values.Length; i++)
		{
			stream.Write(values[i]);
		}
	}

	public void FromBytes(BinaryReader stream)
	{
		int quantity = stream.ReadInt32();
		double[] values = GC.AllocateUninitializedArray<double>(quantity);
		for (int i = 0; i < quantity; i++)
		{
			values[i] = stream.ReadDouble();
		}
		value = values;
	}

	public int GetAttributeId()
	{
		return 13;
	}

	public override string ToJsonToken()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append('[');
		for (int i = 0; i < value.Length; i++)
		{
			if (i > 0)
			{
				sb.Append(", ");
			}
			sb.Append(value[i].ToString(GlobalConstants.DefaultCultureInfo));
		}
		sb.Append(']');
		return sb.ToString();
	}

	public IAttribute Clone()
	{
		return new DoubleArrayAttribute(value);
	}

	Type IAttribute.GetType()
	{
		return GetType();
	}
}
