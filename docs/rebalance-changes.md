# Rebalance Changes

This file records the active gameplay patches in `Sts2Rebalance`.

## Ascension

### A5 Ascender's Bane

Unchanged from vanilla. A5 still adds `Ascender's Bane` to the starting deck.

### A6 Inflation

Vanilla behavior:

- Card removal base cost becomes 100 gold.
- Card removal price increase becomes 50 gold.

Mod behavior:

- Card removal uses the non-ascension formula: base 75 gold, +25 gold per previous removal.
- Non-removal Merchant entries cost 10% more while A6+ is active.
- The 10% increase is applied in `MerchantEntry.Cost` and excludes `MerchantCardRemovalEntry`.

Patch file: `Rebalance/Ascensions/InflationPatch.cs`.

## Cards

Patch file: `Rebalance/Cards/CardRebalancePatches.cs`.

| Card | Change |
| --- | --- |
| Ironclad `Stoke` / 添柴 | 1(0)-cost rare skill. Exhausts your hand, draws the same number of cards, and Exhausts. |
| Ironclad `Spite` / 怨恨 | 0-cost uncommon attack. Deals 5(7) damage. If you lost HP this turn, draw 1 card. |
| Ironclad `Expect a Fight` / 跃跃欲试 | Removes the `NoEnergyGainPower` effect; it no longer prevents later energy gain this turn. |
| Silent `Acrobatics` / 杂技 | Rarity changed from Uncommon to Common. |
| Necrobinder `Borrowed Time` / 预借时间 | 0-cost uncommon skill. Applies 3 `CalamityPower` to yourself, then gains 1(2) energy. |
| Necrobinder `Defy` / 违逆 | Upgrade changed from 9 Block / 1 Weak to 7 Block / 2 Weak. |
| Necrobinder `Dirge` / 挽歌 | Removes Exhaust. |
| Regent `Glow` / 辉光 | 1-cost common skill. Gains 1(2) Star and draws 2 cards. No next-turn draw. |
| Silent `Anticipate` / 预判 | Dexterity changed from 2(3) to 3(4). |
| Defect `Hotfix` / 热修复 | Removes Exhaust. Upgrade now increases temporary Focus from 2 to 3. |
| Colorless `Rolling Boulder` / 滚石 | Cost changed from 3 to 2. |
| Colorless `Eternal Armor` / 永恒铠甲 | Cost changed from 3 to 2. Plating changed from 9(12) to 8(10). |

Localization overrides live in:

- `Sts2Rebalance/localization/eng/cards.json`
- `Sts2Rebalance/localization/zhs/cards.json`
