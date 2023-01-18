using HarmonyLib;
using System;
using SC = System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KazimierzMajor
{
    [HarmonyPatch]
    public class BattleStartCinematic
    {
        public static Queue<CinematicData> Cinematics = new Queue<CinematicData>();
        [HarmonyPatch(typeof(StageController), nameof(StageController.WaitStartBattleEffectPhase))]
        [HarmonyPrefix]
        public static bool StageController_WaitStartBattleEffectPhase()
        {
            if (Cinematics.Count > 0 && !BattleSceneRoot.Instance.currentMapObject.IsRunningEffect)
            {
                BattleSceneRoot.Instance.currentMapObject._bRunningEffect = true;
                BattleSceneRoot.Instance.StartCoroutine(PlayCinematics());
            }
            if (BattleSceneRoot.Instance.currentMapObject.IsRunningEffect)
                return false;
            return true;
        }
        public static SC.IEnumerator PlayCinematics()
        {
            while (Cinematics.Count > 0)
            {
                CinematicData data = Cinematics.Dequeue();
                BattleSceneRoot.Instance.StartCoroutine(data.Instruction);
                yield return YieldCache.WaitForSeconds(data.TimeFrame);
            }
            BattleSceneRoot.Instance.currentMapObject._bRunningEffect = false;
        }
        public struct CinematicData
        {
            public SC.IEnumerator Instruction;
            public float TimeFrame;
        }
    }
}
