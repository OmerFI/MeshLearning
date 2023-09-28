using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePivot : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private bool isClockwise = true;
    
    private void Update()
    {
        float direction = isClockwise ? -1 : 1;
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime * direction);
    }

}
