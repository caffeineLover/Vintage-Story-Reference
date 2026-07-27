# Vintage Story Mod Coding Standards

These standards apply to all code written or modified for the user's Vintage Story mods.

The purpose of this document is to make code easy to understand, maintain, review, and extend. Coding agents must treat these rules as mandatory unless the user explicitly overrides one of them for a specific task.

---

# 1. General Commenting Standard

Code must contain copious, descriptive comments at three distinct levels:

1. File-level comments
2. Function-level comments
3. Internal block-level comments

Comments must explain purpose, responsibility, intent, lifecycle, relationships, assumptions, and non-obvious reasoning.
Comments will have a row length of 120 characters.  If a word goes beyond 120 characters, put a line break and continue
the comment on the next line.

Comments must not merely restate names, parameter lists, return types, or visible syntax.  No "fluff".

Good comments answer questions such as:

- Why does this file, function, or block exist?
- What larger feature or system does it support?
- Who calls it?
- What important methods, services, or APIs does it call?
- What state does it read or modify?
- What side effects does it produce?
- Why must operations occur in this order?
- What game rule, mod rule, or engine constraint is being enforced?
- What would be difficult to infer from the code alone?

Comments must remain accurate when the implementation changes. An outdated comment must be updated or removed during the same change that makes it inaccurate.

Comments are required, but they must not be used to excuse poor structure, misleading names, excessive nesting, hidden side effects, or functions that perform too many unrelated tasks.

---

# 2. File-Level Comments

Every source file must begin with a high-level block comment using `/*` and `*/`.

The file-level block comment must appear before:

- `using` directives
- namespace declarations
- class, struct, interface, enum, or record declarations
- all other code

The comment must describe the file from a high level.

Useful information may include:

- The responsibility of the file
- The feature or subsystem to which it belongs
- The role it plays within the mod
- Important classes, services, or Vintage Story systems with which it interacts
- Important lifecycle details
- Important ownership or persistence rules
- Important design decisions that affect the entire file
- Responsibilities deliberately excluded from the file

The comment must not merely repeat the filename or type name.

## Correct

```csharp
/*
 * Implements the server-side detection system used by the Clockmaker's
 * Gear Sense ability.
 *
 * This service periodically examines eligible players' surroundings,
 * identifies nearby dropped gears, and determines whether each player is
 * facing closely enough toward the nearest gear to receive a sensory cue.
 *
 * The main ModSystem creates this service and registers its tick callback
 * during server startup. This file does not award Temporal Insight, modify
 * item entities, or persist character progression.
 */

using Vintagestory.API.Common;
```

## Incorrect

```csharp
/*
 * GearSenseService.cs
 *
 * Contains GearSenseService.
 */
```

The incorrect example communicates nothing that is not already obvious from the filename and class declaration.

---

# 3. Function-Level Comments

Every named function, method, constructor, local function, operator overload, event handler, callback, and other callable member must be preceded by a descriptive function-level comment block.

Every line in the function-level comment block must begin with exactly four forward slashes:

```csharp
////
```

A blank function-comment line must also use exactly four forward slashes.

The function-level comment block must always end with one blank `////` line immediately before the function declaration.

There must not be a completely blank line between the final `////` line and the function declaration.

## Required Pattern

```csharp
//// Describes the function's responsibility and architectural context.
////
function declaration
```

## Correct

```csharp
//// Searches for the nearest dropped gear within the player's current
//// Gear Sense detection radius.
////
private EntityItem? FindNearestGear(IServerPlayer player)
{
}
```

## Correct Multi-Paragraph Example

```csharp
//// Searches the area surrounding a player for dropped item entities that
//// represent gears and selects the closest qualifying entity.
////
//// The Gear Sense polling routine calls this method after confirming that
//// the player is eligible to use the ability.
////
//// Placed blocks and items stored inside inventories are intentionally
//// excluded from the search. The calling method remains responsible for
//// checking the player's facing direction and producing sensory feedback.
////
private EntityItem? FindNearestGear(
    IServerPlayer player,
    double radius)
{
}
```

## Incorrect: Missing Final Blank Comment Line

```csharp
//// Searches for the nearest dropped gear within the player's current
//// Gear Sense detection radius.
private EntityItem? FindNearestGear(IServerPlayer player)
{
}
```

## Incorrect: Completely Blank Line Before the Declaration

```csharp
//// Searches for the nearest dropped gear within the player's current
//// Gear Sense detection radius.
////

private EntityItem? FindNearestGear(IServerPlayer player)
{
}
```

## Function-Level Comment Content

A useful function-level comment may explain:

