---
title: Config Lib Integration Guide
subtitle: Config Lib 1.12.0 for Vintage Story 1.22.6
date: 2026-08-15
---

This document explains how a Vintage Story mod can support Config Lib while continuing to work when Config Lib is
not installed. It covers content-mod asset patches, ordinary code-mod JSON configuration, live setting updates, and
the public runtime API visible in Config Lib `1.12.0`.

The findings apply to the Config Lib `1.12.0` reference retained for Vintage Story `1.22.6`. That Config Lib release
was published for the Vintage Story `1.22.x` line through `1.22.1`, so the repository classifies it as a same-line
fallback rather than an exact `1.22.6` release. Recheck Config Lib when targeting another game or Config Lib version.

## Short Answer

Yes, a mod can implement Config Lib without making it a mandatory runtime dependency.

The recommended design for an ordinary code mod is:

1. Keep a normal configuration class and load it with Vintage Story's `LoadModConfig<T>()` API.
2. Supply sensible defaults that work with no Config Lib installation.
3. Add `assets/<modid>/config/configlib-patches.json` to describe the Config Lib controls.
4. Set `file` in that asset to the same JSON filename loaded by the mod.
5. If settings must take effect immediately, test `IsModEnabled("configlib")` before resolving
   `ConfigLibModSystem`, then subscribe to its configuration events.
6. Keep the defaults in the C# class, assets, and Config Lib definition identical.

Config Lib's upstream JSON-API documentation explicitly treats it as optional. If it is absent, its configuration
files and asset patches are not applied; the consuming mod's ordinary JSON file or asset defaults remain in effect.

## Choose An Integration Model

| Model | Saved File | Best For | C# API Needed |
| --- | --- | --- | --- |
| Generated YAML | Domain YAML file | Content mods and asset patching | No |
| JSON-backed asset | Named JSON file | Existing code-mod config classes | Only for live callbacks |
| Managed registration | JSON path from code | Code-first configuration | Yes |

The first two models are discovered from `configlib-patches.json`. The third is registered through Config Lib's
public runtime API and must be registered early enough that Config Lib has not already sent its configuration
registry to clients.

## The Required Asset Location

Config Lib `1.12.0` scans the Vintage Story `config` asset category and selects assets whose filename is exactly:

```text
configlib-patches.json
```

Place the file at:

```text
assets/<modid>/config/configlib-patches.json
```

The asset domain becomes the Config Lib configuration domain. For a mod id of `examplemod`, the configuration is
therefore retrieved with `GetConfig("examplemod")`, and Config Lib events report `examplemod` as their domain.

The filename is significant. A differently named JSON file in the same directory is not discovered by the loader.

## Recommended Code-Mod Implementation

### Keep A Normal Fallback Config

Config Lib should enhance the mod's configuration experience rather than own the only viable configuration path. A
minimal ordinary config class can look like this:

```csharp
public sealed class ModConfig
{
    public bool Enabled { get; set; } = true;

    public int SearchRadius { get; set; } = 5;

    public string DisplayMode { get; set; } = "Detailed";
}
```

Load and store it through the normal Vintage Story API before attempting Config Lib integration:

```csharp
private const string ConfigFileName = "ExampleMod.json";

public ModConfig Config { get; private set; } = new();

public override void Start(ICoreAPI api)
{
    base.Start(api);

    try
    {
        Config = api.LoadModConfig<ModConfig>(ConfigFileName) ?? new ModConfig();
        api.StoreModConfig(Config, ConfigFileName);
    }
    catch (Exception exception)
    {
        api.Logger.Error("Unable to load ExampleMod configuration: {0}", exception);
        Config = new ModConfig();
    }

    AttachOptionalConfigLib(api);
}
```

This fallback is the behavior that protects the mod when Config Lib is missing, disabled, or unable to parse its
definition. The exact exception policy is project-specific, but the mod should always retain a usable configuration
object.

### Describe The Same JSON File To Config Lib

The corresponding `configlib-patches.json` can use Config Lib `1.12.0`'s array form:

```json
{
  "version": 1,
  "file": "ExampleMod.json",
  "settings": [
    {
      "code": "Enabled",
      "type": "boolean",
      "ingui": "examplemod:setting-enabled",
      "comment": "examplemod:setting-enabled-description",
      "default": true,
      "weight": 1
    },
    {
      "code": "SearchRadius",
      "type": "integer",
      "ingui": "examplemod:setting-search-radius",
      "comment": "examplemod:setting-search-radius-description",
      "default": 5,
      "range": {
        "min": 1,
        "max": 20,
        "step": 1
      },
      "weight": 2
    },
    {
      "code": "DisplayMode",
      "type": "string",
      "ingui": "examplemod:setting-display-mode",
      "default": "Detailed",
      "values": [
        "Compact",
        "Detailed"
      ],
      "weight": 3
    }
  ]
}
```

