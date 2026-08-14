using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public delegate void FarmlandFastForwardUpdate(double hourIntervall, ClimateCondition conds, double lightGrowthSpeedFactor, bool growthPaused);
