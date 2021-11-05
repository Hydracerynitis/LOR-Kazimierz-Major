using BaseMod;
using LOR_DiceSystem;
using System;
using System.Threading;

namespace KazimierzMajor
{
    public class PassiveAbility_2060034 : PassiveAbilityBase
    {
        private bool _getannoyed;
        public override void OnRoundStart()
        {
            _getannoyed = true;
            foreach (BattleDiceCardModel card in this.owner.allyCardDetail.GetHand())
            {
                if (card.GetID() == Tools.MakeLorId(2060301))
                {
                    _getannoyed = false;
                    return;
                }
            }
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction))
            {
                if (_getannoyed && BattleUnitBuf_Depressed.GetBuf(battleUnitModel,out BattleUnitBuf_Depressed buf)&& buf.stack >= 3)
                {
                    this.owner.allyCardDetail.AddNewCard(Tools.MakeLorId(2060301)).XmlData.optionList.Add(CardOption.ExhaustOnUse);
                    _getannoyed = false;
                    return;
                }
            }
        }
    }
}
