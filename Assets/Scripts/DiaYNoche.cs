using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DiaYNoche : MonoBehaviour
{
    [SerializeField] bool isCountingDay, isNight;
    [SerializeField] float countdownToReset = 10f; 
    [Range (0, 2f)]
    public float dayVelocity;
    Light directionalLight;
    Volume volume;
    void Start()
    {
        directionalLight = GetComponent<Light>();
        volume = FindAnyObjectByType<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isCountingDay)
        {
            return;
        }

        if(isNight)
        {
            ToDay();
        }
        else
        {
            ToNight();
        }
        
    }

    void ToDay()
    {
        directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, 2f, dayVelocity *Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(140f, 0f, 0f), dayVelocity *Time.deltaTime);
        volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
        colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, 0f, dayVelocity);
        if(directionalLight.intensity > 0.9f)
        {
            countdownToReset -= Time.deltaTime;
        }
        if(countdownToReset <= 0f)
        {
            isNight = false;
            countdownToReset = 10f;
        }
    }
    void ToNight()
    {
        directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, 0f, dayVelocity *Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(200f, 0f, 0f), dayVelocity *Time.deltaTime);
        volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
        colorAdjustments.postExposure.value = Mathf.Lerp(colorAdjustments.postExposure.value, -2f, dayVelocity);
        if(directionalLight.intensity <= 0.1f)
        {
            countdownToReset -= Time.deltaTime;
        }
        if(countdownToReset <= 0f)
        {
            isNight = true;
            countdownToReset = 10f;
        }
    }
}
