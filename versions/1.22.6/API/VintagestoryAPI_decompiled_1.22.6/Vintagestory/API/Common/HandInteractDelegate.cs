using Vintagestory.API.Server;

namespace Vintagestory.API.Common;

public delegate void HandInteractDelegate(IServerPlayer player, EnumHandInteractNw enumHandInteract, float secondsPassed, ref EnumHandling handling);
