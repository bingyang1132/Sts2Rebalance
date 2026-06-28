# Sts2Rebalance

Slay the Spire 2 rebalance mod.

## Documentation

- Change list: `docs/rebalance-changes.md`
- Current ascension reference: `docs/current-ascensions.md`
- Map generation investigation notes: `docs/map-generation-investigation.md`
- Release guide: `docs/release.md`

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

Build and publish:

```powershell
dotnet build Sts2Rebalance.sln -m:1 /nr:false
dotnet publish Sts2Rebalance.sln -m:1 /nr:false
```
