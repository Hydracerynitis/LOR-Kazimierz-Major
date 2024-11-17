using UnityEngine;
using System.Collections;
using LOR_DiceSystem;
using System;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2061042 : PassiveAbilityBase
    {
        private int _count=0;
        public override void OnRoundStart()
        {
            _count += 1;
            if (_count >= 4)
            {
                int id = 0;
                if (owner.UnitData.unitData.EnemyUnitId == Tools.MakeLorId(2060004))
                    id = 2060401;
                if (owner.UnitData.unitData.EnemyUnitId == Tools.MakeLorId(2060005))
                    id = 2060402;
                owner.allyCardDetail.AddNewCard(Tools.MakeLorId(id)).XmlData.optionList.Add(CardOption.ExhaustOnUse);
                _count = 0;
            }
        }
    }
}
