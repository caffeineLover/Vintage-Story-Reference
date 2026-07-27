# TentBag-Reworked Player Guide

TentBag-Reworked adds a craftable tent bag that can pack a small built structure into an item, then place it again
later. It is meant for portable camps, tents, and other compact travel builds.

This guide describes the default behavior in TentBag-Reworked `3.2.4` as indexed for Vintage Story `1.22.4`. Server
configuration can change the capture size, hunger cost, banned blocks, waypoint behavior, and some placement rules.

## Crafting

Craft an Empty Tent Bag in the crafting grid with:

- `1` cloth in the center
- `4` rope in the corners
- `4` sticks on the side-center slots

The recipe also works with the rope and stick positions swapped. The cloth color determines the bag color. Supported
colors are plain, black, blue, brown, gray, green, mordant, orange, pink, purple, red, white, and yellow.

The tent bag does not stack.

## Bundled Sample Tent

TentBag-Reworked also ships a sample tent schematic named `tent.json`. On server startup, the mod copies it to the game
data folder as `WorldEdit/tent.json` if that file does not already exist.

This is a WorldEdit schematic/template, not a pre-packed Tent Bag and not an extra survival recipe. Regular players do
not automatically receive it. In survival play, craft an Empty Tent Bag and pack a tent or camp you built yourself.

If you have admin, creative, or WorldEdit access, you can use the copied `tent.json` as a starter/example tent, then
pack that structure into a tent bag like any other build. The bundled schematic is `7 x 3 x 6` and contains a small
camp setup with blocks such as a hay bed, firepit, storage vessels, barrel, chest, trunk, tool rack, support beams,
and ground storage.

## Using The Starter Tent

You must be in creative mode and have the `worldedit` privilege. The file is loaded by filename from the game or
server data `WorldEdit` folder, so use `tent.json`, not an asset path.

For a direct server-side import:

1. Stand at the point where you want the bottom center of the tent.
2. Run `/we start`, then run `/we end` without moving. This gives WorldEdit both ends of a tiny selection and creates
   the start marker that import needs.
3. Optional: run `/we import-rotation 90` or `/we import-rotation 180` before importing.
4. Run `/we import tent.json BottomCenter`.
5. If it lands wrong, run `/we undo`.

`BottomCenter` centers the `7 x 3 x 6` schematic horizontally on your marker. Without it, WorldEdit uses the marker as
the schematic's start corner, which is harder to place by eye.

Use the capitalization `BottomCenter`; lower-case input falls back to the default corner placement.

For a preview workflow:

1. Select the import tool with `/we tool import` or `/we t import`.
2. Load the schematic with `/we ims tent.json`.
3. Use `/we imr 90` to rotate the preview, or `/we imflip` to flip it, if needed.
4. Move the preview with `/we north`, `/we south`, `/we east`, `/we west`, `/we up`, or `/we down`.
5. Right-click a block to place the preview, or run `/we apply`, `/we place`, or `/we commit`.

After the starter tent is pasted into the world, it is just normal blocks. To turn it into a Tent Bag, craft or spawn
an Empty Tent Bag, aim at the top face of the ground block under the center of the tent, and right-click to pack it.

## Packing A Tent

1. Build the camp or tent you want to carry.
2. Hold an Empty Tent Bag.
3. Aim at the top face of the ground block under the center of the build.
4. Right-click to pack the area.

By default, the packed area is a `7 x 7 x 7` box centered on the clicked block, starting above the clicked ground
block. The floor block itself is not packed unless the server enables floor grabbing.

If packing succeeds, the empty bag becomes a Packed Tent Bag and the original blocks are removed. In default
configuration, the packed bag stays in your active slot. If the server enables copy mode, the original blocks are
copied instead of removed.

Packing consumes satiety in survival mode. The default cost is `2` satiety per packed non-air block. If you do not
have enough saturation, the mod refuses to pack and shows: `I'm too hungry to pack this right now!`

## Placing The Tent

1. Hold a Packed Tent Bag.
2. Aim at the ground where you want the tent centered.
3. Use the preview and rotate keys if needed.
4. Right-click to unpack.

