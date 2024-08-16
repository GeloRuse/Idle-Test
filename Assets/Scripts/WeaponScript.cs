using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]
    private WeaponStats stats;

    public WeaponStats GetStats => stats;
}
