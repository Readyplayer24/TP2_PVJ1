using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject tpcObject;
    [SerializeField] private GameObject fpcObject;
    private bool isFps = false;

    private void Start()
    {
        tpcObject.SetActive(true);
        fpcObject.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            isFps = !isFps;

            tpcObject.SetActive(!isFps);
            fpcObject.SetActive(isFps);
        }
    }
}