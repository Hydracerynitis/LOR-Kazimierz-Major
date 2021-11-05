using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2060052 : PassiveAbilityBase
    {
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if (!this.IsDefenseDice(behavior.Detail))
                return;
            BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(12060052)));
            if (playingCard == null)
                return;
            foreach (BattleDiceBehavior dice in playingCard.CreateDiceCardBehaviorList())
                this.owner.currentDiceAction.AddDice(dice);
        }
    }
}
