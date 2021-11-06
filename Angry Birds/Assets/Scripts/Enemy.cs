using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float hp = 50f;

    public UnityAction<GameObject> OnEnemyDestroyed = delegate { };
    private bool _isHit = false;

    void OnDestory()
    {
        if(_isHit) OnEnemyDestroyed(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.GetComponent<Rigidbody2D>() == null) return;

        if(col.gameObject.tag == "Bird")
        {
            _isHit = true;
            Destroy(gameObject);
        }
        else if(col.gameObject.tag == "Obstacle")
        {
            // Hitung dmg yg diperoleh
            float dmg = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            hp -=dmg;
            if(hp <= 0)
            {
                _isHit = true;
                Destroy(gameObject);
            }
        }
    }
}
