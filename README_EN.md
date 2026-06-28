# Sts2Rebalance

**Language:** [中文](./README.md) | **English**

`Sts2Rebalance` is a balance mod for Slay the Spire 2.  
Its main goal is to revert the changes from the official version that prevented players from using the “small loop” strategy, while also adjusting the unreasonable design of some 3-cost colorless cards, in line with STS 1.

## Features

- Rebalances vanilla ascension effects
- Reworks or adjusts vanilla cards
- Keeps encyclopedia cards, rendered card text, upgrade previews, and combat behavior aligned
- Maintains a documented change list for testing and release review

## Current Changes

The current version includes:

- A6 no longer increases card removal scaling; it now raises shop prices instead
- The first batch of card changes across Ironclad, Silent, Necrobinder, Regent, Defect, and colorless cards
- Map generation investigation notes for future mechanic changes

See [`docs/rebalance-changes.md`](docs/rebalance-changes.md) for the full list.

## Dependencies

- BaseLib, Workshop ID: `3737335127`
- RitsuLib, Workshop ID: `3747602295`

When using the Steam Workshop version, subscribe to and enable both dependencies first.

## Download

- Download the package from the GitHub Releases page
- A release package usually contains `Sts2Rebalance.dll`, `Sts2Rebalance.json`, and `Sts2Rebalance.pck`

## Installation

1. Download a release package, or build the mod locally
2. Create or open the game's `mods` folder
3. Put the release files under:

```text
...\Steam\steamapps\common\Slay the Spire 2\mods\Sts2Rebalance
```

4. Start the game and confirm that `Sts2Rebalance` and its dependencies are enabled in the mod list

## Notes

- This mod changes vanilla content and may conflict with other mods that patch the same cards, ascension effects, shops, or reward logic
- Game updates may change vanilla card models, method names, or localization formatting, so each game update should be revalidated
- If encyclopedia rendering, upgrade previews, and combat behavior disagree, please report the card name, language, upgrade state, and reproduction steps

## Feedback

Useful bug reports include:

- The affected card, relic, enemy, room, or ascension level
- Whether other mods were enabled
- Reproduction steps
- Game logs

Logs are usually stored at:

```text
C:\Users\YourUserName\AppData\Roaming\SlayTheSpire2\logs
```

## Developer Notes

- Change list: [`docs/rebalance-changes.md`](docs/rebalance-changes.md)
- Current ascension reference: [`docs/current-ascensions.md`](docs/current-ascensions.md)
- Map generation investigation: [`docs/map-generation-investigation.md`](docs/map-generation-investigation.md)

## Local Build

This project targets `net9.0` and `Godot.NET.Sdk/4.5.1`.

Local machine paths are configured in `Directory.Build.props`, which is intentionally ignored by Git. Create your own local copy with:

```xml
<Project>
  <PropertyGroup>
    <GodotPath>E:/megadot/Godot_v4.5.1-stable_mono_win64/Godot_v4.5.1-stable_mono_win64.exe</GodotPath>
    <Sts2Path>D:/Sponsored/Steam/steamapps/common/Slay the Spire 2</Sts2Path>
    <ModsPath>D:/Sponsored/Steam/steamapps/common/Slay the Spire 2/mods/</ModsPath>
  </PropertyGroup>
</Project>
```

Build and publish to the local `mods` folder:

```powershell
dotnet build Sts2Rebalance.sln -m:1 /nr:false
dotnet publish Sts2Rebalance.sln -m:1 /nr:false
```
