using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Vintagestory.Client.NoObf;

public delegate void TrackStarterLoadedDelegate(MusicTrack track, EnumSoundType soundType, Action<ILoadedSound> onLoaded = null, bool playnow = true);
