using UnityEngine;
using System.Collections.Generic;
using Sound;

namespace KazimierzMajor
{
    public class PassiveAbility_2160011 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            owner.emotionDetail.SetEmotionLevel(5);
            owner.cardSlotDetail.SetRecoverPoint(0);
            owner.bufListDetail.AddBuf(new LightIndicator());
            LightIndicator.RefreshLight(owner);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (owner.PlayPoint <= 0)
            {
                List<BattlePlayingCardDataInUnitModel> eat = new List<BattlePlayingCardDataInUnitModel>();
                foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                {
                    BattlePlayingCardDataInUnitModel card = unit.cardSlotDetail.cardAry.Find(x => x != null && x != curCard.target.currentDiceAction);
                    if (card != null)
                        eat.Add(card);
                }
                foreach(BattlePlayingCardDataInUnitModel card in eat)
                {
                    owner.cardSlotDetail.RecoverPlayPoint(card.card.GetCost());
                    card.DestroyPlayingCard();
                }
                SoundEffectPlayer.PlaySound("Creature/Bigbird_Eyes");
            }
            LightIndicator.RefreshLight(owner);
        }
    }
    public class LightIndicator: BattleUnitBuf
    {
        protected override string keywordId => "LightIndicator";
        protected override string keywordIconId => "ChargeLight";
        public void RefreshLight()
        {
            stack = _owner.PlayPoint;
            Harmony_Patch.UpdateInfo(_owner);
        }
        public static void RefreshLight(BattleUnitModel model)
        {
            if (model.bufListDetail.GetActivatedBufList().Find(x => x is LightIndicator) is LightIndicator battleUnitBufBlessing)
                battleUnitBufBlessing.RefreshLight();
        }
    }
}
