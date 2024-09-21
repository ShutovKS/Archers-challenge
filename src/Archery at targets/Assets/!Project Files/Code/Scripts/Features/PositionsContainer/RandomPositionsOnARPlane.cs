using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infrastructure.Services.ARPlanes;
using Infrastructure.Services.Camera;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;

namespace Features.PositionsContainer
{
    public class RandomPositionsOnARPlane : PositionsContainer
    {
        public override bool InfinitePositions => true;

        private IARPlanesService _arPlanesService;
        private ICameraService _cameraService;
        private bool _planesAvailable;

        [Inject]
        public void Construct(IARPlanesService arPlanesService, ICameraService cameraService)
        {
            _arPlanesService = arPlanesService;
            _cameraService = cameraService;
        }

        private void Awake()
        {
            if (_arPlanesService != null)
            {
                _arPlanesService.OnPlaneDetected += OnPlaneDetected;
            }
        }

        private void OnDestroy()
        {
            if (_arPlanesService != null)
            {
                _arPlanesService.OnPlaneDetected -= OnPlaneDetected;
            }
        }

        private void OnPlaneDetected()
        {
            _planesAvailable = _arPlanesService.IsPlaneDetected;
        }

        public override (Vector3 position, Quaternion rotation) GetTargetPosition()
        {
            if (!_planesAvailable)
            {
                return (Vector3.zero, Quaternion.identity);
            }

            ReadOnlyCollection<ARPlane> planes = null;
            var classifications = new List<PlaneClassifications>
            {
#if UNITY_EDITOR
                PlaneClassifications.None
#else
                PlaneClassifications.WallFace,
                PlaneClassifications.Floor,
                PlaneClassifications.Ceiling
#endif
            };

            while (classifications.Count > 0)
            {
                if (planes == null || planes.Count == 0)
                {
                    var classification = classifications[UnityEngine.Random.Range(0, classifications.Count)];
                    planes = _arPlanesService.GetPlanes(classification);
                    classifications.Remove(classification);
                }
                else
                {
                    break;
                }
            }

            if (planes == null || planes.Count == 0)
            {
                return (Vector3.zero, Quaternion.identity);
            }

            var selectedPlane = planes[UnityEngine.Random.Range(0, planes.Count)];

            var randomPoint = GetRandomPointOnPlane(selectedPlane);

            var rotation = GetRotation(randomPoint);

            return (randomPoint, rotation);
        }

        private Vector3 GetRandomPointOnPlane(ARPlane plane)
        {
            var boundary = plane.boundary;

            if (boundary.Length == 0) throw new InvalidOperationException("Selected AR plane has no valid boundary.");

            const int MAX_ATTEMPTS = 10;
            var attempts = 0;

            do
            {
                var randomPoint =
                    plane.transform.TransformPoint(boundary[UnityEngine.Random.Range(0, boundary.Length)]);
                attempts++;

                var cameraPosition = _cameraService.CameraPosition;
                var direction = randomPoint - cameraPosition;
                var distance = direction.magnitude;
                var mask = LayerMask.GetMask("Target", "Arrow", "Interactable", "Player", "UI", "Interactable Head");

                if (!Physics.Raycast(cameraPosition, direction.normalized, distance, mask))
                {
                    return randomPoint;
                }
            } while (attempts < MAX_ATTEMPTS);

            return Vector3.zero;
        }


        private Quaternion GetRotation(Vector3 position)
        {
            var cameraPosition = _cameraService.CameraPosition;
            var direction = position - cameraPosition;
            return Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}