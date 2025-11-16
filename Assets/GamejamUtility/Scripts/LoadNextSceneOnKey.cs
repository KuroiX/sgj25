using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class LoadNextSceneOnKey : MonoBehaviour
{
    [SerializeField] private Key[] keys;

    private KeyControl[] _keyControls;

    private SceneLoader _sceneLoader;

    private bool _isLoading;

    private void Start()
    {
        _sceneLoader = FindObjectOfType<SceneLoader>().GetComponent<SceneLoader>();
        //_keyControls = Keyboard.current.allKeys.Where(key => keys.Contains(key.keyCode)).ToArray();
    }

    private void Update()
    {
        if (_isLoading) return;

        if (!Keyboard.current.anyKey.wasPressedThisFrame &&
            !(Gamepad.current is not null &&
              (Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.buttonWest.wasPressedThisFrame ||
               Gamepad.current.buttonEast.wasPressedThisFrame ||
               Gamepad.current.startButton.wasPressedThisFrame))) return;

        _sceneLoader.LoadNextScene();
        _isLoading = true;
    }
}