- What work the function performs
- Why the function exists
- What larger operation or feature it supports
- Who or what normally calls it
- When it is called during the mod or game lifecycle
- Which important functions, services, or APIs it calls
- What state it reads or modifies
- What side effects it produces
- What assumptions it relies upon
- What its return value means to the caller
- What it deliberately does not do
- Why an unusual implementation or ordering is necessary
- Whether it runs frequently or on a performance-sensitive path

Not every comment must mention every item. It must include the information that helps a reader understand the function's role and behavior.

## Inadequate Function Comment

```csharp
//// Finds the nearest gear.
////
private EntityItem? FindNearestGear(
    IServerPlayer player,
    double radius)
{
}
```

This comment merely repeats the function name.

## Better Function Comment

```csharp
//// Searches the area around an eligible player for dropped gear items and
//// returns the closest qualifying entity.
////
//// The server-side Gear Sense polling routine calls this method after class
//// and progression checks have already passed. The method searches only
//// dropped EntityItem instances; placed blocks and inventory contents must
//// not activate Gear Sense.
////
//// This method does not determine whether the player is facing the gear and
//// does not produce any feedback. Those responsibilities remain with the
//// caller.
////
private EntityItem? FindNearestGear(
    IServerPlayer player,
    double radius)
{
}
```

## Constructors

Constructor comments must explain initialization responsibilities, dependencies, ownership, and lifecycle when relevant.

They must not merely say that the constructor creates the object.

```csharp
//// Creates the Gear Sense service and records the dependencies needed for
//// player searches, progression lookups, and sensory feedback.
////
//// Construction does not register the tick listener. The owning ModSystem
//// performs registration during server startup so that listener ownership
//// and disposal remain centralized.
////
public GearSenseService(
    ICoreServerAPI serverApi,
    IClockmakerProgressionService progressionService)
{
}
```

## Event Handlers and Callbacks

Comments for event handlers and callbacks should identify:

- What invokes them
- When they run
- Their lifecycle role
- Whether they run frequently
- If they're expected to be expensive.
- Why inexpensive checks occur before expensive work

```csharp
//// Performs one server-side Gear Sense update for all online players.
////
//// Vintage Story invokes this callback through the tick listener registered
//// during server startup. Because it runs repeatedly for the lifetime of
//// the world, inexpensive eligibility checks occur before nearby-entity
//// searches.
////
private void OnGearSenseTick(float deltaTime)
{
}
```

---

# 4. Internal Block-Level Comments

Comments inside functions must use ordinary `//` comments.

Internal comments must describe meaningful stages, branches, calculations, game rules, or implementation decisions.

They should explain:

- The purpose of a phase of the function
- Why a branch or early return exists
- Why operations occur in a particular order
- The game rule or mod rule being enforced
- Why a particular API call or filtering rule is necessary
- What invariant is being established
- What a loop is trying to find, calculate, validate, or accumulate
- Why a fallback or error path is required
- Non-obvious mathematical, geometric, temporal, or domain-specific reasoning

A block-level comment should normally describe several related statements. It should not narrate every individual line.

## Correct

```csharp
//// Performs one server-side Gear Sense update for every eligible online
//// player.
////
//// Vintage Story invokes this method through the tick listener registered
//// during server startup. Inexpensive eligibility checks occur before
//// nearby-entity searches because this method runs repeatedly.
////
private void OnGearSenseTick(float deltaTime)
{
    // Process each player independently so that an ineligible player or a
    // player with no nearby gears does not interrupt checks for anyone else.
    foreach (IServerPlayer player in _serverApi.World.AllOnlinePlayers)
    {
        // Reject non-Clockmakers before performing the more expensive nearby-entity search.
        if (!IsClockmaker(player))
        {
            continue;
        }

        // Calculate the detection radius from the player's current
        // Attunement level rather than using one global range for everyone.
        double detectionRadius = GetDetectionRadius(player);

        EntityItem? nearestGear = FindNearestGear(
            player,
            detectionRadius);

        // A missing result is a normal gameplay condition, not an error.
        // No feedback should be produced until a qualifying gear is nearby.
        if (nearestGear is null)
        {
            continue;
        }

        ProvideGearSenseFeedback(player, nearestGear);
    }
}
```

## Incorrect

```csharp
// Get the player position.
Vec3d playerPosition = player.Entity.Pos.XYZ;

// Get entities.
Entity[] entities = _serverApi.World.GetEntitiesAround(
    playerPosition,
    radius,
    radius);

// Loop through entities.
foreach (Entity entity in entities)
{
}
```

These comments merely translate the visible syntax into English.

## Correct Multi-Paragraph Internal Comment

```csharp
// Search from the player's authoritative server position. Gear Sense is
// server-owned gameplay logic, so the rendered client position must not
// determine whether an item is within range.
//
// Restrict the results to dropped EntityItem instances. Placed gear-related
// blocks and gears stored in inventories must not trigger this ability.
Entity[] nearbyEntities = _serverApi.World.GetEntitiesAround(
    playerPosition,
    radius,
    radius);
```

