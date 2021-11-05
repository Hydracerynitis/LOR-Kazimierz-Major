using UnityEngine;
using System.Collections;
using LOR_DiceSystem;
using System;
using BaseMod;

namespace KazimierzMajor
{
    public class DiceCardAbility_ForceReplicate : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            if (behavior.TargetDice?.behaviourInCard.Type == BehaviourType.Standby)
                return;
            if(BattleUnitBuf_Force.GetBuf(owner,out BattleUnitBuf_Force buf) && buf.stack >= 1)
            {
                BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2060101)));
                if (playingCard == null || card.target.currentDiceAction.cardBehaviorQueue.Count <= 0)
                    return;
                buf.UseStack(1);
                BattleDiceBehavior dice = playingCard.CreateDiceCardBehaviorList()[0];
                this.card.AddDice(dice);
            }
        }
    }
}

