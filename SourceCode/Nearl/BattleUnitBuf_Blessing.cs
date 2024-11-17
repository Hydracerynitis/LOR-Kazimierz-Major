using HarmonyLib;
using LOR_DiceSystem;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor
{
    public class BattleUnitBuf_Blessing : BattleUnitBuf
    {
        public BattleUnitModel Giver;
        public override string keywordId => "Blessing";
        public override string keywordIconId => "Blessing";
        public static void AddBuf(BattleUnitModel model, BattleUnitModel giver, int value)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Blessing) is BattleUnitBuf_Blessing battleUnitBufBlessing))
            {
                battleUnitBufBlessing = new BattleUnitBuf_Blessing{stack = value,Giver = giver};
                model.bufListDetail.AddBuf(battleUnitBufBlessing);
            }
            else
                battleUnitBufBlessing.stack += value;
        }
        public static bool GetBuff(BattleUnitModel model, out BattleUnitBuf_Blessing buf)
        {
            if (model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Blessing) is BattleUnitBuf_Blessing battleUnitBufBlessing)
            {
                buf = battleUnitBufBlessing;
                return true;
            }
            buf = null;
            return false;
        }

        public override void OnRoundEnd()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                unit.RecoverHP((int)(0.4 * unit.MaxHp));
                unit.breakDetail.RecoverBreakLife(_owner.MaxBreakLife);
                unit.breakDetail.RecoverBreak(_owner.breakDetail.GetDefaultBreakGauge());
                unit.breakDetail.nextTurnBreak = false;
            }
            stack -= 1;
            if(stack <= 0)
                Destroy();
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            foreach (BattleDiceBehavior diceBehavior in card.GetDiceBehaviorList())
            {
                if (IsAttackDice(diceBehavior.behaviourInCard.Detail))
                {
                    diceBehavior.behaviourInCard = diceBehavior.behaviourInCard.Copy();
                    diceBehavior.behaviourInCard.Detail = BehaviourDetail.Guard;
                    /*diceBehavior.behaviourInCard.Min -= 1;
                    diceBehavior.behaviourInCard.Dice -= 1;*/
                    diceBehavior.behaviourInCard.Type = BehaviourType.Standby;
                    diceBehavior.behaviourInCard.MotionDetail = MotionDetail.G;
                }
            }
        }
    }
}
