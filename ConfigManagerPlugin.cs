using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.IL2CPP;
using Digit.Engine.Login.Prime;
using Digit.Prime.GameSettings;
using HarmonyLib;
using System;
using System.Data.Common;
using Digit.Prime.ObjectViewer;
using Digit.Prime.Ships;
using Digit.PrimeServer.Models;
using UnityEngine.UI;
using Digit.Prime.SharedFeatures;
using Digit.Client.UI;
using UnityEngine;
using BepInEx.IL2CPP;
using Digit.Client.Core;
using Digit.Prime.FleetManagement;

namespace Optimus.STFC.ConfigManager
{
    //[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public partial class ConfigManagerPlugin : BasePlugin
    {
        #region[Declarations]

        public const string
            AUTHOR = "Optimus",
            PROJECT = "STFC",
            MODNAME = "ConfigManager",
            GUID = AUTHOR + "." + PROJECT + "." + MODNAME,
            VERSION = MyPluginInfo.PLUGIN_VERSION;

        #endregion

        internal static new ManualLogSource Log;

        public static ConfigEntry<bool> configVerbose;

        public override void Load()
        {
            ConfigManagerPlugin.Log = base.Log;
            configVerbose = Config.Bind("General",      // The section under which the option is shown
                "Verbose",  // The key of the configuration option in the configuration file
                false, // The default value
                "write more logs");

            Harmony.CreateAndPatchAll(typeof(ConfigManagerPlugin));
            Harmony.CreateAndPatchAll(typeof(CommunityModSettingsHandler));

            //CommunityModSettingsHandler

            // Plugin startup logic
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Login Logic


        private static int ServerInstanceId { get; set; }
        private static string ServerRegion { get; set; }
        private static string InstanceSessionId { get; set; }
        private static PrimeLoginController PrimeLoginController { get; set; }

        [HarmonyWrapSafe]
        [HarmonyPostfix]
        //[HarmonyPatch(typeof(PrimeLoginController), "SetCurrentState", new System.Type[] { typeof(Digit.Engine.Login.Core.State), typeof(Digit.Networking.Core.GSError) })] // MethodType.Constructor)]// "Login")]
        [HarmonyPatch(typeof(PrimeLoginController), "AssociateAccount")] // MethodType.Constructor)]// "Login")]
        public static void Login_SaveInstanceID(PrimeLoginController __instance)
        {
            //ServerRegion = __instance.AccountInfo.ServerRegion;
            //ServerInstanceId = __instance.AccountInfo.ServerInstanceId;
            //PrimeLoginController = __instance;
            //if (configVerbose.Value) Log.LogInfo($"AccountInfo: \r\n" +
            //    $"\t\t\t\t Email={__instance.AccountInfo.Email} \r\n" +
            //    $"\t\t\t\t Password={__instance.AccountInfo.Password} \r\n" +
            //    $"\t\t\t\t UserDisplayName={__instance.AccountInfo.UserDisplayName}\r\n" +
            //    $"\t\t\t\t UserId={__instance.AccountInfo.UserId}\r\n" +
            //    $"\t\t\t\t MasterAccountId={__instance.AccountInfo.MasterAccountId}\r\n" +
            //    $"\t\t\t\t MasterSessionId={__instance.AccountInfo.MasterSessionId}\r\n" +
            //    $"\t\t\t\t ServerInstanceId={__instance.AccountInfo.ServerInstanceId}\r\n" +
            //    $"\t\t\t\t ServerRegion={__instance.AccountInfo.ServerRegion}\r\n" +
            //    $"\t\t\t\t ManualServerInstanceId={__instance.AccountInfo.ManualServerInstanceId}\r\n" +
            //    $"\t\t\t\t InstanceAccountId={__instance.AccountInfo.InstanceAccountId}\r\n" +
            //    $"\t\t\t\t InstanceSessionId={__instance.AccountInfo.InstanceSessionId}\r\n" +
            //    $"\t\t\t\t AuthSource={__instance.AccountInfo.AuthSource}\r\n");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.AppVersion                 = {DebugInfo.AppVersion}");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.CurrentServerInstanceName  = {DebugInfo.CurrentServerInstanceName}");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.HomeServerInstanceName     = {DebugInfo.HomeServerInstanceName}");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.MemoryMode                 = {DebugInfo.MemoryMode}");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.Platform                   = {DebugInfo.Platform}");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.QualitySettings            = {DebugInfo.QualitySettings}");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.ServerStackPlayerPref      = {DebugInfo.ServerStackPlayerPref}");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.ServerVersion              = {DebugInfo.ServerVersion}");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.StackName                  = {DebugInfo.StackName}");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.UnityVersion               = {DebugInfo.UnityVersion}");
            if (configVerbose.Value) Log.LogInfo($"DebugInfo.UserId                     = {DebugInfo.UserId}");

            if (configVerbose.Value) Log.LogInfo($"DebugInfo.UserId                     = {DebugInfo.UserId}");

        }

        #endregion
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private static RootOptionContext RootOptionContext { get; set; }
        private static bool isRendered = false;
        public static UIManager UIManager { get; set; }

        [HarmonyWrapSafe]
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SettingsContext), "AddCategory", new Type[] { typeof(ContainerOptionContext), typeof(string), typeof(string), typeof(Il2CppSystem.Func<OptionState>) })] // "Show", new Type[] { typeof(int), typeof(bool) })] //"Start")]
        public static void SettingsContext_AddCategory(SettingsContext __instance, ContainerOptionContext parent, string labelIdentifier, string titleIdentifier, Il2CppSystem.Func<OptionState> queryOptionState, ref CategoryOptionContext __result)
        {
            if (configVerbose.Value) Log.LogInfo($"SettingsContext.AddCategory(ContainerOptionContext parent, string labelIdentifier, string titleIdentifier, Func<OptionState> queryOptionState = null) ");

            if (configVerbose.Value) Log.LogInfo($"- __instance:                        {__instance} ");
            if (configVerbose.Value) Log.LogInfo($"- Parameter 0 'parent':              {parent} ");
            if (configVerbose.Value) Log.LogInfo($"- Parameter 1 'labelIdentifier':     {labelIdentifier} ");
            if (configVerbose.Value) Log.LogInfo($"- Parameter 2 'titleIdentifier':     {titleIdentifier} ");
            if (configVerbose.Value) Log.LogInfo($"- Parameter 3 'queryOptionState':    {(queryOptionState == null ? "null" : queryOptionState.GetIl2CppType().Name)} ");
            if (configVerbose.Value) Log.LogInfo($"- Return value:                      {__result} ");

            if (__instance != null && __instance.RootOption != null)
            {
                RootOptionContext = __instance.RootOption;
                if (configVerbose.Value) Log.LogInfo($"  RootOptionContext is binded");


            }


            bool isAdded = false;
            if (RootOptionContext != null)
            {
                var childOptions = RootOptionContext.Children;


                for (int i = 0; i < childOptions.Count; i++)
                {
                    CategoryOptionContext childOp = childOptions[i].Cast<CategoryOptionContext>();
                    if (childOp != null && childOp.TitleContext.Identifier == "COMMUNITY MOD SETTING")
                    {
                        isAdded = true;
                        break;
                    }
                }
                //if (isAdded) { }
                //isRendered = false;
            }
            if (!isAdded && !isRendered)
            {
                //UIManager = Utils.GetUIManager();
                UIManager = GameObject.FindObjectOfType<UIManager>();
                if (CommunityModSettingsHandler.CheckSettingsFileExists())
                {
                    var comModCategory = CommunityModSettingsHandler.AddCategoryToSettingsContext(__instance, RootOptionContext, "Community Mod", "COMMUNITY MOD SETTING");
                    if (comModCategory != null)
                    {
                        CommunityModSettingsHandler.ParseCommunitySettingsToml(__instance, comModCategory);
                    }
                    //if (configVerbose.Value) Log.LogInfo($"  Add [Community Mod] category");
                    //__instance.AddCategory(RootOptionContext, "Community Mod", "COMMUNITY MOD SETTING");

                    isRendered = true;
                }

            }
            else isRendered = false;

            //if (__instance != null)
            //{
            //    if (__instance.name == "playerProfileV2_canvas")
            //    {
            //        //ScreenManager/CanvasRoot/MainFrame/playerProfileV2_canvas/fullscreen container/WindowContainer/Buttons/
            //        if (__instance.transform.Find("fullscreen container/WindowContainer/Buttons/AllianceInvite")?.gameObject?.activeSelf == true)
            //        {
            //            if (configVerbose.Value) Log.LogInfo($"Unlock alliance invite button");

            //            __instance.transform.Find("fullscreen container/WindowContainer/Buttons/AllianceInvite/GenericButton").GetComponent<Button>().interactable = true;
            //        }
            //    }
            //    if (__instance.name == "ServerClashReturnPopup_Canvas" && configPreventRecall.Value)
            //    {
            //        if (configVerbose.Value) Log.LogInfo($"CanvasController.LateUpdate() for ServerClashReturnPopup_Canvas");

            //        //ScreenManager/CanvasRoot/MainFrame/playerProfileV2_canvas/fullscreen container/WindowContainer/Buttons/
            //        if (__instance.gameObject?.activeSelf == true)
            //        {
            //            if (configVerbose.Value) Log.LogInfo($"destroy recall window");

            //            GameObject.Destroy(__instance.gameObject);
            //            ;
            //        }
            //    }
            //    //ServerClashReturnPopup_Canvas
            //}
            ///
            //return true;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Button press logic

        [HarmonyWrapSafe]
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Button), "Press")]
        public static void Button_Press1(Button __instance)
        {
            //Log.LogInfo($"\t\t\t Button.Press()");
            if (__instance != null)
            {
                if (configVerbose.Value) Log.LogInfo($"\t\t\t\t Button Pressed {__instance.transform?.parent.name}>{__instance.name} ");



            }
        }
        #endregion
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }

}