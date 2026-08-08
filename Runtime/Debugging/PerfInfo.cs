#if UNITY_EDITOR || DEBUG || UTILSPACKAGE_DEBUG
using System.Text;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace raiden.utils
{
    public class PerfInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text PerfText;

        private ProfilerRecorder systemMemoryRecorder;
        private ProfilerRecorder drawCallsRecorder;
        private ProfilerRecorder batchesRecorder;
        private ProfilerRecorder staticBatchesRecorder;
        private ProfilerRecorder setpassCallsRecorder;
        private ProfilerRecorder verticesRecorder;
        private ProfilerRecorder trianglesRecorder;
        private ProfilerRecorder shadowCastersRecorder;
        private ProfilerRecorder mainThreadTimeRecorder;
        private ProfilerRecorder gpuFrameTimeRecorder;

        private bool DoUpdate;
        
        private void OnEnable()
        {
            // GetAvailableProfilerStats.EnumerateProfilerStats();
            systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
            drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            batchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
            staticBatchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Static Batches Count");
            setpassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
            verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
            trianglesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
            shadowCastersRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Shadow Casters Count");
            mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread");
            gpuFrameTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "GPU Frame Time");
            
            UpdateAsync().Forget();
        }

        private void OnDisable()
        {
            systemMemoryRecorder.Dispose();
            drawCallsRecorder.Dispose();
            batchesRecorder.Dispose();
            staticBatchesRecorder.Dispose();
            setpassCallsRecorder.Dispose();
            verticesRecorder.Dispose();
            trianglesRecorder.Dispose();
            shadowCastersRecorder.Dispose();
            mainThreadTimeRecorder.Dispose();
            gpuFrameTimeRecorder.Dispose();
        }
        
        private async UniTaskVoid UpdateAsync()
        {
            if (!systemMemoryRecorder.Valid)
            {
                return;
            }
            
            var sb = new StringBuilder(150);
            sb.AppendLine($"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"Draw Calls: {drawCallsRecorder.LastValue}");
            sb.AppendLine($"Batches: {batchesRecorder.LastValue}");
            sb.AppendLine($"Static Batches: {staticBatchesRecorder.LastValue}");
            sb.AppendLine($"SetPass calls: {setpassCallsRecorder.LastValue}");
            sb.AppendLine($"Vertices: {verticesRecorder.LastValue}");
            sb.AppendLine($"Triangles: {trianglesRecorder.LastValue}");
            sb.AppendLine($"Shadow Casters: {shadowCastersRecorder.LastValue}");
            sb.AppendLine($"CPU Frame Time: {mainThreadTimeRecorder.LastValue * 0.000001f} ms");
            sb.AppendLine($"GPU Frame Time: {gpuFrameTimeRecorder.LastValue * 0.000001f} ms");
            PerfText.text = sb.ToString();

            await UniTask.WaitForSeconds(0.25f);
            
            UpdateAsync().Forget();
        }
    }
}
#endif