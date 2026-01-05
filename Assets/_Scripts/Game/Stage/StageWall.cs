using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageWall : MonoBehaviour
{

    public int StageNum = 0;
    public int MaxStage = 0;
    // Start is called before the first frame update
    void Start()
    {
        MaxStage = StatusManager.Instance.MaxStage;
        if (2 <= StageNum && StageNum-2 < MaxStage)
        {
            if (StatusManager.Instance.StageOpen[StageNum - 2] == 1)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
