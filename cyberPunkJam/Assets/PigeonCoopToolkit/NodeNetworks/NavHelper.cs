using UnityEngine;

namespace PigeonCoopToolkit.NavigationSystem
{
    public static class NavHelper {

        public const float gizmoRadius = 0.15f;
        public const float gizmoArrowSize = 0.05f;
        public const float gizmoDistanceFromNode = 0.05f;
        public const float gizmoNodeConnectionSeperation = 0.05f;

        public static Color NavMeshBaseColor = new Color(92 / 255f, 147 / 255f, 205 / 255f, 255 / 255f);
        public static Color NavMeshBaseSelectedColor = new Color(80 / 255f, 228/ 255f, 233 / 255f, 255 / 255f);
        
    }
}
