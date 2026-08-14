using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeResolvers
{
	// Token: 0x0200010B RID: 267
	public class StaticTypeResolver : ITypeResolver
	{
		// Token: 0x060005EE RID: 1518 RVA: 0x00015448 File Offset: 0x00013648
		[NullableContext(1)]
		public virtual Type Resolve(Type staticType, [Nullable(2)] object actualValue)
		{
			if (actualValue != null)
			{
				if (actualValue.GetType().IsEnum)
				{
					return staticType;
				}
				switch (actualValue.GetType().GetTypeCode())
				{
				case 3:
					return typeof(bool);
				case 4:
					return typeof(char);
				case 5:
					return typeof(sbyte);
				case 6:
					return typeof(byte);
				case 7:
					return typeof(short);
				case 8:
					return typeof(ushort);
				case 9:
					return typeof(int);
				case 10:
					return typeof(uint);
				case 11:
					return typeof(long);
				case 12:
					return typeof(ulong);
				case 13:
					return typeof(float);
				case 14:
					return typeof(double);
				case 15:
					return typeof(decimal);
				case 16:
					return typeof(DateTime);
				case 18:
					return typeof(string);
				}
			}
			return staticType;
		}
	}
}
