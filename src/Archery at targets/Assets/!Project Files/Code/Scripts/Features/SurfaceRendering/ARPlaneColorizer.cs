using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Features.SurfaceRendering
{
    [RequireComponent(typeof(ARPlane))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ARPlaneColorizer : MonoBehaviour
    {
        private ARPlane _arPlane;
        private MeshRenderer _planeMeshRenderer;

        private void Awake()
        {
            _arPlane = GetComponent<ARPlane>();
            _planeMeshRenderer = GetComponent<MeshRenderer>();

            UpdatePlaneColor();
        }

        private void UpdatePlaneColor()
        {
            var planeMatColor = GetColorByClassification(_arPlane.classifications);

            planeMatColor.a = 0.0f;
            _planeMeshRenderer.material.color = planeMatColor;
        }

        private static Color GetColorByClassification(PlaneClassifications classifications)
        {
            return classifications switch
            {
                PlaneClassifications.Floor => Color.green,
                PlaneClassifications.WallFace => Color.white,
                PlaneClassifications.Ceiling => Color.red,
                PlaneClassifications.Table => Color.yellow,
                PlaneClassifications.Couch => Color.blue,
                PlaneClassifications.Seat => Color.blue,
                PlaneClassifications.SeatOfAnyType => Color.blue,
                PlaneClassifications.WallArt => new Color(1f, 0.4f, 0f), //orange
                PlaneClassifications.DoorFrame => Color.magenta,
                PlaneClassifications.WindowFrame => Color.cyan,
                _ => Color.gray
            };
        }
    }
}