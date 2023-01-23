using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private bool hasBeentriggered = false;

    void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
            damageable.TakeFatalDamage("The ship failed to stop and was dashed against salvation's surface.");


    }

    void OnTriggerEnter(Collider other)
    {
        if (hasBeentriggered)
            return;

        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            GameManager.instance.ReachTarget();
            hasBeentriggered = true;
        }
    }
}
