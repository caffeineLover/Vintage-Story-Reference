namespace Vintagestory.API.Server;

/// <summary>
/// The current connection state of a player thats currently connecting to the server
/// </summary>
public enum EnumClientState
{
	Offline,
	/// <summary>
	/// Is a newly opened connection, subject to verfication
	/// </summary>
	Connecting,
	/// <summary>
	/// Has passed the MaxClients Test
	/// </summary>
	Admitted,
	/// <summary>
	/// The players client sent us a packet that it loaded
	/// </summary>
	Connected,
	/// <summary>
	/// The players client sent us a packet that this player is now actually playing (after character selection)
	/// </summary>
	Playing,
	/// <summary>
	/// Is in the connection queue without initialized player data
	/// </summary>
	Queued
}
