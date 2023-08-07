using System.ComponentModel;
using UnityEngine;

public class CelestialBody : MonoBehaviour {
    public static readonly ulong G = (ulong)6.67e-11;
    public static readonly ulong GMe = (ulong)3.986e14;
    public static readonly int GMeDivAU = 2664;
    public static readonly ulong AU = (ulong)1.496e11;
    public static readonly ulong ReDivAU = (ulong)4.264e-5;

    [Description("Mass relative to mass of earth")]
    public float mass = 1.0f;
    public float period;
    public float rotationalPeriod = 1.0f;
    public float radius = 1.0f;
    public float visualRadius = 1.0f;
    [Range(0f, 1f)]
    public float eccentricity = 0.0f;
    public bool clockwiseRotation;
    public float inclination;
    public float axialTilt = 0.0f;
    public float translation = 0.0f;
}
