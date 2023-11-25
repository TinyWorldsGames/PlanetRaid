using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 50f;
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<IEnemy>() is IEnemy enemy && enemy != null)
        {
            int randomDamage = Random.Range(16, 27);
            enemy.TakeDamage(randomDamage);

            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        }

        if (other.gameObject.GetComponent<ICollectableNew>() is ICollectableNew collectable && collectable != null)
        {
            collectable.Collect();
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        }

        // else

        if (other.gameObject.GetComponent<IEnemy>() == null && other.gameObject.GetComponent<ICollectableNew>() == null)
        {
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }



        Destroy(gameObject);
    }

}