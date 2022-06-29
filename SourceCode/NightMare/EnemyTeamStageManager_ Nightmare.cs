using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class EnemyTeamStageManager_Nightmare : EnemyTeamStageManager
    {
        public float hp;
        public int Break;
        public override bool HideEnemyTarget()
        {
            return true;
        }
        public EnemyTeamStageManager_Nightmare(float hp,int Break)
        {
            this.hp = hp;
            this.Break = Break;
        }
    }
}
