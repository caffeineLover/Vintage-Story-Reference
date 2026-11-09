using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x02000113 RID: 275
	public abstract class TypeInspectorSkeleton : ITypeInspector
	{
		// Token: 0x060005E7 RID: 1511
		[NullableContext(1)]
		public abstract IEnumerable<IPropertyDescriptor> GetProperties(Type type, [Nullable(2)] object container);

		// Token: 0x060005E8 RID: 1512 RVA: 0x000150E8 File Offset: 0x000132E8
		[NullableContext(1)]
		public IPropertyDescriptor GetProperty(Type type, [Nullable(2)] object container, string name, [MaybeNullWhen(true)] bool ignoreUnmatched)
		{
			IEnumerable<IPropertyDescriptor> enumerable = Enumerable.Where<IPropertyDescriptor>(this.GetProperties(type, container), (IPropertyDescriptor p) => p.Name == name);
			IPropertyDescriptor result;
			using (IEnumerator<IPropertyDescriptor> enumerator = enumerable.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					if (!ignoreUnmatched)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Property '");
						defaultInterpolatedStringHandler.AppendFormatted(name);
						defaultInterpolatedStringHandler.AppendLiteral("' not found on type '");
						defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
						defaultInterpolatedStringHandler.AppendLiteral("'.");
						throw new SerializationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					result = null;
				}
				else
				{
					IPropertyDescriptor propertyDescriptor = enumerator.Current;
					if (enumerator.MoveNext())
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(183, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Multiple properties with the name/alias '");
						defaultInterpolatedStringHandler.AppendFormatted(name);
						defaultInterpolatedStringHandler.AppendLiteral("' already exists on type '");
						defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
						defaultInterpolatedStringHandler.AppendLiteral("', maybe you're misusing YamlAlias or maybe you are using the wrong naming convention? The matching properties are: ");
						defaultInterpolatedStringHandler.AppendFormatted(string.Join(", ", Enumerable.ToArray<string>(Enumerable.Select<IPropertyDescriptor, string>(enumerable, (IPropertyDescriptor p) => p.Name))));
						throw new SerializationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					result = propertyDescriptor;
				}
			}
			return result;
		}
	}
}
