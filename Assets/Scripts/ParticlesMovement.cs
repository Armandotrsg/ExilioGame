using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesMovement : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 10f;

    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private int numParticlesAlive;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void LateUpdate()
    {
        numParticlesAlive = ps.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            particles[i].position += (target.position - transform.position) * (followSpeed * Time.deltaTime);
        }

        ps.SetParticles(particles, numParticlesAlive);
    }
}