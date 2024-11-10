using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;
using Sound;

namespace KazimierzMajor
{
    [HarmonyPatch]
    class NightmareClashHP
    {
        [HarmonyPatch(typeof(StageController), nameof(StageController.StartAction))]
        [HarmonyPrefix]
        public static bool StageController_StartAction_Pre(BattlePlayingCardDataInUnitModel card, bool __runOriginal)
        {
            if (!__runOriginal)
                return false;
            if (card != PassiveAbility_2160031.nightmare)
                return true;
            return InitNightmareClash(card);
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.StartParrying))]
        [HarmonyPrefix]
        public static bool StageController_StartParrying_Pre(BattlePlayingCardDataInUnitModel cardA, BattlePlayingCardDataInUnitModel cardB, bool __runOriginal)
        {
            if (!__runOriginal)
                return false;
            if (cardA == PassiveAbility_2160031.nightmare && cardB.isKeepedCard)
                return InitNightmareClash(cardA);
            if (cardB == PassiveAbility_2160031.nightmare && cardA.isKeepedCard)
                return InitNightmareClash(cardB);
            return true;
        }
        public static bool InitNightmareClash(BattlePlayingCardDataInUnitModel card)
        {
            BattleUnitModel target = card.target;
            List<BattleDiceCardModel> cards = new List<BattleDiceCardModel>(target.allyCardDetail.GetHand().FindAll(x => target.CheckCardAvailable(x) && !KazimierInitializer.IsNotClashCard(x)));
            if (cards.Count <= 0)
                return true;
            cards.Sort((x, y) => y.GetCost() - x.GetCost());
            BattleDiceCardModel clashCard = cards[0];
            BattlePlayingCardDataInUnitModel retaliate = new BattlePlayingCardDataInUnitModel()
            {
                owner = target,
                card = clashCard,
                cardAbility = clashCard.CreateDiceCardSelfAbilityScript(),
                target = card.owner,
                slotOrder = card.targetSlotOrder,
                targetSlotOrder = card.slotOrder
            };
            if (retaliate.cardAbility != null)
                retaliate.cardAbility.card = retaliate;
            retaliate.ResetCardQueueWithoutStandby();
            target.allyCardDetail.UseCard(clashCard);
            target.allyCardDetail.SpendCard(clashCard);
            target.cardSlotDetail.ReserveCost(clashCard.GetCost());
            target.cardSlotDetail.SpendCost(clashCard.GetCost());
            Singleton<StageController>.Instance.sp(card, (retaliate));
            return false;
        }
    }
}
