#if UNITY_EDITOR || DEBUG || UTILSPACKAGE_DEBUG
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace raiden.utils
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private GameObject DebugUIRoot;
        private bool DebugUIOpen;
        private bool HasOpenedDebugUI;

        [SerializeField] private Transform DebugOperationsRoot;
        [SerializeField] private GameObject DebugOperationButton;

        [SerializeField] private TMP_Text SystemInfoText;

        [SerializeField] private Transform MouseCursor;
        [SerializeField] private RectTransform VirtualMouseCursor;
        [SerializeField] private RectTransform canvasRectTransform;
        [SerializeField] private float cursorSpeed = 300f;
        [SerializeField] private float padding = 35f;
        private bool previousMouseState;
        private static Mouse virtualMouse;

        private bool PreviousCheckCursorLockState;

        [SerializeField] private GameObject DialogUI;
        [SerializeField] private TMP_Text DialogText;

        [SerializeField] private GameObject PinBG;
        private bool Pinned;

        public static DebugUI instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < DebugOperationsManager.Methods.Count; i++)
            {
                GameObject go = Instantiate(DebugOperationButton, DebugOperationsRoot);
                
                go.GetComponentInChildren<TMP_Text>().text = DebugOperationsManager.MethodNames[i];
                var i1 = i;
                go.GetComponent<Button>().onClick.AddListener(() => { DebugOperationsManager.Methods[i1].Invoke(null, null);});
            }
        }

        private void Update()
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                ToggleDebugMenu();
            }

            if (DebugUIOpen && Cursor.lockState != CursorLockMode.None)
            {
                VirtualMouseCursor.gameObject.SetActive(true);
                MouseCursor.gameObject.SetActive(false);
            }
            else if (DebugUIOpen && !Cursor.visible)
            {
                VirtualMouseCursor.gameObject.SetActive(false);
                MouseCursor.gameObject.SetActive(true);
                
                MouseCursor.position = Mouse.current.position.ReadValue();
            }
            else
            {
                VirtualMouseCursor.gameObject.SetActive(false);
                MouseCursor.gameObject.SetActive(false);
            }
        }

        public void ToggleDebugMenu()
        {
            PreviousCheckCursorLockState = InputSystemUIInputModule.DisableCursorLockStateChecking;
                
            DebugUIOpen = !DebugUIOpen;
            DebugUIRoot.SetActive(DebugUIOpen);

            if (!DebugUIOpen)
            {
                InputSystemUIInputModule.DisableCursorLockStateChecking = PreviousCheckCursorLockState;
            }
            else
            {
                InputSystemUIInputModule.DisableCursorLockStateChecking = true;
            }

            if (!HasOpenedDebugUI)
            {
                if (virtualMouse == null)
                {
                    virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse", "DebugUICursor");
                }
                
                InputSystem.onAfterUpdate += UpdateMotion;
                
                HasOpenedDebugUI = true;
                
                string advancedString = "SYSTEM INFO\nVersion: " + Application.version + "\n" +
                "Application: " + Application.identifier + "\n" +
                "Unity Version: " + Application.unityVersion + "\n" +
                "Build GUID: " + Application.buildGUID + "\n" +
                "Current Scene: " + SceneManager.GetActiveScene().path + "\n" +
                "\n" +
                "Platform: " + Application.platform + "\n" +
                "System Language: " + Application.systemLanguage + "\n" +
                "Device Model: " + SystemInfo.deviceModel + "\n" +
                "Device Type: " + SystemInfo.deviceType + "\n" +
                "\n" +
                "Internet Reachability: " + Application.internetReachability + "\n" +
                "\n" +
                "Current GPU: " + SystemInfo.graphicsDeviceName + "\n" +
                "Graphics API: " + SystemInfo.graphicsDeviceType + "\n" +
                "GraphicsDeviceVersion: " + SystemInfo.graphicsDeviceVersion + "\n" +
                "GraphicsShaderLevel: " + SystemInfo.graphicsShaderLevel + "\n" +
                "GraphicsMultiThreaded: " + SystemInfo.graphicsMultiThreaded + "\n" +
                "RenderingThreadMode: " + SystemInfo.renderingThreadingMode + "\n" +
                "\n" +
                "Max VRAM: " + SystemInfo.graphicsMemorySize + " MB\n" +
                "Max RAM: " + SystemInfo.systemMemorySize + " MB\n" +
                "\n" +
                "OS: " + SystemInfo.operatingSystem + "\n" +
                "OS Family: " + SystemInfo.operatingSystemFamily + "\n" +
                "CPU: " + SystemInfo.processorModel + "\n" +
                "CPU Cores: " + SystemInfo.processorCount + "\n" +
                "\n" +
                "DataPath: " + Application.dataPath + "\n" +
                "StreamingAssetsPath: " + Application.streamingAssetsPath + "\n" +
                "PersistentDataPath: " + Application.persistentDataPath + "\n" +
                "TemporaryCachePath: " + Application.temporaryCachePath + "\n" +
                "ConsoleLogPath: " + Application.consoleLogPath;

                SystemInfoText.text = advancedString;
            }
            
            if (!virtualMouse.added)
            {
                InputSystem.AddDevice(virtualMouse);
            }
        }

        public void ShowDialog(string message)
        {
            DialogUI.SetActive(true);
            DialogText.text = message;
        }

        public void PinPerfUI()
        {
            Pinned = !Pinned;
            PinBG.SetActive(Pinned);
        }

        private void UpdateMotion()
        {
            if (!VirtualMouseCursor.gameObject.activeSelf)
            {
                return;
            }

            Vector2 deltaValue = Mouse.current.delta.ReadValue();
            deltaValue *= cursorSpeed * Time.deltaTime;

            Vector2 currentPosition = virtualMouse.position.ReadValue();
            Vector2 newPosition = currentPosition + deltaValue;

            newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
            newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

            InputState.Change(virtualMouse.position, newPosition);
            InputState.Change(virtualMouse.delta, deltaValue);

            bool aButtonIsPressed = Mouse.current.leftButton.IsPressed();
            if (previousMouseState != aButtonIsPressed)
            {
                virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
                InputState.Change(virtualMouse, mouseState);
                previousMouseState = aButtonIsPressed;
            }

            AnchorCursor(newPosition);
        }
        
        private void AnchorCursor(Vector2 position)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, null, out Vector2 anchoredPosition);
            VirtualMouseCursor.anchoredPosition = anchoredPosition;
        }

        private void OnDestroy()
        {
            InputSystem.onAfterUpdate -= UpdateMotion;

            if (virtualMouse != null)
            {
                InputSystem.RemoveDevice(virtualMouse);
            }

            instance = null;
        }
    }
}
#endif