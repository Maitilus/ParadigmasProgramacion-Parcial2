using System.Collections.Generic;
using UnityEngine;

public class TrapButton : MonoBehaviour, IInteraction
{
    [SerializeField] private DartTrap Trap1, Trap2, Trap3, Trap4, Trap5;
    public void Interact()
    {
        Trap1.Activate();
        Trap2.Activate();
        Trap3.Activate();
        Trap4.Activate();
        Trap5.Activate();
    }
}
