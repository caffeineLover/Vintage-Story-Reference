using System;

namespace Vintagestory.API.Util;

/// <summary>
/// Indicates that the FastSerialize() method should call a custom method to serialize this field: the custom method being of the pattern FastSerializerDelegate
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class CustomFastSerializerAttribute : Attribute
{
}
