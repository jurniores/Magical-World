using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teste : MonoBehaviour
{
    // Start is called before the first frame update
    List<Coroutine> IE = new();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IE.Add(StartCoroutine(ENumTest(2)));
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            IE.ForEach(e=>StopCoroutine(e));
        }
    }

    IEnumerator ENumTest(float time)
    {
        yield return new WaitForSeconds(time);
        print("IE");
    }
}
