using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteraction
{
    //Animacion para abrir y cerrar una puerta
    [SerializeField] Animation Animation;
    private bool isOpen;


    public void Interact()
    {
        if (isOpen)
        {
            Animation.Play("DoorClosing");
            isOpen = false;
        }
        else if (!isOpen)
        {
            Animation.Play("DoorOpening");
            isOpen = true;
        }
    }
}
