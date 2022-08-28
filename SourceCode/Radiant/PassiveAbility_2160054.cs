using UnityEngine;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2160054 : PassiveAbilityBase
    {
        public override void OnSucceedAreaAttack(BattleDiceBehavior behavior, BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(behavior, target);
            if (target.bufListDetail.FindBuf<Blind>(BufReadyType.NextRound) == null)
                target.bufListDetail.AddReadyBuf(new Blind());
            if (behavior.DiceResultValue > 20)
                target.allyCardDetail.DiscardACardByAbility(target.allyCardDetail.GetHand());
        }
        public class Blind : BattleUnitBuf
        {
            public override string keywordId => "Blind";
            public override string keywordIconId => "ResistWeakBP";
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = -3 });
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
