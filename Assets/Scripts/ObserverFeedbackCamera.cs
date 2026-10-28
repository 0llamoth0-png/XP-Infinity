using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class ObserverFeedbackCamera : MonoBehaviour
{
    [Header("Camera Shake Options")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.5f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;

    PlayerHP playerHP;
    Camera mainCamera;
    Volume postProcessingVolume;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerHP = FindAnyObjectByType<PlayerHP>();
        mainCamera = Camera.main;
    }
    void Start()
    {
        postProcessingVolume = FindAnyObjectByType<Volume>();
    }

    void OnEnable()
    {
        
        playerHP.playerDamaged += ShakeCamera;
        
    }
    void ShakeCamera()
    {
       mainCamera.DOShakePosition(shakeDuration, shakeMagnitude, shakeVibrato, shakeRandomness, true, ShakeRandomnessMode.Harmonic);
       StartCoroutine(DamageCameraCoroutine());
    }

    IEnumerator DamageCameraCoroutine()
    {
        if (postProcessingVolume != null)
        {
            Vignette vignette;
            if (postProcessingVolume.profile.TryGet<Vignette>(out vignette))
            {
                float originalIntensity = vignette.intensity.value;
                float targetIntensity = 0.5f; // Adjust this value for the desired intensity
                float elapsedTime = 0f;
                
                while (elapsedTime < shakeDuration)
                {
                    vignette.color.value = Color.red; // Change the color to red
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / shakeDuration;
                    vignette.intensity.value = Mathf.Lerp(originalIntensity, targetIntensity, t);
                    yield return null;
                }

                // Reset the intensity back to the original value
                vignette.intensity.value = originalIntensity;
                vignette.color.value = Color.gray; // Change the color to gray
            }
        }
        StopCoroutine(DamageCameraCoroutine());
    }
}
