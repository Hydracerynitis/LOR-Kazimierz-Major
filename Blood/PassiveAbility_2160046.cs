﻿using System;
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
        public PassiveAbility_2160046(BattleUnitModel model)
        {
            this.Init(model);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(2160046));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(2160046));
            this.rare = Rarity.Unique;
        }
        public override int SpeedDiceNumAdder() => -2;
        private Queue<int> Priority=new Queue<int>(); 
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i >= 0; i -= 10)
                Priority.Enqueue(i);
            Harmony_Patch.AddNewCard(owner, new List<int>() { 2160403, 2160403, 2160403, 2160403 }, Priority);
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
            SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(2160008), Tools.MakeLorId(12160008));
        }
    }
}
