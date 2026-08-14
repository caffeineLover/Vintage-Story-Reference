# Betterer Prospecting Settings Reference

This document describes selected Betterer Prospecting `3.4.5` settings as indexed for Vintage Story `1.22.6`.
It is based on the decompiled mod code and the bundled Config Lib setting manifest.

The setting key `ReprospectParellelism` is misspelled in the mod's code and assets. Keep that spelling when editing
configuration files.

## Short Answers

| Setting | Default | What It Controls |
| --- | --- | --- |
| `ReprospectParellelism` | `0` | The worker concurrency used by the `/btrpr reprospect` server command. |
| `BoreholeRadius` | `8` | The horizontal radius of the vertical Borehole scan cylinder. |
| `BoreholeScansStone` | `false` | Whether Borehole mode reports rock/stone blocks in addition to ore. |
| `BoreholeDmg` | See note below | The amount of prospector pick durability damage applied by a valid Borehole sample. |

`BoreholeDmg` has a mismatch between two bundled sources. The decompiled runtime config class initializes it to `2`,
while the Config Lib setting manifest advertises a default of `4` with a range of `1` to `10`. Existing saved config
values are authoritative at runtime.

## Borehole Mode Context

Betterer Prospecting replaces the vanilla prospector pick item class and registers additional tool modes. Borehole mode
is one of those modes when `AddBoreHoleMode` is enabled.

When a player breaks a block with the prospector pick, Betterer Prospecting asks the active mode how much durability
damage to apply. The returned value is passed to `DamageItem`. Borehole mode normally returns `BoreholeDmg`.

Borehole mode only accepts samples from the top face of the clicked block. For a valid top-face sample, it walks a
vertical cylinder centered on the clicked block, from the clicked Y level down to the bottom of the world. The cylinder
radius comes from `BoreholeRadius`; the Config Lib manifest defaults that radius to `8`, and `0` makes the scan a
straight vertical line.

## Matching `BoreholeRadius` To `ProximitySearchRadius = 5`

There is no single code-enforced optimum. If "optimal" means the smallest Borehole radius that preserves the same
horizontal reach as `ProximitySearchRadius = 5`, use `BoreholeRadius = 5`.

This is not the same as the bundled default. Betterer Prospecting ships `ProximitySearchRadius = 5`, but its Config
Lib manifest ships `BoreholeRadius = 8`. That default makes Borehole mode a broader survey tool than Proximity mode.

The reason is the scan shape. `ProximitySearchRadius` is used by `WalkBlocksSphere`, so a radius of `5` searches a
bounded 3D sphere around the clicked block. `BoreholeRadius` is used by `WalkBlocksCylinder`, so a radius of `5`
searches the same 5-block horizontal reach, but repeats that circular footprint from the clicked Y level down to the
bottom of the world.

Integer block positions scanned per horizontal Borehole layer:

| `BoreholeRadius` | Blocks Per Y Layer | Relative To Radius `5` |
| --- | ---: | ---: |
| `5` | `81` | `1.00x` |
| `8` | `197` | `2.43x` |
| `10` | `317` | `3.91x` |

Because Borehole mode already scans far more depth than Proximity mode, raising `BoreholeRadius` above `5` increases
coverage and scan cost quickly. Use `8` only when the intent is for Borehole samples to cover a wider column than
Proximity samples. Use `0` for a strict vertical line.

## `BoreholeScansStone`

`BoreholeScansStone` decides whether Borehole mode includes rock/stone block types in its results.

With the default `false` value, Borehole mode does not add stone results. If `BoreholeScansOre` is still enabled, the
scan reports only qualifying ore blocks found in the vertical cylinder.

When `BoreholeScansStone` is `true`, the scan also calls the mod's rock-detection helper and adds qualifying rock
types to the result list. Ore is checked first, so a block that qualifies as ore is handled by the ore path rather than
the stone path.

If both `BoreholeScansOre` and `BoreholeScansStone` are disabled, Borehole mode does not scan. It sends the
`borehole-no-filter` message and returns `1` durability damage instead of `BoreholeDmg`.

## `BoreholeDmg`

`BoreholeDmg` is the durability cost of a valid Borehole sample.

The Borehole code returns this value after it finishes the scan and sends the player either the found-results message
or the no-results message. That means a valid top-face Borehole sample consumes `BoreholeDmg` durability even when the
scan finds no matching ore or stone.

Two early rejection paths return `1` instead:

- both ore and stone scanning are disabled;
- the player tries to take the sample from a face other than the top face.

The bundled English text labels this setting as "Borehole Damage" and describes it as tool damage for Borehole mode.
The Config Lib manifest constrains it to `1` through `10`.

## `ReprospectParellelism`

`ReprospectParellelism` affects the admin reprospecting command, not ordinary prospecting pick use.

Betterer Prospecting registers `/btrpr reprospect` behind the `controlserver` privilege. The command recalculates
stored prospector-map readings and is described by the mod as an expensive operation. During that recalculation, the
mod uses `Parallel.ForEachAsync` over the stored readings for each player entry.

The setting is used as `ParallelOptions.MaxDegreeOfParallelism`:

- values greater than `0` are used directly as the maximum number of concurrent reprospecting workers;
- `0` falls back to `Environment.ProcessorCount / 4`.

Higher values can finish reprospecting faster, but they increase CPU load while the command is running. The bundled
English text says the same thing: higher numbers are faster but more CPU-intensive, and `0` uses one quarter of the
available cores.

The Config Lib manifest allows `0` through `100`. For normal servers, `0` is the conservative choice. Increase it only
for planned maintenance, preferably while few players are online.

## Notes And Quirks

- The spelling `Parellelism` appears in the actual key and UI translation. Do not silently correct it to
  `Parallelism` in config files.
- The decompiled `/btrpr reprospect` command accepts an optional player argument, but the visible `3.4.5` method body
  loops through the stored map-layer readings collection as a whole. Verify in-game before relying on the command for
  per-player-only reprospecting.
- The Config Lib manifest and the direct `ModConfig` class disagree on `BoreholeDmg`'s initial default. Check the saved
  runtime config when diagnosing a server's actual Borehole durability cost.

## Source Paths

All paths below are under `versions/1.22.6/mods/bettererprospecting_3.4.5`.

- `decompiled assembly/BetterErProspecting/BetterErProspecting/Config/ModConfig.cs`
- `decompiled assembly/BetterErProspecting/BetterErProspecting/Item/ItemBetterErProspectingPick.cs`
- `decompiled assembly/BetterErProspecting/BetterErProspecting/Prospecting/ProspectingSystem.cs`
- `assets/bettererprospecting/config/configlib-patches.json`
- `assets/bettererprospecting/lang/en.json`
