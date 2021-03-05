using UnityEngine;

public class SFXDestroyer : MonoBehaviour
{
    private AudioSource _mySource;

    private void Start()
    {
        _mySource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_mySource != null)
        {
            if (_mySource.isPlaying == false)
            {
                Destroy(gameObject);
            }
        }
    }
}
