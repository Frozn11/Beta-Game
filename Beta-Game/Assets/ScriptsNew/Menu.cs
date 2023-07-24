using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{




    public void munes()
    {
        SceneManager.LoadScene(0);
    }
    public void Tutorial()
    {
        SceneManager.LoadScene(1);
    }
    public void Ecape0()
    {
        SceneManager.LoadScene(2);
    }
    public void sky0()
    {
        SceneManager.LoadScene(3);
    }
    public void sky1()
    {
        SceneManager.LoadScene(4);
    }
    public void sandbox2()
    {
        SceneManager.LoadScene(5);
    }
    public void sandbox0()
    {
        SceneManager.LoadScene(6);
    }
    public void sandbox1()
    {
        SceneManager.LoadScene(7);
    }
    public void Sky2()
    {
        SceneManager.LoadScene(8);
    }
    public void Sky3()
    {
        SceneManager.LoadScene(9);
    }
    public void Sky4()
    {
        SceneManager.LoadScene(10);
    }
    public void Ecape1()
    {
        SceneManager.LoadScene(11);
    }
    public void Ecape2()
    {
        SceneManager.LoadScene(12);
    }
    public void Steam()
    {
        Application.OpenURL("https://store.steampowered.com/app/1228610/Karlson");
    }
    public void YouTubeElectronicGems()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCPzWlhG7QM56Y8MYB3qMVnQ");
    }
    public void YouTubeME()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCyLuV5nSpxjPTn-Ozr7imZw");
    }
    public void ExitGmae()
    {
        Application.Quit();
    }

}
