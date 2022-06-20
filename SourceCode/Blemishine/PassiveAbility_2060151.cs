using BaseMod;
using System.Collections;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2060151 : PassiveAbilityBase
    {
        public PassiveAbility_2060151(BattleUnitModel model)
        {
            this.Init(model);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(2060151));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(2060151));
            this.rare = Rarity.Unique;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            int dmg = (int)(behavior.DiceResultDamage * 0.5);
            behavior.card.target.TakeDamage(dmg);
            List<BattleUnitModel> friend = BattleObjectManager.instance.GetAliveList(this.owner.faction);
            friend.Remove(this.owner);
            BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(friend);
            battleUnitModel.RecoverHP(dmg);
            owner.battleCardResultLog.SetSucceedAtkEvent(() => KazimierInitializer.UpdateInfo(battleUnitModel));
        }
    }
}
