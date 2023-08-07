using UnityEngine;
using UnityEngine.SceneManagement;

public class Task4ClickHandler : MonoBehaviour {
    public OrbitManager om;

    public void BackToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToTask5() {
        SceneManager.LoadScene("Task5");
    }

    void Update() {
        om.sun.visualRadius = Mathf.Lerp(0.15f, 2, (Vector3.Distance(om.sun.transform.position, Camera.main.transform.position) - 10.0f) / 50.0f);
    }
}
