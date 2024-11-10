using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using LOR_DiceSystem;
using UnityEngine;

namespace KazimierzMajor
{
    [HarmonyPatch]
    static class LateAttackHandler
    {
        public static List<BattlePlayingCardDataInUnitModel> LateCards = new List<BattlePlayingCardDataInUnitModel>();
    }
    static class FastLateAttack
    {
        public static List<BattlePlayingCardDataInUnitModel> FastCards = new List<BattlePlayingCardDataInUnitModel>();
        

        [HarmonyPatch(typeof(FastLateAttack))]
        [HarmonyPatch(typeof(StageController),nameof(StageController.ActivateStartBattleEffectPhase))]
        [HarmonyPrefix]
        public static void StageController_ActivateStartBattleEffectPhase(List<BattlePlayingCardDataInUnitModel> ____allCardList)
        {
            FastCards.Clear();
            PassiveAbility_2160127.owners.FindAll(x => x != null && x.cardSlotDetail?.cardQueue?.Count > 0).ForEach(x => FastCards.Add(x.cardSlotDetail?.cardAry?.Find(x => x != null)));
            List<BattlePlayingCardDataInUnitModel> LateAttack = ____allCardList.FindAll(x => IsLateAttack(x) || IsLateAttack(GetParry(x)));
            ____allCardList.RemoveAll(x => LateAttack.Contains(x));
            List<BattlePlayingCardDataInUnitModel> FastAttack = ____allCardList.FindAll(x => IsFastAttack(x) || IsFastAttack(GetParry(x)));
            ____allCardList.RemoveAll(x => FastAttack.Contains(x));
            FastAttack.ForEach(x => ____allCardList.Insert(0, x));
            ____allCardList.AddRange(LateAttack);
        }
        [HarmonyPatch(typeof(StageController),nameof(StageController.MoveUnitPhase))]
        [HarmonyPrefix]
        public static bool StageController_MoveUnitPhase(ref StageController.StagePhase ____phase)
        {
            /*List<BattleUnitModel> battleUnitModelList1 = new List<BattleUnitModel>();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive.turnState == BattleUnitTurnState.BREAK || alive.currentDiceAction == null || alive.currentDiceAction.cardBehaviorQueue.Count == 0)
                    alive.moveDetail.Stop();
                else
                    battleUnitModelList1.Add(alive);
            }
            if (battleUnitModelList1.Count == 0)
                ____phase = StageController.StagePhase.RoundEndPhase;*/
            else
            {
                List<BattleUnitModel> LateAttack = battleUnitModelList1.FindAll(x => IsLateAttack(x.currentDiceAction) || IsLateAttack(GetParry(x.currentDiceAction)));
                if (battleUnitModelList1.Count - LateAttack.Count <= 0)
                    battleUnitModelList1 = LateAttack;
                else
                    battleUnitModelList1.RemoveAll(x => LateAttack.Contains(x));
                battleUnitModelList1.Sort((u1, u2) =>
                {
                    BattlePlayingCardDataInUnitModel currentDiceAction1 = u1.currentDiceAction;
                    BattlePlayingCardDataInUnitModel currentDiceAction2 = u2.currentDiceAction;
                    if (IsFastAttack(currentDiceAction1) || IsFastAttack(GetParry(currentDiceAction1)))
                    {
                        if (IsFastAttack(currentDiceAction2) || IsFastAttack(GetParry(currentDiceAction2)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return -1;
                    }
                    if (IsFastAttack(currentDiceAction2) || IsFastAttack(GetParry(currentDiceAction2)))
                    {
                        if (IsFastAttack(currentDiceAction1) || IsFastAttack(GetParry(currentDiceAction1)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return 1;
                    }
                    if (currentDiceAction1.card.GetSpec().Ranged == CardRange.Far)
                    {
                        if (currentDiceAction2.card.GetSpec().Ranged != CardRange.Far)
                            return -1;
                        if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                            return 0;
                        return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                    }
                    if (currentDiceAction2.card.GetSpec().Ranged == CardRange.Far)
                        return 1;
                    if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                        return 0;
                    return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                });
                /*int a = -1;
                for (int index = 0; index < battleUnitModelList1.Count; ++index)
                {
                    if (battleUnitModelList1[index].currentDiceAction.speedDiceResultValue > a)
                        a = battleUnitModelList1[index].currentDiceAction.speedDiceResultValue;
                }
                int num1 = Mathf.Min(a, 10);
                List<BattleUnitModel> battleUnitModelList2 = new List<BattleUnitModel>();
                List<BattleUnitModel> battleUnitModelList3 = new List<BattleUnitModel>();*/
                /*for (int index = 0; index < battleUnitModelList1.Count; ++index)
                {
                    BattleUnitModel unit = battleUnitModelList1[index];
                    BattleUnitModel target = unit.currentDiceAction.target;
                    if (target == null || target.IsDead() || target.IsExtinction())
                        unit.currentDiceAction = null;
                    else if (unit.turnState != BattleUnitTurnState.BREAK)*/
                    {
                        if (unit.currentDiceAction.card.GetSpec().Ranged == CardRange.Far && !IsLateAttack(unit.currentDiceAction.target?.currentDiceAction))
                        {
                            unit.moveDetail.Stop();
                            unit.currentDiceAction?.GetDiceBehaviorList();
                            battleUnitModelList2.Add(target);
                        }
                        /*else if (unit.currentDiceAction.card.GetSpec().Ranged == CardRange.Special)
                        {
                            unit.moveDetail.Stop();
                            battleUnitModelList3.Add(target);
                        }*/
                        else if (IsFastAttack(unit.currentDiceAction))
                        {
                            unit.moveDetail.Stop();
                            battleUnitModelList3.Add(target);
                        }
                        /*else
                        {
                            BattleDiceCardModel card = unit.currentDiceAction.target?.currentDiceAction?.card;
                            bool flag = false;
                            if (card != null && card.GetSpec().Ranged == CardRange.Far)
                            {
                                List<DiceBehaviour> behaviourList = card.GetBehaviourList();
                                if (behaviourList.Count > 0 && behaviourList[0].Type == BehaviourType.Atk)
                                    flag = true;
                            }
                            if (flag)
                                unit.moveDetail.Stop();
                            else if (battleUnitModelList2.Exists(x => x == unit))
                                unit.moveDetail.Stop();
                            else if (battleUnitModelList3.Exists(x => x == unit))
                                unit.moveDetail.Stop();
                            else
                            {
                                float num2 = 1f;
                                int num3 = Mathf.Min(unit.currentDiceAction.speedDiceResultValue, 10);
                                if (num3 < num1)
                                    num2 = (float)((double)num3 / (double)num1 * 0.5);
                                float speed = 60f * num2;
                                if ((double)Vector3.Distance(unit.view.WorldPosition, target.view.WorldPosition) <= (double)unit.moveDetail.GetDistanceBetweenDstAndTarget(unit, target) + 4.0)
                                {
                                    unit.moveDetail.Stop();
                                    target.moveDetail.Stop();
                                }
                                else
                                    unit.moveDetail.Move(target, speed);
                            }
                        }
                        unit.UpdateDirection(target.view.WorldPosition);*/
                    /*}
                    else
                    {
                        unit.currentDiceAction = null;
                        unit.moveDetail.Stop();
                    }
                }
                StageController.Instance.phase = StageController.StagePhase.WaitUnitsArrive;
                SingletonBehavior<BattleCamManager>.Instance.StartMoveUnits();*/
            }
            return false;
        }
        [HarmonyPatch(typeof(StageController),nameof(StageController.WaitUnitArrivePhase))]
        [HarmonyPrefix]
        public static bool StageController_WaitUnitArrivePhase(ref StageController.StagePhase ____phase)
        {
            //List<BattleUnitModel> battleUnitModelList1 = new List<BattleUnitModel>();
            /*foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive.turnState == BattleUnitTurnState.BREAK || alive.currentDiceAction == null || alive.currentDiceAction.cardBehaviorQueue.Count == 0)
                    alive.moveDetail.Stop();
                else
                    battleUnitModelList1.Add(alive);
            }
            if (battleUnitModelList1.Count == 0)
                ____phase = StageController.StagePhase.RoundEndPhase;*/
            else
            {
                List<BattleUnitModel> LateAttack = battleUnitModelList1.FindAll(x => IsLateAttack(x.currentDiceAction) || IsLateAttack(GetParry(x.currentDiceAction)));
                if (battleUnitModelList1.Count - LateAttack.Count <= 0)
                    battleUnitModelList1 = LateAttack;
                else
                    battleUnitModelList1.RemoveAll(x => LateAttack.Contains(x));
                battleUnitModelList1.Sort((u1, u2) =>
                {
                    BattlePlayingCardDataInUnitModel currentDiceAction1 = u1.currentDiceAction;
                    BattlePlayingCardDataInUnitModel currentDiceAction2 = u2.currentDiceAction;
                    if (IsFastAttack(currentDiceAction1) || IsFastAttack(GetParry(currentDiceAction1)))
                    {
                        if (IsFastAttack(currentDiceAction2) || IsFastAttack(GetParry(currentDiceAction2)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return -1;
                    }
                    if (IsFastAttack(currentDiceAction2) || IsFastAttack(GetParry(currentDiceAction2)))
                    {
                        if (IsFastAttack(currentDiceAction1) || IsFastAttack(GetParry(currentDiceAction1)))
                        {
                            if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                                return 0;
                            return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                        }
                        else
                            return 1;
                    }
                    if (currentDiceAction1.card.GetSpec().Ranged == CardRange.Far)
                    {
                        if (currentDiceAction2.card.GetSpec().Ranged != CardRange.Far)
                            return -1;
                        if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                            return 0;
                        return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                    }
                    if (currentDiceAction2.card.GetSpec().Ranged == CardRange.Far)
                        return 1;
                    if (currentDiceAction1.speedDiceResultValue == currentDiceAction2.speedDiceResultValue)
                        return 0;
                    return currentDiceAction1.speedDiceResultValue > currentDiceAction2.speedDiceResultValue ? -1 : 1;
                });
                /*int num1 = -1;
                List<BattleUnitModel> battleUnitModelList2 = new List<BattleUnitModel>();
                for (int index = 0; index < battleUnitModelList1.Count; ++index)
                {
                    if (battleUnitModelList1[index].currentDiceAction.speedDiceResultValue > num1)
                    {
                        num1 = battleUnitModelList1[index].currentDiceAction.speedDiceResultValue;
                        battleUnitModelList2.Add(battleUnitModelList1[index]);
                        List<BattleUnitModel> battleUnitModelList3 = new List<BattleUnitModel>();
                        foreach (BattleUnitModel battleUnitModel in battleUnitModelList2)
                        {
                            if (battleUnitModel.currentDiceAction.speedDiceResultValue < num1)
                                battleUnitModelList3.Add(battleUnitModel);
                        }
                        foreach (BattleUnitModel battleUnitModel in battleUnitModelList3)
                            battleUnitModelList2.Remove(battleUnitModel);
                    }
                    else if (battleUnitModelList1[index].currentDiceAction.speedDiceResultValue == num1 && num1 > -1)
                        battleUnitModelList2.Add(battleUnitModelList1[index]);
                }*/
                /*BattleUnitModel arrivedUnit = null;
                for (int index = 0; index < battleUnitModelList1.Count; ++index)
                {
                    BattleUnitModel unit = battleUnitModelList1[index];
                    BattleUnitModel target = unit.currentDiceAction.target;
                    if (target != null)
                    {
                        if (unit.moveDetail.isArrived && !unit.moveDetail.isKnockback)
                        {
                            if (battleUnitModelList2.Exists(x => x == unit))
                            {
                                arrivedUnit = unit;
                                break;
                            }
                            if (unit.currentDiceAction.card.GetSpec().Ranged == CardRange.Special)
                            {
                                arrivedUnit = unit;
                                break;
                            }*/
                            if (IsFastAttack(unit.currentDiceAction))
                            {
                                arrivedUnit = unit;
                                break;
                            }
                            if (unit.currentDiceAction.card.GetSpec().Ranged == CardRange.Far && !IsLateAttack(unit.currentDiceAction.target.currentDiceAction!))
                            {
                                arrivedUnit = unit;
                                break;
                            }
                        /*}
                        else if (!unit.moveDetail.isKnockback)
                        {
                            float num2 = 1f;
                            int num3 = Mathf.Min(unit.currentDiceAction.speedDiceResultValue, 10);
                            if (num3 < num1)
                                num2 = (float)((double)num3 / (double)num1 * 0.5);
                            if ((double)Vector3.Distance(unit.view.WorldPosition, target.view.WorldPosition) <= (double)unit.moveDetail.GetDistanceBetweenDstAndTarget(unit, target) + 4.0)
                            {
                                unit.moveDetail.Stop();
                                target.moveDetail.Stop();
                            }
                        }
                    }
                }*/
                /*if (arrivedUnit == null)
                    return false;
                for (int index = 0; index < battleUnitModelList1.Count; ++index)
                {
                    if (battleUnitModelList1[index] != arrivedUnit && !battleUnitModelList1[index].moveDetail.isKnockback)
                        battleUnitModelList1[index].moveDetail.MoveSlow(true);
                }
                int slotOrder = arrivedUnit.currentDiceAction.slotOrder;
                BattleUnitModel target1 = arrivedUnit.currentDiceAction.target;
                int targetSlotOrder = arrivedUnit.currentDiceAction.targetSlotOrder;
                BattlePlayingCardDataInUnitModel cardB = null;
                bool flag = false;
                try
                {
                    cardB = target1.cardSlotDetail.cardAry[targetSlotOrder];
                    if (cardB != null)
                    {
                        int? count = cardB.cardBehaviorQueue?.Count;
                        int num2 = 0;
                        if (count.GetValueOrDefault() > num2 & count.HasValue)
                        {
                            if (cardB.isDestroyed)
                                flag = false;
                            else if (cardB.card.GetSpec().affection == CardAffection.TeamNear)
                            {
                                BattlePlayingCardDataInUnitModel.SubTarget subTarget = cardB.subTargets.Find(x => x.target == arrivedUnit && x.targetSlotOrder == slotOrder);
                                if (subTarget != null)
                                {
                                    cardB.subTargets.Add(new BattlePlayingCardDataInUnitModel.SubTarget()
                                    {
                                        target = cardB.target,
                                        targetSlotOrder = cardB.targetSlotOrder
                                    });
                                    cardB.target = subTarget.target;
                                    cardB.targetSlotOrder = subTarget.targetSlotOrder;
                                    cardB.subTargets.Remove(subTarget);
                                }
                                if (cardB.target == arrivedUnit)
                                {
                                    if (cardB.targetSlotOrder == slotOrder)
                                    {
                                        if (!arrivedUnit.IsBreakLifeZero())
                                        {
                                            if (!target1.IsBreakLifeZero())
                                                flag = true;
                                        }
                                    }
                                }
                            }
                            else if (cardB.target == arrivedUnit)
                            {
                                if (cardB.targetSlotOrder == slotOrder)
                                {
                                    if (!arrivedUnit.IsBreakLifeZero())
                                    {
                                        if (!target1.IsBreakLifeZero())
                                            flag = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
                if (flag)
                {
                    target1.moveDetail.MoveSlow(false);
                    for (int index = 0; index < BattleObjectManager.instance.GetList().Count; ++index)
                    {
                        BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetList()[index];
                        if (battleUnitModel != target1 && battleUnitModel != arrivedUnit && !battleUnitModel.IsDead())
                            battleUnitModel.view.charAppearance.ChangeColor(Color.grey);
                    }
                    Singleton<StageController>.Instance.sp(arrivedUnit.currentDiceAction, cardB);
                }
                else if (target1.cardSlotDetail.keepCard.cardBehaviorQueue.Count > 0)
                {
                    target1.moveDetail.MoveSlow(false);
                    for (int index = 0; index < BattleObjectManager.instance.GetList().Count; ++index)
                    {
                        BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetList()[index];
                        if (battleUnitModel != target1 && battleUnitModel != arrivedUnit && !battleUnitModel.IsDead())
                            battleUnitModel.view.charAppearance.ChangeColor(Color.grey);
                    }
                    target1.cardSlotDetail.keepCard.target = arrivedUnit;
                    BattleDiceCardModel card = arrivedUnit.currentDiceAction.card;
                    if ((card != null ? (card.GetSpec().Ranged == CardRange.Far ? 1 : 0) : 0) != 0)
                        target1.moveDetail.Stop();
                    else
                        target1.moveDetail.Move(arrivedUnit, 15f);
                    Singleton<StageController>.Instance.StartParrying(arrivedUnit.currentDiceAction, target1.cardSlotDetail.keepCard);
                }
                else
                {
                    if (arrivedUnit.currentDiceAction.target != null)
                        arrivedUnit.currentDiceAction.target.moveDetail.Stop();
                    for (int index = 0; index < BattleObjectManager.instance.GetList().Count; ++index)
                    {
                        BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetList()[index];
                        if (battleUnitModel != target1 && battleUnitModel != arrivedUnit && !battleUnitModel.IsDead())
                            battleUnitModel.view.charAppearance.ChangeColor(Color.grey);
                    }
                    StageController.Instance.StartAction(arrivedUnit.currentDiceAction);
                }
            }*/
            return false;
        }
        public static BattlePlayingCardDataInUnitModel GetParry(BattlePlayingCardDataInUnitModel card)
        {
            try
            {
                BattlePlayingCardDataInUnitModel oppoist = card.target.cardSlotDetail.cardAry[card.targetSlotOrder];
                if (oppoist.owner.DirectAttack() || card.owner.DirectAttack())
                    return null;
                if (oppoist.target == card.owner && oppoist.targetSlotOrder == card.slotOrder)
                    return oppoist;
                else
                    return null;
            }
            catch
            {

            }
            return null;
        }
        public static bool IsLateAttack(BattlePlayingCardDataInUnitModel card)
        {
            if (card == null)
                return false;
            return card.cardAbility is DiceCardSelfAbility_LateAttack;
        }
        public static bool IsFastAttack(BattlePlayingCardDataInUnitModel card)
        {
            if (card == null)
                return false;
            if (FastCards.Contains(card))
                return true;
            if (card.cardAbility is DiceCardSelfAbility_OneSideFastAttack && GetParry(card) == null)
                return true;
            if (card == PassiveAbility_2160031.nightmare)
                return true;
            return card.cardAbility is DiceCardSelfAbility_FastAttack;
        }
    }
}