Internal comments must not use the file-level `/* ... */` style or the function-level `////` style.

---

# 5. Required Blank-Line Spacing Between Functions

There must be exactly three completely blank lines between functions.

The three blank lines occur after the closing brace of one function and before the `////` comment block for the next function.

## Correct

```csharp
//// Initializes the server-side services used by the mod.
////
public override void StartServerSide(ICoreServerAPI api)
{
    _serverApi = api;
}



//// Releases resources registered during server startup.
////
public override void Dispose()
{
    _tickListener?.Dispose();
}
```

The `////` comment block is considered part of the function it documents. The three blank lines therefore belong before the first `////` line, not between the comment and declaration.

---

# 6. Spacing Before the First Function

The first function in a file or type must also be preceded by exactly three completely blank lines.

In ordinary C#, functions are contained within a class, struct, record, or similar type. Therefore, the three blank lines normally appear after the type's opening brace or after the last field, property, or other member that precedes the first function.

## File-Scoped Namespace Example

```csharp
/*
 * Provides a small example service that demonstrates the required commenting
 * and spacing conventions for Vintage Story mod code.
 */

namespace ExampleMod;

public sealed class ExampleService
{



    //// Creates the service and establishes its initial state.
    ////
    public ExampleService()
    {
    }
}
```

## Block-Scoped Namespace Example

```csharp
/*
 * Provides a small example service that demonstrates the required commenting
 * and spacing conventions for Vintage Story mod code.
 */

namespace ExampleMod
{
    public sealed class ExampleService
    {



        //// Creates the service and establishes its initial state.
        ////
        public ExampleService()
        {
        }
    }
}
```

## Example with Fields Before the First Function

```csharp
public sealed class GearSenseService
{
    private readonly ICoreServerAPI _serverApi;
    private readonly IClockmakerProgressionService _progressionService;



    //// Creates the Gear Sense service and records the dependencies used by
    //// later detection checks.
    ////
    public GearSenseService(
        ICoreServerAPI serverApi,
        IClockmakerProgressionService progressionService)
    {
        _serverApi = serverApi;
        _progressionService = progressionService;
    }
}
```

---

# 7. Spacing After the Final Function

Three blank lines are required after the final function when the containing type's closing brace follows immediately.

```csharp
//// Releases resources owned by this service.
////
public void Dispose()
{
    _tickListener?.Dispose();
}



}
```

When another member follows a function, preserve a clear three-blank-line separation before that member when practical, especially when the next member is another function.

---

# 8. Comments Must Describe Relationships

Comments should identify architectural relationships when those relationships help explain the code.

Examples include:

- A ModSystem creates or registers a service
- A tick listener invokes a callback
- A command handler delegates to a progression service
- A Harmony patch intercepts a game method
- A network handler validates a client request before changing server state
- A calculation method is shared by gameplay logic and a user interface
- One service deliberately avoids persistence because another service owns saved state
- A server-side method uses authoritative state rather than client-rendered state
- A function calls a Vintage Story API whose behavior or limitations are not obvious

These relationships are especially important when the caller, callee, lifecycle, or ownership cannot be inferred easily from the function body.

---

# 9. Comments Must Explain Meaning, Not Syntax

Prefer comments that answer:

- Why is this code here?
- What responsibility does this block have?
- What game behavior is being implemented?
- Why is this condition necessary?
- Why is this API used instead of another API?
- Why must these operations occur in this order?
- What assumptions does this code make?
- What would break if this code were removed or changed?
- How does this code fit into the larger feature?

Avoid comments that answer only:

- What is the function called?
- What is the variable called?
- What type is the parameter?
- What does this assignment visibly do?
- What does the following `if`, `foreach`, or `return` statement literally say?

---

# 10. Comment Accuracy

Whenever code behavior changes, all related comments must be reviewed and updated during the same change.

This includes:

- File-level comments
- Function-level comments
- Internal block comments
- Comments describing callers or downstream calls
- Comments describing ownership or lifecycle
- Comments describing game-version-specific behavior
- Comments describing constants, formulas, thresholds, and timing
- Comments describing persistence, networking, or server/client responsibility

A coding agent must not preserve an existing comment merely because it already exists.

---

# 11. Comment Density

The amount of commentary depends on code complexity, but these minimum rules always apply:

- Every source file has a high-level `/* ... */` block comment
- Every callable member has a descriptive `////` comment block
- Every function-level comment ends with a blank `////` line
- Every meaningful phase inside a function has `//` comments
- Every non-obvious branch, formula, workaround, API limitation, or ordering dependency is explained
- Complex loops explain what they search for, calculate, validate, or enforce
- Early returns are explained when their purpose is not immediately obvious
- Game-engine lifecycle methods explain when and by whom they are invoked
- Performance-sensitive callbacks explain why cheap checks precede expensive work
- Exactly three blank lines separate function definitions

