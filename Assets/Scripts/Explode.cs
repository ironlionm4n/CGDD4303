using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{

    [Header("Explosion Point")]
    [SerializeField] private GameObject explosionPoint;

    [SerializeField] private ParticleSystem explosionParticles;


    public Rigidbody[] rigidbodies;
    public MeshCollider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExplodeBuild()
    {
        rigidbodies = FindObjectsOfType<Rigidbody>();
        colliders = FindObjectsOfType<MeshCollider>();

        foreach(MeshCollider collider in colliders)
        {
            collider.enabled = false;

            if(collider.gameObject.name == "Ground")
            {
                collider.enabled = true;
            }
        }
        explosionParticles.Play();
        for(int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].constraints = RigidbodyConstraints.None;
            rigidbodies[i].AddExplosionForce(50 , explosionPoint.transform.position, 200, 1.0f, ForceMode.Impulse);
            rigidbodies[i].useGravity = true;
        }
    }
}
