using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
	public class PassiveAbility_2160031 : PassiveAbilityBase
	{
        private int AoeCoolDown = 0;
        private int minHP =500;
        private bool nextPhase = false;
        public static BattlePlayingCardDataInUnitModel nightmare;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            minHP = owner.MaxHp / 9 * 5;
        }
        public override bool IsTargetable_theLast()
        {
            return false;
        }

        public override void OnRoundEnd()
        {
            base.OnRoundStart();
            nightmare = null;
            if (nextPhase == true)
            {
                owner.passiveDetail.DestroyPassive(this);
                this.owner.RecoverBreakLife(this.owner.MaxBreakLife);
                this.owner.breakDetail.nextTurnBreak = false;
                owner.ResetBreakGauge();
                StageController.Instance._enemyStageManager = new EnemyTeamStageManager_Nightmare(owner.hp,owner.breakDetail.breakGauge);
                PassiveAbility_2160032 phase2 = new PassiveAbility_2160032();
                owner.passiveDetail.AddPassive(phase2);
                phase2.GetDesc();
                owner.Book._deck._deck.RemoveAll(x => x._id== 2160302 || x._id == 2160303 || x._id == 2160306);
                owner.Book._deck._deck.Add(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2160307)));
                owner.Book._deck._deck.Add(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2160308)));
                owner.Book._deck._deck.Add(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2160309)));
                SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160010), Tools.MakeLorId(12160010)).bufListDetail= owner.bufListDetail;
                SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160010), Tools.MakeLorId(12160010)).bufListDetail = owner.bufListDetail;
                SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160010), Tools.MakeLorId(12160010)).bufListDetail = owner.bufListDetail;
                SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160010), Tools.MakeLorId(12160010)).bufListDetail = owner.bufListDetail;
                
            }
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
        }
        public override void OnFixedUpdateInWaitPhase(float delta)
        {
            BattlePlayingCardDataInUnitModel lastCard = owner.cardSlotDetail.cardAry[owner.cardSlotDetail.cardAry.Count - 1];
            if (lastCard != null && lastCard.card.GetSpec().affection != CardAffection.TeamNear && nightmare== null)
            {
                DiceCardSpec newSpec = lastCard.card.GetSpec().Copy();
                lastCard.card.CopySelf();
                newSpec.affection = CardAffection.TeamNear;
                lastCard.card._xmlData.Spec = newSpec;
                foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                {
                    if (unit != lastCard.target && unit.IsTargetable(owner))
                    {
                        lastCard.subTargets.Add(new BattlePlayingCardDataInUnitModel.SubTarget() { target = unit, targetSlotOrder = RandomUtil.Range(0, unit.cardSlotDetail.cardAry.Count - 1) });
                    }
                }
                nightmare = lastCard;
            }
            BattleManagerUI.Instance.ui_TargetArrow.UpdateTargetListData();
        }
        public override void AfterTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (owner.hp <= minHP)
                nextPhase = true;
            base.AfterTakeDamage(attacker, dmg);
        }
        public override int GetMinHp()
        {
            return minHP;
        }
        public override void OnAfterRollSpeedDice()
        {
            owner.allyCardDetail.ExhaustAllCards();
            foreach (DiceCardXmlInfo xml in owner.UnitData.unitData.GetDeck())
            {
                if (xml.id == Tools.MakeLorId(2160307))
                {
                    if (AoeCoolDown > 0)
                        continue;
                    AoeCoolDown = 3;
                    owner.allyCardDetail.AddNewCard(xml.id).SetPriorityAdder(100);
                    continue;
                }
                owner.allyCardDetail.AddNewCard(xml.id);
            }
            if (AoeCoolDown > 0)
                AoeCoolDown--;
        }
    }
}