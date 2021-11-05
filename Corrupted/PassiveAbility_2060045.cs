using UnityEngine;
using System.Collections;
using LOR_DiceSystem;
using System;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2060045 : PassiveAbilityBase
    {
        private int _count=3;
        public override void OnRoundStart()
        {
            _count += 1;
            if (_count == 6)
            {
                this.owner.allyCardDetail.AddNewCard(Tools.MakeLorId(2060401)).XmlData.optionList.Add(CardOption.ExhaustOnUse);
                _count = 0;
            }
        }
    }
}
