using UnityEngine;
using UnityEngine.UI;

public class ExitUI : MonoBehaviour
{
    private Button exitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        exitButton = GetComponent<Button>();
        exitButton.onClick.AddListener(()=> Application.Quit());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
