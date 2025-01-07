using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Infrastructure.Services.ARPlanes;
using Infrastructure.Services.Camera;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;
using Random = UnityEngine.Random;

namespace Features.TargetsInLevelManager
{
    public class RandomTargetsAndLookingAtTargetOnARPlane : TargetsInLevelManager
    {
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
            if (_arPlanesService != null) _arPlanesService.OnPlaneDetected += OnPlaneDetected;
        }

        public override void PrepareTargets()
        {
        }

        public override async void StartTargets()
        {
            TargetFactory.TargetHit += OnOnTargetHit;

            await InstantiateTarget();
        }

        public override void StopTargets()
        {
            TargetFactory.TargetHit -= OnOnTargetHit;

            TargetFactory.DestroyAll();
        }

        protected override async void OnOnTargetHit(GameObject targetInstance)
        {
            TargetFactory.Destroy(targetInstance);

            await InstantiateTarget();

            base.OnOnTargetHit(targetInstance);
        }

        private async Task InstantiateTarget()
        {
            var position = GetPosition();
            var rotation = GetRotationOnPlayer(position);

            await TargetFactory.Instantiate(position, rotation);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_arPlanesService != null) _arPlanesService.OnPlaneDetected -= OnPlaneDetected;
        }

        private void OnPlaneDetected() => _planesAvailable = _arPlanesService.IsPlaneDetected;

        public Vector3 GetPosition()
        {
            if (!_planesAvailable) return Vector3.zero;

            var cameraPosition = _cameraService.CameraPosition;
            var cameraForward = _cameraService.Camera.transform.forward;

            const int MAX_ATTEMPTS = 15;
            const float MIN_DISTANCE = 0.5f;

            var planeLayerMask = LayerMask.GetMask("Default");

            for (var i = 0; i < MAX_ATTEMPTS; i++)
            {
                var randomDirection = Random.insideUnitSphere.normalized;

                if (Vector3.Dot(randomDirection, cameraForward) < 0f)
                    randomDirection = -randomDirection;

                if (!Physics.Raycast(cameraPosition, randomDirection, out var hit, 30f, planeLayerMask))
                    continue;

                if (!hit.collider.TryGetComponent<ARPlane>(out var plane))
                    continue;

                if (!IsPlaneClassificationValid(plane))
                    continue;

                if (!(Vector3.Distance(cameraPosition, hit.point) >= MIN_DISTANCE))
                    continue;
                
                return hit.point;
            }

            return Vector3.zero;
        }

        private bool IsPlaneClassificationValid(ARPlane plane) => plane.classifications is
#if UNITY_EDITOR
            PlaneClassifications.None or
#endif
            PlaneClassifications.Ceiling or
            PlaneClassifications.DoorFrame or
            PlaneClassifications.Floor or
            PlaneClassifications.WallArt or
            PlaneClassifications.WallFace or
            PlaneClassifications.WindowFrame;


        private Vector3 GetRandomPointOnPlane(ARPlane plane)
        {
            var boundary = plane.boundary;

            if (boundary.Length == 0) throw new InvalidOperationException("Selected AR plane has no valid boundary.");

            const int MAX_ATTEMPTS = 10;
            var attempts = 0;

            do
            {
                attempts++;

                var randomPoint = plane.transform.TransformPoint(boundary[Random.Range(0, boundary.Length)]);

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


        private Quaternion GetRotationOnPlayer(Vector3 position)
        {
            var cameraPosition = _cameraService.CameraPosition;
            var direction = position - cameraPosition;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);

            return rotation;
        }
    }
}