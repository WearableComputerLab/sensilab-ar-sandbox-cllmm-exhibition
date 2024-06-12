using System;
using ARSandbox;
using UnityEngine;

namespace CLLMM.Scripts
{
    /// <summary>
    /// Generates a quad mesh based on the calibration data from the CalibrationManager. This quad can be used to
    /// overlay custom visualisations on the projected surface by rendering the quad via a ProjectorCamera
    /// (set in CalibrationManager).
    /// </summary>
    public class CalibratedQuadGenerator : MonoBehaviour
    {
        private const float ZOFFSET = 0.3f;

        [SerializeField] private CalibrationManager _calibrationManager;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        private MeshFilter _meshFilter;
        
        private void Start()
        {
            CalibrationManager.OnCalibration += OnCalibration;
            CalibrationManager.OnCalibrationComplete += OnCalibrationComplete;
        }

        private void OnDestroy()
        {
            CalibrationManager.OnCalibration -= OnCalibration;
            CalibrationManager.OnCalibrationComplete -= OnCalibrationComplete;
        }

        private void SetupQuad()
        {
            if (_meshRenderer.TryGetComponent(out MeshFilter filter))
            {
                _meshFilter = filter;
            }
            else
            {
                _meshFilter = _meshRenderer.gameObject.AddComponent<MeshFilter>();
            }

            CalibrationDescriptor descriptor = _calibrationManager.GetCalibrationDescriptor();
            Vector2 dataStart = descriptor.DataStart.ToVector() * Sandbox.MESH_XY_STRIDE.x;
            Vector2 dataEnd = descriptor.DataEnd.ToVector() * Sandbox.MESH_XY_STRIDE.x;

            // BL TL TR BR
            Vector3[] verts = new Vector3[4]
            {
                new Vector3(dataStart.x, dataStart.y, ZOFFSET),
                new Vector3(dataStart.x, dataEnd.y, ZOFFSET),
                new Vector3(dataEnd.x, dataEnd.y, ZOFFSET),
                new Vector3(dataEnd.x, dataStart.y, ZOFFSET)
            };
            Vector2[] uvs = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0)
            };
            int[] tris = new int[6]
            {
                0, 1, 2,
                0, 2, 3
            };
            
            Mesh quadMesh = new Mesh();
            quadMesh.vertices = verts;
            quadMesh.triangles = tris;
            quadMesh.uv = uvs;
            quadMesh.RecalculateBounds();
            _meshFilter.mesh = quadMesh;
        }

        /// <summary>
        /// Called when the calibration process is started.
        /// </summary>
        private void OnCalibration()
        {
        }

        /// <summary>
        /// Called when the calibration process is complete.
        /// </summary>
        private void OnCalibrationComplete()
        {
            SetupQuad();
        }
    }
}