The packed build is placed above the block you click. The Packed Tent Bag changes back into an Empty Tent Bag after
unpacking.

Unpacking also consumes satiety in survival mode, using the same saved block count from when the tent was packed. If
you do not have enough saturation, the mod refuses to unpack and shows: `I'm too hungry to unpack this right now!`

By default, unpacking creates a map waypoint for the tent, and packing removes that waypoint.

## Hotkeys

The item tooltip shows the current keybindings. Defaults are:

- `B`: toggle the packed tent hologram preview
- `P`: toggle the empty bag capture-area preview
- `R`: rotate a packed tent by `90` degrees

The packed tent preview follows the block you are aiming at. If placement would fail, the mod highlights problem
blocks.

## What Cannot Be Packed

The default banned list prevents packing some world, resource, food, and utility blocks. Common examples include:

- water and lava
- crops, herbs, mushrooms, and berry bushes
- farmland, raw clay, peat, rocks, and ores
- grown logs and resin logs
- static translocators and teleporter bases
- paper lanterns and chandeliers
- anvils and forges
- cooked crocks, bowls of meal, and cooked clay pots

Servers can change this list or switch to an allow-list mode.

## Troubleshooting

`The tent can only be packed by clicking on the ground!`

You tried to pack by clicking the side or bottom of a block. Aim at the top face of the ground block under the build.

`There are no solid blocks in the build area!`

The capture box did not contain anything packable. Move the bag target closer to the build center or check the
server's configured capture size.

`Cannot pack ... into the bag!`

The capture area contains a banned block. Remove that block or ask the server owner whether the banned list is
intentional.

`Cannot unpack here, a clear area is required!`

Something blocks the placement area. Clear the space or move the target.

`Cannot unpack here, solid ground is required!`

The server requires solid ground under the tent, and the target area does not meet that rule.

`Failed to unpack: Internal error while reading saved data!`

The packed tent data could not be read. This usually means the item data is damaged or incompatible.

## Notes For Players On Servers

If TentBag-Reworked behaves differently from this guide, check with the server owner. The server can change:

- capture radius and height
- whether the floor is packed
- whether packing removes blocks or only copies them
- whether plants and loose rocks are replaced during placement
- whether the bag stays in your inventory or drops as an item after use
- waypoint creation
- blocked or allowed block lists
- satiety cost per packed block

## Reference Sources

This guide was written from the local indexed TentBag-Reworked `3.2.4` reference files.

TentBag-Reworked source files under `versions/1.22.4/mods/tentbagreworked_3.2.4/`:

- `assets/tentbagreworked/itemtypes/tentbagreworked.json`
- `assets/tentbagreworked/recipes/grid/tentbagreworked.json`
- `assets/tentbagreworked/config/tent.json`
- `assets/tentbagreworked/lang/en.json`
- `decompiled assembly/TentBagReworked/TentBagReworked/TentBagReworkedModSystem.cs`
- `decompiled assembly/TentBagReworked/TentBagReworked/Behaviors/PackableBehavior.cs`
- `decompiled assembly/TentBagReworked/TentBagReworked/TentHologramRenderer.cs`
- `decompiled assembly/TentBagReworked/TentBagReworked/Config/TentBagReworkedConfig.cs`
- `decompiled assembly/TentBagReworked/TentBagReworked/Util/TentArea.cs`

WorldEdit source files under `versions/1.22.4/API/`:

- `VSCreativeMod_decompiled_1.22.4/Vintagestory/ServerMods/WorldEdit/WorldEdit.cs`
- `VSCreativeMod_decompiled_1.22.4/Vintagestory/ServerMods/WorldEdit/WorldEditWorkspace.cs`
- `VSCreativeMod_decompiled_1.22.4/Vintagestory/ServerMods/WorldEdit/ImportTool.cs`
- `VintagestoryAPI_decompiled_1.22.4/Vintagestory/API/Common/BlockSchematic.cs`
- `VintagestoryAPI_decompiled_1.22.4/Vintagestory/API/Common/EnumOrigin.cs`
