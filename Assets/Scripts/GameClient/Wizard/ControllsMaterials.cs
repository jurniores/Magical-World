using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllsMaterials : MonoBehaviour
{
    private Renderer renderer;
    private Material material;
    [SerializeField]
    private float time = 5;
    private float range = 0;
    [SerializeField]
    private string property;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        material = renderer.material;;
    }

    void Update()
    {
        if (range <= 1)
        {
            range += Time.deltaTime/time;
        }
        material.SetFloat($"_{property}", range);
    }

    void OnDisable()
    {
        range = 0;
    }
}
