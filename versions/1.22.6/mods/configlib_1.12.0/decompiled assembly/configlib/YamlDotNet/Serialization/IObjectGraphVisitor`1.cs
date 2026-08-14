using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009F RID: 159
	[NullableContext(1)]
	public interface IObjectGraphVisitor<[Nullable(2)] TContext>
	{
		// Token: 0x060003D8 RID: 984
		bool Enter(IObjectDescriptor value, TContext context);

		// Token: 0x060003D9 RID: 985
		bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, TContext context);

		// Token: 0x060003DA RID: 986
		bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, TContext context);

		// Token: 0x060003DB RID: 987
		void VisitScalar(IObjectDescriptor scalar, TContext context);

		// Token: 0x060003DC RID: 988
		void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, TContext context);

		// Token: 0x060003DD RID: 989
		void VisitMappingEnd(IObjectDescriptor mapping, TContext context);

		// Token: 0x060003DE RID: 990
		void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, TContext context);

		// Token: 0x060003DF RID: 991
		void VisitSequenceEnd(IObjectDescriptor sequence, TContext context);
	}
}
