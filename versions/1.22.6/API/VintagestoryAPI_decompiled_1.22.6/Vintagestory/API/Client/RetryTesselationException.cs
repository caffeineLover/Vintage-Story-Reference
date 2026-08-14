using System;

namespace Vintagestory.API.Client;

/// <summary>
/// Used if the chunk will not be fully tesselated, but requeuing it is expected to succeed next time
/// <br />See TextureAtlasManager.RuntimeCreateNewAtlas() for an example: if creation of a new atlas is required, that must occur on the main thread before tesselation can proceed
/// </summary>
public class RetryTesselationException : Exception
{
	public RetryTesselationException(string message)
		: base(message)
	{
	}
}
