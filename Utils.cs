using BepInEx.IL2CPP;
using BepInEx.Logging;
using Digit.Prime.FleetManagement;
using Digit.Prime.SharedFeatures;
using Digit.PrimePlatform.Models;
using HarmonyLib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Optimus.STFC.ConfigManager
{
    internal static class Utils
    {
        public static UIManager GetUIManager()
        {
            var allObjects = Resources.FindObjectsOfTypeAll<UIManager>();
            if (allObjects.Length > 0)
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t\t\t\t\t UIManager is found");
                return allObjects.First();
            }
            else
            {
                if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t\t\t\t\t UIManager is NOT FOUND");
                return null;
            }
        }

        public static GameObject GetgameObjectByName(string name)
        {
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            GameObject result = null;
            for (int i = 0; i < allObjects.Length; i++)
            {
                if (allObjects[i].name == name)
                {
                    result = allObjects[i];
                    if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t\t\t\t\t Found GameObject \"{name}\"");
                    break;
                }
            }
            if (ConfigManagerPlugin.configVerbose.Value) ConfigManagerPlugin.Log.LogInfo($"\t\t\t\t\t GameObject \"{name}\" NOT FOUND");
            return result;
        }

    }
}
