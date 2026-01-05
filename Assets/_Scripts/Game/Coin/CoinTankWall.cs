using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTankWall : MonoBehaviour//코인탱크의 벽면에서 코인이 코인탱크 밖으로 나가는지 감지하는 스크립트
{
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Coin"))//코인탱크에서 코인이 코인탱크 밖으로 탈출시
        {
            other.transform.position = transform.position;//코인의 위치를 코인탱크중심으로 이동
        }
    }
}