When `file` is present, Config Lib treats the target as JSON instead of generating its default
`ModConfig/<domain>.yaml` file. The `code` values act as paths into that JSON document. Simple top-level property
names therefore match ordinary C# property names naturally.

Config Lib's JSON reader also uses its path implementation for nested values. Test nested paths with the target
version rather than assuming that every JSONPath feature is available.

### Apply Initial And Live Values

Code that names Config Lib types needs a compile-time reference to Config Lib, but the runtime path can still be
optional. The retained Betterer Prospecting example first checks `IsModEnabled("configlib")` and only then resolves
`ConfigLibModSystem` and subscribes to events.

A reusable pattern is:

```csharp
private void AttachOptionalConfigLib(ICoreAPI api)
{
    if (!api.ModLoader.IsModEnabled("configlib"))
    {
        return;
    }

    ConfigLibModSystem provider =
        api.ModLoader.GetModSystem<ConfigLibModSystem>(true);

    provider.ConfigsLoaded += () =>
    {
        IConfig? config = provider.GetConfig(Mod.Info.ModID);
        config?.AssignSettingsValues(Config);
        ApplyConfigDependentState();
    };

    provider.SettingChanged += (domain, _, setting) =>
    {
        if (domain != Mod.Info.ModID)
        {
            return;
        }

        setting.AssignSettingValue(Config);
        ApplySettingChange(setting.YamlCode);
    };
}
```

`IConfig.AssignSettingsValues(object)` assigns all current settings to matching fields or properties on an object.
`ISetting.AssignSettingValue(object)` assigns one setting. These methods return or operate through Config Lib's
current parsed values, including values received from the server.

The callbacks that follow assignment are important when a setting affects cached data, Harmony patches, registered
tool modes, meshes, or other state that was constructed earlier. Changing the config object's property alone does
not rebuild such state automatically.

Use the nullable syntax and event-unsubscription pattern appropriate to the consuming project's target framework.
Store delegate instances when they must be removed during `Dispose()`; an inline lambda cannot be reliably removed
later by recreating a visually identical lambda.

## Setting Definition Reference

Config Lib `1.12.0` accepts an array of setting and formatting blocks or the older grouped-object form documented by
the upstream JSON API.

### Array Form

Each setting entry normally includes:

| Property | Purpose |
| --- | --- |
| `code` | Unique setting identifier and JSON/YAML path used for object assignment. |
| `type` | Value type such as `boolean`, `integer`, `float`, `number`, `string`, `color`, or `other`. |
| `default` | Reset value and fallback used when no saved value exists. |
| `weight` | Sort order in the GUI and generated configuration presentation. |
| `ingui` | Optional localization key for the GUI label. |
| `comment` | Optional literal help text or localization key. |
| `clientSide` | When `true`, keeps that setting client-side instead of treating it as server-controlled. |
| `range` | Numeric `min`, `max`, and optional `step`; a complete min/max range produces a slider. |
| `values` | Explicit allowed or suggested values for selection. |
| `mapping` | Maps display/config labels to underlying values. |
| `logarithmic` | Requests a logarithmic numeric slider. |

Config Lib's decompiled enum also contains internal `None` and `Constant` kinds. Constants are supplied separately
through the definition's `constants` object; they are useful to expressions but are not ordinary editable controls.

### Formatting Blocks

Formatting blocks can appear among settings in the array form. For example:

```json
{
  "type": "separator",
  "title": "examplemod:settings-search-title",
  "weight": 10,
  "collapsible": true
}
```

Separators can carry a `title` or explanatory `text`. Config Lib attempts to resolve those strings as localization
keys and otherwise displays them literally.

### Grouped Form

The indexed upstream JSON API documents settings grouped by type:

```json
{
  "version": 1,
  "settings": {
    "boolean": {
      "FEATURE_ENABLED": {
        "name": "feature-enabled",
        "ingui": "examplemod:setting-feature-enabled",
        "comment": "examplemod:setting-feature-enabled-description",
        "default": true,
        "weight": 1
      }
    },
    "integer": {
      "SEARCH_RADIUS": {
        "name": "search-radius",
        "default": 5,
        "weight": 2,
        "range": {
          "min": 1,
          "max": 20,
          "step": 1
        }
      }
    }
  }
}
```

