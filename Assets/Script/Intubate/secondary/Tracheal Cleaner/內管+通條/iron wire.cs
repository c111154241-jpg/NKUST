using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ironwire : MonoBehaviour
{
    [SerializeField] float Distance;
    [SerializeField] GameObject Model;
    [SerializeField] private Vector3 DistanceVector;
    [SerializeField] private GameObject Car;
    void Update()
    {
        Vector3 localPosition = transform.localPosition;


        if (localPosition.y > Distance)
        {
            Model.transform.position = Car.transform.position + DistanceVector;
            Model.SetActive(true);
            this.transform.gameObject.SetActive(false);
        }
    }
}
