using UnityEngine;
using UnityEngine.EventSystems;

public class CloseMenuBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject mainScreen;

    public void onCloseButtonClick()
    {
        mainScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;
        Camera.main.GetComponent<PhysicsRaycaster>().eventMask = LayerMask.GetMask("Ignore Raycast");
        gameObject.SetActive(true);
    }

    public void onQuitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void onAbortButtonClick()
    {
        mainScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Camera.main.GetComponent<PhysicsRaycaster>().eventMask = LayerMask.GetMask("Default");;
        gameObject.SetActive(false);
    }
}
