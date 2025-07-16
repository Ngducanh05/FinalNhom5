using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log(">> StartGame() được gọi!");
        SceneManager.LoadScene("Map 1");//(nh? s?a cái Map thành tên scene c?a ??c Anh làm Nha)
    }
}
