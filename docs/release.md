# Release Guide

本文档说明 `Sts2Rebalance` 的发布产物、打包方式、GitHub Release 内容和 Steam Workshop 上传准备。Release 面向玩家安装，不包含本地开发缓存或参考源码。

## Release 产物

GitHub Release 推荐上传一个主文件：

```text
Sts2Rebalance-vX.Y.Z.zip
```

zip 内部结构必须是：

```text
Sts2Rebalance/
  Sts2Rebalance.dll
  Sts2Rebalance.pck
  Sts2Rebalance.json
```

可选调试文件：

```text
Sts2Rebalance/
  Sts2Rebalance.pdb
```

公开给普通玩家时可以不放 `.pdb`；如果当前阶段需要排查日志栈和行号，可以放入 `.pdb`。

不要放入 release zip：

- `reference/`
- `.agents/`
- `.git/`
- `.godot/`
- `bin/`
- `obj/`
- `Directory.Build.props`
- `docs/`
- 源码 `.cs`
- 整个 `STS2ModDev` 工作区

## 版本号

发布前同步三个位置：

1. `Sts2Rebalance.json`
   - `version`
   - `min_game_version`
   - `dependencies`
2. Git tag
   - 例如 `v0.1.0`
3. GitHub Release 标题
   - 例如 `Sts2Rebalance v0.1.0`

当前依赖：

```json
[
  {"id": "BaseLib", "min_version": "3.3.2"},
  {"id": "STS2-RitsuLib"}
]
```

Workshop 依赖 ID：

- BaseLib: `3737335127`
- RitsuLib: `3747602295`

## 构建

先确认 `Directory.Build.props` 在本机存在且没有提交到 Git。示例：

```xml
<Project>
  <PropertyGroup>
    <GodotPath>E:/megadot/Godot_v4.5.1-stable_mono_win64/Godot_v4.5.1-stable_mono_win64.exe</GodotPath>
    <Sts2Path>D:/Sponsored/Steam/steamapps/common/Slay the Spire 2</Sts2Path>
    <ModsPath>D:/Sponsored/Steam/steamapps/common/Slay the Spire 2/mods/</ModsPath>
  </PropertyGroup>
</Project>
```

执行：

```powershell
dotnet build Sts2Rebalance.sln -m:1 /nr:false
dotnet publish Sts2Rebalance.sln -m:1 /nr:false
```

`dotnet publish` 调用 Godot 导出 `.pck` 时，当前环境可能打印一次 `Could not load file or assembly 'sts2'`。如果命令退出码为 0 且输出出现 `savepack DONE`，视为导出成功。最终仍以游戏实机加载日志为准。

## 本地产物位置

构建后产物应位于：

```text
D:/Sponsored/Steam/steamapps/common/Slay the Spire 2/mods/Sts2Rebalance/
```

应至少包含：

```text
Sts2Rebalance.dll
Sts2Rebalance.pck
Sts2Rebalance.json
```

可用命令检查：

```powershell
Get-ChildItem 'D:/Sponsored/Steam/steamapps/common/Slay the Spire 2/mods/Sts2Rebalance' |
  Select-Object Name,Length,LastWriteTime
```

## 打包 GitHub Release zip

在 workspace 根目录执行：

```powershell
$version = "v0.1.0"
$modDir = "D:/Sponsored/Steam/steamapps/common/Slay the Spire 2/mods/Sts2Rebalance"
$outDir = "E:/Modding/SlayTheSpire2/Sts2Rebalance/release"
$zip = "$outDir/Sts2Rebalance-$version.zip"

New-Item -ItemType Directory -Force $outDir | Out-Null
Remove-Item -LiteralPath $zip -Force -ErrorAction SilentlyContinue
Compress-Archive -Path "$modDir" -DestinationPath $zip
```

如果要排除 `.pdb`，使用临时目录：

```powershell
$version = "v0.1.0"
$modDir = "D:/Sponsored/Steam/steamapps/common/Slay the Spire 2/mods/Sts2Rebalance"
$stage = "E:/Modding/SlayTheSpire2/Sts2Rebalance/release/stage/Sts2Rebalance"
$zip = "E:/Modding/SlayTheSpire2/Sts2Rebalance/release/Sts2Rebalance-$version.zip"

Remove-Item "E:/Modding/SlayTheSpire2/Sts2Rebalance/release/stage" -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force $stage | Out-Null
Copy-Item "$modDir/Sts2Rebalance.dll" $stage
Copy-Item "$modDir/Sts2Rebalance.pck" $stage
Copy-Item "$modDir/Sts2Rebalance.json" $stage
Remove-Item -LiteralPath $zip -Force -ErrorAction SilentlyContinue
Compress-Archive -Path "E:/Modding/SlayTheSpire2/Sts2Rebalance/release/stage/Sts2Rebalance" -DestinationPath $zip
```

