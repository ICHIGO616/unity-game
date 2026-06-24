namespace FpsHorrorKit
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class ITOBook : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactText = "記録を書く [E]";

        [Header("遷移設定")]
        public Image FadeImage;
        public string nextSceneName = "DayEndScene"; // 遷移先のシーン名
        public float fadeDuration = 2.0f;

        public void Interact()
        {
            MonoBehaviour mono = FindFirstObjectByType<MonoBehaviour>();
            mono.StartCoroutine(FadeAndLoad());
        }

        System.Collections.IEnumerator FadeAndLoad()
        {
            // プレイヤー操作を無効化
            var fpsController = FindFirstObjectByType<FpsController>();
            if(fpsController != null) fpsController.enabled = false;

            var playerInput = FindFirstObjectByType<UnityEngine.InputSystem.PlayerInput>();
            if(playerInput != null) playerInput.enabled = false;

            // 暗転
            float elapsed = 0f;
            while(elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                if(FadeImage != null)
                    FadeImage.color = new Color(0, 0, 0, elapsed / fadeDuration);
                yield return null;
            }

            SceneManager.LoadScene(nextSceneName);
        }

        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractText(interactText);
        }

        public void HoldInteract() { }
        public void UnHighlight() { }
    }
}
