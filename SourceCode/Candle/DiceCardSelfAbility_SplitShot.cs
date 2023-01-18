using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_SplitShot: DiceCardSelfAbilityBase
    {
        public override void OnApplyCard()
        {
            List<BattleUnitModel> enemies = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll(x => x.speedDiceCount>0 && x.IsTargetable(owner) && x!=card.target);
            if (enemies.Count <= 0)
                return;
            BattleUnitModel victim = RandomUtil.SelectOne(enemies);
            card.subTargets.Add(new BattlePlayingCardDataInUnitModel.SubTarget() { target = victim, targetSlotOrder = RandomUtil.Range(0, victim.speedDiceCount - 1) });
            BattleManagerUI.Instance.ui_TargetArrow.UpdateTargetListData();
        }
        public override void OnEndBattle()
        {
            base.OnEndBattle();
            if (card.subTargets.Count <= 0)
                return;
            card.ChangeSubTargetToMainTarget();
            StageController.Instance.AddAllCardListInBattle(card, card.target, card.targetSlotOrder);
        }
    }
}
