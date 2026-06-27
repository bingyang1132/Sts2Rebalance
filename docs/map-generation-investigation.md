# Map Generation Investigation

Source: game DLL `d:/sponsored/steam/steamapps/common/Slay the Spire 2/data_sts2_windows_x86_64/sts2.dll`, decompiled around `MegaCrit.Sts2.Core.Map` and `MegaCrit.Sts2.Core.Models.ActModel`.

## Main Entry Points

- `ActModel.CreateMap(RunState runState, bool replaceTreasureWithElites)` creates a `StandardActMap`.
- `StandardActMap.CreateFor(RunState runState, bool replaceTreasureWithElites)` seeds map RNG as `act_{CurrentActIndex + 1}_map`.
- `StandardActMap(...)` does the full map pipeline:
  1. Determine map length from `actModel.GetNumberOfRooms(isMultiplayer) + 1`.
  2. Generate random connected paths on a 7-column grid.
  3. Assign point types.
  4. Optionally prune duplicate path segments and repair missing type counts.
  5. Center/spread/straighten the grid for presentation.

## Room Counts

`ActModel.GetNumberOfRooms(isMultiplayer)` starts from each act's `BaseNumberOfRooms`; multiplayer subtracts 1.

Known defaults:

| Act | Base rooms | Weak encounters | Rest count | Unknown count |
| --- | ---: | ---: | --- | --- |
| `Overgrowth` | 15 | 3 | Gaussian around 7, clamped 6-7 | `StandardRandomUnknownCount(rng)` |
| `Underdocks` | 15 | 3 | Gaussian around 7, clamped 6-7 | `StandardRandomUnknownCount(rng)` |
| `Hive` | 14 | 2 | Gaussian around 6, clamped 6-7 | `StandardRandomUnknownCount(rng) - 1` |
| `Glory` | 13 | 2 | random int 5-6 | `StandardRandomUnknownCount(rng) - 1` |

`MapPointTypeCounts.StandardRandomUnknownCount(rng)` uses `rng.NextGaussianInt(12, 1, 10, 14)`.

Other type counts:

- Shops: always 3.
- Elites: normally round(5); with A1 `SwarmingElites`, round(5 * 1.6) = 8.
- Rest and unknown counts are supplied by the act.

## Fixed Rows

`StandardActMap.AssignPointTypes()` pins several rows before random assignment:

- Row 1: `Monster`, not modifiable.
- Last pre-boss row: `RestSite`, not modifiable.
- Row `GetRowCount() - 7`: `Treasure`, not modifiable.
- If `replaceTreasureWithElites` is true, that treasure row becomes `Elite`.
- Boss and Ancient points are assigned outside the normal grid.

## Random Type Assignment

After fixed rows:

- A queue is filled with target counts for rest sites, shops, elites, and unknown rooms.
- The algorithm tries to assign those types to unassigned points.
- Any unassigned points become `Monster`.

Placement restrictions:

- `Elite` and `RestSite` cannot appear in the lower map before row 6.
- `RestSite` cannot appear in the final upper rows.
- `Elite`, `RestSite`, `Treasure`, and `Shop` cannot be directly adjacent along parent/child paths to the same type.
- Sibling restrictions apply to `RestSite`, `Monster`, `Unknown`, `Elite`, and `Shop`.
- A type can ignore placement rules if present in `MapPointTypeCounts.PointTypesThatIgnoreRules`.

## Path Shape

`StandardActMap.GenerateMap()`:

- Uses 7 iterations.
- Each iteration picks a random starting point in row 1 and grows a path upward one row at a time.
- Each next step may go left, straight, or right, clamped to columns 0-6.
- `HasInvalidCrossover(...)` prevents crossing diagonal edges.
- All final-row points connect to the boss point.
- The Ancient start point connects to all row-1 start points.
- A second boss point is appended if the act has `HasSecondBoss`.

## Post Processing

`MapPathPruning.PruneAndRepair(...)`:

- Finds duplicate path segments by point-type sequence.
- Removes redundant segments where safe.
- Repairs missing shop, elite, rest, and unknown counts by replacing modifiable monsters.
- Repeats up to 3 repair/prune passes.

`MapPostProcessing`:

- `CenterGrid(...)` shifts the map if two leftmost or rightmost columns are empty.
- `SpreadAdjacentMapPoints(...)` moves nodes within path constraints to increase spacing.
- `StraightenPaths(...)` nudges simple one-parent/one-child path points to reduce bends.

## Likely Mod Hooks For Next Phase

Conservative patch points:

- Patch `ActModel.GetMapPointTypes(Rng)` per act to change target counts.
- Patch `MapPointTypeCounts` constructor/result if a global count policy is desired.
- Patch `StandardActMap.AssignPointTypes()` or `IsValidPointType(...)` for placement rules.
- Patch after `StandardActMap` construction to transform `MapPoint.PointType` values while preserving graph connectivity.

Riskier patch points:

- `GenerateMap()` and `GenerateNextCoord(...)`, because they define path topology and crossover avoidance.
- `MapPathPruning`, because bad changes can produce disconnected maps or underfilled required room types.
