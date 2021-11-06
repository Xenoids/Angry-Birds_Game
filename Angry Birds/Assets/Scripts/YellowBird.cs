using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird
{
    [SerializeField]
    public float _boostForce = 100;
    public bool _adaboost = false;

    public void Boost()
    {
        if(State == StatusBird.Thrown && !_adaboost)
        {
            rb.AddForce(rb.velocity * _boostForce);
            _adaboost = true;
        }
    }

    public override void OnTap()
    {
        Boost();
    }
}
