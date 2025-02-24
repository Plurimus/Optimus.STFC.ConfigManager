//using Digit.Client.UI;
//using Digit.Prime.GameSettings;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Digit.Prime.SharedFeatures;
//using BepInEx.IL2CPP;
//using System.Reflection;
//using System.IO;

namespace Optimus.STFC.ConfigManager
{
    //public static class BepInExModsSettingsHandler
    //{
    //    //public static void ParseBepinexConfigs(SettingsContext context, ContainerOptionContext parent)
    //    //{
    //    //    try
    //    //    {
    //    //        IEnumerable<BasePlugin> allBepinexPlugins = AppDomain.CurrentDomain.GetAssemblies()
    //    //                .SelectMany(assembly => assembly.GetTypes())
    //    //                .Where(type => type.IsSubclassOf(typeof(BasePlugin)))
    //    //                .Select(type => Activator.CreateInstance(type) as BaseClass);

    //    //        //GetAll().ToList().ForEach(x => x.DoStuff());

    //    //        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  CurrentWorkingDirectory = {CurrentWorkingDirectory}");

    //    //        //string runtimeSettingsFilePath = Path.Combine(CurrentWorkingDirectory, "community_patch_settings.vars");
    //    //        //if (writeLogs) Log.LogInfo($"  runtimeSettingsFilePath = {runtimeSettingsFilePath}");

    //    //        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  TomlVarsFilePath = {TomlVarsFilePath}");
    //    //        if (File.Exists(TomlVarsFilePath))
    //    //        {
    //    //            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  TomlVarsFilePath exists!");

    //    //            string toml = File.ReadAllText(TomlVarsFilePath);
    //    //            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  read toml vars");

    //    //            //TomlTable table = Toml.Parse(toml);
    //    //            Tomlyn.Syntax.DocumentSyntax table = Toml.Parse(toml);

    //    //            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  parsed toml vars");
    //    //            var sections = table.Tables;
    //    //            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  sections count found = {sections.ChildrenCount}");

    //    //            CommunityModSettingsModel = Toml.ToModel<CommunityModSettingsTomlModel>(toml);
    //    //            PropertyInfo[] properties = typeof(CommunityModSettingsTomlModel).GetProperties();
    //    //            foreach (PropertyInfo property in properties)
    //    //            {
    //    //                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} {property.PropertyType}");
    //    //                if (property.DeclaringType == typeof(CommunityModSettingsTomlModel))
    //    //                {
    //    //                    var section = property.GetValue(CommunityModSettingsModel);
    //    //                    if (section != null)
    //    //                    {
    //    //                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"{property.Name} is a section");
    //    //                        var sectionContext = AddCategoryToSettingsContext(context, parent, property.Name, parent.TitleContext.Identifier + " > " + property.Name);
    //    //                        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t section [{property.Name}] \t possible settings count = {property.PropertyType.GetProperties().Count()}");
    //    //                        PropertyInfo[] sectionProperties = property.PropertyType.GetProperties();
    //    //                        foreach (PropertyInfo setting in sectionProperties)
    //    //                        {
    //    //                            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t\t {setting.Name} [{setting.PropertyType.ToString().Replace("System.Nullable`1", "").Replace("[", "").Replace("]", "")}] = {setting.GetValue(section)}");
    //    //                            var settingValue = setting.GetValue(section);
    //    //                            if (settingValue != null)
    //    //                            {
    //    //                                if (setting.PropertyType == typeof(bool?))
    //    //                                {
    //    //                                    AddToggleToSettingsContext(context, sectionContext, setting, (bool)settingValue);
    //    //                                }
    //    //                                //else AddButtonAndTextToSettingsContext(context, sectionContext, setting, $"{setting.Name} = {settingValue}", "CHANGE");
    //    //                                else AddButtonToSettingsContext(context, sectionContext, $"{setting.Name} = {settingValue}", "CHANGE");
    //    //                            }
    //    //                            //bool shit = true;


    //    //                        }
    //    //                    }

    //    //                }
    //    //            }



    //    //            //foreach (var section in sections)
    //    //            //{
    //    //            //    var sectionContext = AddCategoryToSettingsContext(context, parent, section.Name.ToString(), parent.TitleContext.Identifier + " > " + section.Name.ToString()); 
    //    //            //    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  section [{section.Name.ToString()}] settings count found = {section.Items.Count()}");
    //    //            //    foreach (var item in section.Items)
    //    //            //    {
    //    //            //        if (item.Value?.GetType() == typeof(Tomlyn.Syntax.BooleanValueSyntax))
    //    //            //        {

