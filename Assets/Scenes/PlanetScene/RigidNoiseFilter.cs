using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    // Start is called before the first frame update
    NoiseSettings.RigidNoiseSettings settings;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this.settings = settings;
    }

    Noise noise = new Noise();

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.BaseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.NumLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.Centre));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier);


            noiseValue += v * amplitude;
            frequency *= settings.Roughness;
            amplitude *= settings.Persitence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.MinValue);
        return noiseValue * settings.Strength;
    }
}
