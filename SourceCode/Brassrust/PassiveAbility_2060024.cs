using BaseMod;
using LOR_DiceSystem;
using System;
using System.Threading;

namespace KazimierzMajor 
{
    public class PassiveAbility_2060024 : PassiveAbilityBase
    {
        private int _special;
        public override void OnRoundEnd()
        {
            _special = 0;
            int speeddice = owner.Book.GetSpeedDiceRule(owner).speedDiceList.Count;
            foreach (BattleDiceCardModel card in this.owner.allyCardDetail.GetHand())
            {
                if (card.GetID() == Tools.MakeLorId(2060201))
                    _special += 1;
            }
            if (this.owner.hp <= this.owner.MaxHp / 4)
            {
                this.owner.breakDetail.RecoverBreakLife(this.owner.MaxBreakLife);
                this.owner.breakDetail.nextTurnBreak = false;
                this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
                if (_special >= speeddice || (float)this.owner.hp > (float)(this.owner.MaxHp / 4))
                    return;
                for (; _special < speeddice; _special++)
                {
                    this.owner.allyCardDetail.AddNewCard(Tools.MakeLorId(2060201)).XmlData.optionList.Add(CardOption.ExhaustOnUse);
                }
            }
        }
    }
}

