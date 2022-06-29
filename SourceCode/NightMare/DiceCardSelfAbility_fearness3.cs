using System;
using BaseMod;

namespace KazimierzMajor
{
	public class DiceCardSelfAbility_fearness3 : DiceCardSelfAbilityBase
	{
		public override void OnStartBattle()
		{
			if (card != PassiveAbility_2160031.nightmare)
				return;
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
			{
				BattleUnitBuf_fearness buf = battleUnitModel.bufListDetail.FindBuf<BattleUnitBuf_fearness>();
				if(buf!=null)
					buf.Boost = true;
			}
		}
	}
}