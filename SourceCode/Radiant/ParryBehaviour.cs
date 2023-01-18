using Battle.DiceAttackEffect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class ParryFar : BehaviourActionBase
    {
        public override List<RencounterManager.MovingAction> GetMovingAction(
          ref RencounterManager.ActionAfterBehaviour self,
          ref RencounterManager.ActionAfterBehaviour opponent)
        {
            List<RencounterManager.MovingAction> movingAction1 = new List<RencounterManager.MovingAction>();
            self.view.unitBottomStatUI.EnableCanvas(false);
            RencounterManager.MovingAction movingAction2 = new RencounterManager.MovingAction(ActionDetail.Penetrate, CharMoveState.TeleportBack, delay: 0.5f);
            movingAction2.dstDistance = 3f;
            movingAction2.SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE);
            movingAction1.Add(movingAction2);
            opponent.infoList.Add(new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, updateDir: false, delay: 0.5f));
            return movingAction1;
        }
    }
    public class ParryNear : BehaviourActionBase
    {
        public override List<RencounterManager.MovingAction> GetMovingAction(
          ref RencounterManager.ActionAfterBehaviour self,
          ref RencounterManager.ActionAfterBehaviour opponent)
        {
            List<RencounterManager.MovingAction> movingAction1 = new List<RencounterManager.MovingAction>();
            ActionDetail actionDetail1 = ActionDetail.Guard;
            int num = self.view.model.customBook.ClassInfo.id == 243003 ? 1 : (self.view.model.customBook.ClassInfo.id == 143003 ? 1 : 0);
            if (num != 0)
                actionDetail1 = ActionDetail.S1;
            RencounterManager.MovingAction movingAction2 = new RencounterManager.MovingAction(actionDetail1, CharMoveState.Stop, delay: 1f);
            if (num != 0)
            {
                movingAction2.SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE);
                movingAction2.customEffectRes = "Kimsatgat_S1";
            }
            RencounterManager.MovingAction movingAction3 = new RencounterManager.MovingAction(actionDetail1, CharMoveState.Stop, delay: 0.5f);
            movingAction3.SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE);
            movingAction3.customEffectRes = "Kimsatgat_S2";
            RencounterManager.MovingAction movingAction4 = new RencounterManager.MovingAction(ActionDetail.Slash, CharMoveState.Stop, delay: 1f);
            //movingAction4.SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE);
            if (self.data.actionType == ActionType.Atk)
                movingAction4.SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.PRE);
            else
                movingAction4.SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE); 
            movingAction4.customEffectRes = "Kimsatgat_S3";
            movingAction1.Add(movingAction2);
            movingAction1.Add(movingAction3);
            movingAction1.Add(movingAction4);
            ActionDetail actionDetail2 = ActionDetail.Hit;
            if (opponent.behaviourResultData != null && opponent.behaviourResultData.behaviourRawData != null)
                actionDetail2 = MotionConverter.MotionToAction(opponent.behaviourResultData.behaviourRawData.MotionDetail);
            RencounterManager.MovingAction movingAction5 = new RencounterManager.MovingAction(ActionDetail.Move, CharMoveState.Stop, delay: 1f);
            movingAction5.customEffectRes = "None";
            movingAction5.SetEffectTiming(EffectTiming.NOT_PRINT, EffectTiming.NOT_PRINT, EffectTiming.NOT_PRINT);
            RencounterManager.MovingAction movingAction6 = new RencounterManager.MovingAction(actionDetail2, CharMoveState.Stop, delay: 0.5f);
            movingAction6.customEffectRes = "None";
            movingAction6.SetEffectTiming(EffectTiming.NOT_PRINT, EffectTiming.NOT_PRINT, EffectTiming.NOT_PRINT);
            RencounterManager.MovingAction movingAction7 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Knockback, delay: 1f);
            movingAction7.customEffectRes = "None";
            movingAction7.SetEffectTiming(EffectTiming.NOT_PRINT, EffectTiming.NOT_PRINT, EffectTiming.NOT_PRINT);
            opponent.infoList.Add(movingAction5);
            opponent.infoList.Add(movingAction6);
            opponent.infoList.Add(movingAction7);
            return movingAction1;
        }
    }
}
