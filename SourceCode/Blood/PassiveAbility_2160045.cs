using System;
using System.Collections.Generic;
using System.Linq;
using BaseMod;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160045 :PassiveAbilityBase
    {
        private Queue<int> Priority=new Queue<int>();
        private bool init=false;
        private bool check=false;
        private int phasetrans = 0;
        private int pattern = 0;
        public override bool isImmortal => true;
        public override bool IsImmuneDmg(DamageType type, KeywordBuf keyword = KeywordBuf.None) => phasetrans > 0 || check;
        public override bool isTargetable => phasetrans<=0 && !check;
        public override bool isActionable => phasetrans <= 0 && !check;
        public override void OnRoundStartAfter()
        {
            if (phasetrans > 0 || owner.IsBreakLifeZero())
                return;
            pattern += 1;
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i >= 0; i -= 10)
                Priority.Enqueue(i);
            switch (pattern % 3)
            {
                case 1:
                    Harmony_Patch.AddNewCard(owner, new List<int>() { 2160405, 2160401, 2160401, 2160401, 2160402, 2160402, 2160402 },Priority);
                    break;
                case 2:
                case 0:
                    Harmony_Patch.AddNewCard(owner, new List<int>() { 2160401, 2160401, 2160401, 2160401, 2160402, 2160402, 2160402 }, Priority);
                    break;
            }
        }
        public override void OnBreakState()
        {
            base.OnBreakState();
            pattern = 0;
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (phasetrans<=0 && dmg > owner.hp)
                init = true;
            return base.BeforeTakeDamage(attacker, dmg);
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEnd();
            if (init)
            {
                owner.bufListDetail.RemoveBufAll();
                owner.SetHp(owner.MaxHp / 2);
                this.owner.breakDetail.RecoverBreakLife(this.owner.MaxBreakLife);
                this.owner.breakDetail.nextTurnBreak = false;
                this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
                phasetrans = 3;
                init = false;              
            }
            if (owner.hp == owner.MaxHp)
                phasetrans = 0;
            if (phasetrans > 0)
            {
                SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160011), Tools.MakeLorId(12160011));
                SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160011), Tools.MakeLorId(12160011));
                SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160011), Tools.MakeLorId(12160011));
                SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160011), Tools.MakeLorId(12160011));
                phasetrans--;
                if(phasetrans==0)
                    check = true;
            }
            else if(check)
            {
                if (owner.hp != owner.MaxHp)
                {
                    owner.passiveDetail.AddPassive(new PassiveAbility_2160046(owner));
                    PassiveAbilityBase passive = owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_2160044);
                    if (passive != null)
                        owner.passiveDetail.DestroyPassive(passive);
                    owner.passiveDetail.DestroyPassive(this);
                    SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160011), Tools.MakeLorId(12160011));
                    SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160011), Tools.MakeLorId(12160011));
                }
                pattern = 0;
                check = false;   
            }
        }
    }
}
