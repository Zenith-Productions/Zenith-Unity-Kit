﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

using Newtonsoft.Json.Linq;

namespace Zenith
{
    [InitializeOnLoad]
    public class Zenith_ImportPanel_Addons : EditorWindow
    {
       private const string Url = "https://github.com/Zenith-Productions/ZPUK/";
        private const string Url1 = "https://trigon.systems/";
        private const string Link = "";
        private const string Link1 = "";

        private static GUIStyle _chillHeader;
        private static Dictionary<string, string> assets = new Dictionary<string, string>();
        private static int _sizeX = 400;
        private static int _sizeY = 5000;
        private static Vector2 _changeLogScroll;
        

        [MenuItem("Zenith/Import panel/addons", false, 501)]
        public static void OpenImportPanel()
        {
            GetWindow<Zenith_ImportPanel_Addons>(true);
        }

        public void OnEnable()
        {
            titleContent = new GUIContent("Zenith Import panel addons");

            Zenith_ImportManager_Addons.checkForConfigUpdate();
            LoadJson();

            maxSize = new Vector2(_sizeX, _sizeY);
            minSize = maxSize;

            _chillHeader = new GUIStyle
            {
                normal =
                {
                    background = Resources.Load("ZenithUtil") as Texture2D,
                    textColor = Color.white
                },
                fixedHeight = 200
            };
        }

        public static void LoadJson()
        {
            assets.Clear();
            
            dynamic configJson =
                JObject.Parse(File.ReadAllText(Zenith_Settings.projectConfigPath + Zenith_ImportManager_Addons.configName));

            Debug.Log("Server Asset Url is: " + configJson["config"]["serverUrl"]);
            Zenith_ImportManager_Addons.serverUrl = configJson["config"]["serverUrl"].ToString();
            _sizeX = (int)configJson["config"]["window"]["sizeX"];
            _sizeY = (int)configJson["config"]["window"]["sizeY"];

            foreach (JProperty x in configJson["assets"])
            {
                var value = x.Value;

                var buttonName = "";
                var file = "";
                
                foreach (var jToken in value)
                {
                    var y = (JProperty) jToken;
                    switch (y.Name)
                    {
                        case "name":
                            buttonName = y.Value.ToString();
                            break;
                        case "file":
                            file = y.Value.ToString();
                            break;
                    }
                }
                assets[buttonName] = file;
            }
        }

        public void OnGUI()
        {
            GUILayout.Box("", style: _chillHeader);
            GUILayout.Space(4);
            GUI.backgroundColor = new Color(
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_R"),
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_G"),
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_B"),
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_A")
            );
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Check for Updates"))
            {

                Zenith_AutomaticUpdateAndInstall.AutomaticZPUKInstaller();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Zenith Productions"))
            {
                Application.OpenURL(Url);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Trigon.Systems"))
            {
                Application.OpenURL(Url1);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
            //Update assets
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Update assets (config)"))
            {
                Zenith_ImportManager.updateConfig();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.Space(4);



            //Imports V!V
            GUI.backgroundColor = new Color(
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_R"),
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_G"),
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_B"),
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_A")
            );
            _changeLogScroll = GUILayout.BeginScrollView(_changeLogScroll, GUILayout.Width(_sizeX));
            GUI.backgroundColor = new Color(
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_R"),
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_G"),
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_B"),
            UnityEditor.EditorPrefs.GetFloat("ZPUKColor_A")
            );
            foreach (var asset in assets)
            {
                GUILayout.BeginHorizontal();
                if (asset.Value == "")
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(asset.Key);
                    GUILayout.FlexibleSpace();
                }
                else
                {
                    if (GUILayout.Button(
                        (File.Exists(Zenith_Settings.getAssetPath() + asset.Value) ? "Import" : "Download") +
                        " " + asset.Key))
                    {
                        Zenith_ImportManager_Addons.downloadAndImportAssetFromServer(asset.Value);
                    }

                    if (GUILayout.Button("Del", GUILayout.Width(40)))
                    {
                        Zenith_ImportManager_Addons.deleteAsset(asset.Value);
                    }
                }
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal();
            EditorPrefs.SetBool("Zenith_ShowInfoPanel", GUILayout.Toggle(EditorPrefs.GetBool("Zenith_ShowInfoPanel"), "Show at startup"));
            GUILayout.EndHorizontal();
        }
    }
}