using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Sts2Rebalance.Rebalance.Cards;

[HarmonyPatch]
internal static class CardRebalancePatches
{
    private static readonly ConditionalWeakTable<CardEnergyCost, CostOverride> CostOverrides = new();

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.Rarity), MethodType.Getter)]
    [HarmonyPostfix]
    private static void OverrideRarity(CardModel __instance, ref CardRarity __result)
    {
        if (__instance is Acrobatics)
        {
            __result = CardRarity.Common;
        }
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.EnergyCost), MethodType.Getter)]
    [HarmonyPostfix]
    private static void TrackEnergyCostOwner(CardModel __instance, CardEnergyCost __result)
    {
        if (__instance is BorrowedTime)
        {
            CostOverrides.GetValue(__result, static _ => new CostOverride(0));
        }
    }

    [HarmonyPatch(typeof(AbstractModel), nameof(AbstractModel.MutableClone))]
    [HarmonyPostfix]
    private static void ApplyCardSetupOnMutableClone(AbstractModel __instance, AbstractModel __result)
    {
        if (__result is CardModel card)
        {
            ApplyCardSetup(card);
        }
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.GetDescriptionForPile),
        typeof(PileType), typeof(CardModel.DescriptionPreviewType), typeof(Creature))]
    [HarmonyPrefix]
    private static bool UseRebalancedCloneForCanonicalDescription(
        CardModel __instance,
        PileType pileType,
        CardModel.DescriptionPreviewType previewType,
        Creature target,
        ref string __result)
    {
        if (!__instance.IsCanonical || !IsRebalancedCard(__instance))
        {
            return true;
        }

        CardModel clone = (CardModel)__instance.MutableClone();
        __result = clone.GetDescriptionForPile(pileType, previewType, target);
        return false;
    }

    [HarmonyPatch(typeof(CardEnergyCost), nameof(CardEnergyCost.GetWithModifiers))]
    [HarmonyPostfix]
    private static void OverrideBorrowedTimeCost(CardEnergyCost __instance, ref int __result)
    {
        if (CostOverrides.TryGetValue(__instance, out CostOverride? costOverride))
        {
            __result = costOverride.Cost;
        }
    }
    
    [HarmonyPatch(typeof(Stoke), "OnPlay")]
    [HarmonyPrefix]
    private static bool StokeOnPlay(Stoke __instance, PlayerChoiceContext choiceContext, CardPlay cardPlay, ref Task __result)
    {
        __result = StokeOnPlayReplacement(__instance, choiceContext);
        return false;
    }

    [HarmonyPatch(typeof(Spite), "OnPlay")]
    [HarmonyPrefix]
    private static bool SpiteOnPlay(Spite __instance, PlayerChoiceContext choiceContext, CardPlay cardPlay, ref Task __result)
    {
        __result = SpiteOnPlayReplacement(__instance, choiceContext, cardPlay);
        return false;
    }

    [HarmonyPatch(typeof(ExpectAFight), "OnPlay")]
    [HarmonyPrefix]
    private static bool ExpectAFightOnPlay(ExpectAFight __instance, CardPlay cardPlay, ref Task __result)
    {
        __result = ExpectAFightOnPlayReplacement(__instance, cardPlay);
        return false;
    }

    [HarmonyPatch(typeof(BorrowedTime), "OnPlay")]
    [HarmonyPrefix]
    private static bool BorrowedTimeOnPlay(BorrowedTime __instance, PlayerChoiceContext choiceContext, CardPlay cardPlay, ref Task __result)
    {
        __result = BorrowedTimeOnPlayReplacement(__instance, choiceContext);
        return false;
    }

    [HarmonyPatch(typeof(Glow), "OnPlay")]
    [HarmonyPrefix]
    private static bool GlowOnPlay(Glow __instance, PlayerChoiceContext choiceContext, CardPlay cardPlay, ref Task __result)
    {
        __result = GlowOnPlayReplacement(__instance, choiceContext);
        return false;
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.UpgradeInternal))]
    [HarmonyPrefix]
    private static void CaptureUpgradeState(CardModel __instance, out bool __state)
    {
        __state = __instance.IsUpgraded;
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.UpgradeInternal))]
    [HarmonyPostfix]
    private static void ApplyRebalanceUpgrade(CardModel __instance, bool __state)
    {
        if (__state || !__instance.IsUpgraded)
        {
            return;
        }

        switch (__instance)
        {
            case Stoke:
                __instance.EnergyCost.UpgradeBy(-1);
                break;
            case Spite:
                UpgradeVarTo(__instance.DynamicVars.Damage.BaseValue, 7m, __instance.DynamicVars.Damage.UpgradeValueBy);
                break;
            case BorrowedTime:
                UpgradeVarTo(__instance.DynamicVars.Energy.BaseValue, 2m, __instance.DynamicVars.Energy.UpgradeValueBy);
                break;
            case Defy:
                UpgradeVarTo(__instance.DynamicVars.Block.BaseValue, 7m, __instance.DynamicVars.Block.UpgradeValueBy);
                UpgradeVarTo(__instance.DynamicVars.Weak.BaseValue, 2m, __instance.DynamicVars.Weak.UpgradeValueBy);
                break;
            case Glow:
                UpgradeVarTo(__instance.DynamicVars.Stars.BaseValue, 2m, __instance.DynamicVars.Stars.UpgradeValueBy);
                UpgradeVarTo(__instance.DynamicVars.Cards.BaseValue, 2m, __instance.DynamicVars.Cards.UpgradeValueBy);
                break;
            case Hotfix:
                __instance.RemoveKeyword(CardKeyword.Exhaust);
                UpgradeVarTo(__instance.DynamicVars["FocusPower"].BaseValue, 3m, __instance.DynamicVars["FocusPower"].UpgradeValueBy);
                break;
            case EternalArmor:
                UpgradeVarTo(__instance.DynamicVars["PlatingPower"].BaseValue, 10m, __instance.DynamicVars["PlatingPower"].UpgradeValueBy);
                break;
        }
    }

    private static async Task StokeOnPlayReplacement(Stoke card, PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Cast", card.Owner.Character.CastAnimDelay);
        List<CardModel> hand = PileType.Hand.GetPile(card.Owner).Cards.ToList();
        int drawCount = hand.Count;
        foreach (CardModel handCard in hand)
        {
            await CardCmd.Exhaust(choiceContext, handCard);
        }

        await CardPileCmd.Draw(choiceContext, drawCount, card.Owner);
    }

    private static async Task SpiteOnPlayReplacement(Spite card, PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue)
            .FromCard(card)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        if (LostHpThisTurn(card.Owner.Creature))
        {
            await CardPileCmd.Draw(choiceContext, 1, card.Owner);
        }
    }

    private static async Task ExpectAFightOnPlayReplacement(ExpectAFight card, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Cast", card.Owner.Character.CastAnimDelay);
        int energy = PileType.Hand.GetPile(card.Owner).Cards.Count(c => c.Type == CardType.Attack);
        await PlayerCmd.GainEnergy(energy, card.Owner);
    }

    private static async Task BorrowedTimeOnPlayReplacement(BorrowedTime card, PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Cast", card.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<CalamityPower>(choiceContext, card.Owner.Creature, 3m, card.Owner.Creature, card);
        await PlayerCmd.GainEnergy(card.DynamicVars.Energy.BaseValue, card.Owner);
    }

    private static async Task GlowOnPlayReplacement(Glow card, PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Cast", card.Owner.Character.CastAnimDelay);
        await PlayerCmd.GainStars(card.DynamicVars.Stars.BaseValue, card.Owner);
        await CardPileCmd.Draw(choiceContext, card.DynamicVars.Cards.BaseValue, card.Owner);
    }

    private static bool LostHpThisTurn(Creature creature)
    {
        return CombatManager.Instance.History.Entries
            .OfType<DamageReceivedEntry>()
            .Any(e => e.HappenedThisTurn(creature.CombatState)
                && e.Receiver == creature
                && e.Result.UnblockedDamage > 0);
    }

    private static bool IsRebalancedCard(CardModel card)
    {
        return card is Stoke
            or Spite
            or ExpectAFight
            or BorrowedTime
            or Defy
            or Dirge
            or Glow
            or Anticipate
            or Hotfix
            or RollingBoulder
            or EternalArmor;
    }

    private static void ApplyCardSetup(CardModel card)
    {
        if (card.IsCanonical)
        {
            return;
        }

        switch (card)
        {
            case Stoke:
                card.EnergyCost.SetCustomBaseCost(card.IsUpgraded ? 0 : 1);
                card.AddKeyword(CardKeyword.Exhaust);
                break;
            case Spite:
                card.DynamicVars.Damage.BaseValue = card.IsUpgraded ? 7 : 5;
                break;
            case BorrowedTime:
                card.EnergyCost.SetCustomBaseCost(0);
                card.DynamicVars.Energy.BaseValue = card.IsUpgraded ? 2 : 1;
                break;
            case Dirge:
                card.RemoveKeyword(CardKeyword.Exhaust);
                break;
            case Glow:
                card.DynamicVars.Stars.BaseValue = card.IsUpgraded ? 2 : 1;
                card.DynamicVars.Cards.BaseValue = 2;
                break;
            case Anticipate:
                card.DynamicVars.Dexterity.BaseValue = card.IsUpgraded ? 4 : 3;
                break;
            case Hotfix:
                card.RemoveKeyword(CardKeyword.Exhaust);
                card.DynamicVars["FocusPower"].BaseValue = card.IsUpgraded ? 3 : 2;
                break;
            case RollingBoulder:
                card.EnergyCost.SetCustomBaseCost(2);
                break;
            case EternalArmor:
                card.EnergyCost.SetCustomBaseCost(2);
                card.DynamicVars["PlatingPower"].BaseValue = card.IsUpgraded ? 10 : 8;
                break;
        }
    }

    private static void UpgradeVarTo(decimal current, decimal target, Action<decimal> upgradeValueBy)
    {
        decimal delta = target - current;
        if (delta != 0)
        {
            upgradeValueBy(delta);
        }
    }

    private sealed record CostOverride(int Cost);
}
