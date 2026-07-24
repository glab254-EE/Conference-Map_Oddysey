using System.Collections.Generic;
using System.Linq;
using Clipper2Lib;
using UnityEngine;
using WaystoneMason.Utils;

namespace WaystoneMason.Agents
{
    public abstract class ObstaclesScannerBase : MonoBehaviour
    {
        public WMNavMeshHolder PreferredHolder;
        
        private readonly HashSet<WMNavMeshDynamicObstacle> _obstaclesOnNavMesh = new();

        public WMNavMeshHolder Holder { get; private set; }
        
        #region Unity Events

        private void OnValidate()
        {
            PreferredHolder ??= GetComponent<WMNavMeshHolder>();
        }
        
        private void OnEnable()
        {
            Holder ??= PreferredHolder;
            Holder.OnBeforeRebuild += HandleOnBeforeRebuild;
        }
        
        private void OnDisable()
        {
            Holder.OnBeforeRebuild -= HandleOnBeforeRebuild;
        }

        #endregion

        #region Abstract Getters

        protected abstract IEnumerable<WMNavMeshObstacle> GetSeenObstacles();
        
        protected abstract bool IsVisible(PathD contour, Rect bounds);

        protected abstract Rect GetChunkPregenerationRect();
        
        #endregion

        private void HandleOnBeforeRebuild()
        {
            Scan();
            PregenerateChunks();
        }
        
        private void Scan()
        {
            var obstacles = GetSeenObstacles();
            
            // First, the scanner finds obstacles in the field of view and updates them,
            // deleting the previous position of the object from memory and remembering the current one.
            var navMesh = Holder.NavMesh;
            foreach (var obstacle in obstacles)
            {
                obstacle.Affect(navMesh);
                if (obstacle is WMNavMeshDynamicObstacle dynamic) _obstaclesOnNavMesh.Add(dynamic);
            }
            
            // Among all the objects in memory, the scanner finds objects whose objective and subjective positions
            // do not match, and at the same time, the object in the subjective position is now within the scanner's
            // field of view. For such objects, their subjective position is deleted from memory, because the scanner
            // has noticed that the object has gone somewhere, but it does not know where.
            foreach (var obstacle in _obstaclesOnNavMesh.ToList())
            {
                var actualState = obstacle.IsActualOn(navMesh, out var currentContour, out var bounds);

                if (obstacle && actualState) continue;
                if (!IsVisible(currentContour, bounds)) continue;
                
                navMesh.RemoveObstacle(currentContour);
                _obstaclesOnNavMesh.Remove(obstacle);
            }
            
        }

        private void PregenerateChunks()
        {
            var rect = GetChunkPregenerationRect();
            foreach (var chunkPosition in GeometryUtils.GetChunksInRect(rect.min, rect.max))
            {
                Holder.NavMesh.GetOrCreateChunk(chunkPosition);
            }
        }
    }
}