using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.ObjectGraphTraversalStrategies
{
	// Token: 0x02000132 RID: 306
	[NullableContext(1)]
	[Nullable(0)]
	public class RoundtripObjectGraphTraversalStrategy : FullObjectGraphTraversalStrategy
	{
		// Token: 0x06000690 RID: 1680 RVA: 0x00016BAA File Offset: 0x00014DAA
		public RoundtripObjectGraphTraversalStrategy(IEnumerable<IYamlTypeConverter> converters, ITypeInspector typeDescriptor, ITypeResolver typeResolver, int maxRecursion, INamingConvention namingConvention, Settings settings, IObjectFactory factory) : base(typeDescriptor, typeResolver, maxRecursion, namingConvention, factory)
		{
			this.converters = converters;
			this.settings = settings;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00016BCC File Offset: 0x00014DCC
		protected override void TraverseProperties<[Nullable(2)] TContext>(IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<FullObjectGraphTraversalStrategy.ObjectPathSegment> path)
		{
			RoundtripObjectGraphTraversalStrategy.<>c__DisplayClass3_0<TContext> CS$<>8__locals1 = new RoundtripObjectGraphTraversalStrategy.<>c__DisplayClass3_0<TContext>();
			CS$<>8__locals1.value = value;
			if (!CS$<>8__locals1.value.Type.HasDefaultConstructor(this.settings.AllowPrivateConstructors) && !Enumerable.Any<IYamlTypeConverter>(this.converters, new Func<IYamlTypeConverter, bool>(CS$<>8__locals1.<TraverseProperties>b__0)))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(98, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Type '");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(CS$<>8__locals1.value.Type);
				defaultInterpolatedStringHandler.AppendLiteral("' cannot be deserialized because it does not have a default constructor or a type converter.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			base.TraverseProperties<TContext>(CS$<>8__locals1.value, visitor, context, path);
		}

		// Token: 0x040002EF RID: 751
		private readonly IEnumerable<IYamlTypeConverter> converters;

		// Token: 0x040002F0 RID: 752
		private readonly Settings settings;
	}
}
