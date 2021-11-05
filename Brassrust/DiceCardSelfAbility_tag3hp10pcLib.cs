using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_tag3hp10pcLib : DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            this.card.ApplyDiceAbility(DiceMatch.AllDice, new DiceCardAbility_tagLib());
            this.card.ApplyDiceAbility(DiceMatch.LastDice, new DiceCardAbility_removetagLib());
        }
        public class DiceCardAbility_tagLib : DiceCardAbilityBase
        {
            public override void OnSucceedAttack()
            {
                BattleUnitBuf_Tag.AddBuf(this.card.target);
            }
        }
        public class DiceCardAbility_removetagLib : DiceCardAbilityBase
        {
            public override void AfterAction()
            {
                BattleUnitBuf_Tag.Remove(this.card.target);
            }
        }
        public class BattleUnitBuf_Tag : BattleUnitBuf
        {
            public static void AddBuf(BattleUnitModel model)
            {
                if (!(model.bufListDetail.GetActivatedBufList().Find((x => x is BattleUnitBuf_Tag)) is BattleUnitBuf_Tag tag))
                {
                    tag = new BattleUnitBuf_Tag() { stack = 1 };
                    model.bufListDetail.AddBuf(tag);
                }
                else
                    tag.stack += 1;
                if (tag.stack == 3)
                    tag._owner.TakeDamage(Math.Min((int)(tag._owner.MaxHp * 0.1), 24));
            }
            public static void Remove(BattleUnitModel model)
            {
                if (model.bufListDetail.GetActivatedBufList().Find((x => x is BattleUnitBuf_Tag)) is BattleUnitBuf_Tag tag)
                    tag.Destroy();
            }
        }
    }
}
