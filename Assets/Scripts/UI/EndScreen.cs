using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private MainMenuButton mainMenuButton;

    public void Enable()
    {
        gameObject.SetActive(true);
        mainMenuButton.Enable();
    }
}