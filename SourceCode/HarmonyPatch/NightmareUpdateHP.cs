using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;
using Sound;


namespace KazimierzMajor
{
    [HarmonyPatch]
    class NightmareUpdateHP
    {
        [HarmonyPatch(typeof(BattleUnitBreakDetail), nameof(BattleUnitBreakDetail.TakeBreakDamage))]
        [HarmonyPostfix]
        static void BattleUnitBreakDetail_TakeBreakDamage_Post(BattleUnitBreakDetail __instance)
        {
            foreach (PassiveAbilityBase passive in __instance._self.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance._self.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeBreak());
                    else
                        (passive as NightmareUpdater).AfterChangeBreak();
                }
        }
        [HarmonyPatch(typeof(BattleUnitBreakDetail), nameof(BattleUnitBreakDetail.RecoverBreak))]
        [HarmonyPostfix]
        static void BattleUnitBreakDetail_RecoverBreak_Post(BattleUnitBreakDetail __instance)
        {
            foreach (PassiveAbilityBase passive in __instance._self.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance._self.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeBreak());
                    else
                        (passive as NightmareUpdater).AfterChangeBreak();
                }
        }
        [HarmonyPatch(typeof(BattleUnitBreakDetail), nameof(BattleUnitBreakDetail.LoseBreakGauge))]
        [HarmonyPostfix]
        static void BattleUnitBreakDetail_LoseBreakGauge_Post(BattleUnitBreakDetail __instance)
        {
            foreach (PassiveAbilityBase passive in __instance._self.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance._self.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeBreak());
                    else
                        (passive as NightmareUpdater).AfterChangeBreak();
                }
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.RecoverHP))]
        [HarmonyPostfix]
        static void BattleUnitModel_RecoverHP_Post(BattleUnitModel __instance)
        {
            foreach (PassiveAbilityBase passive in __instance.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeHp());
                    else
                        (passive as NightmareUpdater).AfterChangeHp();
                }
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.TakeDamage))]
        [HarmonyPostfix]
        static void BattleUnitModel_TakeDamage_Post(BattleUnitModel __instance)
        {
            foreach (PassiveAbilityBase passive in __instance.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeHp());
                    else
                        (passive as NightmareUpdater).AfterChangeHp();
                }
            if (__instance.bufListDetail.FindBuf<BattleUnitBuf_Shield>() is BattleUnitBuf_Shield s)
            {
                s.Reduce();
                if (Singleton<StageController>.Instance.IsLogState())
                    __instance.battleCardResultLog?.SetCreatureEffectSound("Creature/Greed_StrongAtk_Defensed");
                else
                    SoundEffectPlayer.PlaySound("Creature/Greed_StrongAtk_Defensed");
            }
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.LoseHp))]
        [HarmonyPostfix]
        static void BattleUnitModel_LoseHp_Post(BattleUnitModel __instance)
        {
            foreach (PassiveAbilityBase passive in __instance.passiveDetail.PassiveList)
                if (passive is NightmareUpdater)
                {
                    if (StageController.Instance.IsLogState())
                        __instance.battleCardResultLog?.SetPrintDamagedEffectEvent(() => (passive as NightmareUpdater).AfterChangeHp());
                    else
                        (passive as NightmareUpdater).AfterChangeHp();
                }
        }
    }
}
