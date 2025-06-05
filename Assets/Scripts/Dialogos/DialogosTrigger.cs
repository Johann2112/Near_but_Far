using UnityEngine;

public class DialogosTrigger : MonoBehaviour
{
    [SerializeField] private Dialogos dialogosSystem;
    [SerializeField] private int dialogueIndex;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogosSystem.TriggerDialogue(dialogueIndex);
            Debug.Log("Entro");
            Destroy(gameObject);
        }
    }
}
