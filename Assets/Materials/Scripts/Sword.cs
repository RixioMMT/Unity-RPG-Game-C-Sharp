using UnityEngine;

public class Sword : MonoBehaviour
{
    public SwordAttack swordAttack;

    public void OnSwingAnimationEnd()
    {
        if (swordAttack != null)
        {
            swordAttack.OnSwingAnimationEnd();
        }
    }
}