using UnityEngine;

namespace BowAndArrow.BowScripts
{
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class LineBetweenTransforms : MonoBehaviour
    {
        [SerializeField] private Transform[] transforms;
        [SerializeField] private float lineWidth = .01f;
        private LineRenderer _lineRenderer;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = transforms.Length;
            _lineRenderer.widthMultiplier = lineWidth;
        }

        private void Update()
        {
            for (var i = 0; i < transforms.Length; i++)
            {
                _lineRenderer.SetPosition(i, transforms[i].position);
            }
        }
    }
}