using UnityEngine;
using System.Collections.Generic;
using LOR_DiceSystem;
using Sound;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2160053 : PassiveAbilityBase
    {
        public static Dictionary<LorId, List<ParryStruct>> ParryDict = new Dictionary<LorId, List<ParryStruct>>();
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            ParryDict.Clear();
            foreach(BattleDiceCardModel card in owner.allyCardDetail.GetHand())
            {
                if (KazimierInitializer.IsNotClashCard(card))
                    continue;
                ParryDict.Add(card.GetID(), new List<ParryStruct>());
                if (card.GetID() == Tools.MakeLorId(2160510) && owner.hp<owner.MaxHp/2)
                {
                    ParryDict[card.GetID()].Add(new ParryStruct() { index = 2, behaviour = RandomUtil.SelectOne(BehaviourDetail.Slash, BehaviourDetail.Hit, BehaviourDetail.Penetrate) });
                    ParryDict[card.GetID()].Add(new ParryStruct() { index = Random.Range(0, 2), behaviour = RandomUtil.SelectOne(BehaviourDetail.Slash, BehaviourDetail.Hit, BehaviourDetail.Penetrate) });
                    continue;
                }
                else if(card.GetID()== Tools.MakeLorId(2160513))
                {
                    ParryDict[card.GetID()].Add(new ParryStruct() { index = 0, behaviour = RandomUtil.SelectOne(BehaviourDetail.Slash, BehaviourDetail.Hit, BehaviourDetail.Penetrate) });
                    continue;
                }
                int index = Random.Range(0, card.GetBehaviourList().Count);
                if(card.GetID()== Tools.MakeLorId(2160516) && index==1)
                    ParryDict[card.GetID()].Add(new ParryStruct() { index = index, behaviour = BehaviourDetail.None });
                else
                    ParryDict[card.GetID()].Add(new ParryStruct() { index = index, behaviour = RandomUtil.SelectOne(BehaviourDetail.Slash, BehaviourDetail.Hit, BehaviourDetail.Penetrate) });
            }
        }
    }
    public struct ParryStruct
    {
        public int index;
        public BehaviourDetail behaviour;
    }
    public class Parry: DiceCardAbilityBase
    {
        public static BattleCardBehaviourResult ParryTriggered;
        private BehaviourDetail bd = BehaviourDetail.Evasion;
        public Parry(BehaviourDetail detail)
        {
            bd = detail;
        }
        public override void BeforeRollDice()
        {
            base.BeforeRollDice();
            if (behavior.TargetDice != null)
            {
                if((bd==BehaviourDetail.None && behavior.TargetDice.Type==BehaviourType.Atk) || behavior.TargetDice.Detail == bd)
                {
                    behavior.TargetDice.SetBlocked(true);
                    behavior.TargetDice.forbiddenBonusDice = true;
                    behavior.TargetDice.card.DestroyDice(DiceMatch.NextDice,DiceUITiming.AttackAfter);
                    owner.breakDetail.ResetBreakDefault();
                    if (StageController.Instance._allCardList.Find(x => x.card.GetID()==Tools.MakeLorId(2160508)) is BattlePlayingCardDataInUnitModel Die)
                    {
                        Die.DestroyPlayingCard();
                        Die.GetDiceBehaviorList().ForEach(x => behavior.card.AddDice(x));
                    }
                    behavior.abilityList.FindAll(x => x is ParryAbility).ForEach(x => (x as ParryAbility).TriggerParry());
                    ParryTriggered = owner.battleCardResultLog.CurbehaviourResult;
                }
            }
        }
    }
    public interface ParryAbility
    {
        public void TriggerParry();
    }
}
