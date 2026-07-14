# Peter's Personal Notes

## Skills
- `Update Mods`: Checks all the mods; downloads updates; decompiles using dot net spy if necessary.  Won't delete  old
   mod folders, so I need to do any cleanup.

   Use $vintage-story-reference-mod-updater to update mods for version <version>.

- `Index Assets`: Indexes the assets folder to save AI credits and conserve memory, since the assets folder has too
   too many files and has too much information.

   Use $vintage-story-reference-asset-indexer to index the assets folder for version <version>.

- `Index Reference Code`: Indexes the three decompiled dlls: VintageStoryAPI, VintageStoryLib, and VSSurvivalMod to
   conserve AI credit and memory.

   Use $vintage-story-reference-code-indexer to index the decompiled reference code for version <version>.

## Function of the 3 Source Trees:
- `VintagestoryAPI`: The public-facing modding API assembly.  This is the main thing to reference when writing mods:
   interfaces, attributes, block/item/entity APIs, world access, networking abstractions, GUI APIs, etc.
- `VintagestoryLib`: The game/client/server implementation library.  It depends on VintagestoryAPI and contains
   engine-side/internal implementation code.  Useful for understanding behavior, but generally not the primary surface
   you should build mods against unless you have a specific reason.
- `VintagestoryLib`: The built-in survival gameplay mod.  It depends on VintagestoryAPI plus other built-in mods like
   VSCreativeMod and VSEssentials.  This is where survival systems are: gameplay mechanics, block/item behaviors,
   crafting/survival features, etc.  Best treated as reference/example code for how vanilla survival implements things.


