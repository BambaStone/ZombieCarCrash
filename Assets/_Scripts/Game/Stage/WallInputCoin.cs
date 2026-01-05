using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WallInputCoin : MonoBehaviour
{
    public PlayerController PC;
    public GameObject Wall;
    public TMP_Text Text;
    public int InputCoin = 0;
    public int NeedCoin = 200;
    public int StageNum = 0;

    private void Start()
    {
        Wall.GetComponent<WallUnActive>().NeedCoin = NeedCoin;
        Text.text = "Need\nCoin\n"+NeedCoin;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            if (NeedCoin <= PC.Coin)
            {
                for (int i = 0; i < NeedCoin; i++)
                {
                    PC.MinusCoinWithTarget(Wall);
                }
                StatusManager.Instance.StageOpen[StageNum - 2] = 1;
                Destroy(gameObject);
            }
        }
    }
}
