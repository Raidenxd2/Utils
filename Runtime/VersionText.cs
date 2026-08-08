using TMPro;
using UnityEngine;

namespace raiden.utils
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionText : MonoBehaviour
    {
#if KILLITMYSELF_FULL
        [SerializeField] private GameObject AdvancedVersionTextGO;
#endif

        private void Start()
        {
            GetComponent<TMP_Text>().text = "v" + Application.version + "-" + Application.platform + " (" + Application.unityVersion + ", " + SystemInfo.graphicsDeviceType + ")";
        }

#if KILLITMYSELF_FULL
        public void ShowAdvancedVersionText()
        {
            AdvancedVersionTextGO.SetActive(true);
            gameObject.SetActive(false);
        }
#endif
    }
}