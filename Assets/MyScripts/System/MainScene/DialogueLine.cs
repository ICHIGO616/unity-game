using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public enum Speaker { Villager, Player }
    public Speaker speaker;

    [TextArea(2, 4)]
    public string text;
}