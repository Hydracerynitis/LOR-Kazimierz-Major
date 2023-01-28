using System;
using System.Collections.Generic;
using BaseMod;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160142 : PassiveAbilityBase
    {
        private static BattleDiceCardModel Counter = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2161407)));
        private Dictionary<BattleUnitModel, int> Dmg = new Dictionary<BattleUnitModel, int>();
        private List<BattleUnitModel> triggered = new List<BattleUnitModel>();
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                Dmg.Add(unit, 0);
        }
        public override void OnRoundStart()
        {
            triggered.ForEach(x => Dmg[x] = 0);
            triggered.Clear();
        }
        public override void AfterTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (owner.IsBreakLifeZero() || !Dmg.ContainsKey(attacker) || attacker == null || attacker==owner)
                return;
            Dmg[attacker] += dmg;
            if (Dmg[attacker] > 20 && !triggered.Contains(attacker))
            {
                BattlePlayingCardDataInUnitModel card = new BattlePlayingCardDataInUnitModel() { owner = owner, card = Counter, target = attacker, targetSlotOrder = RandomUtil.Range(0, attacker.cardSlotDetail.cardAry.Count - 1) };
                card.ResetCardQueueWithoutStandby();
                Singleton<StageController>.Instance.GetAllCards().Insert(0, card);
                triggered.Add(attacker);
            }
        }
        public override void OnBreakState()
        {
            base.OnBreakState();
            foreach(BattleUnitModel enemy in Dmg.Keys)
                Dmg[enemy] = 0;
        }
    }
}
