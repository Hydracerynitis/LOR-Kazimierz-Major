﻿using System;
using HarmonyLib;
using System.Reflection;
using System.IO;
using UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using BaseMod;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using UnityEngine.Networking;
using GameSave;
using UnityEngine.UI;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    [HarmonyPatch]
    public class KazimierInitializer: ModInitializer
    {
        public static bool BattleStoryPanel_Init;
        public static bool CenterPanel_Init;
        public static string ModPath;
        public static Dictionary<string, AudioClip> BGM;
        public static Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot> Storyslots;

        public override void OnInitializeMod()
        {
            try
            {
                Harmony harmony = new Harmony("Kazimier");
                ModPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
                BGM = new Dictionary<string, AudioClip>();
                harmony.PatchAll(typeof(FastLateAttack));
                harmony.PatchAll(typeof(KazimierInitializer));
                GetBGMs(new DirectoryInfo(ModPath + "/BGM"));
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(20600003), ModPath + "/ArtWork/background.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600013), ModPath + "/ArtWork/Candle.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600023), ModPath + "/ArtWork/Street.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600043), ModPath + "/ArtWork/Tournament.png");
            }
            catch(Exception ex)
            {
                File.WriteAllText(Application.dataPath + "/Mods/Kazimier.log", ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        public static void GetBGMs(DirectoryInfo dir)
        {
            if (dir.GetDirectories().Length != 0)
            {
                foreach (DirectoryInfo directory in dir.GetDirectories())
                    GetBGMs(directory);
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(file.FullName);
                BGM[withoutExtension] = Tools.GetAudio(file.FullName);
            }
        }
        [HarmonyPatch(typeof(DropBookInventoryModel),nameof(DropBookInventoryModel.GetBookList_invitationBookList))]
        [HarmonyPostfix]
        public static void DropBookInventoryModel_GetBookList_invitationBookList(ref List<LorId> __result)
        {
            __result.Add(Tools.MakeLorId(2060000));
            if (LibraryModel.Instance?.ClearInfo?.GetClearCount(Tools.MakeLorId(20600003)) >= 1)
                __result.Add(Tools.MakeLorId(2160000));
            if (!__result.Contains(Tools.MakeLorId(2160001)) && LibraryModel.Instance?.ClearInfo?.GetClearCount(Tools.MakeLorId(21600013)) >= 1)
                __result.Add(Tools.MakeLorId(2160001));
            if (!__result.Contains(Tools.MakeLorId(2160002)) && LibraryModel.Instance?.ClearInfo?.GetClearCount(Tools.MakeLorId(21600023))>=1)
                __result.Add(Tools.MakeLorId(2160002));
        }
        [HarmonyPatch(typeof(UIStoryProgressPanel),nameof(UIStoryProgressPanel.SetStoryLine))]
        [HarmonyPostfix]
        public static void UIStoryProgressPanel_SetStoryLine(UIStoryProgressPanel __instance)
        {
            if (__instance.gameObject.transform.parent.gameObject.name== "[Rect]CenterPanel" && !CenterPanel_Init)
            {
                if (Storyslots == null)
                    Storyslots = new Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot>();
                CreateStoryLine(__instance, 20600003, UIStoryLine.CaneOffice, new Vector3(500f, 0.0f));
                CreateStoryLine(__instance, 21600013, UIStoryLine.CaneOffice, new Vector3(250f, 160f));
                CreateStoryLine(__instance, 21600023, UIStoryLine.CaneOffice, new Vector3(375f, 320f));
                //CreateStoryLine(__instance, 21600033, UIStoryLine.CaneOffice, new Vector3(625f, 320f));
                CreateStoryLine(__instance, 21600043, UIStoryLine.CaneOffice, new Vector3(750f, 160f));
                //CreateStoryLine(__instance, 21600053, UIStoryLine.CaneOffice, new Vector3(500f, 480f));
                CenterPanel_Init = true;
            }
            if (__instance.gameObject.transform.parent.gameObject.name == "BattleStoryPanel" && !BattleStoryPanel_Init)
            {
                if(Storyslots==null)
                    Storyslots = new Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot>();
                CreateStoryLine(__instance, 20600003, UIStoryLine.CaneOffice, new Vector3(500f, 0.0f));
                CreateStoryLine(__instance, 21600013, UIStoryLine.CaneOffice, new Vector3(250f, 160f));
                CreateStoryLine(__instance, 21600023, UIStoryLine.CaneOffice, new Vector3(375f, 320f));
                //CreateStoryLine(__instance, 21600033, UIStoryLine.CaneOffice, new Vector3(625f, 320f));
                CreateStoryLine(__instance, 21600043, UIStoryLine.CaneOffice, new Vector3(750f, 160f));
                //CreateStoryLine(__instance, 21600053, UIStoryLine.CaneOffice, new Vector3(500f, 480f));
                BattleStoryPanel_Init = true;
            }
            foreach (List<StageClassInfo> key in Storyslots.Keys)
            {
                Storyslots[key].SetSlotData(key);
                if (key[0].currentState != StoryState.Close)
                    Storyslots[key].SetActiveStory(true);
                else
                    Storyslots[key].SetActiveStory(false);
            }
        }
        public static void CreateStoryLine(UIStoryProgressPanel __instance,int stageId, UIStoryLine reference, Vector3 vector)
        {
            StageClassInfo data = Singleton<StageClassInfoList>.Instance.GetData(Tools.MakeLorId(stageId));
            if (Storyslots.TryGetValue(new List<StageClassInfo>() { data }, out UIStoryProgressIconSlot slot))
            {
                slot.Initialized(__instance);
                return;
            }
            UIStoryProgressIconSlot progressIconSlot = __instance.iconList.Find(x => x.currentStory == reference);
            UIStoryProgressIconSlot newslot = UnityEngine.Object.Instantiate<UIStoryProgressIconSlot>(progressIconSlot, progressIconSlot.transform.parent);
            newslot.currentStory = UIStoryLine.Rats;
            newslot.Initialized(__instance);
            newslot.transform.localPosition += vector;
            newslot.connectLineList = new List<GameObject>();
            Storyslots[new List<StageClassInfo>() { data }] = newslot;
        }
        public static void UpdateInfo(BattleUnitModel unit) => SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(unit, unit.faction, unit.hp, unit.breakDetail.breakGauge);
        public static void AddNewCard(BattleUnitModel unit, List<int> cards, Queue<int> priority)
        {
            do
            {
                int p = 0;
                if (priority.Count > 0)
                    p = priority.Dequeue();
                unit.allyCardDetail.AddNewCard(Tools.MakeLorId(cards[0])).SetPriorityAdder(p);
                cards.RemoveAt(0);
            }
            while (cards.Count > 0);
        }
    }

}