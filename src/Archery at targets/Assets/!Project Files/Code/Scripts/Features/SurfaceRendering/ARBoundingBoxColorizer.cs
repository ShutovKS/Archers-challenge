using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Features.SurfaceRendering
{
    [RequireComponent(typeof(ARBoundingBox))]
    public class ARBoundingBoxColorizer : MonoBehaviour
    {
        private ARBoundingBox _boundingBox;
        private MeshRenderer _meshRenderer;

        private void Reset()
        {
            _boundingBox = GetComponent<ARBoundingBox>();
        }

        private void Awake()
        {
            _boundingBox = GetComponent<ARBoundingBox>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();

            if (_meshRenderer == null)
            {
                Debug.LogError($"{nameof(_meshRenderer)} is null.");
            }

            UpdateBoxColor();
        }

        private void Update()
        {
            if (_meshRenderer != null)
            {
                _meshRenderer.transform.localScale = _boundingBox.size;
            }
        }

        private void UpdateBoxColor()
        {
            var boxMatColor = GetColorByClassification(_boundingBox.classifications);

            boxMatColor.a = 0.0f;
            _meshRenderer.material.color = boxMatColor;
        }

        private static Color GetColorByClassification(BoundingBoxClassifications classifications)
        {
            return classifications switch
            {
                BoundingBoxClassifications.Couch => Color.blue,
                BoundingBoxClassifications.Table => Color.yellow,
                BoundingBoxClassifications.Bed => Color.cyan,
                BoundingBoxClassifications.Lamp => Color.magenta,
                BoundingBoxClassifications.Plant => Color.green,
                BoundingBoxClassifications.Screen => Color.white,
                BoundingBoxClassifications.Storage => Color.red,
                BoundingBoxClassifications.None => Color.gray,
                BoundingBoxClassifications.Other => Color.gray,
                _ => Color.gray
            };
        }
    }
}