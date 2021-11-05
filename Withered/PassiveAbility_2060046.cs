using UnityEngine;
using System.Collections;
using System;
using LOR_DiceSystem;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2060046 : PassiveAbilityBase
    {
        private int _count =0;
        public override void OnRoundStart()
        {
            _count += 1;
            if (_count == 6)
            {
                this.owner.allyCardDetail.AddNewCard(Tools.MakeLorId(2060402)).XmlData.optionList.Add(CardOption.ExhaustOnUse);
                _count = 0;
            }
        }
    }
}
