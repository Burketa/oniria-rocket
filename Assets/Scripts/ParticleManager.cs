using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleManager
{
    public static void EmmitParticle(ParticleSystem particle, bool state)
    {
        if (state)
            particle.Play();
        else
        {
            particle.Clear();
            particle.Stop();
        }
    }

    public static void MoveParticle(ParticleSystem particle, Vector3 position)
    {
        particle.transform.position = position;
    }
}
