using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_allydrawCard1atk : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x != owner))?.allyCardDetail.DrawCards(1);
        }
    }
}
