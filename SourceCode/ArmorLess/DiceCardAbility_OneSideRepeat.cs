using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardAbility_OneSideRepeat : DiceCardAbilityBase
    {
        private bool isOneSide;
        private bool Activated;
        public override void BeforeRollDice()
        {
            isOneSide = behavior.TargetDice == null;
        }
        public override void AfterAction()
        {
            base.AfterAction();
            if (!isOneSide || Activated)
                return;
            ActivateBonusAttackDice();
            Activated = true;
        }
    }
}
