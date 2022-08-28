using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_ObserveRollMax : DiceCardSelfAbilityBase
	{
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddBuf(new ObserveRollMax());
        }
    }
    class ObserveRollMax: BattleUnitBuf
    {
        bool hit = false;
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            base.OnTakeDamageByAttack(atkDice, dmg);
            hit = true;
        }
        public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
        {
            base.ChangeDiceResult(behavior, ref diceResult);
            if (behavior.Type == BehaviourType.Atk && !hit)
                diceResult = behavior.GetDiceMax();
        }
        public override void OnEndBattle(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnEndBattle(curCard);
            Destroy();
        }
    }
}