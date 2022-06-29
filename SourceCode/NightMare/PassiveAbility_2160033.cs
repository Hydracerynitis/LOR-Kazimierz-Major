using System;
using LOR_DiceSystem;
using BaseMod;

namespace KazimierzMajor
{
	public class PassiveAbility_2160033 : PassiveAbilityBase
	{
		private int AoeCoolDown = 0;
		public override void OnWaveStart()
		{
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction))
			{
				battleUnitModel.bufListDetail.AddBufByEtc<BattleUnitBuf_fearness>(0, null, BufReadyType.ThisRound);
			}
		}
        public override void OnRoundStart()
        {
			owner.allyCardDetail.ExhaustAllCards();
			foreach (DiceCardXmlInfo xml in owner.UnitData.unitData.GetDeck())
            {
                if (xml.id == Tools.MakeLorId(2160307))
                {
					if (AoeCoolDown > 0)
						continue;
					AoeCoolDown = 3;
					owner.allyCardDetail.AddNewCard(xml.id).SetPriorityAdder(100);
					continue;
				}
				owner.allyCardDetail.AddNewCard(xml.id);
			}
			if (AoeCoolDown > 0)
				AoeCoolDown--;
        }
    }
};