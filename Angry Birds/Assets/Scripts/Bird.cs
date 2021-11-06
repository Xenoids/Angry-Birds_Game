using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum StatusBird {Idle, Thrown, HitSesuatu}
    public GameObject Parent;

    public Rigidbody2D rb;

    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    public StatusBird State {get { return _status;}}

    private StatusBird _status;
    private float _minVelocity = 0.05f;
    private bool _flagDestory = false;

    // Start is called before the first frame update
    void Start()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _status = StatusBird.Idle;
    }

    void OnDestroy()
    {
        if(_status == StatusBird.Thrown || _status == StatusBird.HitSesuatu)
        OnBirdDestroyed();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        _status = StatusBird.HitSesuatu;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_status == StatusBird.Idle && rb.velocity.sqrMagnitude >= _minVelocity)
        _status = StatusBird.Thrown;

        if((_status == StatusBird.Thrown || _status == StatusBird.HitSesuatu)&& rb.velocity.sqrMagnitude < _minVelocity && !_flagDestory)
        {
            // Hancurkan gameObject stlh 2 detik
            _flagDestory = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    private IEnumerator DestroyAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float spd)
    {
        Collider.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = velocity * spd * distance;
        OnBirdShot(this);
    }
    
    public virtual void OnTap()
    {
        // nope
    }

    
}
