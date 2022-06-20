using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LOR_DiceSystem;
using System;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2060056 : PassiveAbilityBase
    {
        public int _count = 0;
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (_count >= 5)
                return;
            _count += 1;
            RefreshBuf();
        }
        public override void OnRoundStart()
        {
            RefreshBuf();
        }
        public override void OnStartBattle()
        {
            if (_count >= 5)
            {
                BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2060501)));
                if (playingCard == null)
                    return;
                foreach (BattleDiceBehavior diceCardBehavior in playingCard.CreateDiceCardBehaviorList())
                    this.owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(playingCard, diceCardBehavior);
            }       
        }
        public void RefreshBuf()
        {
            if (owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_BlemishineReady) is BattleUnitBuf_BlemishineReady ready)
                ready.stack = _count;
            else
                owner.bufListDetail.AddBuf(new BattleUnitBuf_BlemishineReady(_count));
        }
        public class BattleUnitBuf_BlemishineReady : BattleUnitBuf
        {
            public override string keywordId => stack >= 5 ? "Blemishine_Ready" : "Blemishine_Not_Ready";
            public override string keywordIconId => "YanArea_Ready";
            public override int paramInBufDesc => stack >= 5 ? base.paramInBufDesc: 5-stack ;
            public override void OnRoundEnd() => this.Destroy();
            public BattleUnitBuf_BlemishineReady(int stack)
            {
                this.stack = stack;
            }
        }
    }
}
