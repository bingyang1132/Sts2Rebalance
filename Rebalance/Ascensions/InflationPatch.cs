using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Sts2Rebalance.Rebalance.Ascensions;

[HarmonyPatch]
internal static class InflationPatch
{
    private const float ShopPriceMultiplier = 1.1f;

    [HarmonyPatch(typeof(MerchantCardRemovalEntry), nameof(MerchantCardRemovalEntry.CalcCost))]
    [HarmonyPostfix]
    private static void RestoreCardRemovalCost(MerchantCardRemovalEntry __instance)
    {
        __instance._cost = 75 + 25 * __instance._player.ExtraFields.CardShopRemovalsUsed;
    }

    [HarmonyPatch(typeof(MerchantEntry), nameof(MerchantEntry.Cost), MethodType.Getter)]
    [HarmonyPostfix]
    private static void IncreaseShopPrices(MerchantEntry __instance, ref int __result)
    {
        if (__instance is MerchantCardRemovalEntry)
        {
            return;
        }

        if (__instance._player.RunState.CurrentRoom is not MerchantRoom)
        {
            return;
        }

        if (!RunManager.Instance.HasAscension(AscensionLevel.Inflation))
        {
            return;
        }

        __result = Mathf.RoundToInt(__result * ShopPriceMultiplier);
    }
}
