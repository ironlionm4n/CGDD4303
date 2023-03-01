using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{

    [Header("Explosion Point")]
    [SerializeField] private GameObject explosionPoint;


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

        List<float> directionFromExplosionCenter = new List<float>();

        foreach(Rigidbody rb in rigidbodies)
        {
            float direction = Vector3.Distance(rb.transform.position, explosionPoint.transform.position);
            directionFromExplosionCenter.Add(direction);
        }

        foreach(MeshCollider collider in colliders)
        {
            collider.enabled = false;

            if(collider.gameObject.name == "Ground")
            {
                collider.enabled = true;
            }
        }

        for(int i = 0; i < directionFromExplosionCenter.Count; i++)
        {
            rigidbodies[i].constraints = RigidbodyConstraints.None;
            rigidbodies[i].AddExplosionForce(50 , explosionPoint.transform.position, 200, 1.0f, ForceMode.Impulse);
            //rigidbodies[i].AddForce(new Vector3(10, 10, 10) * -directionFromExplosionCenter[i], ForceMode.Impulse);
            rigidbodies[i].useGravity = true;
        }
    }
}
