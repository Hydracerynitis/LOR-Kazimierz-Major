using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMod;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2160048 :PassiveAbilityBase
    {
        private BattleUnitModel BloodKnight;
        public BattleUnitModel stunModel
        {
            get
            {
                if(BloodKnight==null)
                    return null;
                else
                {
                    PassiveAbility_2160042 passive=BloodKnight.passiveDetail.PassiveList.Find(x => x is PassiveAbility_2160042) as PassiveAbility_2160042;
                    if (passive == null)
                        return null;
                    return passive.stunModel;
                }
            }
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            BloodKnight = BattleObjectManager.instance.GetAliveList(this.owner.faction).Find(x => x.UnitData.unitData.EnemyUnitId == Tools.MakeLorId(2160007));
        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            List<BattleUnitModel> units = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
            if (!units.Exists(x => !x.IsBreakLifeZero()))
                return base.ChangeAttackTarget(card, idx);
            else if (stunModel != null)
            {
                if (units.Exists(x => x != stunModel && !x.IsBreakLifeZero()))
                    return RandomUtil.SelectOne<BattleUnitModel>(units.FindAll(x => x != stunModel && !x.IsBreakLifeZero()));
                else
                    return base.ChangeAttackTarget(card, idx);
            }
            return RandomUtil.SelectOne<BattleUnitModel>(units.FindAll(x => !x.IsBreakLifeZero()));
        }
    }
}
