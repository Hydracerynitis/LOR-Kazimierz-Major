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
        public BattleDiceCardModel GetBloodStun()
        {
            return BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(2161407)));
        }
        public override void AfterTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (owner.IsBreakLifeZero() || !Dmg.ContainsKey(attacker) || attacker==null || attacker == owner)
                return;
            Dmg[attacker] += dmg;
            if (Dmg[attacker] > 50 && !triggered.Contains(attacker))
            {
                BattlePlayingCardDataInUnitModel card = new BattlePlayingCardDataInUnitModel() { owner = owner, card = GetBloodStun() };
                StageController.Instance.AddAllCardListInBattle(card, attacker);
                triggered.Add(attacker);
            }
        }
    }
}
