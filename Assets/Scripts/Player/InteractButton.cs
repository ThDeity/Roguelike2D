using UnityEngine;

public class InteractButton : MonoBehaviour
{
    public Interactive interactiveObj;

    public void Interact()
    {
        if (interactiveObj == null) return;
        Debug.Log(interactiveObj);

        interactiveObj.Interact();
    }
}