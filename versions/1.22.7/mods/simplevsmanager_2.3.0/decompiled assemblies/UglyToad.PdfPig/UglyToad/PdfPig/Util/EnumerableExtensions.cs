using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x0200002E RID: 46
	internal static class EnumerableExtensions
	{
		// Token: 0x06000132 RID: 306 RVA: 0x0000BD70 File Offset: 0x00009F70
		public static List<T> ToRecursiveOrderList<T>(this IEnumerable<T> collection, Expression<Func<T, IEnumerable<T>>> childCollection)
		{
			List<T> list = new List<T>();
			Func<T, ValueTuple<int, T, int>> func;
			if ((func = EnumerableExtensions.<>c__0<T>.<>9__0_0) == null)
			{
				func = (EnumerableExtensions.<>c__0<T>.<>9__0_0 = new Func<T, ValueTuple<int, T, int>>(EnumerableExtensions.<>c__0<T>.<>9.<ToRecursiveOrderList>b__0_0));
			}
			Queue<ValueTuple<int, T, int>> queue = new Queue<ValueTuple<int, T, int>>(Enumerable.Select<T, ValueTuple<int, T, int>>(collection, func));
			int num = 0;
			int num2 = 0;
			PropertyInfo propertyInfo = (PropertyInfo)((MemberExpression)childCollection.Body).Member;
			while (queue.Count > 0)
			{
				ValueTuple<int, T, int> valueTuple = queue.Dequeue();
				if (valueTuple.Item3 != num2)
				{
					num = 0;
				}
				int num3 = valueTuple.Item1 + num++;
				list.Insert(num3, valueTuple.Item2);
				foreach (T t in ((propertyInfo.GetValue(valueTuple.Item2) as IEnumerable<T>) ?? Enumerable.Empty<T>()))
				{
					queue.Enqueue(new ValueTuple<int, T, int>(num3 + 1, t, valueTuple.Item3 + 1));
				}
				num2 = valueTuple.Item3;
			}
			return list;
		}
	}
}
