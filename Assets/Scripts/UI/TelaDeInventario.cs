using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelaDeInventario : MonoBehaviour
{
    [SerializeField]
    private GameObject[] telaParaAbrir = new GameObject[5];

    public void AbrirTela(GameObject minhaTela){
        foreach(GameObject obj in telaParaAbrir){
            obj.SetActive(false);
        }

        minhaTela.SetActive(true);
    }

}
