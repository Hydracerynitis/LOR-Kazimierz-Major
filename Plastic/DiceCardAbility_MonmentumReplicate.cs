using UnityEngine;
using System.Collections;
using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class DiceCardAbility_MonmentumReplicate : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            if (behavior.TargetDice?.behaviourInCard.Type == BehaviourType.Standby)
                return;
            if (BattleUnitBuf_Monmentum.GetBuf(owner,out BattleUnitBuf_Monmentum buf) && buf.stack >= 1)
            {
                BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2060101)));
                if (playingCard == null || card.target.currentDiceAction.cardBehaviorQueue.Count <= 0)
                    return;
                buf.UseStack(1);
                this.card.AddDice(playingCard.CreateDiceCardBehaviorList()[1]);
            }
        }
    }
}

