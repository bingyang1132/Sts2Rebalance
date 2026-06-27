# Current Ascension Design

Source: `reference/spire-codex/data-beta/v0.107.1/zhs/ascensions.json` and the game DLL from `d:/sponsored/steam/steamapps/common/Slay the Spire 2`.

| Level | Name | Description |
| --- | --- | --- |
| A0 | 无进阶 | 在没有任何进阶调整的影响下游玩。 |
| A1 | 精英蜂拥 | 精英敌人出现更加频繁。 |
| A2 | 旅途劳顿 | 先古之民只会回复你已损失生命值的 80%。 |
| A3 | 贫穷 | 敌人和宝箱掉落的金币减少 25%。 |
| A4 | 收紧腰带 | 游戏开始时药水栏位减少 1。 |
| A5 | 进阶之灾 | 游戏开始时被诅咒。 |
| A6 | 通货膨胀 | 原版：商人的卡牌移除服务价格提升。本 mod：卡牌移除恢复为 75 金币基础价、每次涨价 25 金币；商店商品价格提高 10%。 |
| A7 | 稀缺 | 稀有和已升级卡牌更少出现。 |
| A8 | 强韧敌人 | 所有的敌人变得更难杀死。 |
| A9 | 致命敌人 | 所有的敌人变得更加致命。 |
| A10 | 双重Boss | 在第 3 幕末尾与两名首领战斗。 |

## Implementation Notes

- `AscensionManager.ApplyEffectsTo(Player)` applies A4 `TightBelt` and A5 `AscendersBane`.
- `AscensionHelper.GetValueIfAscension(...)` is the shared helper used by many numeric checks.
- `RunManager.HasAscension(AscensionLevel)` delegates to `AscensionManager.HasLevel(...)`.
- Enemy HP/damage and many economy/card reward changes are not one global multiplier; they are distributed across source-level constants and `AscensionHelper` calls.

## Current Rebalance

A5 `AscendersBane` is unchanged.

A6 `Inflation` is reworked:

- `MerchantCardRemovalEntry.CalcCost()` is patched back to the non-ascension formula: `75 + 25 * removalsUsed`.
- `MerchantEntry.Cost` is patched to increase non-removal Merchant entries by 10% while A6+ is active in a `MerchantRoom`.

The in-game A6 description is overridden through:

- `Sts2Rebalance/localization/eng/ascension.json`
- `Sts2Rebalance/localization/zhs/ascension.json`
