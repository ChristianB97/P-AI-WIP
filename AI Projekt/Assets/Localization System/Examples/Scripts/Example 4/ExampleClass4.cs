using UnityEngine;

public class ExampleClass4 : MonoBehaviour
{
    private string[] allLanguages;

    public Vector2 buttonSize = new Vector2(100, 40);
    public GameObject myButtonPrefab;

    void Start()
    {
        allLanguages = TranslationEngine.Instance.Translator.GetCodeAvailableLanguages();

        Vector2 position = new Vector2(0, 50);
        foreach (string langKey in allLanguages)
        {
            //  Creates a new button
            GameObject button = Instantiate(myButtonPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);

            //  Sets the button's language
            button.GetComponent<ButtonScript>().SetLanguage(langKey);

            // Change the position of the next button
            position = new Vector2(position.x + buttonSize.x, position.y);

            if (position.x + buttonSize.x >= Screen.width)
            {
                position.x = 0;
                position.y += buttonSize.y;
            }
        }
    }

    public void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 200, 40), "Current language: " + TranslationEngine.Instance.Translator.SelectedLanguage);
    }

}
