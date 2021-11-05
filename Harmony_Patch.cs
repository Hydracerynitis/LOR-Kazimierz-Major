using System;
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

namespace KazimierzMajor
{
    public class Harmony_Patch
    {
        public static bool BattleStoryPanel_Init;
        public static bool CenterPanel_Init;
        public static string ModPath;
        public static AudioClip Corrotion;
        public static AudioClip Putrid;
        public static AudioClip Knight;
        public static Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot> Storyslots;
        public Harmony_Patch()
        {
            try
            {
                Harmony harmony = new Harmony("Kazimier");
                ModPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
                Corrotion = Tools.GetAudio(ModPath + "/BGM/Corrosion.mp3");
                Putrid = Tools.GetAudio(ModPath + "/BGM/Putrid.mp3");
                Knight = Tools.GetAudio(ModPath + "/BGM/Knight.mp3");
                PatchGeneral(ref harmony);
                PatchStoryLine(ref harmony);
                //PatchEntryCG(ref harmony);
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(20600003), ModPath + "/ArtWork/background.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600003), ModPath + "/ArtWork/Tournament.png");
            }
            catch(Exception ex)
            {
                File.WriteAllText(Application.dataPath + "/Mods/Kazimier.log", ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        private void PatchGeneral(ref Harmony harmony)
        {
            MethodInfo Patch1 = typeof(Harmony_Patch).GetMethod("DropBookInventoryModel_GetBookList_invitationBookList");
            MethodInfo Method1 = typeof(DropBookInventoryModel).GetMethod("GetBookList_invitationBookList", AccessTools.all);
            harmony.Patch(Method1, postfix: new HarmonyMethod(Patch1));
            MethodInfo Patch2 = typeof(Harmony_Patch).GetMethod("UIInvitationDropBookSlot_SetData_DropBook");
            MethodInfo Method2 = typeof(UIInvitationDropBookSlot).GetMethod("SetData_DropBook", AccessTools.all);
            harmony.Patch(Method2, postfix: new HarmonyMethod(Patch2));
            MethodInfo Patch3 = typeof(Harmony_Patch).GetMethod("BattleUnitBuf_GetBufIcon");
            MethodInfo Method3 = typeof(BattleUnitBuf).GetMethod("GetBufIcon", AccessTools.all);
            harmony.Patch(Method3, prefix: new HarmonyMethod(Patch3));
        }
        private void PatchStoryLine(ref Harmony harmony)
        {
            MethodInfo method1 = typeof(Harmony_Patch).GetMethod("UIStoryProgressPanel_SetStoryLine");
            MethodInfo method2 = typeof(UIStoryProgressPanel).GetMethod("SetStoryLine", AccessTools.all);
            harmony.Patch((MethodBase)method2, postfix: new HarmonyMethod(method1));
        }
        private void PatchEntryCG(ref Harmony harmony)
        {
            MethodInfo method1 = typeof(LibraryModel).GetMethod("OnClearStage", AccessTools.all);
            harmony.Patch(method1, postfix: new HarmonyMethod(typeof(Harmony_Patch).GetMethod("LibraryModel_OnClearStage")));
            MethodInfo method2 = typeof(EntryScene).GetMethod("SetCG", AccessTools.all);
            harmony.Patch(method2, postfix: new HarmonyMethod(typeof(Harmony_Patch).GetMethod("EntryScene_SetCG")));
        }
        public static void DropBookInventoryModel_GetBookList_invitationBookList(ref List<LorId> __result)
        {
            __result.Add(Tools.MakeLorId(2060000));
            __result.Add(Tools.MakeLorId(2160000));
        }
        public static void UIInvitationDropBookSlot_SetData_DropBook(ref TextMeshProUGUI ___txt_bookNum, LorId bookId)
        {
            if (Singleton<DropBookInventoryModel>.Instance.GetBookCount(bookId) == 0)
                ___txt_bookNum.text = "∞";
        }
        public static bool BattleUnitBuf_GetBufIcon(ref Sprite ____bufIcon, ref bool ____iconInit, BattleUnitBuf __instance, ref Sprite __result)
        {
            string iconid = "";
            try
            {
                MethodInfo getId = typeof(BattleUnitBuf).GetMethod("get_keywordIconId", AccessTools.all);
                iconid = getId.Invoke(__instance, Array.Empty<object>()) as string;
            }
            catch(Exception ex)
            {
                File.AppendAllText(Application.dataPath + "/Mods/iconidError.txt", iconid + "\n" + ex.Message + "\n" + ex.StackTrace + "\n\n");
            }
            if (!____iconInit)
            {
                if (__instance.Hide)
                    ____bufIcon = null;
                else
                {
                    try
                    {
                        ____bufIcon = Resources.Load<Sprite>("Sprites/BufIcon/" + iconid);
                        if (____bufIcon == null)
                            ____bufIcon = BaseMod.Harmony_Patch.ArtWorks[iconid];
                    }
                    catch 
                    {
                        ____bufIcon = null;
                    }
                    ____iconInit = true;
                }            
            }
            __result = ____bufIcon;
            return false;
        }
        public static void UIStoryProgressPanel_SetStoryLine(UIStoryProgressPanel __instance)
        {
            if (__instance.gameObject.transform.parent.gameObject.name== "[Rect]CenterPanel" && !CenterPanel_Init)
            {
                if (Storyslots == null)
                    Storyslots = new Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot>();
                CreateKazimier1(__instance);
                CreateKazimier2(__instance);
                CenterPanel_Init = true;
            }
            if (__instance.gameObject.transform.parent.gameObject.name == "BattleStoryPanel" && !BattleStoryPanel_Init)
            {
                if(Storyslots==null)
                    Storyslots = new Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot>();
                CreateKazimier1(__instance);
                CreateKazimier2(__instance);
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
        public static void CreateKazimier1(UIStoryProgressPanel __instance)
        {
            StageClassInfo data = Singleton<StageClassInfoList>.Instance.GetData(Tools.MakeLorId(20600003));
            if (Storyslots.TryGetValue(new List<StageClassInfo>() { data }, out UIStoryProgressIconSlot slot))
                slot.Initialized(__instance);
            UIStoryProgressIconSlot progressIconSlot = ((List<UIStoryProgressIconSlot>)typeof(UIStoryProgressPanel).GetField("iconList", AccessTools.all).GetValue((object)__instance)).Find((Predicate<UIStoryProgressIconSlot>)(x => x.currentStory == UIStoryLine.CaneOffice));
            UIStoryProgressIconSlot newslot = UnityEngine.Object.Instantiate<UIStoryProgressIconSlot>(progressIconSlot, progressIconSlot.transform.parent);
            SlotCopying(__instance, progressIconSlot, newslot);
            newslot.transform.localPosition += new Vector3(250f, 0.0f);
            ((List<GameObject>)typeof(UIStoryProgressIconSlot).GetField("connectLineList", AccessTools.all).GetValue((object)newslot))[0].transform.localPosition += new Vector3(250f, 0.0f);
            Storyslots[new List<StageClassInfo>(){data}] = newslot;
        }
        public static void CreateKazimier2(UIStoryProgressPanel __instance)
        {
            StageClassInfo data = Singleton<StageClassInfoList>.Instance.GetData(Tools.MakeLorId(21600003));
            if (Storyslots.TryGetValue(new List<StageClassInfo>() { data }, out UIStoryProgressIconSlot slot))
                slot.Initialized(__instance);
            UIStoryProgressIconSlot progressIconSlot = ((List<UIStoryProgressIconSlot>)typeof(UIStoryProgressPanel).GetField("iconList", AccessTools.all).GetValue((object)__instance)).Find((Predicate<UIStoryProgressIconSlot>)(x => x.currentStory == UIStoryLine.HanaAssociation));
            UIStoryProgressIconSlot newslot = UnityEngine.Object.Instantiate<UIStoryProgressIconSlot>(progressIconSlot, progressIconSlot.transform.parent);
            SlotCopying(__instance, progressIconSlot, newslot);
            newslot.transform.localPosition += new Vector3(1000f, 0.0f);
            ((List<GameObject>)typeof(UIStoryProgressIconSlot).GetField("connectLineList", AccessTools.all).GetValue((object)newslot))[0].transform.localPosition += new Vector3(750f, 0.0f);
            Storyslots[new List<StageClassInfo>() { data }] = newslot;
        }
        public static void SlotCopying(UIStoryProgressPanel __instance, UIStoryProgressIconSlot slot, UIStoryProgressIconSlot newslot)
        {
            newslot.currentStory = UIStoryLine.Rats;
            List<GameObject> gameObjectList = new List<GameObject>();
            GameObject original = ((List<GameObject>)slot.GetType().GetField("connectLineList", AccessTools.all).GetValue((object)slot))[0];
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, original.transform.parent);
            gameObjectList.Add(gameObject);
            newslot.Initialized(__instance);
            newslot.GetType().GetField("connectLineList", AccessTools.all).SetValue((object)newslot, (object)gameObjectList);
        }
        public static void LibraryModel_OnClearStage(LorId stageId)
        {
            if (stageId == Tools.MakeLorId(20600003))
            {
                LatestDataModel data = new LatestDataModel();
                Singleton<SaveManager>.Instance.LoadLatestData(data);
                data.LatestStorychapter = 9;
                data.LatestStorygroup = 20600003;
                data.LatestStoryepisode = 20600003;
                data.RuinTitle = LibraryModel.Instance.PlayHistory.Clear_EndcontentsAllStage == 1 ? 1 : 0;
                Singleton<SaveManager>.Instance.SaveLatestData(data);
            }
        }
        public static void EntryScene_SetCG(EntryScene __instance,LatestDataModel ____latestData)
        {
            if (____latestData.LatestStorychapter==6 &&  ____latestData.LatestStorygroup== 20600003 && ____latestData.LatestStoryepisode== 20600003)
            {
                Texture2D texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(File.ReadAllBytes(Harmony_Patch.ModPath + "/ArtWork/background.png"));
                Sprite sprite= Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                __instance.CGImage.sprite = sprite;
            }
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
