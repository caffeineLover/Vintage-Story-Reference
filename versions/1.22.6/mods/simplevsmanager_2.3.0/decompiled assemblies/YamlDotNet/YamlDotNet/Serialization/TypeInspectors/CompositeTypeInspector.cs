using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000056 RID: 86
	[NullableContext(1)]
	[Nullable(0)]
	public class CompositeTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x06000237 RID: 567 RVA: 0x00007A23 File Offset: 0x00005C23
		public CompositeTypeInspector(params ITypeInspector[] typeInspectors) : this(typeInspectors)
		{
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00007A2C File Offset: 0x00005C2C
		public CompositeTypeInspector(IEnumerable<ITypeInspector> typeInspectors)
		{
			List<ITypeInspector> list = (typeInspectors != null) ? Enumerable.ToList<ITypeInspector>(typeInspectors) : null;
			if (list == null)
			{
				throw new ArgumentNullException("typeInspectors");
			}
			this.typeInspectors = list;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00007A58 File Offset: 0x00005C58
		public override string GetEnumName(Type enumType, string name)
		{
			foreach (ITypeInspector typeInspector in this.typeInspectors)
			{
				try
				{
					return typeInspector.GetEnumName(enumType, name);
				}
				catch
				{
				}
			}
			throw new ArgumentOutOfRangeException("enumType,name", "Name not found on enum type");
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00007ACC File Offset: 0x00005CCC
		public override string GetEnumValue(object enumValue)
		{
			if (enumValue == null)
			{
				throw new ArgumentNullException("enumValue");
			}
			foreach (ITypeInspector typeInspector in this.typeInspectors)
			{
				try
				{
					return typeInspector.GetEnumValue(enumValue);
				}
				catch
				{
				}
			}
			string text = "enumValue";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Value not found for (");
			defaultInterpolatedStringHandler.AppendFormatted<object>(enumValue);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00007B78 File Offset: 0x00005D78
		public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container)
		{
			return Enumerable.SelectMany<ITypeInspector, IPropertyDescriptor>(this.typeInspectors, (ITypeInspector i) => i.GetProperties(type, container));
		}

		// Token: 0x0400008C RID: 140
		private readonly IEnumerable<ITypeInspector> typeInspectors;
	}
}
