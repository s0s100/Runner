using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BorderEffectControl : MonoBehaviour
{
    private const float MIN_REQUIRED_DIFFERENCE = 0.1f;
    private const float MAX_REQUIRED_DIFFERENCE = 0.9f;
    private const float MIN_VIGNETTE_SMOOTHNESS = 0.15f;
    private const float MAX_VIGNETTE_SMOOTHNESS = 0.5f;
    private const float MIN_FOCUS_DISTANCE = 1.0f;
    private const float MAX_FOCUS_DISTANCE = 5.0f;

    [SerializeField]
    private Volume volume;

    [SerializeField]
    private LevelGenerator levelGenerator;

    private GameObject playerObject;

    private void Start()
    {
        playerObject = levelGenerator.GetPlayer();
    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(playerObject.transform.position);
        CheckPos(screenPos);
    }

    private void CheckPos(Vector3 screenPos)
    {
        float effectCoefficient = 0.0f;
        if (screenPos.x < MIN_REQUIRED_DIFFERENCE)
        {
            effectCoefficient = (MIN_REQUIRED_DIFFERENCE - screenPos.x) / MIN_REQUIRED_DIFFERENCE;
            
        } else if (screenPos.x > MAX_REQUIRED_DIFFERENCE)
        {
            effectCoefficient = (screenPos.x - MAX_REQUIRED_DIFFERENCE) / (1.0f -MAX_REQUIRED_DIFFERENCE);
        } else if (screenPos.y < MIN_REQUIRED_DIFFERENCE)
        {
            effectCoefficient = (MIN_REQUIRED_DIFFERENCE - screenPos.y) / MIN_REQUIRED_DIFFERENCE;
        }
        
        SetVolumeEffect(effectCoefficient);
    }

    // Volume effect from 0 to 1
    private void SetVolumeEffect(float volumeEffectCoefficient)
    {
        VolumeProfile volumeProfile = volume.sharedProfile;
        volumeProfile.TryGet<Vignette>(out var vignette);
        volumeProfile.TryGet<DepthOfField>(out var depthOfField);

        float resultSmoothness = MIN_VIGNETTE_SMOOTHNESS + 
            (volumeEffectCoefficient * (MAX_VIGNETTE_SMOOTHNESS - MIN_VIGNETTE_SMOOTHNESS));
        ClampedFloatParameter parameter = new ClampedFloatParameter(resultSmoothness, 0, 1);
        vignette.smoothness.SetValue(parameter);

        float resultFocus = MAX_FOCUS_DISTANCE -
            (volumeEffectCoefficient * (MAX_FOCUS_DISTANCE - MIN_FOCUS_DISTANCE));
        parameter = new ClampedFloatParameter(resultFocus, 0, 1);
        depthOfField.focusDistance.SetValue(parameter);        
    }
}
