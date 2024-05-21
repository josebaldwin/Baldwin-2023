using UnityEngine;

public class RotateMenuSky : MonoBehaviour
{
    public float rotationSpeedMenu = 10.0f;  // Speed of rotation, can be adjusted in the inspector

    void Update()
    {
        // Use the current skybox material from RenderSettings
        Material currentSkyboxMaterial = RenderSettings.skybox;
        if (currentSkyboxMaterial != null)
        {
            // Rotate the skybox by modifying a shader property
            float rotation = Mathf.Repeat(currentSkyboxMaterial.GetFloat("_Rotation") + Time.deltaTime * rotationSpeedMenu, 360);
            currentSkyboxMaterial.SetFloat("_Rotation", rotation);
        }
        else
        {
            Debug.LogError("Skybox material is not assigned or has been unset.");
        }
    }
}
