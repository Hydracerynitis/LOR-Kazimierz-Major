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
    class NightmareHp
    {
        [HarmonyPatch(typeof(StageController), nameof(StageController.StartAction))]
        [HarmonyPrefix]
        public static bool StageController_StartAction_Pre(BattlePlayingCardDataInUnitModel card)
        {
            if (card != PassiveAbility_2160031.nightmare)
                return true;
            return InitNightmareClash(card);
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.StartParrying))]
        [HarmonyPrefix]
        public static bool StageController_StartParrying_Pre(BattlePlayingCardDataInUnitModel cardA, BattlePlayingCardDataInUnitModel cardB)
        {
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
        [HarmonyPatch(typeof(BattleUnitBreakDetail), nameof(BattleUnitBreakDetail.TakeBreakDamage))]
        [HarmonyPostfix]
        static void BattleUnitBreakDetail_TakeBreakDamage_Post(BattleUnitBreakDetail __instance)
        {
            foreach (PassiveAbilityBase passive in __instance._self.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance._self.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeBreak());
                    else
                        (passive as NightmareUpdater).AfterChangeBreak();
                }
        }
        [HarmonyPatch(typeof(BattleUnitBreakDetail), nameof(BattleUnitBreakDetail.RecoverBreak))]
        [HarmonyPostfix]
        static void BattleUnitBreakDetail_RecoverBreak_Post(BattleUnitBreakDetail __instance)
        {
            foreach (PassiveAbilityBase passive in __instance._self.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance._self.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeBreak());
                    else
                        (passive as NightmareUpdater).AfterChangeBreak();
                }
        }
        [HarmonyPatch(typeof(BattleUnitBreakDetail), nameof(BattleUnitBreakDetail.LoseBreakGauge))]
        [HarmonyPostfix]
        static void BattleUnitBreakDetail_LoseBreakGauge_Post(BattleUnitBreakDetail __instance)
        {
            foreach (PassiveAbilityBase passive in __instance._self.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance._self.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeBreak());
                    else
                        (passive as NightmareUpdater).AfterChangeBreak();
                }
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.RecoverHP))]
        [HarmonyPostfix]
        static void BattleUnitModel_RecoverHP_Post(BattleUnitModel __instance)
        {
            foreach (PassiveAbilityBase passive in __instance.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeHp());
                    else
                        (passive as NightmareUpdater).AfterChangeHp();
                }
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.TakeDamage))]
        [HarmonyPostfix]
        static void BattleUnitModel_TakeDamage_Post(BattleUnitModel __instance)
        {
            foreach (PassiveAbilityBase passive in __instance.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeHp());
                    else
                        (passive as NightmareUpdater).AfterChangeHp();
                }
            if (__instance.bufListDetail.FindBuf<Shield>() is Shield s)
            {
                s.Reduce();
                if (Singleton<StageController>.Instance.IsLogState())
                    __instance.battleCardResultLog?.SetCreatureEffectSound("Creature/Greed_StrongAtk_Defensed");
                else
                    SoundEffectPlayer.PlaySound("Creature/Greed_StrongAtk_Defensed");
            }        
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.LoseHp))]
        [HarmonyPostfix]
        static void BattleUnitModel_LoseHp_Post(BattleUnitModel __instance)
        {
            foreach (PassiveAbilityBase passive in __instance.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeHp());
                    else
                        (passive as NightmareUpdater).AfterChangeHp();
                }
        }
        [HarmonyPatch(typeof(BattleObjectManager), nameof(BattleObjectManager.OnFixedUpdate))]
        [HarmonyPostfix]
        static void BattleObjectManager_OnFixedUpdate_Post(float deltaTime)
        {
            foreach (BattleUnitModel unit in KhanEffectData.added)
                unit.OnFixedUpdate(deltaTime);
        }
        [HarmonyPatch(typeof(BattleAllyCardDetail),nameof(BattleAllyCardDetail.ReturnCardToHand))]
        [HarmonyPostfix]
        static void BattleAllyCardDetail_ReturnCardToHand(BattleAllyCardDetail __instance,BattleDiceCardModel appliedCard)
        {
            if (__instance._self.bufListDetail.HasBuf<KhanStance>())
            {
                if (!KhanStance.ChangeCards.Contains(appliedCard))
                    KhanStance.ChangeToTeamNear(appliedCard);
            }
            else if (KhanStance.ChangeCards.Contains(appliedCard))
            {
                KhanStance.ChangeBack(appliedCard);
            }
        }
        [HarmonyPatch(typeof(StageController),nameof(StageController.EndBattle))]
        [HarmonyPostfix]
        static void StageController_EndBattle()
        {
            KhanStance.ChangeCards.Clear();
        }
    }
}
