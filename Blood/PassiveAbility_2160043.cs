using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            base.AfterTakeDamage(attacker, dmg);
            if (owner.IsBreakLifeZero() || !Dmg.ContainsKey(attacker))
                return;
            Dmg[attacker] += dmg;
            if (Dmg[attacker] > 50 && !triggered.Contains(attacker))
            {
                DiceCardSelfAbility_BloodStun cardability = new DiceCardSelfAbility_BloodStun();
                BattlePlayingCardDataInUnitModel card = new BattlePlayingCardDataInUnitModel() { owner = owner, card = BloodStun, cardAbility= cardability };
                cardability.card = card;
                Singleton<StageController>.Instance.AddAllCardListInBattle(card, attacker);
                triggered.Add(attacker);
            }
        }
    }
}
