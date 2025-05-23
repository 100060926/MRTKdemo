// Copyright (c) Mixed Reality Toolkit Contributors
// Licensed under the BSD 3-Clause

using MixedReality.Toolkit.Input.Simulation;
using MixedReality.Toolkit.Subsystems;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityInputSystem = UnityEngine.InputSystem;

namespace MixedReality.Toolkit.Input
{
    /// <summary>
    /// Basic controller visualizer which renders the a controller model when one is detected.
    /// The platform controller model is used when available, otherwise a generic controller model is used.
    /// </summary>
    [AddComponentMenu("MRTK/Input/Controller Visualizer")]
    public class ControllerVisualizer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The XRNode on which this hand is located.")]
        private XRNode handNode = XRNode.LeftHand;

        /// <summary> The XRNode on which this hand is located. </summary>
        public XRNode HandNode { get => handNode; set => handNode = value; }

        [SerializeField]
        [Tooltip("A fallback controller model to render in case the platform model fails to load")]
        private GameObject fallbackControllerModel;

        // The controller usages we want the input device to have;
        private InternedString targetUsage;

        // A GameObject representing the root which contains any loaded platform models.
        // This root is necessary since platform models are rotated 180 degrees by default.
        private GameObject platformLoadedGameObjectRoot;

        // A GameObject representing the fallback controller model.
        private GameObject fallbackGameObject;

        /// <summary>
        /// Cached reference to hands aggregator for efficient per-frame use.
        /// </summary>
        [Obsolete("Deprecated, please use XRSubsystemHelpers.HandsAggregator instead.")]
        protected HandsAggregatorSubsystem HandsAggregator => XRSubsystemHelpers.HandsAggregator as HandsAggregatorSubsystem;

        [SerializeField]
        [Tooltip("The input action we key into to determine whether this controller is tracked or not")]
        private InputActionProperty controllerDetectedAction;

        /// <summary>
        /// A Unity event function that is called when the script component has been enabled.
        /// </summary>
        protected void OnEnable()
        {
            Debug.Assert(handNode == XRNode.LeftHand || handNode == XRNode.RightHand, $"HandVisualizer has an invalid XRNode ({handNode})!");

            switch (handNode)
            {
                case XRNode.LeftHand:
                    targetUsage = UnityInputSystem.CommonUsages.LeftHand;
                    break;
                case XRNode.RightHand:
                    targetUsage = UnityInputSystem.CommonUsages.RightHand;
                    break;
                default:
                    break;
            }

            if (controllerDetectedAction.action == null) { return; }
            controllerDetectedAction.action.started += RenderControllerVisuals;
            controllerDetectedAction.action.canceled += RemoveControllerVisuals;
            controllerDetectedAction.EnableDirectAction();
        }

        /// <summary>
        /// A Unity event function that is called when the script component has been disabled.
        /// </summary>
        protected void OnDisable()
        {
            if (controllerDetectedAction.action == null) { return; }
            controllerDetectedAction.DisableDirectAction();
            controllerDetectedAction.action.started -= RenderControllerVisuals;
            controllerDetectedAction.action.canceled -= RemoveControllerVisuals;
        }

        /// <summary>
        /// A Unity event function that is called every frame, if this object is enabled.
        /// </summary>
        protected void Update()
        {
            // If we're currently rendering controller models, we want to update the visibility every frame
            // in case hand joints are intermittently tracked. The check at controller model instantiation may not be sufficient.
            if (controllerDetectedAction.action?.inProgress ?? false)
            {
                if (XRSubsystemHelpers.HandsAggregator == null ||
                    !XRSubsystemHelpers.HandsAggregator.TryGetJoint(TrackedHandJoint.Palm, handNode, out _))
                {
                    RenderControllerVisuals(controllerDetectedAction.action.activeControl.device, true);
                }
                // Only remove the fallback controller. Some runtimes provide synthesized hand joints when using controllers,
                // and we may want to render them conforming to the controller model.
                else if (controllerGameObject != null && controllerGameObject == fallbackGameObject)
                {
                    RemoveControllerVisuals();
                }
            }
        }

        private void RenderControllerVisuals(InputAction.CallbackContext context)
        {
            RenderControllerVisuals(context.control.device);
        }

