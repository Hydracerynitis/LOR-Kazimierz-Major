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
        public void SuckLight()
        {
            if (owner.PlayPoint <= 0)
            {
                List<BattlePlayingCardDataInUnitModel> eat = new List<BattlePlayingCardDataInUnitModel>();
                foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                {
                    BattlePlayingCardDataInUnitModel card;
                    if (owner.currentDiceAction!=null)
                        card = unit.cardSlotDetail.cardAry.Find(x => x != null && x != owner.currentDiceAction.target.currentDiceAction && !x.isDestroyed);
                    else
                        card = unit.cardSlotDetail.cardAry.Find(x => x != null && !x.isDestroyed);
                    if (card != null)
                        eat.Add(card);
                }
                foreach (BattlePlayingCardDataInUnitModel card in eat)
                {
                    owner.cardSlotDetail.RecoverPlayPoint(card.card.GetCost());
                    card.DestroyPlayingCard();
                }
                SoundEffectPlayer.PlaySound("Creature/Bigbird_Eyes");
            }
            LightIndicator.RefreshLight(owner);
        }
        public override void OnStartBattle()
        {
            SuckLight();
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            SuckLight();
        }
    }
    public class LightIndicator: BattleUnitBuf
    {
        public override string keywordId => "LightIndicator";
        public override string keywordIconId => "ChargeLight";
        public void RefreshLight()
        {
            stack = _owner.PlayPoint;
            KazimierInitializer.UpdateInfo(_owner);
        }
        public static void RefreshLight(BattleUnitModel model)
        {
            if (model.bufListDetail.GetActivatedBufList().Find(x => x is LightIndicator) is LightIndicator battleUnitBufBlessing)
                battleUnitBufBlessing.RefreshLight();
        }
    }
}
