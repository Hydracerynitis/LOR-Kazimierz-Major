using UnityEngine;
using System.Collections.Generic;
using Sound;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2160011 : PassiveAbilityBase
    {
        private int LightSucked = 0;
        public override void OnWaveStart()
        {
            owner.emotionDetail.SetEmotionLevel(5);
            owner.cardSlotDetail.SetRecoverPoint(0);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.cardSlotDetail.RecoverPlayPoint(LightSucked);
            LightSucked = 0;
        }
        public void SuckLight()
        {
            List<BattlePlayingCardDataInUnitModel> eat = new List<BattlePlayingCardDataInUnitModel>();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                eat.Add(GetCardSucked(unit));
            foreach (BattlePlayingCardDataInUnitModel card in eat)
            {
                if (card == null)
                    continue;
                LightSucked += card.card.GetCost();
                card.DestroyPlayingCard();
                card.owner.allyCardDetail.SpendCard(card.card);
                card.owner.cardSlotDetail.cardAry[card.owner.cardSlotDetail.cardAry.IndexOf(card)] = null;
            }
        }
        public override void OnStartBattle()
        {
            if (owner.cardSlotDetail.PlayPoint >= 20)
                return;
            BattleStartCinematic.Cinematics.Enqueue(new BattleStartCinematic.CinematicData() { Instruction = SuckLightCinematic(), TimeFrame = 5f });
        }
        public IEnumerator SuckLightCinematic()
        {
            owner.view.charAppearance.ChangeMotion(ActionDetail.Aim);
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).ForEach(x =>
            {
                BattlePlayingCardDataInUnitModel sucked = GetCardSucked(x);
                if (sucked != null)
                    x.view.diceActionUI.InitWithoutLog(sucked);
            });
            yield return YieldCache.WaitForSeconds(2f);
            owner.view.charAppearance.ChangeMotion(ActionDetail.Fire);
            SoundEffectPlayer.PlaySound("Creature/Bigbird_Eyes");
            yield return YieldCache.WaitForSeconds(1.5f);
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).ForEach(x =>
            {
                x.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                x.view.diceActionUI.EndAction();
            });
            yield return YieldCache.WaitForSeconds(1f);
            SuckLight();
            owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).ForEach(x => x.view.charAppearance.ChangeMotion(ActionDetail.Default));
            BattleSceneRoot.Instance.currentMapObject?.SetRunningState(false);
            yield break;
        }
        public BattlePlayingCardDataInUnitModel GetCardSucked(BattleUnitModel unit)
        {
            List<BattlePlayingCardDataInUnitModel> cards = new List<BattlePlayingCardDataInUnitModel>(unit.cardSlotDetail.cardAry);
            cards.Sort((x, y) =>
            {
                if (x == null && y == null)
                    return 0;
                else if (x == null)
                    return 1;
                else if (y == null)
                    return -1;
                else
                    return y.card.GetCost() - x.card.GetCost();
            });
            return cards[0];
        }
    }
}
