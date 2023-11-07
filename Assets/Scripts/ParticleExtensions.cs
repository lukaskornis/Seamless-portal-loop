using UnityEngine;

public static class ParticleExtensions
{
    public static void Emit(this ParticleSystem ps, bool emit, bool recursive = false)
    {
        var em = ps.emission;
        em.enabled = emit;
        if (!recursive) return;

        foreach (Transform child in ps.transform)
        {
            var childPs = child.GetComponent<ParticleSystem>();
            if (childPs != null)
            {
                childPs.Emit(emit);
            }
        }
    }
}