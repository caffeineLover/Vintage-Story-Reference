using System;

namespace Vintagestory.API.Common;

/// <summary>
/// A server/client side used by for the Vintage Story app. 
/// </summary>
[Flags]
[DocumentAsJson]
public enum EnumAppSide
{
	/// <summary>
	/// For server side things only.
	/// </summary>
	Server = 1,
	/// <summary>
	/// For client side things only.
	/// </summary>
	Client = 2,
	/// <summary>
	/// For server and client side things.
	/// </summary>
	Universal = 3
}
