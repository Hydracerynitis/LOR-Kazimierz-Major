using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
	public class PassiveAbility_2160032 : PassiveAbility_2160132
    {
        public override int SpeedDiceNumAdder()
        {
            return -2;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (BattleSceneRoot.Instance.currentMapObject is TournamentMapManager)
                (BattleSceneRoot.Instance.currentMapObject as TournamentMapManager).ChangeToKhan();
        }
        public PassiveAbility_2160032()
        {
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(2160046));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(2160046));
            this.rare = Rarity.Unique;
        }
    }
}