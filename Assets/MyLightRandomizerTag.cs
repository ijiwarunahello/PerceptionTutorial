using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;

// Add this Component to any GameObject that you would like to be randomized. This class must have an identical name to
// the .cs file it is defined in.
[RequireComponent(typeof(Light))]
public class MyLightRandomizerTag : RandomizerTag 
{
    public float minIntensity;
    public float maxIntensity;

    public void SetIntensity(float rawIntensity)
    {
        var tagLight = GetComponent<Light>();
        var scaledIntensity = rawIntensity * (maxIntensity - minIntensity) + minIntensity;
        tagLight.intensity = scaledIntensity;
    }
}

[Serializable]
[AddRandomizerMenu("MyLight Randomizer")]
public class MyLightRandomizer : Randomizer
{
    // Sample FloatParameter that can generate random floats in the [0,360) range. The range can be modified using the
    // Inspector UI of the Randomizer.
    public FloatParameter lightIntensity = new()
    {
        value = new UniformSampler(0, 1)
    };

    public ColorRgbParameter color;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<MyLightRandomizerTag>();
        foreach (var tag in tags)
        {
            // Get the light attached to the object
            var tagLight = tag.GetComponent<Light>();
            tagLight.color = color.Sample();
            // Call the SetIntensity function we defined in the tag instead!
            tag.SetIntensity(lightIntensity.Sample());
        }
    }
}
