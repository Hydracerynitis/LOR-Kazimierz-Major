﻿using System;

namespace KazimierzMajor
{
	// Token: 0x020009A1 RID: 2465
	public class DiceCardSelfAbility_kehan2 : DiceCardSelfAbilityBase
	{
		// Token: 0x060033ED RID: 13293 RVA: 0x00131A70 File Offset: 0x0012FC70
		public override void OnStartBattle()
		{
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(base.owner.faction))
			{
				battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 2, base.owner);
				battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 2, base.owner);
			}
		}
	}
}