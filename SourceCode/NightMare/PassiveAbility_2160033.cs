using System;
using BaseMod;

namespace KazimierzMajor
{
	// Token: 0x02000020 RID: 32
	public class PassiveAbility_2160033 : PassiveAbilityBase
	{
		public override void OnWaveStart()
		{
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction))
			{
				battleUnitModel.bufListDetail.AddBufByEtc<BattleUnitBuf_fearness>(1, null, BufReadyType.ThisRound);
			}
		}
	}
}