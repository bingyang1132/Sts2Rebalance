# Sts2Rebalance

**Language:** **中文** | [English](./README_EN.md)

`Sts2Rebalance` 是一个面向《Slay the Spire 2》的平衡性调整 Mod。  
它的主要目标是把正式版按头不让玩小循环的改动还回去，同时调整了部分3费无色卡的不合理设计，对标塔1。

## 功能

- 调整原版进阶效果
- 重做或微调原版卡牌
- 支持百科、卡面、升级预览与实战效果保持一致
- 保留完整改动清单，便于测试和版本回退

## 当前改动

当前版本主要包含：

- A6 取消原版卡牌移除涨价惩罚，改为商店整体涨价
- 铁甲战士、静默猎手、亡灵契约师、储君、故障机器人的运转
- 地图生成逻辑调查文档，为后续机制改动做准备

完整列表见 [`docs/rebalance-changes.md`](docs/rebalance-changes.md)。

## 依赖

- BaseLib, Workshop ID: `3737335127`
- RitsuLib, Workshop ID: `3747602295`

如果通过 Steam Workshop 使用，请先订阅并启用依赖 Mod。

## 下载

- 前往 GitHub Releases 页面下载发布包
- 发布包通常包含 `Sts2Rebalance.dll`、`Sts2Rebalance.json` 与 `Sts2Rebalance.pck`

## 安装

1. 下载发布包，或自行构建得到发布文件
2. 在游戏安装目录下创建或打开 `mods` 文件夹
3. 将发布文件放入游戏目录下的mod文件夹：

```text
...\Steam\steamapps\common\Slay the Spire 2\mods\Sts2Rebalance
```

4. 启动游戏，在 Mod 列表中确认 `Sts2Rebalance` 与依赖已启用

## 已知说明

- 本 Mod 修改原版内容，可能与其他修改同一卡牌、进阶、商店或奖励逻辑的 Mod 冲突
- 游戏更新后，原版卡牌模型、方法名或本地化格式可能变化，需要重新验证
- 如果百科卡面、升级预览和实战效果不一致，请优先反馈具体卡牌、语言、是否升级和复现路径

## 反馈

反馈时如果能提供以下信息，会更容易定位问题：

- 触发问题的卡牌、遗物、敌人、房间或进阶等级
- 是否启用了其他 Mod
- 复现步骤
- 游戏日志

日志通常位于：

```text
C:\Users\你的用户名\AppData\Roaming\SlayTheSpire2\logs
```

## 开发文档

- 改动列表：[`docs/rebalance-changes.md`](docs/rebalance-changes.md)
- 当前进阶参考：[`docs/current-ascensions.md`](docs/current-ascensions.md)
- 地图生成调查：[`docs/map-generation-investigation.md`](docs/map-generation-investigation.md)

## 本地构建

本项目目标框架为 `net9.0`，使用 `Godot.NET.Sdk/4.5.1`。

本机路径通过 `Directory.Build.props` 配置，该文件有意不提交到 Git。可在本地创建：

```xml
<Project>
  <PropertyGroup>
    <GodotPath>E:/megadot/Godot_v4.5.1-stable_mono_win64/Godot_v4.5.1-stable_mono_win64.exe</GodotPath>
    <Sts2Path>D:/Sponsored/Steam/steamapps/common/Slay the Spire 2</Sts2Path>
    <ModsPath>D:/Sponsored/Steam/steamapps/common/Slay the Spire 2/mods/</ModsPath>
  </PropertyGroup>
</Project>
```

构建并发布到本地 `mods` 目录：

```powershell
dotnet build Sts2Rebalance.sln -m:1 /nr:false
dotnet publish Sts2Rebalance.sln -m:1 /nr:false
```

