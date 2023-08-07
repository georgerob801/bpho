using UnityEngine;

public class OrbitPath : MonoBehaviour {
    #region
    public CelestialBody planet;
    public CelestialBody sun;

    public OrbitManager om;

    public LineRenderer lr;

    public int segments = 64;

    private float startTime = 0.0f;

    private float scale = 1.0f;
    #endregion
    public void initialize() {
        lr = GetComponent<LineRenderer>();

        float scale = planet.visualRadius * 2.0f * this.scale;
        planet.transform.localScale = new Vector3(scale, scale, scale);
        planet.transform.position = new Vector3(sun.transform.position.x, sun.transform.position.y, sun.transform.position.z);
        planet.transform.position += new Vector3(calculateA() - calculateC(), 0.0f, 0.0f);

        scale = sun.visualRadius * 2.0f * this.scale;
        sun.transform.localScale = new Vector3(scale, scale, scale);

        startTime = Time.time;

        calculateEllipse();
    }

    public float calculateA() {
        // calculates the semi-major axis of the ellipse
        // a^3 = (p^2)(G(m + M)) / 4pi^2
        return Mathf.Pow((Mathf.Pow(planet.period * (float)3.154e7, 2) * (CelestialBody.GMe * (planet.mass + sun.mass))) / (4.0f * Mathf.Pow(Mathf.PI, 2)), (float)1 / (float)3) / CelestialBody.AU;
    }

    public float calculateB() {
        // calculates the semi-minor axis of the ellipse
        // b = a * (1-(ε^2))
        return calculateA() * (1 - Mathf.Pow(planet.eccentricity, 2));
    }

    public float calculateC() {
        // calculates the left focus of the ellipse
        // c = a * ε
        return calculateA() * planet.eccentricity;
    }

    public void calculateEllipse() {
        // calculates the points that make up the ellipse based on a set number of segments
        Vector3[] points = new Vector3[segments];

        float focus = calculateC();
        planet.translation = calculatePlanetTranslation(0.0f).x;

        for (int i = 0; i < segments; i++) {
            // space the segments out at equal angles
            float angle = (this.planet.period * i) / segments;
            // calculate the position the planet would be in at this angle
            // and translate it over by the left focus of the ellipse
            points[i] = calculatePlanetTranslation(angle) - new Vector3(focus, 0.0f, 0.0f);
            // (for 3d simulations)
            // if inclination is being used, transform each point to the correct angle
            if (this.om.useInclination) {
                points[i] = new Vector3(points[i].x * Mathf.Cos(planet.inclination * Mathf.Deg2Rad), points[i].x * Mathf.Sin(planet.inclination * Mathf.Deg2Rad), points[i].z);
            }
        }
        // set the points so they can be drawn
        lr.positionCount = segments;
        lr.SetPositions(points);
    }

    public void Update() {
        if (!this.om.blockMovement) {
            // if this is an animation:
            // calculate the planet's current position based on the time
            planet.transform.localPosition = calculatePlanetTranslation((Time.time - startTime) * this.om.speedMultiplier) - new Vector3(calculateC(), 0.0f, 0.0f);
            // if inclination is being used, transform the planet to the correct angle
            if (this.om.useInclination) {
                planet.transform.localPosition = new Vector3(planet.transform.localPosition.x * Mathf.Cos(planet.inclination * Mathf.Deg2Rad), planet.transform.localPosition.x * Mathf.Sin(planet.inclination * Mathf.Deg2Rad), planet.transform.localPosition.z);
            }
            // rotate the planet to the point it would be at at the current time of day
            planet.transform.eulerAngles = new Vector3(0, (((Time.time - startTime) * this.om.speedMultiplier) / ((float)planet.rotationalPeriod / 365.0f) % 1) * 360 * (planet.clockwiseRotation ? 1 : -1), 0);
            // if axial tilt is being used, rotate the planet to account for it
            if (this.om.useAxialTilt) {
                planet.transform.RotateAround(planet.transform.position, Vector3.left, planet.axialTilt);
            }
        }

        // set the visual size of the planet
        float scale = planet.visualRadius * 2.0f * this.scale;
        planet.transform.localScale = new Vector3(scale, scale, scale);
    }

    public Vector3 calculatePlanetTranslation(float time) {
        // calculate the current position of the planet based on time
        Vector3 pos = new Vector3();
        // this works as the planets have constant angluar momentum around the sun
        float theta = (2 * Mathf.PI * time) / planet.period;
        // caluclate the current radius the planet would be at, then use cos and sin 
        // to get the required x and y components
        pos.x = calculateRFromSun(theta) * Mathf.Cos(theta);
        pos.z = calculateRFromSun(theta) * Mathf.Sin(theta);
        // transform the position to position the sun as the left focus and account
        // for the position of the sun in the simulation (likely always 0)
        pos.x += sun.transform.position.x - calculateC();
        return pos;
    }

    private float calculateRFromSun(float angle) {
        // calculate the distance from the sun based on an angle
        // r = (a * (1 - (ε^2))) / (1 - (ε * cos(θ)))
        return (calculateA() * (1 - Mathf.Pow(planet.eccentricity, 2))) / (1 - (planet.eccentricity * Mathf.Cos(angle)));
    }
    #region
    //public float calculateR(float angle) {
    //    float semiMajor = calculateA();
    //    float semiMinor = calculateB();

    //    return ((semiMajor * semiMinor) / Mathf.Sqrt(Mathf.Pow(semiMinor * Mathf.Cos(angle), 2) + Mathf.Pow(semiMajor * Mathf.Sin(angle), 2))) * scale;
    //}

    //public float calculateR(float angle, float semiMajor, float semiMinor) {
    //    return ((semiMajor * semiMinor) / Mathf.Sqrt(Mathf.Pow(semiMinor * Mathf.Cos(angle), 2) + Mathf.Pow(semiMajor * Mathf.Sin(angle), 2))) * scale;
    //}
    #endregion
}