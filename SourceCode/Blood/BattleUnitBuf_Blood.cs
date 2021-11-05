﻿using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor 
{
    public class BattleUnitBuf_Blood : BattleUnitBuf
    {
        protected override string keywordId => "Blood";
        protected override string keywordIconId => "Nosferatu_Blood";
        public override int paramInBufDesc => 20*stack;
        public static void AddBuf(BattleUnitModel model, int value)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Blood) is BattleUnitBuf_Blood battleUnitBufBlood))
            {
                battleUnitBufBlood = new BattleUnitBuf_Blood{ stack = value};
                model.bufListDetail.AddBuf(battleUnitBufBlood);
            }
            else
                battleUnitBufBlood.stack+=value;
        }
        public static bool GetBuf(BattleUnitModel model, out BattleUnitBuf_Blood buf)
        {
            buf = null;
            if(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Blood) is BattleUnitBuf_Blood battleUnitBufBlood)
            {
                buf = battleUnitBufBlood;
                return true;
            }
            return false;
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus { dmgRate=20*stack });
            this.Destroy();
        }
    }
}