using System;
using BaseMod;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;

namespace KazimierzMajor
{
	public class PassiveAbility_2160131 : PassiveAbilityBase
	{
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(2161301));
        }
        public override void OnRoundStart()
        {
            if (!owner.bufListDetail.HasBuf<KhanStance>())
            {
                foreach (BattleDiceCardModel cards in owner.allyCardDetail.GetAllDeck())
                {
                    if (KhanStance.ChangeCards.Contains(cards))
                        KhanStance.ChangeBack(cards);
                }
            }
            else
            {
                foreach (BattleDiceCardModel cards in owner.allyCardDetail.GetAllDeck())
                {
                    if (!KhanStance.ChangeCards.Contains(cards))
                        KhanStance.ChangeToTeamNear(cards);
                }
            }
            
        }
    }
    public class DiceCardSelfAbility_KhanStance: DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            if (unit.bufListDetail.FindBuf<KhanStance>() is KhanStance ks)
            {
                ks.Destroy();
                unit.bufListDetail._bufList.Remove(ks);
            }
            else
                unit.bufListDetail.AddBuf(new KhanStance() { stack=0});
        }
    }
    public class KhanStance : BattleUnitBuf
    {
        public override string keywordId => "KhanStance";
        public override string keywordIconId => "DmgUp";
        public static List<BattleDiceCardModel> ChangeCards = new List<BattleDiceCardModel>();
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            owner.allyCardDetail._cardInHand.ForEach(x => ChangeToTeamNear(x));
            owner.allyCardDetail._cardInDeck.ForEach(x => ChangeToTeamNear(x));
            owner.allyCardDetail._cardInDiscarded.ForEach(x => ChangeToTeamNear(x));
        }
        public override void OnDie()
        {
            base.OnDie();
            Destroy();
        }
        public override void Destroy()
        {
            base.Destroy();
            _owner.allyCardDetail._cardInHand.ForEach(x => ChangeBack(x));
            _owner.allyCardDetail._cardInDeck.ForEach(x => ChangeBack(x));
            _owner.allyCardDetail._cardInDiscarded.ForEach(x => ChangeBack(x));
        }
        public static void ChangeToTeamNear(BattleDiceCardModel card)
        {
            if (KazimierInitializer.IsNotClashCard(card))
                return;
            card.CopySelf();
            DiceCardSpec spec = card.XmlData.Spec.Copy();
            spec.affection = CardAffection.TeamNear;
            spec.Cost = spec.Cost+ 4;
            card.XmlData.Spec = spec;
            card._curCost = spec.Cost;
            ChangeCards.Add(card);
        }
        public static void ChangeBack(BattleDiceCardModel card)
        {
            if(card._originalXmlData!=null)
                card.ResetToOriginalData();  
            card._curCost = card.GetSpec().Cost;
            ChangeCards.Remove(card);
        }
    }
}