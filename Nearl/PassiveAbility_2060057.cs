using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LOR_DiceSystem;
using System;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2060057 : PassiveAbilityBase
    {
        private bool _activated = false;
        public override void OnRoundStart()
        {
            if (_activated)
                return;
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                if (battleUnitModel.hp < (int)(battleUnitModel.MaxHp / 2))
                {
                    this.owner.allyCardDetail.AddNewCard(Tools.MakeLorId(2060502)).XmlData.optionList.Add(CardOption.ExhaustOnUse);
                    _activated = true;
                }
            }
        }
    }
}