Very small functions still require a function-level comment, although they may not need internal block comments when the entire body performs one obvious operation.

---

# 12. Complete Example

```csharp
/*
 * Implements a server-side service that detects nearby dropped gears for
 * players who possess the Clockmaker's Gear Sense ability.
 *
 * The owning ModSystem constructs this service during server startup and
 * registers its periodic callback with the Vintage Story server API. This
 * class performs detection and feedback selection only; progression and
 * persistent character data are owned by separate services.
 */

using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ClockmakerAscendant.Services;

public sealed class GearSenseService
{
    private readonly ICoreServerAPI _serverApi;
    private readonly IClockmakerProgressionService _progressionService;



    //// Creates the Gear Sense service and records the dependencies required
    //// for server-side entity searches and progression lookups.
    ////
    //// Construction does not register the periodic callback. The owning
    //// ModSystem performs registration so that startup and disposal remain
    //// centralized.
    ////
    public GearSenseService(
        ICoreServerAPI serverApi,
        IClockmakerProgressionService progressionService)
    {
        _serverApi = serverApi;
        _progressionService = progressionService;
    }



    //// Performs one server-side Gear Sense update for every online player.
    ////
    //// Vintage Story invokes this method through a tick listener registered
    //// by the owning ModSystem. Cheap eligibility checks occur before entity
    //// searches because this method runs repeatedly.
    ////
    private void OnGearSenseTick(float deltaTime)
    {
        // Process each player independently so that one ineligible player or
        // one player without nearby gears does not interrupt anyone else.
        foreach (IServerPlayer player in _serverApi.World.AllOnlinePlayers)
        {
            // Reject players who cannot use Gear Sense before performing the
            // more expensive nearby-entity search.
            if (!_progressionService.CanUseGearSense(player))
            {
                continue;
            }

            double radius = _progressionService.GetGearSenseRadius(player);
            EntityItem? nearestGear = FindNearestGear(player, radius);

            // Finding no qualifying gear is an ordinary gameplay condition.
            // The service should remain silent rather than treating it as an
            // error or producing misleading feedback.
            if (nearestGear is null)
            {
                continue;
            }

            ProvideFeedback(player, nearestGear);
        }
    }



    //// Searches around the player's authoritative server position for the
    //// nearest dropped item entity that represents a gear.
    ////
    //// The periodic Gear Sense callback calls this method only after player
    //// eligibility has already been established. Placed blocks and items
    //// stored in inventories are intentionally excluded.
    ////
    //// This method identifies a target only. The caller remains responsible
    //// for determining whether and how feedback should be presented.
    ////
    private EntityItem? FindNearestGear(
        IServerPlayer player,
        double radius)
    {
        Vec3d playerPosition = player.Entity.Pos.XYZ;

        // Search from the authoritative server position because Gear Sense is
        // server-owned gameplay logic and must not depend on client rendering
        // or interpolation.
        Entity[] nearbyEntities = _serverApi.World.GetEntitiesAround(
            playerPosition,
            radius,
            radius);

        EntityItem? nearestGear = null;
        double nearestDistanceSquared = double.MaxValue;

        // Restrict the search to dropped gear items, then retain the closest
        // qualifying result without allocating an intermediate collection.
        foreach (Entity nearbyEntity in nearbyEntities)
        {
            if (nearbyEntity is not EntityItem itemEntity)
            {
                continue;
            }

            if (!IsGearItem(itemEntity.Itemstack))
            {
                continue;
            }

            double distanceSquared =
                itemEntity.Pos.XYZ.SquareDistanceTo(playerPosition);

            if (distanceSquared >= nearestDistanceSquared)
            {
                continue;
            }

            nearestGear = itemEntity;
            nearestDistanceSquared = distanceSquared;
        }

        return nearestGear;
    }
}
```

---

# 13. Review Checklist

Before considering a source file complete, verify all of the following:

- The file begins with a high-level `/* ... */` block comment
- The file comment explains responsibility rather than repeating the filename
- Every callable member has a meaningful `////` comment block
- Every function-level comment ends with a blank `////` line
- No completely blank line appears between that final `////` line and the declaration
- Function comments describe behavior and architectural context
- Important callers, lifecycle triggers, downstream calls, side effects, or limitations are identified when useful
- Meaningful phases inside functions have `//` comments
- Comments explain intent and reasoning rather than visible syntax
- Exactly three completely blank lines separate functions
- The first function is preceded by exactly three completely blank lines
- No formatter has collapsed the required blank-line spacing
- No comment refers to behavior the code no longer implements
- The comments are detailed enough that a future maintainer can understand the design without reconstructing it from scratch
