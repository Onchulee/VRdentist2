using UnityEngine;

public class LightTest : MonoBehaviour
{
    public Renderer model;
    public Light lighting;
    public Color lightColor = Color.white;
    private Color defaultEmissionColor;

    // Start is called before the first frame update
    void Start()
    {
        if(lighting) lighting.color = lightColor;
        if (model) defaultEmissionColor = model.material.GetColor("_EmissionColor");
        TurnLight(false);
    }

    public void TurnLight(bool on) {
        if (on == true)
        {

            if (model) {
                model.material.EnableKeyword("_EMISSION");
                model.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                model.material.SetColor("_EmissionColor", lightColor);
            }
            if (lighting) lighting.intensity = 1;
        }
        else
        {
            if (model)
            {
                model.material.EnableKeyword("_EMISSION");
                model.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                model.material.SetColor("_EmissionColor", defaultEmissionColor);
            }
            if (lighting) lighting.intensity = 0;
        }
    }
}
