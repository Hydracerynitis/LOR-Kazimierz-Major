using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OB = UnityEngine.Object;
using UnityEngine;
using UnityEngine.UI;

namespace KazimierzMajor
{
    [HarmonyPatch]
    class CandleHP
    {
        [HarmonyPatch(typeof(BattleUnitCostUI), nameof(BattleUnitCostUI.Awake))]
        [HarmonyPrefix]
        public static bool BattleUnitCostUI_Awake(BattleUnitCostUI __instance)
        {
            __instance.fixedMaxcost = 20;
            __instance.prefab_cost.SetActive(false);
            for (int index = 0; index < __instance.fixedMaxcost; ++index)
            {
                __instance.costLists.Add(new BattleUnitCostUI.CostBattleUIImage(OB.Instantiate<GameObject>(__instance.prefab_cost, index < 5 || index>14 ? __instance.upperLine : __instance.lowerLine)));
                __instance.costLists[index].parent.SetActive(false);
            }
            __instance._canvas = __instance.GetComponent<Canvas>();
            return false;
        }
        [HarmonyPatch(typeof(BattleUnitCostUI), nameof(BattleUnitCostUI.SetCurrentMaxCost))]
        [HarmonyPrefix]
        public static bool BattleUnitCostUI_SetCurrentMaxCost(BattleUnitCostUI __instance, int _maxcost)
        {
            float upperSize = 0.0f;
            float lowerSize = 0.0f;
            for (int index = 0; index < _maxcost; ++index)
            {
                if (index < 5 || index>14)
                {
                    upperSize += __instance.spacing;
                    __instance.upperLine.sizeDelta = new Vector2(upperSize, 0.0f);
                }
                else
                {
                    lowerSize += __instance.spacing;
                    __instance.lowerLine.sizeDelta = new Vector2(lowerSize, 0.0f);
                }
                __instance.costLists[index].parent.SetActive(true);
            }
            __instance.img_linebase[0].enabled = true;
            __instance.img_linebase[1].enabled = _maxcost > 5;
            __instance.SetHightlightedCurrentCostCard(0, 0);
            for (int index = _maxcost; index < __instance.fixedMaxcost; ++index)
                __instance.costLists[index].parent.SetActive(false);
            return false;
        }
        [HarmonyPatch(typeof(BattleUnitCostUI), nameof(BattleUnitCostUI.SetHightlightedCurrentCostCard))]
        [HarmonyPrefix]
        public static bool BattleUnitCostUI_SetHightlightedCurrentCostCard(BattleUnitCostUI __instance, int overlaycost, int currentequipcardCost)
        {
            if (__instance.costLists == null)
                return false;
            int index = __instance.currentcost - 1 + currentequipcardCost;
            if (__instance.currentcost + currentequipcardCost < overlaycost)
                return false;
            for (; index >= __instance.currentcost - overlaycost + currentequipcardCost; --index)
            {
                Image costOverlay = __instance.costLists[GetCostListIndex(index,__instance.currentmaxcost)].costOverlay;
                if (costOverlay != null)
                    costOverlay.enabled = true;
            }
            for (; index >= 0 && index < __instance.costLists.Count; --index)
            {
                Image costOverlay = __instance.costLists[GetCostListIndex(index, __instance.currentmaxcost)].costOverlay;
                if (costOverlay != null)
                    costOverlay.enabled = false;
            }
            return false;
        }
        [HarmonyPatch(typeof(BattleUnitCostUI), nameof(BattleUnitCostUI.SetCurrentCost))]
        [HarmonyPrefix]
        public static bool BattleUnitCostUI_SetCurrentCost(BattleUnitCostUI __instance, int currentcost)
        {
            int playPoint = __instance._view.model.PlayPoint;
            int index;
            for (index = 0; index < currentcost; ++index)
            {
                __instance.costLists[GetCostListIndex(index, __instance.currentmaxcost)].costCotent.enabled = true;
                if (__instance.costLists[GetCostListIndex(index, __instance.currentmaxcost)].costCotent.color == __instance._usedCostColor)
                    __instance.costLists[GetCostListIndex(index, __instance.currentmaxcost)].costanim.SetTrigger("Show");
                __instance.costLists[GetCostListIndex(index, __instance.currentmaxcost)].costCotent.color = __instance._remainedCostColor;
            }
            for (; index < playPoint; ++index)
            {
                __instance.costLists[GetCostListIndex(index, __instance.currentmaxcost)].costCotent.enabled = true;
                __instance.costLists[GetCostListIndex(index, __instance.currentmaxcost)].costCotent.color = __instance._usedCostColor;
                __instance.costLists[GetCostListIndex(index, __instance.currentmaxcost)].costanim.SetTrigger("Use");
            }
            for (; index < __instance.currentmaxcost; ++index)
                __instance.costLists[GetCostListIndex(index, __instance.currentmaxcost)].costanim.SetTrigger("Hide");
            return false;
        }
        public static int GetCostListIndex(int index, int maxCost)
        {
            if (maxCost <= 15)
                return index;
            else
            {
                int misAlignedUI = maxCost - 15;
                if (index <= 4)
                    return index;
                else if (index <= 4 + misAlignedUI)
                    return index + 10;
                else
                    return index - misAlignedUI;
            }
        }
    }
}