        private void RenderControllerVisuals(UnityInputSystem.InputDevice inputDevice, bool forceAllowFallback = false)
        {
            // We're already loading or rendering a controller, so there's no need to change it
            if (controllerTask != null || controllerGameObject != null)
            {
                return;
            }

            // This process may change in the future as unity updates its input subsystem.
            // In the future, there will be a different way of distinguishing between physical controllers
            // and tracked hands, forgoing the UnityEngine.XR.InputDevices route

            // Upon detecting a generic input device with the appropriate usages, load or remove the controller visuals
            // when appropriate
            if (inputDevice is UnityInputSystem.XR.XRController xrInputDevice && xrInputDevice.usages.Contains(targetUsage))
            {
                // Fallback visuals are only used if NO hand joints are detected
                // OR the input device is specifically a simulated controller that is in the MotionController Simulation Mode.
                if (xrInputDevice is MRTKSimulatedController simulatedController)
                {
                    InstantiateControllerVisuals(inputDevice.deviceId.ToString(), false, simulatedController.SimulationMode == ControllerSimulationMode.MotionController);
                }
                else
                {
                    InstantiateControllerVisuals(inputDevice.deviceId.ToString(), true, forceAllowFallback ||
                        XRSubsystemHelpers.HandsAggregator == null || !XRSubsystemHelpers.HandsAggregator.TryGetJoint(TrackedHandJoint.Palm, handNode, out _));
                }
            }
        }

        // Private reference to the GameObject which represents the visualized controller
        // Needs to be explicitly set to null in cases where no controller visuals are ever loaded.
        private GameObject controllerGameObject = null;
        private Task<GameObject> controllerTask = null;

        // Platform models are "rotated" 180 degrees because their forward vector points towards the user.
        private static readonly Quaternion ControllerModelRotatedOffset = Quaternion.Euler(0, 180, 0);

        /// <summary>
        /// Tries to instantiate controller visuals for the specified input device
        /// </summary>
        /// <param name="inputDevice">The input device we want to generate visuals for</param>
        /// <param name="usePlatformVisuals">Whether or not to try to load visuals from the platform provider</param>
        /// <param name="useFallbackVisuals">Whether or not to use the fallback controller visuals</param>
        private async void InstantiateControllerVisuals(string inputDeviceKey, bool usePlatformVisuals, bool useFallbackVisuals)
        {
            // Disable any preexisting controller models before trying to render new ones.
            if (platformLoadedGameObjectRoot != null)
            {
                platformLoadedGameObjectRoot.SetChildrenActive(false);
                platformLoadedGameObjectRoot.SetActive(false);
            }
            if (fallbackGameObject != null)
            {
                fallbackGameObject.SetActive(false);
            }

            // Try to load the controller model from the platform
            if (usePlatformVisuals)
            {
                GameObject platformLoadedGameObject = await (controllerTask = ControllerModelLoader.TryGenerateControllerModelFromPlatformSDK(inputDeviceKey, handNode.ToHandedness()));
                if (platformLoadedGameObject != null)
                {
                    // Platform models are "rotated" 180 degrees because their forward vector points towards the user.
                    // We need to rotate these models in order to have them pointing in the correct direction on device.
                    if (platformLoadedGameObjectRoot == null)
                    {
                        platformLoadedGameObjectRoot = new GameObject("Platform Model Root");
                    }
                    platformLoadedGameObject.transform.parent = platformLoadedGameObjectRoot.transform;
                    platformLoadedGameObject.transform.SetPositionAndRotation(platformLoadedGameObjectRoot.transform.position, platformLoadedGameObjectRoot.transform.rotation * ControllerModelRotatedOffset);

                    controllerGameObject = platformLoadedGameObjectRoot;
                }
                controllerTask = null;
            }

            // If the ControllerGameObject is still not initialized after this, then use the fallback model if told to
            if (useFallbackVisuals && controllerGameObject == null)
            {
                if (fallbackGameObject == null && fallbackControllerModel != null)
                {
                    fallbackGameObject = Instantiate(fallbackControllerModel);
                }

                controllerGameObject = fallbackGameObject;
            }

            if (controllerGameObject != null)
            {
                controllerGameObject.SetActive(usePlatformVisuals || useFallbackVisuals);
                controllerGameObject.transform.parent = transform;
                controllerGameObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }
        }

        private void RemoveControllerVisuals(InputAction.CallbackContext obj) => RemoveControllerVisuals();

        private void RemoveControllerVisuals()
        {
            if (controllerGameObject != null)
            {
                controllerGameObject.SetActive(false);
                controllerGameObject = null;
            }
        }
    }
}
