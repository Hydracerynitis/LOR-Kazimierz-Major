using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_Improvise: DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            if (card.target.currentDiceAction.GetOriginalDiceBehaviorList().FindAll(x => x.Type != BehaviourType.Standby).Count <= 1)
            {
                BattleDiceBehavior diceBehavior = this.card.GetDiceBehaviorList()[1];
                diceBehavior.behaviourInCard = diceBehavior.behaviourInCard.Copy();
                diceBehavior.behaviourInCard.Detail = BehaviourDetail.Hit;
                diceBehavior.behaviourInCard.Min -= 2;
                diceBehavior.behaviourInCard.Dice -= 2;
                diceBehavior.behaviourInCard.Type = BehaviourType.Atk;
            }
        }
        public override void OnStartOneSideAction()
        {
            base.OnStartOneSideAction();
            foreach (BattleDiceBehavior diceBehavior in this.card.GetDiceBehaviorList())
            {
                if (this.IsDefenseDice(diceBehavior.behaviourInCard.Detail))
                {
                    diceBehavior.behaviourInCard = diceBehavior.behaviourInCard.Copy();
                    diceBehavior.behaviourInCard.Detail = BehaviourDetail.Hit;
                    diceBehavior.behaviourInCard.Min -= 2;
                    diceBehavior.behaviourInCard.Dice -= 2;
                    diceBehavior.behaviourInCard.Type = BehaviourType.Atk;
                    diceBehavior.behaviourInCard.EffectRes = "LiuSection2Unit_J";
                    diceBehavior.behaviourInCard.MotionDetail = MotionDetail.J;
                }
            }
        }
    }
}
