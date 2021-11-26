using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class EnemyTeamStageManager_ArmorLess : EnemyTeamStageManager
    {
        public List<int> Enemy = new List<int>() { 2160007, 2160007, 2160006, 2160006, 2160007, 2160008, 2160007, 2160005, 2160006, 2160008, 2160006, 2160008,
                                                   2160004, 2160008, 2160007, 2160007, 2160006, 2160006, 2160008, 2160008, 2160005, 2160004};
        private static List<int> index = new List<int>() { 0, 1, 2, 3, 4, 5 };
        public override bool IsStageFinishable() => Enemy.Count<=0;
        public override void OnRoundStart()
        {
            BattleObjectManager.instance.GetList(Faction.Enemy).FindAll(x => x.IsDead()).ForEach(x => BattleObjectManager.instance.UnregisterUnit(x));
            foreach(int index in index)
            {
                if (BattleObjectManager.instance.GetUnitWithIndex(Faction.Enemy, index) == null)
                {
                    SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, Tools.MakeLorId(Enemy[0]), Tools.MakeLorId(10000000 + Enemy[0]), index);
                    Enemy.RemoveAt(0);
                }
            }
        }
        public override void OnEndBattle()
        {
            if (BattleObjectManager.instance.GetAliveList(Faction.Enemy).Count > 0 && Enemy.Count > 0)
                return;
            for (int index = 0; index < 3; ++index)
            {
                Singleton<StageController>.Instance.OnEnemyDropBookForAdded(new DropBookDataForAddedReward(Tools.MakeLorId(2160002)));
                Singleton<StageController>.Instance.OnEnemyDropBookForAdded(new DropBookDataForAddedReward(Tools.MakeLorId(2160002), true));
                SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.DropBook(new List<string>() { TextDataModel.GetText("BattleUI_GetBook", Singleton<DropBookXmlList>.Instance.GetData(Tools.MakeLorId(2160002)).Name) });
            }
        }
    }
}
