using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickUnActive : MonoBehaviour
{
    public List<GameObject> Bricks;
    private int _nowBricks = 0;
    private bool _startUnActive = false;
    public bool EndUnActive = false;

    private void FixedUpdate()
    {
        if (_startUnActive)
        {
            for (int i = 0; i < _nowBricks; i++)
            {
                if (Bricks[i].activeSelf)
                {
                    Bricks[i].transform.localScale -= Vector3.one *3* Time.deltaTime;
                    if (Bricks[i].transform.localScale.x < 0)
                    {
                        if(i== Bricks.Count-1)
                        {
                            EndUnActive = true;
                        }
                        Bricks[i].SetActive(false);
                    }
                }
            }
        }
    }

    public void StartUnActiveBricks()
    {
        StartCoroutine(SmallerBricks());
        _startUnActive = true;
    }
    IEnumerator SmallerBricks()
    {
        yield return new WaitForSeconds(0.3f);
        if(_nowBricks<Bricks.Count)
        {
            _nowBricks++;
            StartCoroutine(SmallerBricks());
        }
    }

    
}
