using Digit.Client.Core;
using Digit.Client.UI;
using Digit.Prime.GameSettings;
using Digit.Prime.SharedFeatures;
using HarmonyLib;
using Il2CppSystem;
using Optimus.STFC.ConfigManager.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Tomlyn;
using Tomlyn.Model;
//using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using BepInEx.IL2CPP;

namespace Optimus.STFC.ConfigManager
{
    public static class CommunityModSettingsHandler
    {
        public static string CurrentWorkingDirectory => Directory.GetCurrentDirectory();
        public static string TomlVarsFilePath => Path.Combine(CurrentWorkingDirectory, "community_patch_runtime.vars");
        public static string SettingsFilePath => Path.Combine(CurrentWorkingDirectory, "community_patch_settings.toml");

        public static bool CheckSettingsFileExists()
        {            
            if (File.Exists(SettingsFilePath))
            {
                return true;
            }
            return false;
        }
        public static CategoryOptionContext AddCategoryToSettingsContext(SettingsContext context, ContainerOptionContext parent, string categoryLabel, string categoryTitle)
        {
            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  Add [{categoryLabel}] category to [{parent.TitleContext.Identifier}]");
            return context.AddCategory(parent, categoryLabel, categoryTitle);
        }

        public static void AddButtonAndTextToSettingsContext(SettingsContext context,ContainerOptionContext parent, PropertyInfo setting, string settingLabel,string buttonLabel)
        {
            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  Add [{settingLabel}] with button [{buttonLabel}] category to [{parent.TitleContext.Identifier}]");
            UIManager UIManager = ConfigManagerPlugin.UIManager;
            System.Action raiseInfoBox = () => {
                GenericInfoPopupContext infoContext = new GenericInfoPopupContext();
                infoContext.Title = parent.TitleContext;
                infoContext.Narrative = new LocaleTextContext(settingLabel, parent.TitleContext.Identifier);
                ConfigManagerPlugin.UIManager.ShowInfoBox(infoContext.Cast<IMutableTitle>());
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"AddButtonAndTextToSettingsContext() -> change button pressed for [{settingLabel}]");

            };

            System.Func<string> getValueCallback = delegate () {

                string value = GetModelStringValue(setting.DeclaringType.Name.ToLower(), setting.Name);
                string result = setting.Name + " = " + value;
                return result;
            };
            Il2CppSystem.Func<string> il2cppGetValueCallback = getValueCallback;
            context.AddButtonAndText(parent, settingLabel, buttonLabel, raiseInfoBox, il2cppGetValueCallback);            
        }

