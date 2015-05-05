using UnityEngine;
using ConsoleX;

public class TestConsole : MonoBehaviour
{
    public ConsoleController ConsoleController;

	// Use this for initialization
	void Start () 
    {
        ConsoleController.Console.RegisterCommand("test", strings => {Debug.Log("test");});
        ConsoleController.Console.RegisterCommand("test2", strings => { Debug.Log("test2"); });
	}

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (!ConsoleController.IsVisible)
            {
                ConsoleController.Show();
            }
            else
            {
                ConsoleController.Hide();
            }
        }
    }
}