Without `file`, this form generates `ModConfig/examplemod.yaml`. The uppercase keys are setting codes used by asset
patches, while `name` is the user-facing YAML key. The upstream documentation says YAML names should avoid spaces
and characters such as `:`; hyphens and underscores are suitable separators.

## Asset Patching For Content Mods

Config Lib can patch asset values without any consuming-mod C# code. Patches are grouped by the value type they
produce and then by asset location:

```json
{
  "version": 1,
  "settings": {
    "float": {
      "DAMAGE_MULTIPLIER": {
        "name": "damage-multiplier",
        "default": 1.0,
        "weight": 1,
        "range": {
          "min": 0.5,
          "max": 2.0,
          "step": 0.1
        }
      }
    }
  },
  "patches": {
    "float": {
      "examplemod:itemtypes/example-tool.json": {
        "behaviors/0/properties/damage": "value * DAMAGE_MULTIPLIER"
      }
    }
  }
}
```

Here, `value` means the asset's original value at the patched path. Config Lib supports setting references, arithmetic,
constants such as `pi` and `e`, and functions including trigonometric operations, absolute value, square root,
rounding, logarithms, min/max, clamp, and conditional comparison helpers.

Patches can target the consuming mod's assets, vanilla assets, JSON patch files, or another mod's assets. Cross-mod
patching is useful for server configuration add-ons, but the target paths and load-order assumptions must be tested
against the exact target versions.

### Conditional Patches

The JSON API also supports boolean expressions that select between values. A representative expression is:

```json
"some/path": "(FEATURE_ENABLED) ? NEW_VALUE : value"
```

This keeps the original asset value when the feature is disabled. Parenthesize nontrivial arithmetic and boolean
expressions clearly; compact expressions are easier to validate and diagnose from Config Lib logs.

## Runtime API In Config Lib 1.12.0

### `IConfigProvider`

`ConfigLibModSystem` implements `IConfigProvider`. Its relevant public surface includes:

- `Domains`, listing registered configuration domains;
- `GetConfig(domain)` and `GetSetting(domain, code)`;
- `ConfigsLoaded`, `SettingChanged`, `ConfigWindowOpened`, and `ConfigWindowClosed` events;
- `RegisterCustomConfig`, for a custom-drawn configuration window;
- `RegisterCustomManagedConfig`, for a configuration object managed by Config Lib.

`SettingChanged` supplies the domain, owning `IConfig`, and changed `ISetting`. Always filter the domain before
modifying a consuming mod's state.

### `IConfig`

The public interface exposes:

- `ConfigFilePath` and `Version`;
- `WriteToFile()`, `ReadFromFile()`, and `RestoreToDefaults()`;
- `GetSetting(code)`;
- the per-config `SettingChanged` event;
- `AssignSettingsValues(target)`.

Prefer this interface when no implementation-specific behavior is needed. Casting to Config Lib's concrete `Config`
class couples the mod more tightly to the retained version.

### `ISetting`

The public setting interface exposes the current `Value`, optional `MappingKey`, `DefaultValue`, `SettingType`,
`YamlCode`, validation metadata, and `AssignSettingValue(target)`.

The assignment helper looks for a matching field or property on the supplied object. Keep setting codes aligned with
the configuration object's actual member names, including capitalization and any historical misspellings that have
already become part of the saved-file contract.

### Managed Registration

Config Lib `1.12.0` also exposes:

```csharp
RegisterCustomManagedConfig(
    string domain,
    object configObject,
    string? path = null,
    Action? onSyncedFromServer = null,
    Action<string>? onSettingChanged = null,
    Action? onConfigSaved = null)
```

It derives a definition from the supplied object's fields and properties, uses `<domain>.json` when no path is
provided, assigns changed values back to the object, and offers callbacks for synchronization, individual changes,
and saves.

The implementation rejects registration after Config Lib has already serialized its registry for clients. Register
managed configurations during early mod startup, not from late gameplay events. This is an advanced alternative to
the asset definition; the asset-driven JSON file is easier to inspect, localize, and support without Config Lib code
references.

## Load Order And Synchronization

Config Lib `1.12.0` registers its configuration recipe registry during `Start()` and loads discovered
`configlib-patches.json` assets during `AssetsLoaded()`. After loading, it applies configured asset patches and raises
`ConfigsLoaded`.

The implementation distinguishes client-side settings from server-controlled settings:

- `clientSide: true` marks a setting as local to the client;
- server-controlled changes require the Vintage Story `controlserver` privilege;
- server changes are sent through Config Lib's network channel and broadcast to other clients;
- received settings are applied to the Config Lib configuration object before callbacks are raised.

