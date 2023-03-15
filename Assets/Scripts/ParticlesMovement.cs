using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesMovement : MonoBehaviour
{
    public Transform target; // Referencia al jugador
    public float followSpeed = 10f; // Velocidad de seguimiento de las particulas

    private ParticleSystem ps; // Referencia al sistema de particulas
    private ParticleSystem.Particle[] particles; // Arreglo de particulas en el sistema
    private int numParticlesAlive; // Numero de particulas en el sistema

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