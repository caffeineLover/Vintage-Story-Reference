using Vintagestory.API.Common.Entities;

namespace Vintagestory.API.Common;

/// <summary>
/// Triggered when a server receives a gait update from a client-authoritative mount
/// </summary>
/// <param name="mountEntity"></param>
/// <param name="gaitCode"></param>
public delegate void MountGaitReceivedDelegate(Entity mountEntity, string gaitCode);
