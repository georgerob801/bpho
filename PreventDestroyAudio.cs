using UnityEngine;

public class PreventDestroyAudio : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
}
