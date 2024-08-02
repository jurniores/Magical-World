using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllsMaterials : MonoBehaviour
{
    private Renderer render;
    private Material material;
    [SerializeField]
    private float time = 5;
    private float range = 1;
    [SerializeField]
    private string property;

    void Start()
    {
        render = GetComponent<Renderer>();

        material = render.material;
    }

    void Update()
    {
        if (range <= 1)
        {
            range += Time.deltaTime/time;
            material.SetFloat($"_{property}", range);
        }
        
        
    }
}
