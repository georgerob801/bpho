using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpirographManager : MonoBehaviour {
    public TMP_Dropdown planet1Name;
    public TMP_Dropdown planet2Name;
    public TMP_Text errorMessage;
    public TMP_InputField intervalField;
    public TMP_InputField speedField;
    public GameObject setupUI;
    public GameObject spiroUI;
    public Material[] orbitMaterials = new Material[2];

    public TextMeshProUGUI[] infoDisplay = new TextMeshProUGUI[5];

    public List<GameObject> celestialObjects = new List<GameObject>();

    private List<GameObject> allTheLines = new List<GameObject>();

    private float[] requiredOrthoSizes = {
        0.0f,
        0.51f,
        0.8f,
        1.11f,
        1.68f,
        5.67f,
        10.52f,
        20.57f,
        33.39f,
        46.92f
    };

    private float[] requiredCameraPositions = { 
        0.0f,
        2.48f,
        2.48f,
        2.48f,
        2.48f,
        15.62f,
        22.42f,
        48.47f,
        86.59f,
        60.6f
    };

    private OrbitManager om;

    private float speed;
    private float interval;
    private double endTime;
    private double startTime;
    private double counter = 0;
    private bool drawing = false;

    private Transform planet1;
    private Transform planet2;

    public void ToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToSetup() {
        SceneManager.LoadScene("Task6");
    }

    public void LetsGOOOOO() {
        if (planet1Name.value == planet2Name.value) {
            errorMessage.text = "Please select two different planets.";
            return;
        }

        if (!float.TryParse(intervalField.text, out interval)) {
            errorMessage.text = "Please provide a decimal number for the interval.";
            return;
        } 

        if (interval <= 0) {
            errorMessage.text = "I mean come on. Really?";
            return;
        }

        if (!float.TryParse(speedField.text, out speed)) {
            errorMessage.text = "Please provide a decimal number for the speed.";
            return;
        }

        if (speed == 0) {
            errorMessage.text = "0 speed mhm ok yeah that'll definitely work";
            return;
        }

        // disable the ui and create the thing
        setupUI.SetActive(false);
        spiroUI.SetActive(true);

        GameObject first = Instantiate(celestialObjects.ElementAt(planet1Name.value));
        GameObject second = Instantiate(celestialObjects.ElementAt(planet2Name.value));
        GameObject sun = Instantiate(celestialObjects.ElementAt(0));

        GameObject system = new GameObject();
        system.name = "Solar System";
        first.transform.SetParent(system.transform);
        second.transform.SetParent(system.transform);
        sun.transform.SetParent(system.transform);

        planet1 = first.transform;
        planet2 = second.transform;

        sun.GetComponent<MeshRenderer>().enabled = false;

        om = system.AddComponent<OrbitManager>();
        om.orbitPathMaterial = orbitMaterials[0];
        om.speedMultiplier = speed;
        Debug.Log(om.speedMultiplier);
        om.sun = sun.GetComponent<CelestialBody>();
        om.planets.Add(first.GetComponent<CelestialBody>());
        om.planets.Add(second.GetComponent<CelestialBody>());
        om.Awake();

        endTime = Time.time + (celestialObjects.ElementAt(Mathf.Max(planet1Name.value, planet2Name.value)).GetComponent<CelestialBody>().period * 10 / speed);
        drawing = true;
        startTime = Time.time;

        infoDisplay[1].text = $"Planet 1: {celestialObjects.ElementAt(planet1Name.value).name}";
        infoDisplay[2].text = $"Planet 2: {celestialObjects.ElementAt(planet2Name.value).name}";
        infoDisplay[3].text = $"1 s = {speed} yr";
        infoDisplay[4].text = $"Line every {interval} s";

        Camera.main.orthographicSize = requiredOrthoSizes[Mathf.Max(planet1Name.value, planet2Name.value)];
        Camera.main.transform.position = new Vector3(0.0f, requiredCameraPositions[Mathf.Max(planet1Name.value, planet2Name.value)], 0.0f);
    }

    private void Update() {
        if (drawing) {
            if (Time.time >= endTime) EndDrawing();

            infoDisplay[0].text = $"t = {(Time.time - startTime).ToString("F2")} s";

            if (counter >= interval) {
                counter = 0.0d;
                // draw line
                GameObject go = new GameObject();
                go.name = "Spirograph Line";
                go.transform.SetParent(transform);
                allTheLines.Add(go);
                LineRenderer lr = go.AddComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.SetPositions(new Vector3[] { planet1.position, planet2.position });
                lr.material = orbitMaterials[1];
                lr.startWidth = Vector3.Distance(Camera.main.transform.position, go.transform.position) / 1000;
            }

            counter += Time.deltaTime;
        }
    }

    private void EndDrawing() {
        drawing = false;
        planet1.gameObject.SetActive(false);
        planet2.gameObject.SetActive(false);

        om.speedMultiplier = 0.0f;
    }
}
