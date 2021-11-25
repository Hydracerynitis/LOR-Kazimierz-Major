using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMod;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160042 :PassiveAbilityBase
    {
        public BattleUnitModel stunModel;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            stunModel = null;
        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            List<BattleUnitModel> units = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
            if(!units.Exists(x => !x.IsBreakLifeZero()))
                return base.ChangeAttackTarget(card, idx);
            if (card.GetID() == Tools.MakeLorId(2160405))
            {
                stunModel = RandomUtil.SelectOne<BattleUnitModel>(units.FindAll(x => !x.IsBreakLifeZero()));
                return stunModel;
            }
            else if(stunModel != null){
                if(units.Exists(x => x != stunModel && !x.IsBreakLifeZero()))
                    return RandomUtil.SelectOne<BattleUnitModel>(units.FindAll(x => x != stunModel && !x.IsBreakLifeZero()));
                else
                    return base.ChangeAttackTarget(card, idx);
            }
            return RandomUtil.SelectOne<BattleUnitModel>(units.FindAll(x => !x.IsBreakLifeZero()));
        }
    }
}
