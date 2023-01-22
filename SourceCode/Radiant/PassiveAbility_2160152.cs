using UnityEngine;
using BaseMod;
using LOR_DiceSystem;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2160152 : PassiveAbilityBase
    {
        private int Emotion = 0;
        public override void OnRoundStart()
        {
            for (; Emotion < owner.emotionDetail.EmotionLevel; Emotion++)
            {
                owner.personalEgoDetail.AddCard(Tools.MakeLorId(2161501 + Emotion));
            }
        }
        public static bool IsRadiantCard(BattleDiceCardModel card)
        {
            LorId id = card.GetID();
            return id.packageId == "KazimierMajor" && id.id >= 2160501 && id.id <= 2160516;
        }
        public static LorId GetRadiantCard(List<BattleDiceCardModel> AllDeck,int[] cardList)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < cardList.Length; i++)
                if (!AllDeck.Exists(x => x.GetID() == Tools.MakeLorId(cardList[i])))
                    list.Add(cardList[i]);
            return Tools.MakeLorId(RandomUtil.SelectOne(list));
        }
    }
    public class DiceCardSelfAbility_ReplaceRadiant1 : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.allyCardDetail.GetHand().Exists(x => x.GetOriginCost() == 2 && !PassiveAbility_2160152.IsRadiantCard(x));
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            unit.allyCardDetail.ExhaustACard(RandomUtil.SelectOne(owner.allyCardDetail.GetHand().FindAll(x => x.GetOriginCost() == 2 && !PassiveAbility_2160152.IsRadiantCard(x))));
            BattleDiceCardModel card = owner.allyCardDetail.AddNewCard(PassiveAbility_2160152.GetRadiantCard(owner.allyCardDetail.GetAllDeck(), PassiveAbility_2160052.WeakCard));
            card.CopySelf();
            DiceCardSpec spec = card.XmlData.Spec.Copy();
            spec.Cost = 2;
            card.XmlData.Spec = spec;
            card._curCost = 2;
        }
    }
    public class DiceCardSelfAbility_ReplaceRadiant2 : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.allyCardDetail.GetHand().Exists(x => x.GetOriginCost() == 3 && !PassiveAbility_2160152.IsRadiantCard(x));
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            unit.allyCardDetail.ExhaustACard(RandomUtil.SelectOne(owner.allyCardDetail.GetHand().FindAll(x => x.GetOriginCost() == 3 && !PassiveAbility_2160152.IsRadiantCard(x))));
            BattleDiceCardModel card = owner.allyCardDetail.AddNewCard(PassiveAbility_2160152.GetRadiantCard(owner.allyCardDetail.GetAllDeck(), PassiveAbility_2160052.StrongCard));
            card.CopySelf();
            DiceCardSpec spec = card.XmlData.Spec.Copy();
            spec.Cost = 3;
            card.XmlData.Spec = spec;
            card._curCost = 3;
        }
    }
    public class DiceCardSelfAbility_ReplaceRadiant3 : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.allyCardDetail.GetHand().Exists(x => x.GetOriginCost() == 3 && !PassiveAbility_2160152.IsRadiantCard(x));
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            unit.allyCardDetail.ExhaustACard(RandomUtil.SelectOne(owner.allyCardDetail.GetHand().FindAll(x => x.GetOriginCost() == 3 && !PassiveAbility_2160152.IsRadiantCard(x))));
            BattleDiceCardModel card = owner.allyCardDetail.AddNewCard(PassiveAbility_2160152.GetRadiantCard(owner.allyCardDetail.GetAllDeck(), PassiveAbility_2160052.StrongCard));
            card.CopySelf();
            DiceCardSpec spec = card.XmlData.Spec.Copy();
            spec.Cost = 3;
            card.XmlData.Spec = spec;
            card._curCost = 3;
            card = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2160501)));
            card.CopySelf();
            spec = card.XmlData.Spec.Copy();
            spec.Cost = 3;
            card.XmlData.Spec = spec;
            card._curCost = 3;
            unit.allyCardDetail._cardInDeck.Add(card);
            unit.allyCardDetail.Shuffle();
        }
    }
}
