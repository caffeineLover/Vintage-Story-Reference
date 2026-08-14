using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace YamlDotNet.Serialization.TypeInspectors
{
	// Token: 0x0200005B RID: 91
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ReflectionTypeInspector : TypeInspectorSkeleton
	{
		// Token: 0x0600024D RID: 589 RVA: 0x00007DB0 File Offset: 0x00005FB0
		public override string GetEnumName(Type enumType, string name)
		{
			foreach (MemberInfo memberInfo in enumType.GetMembers())
			{
				EnumMemberAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<EnumMemberAttribute>(memberInfo);
				if (((customAttribute != null) ? customAttribute.Value : null) == name)
				{
					return memberInfo.Name;
				}
			}
			return name;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00007DFC File Offset: 0x00005FFC
		public override string GetEnumValue(object enumValue)
		{
			if (enumValue == null)
			{
				return string.Empty;
			}
			string text = enumValue.ToString();
			Type type = enumValue.GetType();
			MemberInfo[] member = type.GetMember(text);
			if (member.Length != 0)
			{
				EnumMemberAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<EnumMemberAttribute>(member[0]);
				if (customAttribute != null && customAttribute.Value != null)
				{
					text = customAttribute.Value;
				}
			}
			return text;
		}
	}
}
