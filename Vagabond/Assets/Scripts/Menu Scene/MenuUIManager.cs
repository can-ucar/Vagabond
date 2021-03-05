using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{

    public void PlayButton()
    {
        SceneManager.LoadScene(2);
    }
    
}