Do not use `clientSide: true` for values that affect authoritative gameplay, world state, recipes, damage, or server
simulation. Conversely, purely visual or local convenience settings should not require server administration.

Config Lib also publishes event-bus names in `1.12.0`:

```text
configlib:<domain>:config-saved
configlib:<domain>:setting-changed
configlib:<domain>:setting-loaded
configlib:config-reload
```

The strongly typed provider events are normally simpler for a code mod that already references Config Lib. Event-bus
integration can reduce direct coupling but still requires exact payload handling for the target Config Lib version.

## Optional Dependency Rules

The indexed upstream JSON documentation says Config Lib does not need to be listed in `modinfo.json` for the
asset-driven integration. That is the cleanest option for content mods.

For code mods that call Config Lib types:

- compile against the intended Config Lib API;
- do not call or resolve its mod system until `IsModEnabled("configlib")` succeeds;
- isolate Config Lib-specific work in a guarded method;
- keep all essential configuration loading outside that method;
- verify a clean startup with Config Lib absent;
- verify startup with Config Lib present but a malformed definition;
- avoid packaging Config Lib's assembly as though it were part of the consuming mod.

Betterer Prospecting `3.4.5` demonstrates an optional `modinfo.json` contract: its dependency list names only the
game, while its guarded runtime path checks for Config Lib before resolving `ConfigLibModSystem`.

## Defaults, Versions, And Migration

The most important maintenance rule is to keep defaults consistent across all sources:

- C# property or field initializers;
- fallback asset values;
- `configlib-patches.json` setting defaults;
- any example configuration shipped in documentation.

Betterer Prospecting provides a concrete failure example. Its C# `ModConfig` initializes `BoreholeDmg` to `2`, while
its Config Lib manifest advertises `4`. The effective initial behavior can therefore depend on which file was created
and whether Config Lib participated.

The upstream YAML documentation uses `version` to regenerate an old generated configuration with new defaults and
instructs authors to increase it when defaults need to be regenerated. Treat that as a migration boundary, not as a
substitute for deliberate compatibility handling. In particular:

- do not bump the version merely because the mod itself received a release;
- preserve user choices where possible;
- test the behavior of `version` separately for YAML and `file`-backed JSON definitions;
- document intentional resets or renamed keys;
- never silently correct a misspelled setting key after users may already have it in saved files.

## Localization

For setting labels, Config Lib uses `ingui` when present and otherwise falls back to the setting name/code. Comments
can also be translation keys. If a key has no domain prefix, Config Lib attempts to use the consuming mod's asset
domain; explicit `<modid>:<key>` values are clearer in shared examples.

Add matching strings to the mod's language assets, for example:

```json
{
  "setting-enabled": "Enable feature",
  "setting-enabled-description": "Turns the example feature on or off.",
  "setting-search-radius": "Search radius",
  "setting-search-radius-description": "Maximum horizontal search distance."
}
```

Test both translated and missing-key behavior. Config Lib falls back to displaying literal text when a separator or
description cannot be resolved as a translation.

## Recommended Verification Matrix

Before release, test at least the following:

| Scenario | Expected Result |
| --- | --- |
| Config Lib absent | Mod loads; normal JSON/default configuration works; no Config Lib errors occur. |
| Config Lib present, first run | Config appears in the GUI and the expected JSON/YAML file is generated or updated. |
| Existing valid config | Saved values survive restart and appear correctly in the GUI. |
| Invalid or partial config | Mod falls back safely or reports a useful error without losing unrelated functionality. |
| Live setting edit | Matching config member changes and dependent runtime state refreshes. |
| Dedicated server edit | Only authorized server settings change; clients receive the synchronized values. |
| Client-only edit | Local visual/convenience behavior changes without modifying server-authoritative state. |
| Reset to defaults | C# defaults, asset defaults, and Config Lib defaults agree. |
| Config Lib removed after use | The mod still reads its ordinary config or uses sensible fallback defaults. |

For asset patches, also inspect the target asset after loading and test with every targeted mod/version combination.
An asset path that worked in one game patch or dependency release is not a permanent API.

## Known Limitations And Uncertainty

- The upstream JSON-API page indexed here was last known edited on `2024-05-14` and retrieved on `2026-08-10`.
  Config Lib `1.12.0` contains newer behavior, including array-form settings, JSON `file` backing, color settings, and
  managed object registration that is not fully explained on that page.
- The retained Config Lib release is a same-line fallback for Vintage Story `1.22.6`, not an exact patch-tag match.
- Decompiled source is behavioral evidence, not build-ready source and not a substitute for the upstream public
  package/API documentation.
