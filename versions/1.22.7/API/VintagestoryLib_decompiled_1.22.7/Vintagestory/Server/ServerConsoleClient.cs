using System;

namespace Vintagestory.Server;

public class ServerConsoleClient : ConnectedClient
{
	public ServerPlayerData serverdata;

	public override ServerPlayerData ServerData => serverdata;

	public override bool IsPlayingClient => false;

	public ServerConsoleClient(int clientId)
		: base(clientId)
	{
	}

	public override string ToString()
	{
		return string.Format("Server Console", default(ReadOnlySpan<object>));
	}
}
