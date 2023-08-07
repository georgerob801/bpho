using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Task3ClickHandler : MonoBehaviour {
    public GameObject large;
    public GameObject small;
    public OrbitManager orbitManager;
    public TextMeshProUGUI speedText;
    private float cameraTransitionStartTime;
    private bool transitionToBig;
    private bool transitionToSmall;
    private float startOrthoSize;
    private Vector3 startPosition;

    public void BackToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToLargeScale() {
        this.large.SetActive(false);
        this.small.SetActive(true);
        this.startOrthoSize = Camera.main.orthographicSize;
        this.startPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        this.cameraTransitionStartTime = Time.time;
        this.transitionToBig = true;
    }

    public void ToSmallScale() {
        this.small.SetActive(false);
        this.large.SetActive(true);
        this.startOrthoSize = Camera.main.orthographicSize;
        this.startPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        this.cameraTransitionStartTime = Time.time;
        this.transitionToSmall = true;
    }

    public void ToTask4() {
        SceneManager.LoadScene("Task4");
    }

    public void Awake() {
        this.large.SetActive(true);
        this.small.SetActive(false);
        orbitManager.speedMultiplier = 1.0f;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 21.6f, Camera.main.transform.position.z);
    }

    public void Update() {
        if (this.transitionToBig) {
            float t = (Time.time - this.cameraTransitionStartTime) / 1.0f;
            Camera.main.transform.position = Vector3.Lerp(startPosition, new Vector3(this.startPosition.x, 300, this.startPosition.z), t);
            Camera.main.orthographicSize = Mathf.Lerp(this.startOrthoSize, 38.1f, t);
            orbitManager.speedMultiplier = Mathf.Lerp(1.0f, 11.86f, t);
            orbitManager.sun.visualRadius = Mathf.Lerp(0.15f, 2, t);
            if (Time.time - this.cameraTransitionStartTime >= 1) this.transitionToBig = false;
        } else if (this.transitionToSmall) {
            float t = (Time.time - this.cameraTransitionStartTime) / 1.0f;
            Camera.main.transform.position = Vector3.Lerp(startPosition, new Vector3(this.startPosition.x, 21.6f, this.startPosition.z), t);
            Camera.main.orthographicSize = Mathf.Lerp(this.startOrthoSize, 1.68f, t);
            orbitManager.speedMultiplier = Mathf.Lerp(11.86f, 1.0f, t);
            orbitManager.sun.visualRadius = Mathf.Lerp(2, 0.15f, t);
            if (Time.time - this.cameraTransitionStartTime >= 1) this.transitionToSmall = false;
        }

        this.speedText.text = $"1 s = {this.orbitManager.speedMultiplier} yr";
    }
}
