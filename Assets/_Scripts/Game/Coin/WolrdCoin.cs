using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WolrdCoin : MonoBehaviour
{
    public TMP_Text WorldCoinText;
    private int _wolrdCoin=0;
    void Start()
    {
        WorldCoinText = gameObject.GetComponent<TMP_Text>();
        _wolrdCoin = StatusManager.Instance.WorldCoin;
        WorldCoinText.text = _wolrdCoin + "";
    }

    private void FixedUpdate()
    {
        if(_wolrdCoin != StatusManager.Instance.WorldCoin)
        {
            _wolrdCoin = StatusManager.Instance.WorldCoin;
            WorldCoinText.text = _wolrdCoin + "";
        }
    }
}