    //    //            //        }
    //    //            //        //AddButtonAndTextToSettingsContext(context, sectionContext, $"{item.Key} = {item.Value} = value type = [{item.Value?.GetType()}]", "CHANGE", writeLogs, Log);
    //    //            //        AddButtonToSettingsContext(context, sectionContext, $"{item.Key} = {item.Value}", "CHANGE");
    //    //            //    }
    //    //            //}


    //    //            //TomlTable parameters = table.Get<TomlTable>("Parameters");

    //    //            //Dictionary<string, object> dictionary = new Dictionary<string, object>();

    //    //            //foreach (var kvp in parameters)
    //    //            //{
    //    //            //    string key = kvp.Key.ToString();
    //    //            //    object value = kvp.Value.ToObject();

    //    //            //    dictionary.Add(key, value);
    //    //            //}

    //    //            //// Print the contents of the dictionary
    //    //            //foreach (var pair in dictionary)
    //    //            //{
    //    //            //    Console.WriteLine($"{pair.Key}: {pair.Value}");
    //    //            //}
    //    //        }
    //    //    }
    //    //    catch (System.Exception e)
    //    //    {
    //    //        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"ERROR in ParseCommunitySettingsToml(): \r\n {e.Message} \r\n {e.StackTrace}");

    //    //        throw;
    //    //    }



    //    //}

    //    public static CategoryOptionContext AddCategoryToSettingsContext(SettingsContext context, ContainerOptionContext parent, string categoryLabel, string categoryTitle)
    //    {
    //        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  Add [{categoryLabel}] category to [{parent.TitleContext.Identifier}]");
    //        return context.AddCategory(parent, categoryLabel, categoryTitle);
    //    }

    //    public static void AddButtonAndTextToSettingsContext(SettingsContext context, ContainerOptionContext parent, PropertyInfo setting, string settingLabel, string buttonLabel)
    //    {
    //        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  Add [{settingLabel}] with button [{buttonLabel}] category to [{parent.TitleContext.Identifier}]");
    //        UIManager UIManager = ConfigManagerPlugin.UIManager;
    //        System.Action raiseInfoBox = () => {
    //            GenericInfoPopupContext infoContext = new GenericInfoPopupContext();
    //            infoContext.Title = parent.TitleContext;
    //            infoContext.Narrative = new LocaleTextContext(settingLabel, parent.TitleContext.Identifier);
    //            ConfigManagerPlugin.UIManager.ShowInfoBox(infoContext.Cast<IMutableTitle>());
    //            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  change button pressed for [{settingLabel}]");

    //        };

    //        System.Func<string> getValueCallback = delegate () {

    //            string value = GetModelStringValue(setting.DeclaringType.Name.ToLower(), setting.Name);
    //            string result = setting.Name + " = " + value;
    //            return result;
    //        };
    //        Il2CppSystem.Func<string> il2cppGetValueCallback = getValueCallback;
    //        context.AddButtonAndText(parent, settingLabel, buttonLabel, raiseInfoBox, il2cppGetValueCallback);
    //    }

    //    public static void AddButtonToSettingsContext(SettingsContext context, ContainerOptionContext parent, string settingLabel, string buttonLabel)
    //    {
    //        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  Add [{settingLabel}] with button [{buttonLabel}] category to [{parent.TitleContext.Identifier}]");
    //        System.Action raiseInfoBox = () => {
    //            GenericInfoPopupContext infoContext = new GenericInfoPopupContext();
    //            infoContext.Title = parent.TitleContext;
    //            infoContext.Narrative = new LocaleTextContext(settingLabel, parent.TitleContext.Identifier);
    //            ConfigManagerPlugin.UIManager.ShowInfoBox(infoContext.Cast<IMutableTitle>());
    //            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  change button pressed for [{settingLabel}]");

    //        };
    //        context.AddButton(parent, settingLabel, buttonLabel, raiseInfoBox);
    //    }
    //    public static void AddToggleToSettingsContext(SettingsContext context, ContainerOptionContext parent, PropertyInfo setting, bool value)
    //    {
    //        if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"  Add [{setting.Name}] toggle to [{parent.TitleContext.Identifier}]");
    //        System.Func<bool> getValueCallback = () => {

    //            bool value = GetModelBoolValue(setting.DeclaringType.Name.ToLower(), setting.Name);
    //            return value;
    //        };

    //        System.Action<bool> onValueChangedCallback = (value) => {
    //            SetModelValue(setting.DeclaringType.Name.ToLower(), setting.Name, value);
    //            SaveModelToFile();
    //        };

    //        context.AddToggle(parent, setting.Name, getValueCallback, onValueChangedCallback);
    //    }


    //}
}
