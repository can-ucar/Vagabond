using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingControl : MonoBehaviour
{
   void Awake()
   {
      Invoke(nameof(ChangeScene),0.3f);
   }

   void ChangeScene()
   {
      SceneManager.LoadScene(1);
   }
}
