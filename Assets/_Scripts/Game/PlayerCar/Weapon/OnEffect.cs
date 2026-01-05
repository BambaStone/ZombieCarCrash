using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEffect : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(UnActive());
    }

    IEnumerator UnActive()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
