using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public Sprite LoseBack;
    public Sprite LoseTitle;
    // Start is called before the first frame update
    public static UIHandler instance {get; private set;}
    private Label time;
    private ProgressBar health;
    private Button menu;
    private Button play;
    private Button again;
    private VisualElement overBackground;
    private VisualElement overTitle;
    private void Awake() {
        instance = this;
        time = GetComponent<UIDocument>().rootVisualElement.Q<Label>("Time");
        health = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("HealthBar");
        menu = GetComponent<UIDocument>().rootVisualElement.Q<Button>("Menu");
        if (menu != null) {
            menu.clickable.clicked += () => {
                SceneManager.LoadScene("Menu");
                Globalvar.Instance().lifeTime = 0;
            };
        }
        play = GetComponent<UIDocument>().rootVisualElement.Q<Button>("Play");
        if (play != null) {
            play.clickable.clicked += () => {
                SceneManager.LoadScene("SampleScene");
                Globalvar.Instance().lifeTime = 0;
            };
        }
        again = GetComponent<UIDocument>().rootVisualElement.Q<Button>("Again");
        if (again != null) {
            again.clickable.clicked += () => {
                SceneManager.LoadScene("Menu");
                Globalvar.Instance().lifeTime = 0;
            };
        }
        overBackground = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("OverBackground");
        overTitle = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("OverTitle");
        if (overBackground != null && overTitle != null) {
            if (!Globalvar.Instance().win) {
                overBackground.style.backgroundImage = new StyleBackground(LoseBack);
                overTitle.style.backgroundImage = new StyleBackground(LoseTitle);
            }
        }
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateTime(string text) {
        time.text = text;
    }
    public void UpdateHealth(int val) {
        health.value = val;
    }
}
