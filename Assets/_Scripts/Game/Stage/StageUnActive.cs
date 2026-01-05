using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUnActive : MonoBehaviour
{
    public Stage ActiveStage;
    public PlayerController PC;
    private void FixedUpdate()
    {
        if(!PC.OnStage)
        {
            if (ActiveStage.isActive)
            {
                ActiveStage.UnActiveSpanwer();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Car"))
        {
            if (ActiveStage.isActive)
            {
                ActiveStage.UnActiveSpanwer();
                PC.OnStage = false;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Car"))
        {
            if (!ActiveStage.isActive)
            {
                PC.OnStage = true;
            }
            
        }
    }
}
