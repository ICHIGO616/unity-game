using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance {get; private set;}

    [Header("UI References")]
    public GameObject DialoguePanel;
    public TextMeshProUGUI SpeakerNameText;
    public TextMeshProUGUI DialogueText;
    public GameObject NextIndicator;

    [Header("Speaker")]
    public string PlayerName = "";

    private DialogueLine[] currentLines;
    private string villagerName;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool dialogueJustStarted = false;
    private Action onDialogueEndCallback;

    private FpsHorrorKit.FpsController fpsController;
    private UnityEngine.InputSystem.PlayerInput playerInput;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        DialoguePanel.SetActive(false);

        fpsController = FindFirstObjectByType<FpsHorrorKit.FpsController>();
        playerInput = FindFirstObjectByType<UnityEngine.InputSystem.PlayerInput>();
    }

    void Update()
    {
        if(isDialogueActive)
        {
            if(dialogueJustStarted)
            {
                dialogueJustStarted = false;
                return;
            }

            if(Input.GetMouseButtonDown(0))
            {
                NextLine();
            }
        }
    }

    public void StartDialogue(string vName, DialogueLine[] lines, Action onEnd = null)
    {
        villagerName = vName;
        currentLines = lines;
        currentLineIndex = 0;
        isDialogueActive = true;
        dialogueJustStarted = true;
        onDialogueEndCallback = onEnd;

        DialoguePanel.SetActive(true);
        ShowLine();

        SetPlayerControl(false);
    }

    void ShowLine()
    {
        var line = currentLines[currentLineIndex];

        //会話者によって名前を切り替え
        SpeakerNameText.text = line.speaker == DialogueLine.Speaker.Villager ? villagerName : PlayerName;

        DialogueText.text = line.text;
        NextIndicator.SetActive(currentLineIndex < currentLines.Length - 1);
    }

    void NextLine()
    {
        if(currentLineIndex < currentLines.Length - 1)
        {
            currentLineIndex++;
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        DialoguePanel.SetActive(false);
        currentLines = null;

        SetPlayerControl(true);

        onDialogueEndCallback?.Invoke();
        onDialogueEndCallback = null;
    }

    void SetPlayerControl(bool enabled)
    {
        if(fpsController != null)
        {
            fpsController.enabled = enabled;
        }

        if(playerInput != null)
        {
            playerInput.enabled = enabled;
        }
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }
}
