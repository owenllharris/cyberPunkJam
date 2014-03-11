using UnityEditor;
using UnityEngine;

namespace PigeonCoopToolkit.Generic.Editor
{
    public class AboutUtillityWindow : EditorWindow
    {
        public VersionInformation versionInformation;
        public Texture2D logo;
        public string UserGuidePath;

        void OnGUI()
        {
            if(logo == null)
            {
                return;
            }


            GUI.DrawTexture(new Rect(5, 5, logo.width, logo.height), logo);
            if(versionInformation != null) GUI.Label(new Rect(5, logo.height + 10, position.width, position.height), versionInformation.ToString());
            
            if(!string.IsNullOrEmpty(UserGuidePath))
            {
                if (GUI.Button(new Rect(5, logo.height + 26, position.width, position.height), "Need help? Click here to read the guide!", EditorStyles.label))
                {
                    Application.OpenURL(UserGuidePath);
                };

            }
            
            if (GUI.Button(new Rect(5, logo.height + 42, position.width, position.height), "Click here to say hello @PigeonCoopAU", EditorStyles.label))
            {
                Application.OpenURL("http://www.twitter.com/PigeonCoopAU");
            };

            

        }

        public void Init(Texture2D _logo, VersionInformation _versionInformation, string userGuidePath)
        {
            logo = _logo;
            UserGuidePath = userGuidePath;
            versionInformation = _versionInformation;
            minSize = maxSize = new Vector2(_logo ? _logo.width + 10 : 0, _logo ? _logo.height + 60 : 0);
        }
    }
}