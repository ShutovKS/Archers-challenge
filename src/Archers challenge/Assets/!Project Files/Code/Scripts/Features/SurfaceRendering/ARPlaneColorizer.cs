using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Features.SurfaceRendering
{
    [RequireComponent(typeof(ARPlane))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ARPlaneColorizer : MonoBehaviour
    {
        [SerializeField] private bool isShow;

        private ARPlane _arPlane;
        private MeshRenderer _meshRenderer;
        private ARPlaneMeshVisualizer _arPlaneMeshVisualizer;

        private void Awake()
        {
            _arPlane = GetComponent<ARPlane>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _arPlaneMeshVisualizer = GetComponent<ARPlaneMeshVisualizer>();

            _arPlaneMeshVisualizer.enabled = true;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
            
            if (!isShow)
            {
                _arPlaneMeshVisualizer.enabled = false;
            }
        }

        private void OnEnable()
        {
            _arPlane.boundaryChanged += UpdatePlaneColor;
            UpdatePlaneColor();
        }

        private void OnDisable()
        {
            _arPlane.boundaryChanged -= UpdatePlaneColor;
        }

        private void UpdatePlaneColor(ARPlaneBoundaryChangedEventArgs args) => UpdatePlaneColor();

        private void UpdatePlaneColor()
        {
            if (_arPlane == null || _meshRenderer == null || isShow == false) return;
            
            _meshRenderer.material.color = GetColorByClassification(_arPlane.classifications);
        }

        private static Color GetColorByClassification(PlaneClassifications type) => type switch
        {
            PlaneClassifications.None => new Color(1f, 1f, 1f),
            PlaneClassifications.Ceiling => new Color(0.7f, 0.7f, 1f),
            PlaneClassifications.DoorFrame => new Color(0.8f, 0.4f, 0.2f),
            PlaneClassifications.Floor => new Color(0.4f, 0.3f, 0.3f),
            PlaneClassifications.WallArt => new Color(0.9f, 0.5f, 0.5f),
            PlaneClassifications.WallFace => new Color(0.6f, 0.4f, 0.3f),
            PlaneClassifications.WindowFrame => new Color(0.8f, 0.8f, 0.2f),
            PlaneClassifications.Couch => new Color(0.5f, 0.2f, 0.2f),
            PlaneClassifications.Seat => new Color(0.5f, 0.2f, 0.5f),
            PlaneClassifications.SeatOfAnyType => new Color(0.7f, 0.3f, 0.7f),
            PlaneClassifications.Table => new Color(0.2f, 0.6f, 0.5f),
            PlaneClassifications.InvisibleWallFace => new Color(0.7f, 0.7f, 0.7f),
            PlaneClassifications.Other => new Color(0.5f, 0.5f, 0.5f),
            _ => new Color(1f, 1f, 1f)
        };
    }
}