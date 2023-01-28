using BaseMod;
using System.Collections.Generic;
using Sound;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2160111 : PassiveAbilityBase
    {
        private int LightSucked = 0;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.cardSlotDetail.LosePlayPoint(owner.cardSlotDetail.GetMaxPlayPoint());
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(2161113));
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.cardSlotDetail._playPoint = Math.Min(LightSucked+owner.cardSlotDetail._playPoint,owner.cardSlotDetail.GetMaxPlayPoint());
            LightSucked = 0;
        }
        public void SuckLight()
        {
            List<BattlePlayingCardDataInUnitModel> eat = new List<BattlePlayingCardDataInUnitModel>();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x.bufListDetail.HasBuf<SuckTarget>()))
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
            if (BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x.bufListDetail.HasBuf<SuckTarget>()).Count<=0)
                return;
            BattleStartCinematic.Cinematics.Enqueue(new BattleStartCinematic.CinematicData() { Instruction = SuckLightCinematic(), TimeFrame = 5f });
        }
        public IEnumerator SuckLightCinematic()
        {
            owner.view.charAppearance.ChangeMotion(ActionDetail.Aim);
            BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x.bufListDetail.HasBuf<SuckTarget>()).ForEach(x =>
            {
                BattlePlayingCardDataInUnitModel sucked = GetCardSucked(x);
                if (sucked != null)
                    x.view.diceActionUI.InitWithoutLog(sucked);
            });
            yield return YieldCache.WaitForSeconds(2f);
            owner.view.charAppearance.ChangeMotion(ActionDetail.Fire);
            SoundEffectPlayer.PlaySound("Creature/Bigbird_Eyes");
            yield return YieldCache.WaitForSeconds(1.5f);
            BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x.bufListDetail.HasBuf<SuckTarget>()).ForEach(x =>
            {
                x.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                x.view.diceActionUI.EndAction();
            });
            yield return YieldCache.WaitForSeconds(1f);
            SuckLight();
            owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
            BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x.bufListDetail.HasBuf<SuckTarget>()).ForEach(x => x.view.charAppearance.ChangeMotion(ActionDetail.Default));
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
