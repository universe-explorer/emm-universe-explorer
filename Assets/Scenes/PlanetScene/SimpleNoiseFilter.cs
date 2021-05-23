using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    NoiseSettings.SimpleNoiseSettings settings;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }

    Noise noise = new Noise();

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.BaseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.NumLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.Centre);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= settings.Roughness;
            amplitude *= settings.Persitence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.MinValue);
        return noiseValue * settings.Strength;
    }

    
}
