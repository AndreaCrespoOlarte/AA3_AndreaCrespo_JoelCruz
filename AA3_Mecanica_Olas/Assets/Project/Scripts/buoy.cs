using UnityEngine;

public class Buoy : MonoBehaviour
{
    public Rigidbody rb;
    public float waterDensity = 1f;       // rho (p)
    public float gravity = 9.81f;         // g
    public float objectVolume = 2f;        // V total
    public float floatHeightOffset = 0.5f; // Ajuste del centro de masa de la boya

    // Referencia a tus scripts de simulación
    public sinusoidal waveSinusoidal;
    public gerstner waveGerstner;

    public bool useGerstner = false; // Conmutador/Toggle solicitado en el PDF

    void FixedUpdate()
    {
        float waterHeight = 0f;

        // 1. Obtener la altura del agua según el modo activo
        if (useGerstner && waveGerstner != null)
        {
            // Método de búsqueda por malla/celda
            waterHeight = waveGerstner.GetGerstnerWaterHeight(transform.position);
        }
        else if (waveSinusoidal != null)
        {
            // Método matemático directo
            waterHeight = waveSinusoidal.GetSinusoidalWaterHeight(transform.position);
        }

        // 2. Calcular sumersión
        float depefInWater = waterHeight - (transform.position.y - floatHeightOffset);

        if (depefInWater > 0) // Si está tocando o bajo el agua
        {
            // Porcentaje sumergido (clampeado entre 0 y 1)
            float displacementMultiplier = Mathf.Clamp01(depefInWater / floatHeightOffset);

            // Fórmula: F = rho * g * V_desplazado
            float buoyancyForceY = waterDensity * gravity * (displacementMultiplier * objectVolume);

            // Aplicamos la fuerza hacia arriba
            rb.AddForce(new Vector3(0f, buoyancyForceY, 0f), ForceMode.Acceleration);

            // Amortiguación (Damping) para que la boya no rebote infinitamente como loca
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.9f, rb.linearVelocity.z);
        }
    }
}