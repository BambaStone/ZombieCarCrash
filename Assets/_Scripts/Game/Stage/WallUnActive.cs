using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallUnActive : MonoBehaviour
{
    public List<BrickUnActive> BrickLines;
    public int InputCoin = 0;
    public int NeedCoin = 200;
    public CoinSounds CS;
    private bool _startUnActive = false;
    private bool _unActiveSucces = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DropCoin"))
        {
            CS.OnCoinSound(other.transform.position);
            other.gameObject.SetActive(false);
            InputCoin++;
            if(NeedCoin<=InputCoin)
            {
                StartUnActiveWall();
            }
        }
    }
    private void StartUnActiveWall()
    {
        _startUnActive = true;
        for (int i = 0; i <BrickLines.Count;i++)
        {
            BrickLines[i].StartUnActiveBricks();
        }
    }
    private void FixedUpdate()
    {
        if (_startUnActive)
        {
            for (int i = 0; i < BrickLines.Count; i++)
            {
                if (!BrickLines[i].EndUnActive)
                {
                    _unActiveSucces = false;
                    break;
                }
                else
                {
                    _unActiveSucces = true;
                }
            }
            if (_unActiveSucces)
            {
                Destroy(gameObject);
            }
        }
    }
}
