using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActive : MonoBehaviour
{
    public Stage ActiveStage;
    public PlayerController PC;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            if (PC.OnStage)
            {
                if (!ActiveStage.isActive)
                {
                    ActiveStage.ActiveSpanwer();
                }
            }
        }
    }
}
