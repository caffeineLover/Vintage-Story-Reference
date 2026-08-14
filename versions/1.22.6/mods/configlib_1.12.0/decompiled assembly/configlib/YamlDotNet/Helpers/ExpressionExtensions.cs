using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x02000196 RID: 406
	[NullableContext(1)]
	[Nullable(0)]
	public static class ExpressionExtensions
	{
		// Token: 0x060008A3 RID: 2211 RVA: 0x0001C9A2 File Offset: 0x0001ABA2
		public static PropertyInfo AsProperty(this LambdaExpression propertyAccessor)
		{
			PropertyInfo propertyInfo = ExpressionExtensions.TryGetMemberExpression<PropertyInfo>(propertyAccessor);
			if (propertyInfo == null)
			{
				throw new ArgumentException("Expected a lambda expression in the form: x => x.SomeProperty", "propertyAccessor");
			}
			return propertyInfo;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001C9C4 File Offset: 0x0001ABC4
		[return: MaybeNull]
		private static TMemberInfo TryGetMemberExpression<[Nullable(0)] TMemberInfo>(LambdaExpression lambdaExpression) where TMemberInfo : MemberInfo
		{
			if (lambdaExpression.Parameters.Count != 1)
			{
				return default(TMemberInfo);
			}
			Expression expression = lambdaExpression.Body;
			UnaryExpression unaryExpression = expression as UnaryExpression;
			if (unaryExpression != null)
			{
				if (unaryExpression.NodeType != 10)
				{
					return default(TMemberInfo);
				}
				expression = unaryExpression.Operand;
			}
			MemberExpression memberExpression = expression as MemberExpression;
			if (memberExpression == null)
			{
				return default(TMemberInfo);
			}
			if (memberExpression.Expression != lambdaExpression.Parameters[0])
			{
				return default(TMemberInfo);
			}
			return memberExpression.Member as TMemberInfo;
		}
	}
}
