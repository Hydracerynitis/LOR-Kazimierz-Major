using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
	public class PassiveAbility_2160032 : PassiveAbility_2160132
    {
        private int AoeCoolDown = 0;
        private int AoeCount = 0;
        public override int SpeedDiceNumAdder()
        {
            return -2;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (BattleSceneRoot.Instance.currentMapObject is TournamentMapManager)
                (BattleSceneRoot.Instance.currentMapObject as TournamentMapManager).ChangeToKhan();
            owner.bufListDetail.AddBuf(new aoeReduction());
        }
        public PassiveAbility_2160032()
        {
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(2160032));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(2160032));
            this.rare = Rarity.Unique;
        }
        public override void OnAfterRollSpeedDice()
        {
            owner.allyCardDetail.ExhaustAllCards();
            if (AoeCoolDown <= 0)
            {
                if (AoeCount < 2)
                    AoeCoolDown = 1;
                else
                    AoeCoolDown = 2;
                AoeCount++;
                owner.allyCardDetail.AddNewCard(Tools.MakeLorId(2160307)).SetPriorityAdder(100);
            }
            else
            {
                AoeCoolDown--;
                owner.allyCardDetail.AddNewCard(Tools.MakeLorId(2160306));
            }
            owner.allyCardDetail.AddNewCard(Tools.MakeLorId(RandomUtil.SelectOne(2160302, 2160303, 2160304)));
            owner.allyCardDetail.AddNewCard(Tools.MakeLorId(RandomUtil.SelectOne(2160308)));
            owner.allyCardDetail.AddNewCard(Tools.MakeLorId(RandomUtil.SelectOne(2160309)));
        }
        public class aoeReduction : BattleUnitBuf
        {
            public override int GetDamageReductionRate()
            {
                return StageController.Instance.phase == StageController.StagePhase.ExecuteFarAreaPlay ? 90 : 0;
            }
            public override int GetBreakDamageReductionRate()
            {
                return StageController.Instance.phase == StageController.StagePhase.ExecuteFarAreaPlay ? 90 : 0;
            }
        }
    }
}