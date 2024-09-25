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
            var planeMatColor = GetColorByClassification(_arPlane.classification);

            planeMatColor.a = 0.0f;
            _planeMeshRenderer.material.color = planeMatColor;
        }

        private static Color GetColorByClassification(PlaneClassification classifications) => classifications switch
        {
            PlaneClassification.None => Color.green,
            PlaneClassification.Wall => Color.white,
            PlaneClassification.Floor => Color.red,
            PlaneClassification.Ceiling => Color.yellow,
            PlaneClassification.Table => Color.blue,
            PlaneClassification.Seat => Color.blue,
            PlaneClassification.Door => Color.blue,
            PlaneClassification.Window => new Color(1f, 0.4f, 0f), //orange
            PlaneClassification.Other => Color.magenta,
            _ => Color.gray
        };
    }
}