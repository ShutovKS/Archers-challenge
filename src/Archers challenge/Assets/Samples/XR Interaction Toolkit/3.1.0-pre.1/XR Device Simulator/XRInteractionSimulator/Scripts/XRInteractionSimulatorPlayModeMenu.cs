using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.DeviceSimulator
{
    class XRInteractionSimulatorPlayModeMenu : MonoBehaviour
    {
        [Header("Menus")]

        [SerializeField]
        GameObject m_InputSelectionMenu;

        [SerializeField]
        GameObject m_ClosedInputSelectionMenu;

        [SerializeField]
        GameObject m_ControllerActionsMenu;

        [SerializeField]
        GameObject m_ClosedControllerActionsMenu;

        [SerializeField]
        GameObject m_HandActionsMenu;

        [SerializeField]
        GameObject m_ClosedHandActionsMenu;

        [Header("Input Readers")]

        [SerializeField]
        XRInputButtonReader m_ToggleActionMenu;

        [SerializeField]
        XRInputButtonReader m_ToggleInputSelectionMenu;

        [Header("Device Highlight Panels")]

        [SerializeField]
        GameObject m_HighlightFullBodyPanel;

        [SerializeField]
        GameObject m_HighlightLeftControllerPanel;

        [SerializeField]
        GameObject m_HighlightRightControllerPanel;

        [SerializeField]
        GameObject m_HighlightLeftHandPanel;

        [SerializeField]
        GameObject m_HighlightRightHandPanel;

        [SerializeField]
        GameObject m_HighlightHeadPanel;

        [Header("Action Highlight Panels")]

        [SerializeField]
        GameObject m_HighlightTriggerPanel;

        [SerializeField]
        GameObject m_HighlightGripPanel;

        [SerializeField]
        GameObject m_HighlightPrimaryPanel;

        [SerializeField]
        GameObject m_HighlightSecondaryPanel;

        [SerializeField]
        GameObject m_HighlightPokePanel;

        [SerializeField]
        GameObject m_HighlightPinchPanel;

        [SerializeField]
        GameObject m_HighlightGrabPanel;

        [Header("Hand UI")]
        [SerializeField]
        Image m_LeftHandIcon;

        [SerializeField]
        Image m_RightHandIcon;

        [SerializeField]
        GameObject m_HandPackageWarningPanel;

        XRInteractionSimulator m_Simulator;
        SimulatedDeviceLifecycleManager m_DeviceLifecycleManager;
        SimulatedDeviceLifecycleManager.DeviceMode m_PreviousDeviceMode = SimulatedDeviceLifecycleManager.DeviceMode.None;
        TargetedDevices m_PreviousTargetedDeviceInput = TargetedDevices.None;
        ControllerInputMode m_PreviousControllerInputMode = ControllerInputMode.None;
        SimulatedHandExpression m_PreviousHandExpression = new SimulatedHandExpression();

        static readonly Color k_DisabledColor = new Color(0x70 / 255f, 0x70 / 255f, 0x70 / 255f);

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void Start()
        {
#if HAS_FIND_FIRST_OBJECT_BY_TYPE
            var simulator = Object.FindFirstObjectByType<XRInteractionSimulator>();
#else
            var simulator = Object.FindObjectOfType<XRInteractionSimulator>();
#endif
            if (simulator != null)
            {
                m_Simulator = simulator;
            }
            else
            {
                Debug.LogError($"Could not find the XRInteractionSimulator component, disabling simulator UI.", this);
                gameObject.SetActive(false);
                return;
            }

            if (!m_Simulator.gameObject.TryGetComponent(out m_DeviceLifecycleManager))
            {
                Debug.LogError($"Could not find SimulatedDeviceLifecycleManager component on {m_Simulator.name}, disabling simulator UI.", this);
                gameObject.SetActive(false);
                return;
            }

#if !XR_HANDS_1_1_OR_NEWER
            m_HandPackageWarningPanel.SetActive(true);
            m_LeftHandIcon.color = k_DisabledColor;
            m_RightHandIcon.color = k_DisabledColor;
#endif

        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void Update()
        {
            HandleHighlightedDevicePanels();
            HandleHighlightedControllerActionPanels();
            HandleHighlightedHandActionPanels();
            HandleActiveMenus();
        }

        /// <summary>
        /// Toggles the visibility of the input selection menu.
        /// </summary>
        public void OpenCloseInputSelectionMenu()
        {
            if (m_InputSelectionMenu.activeSelf)
            {
                m_ClosedInputSelectionMenu.SetActive(true);
                m_InputSelectionMenu.SetActive(false);
            }
            else
            {
                m_ClosedInputSelectionMenu.SetActive(false);
                m_InputSelectionMenu.SetActive(true);
            }
        }

        /// <summary>
        /// Toggles the visibility of for the controller actions menu.
        /// </summary>
        public void OpenCloseControllerActionsMenu()
        {
            if (m_ControllerActionsMenu.activeSelf)
            {
                m_ClosedControllerActionsMenu.SetActive(true);
                m_ControllerActionsMenu.SetActive(false);
            }
            else
            {
                m_ClosedControllerActionsMenu.SetActive(false);
                m_ControllerActionsMenu.SetActive(true);
            }
        }

        /// <summary>
        /// Toggles the visibility of for the hand actions menu.
        /// </summary>
        public void OpenCloseHandActionsMenu()
        {
            if (m_HandActionsMenu.activeSelf)
            {
                m_ClosedHandActionsMenu.SetActive(true);
                m_HandActionsMenu.SetActive(false);
            }
            else
            {
                m_ClosedHandActionsMenu.SetActive(false);
                m_HandActionsMenu.SetActive(true);
            }
        }

        void HandleActiveMenus()
        {
            if (m_PreviousDeviceMode != m_DeviceLifecycleManager.deviceMode && !m_Simulator.manipulatingFPS)
            {
                if (m_Simulator.manipulatingLeftController || m_Simulator.manipulatingRightController)
                {
                    m_HandActionsMenu.SetActive(false);
                    m_ClosedHandActionsMenu.SetActive(false);
                    m_ClosedControllerActionsMenu.SetActive(true);
                }
                else if (m_Simulator.manipulatingLeftHand || m_Simulator.manipulatingRightHand)
                {
                    m_ControllerActionsMenu.SetActive(false);
                    m_ClosedControllerActionsMenu.SetActive(false);
                    m_ClosedHandActionsMenu.SetActive(true);
                }

                m_PreviousDeviceMode = m_DeviceLifecycleManager.deviceMode;
            }

            if (m_Simulator.manipulatingFPS)
            {
                m_HandActionsMenu.SetActive(false);
                m_ClosedHandActionsMenu.SetActive(false);
                m_ControllerActionsMenu.SetActive(false);
                m_ClosedControllerActionsMenu.SetActive(false);

                m_PreviousDeviceMode = SimulatedDeviceLifecycleManager.DeviceMode.None;
            }

            if (m_ToggleActionMenu.ReadWasPerformedThisFrame())
            {
                if (m_Simulator.manipulatingLeftController || m_Simulator.manipulatingRightController)
                    OpenCloseControllerActionsMenu();
                else if (m_Simulator.manipulatingLeftHand || m_Simulator.manipulatingRightHand)
                    OpenCloseHandActionsMenu();
            }

            if (m_ToggleInputSelectionMenu.ReadWasPerformedThisFrame())
                OpenCloseInputSelectionMenu();
        }

        void HandleHighlightedDevicePanels()
        {
            if (m_Simulator.targetedDeviceInput != m_PreviousTargetedDeviceInput || m_PreviousDeviceMode != m_DeviceLifecycleManager.deviceMode)
            {
                ClearHighlightedDevicePanels();

                if (m_Simulator.manipulatingFPS)
                {
                    m_HighlightFullBodyPanel.SetActive(true);
                    return;
                }

                if (m_Simulator.manipulatingLeftController)
                {
                    m_HighlightLeftControllerPanel.SetActive(true);
                }

                if (m_Simulator.manipulatingRightController)
                {
                    m_HighlightRightControllerPanel.SetActive(true);
                }

                if (m_Simulator.manipulatingLeftHand)
                {
                    m_HighlightLeftHandPanel.SetActive(true);
                }

                if (m_Simulator.manipulatingRightHand)
                {
                    m_HighlightRightHandPanel.SetActive(true);
                }

                if (m_Simulator.manipulatingHMD)
                {
                    m_HighlightHeadPanel.SetActive(true);
                }

                m_PreviousTargetedDeviceInput = m_Simulator.targetedDeviceInput;
            }
        }

        void HandleHighlightedControllerActionPanels()
        {
            if (m_Simulator.controllerInputMode != m_PreviousControllerInputMode)
            {
                ClearHighlightedControllerActionPanels();
                switch (m_Simulator.controllerInputMode)
                {
                    case ControllerInputMode.Trigger:
                        m_HighlightTriggerPanel.SetActive(true);
                        break;
                    case ControllerInputMode.Grip:
                        m_HighlightGripPanel.SetActive(true);
                        break;
                    case ControllerInputMode.PrimaryButton:
                        m_HighlightPrimaryPanel.SetActive(true);
                        break;
                    case ControllerInputMode.SecondaryButton:
                        m_HighlightSecondaryPanel.SetActive(true);
                        break;
                }

                m_PreviousControllerInputMode = m_Simulator.controllerInputMode;
            }
        }

        void HandleHighlightedHandActionPanels()
        {
            if (m_Simulator.currentHandExpression != m_PreviousHandExpression)
            {
                ClearHighlightedHandActionPanels();
                switch (m_Simulator.currentHandExpression.name)
                {
                    case "Poke":
                        m_HighlightPokePanel.SetActive(true);
                        break;
                    case "Pinch":
                        m_HighlightPinchPanel.SetActive(true);
                        break;
                    case "Grab":
                        m_HighlightGrabPanel.SetActive(true);
                        break;
                }

                m_PreviousHandExpression = m_Simulator.currentHandExpression;
            }

        }

        void ClearHighlightedDevicePanels()
        {
            m_HighlightFullBodyPanel.SetActive(false);
            m_HighlightLeftControllerPanel.SetActive(false);
            m_HighlightRightControllerPanel.SetActive(false);
            m_HighlightLeftHandPanel.SetActive(false);
            m_HighlightRightHandPanel.SetActive(false);
            m_HighlightHeadPanel.SetActive(false);
        }

        void ClearHighlightedControllerActionPanels()
        {
            m_HighlightTriggerPanel.SetActive(false);
            m_HighlightGripPanel.SetActive(false);
            m_HighlightPrimaryPanel.SetActive(false);
            m_HighlightSecondaryPanel.SetActive(false);
        }

        void ClearHighlightedHandActionPanels()
        {
            m_HighlightPokePanel.SetActive(false);
            m_HighlightPinchPanel.SetActive(false);
            m_HighlightGrabPanel.SetActive(false);
        }
    }
}
