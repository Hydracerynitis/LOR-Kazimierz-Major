using System;
using LOR_DiceSystem;
using BaseMod;

namespace KazimierzMajor
{
	public class PassiveAbility_2160033 : PassiveAbilityBase
	{
		public override void OnWaveStart()
		{
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction))
			{
				battleUnitModel.bufListDetail.AddBufByEtc<BattleUnitBuf_fearness>(0, null, BufReadyType.ThisRound);
			}
		}
    }
};