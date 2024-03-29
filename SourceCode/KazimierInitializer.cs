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
        public static GameObject mainGameobject;
        public static Dictionary<string, AssetBundle> assetBundle;
        public static bool MlynarOn = false;
        public override void OnInitializeMod()
        {
            try
            {
                Harmony harmony = new Harmony("Kazimier");
                ModPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
                BGM = new Dictionary<string, AudioClip>();
                harmony.PatchAll(typeof(FastLateAttack));
                harmony.PatchAll(typeof(NightmareHp));
                harmony.PatchAll(typeof(MagerateHp));
                harmony.PatchAll(typeof(CandleHP));
                harmony.PatchAll(typeof(BattleStartCinematic));
                harmony.PatchAll(typeof(KazimierInitializer));
                KazimierInitializer.assetBundle = new Dictionary<string, AssetBundle>();
                KazimierInitializer.mainGameobject = new GameObject();
                KazimierInitializer.AddAssets();
                GetBGMs(new DirectoryInfo(ModPath + "/BGM"));
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(20600003), ModPath + "/ArtWork/background.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600013), ModPath + "/ArtWork/Candle.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600023), ModPath + "/ArtWork/Street.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600023), ModPath + "/ArtWork/NightMare.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600043), ModPath + "/ArtWork/Tournament.png");
                BaseMod.Harmony_Patch.GetModStoryCG(Tools.MakeLorId(21600053), ModPath + "/ArtWork/Duel.png");
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
        public static void AddAsset(string name, string path)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            KazimierInitializer.assetBundle.Add(name, assetBundle);
        }

        public static void AddAssets() => KazimierInitializer.AddAsset("耀阳颔首", KazimierInitializer.ModPath + "/ab/耀阳颔首.ab");
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
                CreateStoryLine(__instance, 21600033, UIStoryLine.CaneOffice, new Vector3(625f, 320f));
                CreateStoryLine(__instance, 21600043, UIStoryLine.CaneOffice, new Vector3(750f, 160f));
                CreateStoryLine(__instance, 21600053, UIStoryLine.CaneOffice, new Vector3(500f, 480f));
                //CreateStoryLine(__instance, 22600002, UIStoryLine.CaneOffice, new Vector3(500f, 750f));
                CenterPanel_Init = true;
            }
            if (__instance.gameObject.transform.parent.gameObject.name == "BattleStoryPanel" && !BattleStoryPanel_Init)
            {
                if(Storyslots==null)
                    Storyslots = new Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot>();
                CreateStoryLine(__instance, 20600003, UIStoryLine.CaneOffice, new Vector3(500f, 0.0f));
                CreateStoryLine(__instance, 21600013, UIStoryLine.CaneOffice, new Vector3(250f, 160f));
                CreateStoryLine(__instance, 21600023, UIStoryLine.CaneOffice, new Vector3(375f, 320f));
                CreateStoryLine(__instance, 21600033, UIStoryLine.CaneOffice, new Vector3(625f, 320f));
                CreateStoryLine(__instance, 21600043, UIStoryLine.CaneOffice, new Vector3(750f, 160f));
                CreateStoryLine(__instance, 21600053, UIStoryLine.CaneOffice, new Vector3(500f, 480f));
                //CreateStoryLine(__instance, 22600002, UIStoryLine.CaneOffice, new Vector3(500f, 750f));
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
        public static void CreateStoryLine(UIStoryProgressPanel __instance, int stageId, UIStoryLine reference, Vector3 vector)
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
        /*[HarmonyPatch(typeof(UIInvitationPanel),nameof(UIInvitationPanel.GetTheBlueReverberationPrimaryStage))]
        [HarmonyPostfix]
        public static void UIInvitationPanel_GetTheBlueReverberationPrimaryStage_Post(UIInvitationPanel __instance,ref UIStoryLine __result)
        {
            if (__instance.CurrentStage!=null && __instance.CurrentStage.id==Tools.MakeLorId(22600002))
                __result = UIStoryLine.TwistedBlue;
        }
        [HarmonyPatch(typeof(UIInvitationRightMainPanel),nameof(UIInvitationRightMainPanel.OnClickSendButtonForBlue))]
        [HarmonyPrefix]
        public static bool UIInvitationRightMainPanel_OnClickSendButtonForBlue_Pre(UIInvitationRightMainPanel __instance)
        {
            StageClassInfo mlynar = __instance.invPanel.CurrentStage;
            if (mlynar != null && mlynar.id == Tools.MakeLorId(22600002))
            {
                UIAlarmPopup.instance.SetAlarmTextForBlue(UIAlarmType.StartTwistedBlue, (ConfirmEvent)(yesno =>
                {
                    if (!yesno)
                        return;
                    Singleton<LibraryQuestManager>.Instance.OnSendInvitation(mlynar.id);
                    UI.UIController.Instance.PrepareBattle(mlynar, new List<DropBookXmlInfo>());
                    UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Invite);
                }), TextDataModel.GetText("ui_invitation_Mlynar"), UIStoryLine.TwistedBlue);
                return false;
            }
            return true;
        }
        [HarmonyPatch(typeof(UIAlarmPopup),nameof(UIAlarmPopup.SetAlarmTextForBlue))]
        [HarmonyPostfix]
        public static void UIAlarmPopup_SetAlarmTextForBlue_Post(UIAlarmType alarmtype,TextMeshProUGUI ___txt_alarmForBlue,TextMeshProUGUI[] ___txt_alarmEffectTextForBlues,string param = "")
        {
            if (alarmtype != UIAlarmType.StartTwistedBlue || !MlynarOn)
                return;
            ___txt_alarmForBlue.text = param;
            for (int index = 0; index < ___txt_alarmEffectTextForBlues.Length; ++index)
                ___txt_alarmEffectTextForBlues[index].text = param;
        }
        [HarmonyPatch(typeof(UIInvitationRightMainPanel),nameof(UIInvitationRightMainPanel.SetInvBookApplyState))]
        [HarmonyPostfix]
        public static void UIInvitationRightMainPanel_SetInvBookApplyState_Post(UIInvitationRightMainPanel __instance,InvitationApply_State state,UICustomGraphicObject ___button_SendButtonForBlue,Image ___img_endcontents_content,TextMeshProUGUI ___txt_SendButton)
        {
            StageClassInfo mlynar = __instance.invPanel.CurrentStage;
            MlynarOn = false;
            if (mlynar != null && mlynar.id == Tools.MakeLorId(22600002))
            {
                MlynarOn = true;
                ___img_endcontents_content.sprite = Harmony_Patch.ArtWorks["OWPlaceHolder"];
                ___img_endcontents_content.SetNativeSize();
            }
        }*/
        [HarmonyPatch(typeof(BattlePlayingCardSlotDetail),nameof(BattlePlayingCardSlotDetail.RecoverPlayPoint))]
        [HarmonyPrefix]
        static bool BattlePlayingCardSlotDetail_RecoverPlayPoint_Pre(BattlePlayingCardSlotDetail __instance)
        {
            if (__instance._self.passiveDetail.HasPassive<PassiveAbility_2160111>())
                return false;
            return true;
        }
        [HarmonyPatch(typeof(BattlePersonalEgoCardDetail),nameof(BattlePersonalEgoCardDetail.UseCard))]
        [HarmonyPrefix]
        static bool BattlePersonalEgoCardDetail_UseCard_Pre(BattleDiceCardModel card)
        {
            if (card.GetID() == Tools.MakeLorId(2161301) || card.GetID() == Tools.MakeLorId(2161113))
                return false;
            return true;
        }
        [HarmonyPatch(typeof(DiceCardSelfAbilityBase),nameof(DiceCardSelfAbilityBase.IsTrueDamage))]
        [HarmonyPostfix]
        static void DiceCardSelfAbilityBase_IsTrueDamage(DiceCardSelfAbilityBase __instance, ref bool __result)
        {
            BattleUnitModel owner = __instance.owner;
            if (owner != null && owner.passiveDetail.HasPassive<PassiveAbility_2160128>())
                if (__instance.card!=null &&  __instance.card.currentBehavior != null && !__instance.card.currentBehavior.IsParrying())
                    __result = true;
        }
        [HarmonyPatch(typeof(LibraryModel),nameof(LibraryModel.OnClearStage))]
        [HarmonyPostfix]
        public static void LibraryModel_OnClearStage(LorId stageId)
        {
            if (stageId == Tools.MakeLorId(21600053))
            {
                LorId radiantReward = Tools.MakeLorId(2160012);
                List<BookModel> all = Singleton<BookInventoryModel>.Instance.GetBookListAll().FindAll(x => x.ClassInfo.id == radiantReward);
                BookXmlInfo data2 = Singleton<BookXmlList>.Instance.GetData(radiantReward);
                if (data2 == null || all.Count >= data2.Limit)
                    return;
                int difference = data2.Limit - all.Count;
                for (int i = 0; i < difference; i++)
                    Singleton<BookInventoryModel>.Instance.CreateBook(radiantReward);
                UIAlarmPopup.instance.SetAlarmText(TextDataModel.GetText("ui_popup_getequippage", (object)Singleton<BookDescXmlList>.Instance.GetBookName(Tools.MakeLorId(2160013)), (object)difference));
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
                BattleDiceCardModel newCard = unit.allyCardDetail.AddNewCard(Tools.MakeLorId(cards[0]));
                if (newCard == null)
                    return;
                newCard.SetPriorityAdder(p);
                if(unit.UnitData.unitData.EnemyUnitId!=Tools.MakeLorId(2160001))
                    newCard.SetCostToZero();
                cards.RemoveAt(0);
            }
            while (cards.Count > 0);
        }
        public static bool IsNotClashCard(BattleDiceCardModel card) => card.XmlData.Spec.Ranged == CardRange.FarArea || card.XmlData.Spec.Ranged == CardRange.FarAreaEach || card.XmlData.Spec.Ranged == CardRange.Instance;
    }
    public interface NightmareUpdater
    {
        public void AfterChangeBreak();
        public void AfterChangeHp();
    }
}
