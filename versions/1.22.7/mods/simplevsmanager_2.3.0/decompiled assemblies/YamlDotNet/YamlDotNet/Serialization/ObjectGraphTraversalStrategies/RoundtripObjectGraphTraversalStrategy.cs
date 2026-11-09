using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ObjectGraphTraversalStrategies
{
	// Token: 0x0200006C RID: 108
	[NullableContext(1)]
	[Nullable(0)]
	public class RoundtripObjectGraphTraversalStrategy : FullObjectGraphTraversalStrategy
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x00008F3E File Offset: 0x0000713E
		public RoundtripObjectGraphTraversalStrategy(IEnumerable<IYamlTypeConverter> converters, ITypeInspector typeDescriptor, ITypeResolver typeResolver, int maxRecursion, INamingConvention namingConvention, Settings settings, IObjectFactory factory) : base(typeDescriptor, typeResolver, maxRecursion, namingConvention, factory)
		{
			this.converters = new TypeConverterCache(converters);
			this.settings = settings;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00008F64 File Offset: 0x00007164
		protected override void TraverseProperties<[Nullable(2)] TContext>(IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path, ObjectSerializer serializer)
		{
			IYamlTypeConverter yamlTypeConverter;
			if (!value.Type.HasDefaultConstructor(this.settings.AllowPrivateConstructors) && !this.converters.TryGetConverterForType(value.Type, out yamlTypeConverter))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(98, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Type '");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(value.Type);
				defaultInterpolatedStringHandler.AppendLiteral("' cannot be deserialized because it does not have a default constructor or a type converter.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			base.TraverseProperties<TContext>(value, visitor, context, path, serializer);
		}

		// Token: 0x040000A7 RID: 167
		private readonly TypeConverterCache converters;

		// Token: 0x040000A8 RID: 168
		private readonly Settings settings;
	}
}
