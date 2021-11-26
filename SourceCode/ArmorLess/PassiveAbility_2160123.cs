using System;
using System.Collections.Generic;
using System.Linq;
using BaseMod;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160123 :PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x!=owner))
                unit.bufListDetail.AddBuf(new PassiveAbility_2160023.BountySearcher());
        }
        public override void OnRoundStart()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                unit.bufListDetail.RemoveBufAll(typeof(PassiveAbility_2160023.Bounty));
        }
        public override void OnDie()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                unit.bufListDetail.RemoveBufAll(typeof(PassiveAbility_2160023.Bounty));
        }
        public override void OnFixedUpdateInWaitPhase(float delta)
        {
            if (!owner.allyCardDetail.GetHand().Exists(x => x.GetID() == Tools.MakeLorId(2161207)))
                owner.allyCardDetail.AddTempCard(Tools.MakeLorId(2161207));
        }
    }
}
