# 平衡性改动列表

本文记录 `Sts2Rebalance` 当前生效的游戏性补丁。

## Mod 定位

一个平衡性调整 Mod。

主要目标是把正式版按头不让玩小循环的改动还回去，同时调整了部分 3 费无色卡的不合理设计，对标塔 1。

## 进阶

### A5 进阶者之灾

保持原版不变。A5 仍然会向初始牌组加入 `Ascender's Bane`。

### A6 通货膨胀

原版效果：

- 商人的卡牌移除服务基础价格变为 100 金币。
- 每次使用卡牌移除服务后的涨价幅度变为 50 金币。

Mod 效果：

- 卡牌移除服务恢复为无进阶时的公式：基础价格 75 金币，每次使用后 +25 金币。
- A6 及以上时，商店中非卡牌移除项目的价格提高 10%。
- 10% 涨价作用于 `MerchantEntry.Cost`，并排除 `MerchantCardRemovalEntry`。

补丁文件：`Rebalance/Ascensions/InflationPatch.cs`。

## 卡牌

补丁文件：`Rebalance/Cards/CardRebalancePatches.cs`。

| 卡牌 | 改动 |
| --- | --- |
| 铁甲战士 `Stoke` / 添柴 | 重做为 1(0) 费稀有技能牌。消耗所有手牌，然后抽等量的牌。消耗。 |
| 铁甲战士 `Spite` / 怨恨 | 重做为 0 费罕见攻击牌。造成 5(7) 点伤害。如果你在本回合失去过生命值，抽 1 张牌。 |
| 铁甲战士 `Expect a Fight` / 跃跃欲试 | 移除 `NoEnergyGainPower` 效果；本回合内不再阻止后续获得能量。 |
| 静默猎手 `Acrobatics` / 杂技 | 稀有度从罕见改为普通。 |
| 亡灵契约师 `Borrowed Time` / 预借时间 | 重做为 0 费罕见技能牌。对自己施加 3 层 `CalamityPower`，然后获得 1(2) 点能量。 |
| 亡灵契约师 `Defy` / 违逆 | 升级版从 9 点格挡 / 1 层虚弱改为 7 点格挡 / 2 层虚弱。 |
| 亡灵契约师 `Dirge` / 挽歌 | 移除消耗。 |
| 储君 `Glow` / 辉光 | 重做为 1 费普通技能牌。获得 1(2) 辉星，抽 2 张牌。不再提供下回合抽牌。 |
| 静默猎手 `Anticipate` / 预判 | 获得的敏捷从 2(3) 点提高到 3(4) 点。 |
| 故障机器人 `Hotfix` / 热修复 | 移除消耗。升级后本回合获得的集中从 2 点提高到 3 点。 |
| 无色 `Rolling Boulder` / 滚石 | 耗能从 3 改为 2。 |
| 无色 `Eternal Armor` / 永恒铠甲 | 耗能从 3 改为 2。获得的覆甲从 9(12) 改为 8(10)。 |

## 本地化

卡牌文本覆盖位于：

- `Sts2Rebalance/localization/eng/cards.json`
- `Sts2Rebalance/localization/zhs/cards.json`
