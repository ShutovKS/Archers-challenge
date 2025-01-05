using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Features.SurfaceRendering
{
    [RequireComponent(typeof(ARBoundingBox))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ARBoundingBoxColorizer : MonoBehaviour
    {
        private ARBoundingBox _arBoundingBox;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _arBoundingBox = GetComponent<ARBoundingBox>();
            _meshRenderer = GetComponent<MeshRenderer>();

            UpdatePlaneColor();
        }

        private void UpdatePlaneColor()
        {
            var color = GetColorByClassification(_arBoundingBox.classifications);
            color.a = 0.25f;
            _meshRenderer.material.color = color;
        }

        private static Color GetColorByClassification(BoundingBoxClassifications type) => type switch
        {
            BoundingBoxClassifications.None          => new Color(1f, 1f, 1f),
            BoundingBoxClassifications.Couch         => new Color(0.5f, 0.2f, 0.2f),
            BoundingBoxClassifications.Table         => new Color(0.2f, 0.6f, 0.5f),
            BoundingBoxClassifications.Bed           => new Color(0.6f, 0.4f, 0.7f),
            BoundingBoxClassifications.Lamp          => new Color(1f, 0.9f, 0.6f),
            BoundingBoxClassifications.Plant         => new Color(0.2f, 0.8f, 0.3f),
            BoundingBoxClassifications.Screen        => new Color(0.1f, 0.3f, 0.7f),
            BoundingBoxClassifications.Storage       => new Color(0.5f, 0.4f, 0.3f),
            BoundingBoxClassifications.Bathtub       => new Color(0.7f, 0.9f, 1f),
            BoundingBoxClassifications.Chair         => new Color(0.7f, 0.5f, 0.3f),
            BoundingBoxClassifications.Dishwasher    => new Color(0.6f, 0.6f, 0.8f),
            BoundingBoxClassifications.Fireplace     => new Color(0.9f, 0.4f, 0.2f),
            BoundingBoxClassifications.Oven          => new Color(0.8f, 0.7f, 0.6f),
            BoundingBoxClassifications.Refrigerator  => new Color(0.7f, 0.8f, 0.9f),
            BoundingBoxClassifications.Sink          => new Color(0.8f, 0.9f, 1f),
            BoundingBoxClassifications.Stairs        => new Color(0.4f, 0.3f, 0.2f),
            BoundingBoxClassifications.Stove         => new Color(0.7f, 0.7f, 0.7f),
            BoundingBoxClassifications.Toilet        => new Color(0.9f, 0.9f, 0.9f),
            BoundingBoxClassifications.WasherDryer   => new Color(0.8f, 0.8f, 0.9f),
            BoundingBoxClassifications.Other         => new Color(0.5f, 0.5f, 0.5f),
            _                                        => new Color(1f, 1f, 1f)
        };
    }
}