using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infrastructure.Factories.Player;
using Infrastructure.Services.ARPlanes;
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
        private IPlayerFactory _playerFactory;
        private bool _planesAvailable;

        [Inject]
        public void Construct(IARPlanesService arPlanesService, IPlayerFactory playerFactory)
        {
            _arPlanesService = arPlanesService;
            _playerFactory = playerFactory;
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
            if (boundary.Length == 0)
            {
                throw new InvalidOperationException("Selected AR plane has no valid boundary.");
            }

            return plane.transform.TransformPoint(boundary[UnityEngine.Random.Range(0, boundary.Length)]);
        }

        private Quaternion GetRotation(Vector3 position)
        {
            var player = _playerFactory.PlayerContainer.CameraGameObject;
            var direction = position - player.transform.position;
            return Quaternion.LookRotation(direction);
        }
    }
}