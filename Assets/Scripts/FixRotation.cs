﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour
{
    Quaternion rotation;

    void Start() {
        rotation = transform.rotation;
    }

    void LateUpdate() {
        transform.rotation = rotation;
    }
}
