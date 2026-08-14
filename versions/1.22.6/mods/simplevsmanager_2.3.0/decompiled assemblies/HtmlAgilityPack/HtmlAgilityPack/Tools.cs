using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x02000013 RID: 19
	[NullableContext(1)]
	[Nullable(0)]
	internal static class Tools
	{
		// Token: 0x06000167 RID: 359 RVA: 0x0000C4C4 File Offset: 0x0000A6C4
		internal static bool IsDefinedAttribute(this Type type, Type attributeType)
		{
			if (type == null)
			{
				throw new ArgumentNullException("Parameter type is null when checking type defined attributeType or not.");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("Parameter attributeType is null when checking type defined attributeType or not.");
			}
			return type.IsDefined(attributeType, false);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000C4FB File Offset: 0x0000A6FB
		internal static IEnumerable<PropertyInfo> GetPropertiesDefinedXPath(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("Parameter type is null while retrieving properties defined XPathAttribute of Type type.");
			}
			return type.GetProperties().HAPWhere((PropertyInfo x) => x.IsDefined(typeof(XPathAttribute), false));
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000C53C File Offset: 0x0000A73C
		internal static bool IsIEnumerable(this PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("Parameter propertyInfo is null while checking propertyInfo for being IEnumerable or not.");
			}
			return !(propertyInfo.PropertyType == typeof(string)) && typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000C58B File Offset: 0x0000A78B
		internal static IEnumerable<Type> GetGenericTypes(this PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("Parameter propertyInfo is null while Getting generic types of Property.");
			}
			return propertyInfo.PropertyType.GetGenericArguments();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000C5AC File Offset: 0x0000A7AC
		internal static MethodInfo GetMethodByItsName(this Type type, string methodName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("Parameter type is null while Getting method from it.");
			}
			if (methodName == null || methodName == "")
			{
				throw new ArgumentNullException("Parameter methodName is null while Getting method from Type type.");
			}
			return type.GetMethod(methodName);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
		internal static IList CreateIListOfType(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("Parameter type is null while creating List<type>.");
			}
			return Activator.CreateInstance(typeof(List).MakeGenericType(new Type[]
			{
				type
			})) as IList;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000C61D File Offset: 0x0000A81D
		internal static T GetNodeValueBasedOnXPathReturnType<[Nullable(2)] T>(HtmlNode htmlNode, XPathAttribute xPathAttribute)
		{
			if (htmlNode == null)
			{
				throw new ArgumentNullException("parameter html node is null");
			}
			if (xPathAttribute == null)
			{
				throw new ArgumentNullException("parameter xpathAttribute is null");
			}
			return (T)((object)Convert.ChangeType(Tools.GetHtmlForEncapsulation(htmlNode, xPathAttribute.NodeReturnType), typeof(T)));
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000C65C File Offset: 0x0000A85C
		internal static IList GetNodesValuesBasedOnXPathReturnType(HtmlNodeCollection htmlNodeCollection, XPathAttribute xPathAttribute, Type listGenericType)
		{
			if (htmlNodeCollection == null || htmlNodeCollection.Count == 0)
			{
				throw new ArgumentNullException("parameter htmlNodeCollection is null or empty.");
			}
			if (xPathAttribute == null)
			{
				throw new ArgumentNullException("parameter xpathAttribute is null");
			}
			IList list = listGenericType.CreateIListOfType();
			foreach (HtmlNode node in htmlNodeCollection)
			{
				list.Add(Convert.ChangeType(Tools.GetHtmlForEncapsulation(node, xPathAttribute.NodeReturnType), listGenericType));
			}
			return list;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000C6E8 File Offset: 0x0000A8E8
		internal static IEnumerable<TSource> HAPWhere<[Nullable(2)] TSource>(this IEnumerable<TSource> source, Tools.HAPFunc<TSource, bool> predicate)
		{
			Tools.<HAPWhere>d__9<TSource> <HAPWhere>d__ = new Tools.<HAPWhere>d__9<TSource>(-2);
			<HAPWhere>d__.<>3__source = source;
			<HAPWhere>d__.<>3__predicate = predicate;
			return <HAPWhere>d__;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000C6FF File Offset: 0x0000A8FF
		internal static bool IsInstantiable(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type is null");
			}
			return !(type.GetConstructor(Type.EmptyTypes) == null);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000C72C File Offset: 0x0000A92C
		internal static int CountOfIEnumerable<[Nullable(2)] T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("Parameter source is null while counting the IEnumerable");
			}
			int num = 0;
			foreach (T t in source)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000C784 File Offset: 0x0000A984
		internal static string GetHtmlForEncapsulation(HtmlNode node, ReturnType returnType)
		{
			switch (returnType)
			{
			case ReturnType.InnerText:
				return node.InnerText;
			case ReturnType.InnerHtml:
				return node.InnerHtml;
			case ReturnType.OuterHtml:
				return node.OuterHtml;
			default:
				throw new InvalidNodeReturnTypeException(string.Format("Invalid ReturnType value {0}", returnType));
			}
		}

		// Token: 0x02000042 RID: 66
		// (Invoke) Token: 0x060002F6 RID: 758
		[NullableContext(0)]
		internal delegate TResult HAPFunc<[Nullable(2)] T, [Nullable(2)] TResult>(T arg);
	}
}
