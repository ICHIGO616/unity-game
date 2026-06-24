using UnityEngine;
using System.Collections;

public class DoorTextTrigger : MonoBehaviour
{
    [Header("表示するテキスト")]
    public DialogueLine[] Lines;

    [Header("Timing")]
    public float delayAfterOpen = 3.0f; // 扉を開けてから表示するまでの秒数

    private bool hasTriggered = false;

    // FpsHorrorKitの扉スクリプトから呼び出す
    public void OnDoorOpened()
    {
        if(hasTriggered) return;
        hasTriggered = true;
        StartCoroutine(ShowTextAfterDelay());
    }

    IEnumerator ShowTextAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterOpen);

        // 名前は空文字
        DialogueManager.Instance.StartDialogue("", Lines);
    }
}