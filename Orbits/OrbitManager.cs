using System.Collections.Generic;
using UnityEngine;

public class OrbitManager : MonoBehaviour {
    public CelestialBody sun;
    public List<CelestialBody> planets = new List<CelestialBody>();

    public Material orbitPathMaterial;

    public bool blockMovement;
    public bool perspectiveCamera;
    public bool useInclination;
    public bool useAxialTilt = true;

    public float speedMultiplier;

    private List<LineRenderer> lines = new List<LineRenderer>();

    public List<LineRenderer> getLines() {
        return this.lines;
    }

    public void Awake() {
        // set up the camera
        Camera cam = Camera.main;
        if (!perspectiveCamera) {
            cam.orthographic = true;
            cam.orthographicSize = 1.68f;
        }

        // create a new orbit path for each of the planets given
        // this will manage drawing the line of the orbit as well
        // as any calculations necessary
        this.planets.ForEach(x => {
            GameObject go = new GameObject();
            go.transform.SetParent(this.transform);
            go.name = x.gameObject.name + " Orbit Path";

            LineRenderer lr = go.AddComponent<LineRenderer>();

            this.lines.Add(lr);

            lr.material = this.orbitPathMaterial;

            lr.loop = true;
            // lr.startWidth = 0.02f;

            OrbitPath op = go.AddComponent<OrbitPath>();
            op.om = this;

            op.planet = x;
            op.sun = this.sun;

            op.initialize();
        });
    }

    private void Update() {
        Camera cam = Camera.main;

        float scale = sun.visualRadius * 2.0f;
        sun.transform.localScale = new Vector3(scale, scale, scale);

        // scale the width of the lines depending on their distance from
        // the camera (making them appear to have the same width no matter
        // how far away the camera is)
        this.lines.ForEach(lr => {
            lr.startWidth = Vector3.Distance(cam.transform.position, lr.transform.position) / 1000;
        });
    }
}
