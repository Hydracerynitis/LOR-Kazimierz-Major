﻿using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor 
{
    public class BattleUnitBuf_Depressed : BattleUnitBuf
    {
        public override string keywordId => "Depressed";
        public override string bufActivatedText => string.Format(BattleEffectTextsXmlList.Instance.GetEffectText("Depressed").Desc, stack.ToString(), (-20*stack).ToString());
        public static void AddBuf(BattleUnitModel model, int value)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Depressed) is BattleUnitBuf_Depressed battleUnitBufdepressed))
            {
                battleUnitBufdepressed = new BattleUnitBuf_Depressed{ stack = value };
                model.bufListDetail.AddBuf(battleUnitBufdepressed);
            }
            else
                battleUnitBufdepressed.stack+=value;
        }
        public static bool GetBuf(BattleUnitModel model, out BattleUnitBuf_Depressed buf)
        {
            buf = null;
            if(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Depressed) is BattleUnitBuf_Depressed battleUnitBufdepressed)
            {
                buf = battleUnitBufdepressed;
                return true;
            }
            return false;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus { power = -stack });
        }
        public override StatBonus GetStatBonus()
        {
            return new StatBonus() { breakRate=-20*stack};
        }
    }
}
