using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Task7Manager : MonoBehaviour {
    public List<GameObject> celestialObjects = new List<GameObject>();
    public Material orbitLineMaterial;
    public List<Material> orbitMaterials = new List<Material>();

    public TMP_Dropdown planetIndex;
    public TMP_InputField scaleField;
    public TMP_Text errorMessage;
    public GameObject setupUI;
    public GameObject otherUI;

    public double interval = 0.1d;

    private OrbitManager om = new OrbitManager();
    private List<GameObject> planets = new List<GameObject>();
    private List<LineRenderer> lrs = new List<LineRenderer>();
    private bool drawing = false;

    private double counter = 0.0d;

    public void ToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadScene() {
        SceneManager.LoadScene("Task7");
    }

    private void Awake() {
        Camera camera = Camera.main;
        camera.transform.position = new Vector3(0.0f, 10.0f, 0.0f);
        camera.orthographic = true;
        setupUI.SetActive(true);
        otherUI.SetActive(false);
    }

    public void LetsGOOOOOOOOOOOOOOOOOOO() {
        float scale;
        if (!float.TryParse(scaleField.text, out scale)) {
            errorMessage.text = "Please provide a decimal number for the scale.";
            return;
        }

        GameObject sun = Instantiate(celestialObjects.ElementAt(0));
        sun.transform.SetParent(transform);
        planets.Add(sun);

        GameObject system = new GameObject();
        system.name = "SolarSystem";
        system.transform.SetParent(transform);
        om = system.AddComponent<OrbitManager>();
        om.sun = sun.GetComponent<CelestialBody>();
        om.orbitPathMaterial = orbitLineMaterial;

        for (int i = 0; i < celestialObjects.Count - 1; i++) {
            GameObject planet = Instantiate(celestialObjects.ElementAt(i + 1));
            planets.Add(planet);
            planet.transform.SetParent(om.transform);

            om.planets.Add(planet.GetComponent<CelestialBody>());
        }

        for (int i = 0; i < planets.Count; i++) {
            if (i != planetIndex.value) {
                planets[i].GetComponent<MeshRenderer>().enabled = false;
                if (planets[i].transform.childCount > 0) planets[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        om.useAxialTilt = false;
        om.Awake();

        //om.getLines().ForEach(x => {
        //    x.gameObject.SetActive(false);
        //});

        GameObject selectedPlanet = planets.ElementAt(planetIndex.value);

        Camera.main.transform.localPosition = selectedPlanet.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
        Camera.main.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);

        Camera.main.orthographicSize = scale;

        setupUI.SetActive(false);
        otherUI.SetActive(true);

        for (int i = 0; i < planets.Count;i++) {
            GameObject go = new GameObject();
            go.name = $"Line{planets.ElementAt(i).name}";
            go.transform.SetParent(transform);
            LineRenderer lr = go.AddComponent<LineRenderer>();
            lr.material = orbitMaterials.ElementAt(i);
            lr.positionCount = 0;
            lr.transform.SetParent(Camera.main.transform);
            lr.startWidth = Vector3.Distance(Camera.main.transform.position, go.transform.position) / 1000;
            lr.useWorldSpace = false;
            lrs.Add(lr);
        }

        drawing = true;
        om.speedMultiplier = 1.0f;
    }

    private void Update() {
        if (drawing) {
            Camera.main.transform.localPosition = planets.ElementAt(planetIndex.value).transform.position + new Vector3(0.0f, 10.0f, 0.0f);
            
            if (counter >= interval) {
                counter = 0.0d;

                for (int i = 0; i < planets.Count; i++) {
                    LineRenderer lr = lrs.ElementAt(i);
                    lr.positionCount++;
                    lr.SetPosition(lr.positionCount - 1, planets.ElementAt(i).transform.position - Camera.main.transform.position + new Vector3(planets.ElementAt(planetIndex.value).GetComponent<CelestialBody>().translation, 10.0f, 0.0f));
                }
            } else {
                counter += Time.deltaTime;
            }
        }
    }
}
