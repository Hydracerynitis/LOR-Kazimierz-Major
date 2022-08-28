using UnityEngine;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_RadiantSpecial : DiceCardSelfAbilityBase
    {
        public override void OnApplyCard()
        {
            base.OnApplyCard();
            card.target.bufListDetail.AddBuf(new Indicator() { stack=0});
            KazimierInitializer.UpdateInfo(card.target);
        }
        public override void OnReleaseCard()
        {
            base.OnReleaseCard();
            card.target.bufListDetail.RemoveBufAll(typeof(Indicator));
            KazimierInitializer.UpdateInfo(card.target);
        }
        public class Indicator: BattleUnitBuf
        {
            public override string keywordId => "TargetIndicator";
            public override string keywordIconId => "Philip_Strong";
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                Destroy();
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
    public class DiceCardAbility_RadiantSum : DiceCardAbilityBase
    {
        public static List<BattleFarAreaPlayManager.VictimInfo> subTarget=new List<BattleFarAreaPlayManager.VictimInfo>();
        public override void BeforeRollDice()
        {
            base.BeforeRollDice();
            subTarget.Clear();
            foreach(BattleFarAreaPlayManager.VictimInfo v in BattleFarAreaPlayManager.Instance.victims)
            {
                if (card.subTargets.Exists(x => x.target == v.unitModel))
                    subTarget.Add(v);
            }
            BattleFarAreaPlayManager.Instance.victims.RemoveAll(x => subTarget.Contains(x));
        }
        public override void OnAfterAreaAtk(List<BattleUnitModel> damagedList, List<BattleUnitModel> defensedList)
        {
            base.OnAfterAreaAtk(damagedList, defensedList);
            if (damagedList.Contains(BattleFarAreaPlayManager.Instance.victims[0].unitModel))
                BattleFarAreaPlayManager.Instance.victims[0].cardDestroyed = true;
            BattleFarAreaPlayManager.Instance.victims.AddRange(subTarget);
        }
    }
    public class DiceCardAbility_RadiantSingle : DiceCardAbilityBase
    {
        public static BattleFarAreaPlayManager.VictimInfo target;
        public override void BeforeRollDice()
        {
            base.BeforeRollDice();
            foreach (BattleFarAreaPlayManager.VictimInfo v in BattleFarAreaPlayManager.Instance.victims)
            {
                if (card.target == v.unitModel)
                {
                    target = v;
                    BattleFarAreaPlayManager.Instance.victims.Remove(v);
                    return;
                }
            }
            
        }
        public override void OnAfterAreaAtk(List<BattleUnitModel> damagedList, List<BattleUnitModel> defensedList)
        {
            base.OnAfterAreaAtk(damagedList, defensedList);
            BattleFarAreaPlayManager.Instance.victims.Add(target);
        }
    }
}
