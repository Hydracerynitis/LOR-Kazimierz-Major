using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_fearness2 : DiceCardSelfAbilityBase
	{
        public override void OnUseCard()
        {
            if (card != PassiveAbility_2160031.nightmare)
                return;
            DiceBehaviour dice = new DiceBehaviour()
            {
                Min = 5,
                Dice = 9,
                Type = BehaviourType.Atk,
                Detail = BehaviourDetail.Slash,
                MotionDetail = MotionDetail.J,
                EffectRes = ""
            };
            BattleDiceBehavior diceBehavior = new BattleDiceBehavior() { behaviourInCard = dice };
            card.AddDiceFront(diceBehavior);
        }
    }
}