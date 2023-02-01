using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_StrEndBuf : DiceCardSelfAbilityBase
	{
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            BattleObjectManager.instance.GetAliveList_random(owner.faction, 2).ForEach(x =>
            {
                x.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1,owner);
                x.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 1, owner);
            });
        }
    }
}