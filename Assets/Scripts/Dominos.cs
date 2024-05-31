using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dominos : MonoBehaviour
{
    [SerializeField]
    private int lado1, lado2;
    [SerializeField]
    private GameObject dominos;
    private Transform areaDeJogo;

    
    void Start()
    {
        areaDeJogo = GameObject.Find("AreadeJogo").transform;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InstDominosP()
    {
        GameObject objtInstanciado = Instantiate(dominos, Vector2.zero, Quaternion.identity);

        //Setei no parente
        
        objtInstanciado.transform.SetParent(areaDeJogo);
        //Setando escalas posições e valores

        Vector3 pos = objtInstanciado.transform.position;
        objtInstanciado.transform.position = new Vector3(pos.x, pos.y, 1);
        objtInstanciado.transform.localScale = Vector3.one;

        //setandos os mesmos valores do dominó que instancia
        DominosInstance inst = objtInstanciado.GetComponent<DominosInstance>();
        inst.Val(lado1, lado2);

    }
}
