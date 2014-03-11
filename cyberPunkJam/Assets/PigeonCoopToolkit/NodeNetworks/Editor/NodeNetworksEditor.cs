using System.Linq;
using PigeonCoopToolkit.Generic;
using PigeonCoopToolkit.Generic.Editor;
using UnityEditor;
using UnityEngine;

namespace PigeonCoopToolkit.NavigationSystem.Editor
{
    public class NodeNetworksEditor : VersionedEditorWindow {

        public GUIStyle LightBackgroundStyle;
        public GUIStyle DarkBackgroundStyle;
        public GUIStyle NoBackgroundBorderStyle;
        public Vector2 _scrollBarPos;

        public Texture2D NodeNetworksLogo;

        [MenuItem("Window/Pigeon Coop Toolkit/Node Networks")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            NodeNetworksEditor win = (NodeNetworksEditor)GetWindow(typeof(NodeNetworksEditor), false, "Node Editor");
            win.minSize = new Vector2(370, 650);

            win.NodeNetworksLogo = Resources.Load<Texture2D>("PCTK/NN/NodeNetworksLogo");
            win.LightBackgroundStyle = new GUIStyle(EditorStyles.largeLabel);
            win.LightBackgroundStyle.padding = new RectOffset(15, 25, 25, 25);
            win.LightBackgroundStyle.margin = new RectOffset();
            win.LightBackgroundStyle.border = new RectOffset(0, 0, 0, 1);
            win.LightBackgroundStyle.normal.background = Resources.Load<Texture2D>("PCTK/Generic/LightBackground"); ;

            win.DarkBackgroundStyle = new GUIStyle(EditorStyles.largeLabel);
            win.DarkBackgroundStyle.padding = new RectOffset(15, 25, 25, 10);
            win.DarkBackgroundStyle.margin = new RectOffset();
            win.DarkBackgroundStyle.border = new RectOffset(0, 0, 1, 0);
            win.DarkBackgroundStyle.normal.background = Resources.Load<Texture2D>("PCTK/Generic/DarkBackground"); ;

            win.NoBackgroundBorderStyle = new GUIStyle(EditorStyles.largeLabel);
            win.NoBackgroundBorderStyle.padding = new RectOffset(15, 15, 20, 20);
            win.NoBackgroundBorderStyle.margin = new RectOffset();
            win.NoBackgroundBorderStyle.border = new RectOffset(0, 0, 0, 1);
            win.NoBackgroundBorderStyle.normal.background = Resources.Load<Texture2D>("PCTK/Generic/JustBorder"); ;

            win._scrollBarPos = Vector2.zero;
        }

        void OnGUI()
        {
            if (NodeNetworksLogo == null || LightBackgroundStyle == null || DarkBackgroundStyle == null ||
                NoBackgroundBorderStyle == null)
            {
                return;
            }

            if (EditorApplication.isPlaying)
            {
                GUI.enabled = false;
            }
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("About", EditorStyles.toolbarButton))
            {
                ShowAbout(NodeNetworksLogo,null);
            }

            EditorGUILayout.EndHorizontal();

            _scrollBarPos = EditorGUILayout.BeginScrollView(_scrollBarPos);

            EditorGUILayout.BeginHorizontal(LightBackgroundStyle,GUILayout.Height(83));
                EditorGUILayout.LabelField("");
                if (NodeNetworksLogo != null)
                    GUI.DrawTexture(new Rect(15, 13, 355, 47), NodeNetworksLogo);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(EditorStyles.largeLabel);
            EditorGUILayout.HelpBox("The NavMesh2D Editor will generate the node network for you. This is a WIP tool to help you create seperate node networks by hand. ", MessageType.Info);
            EditorGUILayout.EndVertical();

            

            EditorGUILayout.EndScrollView();

        }


        #region Overrides of VersionedEditorWindow

        public override VersionInformation VersionInformation()
        {
            return new VersionInformation("NavNode Editor", 0, 1, 0);
        }

        #endregion
    }
}
