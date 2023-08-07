using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Task2ClickHandler : MonoBehaviour {
    public GameObject large;
    public GameObject small;
    public OrbitManager orbitManager;
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

    public void ToTask3() {
        SceneManager.LoadScene("Task3");
    }

    public void Awake() {
        this.large.SetActive(true);
        this.small.SetActive(false);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 21.6f, Camera.main.transform.position.z);
    }

    public void Update() {
        if (this.transitionToBig) {
            float t = (Time.time - this.cameraTransitionStartTime) / 1.0f;
            Camera.main.transform.position = Vector3.Lerp(startPosition, new Vector3(this.startPosition.x, 300, this.startPosition.z), (Time.time - this.cameraTransitionStartTime) / 1.0f);
            Camera.main.orthographicSize = Mathf.Lerp(this.startOrthoSize, 38.1f, (Time.time - this.cameraTransitionStartTime) / 1.0f);
            orbitManager.sun.visualRadius = Mathf.Lerp(0.15f, 2, t);
            if (Time.time - this.cameraTransitionStartTime >= 1) this.transitionToBig = false;
        } else if (this.transitionToSmall) {
            float t = (Time.time - this.cameraTransitionStartTime) / 1.0f;
            Camera.main.transform.position = Vector3.Lerp(startPosition, new Vector3(this.startPosition.x, 21.6f, this.startPosition.z), (Time.time - this.cameraTransitionStartTime) / 1.0f);
            Camera.main.orthographicSize = Mathf.Lerp(this.startOrthoSize, 1.68f, (Time.time - this.cameraTransitionStartTime) / 1.0f);
            orbitManager.sun.visualRadius = Mathf.Lerp(2, 0.15f, t);
            if (Time.time - this.cameraTransitionStartTime >= 1) this.transitionToSmall = false;
        }
    }
}
