using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160023 :PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (!unit.bufListDetail.GetActivatedBufList().Exists(x => x is BountySearcher) && unit!=owner)
                    unit.bufListDetail.AddBuf(new BountySearcher());
            }
        }
        public override void OnDie()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                unit.bufListDetail.RemoveBufAll(typeof(Bounty));
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (curCard.earlyTarget != curCard.target)
            {
                Bounty buf = curCard.target.bufListDetail.GetActivatedBufList().Find(x => x is Bounty) as Bounty;
                if (buf == null)
                {
                    buf = new Bounty();
                    curCard.target.bufListDetail.AddBuf(buf);
                }
                buf.Add(1);
            }
        }
        public class BountySearcher: BattleUnitBuf
        {
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                Bounty bounty = card.target.bufListDetail.GetActivatedBufList().Find(x => x is Bounty) as Bounty;
                if (bounty != null)
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = bounty.stack });
            }
        }
        public class Bounty: BattleUnitBuf
        {
            protected override string keywordId => "Bounty";
            protected override string keywordIconId => "NicolaiTarget";
            public void Add(int count) => stack += count;
            public override void Init(BattleUnitModel owner)
            {
                stack = 0;
            }
        }
    }
}
