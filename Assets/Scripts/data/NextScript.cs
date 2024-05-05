using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class nextScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            SceneManager.LoadScene("RoleScene");
            Debug.Log("Detected key code: " + e.keyCode);
        }
    }
}
