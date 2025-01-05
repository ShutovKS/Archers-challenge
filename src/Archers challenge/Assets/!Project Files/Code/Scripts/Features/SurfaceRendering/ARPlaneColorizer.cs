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
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _arPlane = GetComponent<ARPlane>();
            _meshRenderer = GetComponent<MeshRenderer>();

            UpdatePlaneColor();
        }

        private void UpdatePlaneColor()
        {
            var color = GetColorByClassification(_arPlane.classifications);
            color.a = 0.25f;
            _meshRenderer.material.color = color;
        }

        private static Color GetColorByClassification(PlaneClassifications type) => type switch
        {
            PlaneClassifications.None             => new Color(1f, 1f, 1f),
            PlaneClassifications.Ceiling          => new Color(0.7f, 0.7f, 1f),
            PlaneClassifications.DoorFrame        => new Color(0.8f, 0.4f, 0.2f),
            PlaneClassifications.Floor            => new Color(0.4f, 0.3f, 0.3f),
            PlaneClassifications.WallArt          => new Color(0.9f, 0.5f, 0.5f),
            PlaneClassifications.WallFace         => new Color(0.6f, 0.4f, 0.3f),
            PlaneClassifications.WindowFrame      => new Color(0.8f, 0.8f, 0.2f),
            PlaneClassifications.Couch            => new Color(0.5f, 0.2f, 0.2f),
            PlaneClassifications.Seat             => new Color(0.5f, 0.2f, 0.5f),
            PlaneClassifications.SeatOfAnyType    => new Color(0.7f, 0.3f, 0.7f),
            PlaneClassifications.Table            => new Color(0.2f, 0.6f, 0.5f),
            PlaneClassifications.InvisibleWallFace=> new Color(0.7f, 0.7f, 0.7f),
            PlaneClassifications.Other            => new Color(0.5f, 0.5f, 0.5f),
            _                                     => new Color(1f, 1f, 1f)
        };
    }
}