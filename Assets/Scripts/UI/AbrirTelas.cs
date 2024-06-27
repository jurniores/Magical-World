using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirTelas : MonoBehaviour
{
    [SerializeField]
    private GameObject[] telaParaAbrir = new GameObject[5];

    public void AbrirTela(GameObject minhaTela){
        gameObject.SetActive(true);
        foreach(GameObject obj in telaParaAbrir){
            
            if(obj != minhaTela)obj.SetActive(false);
        }

        minhaTela.SetActive(true);
    }

}
