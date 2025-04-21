// Copyright (c) Mixed Reality Toolkit Contributors
// Licensed under the BSD 3-Clause

using UnityEngine;
using TMPro;

namespace MixedReality.Toolkit.Examples
{
    using System.Collections;

    /// <summary>
    /// Handles events triggered from the attached <see cref="StatefulInteractable"/>
    /// </summary>
    [AddComponentMenu("Scripts/MRTK/Examples/EyeTrackingTargetX")]
    public class EyeTrackingTargetX : MonoBehaviour
    {
        [Tooltip("Visual effect (e.g., particle explosion or animation) that is played when a target is selected.")]
        [SerializeField]
        private GameObject visualEffectsOnHit = null;
        [Tooltip("Visual effect (e.g., particle explosion or animation) that is played when a target is selected.")]
        [SerializeField]
        private GameObject visualEffectsOnHit2 = null;

        [Tooltip("Audio clip that is played when a target is selected.")]
        [SerializeField]
        private AudioClip audioFxCorrectTarget = null;

        [Tooltip("Audio clip that is played when a wrong target is selected.")]
        [SerializeField]
        private AudioClip audioFxIncorrectTarget = null;

        [Tooltip("Manually indicate whether this is an incorrect target.")]
        [SerializeField]
        private bool isValidTarget = true;

        [Tooltip("Euler angles by which the object should be rotated by.")]
        [SerializeField]
        private Vector3 rotateByEulerAngles = Vector3.zero;

        [Tooltip("Rotation speed factor.")]
        [SerializeField]
        private float speed = 1f;

        /// <summary>
        /// Coroutine that plays when the game object is hovered over.
        /// </summary>
        private Coroutine rotationCoroutine;

        /// <summary>
        /// Internal audio source associated with the game object.
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// The StatefulInteractable associated with this game object.
        /// </summary>
        private StatefulInteractable interactable;
        private TMP_Text paragraph;

        private void Awake()
        {
            SetUpAudio();
            interactable = GetComponent<StatefulInteractable>();
        }

        /// <summary>
        /// Called when a user begins a hover on the GameObject using a gaze based interactor.
        /// </summary>
        public void OnGazeHoverEntered()
        {
            rotationCoroutine = StartCoroutine(RotateTarget());
        }

        /// <summary>
        /// Called when a user leaves a hover on the GameObject using a gaze based interactor.
        /// </summary>
        public void OnGazeHoverExited()
        {
            StopCoroutine(rotationCoroutine);
        }

        /// <summary>
        /// Called when a user selects the GameObject.
        /// </summary>
        public void OnTargetSelected()
        {
            if (!interactable.isHovered)
            {
                return;
            }

            if (!isValidTarget)
            {
                
                return;
            }

            // Play audio clip
            float audioClipLength = PlayAudioOnHit(audioFxCorrectTarget);

            // Play animation
            float animationLength = PlayAnimationOnHit();

            // Destroy target
            gameObject.SetActive(true);
        }

        private void SetUpAudio()
        {
            audioSource = gameObject.EnsureComponent<AudioSource>();

            audioSource.playOnAwake = false;
            audioSource.enabled = true;
        }

        /// <summary>
        /// Play given audio clip.
        /// </summary>
        private float PlayAudioOnHit(AudioClip audioClip)
        {
            if (audioClip == null || audioSource == null)
            {
                return 0f;
            }

            // Play the given audio clip
            audioSource.clip = audioClip;
            audioSource.PlayOneShot(audioSource.clip);
            return audioSource.clip.length;
        }

        /// <summary>
        /// Show given GameObject when target is selected. 
        /// </summary>
        public void hideTarget() {
            if (!interactable.isHovered)
            {
                return;
            }

            if (!isValidTarget)
            {
                
                return;
            }

            if (visualEffectsOnHit == null || visualEffectsOnHit2 == null)
            {
                return ;
            }
            // visualEffectsOnHit.SetActive(false);
            visualEffectsOnHit2.SetActive(false);
        }
        public void showTarget() {
            if (!interactable.isHovered)
            {
                return;
            }

            if (!isValidTarget)
            {
                
                return;
            }
            if (visualEffectsOnHit == null || visualEffectsOnHit2 == null)
            {
                return ;
            }
            // visualEffectsOnHit.SetActive(true);
            visualEffectsOnHit2.SetActive(true);
        }
        private float PlayAnimationOnHit()
        {
            if (visualEffectsOnHit == null)
            {
                return 0f;
            }

            visualEffectsOnHit.SetActive(true);
            paragraph = visualEffectsOnHit.GetComponent<TMP_Text>();
            if (gameObject.CompareTag("Stadium")) {
                paragraph.text = "This is a Stadium\n, A large open-air venue with seating, likely used for sports or public events, identifiable by its oval or circular shape.";
            } else if (gameObject.CompareTag("plane")) {
                paragraph.text = "this is a plane\n, ";
            } else if (gameObject.CompareTag("cars")){
                paragraph.text = "This is a traffic\n, Several vehicles are visible on the roads, representing typical city traffic and transportation infrastructure.";
            } else if (gameObject.CompareTag("residental")){
                paragraph.text = "This is a building\n, A multi-story structure likely serving as housing for city residents, often seen with balconies or windows aligned vertically.";
            } else if (gameObject.CompareTag("mountains")){
                paragraph.text = "This is a mountain\n, A natural elevated landform in the background, giving geographical context and enhancing the landscape’s diversity.";
            } else if (gameObject.CompareTag("House")){
                paragraph.text = "This is a house\n, Individual standalone buildings, often smaller than residential towers, suggesting suburban or low-density housing areas.";
            } else if (gameObject.CompareTag("hotBaloon")){
                paragraph.text = "This is a hot balloon\n, A colorful balloon floating above the landscape, adding a scenic and tourist-attraction element to the area.";
            } else if (gameObject.CompareTag("airport")){
                paragraph.text = "This is an airport\n, A large complex featuring runways, terminals, and sometimes control towers, used for the arrival and departure of aircraft.";
            }
            
            
            return visualEffectsOnHit.GetComponent<ParticleSystem>().main.duration;
        }

        /// <summary>
        /// Rotates game object based on specified rotation speed and Euler angles.
        /// </summary>
        private IEnumerator RotateTarget()
        {
            while (true)
            {
                transform.eulerAngles += speed * rotateByEulerAngles;
                yield return null;
            }
        }
    }
}
