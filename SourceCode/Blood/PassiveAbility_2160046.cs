using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMod;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2160046 :PassiveAbilityBase
    {
        private int pattern = 0;
        public PassiveAbility_2160046(BattleUnitModel model)
        {
            this.Init(model);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(2160046));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(2160046));
            this.rare = Rarity.Unique;
            model.view.ChangeSkin("Custom_BLOOD2");
        }
        public override int SpeedDiceNumAdder() => -3;
        private Queue<int> Priority=new Queue<int>(); 
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i >= 0; i -= 10)
                Priority.Enqueue(i);
            pattern += 1;
            switch (pattern % 3)
            {
                case 1:
                    KazimierInitializer.AddNewCard(owner, new List<int>() { 2160405, 2160403, 2160403, 2160403 }, Priority);
                    break;
                case 2:
                case 0:
                    KazimierInitializer.AddNewCard(owner, new List<int>() { 2160403, 2160403, 2160403, 2160403 }, Priority);
                    break;
            }
        }
        public override void OnBreakState()
        {
            base.OnBreakState();
            pattern = 0;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 25 });
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2160406));
            List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
            int num = 0;
            foreach (DiceBehaviour diceBehaviour2 in cardItem.DiceBehaviourList)
            {
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                battleDiceBehavior.behaviourInCard = diceBehaviour2.Copy();
                battleDiceBehavior.SetIndex(num++);
                behaviourList.Add(battleDiceBehavior);
            }
            this.owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, behaviourList);
        }
        public override void OnRoundEndTheLast()
        {
            SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160012), Tools.MakeLorId(12160012));
            SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160012), Tools.MakeLorId(12160012));
        }
        public override void OnDie()
        {
            base.OnDie();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (unit != owner)
                    unit.Die();
            }
        }
    }
}
