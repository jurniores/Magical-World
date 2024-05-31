using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelaInicialUI : MonoBehaviour
{
    [SerializeField]
    private GameObject telaPrincipal, TelainvLoja;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AbreTelaInvLoja()
    {
        if (telaPrincipal.activeSelf)
        {
            telaPrincipal.SetActive(false);
            TelainvLoja.SetActive(true);
            return;
        }

        telaPrincipal.SetActive(true);
        TelainvLoja.SetActive(false);
    }

}
