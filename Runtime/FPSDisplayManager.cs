#if SERIAL_UNITASK
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SerialPackage.Runtime
{
    public class FPSDisplayManager : MonoBehaviour
    {
        [SerializeField] private GameObject FPSDisplayRoot;
        [SerializeField] private TMP_Text FPSText;

        private int FramesPerSec;

        void Start()
        {
            FPSUpdate().Forget();
        }

        private async UniTask FPSUpdate()
        {
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            await UniTask.WaitForSeconds(1f);

            if (FPSDisplayRoot == null)
            {
                return;
            }

            if (!FPSDisplayRoot.activeSelf)
            {
                FPSUpdate().Forget();
                return;
            }

            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;
 
            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);

            FPSText.text = "FPS: " + FramesPerSec.ToString();

            FPSUpdate().Forget();
        }
    }
}
#endif