        public static void AddButtonToSettingsContext(SettingsContext context,ContainerOptionContext parent,string settingLabel,string buttonLabel)
        {
            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  Add [{settingLabel}] with button [{buttonLabel}] category to [{parent.TitleContext.Identifier}]");
            System.Action raiseInfoBox = () => {
                GenericInfoPopupContext infoContext = new GenericInfoPopupContext();
                infoContext.Title = parent.TitleContext;
                infoContext.Narrative = new LocaleTextContext(settingLabel, parent.TitleContext.Identifier);
                ConfigManagerPlugin.UIManager.ShowInfoBox(infoContext.Cast<IMutableTitle>());
                //ConfigManagerPlugin.UIManager.ShowInfoBoxWithHyperlinks(infoContext);
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"AddButtonToSettingsContext() -> change button pressed for [{settingLabel}]");

            };
            context.AddButton(parent, settingLabel, buttonLabel, raiseInfoBox);
        }
        public static void AddToggleToSettingsContext(SettingsContext context, ContainerOptionContext parent, PropertyInfo setting, bool value)
        {
            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  Add [{setting.Name}] toggle to [{parent.TitleContext.Identifier}]");
            System.Func<bool> getValueCallback = () => {

                bool value = GetModelBoolValue(setting.DeclaringType.Name.ToLower(), setting.Name);
                return value;
            };

            System.Action<bool> onValueChangedCallback = (value) => {
                SetModelValue(setting.DeclaringType.Name.ToLower(), setting.Name, value);
                SaveModelToFile();
            };

            context.AddToggle(parent, setting.Name, getValueCallback, onValueChangedCallback);
        }

        public static CommunityModSettingsTomlModel CommunityModSettingsModel { get; set; }

        public static void ParseCommunitySettingsToml(SettingsContext context, ContainerOptionContext parent)
        {
            try
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  CurrentWorkingDirectory = {CurrentWorkingDirectory}");

                //string runtimeSettingsFilePath = Path.Combine(CurrentWorkingDirectory, "community_patch_settings.vars");
                //if (writeLogs) Log.LogInfo($"  runtimeSettingsFilePath = {runtimeSettingsFilePath}");
                
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  TomlVarsFilePath = {TomlVarsFilePath}");
                if (File.Exists(TomlVarsFilePath))
                {
                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  TomlVarsFilePath exists!");

                    string toml = File.ReadAllText(TomlVarsFilePath);
                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  read toml vars");

                    //TomlTable table = Toml.Parse(toml);
                    Tomlyn.Syntax.DocumentSyntax table = Toml.Parse(toml);
                    
                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  parsed toml vars");
                    var sections = table.Tables;
                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  sections count found = {sections.ChildrenCount}");

                    CommunityModSettingsModel = Toml.ToModel<CommunityModSettingsTomlModel>(toml);
                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  parsed toml file");

                    PropertyInfo[] properties = typeof(CommunityModSettingsTomlModel).GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} {property.PropertyType}");
                        if (property.DeclaringType == typeof(CommunityModSettingsTomlModel))
                        {
                            var section = property.GetValue(CommunityModSettingsModel);
                            if (section != null)
                            {
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} is a section");
                                var sectionContext = AddCategoryToSettingsContext(context, parent, property.Name, parent.TitleContext.Identifier + " > " + property.Name);
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t section [{property.Name}] \t possible settings count = {property.PropertyType.GetProperties().Count()}");
                                PropertyInfo[] sectionProperties = property.PropertyType.GetProperties();
                                foreach (PropertyInfo setting in sectionProperties)
                                {
                                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t\t {setting.Name} [{setting.PropertyType.ToString().Replace("System.Nullable`1", "").Replace("[", "").Replace("]", "")}] = {setting.GetValue(section)}");
                                    var settingValue = setting.GetValue(section);
                                    if (settingValue != null)
                                    {
                                        if (setting.PropertyType == typeof(bool?))
                                        {
                                            AddToggleToSettingsContext(context, sectionContext, setting, (bool)settingValue);
                                        }
                                        //else AddButtonAndTextToSettingsContext(context, sectionContext, setting, $"{setting.Name} = {settingValue}", "CHANGE");
                                        else AddButtonToSettingsContext(context, sectionContext, $"{setting.Name} = {settingValue}", "CHANGE");
                                    }
                                    //bool shit = true;


                                }
                            }

                        }
                    }



                    //foreach (var section in sections)
                    //{
                    //    var sectionContext = AddCategoryToSettingsContext(context, parent, section.Name.ToString(), parent.TitleContext.Identifier + " > " + section.Name.ToString()); 
                    //    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  section [{section.Name.ToString()}] settings count found = {section.Items.Count()}");
                    //    foreach (var item in section.Items)
                    //    {
                    //        if (item.Value?.GetType() == typeof(Tomlyn.Syntax.BooleanValueSyntax))
                    //        {

                    //        }
                    //        //AddButtonAndTextToSettingsContext(context, sectionContext, $"{item.Key} = {item.Value} = value type = [{item.Value?.GetType()}]", "CHANGE", writeLogs, Log);
                    //        AddButtonToSettingsContext(context, sectionContext, $"{item.Key} = {item.Value}", "CHANGE");
                    //    }
                    //}


                    //TomlTable parameters = table.Get<TomlTable>("Parameters");

                    //Dictionary<string, object> dictionary = new Dictionary<string, object>();

                    //foreach (var kvp in parameters)
                    //{
                    //    string key = kvp.Key.ToString();
                    //    object value = kvp.Value.ToObject();

                    //    dictionary.Add(key, value);
                    //}

                    //// Print the contents of the dictionary
                    //foreach (var pair in dictionary)
                    //{
                    //    Console.WriteLine($"{pair.Key}: {pair.Value}");
                    //}
                }
            }
            catch (System.Exception e)
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogError($"ERROR in ParseCommunitySettingsToml(): \r\n {e.Message} \r\n {e.StackTrace}");

                throw;
            }



        }

        public static T? GetModelValue<T>(string sectionName, string settingName)
        {
            try
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"Try get Coomunity Mod setting [{settingName}] from section [{sectionName}]");

                PropertyInfo[] properties = typeof(CommunityModSettingsTomlModel).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} {property.PropertyType}");
                    if (property.DeclaringType == typeof(CommunityModSettingsTomlModel) && property.Name == sectionName)
                    {
                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} section is found");
                        var section = property.GetValue(CommunityModSettingsModel);
                        PropertyInfo[] sectionProperties = property.PropertyType.GetProperties();
                        foreach (PropertyInfo property2 in sectionProperties)
                        {
                            if (property2.Name == settingName)
                            {
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property2.Name} setting is found");
                                var settingValue = property2.GetValue(section);
                                //return (T)settingValue!;
                                //return default(T);
                                if (settingValue == null) return default(T);
                                else return (T)settingValue!;
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"ERROR: {e.Message}\r\n {e.StackTrace}");
            }
            return default(T);


        }

        public static string GetModelStringValue(string sectionName, string settingName)
        {
            try
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"Try get Comunity Mod <string> setting [{settingName}] from section [{sectionName}]");

                PropertyInfo[] properties = typeof(CommunityModSettingsTomlModel).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    //if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} {property.PropertyType}");
                    if (property.DeclaringType == typeof(CommunityModSettingsTomlModel) && property.Name == sectionName)
                    {
                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} section is found");
                        var section = property.GetValue(CommunityModSettingsModel);
                        PropertyInfo[] sectionProperties = property.PropertyType.GetProperties();
                        foreach (PropertyInfo property2 in sectionProperties)
                        {
                            if (property2.Name == settingName)
                            {
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property2.Name} setting is found");
                                var settingValue = property2.GetValue(section);
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t Got [{property2.Name}] = [{settingValue}] from [{property.Name}] of CommunityModSettingsModel");
                                //return (T)settingValue!;
                                //return default(T);
                                if (settingValue == null) return default(string);
                                else return settingValue.ToString();
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"ERROR: {e.Message}\r\n {e.StackTrace}");
            }
            return default(string);


        }

        public static System.Type GetModelValueType(string sectionName, string settingName)
        {
            try
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"Try get Comunity Mod <string> setting [{settingName}] from section [{sectionName}]");

                PropertyInfo[] properties = typeof(CommunityModSettingsTomlModel).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    //if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} {property.PropertyType}");
                    if (property.DeclaringType == typeof(CommunityModSettingsTomlModel) && property.Name == sectionName)
                    {
                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} section is found");
                        var section = property.GetValue(CommunityModSettingsModel);
                        PropertyInfo[] sectionProperties = property.PropertyType.GetProperties();
                        foreach (PropertyInfo property2 in sectionProperties)
                        {
                            if (property2.Name == settingName)
                            {
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property2.Name} setting is found. Type [{property2.PropertyType}]");
                                return property2.PropertyType;
                            }
                        }
                    }
                }
                return null;
            }
            catch (System.Exception e)
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"ERROR: {e.Message}\r\n {e.StackTrace}");
            }
            return null;


        }

        public static bool GetModelBoolValue(string sectionName, string settingName)
        {
            try
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"Try get Comunity Mod <bool> setting [{settingName}] from section [{sectionName}]");

                PropertyInfo[] properties = typeof(CommunityModSettingsTomlModel).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    //if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} {property.PropertyType}");
                    if (property.DeclaringType == typeof(CommunityModSettingsTomlModel) && property.Name == sectionName)
                    {
                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} section is found");
                        var section = property.GetValue(CommunityModSettingsModel);
                        PropertyInfo[] sectionProperties = property.PropertyType.GetProperties();
                        foreach (PropertyInfo property2 in sectionProperties)
                        {
                            if (property2.Name == settingName)
                            {
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property2.Name} setting is found");
                                var settingValue = property2.GetValue(section);
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t Got [{property2.Name}] = [{settingValue}] from [{property.Name}] of CommunityModSettingsModel");
                                //return (T)settingValue!;
                                //return default(T);
                                if (settingValue == null) return default(bool);
                                else return (bool)settingValue;
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"ERROR: {e.Message}\r\n {e.StackTrace}");
            }
            return default(bool);


        }

        public static void SetModelValue(string sectionName, string settingName, object value)
        {
            try
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"Try get Comunity Mod setting [{settingName}] from section [{sectionName}]");

                PropertyInfo[] properties = typeof(CommunityModSettingsTomlModel).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    //if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} {property.PropertyType}");
                    if (property.DeclaringType == typeof(CommunityModSettingsTomlModel) && property.Name == sectionName)
                    {
                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} section is found");
                        var section = property.GetValue(CommunityModSettingsModel);
                        PropertyInfo[] sectionProperties = property.PropertyType.GetProperties();
                        foreach (PropertyInfo property2 in sectionProperties)
                        {
                            //if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property2.Name} [{property2.PropertyType}] = {property2.GetValue(section)}");
                            if (property2.Name == settingName)
                            {
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property2.Name} setting is found. Try set value to [{value}]");
                                property2.SetValue(section, value);
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t Set [{property2.Name}] = [{property2.GetValue(section)}]b");
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"ERROR: {e.Message}\r\n {e.StackTrace}");
            }
        }

        public static void SaveModelToFile()
        {
            try
            {
                string modelTomlString = Toml.FromModel(CommunityModSettingsModel);
                File.WriteAllText(SettingsFilePath, modelTomlString);
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t\t Comunity Mod setting saved to file [{SettingsFilePath}]");
            }
            catch (System.Exception e)
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"ERROR: {e.Message}\r\n {e.StackTrace}");
            }
        }

        [HarmonyWrapSafe]
        [HarmonyPostfix]
        //[HarmonyPatch(typeof(CanvasController), "Show")]//, new System.Type[] { typeof(int), typeof(bool) })]
        [HarmonyPatch(typeof(CanvasController), "Start")]//, new System.Type[] { typeof(int), typeof(bool) })]
        //public static void CanvasController_Show(CanvasController __instance)
        public static void CanvasController_Start(CanvasController __instance)
        {

            if (__instance != null)
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{__instance.name}");

                if (__instance.name == "GenericInfoPopup_Canvas")
                {
                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"GenericInfoPopup_Canvas Show()");

                    var titleController = __instance.GetComponent<TitleViewController>();
                    if (titleController != null)
                    {
                        var context = titleController.CanvasContext;
                        if (context != null)
                        {
                            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t title [{context.Title.Identifier}] Narrative [{context.Narrative.Identifier}]");
                            var sectionName = context.Title.Identifier.Replace("COMMUNITY MOD SETTING > ", "");
                            var settingName = context.Narrative.Identifier.Split('=')[0].Trim();
                            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t sectionName [{sectionName}]  settingName [{settingName}]");

                            var gameSettingsCanvas = __instance.transform.parent.Find("GameSettings_Canvas");
                            if (gameSettingsCanvas != null && gameSettingsCanvas.gameObject.active)
                            {
                                // it is popup info from ComMod settings
                                // todo: add text input from chat; add get/set values;
                                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t GameSettings are opened");

                                Transform chatFooterGameObject = Utils.GetgameObjectByName("MainChatScreen_Canvas")?.transform.Find("Footer");
                                if (chatFooterGameObject != null)
                                {
                                    Transform settingTextInput = UnityEngine.Object.Instantiate(chatFooterGameObject, __instance.transform.Find("BodyContainer/Scroller/Viewport/"));
                                    if (settingTextInput != null)
                                    {
                                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"created");
                                        settingTextInput.name = "SettingTextInputFooter";
                                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"set name");
                                        settingTextInput.Find("BG").gameObject.SetActive(false);
                                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"disable BG");
                                        settingTextInput.Find("InputContent/Emoji Button").gameObject.SetActive(false);
                                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"disable Emoji Button");
                                        settingTextInput.Find("InputContent/EmojiPanelContainer").gameObject.SetActive(false);
                                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"disable EmojiPanelContainer");
                                        GameObject.Destroy(settingTextInput.transform.GetComponent<CanvasGroup>());

                                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"destroy CanvasGroup");
                                        var settingValue = GetModelStringValue(sectionName, settingName);
                                        settingTextInput.Find("InputContent/InputContainer/InputField").GetComponent<TMPro.TMP_InputField>().Append(settingValue);
                                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"set value");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyWrapSafe]
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Button), "Press")]
        public static void Button_Press1(Button __instance)
        {
            //Log.LogInfo($"\t\t\t Button.Press()");
            if (__instance != null)
            {
                if (__instance.name == "ButtonPrimary"
                    && __instance.transform?.parent?.name == "SendButtonContainer"
                    && __instance.transform?.parent?.parent?.name == "SettingTextInputFooter"
                    && __instance.transform?.parent?.parent?.parent?.name == "Viewport"
                    && __instance.transform?.parent?.parent?.parent?.parent?.name == "Scroller"
                    && __instance.transform?.parent?.parent?.parent?.parent?.parent?.name == "BodyContainer"
                    && __instance.transform?.parent?.parent?.parent?.parent?.parent?.parent?.name == "GenericInfoPopup_Canvas"
                    )
                //ScreenManager/CanvasRoot/MainFrame/GenericInfoPopup_Canvas/BodyContainer/Scroller/Viewport/SettingTextInputFooter/SendButtonContainer/
                {
                    try
                    {
                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t\t\t\t Button Pressed [GenericInfoPopup_Canvas/BodyContainer/Scroller/Viewport/SettingTextInputFooter/SendButtonContainer/ButtonPrimary] ");
                        var settingTextInputFooter = __instance.transform?.parent?.parent;
                        var genericInfoPopupCanvas = __instance.transform?.parent?.parent?.parent?.parent?.parent?.parent;
                        var gameSettingsCanvas = __instance.transform?.parent?.parent?.parent?.parent?.parent?.parent?.parent.Find("GameSettings_Canvas");
                        if (gameSettingsCanvas != null && gameSettingsCanvas.gameObject.active)
                        {
                            // it is button from text input from popup info window from ComMod settings
                            var popupCanvasCloseButton = genericInfoPopupCanvas.Find("BodyContainer/CloseButtonContainer/GenericButton").gameObject.GetComponent<Button>();
                            var titleController = genericInfoPopupCanvas.GetComponent<TitleViewController>();
                            if (titleController != null)
                            {
                                var context = titleController.CanvasContext;
                                if (context != null)
                                {
                                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t title [{context.Title.Identifier}] Narrative [{context.Narrative.Identifier}]");
                                    var sectionName = context.Title.Identifier.Replace("COMMUNITY MOD SETTING > ", "");
                                    var settingName = context.Narrative.Identifier.Split('=')[0].Trim();
                                    string? settingStringValue = settingTextInputFooter.Find("InputContent/InputContainer/InputField").GetComponent<TMPro.TMP_InputField>().text;
                                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t sectionName [{sectionName}]  settingName [{settingName}]  settingValue [{settingStringValue}]");
                                    var settingType = GetModelValueType(sectionName, settingName);
                                    if (settingType == typeof(int?) && int.TryParse(settingStringValue, out int settingIntValue))
                                    {
                                        SetModelValue(sectionName, settingName, settingIntValue);
                                    }
                                    else if (settingType == typeof(float?) && float.TryParse(settingStringValue, out float settingFloatValue))
                                    {
                                        SetModelValue(sectionName, settingName, settingFloatValue);

                                    }
                                    else if (settingType == typeof(string))
                                    {
                                        SetModelValue(sectionName, settingName, settingStringValue);
                                    }
                                    SaveModelToFile();
                                    UpdateGameSettingsCanvasValues(gameSettingsCanvas, settingName, settingStringValue);
                                    popupCanvasCloseButton.Press();
                                }
                            }
                        }

                    }
                    catch (System.Exception e)
                    {
                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"ERROR: {e.Message}\r\n {e.StackTrace}");
                    }
                }
            }
        }

        public static void UpdateGameSettingsCanvasValues(Transform gameSettingsCanvas, string settingName, string settingStringValue)
        {
            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"Start updating rendered settings values");

            var buttonOptions = gameSettingsCanvas.GetComponentsInChildren<ButtonOptionWidget>();
            foreach (var option in buttonOptions)
            {
                var name = option.Context.LabelContext.Identifier.Split(" = ")[0];
                if (name == settingName)
                {
                    option.Context.LabelContext.Identifier = settingName + " = " + settingStringValue;
                    option.OnDidBindContext();
                }             
            }
        }
    }
}
