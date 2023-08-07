using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour {
    public float scale = 0.1f;
    public Material[] materials = new Material[3];
    public GameObject labelPreset;
    public float[] lineWidths = new float[3];
    public Material exampleMat;
    public Material exMat2;
    public bool hidePlanets = false;
    public bool showTask5 = false;
    [Header("X axis")]
    public float xAxisMax;
    public float xAxisInterval;
    public float xAxisSubdivisions;
    [Header("Y axis")]
    public float yAxisMax;
    public float yAxisInterval;
    public float yAxisSubdivisions;

    [Header("Planets")]
    public CelestialBody sun;
    public GameObject sunObject;
    public List<GameObject> planets = new List<GameObject>();

    public void ToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    private void Awake() {
        // code to draw a graph
        #region Draw graph
        GameObject xAxis = new GameObject("X axis");
        xAxis.transform.parent = transform;
        LineRenderer xAxisLr = xAxis.AddComponent<LineRenderer>();
        xAxisLr.material = materials[0];
        xAxisLr.startWidth = lineWidths[0];
        xAxisLr.positionCount = 2;
        xAxisLr.SetPositions(new Vector3[] { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(xAxisMax * scale, 0.0f, 0.0f) });

        for (float x = 0; x <= xAxisMax; x += xAxisInterval) {
            GameObject go = new GameObject();
            go.transform.SetParent(xAxis.transform);
            go.name = $"X axis {x}";

            LineRenderer lr = go.AddComponent<LineRenderer>();

            lr.material = materials[1];
            lr.startWidth = lineWidths[1];
            lr.positionCount = 2;
            lr.SetPositions(new Vector3[] { new Vector3(x * scale, 0.0f, 0.0f), new Vector3(x * scale, yAxisMax * scale * (showTask5 ? 20.0f : 1.0f), 0.0f) });

            GameObject label = Instantiate(labelPreset, new Vector3(x * scale, -0.6f, 0.0f), new Quaternion());
            label.GetComponent<TMP_Text>().text = $"{x}";
            label.transform.SetParent(go.transform);
            label.name = $"X axis {x}";

            for (int sX = 0; sX <= xAxisSubdivisions && (x * scale) + ((xAxisInterval / xAxisSubdivisions) * sX * scale) < xAxisMax * scale; sX++) {
                float subX = (xAxisInterval / xAxisSubdivisions) * sX;
                GameObject sGo = new GameObject();
                sGo.name = $"Sub-line {sX}";
                sGo.transform.SetParent(go.transform);
                LineRenderer sLr = sGo.AddComponent<LineRenderer>();

                sLr.material = materials[2];
                sLr.startWidth = lineWidths[2];
                sLr.positionCount = 2;
                sLr.SetPositions(new Vector3[] { new Vector3((x * scale) + (subX * scale), 0.0f, 0.0f), new Vector3((x * scale) + (subX * scale), yAxisMax * scale * (showTask5 ? 20.0f : 1.0f), 0.0f) });
            }
        }

        if (showTask5) scale *= 20;

        GameObject yAxis = new GameObject("Y axis");
        yAxis.transform.parent = transform;
        LineRenderer yAxisLr = yAxis.AddComponent<LineRenderer>();
        yAxisLr.material = materials[0];
        yAxisLr.startWidth = lineWidths[0];
        yAxisLr.positionCount = 2;
        yAxisLr.SetPositions(new Vector3[] { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, yAxisMax * scale, 0.0f) });

        for (float y = 0; y <= yAxisMax; y += yAxisInterval) {
            GameObject go = new GameObject();
            go.transform.SetParent(yAxis.transform);
            go.name = $"Y axis {y}";

            LineRenderer lr = go.AddComponent<LineRenderer>();

            lr.material = materials[1];
            lr.startWidth = lineWidths[1];
            lr.positionCount = 2;
            lr.SetPositions(new Vector3[] { new Vector3(0.0f, y * scale, 0.0f), new Vector3(xAxisMax * scale * (showTask5 ? 0.05f : 1.0f), y * scale, 0.0f) });

            GameObject label = Instantiate(labelPreset, new Vector3(-0.8f, y * scale, 0.0f), new Quaternion());
            label.GetComponent<TMP_Text>().text = $"{y}";
            label.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.MidlineRight;
            label.transform.SetParent(go.transform);
            label.name = $"Y axis {y}";

            for (int sY = 0; sY <= yAxisSubdivisions && (y * scale) + ((yAxisInterval / yAxisSubdivisions) * sY * scale) < yAxisMax * scale; sY++) {
                float subY = (yAxisInterval / yAxisSubdivisions) * sY;
                GameObject sGo = new GameObject();
                sGo.name = $"Sub-line {sY}";
                sGo.transform.SetParent(go.transform);
                LineRenderer sLr = sGo.AddComponent<LineRenderer>();

                sLr.material = materials[2];
                sLr.startWidth = lineWidths[2];
                sLr.positionCount = 2;
                sLr.SetPositions(new Vector3[] { new Vector3(0.0f, (y * scale) + (subY * scale),0.0f), new Vector3(xAxisMax * scale * (showTask5 ? 0.05f : 1.0f), (y * scale) + (subY * scale), 0.0f) });
            }
        }

        if (showTask5) scale /= 20;

        #endregion

        // for task 1:
        if (!hidePlanets) {
            // put the planets on it
            planets.ForEach(planet => {
                GameObject instance = Instantiate(planet, transform);
                // position each planet on the graph with an x coordinate of the semi major axis to the power of 3/2 (in AU), and a y coordinate of the period of the planet (in yr)
                instance.transform.position = new Vector3(Mathf.Pow(calculateA(instance.GetComponent<CelestialBody>(), sun), 1.5f) * scale, instance.GetComponent<CelestialBody>().period * scale, 0.0f);
                // make them all the same size for consistency
                instance.transform.localScale = Vector3.one * 2.0f;
            });
        }

        // show the comparison line with a gradient of 1
        if (!showTask5) {
            // line for y = x
            GameObject yIsX = new GameObject();
            LineRenderer lrT = yIsX.AddComponent<LineRenderer>();
            lrT.material = exampleMat;
            lrT.startWidth = lineWidths[0];
            lrT.positionCount = 2;
            lrT.SetPositions(new Vector3[] { new Vector3(0.0f, 0.0f, -1.0f), new Vector3(30.0f, 30.0f, -1.0f) });
        } else {
            // line for y = x
            GameObject yIsX = new GameObject();
            LineRenderer lrT = yIsX.AddComponent<LineRenderer>();
            lrT.material = exampleMat;
            lrT.startWidth = lineWidths[0];
            lrT.positionCount = 2;
            lrT.SetPositions(new Vector3[] { new Vector3(0.0f, 0.0f, -1.0f), new Vector3(planets[8].GetComponent<CelestialBody>().period * scale, 2 * Mathf.PI * 2, -1.0f) });
        }

        // for task 5:
        if (showTask5) {
            // generate a solar system comprised of just the sun and pluto
            GameObject fakeOm = new GameObject();
            OrbitManager om = fakeOm.AddComponent<OrbitManager>();
            om.speedMultiplier = 1.0f;
            GameObject pluto = Instantiate(planets[8], new Vector3(0.0f, 0.0f, -10.0f), new Quaternion());
            GameObject fakeOp = new GameObject();
            OrbitPath op = fakeOp.AddComponent<OrbitPath>();
            op.om = om;
            op.planet = pluto.GetComponent<CelestialBody>();
            GameObject tempSun = Instantiate(sunObject);
            op.sun = tempSun.GetComponent<CelestialBody>();

            LineRenderer lr2 = fakeOm.AddComponent<LineRenderer>();
            lr2.positionCount = 0;
            lr2.material = exMat2;
            lr2.startWidth = lineWidths[0];

            // variable to count the Y coordinate
            float cumulativeY = 0.0f;

            for (float i = 0; i < pluto.GetComponent<CelestialBody>().period; i += 0.1f) {
                // cumulativeY += Mathf.Max(Vector3.Angle(op.calculatePlanetTranslation(i), Vector3.right) * Mathf.Deg2Rad, cumulativeY) - Mathf.Min(Vector3.Angle(op.calculatePlanetTranslation(i), Vector3.right) * Mathf.Deg2Rad, cumulativeY);

                cumulativeY = i < pluto.GetComponent<CelestialBody>().period / 2.0f ? Vector3.Angle(op.calculatePlanetTranslation(i), Vector3.right) * Mathf.Deg2Rad : Mathf.PI + Mathf.PI - (Vector3.Angle(op.calculatePlanetTranslation(i), Vector3.right) * Mathf.Deg2Rad);

                lr2.positionCount++;
                lr2.SetPosition(lr2.positionCount - 1, new Vector3(i * scale, cumulativeY * 2, 0.0f));
            }

            // remove the solar system so it can't be seen
            pluto.SetActive(false);
            fakeOp.SetActive(false);
            om.enabled = false;
            tempSun.SetActive(false);
        }

        // put the camera in the right place
        Camera.main.orthographicSize = 18;
        if (showTask5) Camera.main.orthographicSize = 14.4f;
    }

    // duplicate of OrbitPath::calculateA for convenience
    private float calculateA(CelestialBody planet, CelestialBody sun) {
        // a^3 = (p^2)(G(m + M)) / 4pi^2
        return Mathf.Pow((Mathf.Pow(planet.period * (float)3.154e7, 2) * (CelestialBody.GMe * (planet.mass + sun.mass))) / (4.0f * Mathf.Pow(Mathf.PI, 2)), (float)1 / (float)3) / CelestialBody.AU;
    }
}
