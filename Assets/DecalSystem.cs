using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalSystem : MonoBehaviour
{
    public static DecalSystem instance;

    public void Awake()
    {
        instance = this;
    }
    
    public Material youMaterial , heMaterial , noneMaterial;
    public List<DecalProjector> decalPrefabs = new List<DecalProjector>();  

    public void SwitchMaterial(int index)
    {
        foreach (var decal in decalPrefabs)
        {
            decal.material = index == 0 ? youMaterial : index == 1 ? heMaterial : noneMaterial;
        }
    }
}
