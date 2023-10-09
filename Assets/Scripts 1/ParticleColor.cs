using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleColor : MonoBehaviour
{
    public Color newColor;

    private new ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        ChangeColor();
    }

    private void ChangeColor()
    {
        var mainModule = particleSystem.main;
        mainModule.startColor = newColor;
    }
}

