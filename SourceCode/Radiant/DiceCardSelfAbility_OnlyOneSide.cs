using UnityEngine;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_OnlyOneSide : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            List<BattleUnitModel> oneSider = new List<BattleUnitModel>();
            foreach(BattlePlayingCardDataInUnitModel cards in StageController.Instance.GetAllCards())
            {
                if (FastLateAttack.GetParry(cards) == null && cards.target==owner)
                {
                    oneSider.Add(cards.owner);
                }
            }
            card.subTargets.RemoveAll(x => !oneSider.Contains(x.target));
            if (!oneSider.Contains(card.target))
            {
                if (card.subTargets.Count > 0)
                    card.ChangeSubTargetToMainTarget();
                else
                    card.DestroyDice(DiceMatch.AllDice);
            }
        }
    }
}
