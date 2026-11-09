namespace Vintagestory.API.Server;

public static class EnumClientStateExtensions
{
	public static bool ConnectedOrPlaying(this EnumClientState state)
	{
		if (state != EnumClientState.Connected)
		{
			return state == EnumClientState.Playing;
		}
		return true;
	}

	public static bool IsAdmitted(this EnumClientState state)
	{
		if (state != EnumClientState.Admitted && state != EnumClientState.Connected)
		{
			return state == EnumClientState.Playing;
		}
		return true;
	}
}
