using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class ClickHandler : MonoBehaviour {
    public GameObject MainMenu;
    public GameObject Licenses;

    public Cubemap cubemap;

    private void Start() {
        MainMenu.SetActive(true);
        Licenses.SetActive(false);
    }

    public void ToLicenses() {
        MainMenu.SetActive(false);
        Licenses.SetActive(true);
    }

    public void ToTask(string taskNum) {
        SceneManager.LoadScene($"Task{taskNum}");
    }

    public void FromLicenses() {
        Licenses.SetActive(false);
        MainMenu.SetActive(true);
    }
}