检查 zip：

```powershell
tar -tf "E:/Modding/SlayTheSpire2/Sts2Rebalance/release/Sts2Rebalance-v0.1.0.zip"
```

期望看到：

```text
Sts2Rebalance/Sts2Rebalance.dll
Sts2Rebalance/Sts2Rebalance.pck
Sts2Rebalance/Sts2Rebalance.json
```

## 实机验收

发布前至少验证：

- 游戏启动无 `Sts2Rebalance` 初始化错误。
- 日志中出现：
  - `Found mod manifest file ... Sts2Rebalance.json`
  - `Finished mod initialization for 'Sts2Rebalance'`
- 百科能显示所有角色、无色牌和诅咒牌。
- `预借时间`
  - 百科网格费用为 0。
  - 详情费用为 0。
  - 描述图标正常渲染。
  - 实战施加 3 层灾厄，获得 1(2) 能量。
- `辉光`
  - 描述图标正常渲染。
  - 获得 1(2) 辉星，抽 2。
- `热修复`
  - 百科与详情不显示原版消耗效果。
  - 不消耗，升级后本回合获得 3 集中。
- A6
  - 移除服务恢复 75 基础、每次 +25。
  - 商店非移除服务价格提升 10%。

日志目录：

```text
C:/Users/26636/AppData/Roaming/SlayTheSpire2/logs
```

## GitHub Release 内容

Release 标题：

```text
Sts2Rebalance v0.1.0
```

Release 正文模板：

```markdown
## Requirements

- Slay the Spire 2, tested on v0.107.1
- BaseLib: Steam Workshop 3737335127
- RitsuLib: Steam Workshop 3747602295

## Install

Unzip `Sts2Rebalance-v0.1.0.zip` into:

`<Slay the Spire 2 install>/mods/`

The final path should be:

`<Slay the Spire 2 install>/mods/Sts2Rebalance/Sts2Rebalance.json`

## Changes

See `docs/rebalance-changes.md` in the repository.

## Known Notes

- This is a gameplay-affecting rebalance mod.
- Local testing used BaseLib 3.3.2 and RitsuLib 0.4.40.
```

## Git tag and push

```powershell
git status --short
git tag v0.1.0
git push origin main
git push origin v0.1.0
```

如果 tag 已存在但需要重做，先确认没有公开依赖该 tag，再删除重建：

```powershell
git tag -d v0.1.0
git push origin :refs/tags/v0.1.0
git tag v0.1.0
git push origin v0.1.0
```

## Steam Workshop 上传准备

Workshop workspace 建议结构：

```text
workshop/Sts2Rebalance/
  workshop.json
  image.png
  README.md
  content/
    Sts2Rebalance/
      Sts2Rebalance.dll
      Sts2Rebalance.pck
      Sts2Rebalance.json
```

`workshop.json` 示例：

```json
{
  "title": "Sts2Rebalance",
  "description": "Slay the Spire 2 rebalance mod.",
  "visibility": "private",
  "changeNote": "Initial release",
  "tags": [],
  "dependencies": [3737335127, 3747602295],
  "minBranch": null,
  "maxBranch": null
}
```

注意：

- `dependencies` 使用 Workshop item id，不是 manifest 里的 mod id。
- `visibility` 源码实际接受 `private`、`public`、`unlisted`、`friends_only`。
- 首次上传建议 `private`。

上传命令在 `STS2ModDev` 工作区的 `sts2-mod-uploader` submodule 中执行：

```powershell
dotnet run --project sts2-mod-uploader/ModUploader.csproj -- upload -w workshop/Sts2Rebalance
```

指定已有 item id：

```powershell
dotnet run --project sts2-mod-uploader/ModUploader.csproj -- upload -w workshop/Sts2Rebalance -i <item-id>
```

上传成功后会写入 `mod_id.txt`。不要把 `mod_id.txt` 当作新 mod 的模板提交；它只代表对应 Workshop item。

## 发布前清单

- `Sts2Rebalance.json` 版本号已更新。
- `docs/rebalance-changes.md` 已记录本版变更。
- `dotnet build` 通过。
- `dotnet publish` 通过。
- 本地游戏加载通过。
- 百科和关键卡牌显示通过。
- Git 工作区 clean。
- Git tag 已创建。
- Release zip 内部结构正确。
- GitHub Release 上传 zip。
- 如发布 Workshop，`workshop.json.dependencies` 已填 BaseLib 和 RitsuLib 的 Workshop id。
