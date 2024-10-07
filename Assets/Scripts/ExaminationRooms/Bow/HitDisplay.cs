using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDisplay : MonoBehaviour
{
    [SerializeField] Material[] displayMaterials;

    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(int materialToSet)
    {
        meshRenderer.material = displayMaterials[materialToSet];
    }
}
