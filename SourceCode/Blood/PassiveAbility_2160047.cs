using System;
using System.Collections.Generic;
using BaseMod;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2160047 :PassiveAbilityBase
    {
        private bool AreaTarget = false;
        private BattleUnitModel BloodKnight;
        private Queue<int> Priority=new Queue<int>(); 
        private bool sacrifice=false;

        public override int MaxPlayPointAdder()
        {
            return -owner.emotionDetail.MaxPlayPointAdderByLevel();
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            BloodKnight = BattleObjectManager.instance.GetAliveList(this.owner.faction).Find(x => x.UnitData.unitData.EnemyUnitId == Tools.MakeLorId(2160010));
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i >= 0; i -= 10)
                Priority.Enqueue(i);
            Harmony_Patch.AddNewCard(owner, new List<int>() { 2160404, 2160404, 2160404}, Priority);
            sacrifice = true;
            AreaTarget = false;
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            owner.bufListDetail.OnRoundEnd();
            if (sacrifice && !owner.IsDead())
            {
                BloodKnight.RecoverHP(BloodKnight.MaxHp/10);
                owner.Die();
            }
        }

        public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
        {
            if (AreaTarget)
                return AtkResist.Endure;
            return base.GetResistBP(origin, detail);
        }
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
        {
            if (AreaTarget)
                return AtkResist.Endure;
            return base.GetResistHP(origin, detail);
        }
        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            base.OnStartParrying(card);
            AreaTarget = false;
        }
        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            base.OnStartTargetedOneSide(attackerCard);
            AreaTarget = false;
        }
        public override void OnStartTargetedByAreaAtk(BattlePlayingCardDataInUnitModel attackerCard)
        {
            base.OnStartTargetedByAreaAtk(attackerCard);
            AreaTarget = true;
        }
    }
}
