using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x02000062 RID: 98
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ReflectionContext<[Nullable(2)] TResult, [Nullable(2)] TArguments> : IContext<TResult, TArguments>
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000EC2E File Offset: 0x0000CE2E
		public ReflectionContext(object source)
		{
			this.mSource = source;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000EC40 File Offset: 0x0000CE40
		public TResult Resolve(string name, params TArguments[] arguments)
		{
			if (arguments.Length != 0)
			{
				ValueTuple<bool, TResult> valueTuple = this.CallFunction(name, arguments);
				bool resolved = valueTuple.Item1;
				TResult value = valueTuple.Item2;
				if (!resolved || value == null)
				{
					throw new InvalidDataException("Unknown function: '" + name + "'");
				}
				return value;
			}
			else
			{
				ValueTuple<bool, TResult> valueTuple2 = this.ResolveProperty(name);
				bool propertyResolved = valueTuple2.Item1;
				TResult propertyValue = valueTuple2.Item2;
				if (propertyResolved && propertyValue != null)
				{
					return propertyValue;
				}
				ValueTuple<bool, TResult> valueTuple3 = this.ResolveField(name);
				bool fieldResolved = valueTuple3.Item1;
				TResult fieldValue = valueTuple3.Item2;
				if (fieldResolved && fieldValue != null)
				{
					return fieldValue;
				}
				ValueTuple<bool, TResult> valueTuple4 = this.CallFunction(name, arguments);
				bool functionResolved = valueTuple4.Item1;
				TResult functionValue = valueTuple4.Item2;
				if (functionResolved && functionValue != null)
				{
					return functionValue;
				}
				throw new InvalidDataException("Unknown function, property or field: '" + name + "'");
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000ED0C File Offset: 0x0000CF0C
		public bool Resolvable(string name)
		{
			return this.mSource.GetType().GetProperty(name, 52) != null || this.mSource.GetType().GetField(name, 52) != null || this.mSource.GetType().GetMethod(name, 52) != null;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000ED74 File Offset: 0x0000CF74
		[return: TupleElementNames(new string[]
		{
			"resolved",
			"value"
		})]
		[return: Nullable(new byte[]
		{
			0,
			2
		})]
		private ValueTuple<bool, TResult> ResolveProperty(string name)
		{
			PropertyInfo property = this.mSource.GetType().GetProperty(name, 52);
			object value = (property != null) ? property.GetValue(this.mSource) : null;
			return new ValueTuple<bool, TResult>(value != null, (value == null) ? default(TResult) : ((TResult)((object)value)));
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
		[return: TupleElementNames(new string[]
		{
			"resolved",
			"value"
		})]
		[return: Nullable(new byte[]
		{
			0,
			2
		})]
		private ValueTuple<bool, TResult> ResolveField(string name)
		{
			FieldInfo field = this.mSource.GetType().GetField(name, 52);
			object value = (field != null) ? field.GetValue(this.mSource) : null;
			return new ValueTuple<bool, TResult>(value != null, (value == null) ? default(TResult) : ((TResult)((object)value)));
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000EE14 File Offset: 0x0000D014
		[return: TupleElementNames(new string[]
		{
			"resolved",
			"value"
		})]
		[return: Nullable(new byte[]
		{
			0,
			2
		})]
		public ValueTuple<bool, TResult> CallFunction(string name, params TArguments[] arguments)
		{
			MethodInfo methodInfo = this.mSource.GetType().GetMethod(name, 52);
			if (methodInfo == null)
			{
				return new ValueTuple<bool, TResult>(false, default(TResult));
			}
			bool flag = true;
			MethodBase methodBase = methodInfo;
			object obj = this.mSource;
			Func<TArguments, object> func;
			if ((func = ReflectionContext<TResult, TArguments>.<>c.<>9__6_0) == null)
			{
				func = (ReflectionContext<TResult, TArguments>.<>c.<>9__6_0 = new Func<TArguments, object>(ReflectionContext<TResult, TArguments>.<>c.<>9.<CallFunction>b__6_0));
			}
			return new ValueTuple<bool, TResult>(flag, (TResult)((object)methodBase.Invoke(obj, Enumerable.ToArray<object>(Enumerable.Select<TArguments, object>(arguments, func)))));
		}

		// Token: 0x04000164 RID: 356
		private readonly object mSource;
	}
}
