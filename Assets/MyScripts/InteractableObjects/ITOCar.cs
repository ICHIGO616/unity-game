namespace FpsHorrorKit
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class ITOCar : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactText = "Get in the car [E]";

        [Header("References")]
        public Image FadeImage;
        public AmbientSoundManager AmbientSoundManager;

        [Header("Timing")]
        public float fadeDuration = 2.0f;
        public float waitDuration = 1.0f;

        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Interact()
        {
            var fpsController = FindFirstObjectByType<FpsController>();
            if(fpsController != null)
            {
                fpsController.enabled = false;
            }

            var playerInput = FindFirstObjectByType<UnityEngine.InputSystem.PlayerInput>();
            if(playerInput != null)
            {
                playerInput.enabled = false;
            }

            if(AmbientSoundManager != null)
            {
                AmbientSoundManager.StopAllBGM();
            }

            MonoBehaviour mono = FindFirstObjectByType<MonoBehaviour>();
            mono.StartCoroutine(GameClearSequence());
        }

        IEnumerator GameClearSequence()
        {
            float elapsed = 0f;
            while(elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;

                if(FadeImage != null)
                {
                    FadeImage.color = new Color(0,0,0,t);
                }

                yield return null;
            }

            yield return new WaitForSeconds(waitDuration);
            SceneManager.LoadScene("ClearScene");
        }

        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractText(interactText);
        }

        public void HoldInteract(){}
        public void UnHighlight(){}
    }
}