- Config Lib's asset patch system is intentionally narrower than Vintage Story's complete JSON patch feature set.
- Optional direct C# linkage should be tested in the consuming mod's actual build and packaging arrangement. The
  Betterer Prospecting pattern demonstrates that it can work, but it does not prove every assembly layout is safe.
- The precise GUI capabilities depend on Config Lib and its VSImGui dependency being compatible with the client.

## Evidence And Source Paths

The research used the Vintage Story Reference `1.22.6` mod manifest, web index, asset index, and code index before
opening the following narrow source candidates.

### Indexed Upstream Documentation

- Config Lib JSON API:
  <https://github.com/maltiez2/vsmod_configlib/wiki/JSON-API>
- Indexed raw Markdown endpoint:
  <https://raw.githubusercontent.com/wiki/maltiez2/vsmod_configlib/JSON-API.md>
- Indexed retrieval time: `2026-08-10T03:39:34.076833+00:00`
- Last-known upstream edit recorded by the source manifest: `2024-05-14`

### Config Lib 1.12.0 Reference

All paths begin at `versions/1.22.6/mods/configlib_1.12.0/`.

- `modinfo.json` --- retained package identity, VSImGui dependency, and side metadata.
- `decompiled assembly/configlib/ConfigLib/ConfigLibModSystem.cs` --- asset discovery, lifecycle, networking, public
  provider API, managed registration, and event-bus constants.
- `decompiled assembly/configlib/ConfigLib/Config.cs` --- YAML/JSON backing, version parsing, setting assignment,
  formatting, constants, and file read/write behavior.
- `decompiled assembly/configlib/ConfigLib/ConfigPatches.cs` --- collection and application of asset patches.
- `decompiled assembly/configlib/ConfigLib/ConfigSetting.cs` --- setting parsing, validation, mapping, side metadata,
  and reflection-based assignment.
- `decompiled assembly/configlib/ConfigLib/IConfigProvider.cs` --- public provider contract.
- `decompiled assembly/configlib/ConfigLib/IConfig.cs` --- public configuration contract.
- `decompiled assembly/configlib/ConfigLib/ISetting.cs` --- public setting contract.
- `decompiled assembly/configlib/ConfigLib/ConfigSettingType.cs` --- setting type enum.

### Real Consumer: Betterer Prospecting 3.4.5

All paths begin at `versions/1.22.6/mods/bettererprospecting_3.4.5/`.

- `assets/bettererprospecting/config/configlib-patches.json` --- array-form settings, JSON `file` backing,
  separators, localization keys, ranges, client-side setting, and version `31`.
- `decompiled assembly/BetterErProspecting/BetterErProspecting/BetterErProspect.cs` --- normal config loading,
  optional Config Lib guard, provider events, object assignment, and runtime refresh behavior.
- `decompiled assembly/BetterErProspecting/BetterErProspecting/Config/ModConfig.cs` --- ordinary fallback config and
  the `BoreholeDmg` default mismatch.
- `modinfo.json` --- no hard Config Lib dependency declaration.

### Representative Index Queries

```powershell
python -B tools/reference-query.py --version 1.22.6 `
  mod --mod-id configlib --format json

python -B tools/reference-query.py --version 1.22.6 `
  web --source configlib-json-api --limit 100

python -B tools/reference-query.py --version 1.22.6 `
  asset --search configlib --limit 30

python -B tools/reference-query.py --version 1.22.6 `
  code --root "configlib_1.12.0/decompiled assembly" `
  --type IConfigProvider --limit 10

python -B tools/reference-query.py --version 1.22.6 `
  code --root "configlib_1.12.0/decompiled assembly" `
  --member AssignSettingValue --limit 10
```

## Implementation Checklist

- [ ] Normal config loading works without Config Lib.
- [ ] `configlib-patches.json` is under the exact domain/config path.
- [ ] `file` matches the code mod's exact JSON filename when JSON backing is intended.
- [ ] Setting codes match config member names or intended JSON paths.
- [ ] Defaults agree across C#, assets, Config Lib, and documentation.
- [ ] Server-authoritative and `clientSide` settings are classified correctly.
- [ ] Config Lib calls are guarded by `IsModEnabled("configlib")`.
- [ ] Initial values are assigned after Config Lib reports configs loaded.
- [ ] Live callbacks refresh any derived runtime state.
- [ ] Event handlers are removed during disposal when the mod system can be reloaded.
- [ ] Localization keys exist and missing-key fallbacks were inspected.
- [ ] Config Lib absent/present, client/server, restart, reset, and migration cases were tested.
