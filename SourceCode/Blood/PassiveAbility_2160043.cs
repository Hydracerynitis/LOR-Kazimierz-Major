using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using BaseMod;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160043 :PassiveAbilityBase
    {
        private static BattleDiceCardModel BloodStun= BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2160405)));
        private Dictionary<BattleUnitModel, int> Dmg= new Dictionary<BattleUnitModel, int>();
        private List<BattleUnitModel> triggered = new List<BattleUnitModel>();
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            Dmg.Clear();
            triggered.Clear();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                Dmg.Add(unit, 0);
            }
        }
        public override void AfterTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (owner.IsBreakLifeZero() || !Dmg.ContainsKey(attacker) || attacker==null || attacker == owner)
                return;
            Dmg[attacker] += dmg;
            if (Dmg[attacker] > 50 && !triggered.Contains(attacker))
            {
                DiceCardSelfAbility_BloodStun cardability = new DiceCardSelfAbility_BloodStun();
                BattlePlayingCardDataInUnitModel card = new BattlePlayingCardDataInUnitModel() { owner = owner, card = BloodStun, cardAbility= cardability,target=attacker, targetSlotOrder= RandomUtil.Range(0, attacker.cardSlotDetail.cardAry.Count - 1) };
                cardability.card = card;
                card.ResetCardQueueWithoutStandby();
                Singleton<StageController>.Instance.GetAllCards().Insert(0,card);
                triggered.Add(attacker);
            }
        }
    }
}
