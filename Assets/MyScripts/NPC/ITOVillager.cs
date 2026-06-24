namespace FpsHorrorKit
{
    using UnityEngine;

    public class ITOVillager : MonoBehaviour, IInteractable
    {
        [Header("Talk Setting")]
        public string VillagerName = "村人";
        public DialogueLine[] DialogueLines; 

        public GameObject BookObject;

        private VillagerController villagerController;
        private bool hasSpoken = false; //一回でも話しかけたかどうか

        void Start()
        {
            villagerController = GetComponent<VillagerController>();

            if(BookObject != null)
            {
                BookObject.SetActive(false);
            }

        }

        public void Interact()
        {
            if(DialogueManager.Instance.IsDialogueActive())
            {
                return;
            }
            if(hasSpoken)
            {
                return;
            }

            hasSpoken = true;

            if(villagerController != null)
            {
                villagerController.OnTalkStart();
            }

            DialogueManager.Instance.StartDialogue(VillagerName, DialogueLines, OnDialogueEnd);
        }

        void OnDialogueEnd()
        {
            if(villagerController != null)
            {
                villagerController.OnTalkEnd();
            }

            if(BookObject != null)
            {
                BookObject.SetActive(true);
            }
        }

        public void Highlight()
        {
            if(!hasSpoken && !DialogueManager.Instance.IsDialogueActive())
            {
                PlayerInteract.Instance.ChangeInteractText("Talk [E]");
            }
            else
            {
                PlayerInteract.Instance.UnHighlight();
            }
        }

        public void HoldInteract(){}
        public void UnHighlight(){}
    }
}