using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
	public class PassiveAbility_2160132 : PassiveAbilityBase, NightmareUpdater
	{
        private EnemyTeamStageManager_Nightmare stageManager;
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            stageManager = (EnemyTeamStageManager_Nightmare)StageController.Instance._enemyStageManager;
        }
        public override void OnBreakState()
        {
            base.OnBreakState();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
                unit.breakDetail.LoseBreakLife();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.SetHp((int)stageManager.hp);
            owner.breakDetail.breakGauge = stageManager.Break;
            KazimierInitializer.UpdateInfo(owner);
            owner.view.EnableStatNumber(false);
        }   
        public override void OnDie()
        {
            base.OnDie();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
                unit.Die();
        }

        public void AfterChangeBreak()
        {
            if (owner.breakDetail.breakLife <= 0)
                return;
            stageManager.Break = owner.breakDetail.breakGauge;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                unit.breakDetail.breakGauge = stageManager.Break;
                if (StageController.Instance.IsLogState())
                    unit.battleCardResultLog?.SetPrintDamagedEffectEvent(()=>KazimierInitializer.UpdateInfo(unit));
                else
                    KazimierInitializer.UpdateInfo(unit);
            }
                
        }

        public void AfterChangeHp()
        {
            stageManager.hp = owner.hp;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                unit.SetHp((int)stageManager.hp);
                if (StageController.Instance.IsLogState())
                    unit.battleCardResultLog?.SetPrintDamagedEffectEvent(() => KazimierInitializer.UpdateInfo(unit));
                else
                    KazimierInitializer.UpdateInfo(unit);
            }
        }
    }
}