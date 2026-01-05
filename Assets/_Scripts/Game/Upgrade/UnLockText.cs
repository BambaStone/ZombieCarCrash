using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockText : MonoBehaviour
{
    public int UnLockLevel = 0;

    private void FixedUpdate()
    {
        if (UnLockLevel <= StatusManager.Instance.WeaponDamage)
        {
            gameObject.SetActive(false);
        }
    }
}
