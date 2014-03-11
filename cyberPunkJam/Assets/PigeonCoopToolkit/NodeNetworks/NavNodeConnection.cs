namespace PigeonCoopToolkit.NavigationSystem
{
    [System.Serializable]
    public class NavNodeConnection {

        public enum NavConnectionType
        {
            Standard,
        }

        public NavConnectionType connectionType; 
        public NavNode connectedNode;

    }
}

