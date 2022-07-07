using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
	public class PassiveAbility_2160232 : PassiveAbilityBase
	{
		private int accumulated=0;
        private int stack = 0;
        public override void AfterGiveDamage(int damage)
        {
            base.AfterGiveDamage(damage);
            accumulated += damage;
            for (; accumulated > 50; accumulated -= 50)
                stack++;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
                if (unit != owner)
                    unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DmgUp, 2 * stack);
        }
    }
}