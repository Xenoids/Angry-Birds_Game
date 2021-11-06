using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShooter : MonoBehaviour
{
    public Collider2D Collider;
    public LineRenderer Trajectory;
    private Vector2 _startPos;

    [SerializeField]
    private float _rad = 0.75f;

    [SerializeField]
    private float _throwSpd = 30f;
    
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
    }

    private Bird _bird;
    void OnMouseUp()
    {
        Collider.enabled = false;
        Vector2 velocity = _startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(_startPos, transform.position);

        _bird.Shoot(velocity,distance,_throwSpd);
        // kembalikan ketapel ke pos awal
        gameObject.transform.position = _startPos;
        Trajectory.enabled = false;
    }

    public void InitBird(Bird bird)
    {
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position,gameObject);
        Collider.enabled = true;
    }

    void OnMouseDrag()
    {
        // Mengubah posisi mouse ke world position
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // hitung agar 'karet' ketapel berada dalam radius yang ditentukan
        Vector2 dir = p - _startPos;
        if(dir.sqrMagnitude > _rad) dir = dir.normalized * _rad;
        transform.position = _startPos + dir;

        float distance = Vector2.Distance(_startPos, transform.position);

        if(!Trajectory.enabled) Trajectory.enabled = true;

        DisplayTrajectory(distance);

            
    }

    void DisplayTrajectory(float distance)
    {
        if(_bird == null) return;

        Vector2 velocity = _startPos - (Vector2)transform.position;
        int segCount = 5;
        Vector2[] segments = new Vector2[segCount];

        // pos awal Trajectory -> posisi mouse skrg
        segments[0] = transform.position;

        // Velocity awal
        Vector2 segVelocity = velocity * _throwSpd * distance;

        for(int i=1;i<segCount ; i++)
        {
            float elapsedTime = i*Time.fixedDeltaTime* 5;
            segments[i] = segments[0] + segVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime,2);
        }

        Trajectory.positionCount = segCount;
        for(int i=0;i<segCount ; i++)
        {
            Trajectory.SetPosition(i,segments[i]);
        }

    }
}
