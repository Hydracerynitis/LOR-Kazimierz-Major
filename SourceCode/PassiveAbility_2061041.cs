using UnityEngine;
using System.Collections.Generic;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2061041 : PassiveAbilityBase
    {
        private bool _dead = false;
        public override void OnRoundStart()
        {
            _dead = false;
            List<BattleUnitModel> enemy = BattleObjectManager.instance.GetList(x => x.faction != owner.faction);
            List<BattleUnitModel> aliveEnemy= BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
            List<BattleUnitModel> ally = BattleObjectManager.instance.GetAliveList(owner.faction);
            if (enemy.Exists(x => x.IsDead()) && ally.Count > aliveEnemy.Count)
                _dead = true;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (_dead)
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
    }
}
