using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSceneButton : MonoBehaviour
{
    [SerializeField] protected string sceneName; // Задаем нужную сцену в инспекторе

    //Инициализация кнопки
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => GoToNextScene(sceneName));
        }
    }


    // Метод, вызываемый при нажатии кнопки
    public void GoToNextScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene); // Переход на следую сцену
    }
}