using UnityEngine;
using ConsoleX;
using ConsoleX.Helpers;

public class TestConsole : MonoBehaviour
{
    public ConsoleController ConsoleController;

	// Use this for initialization
	void Start () 
    {
        ConsoleController.Console.RegisterCommand("test", strings => {Debug.Log("test");});
        ConsoleController.Console.RegisterCommand("test2", strings => { Debug.Log("test2"); });

        Debug.Log(Time.frameCount);
	    StartCoroutine(this.DoActionAfterFrames(() => Debug.Log(Time.frameCount), 5));
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

        //Event currentEvent = new Event();

        //while (Event.PopEvent(currentEvent))
        //{
        //    if (currentEvent.rawType == EventType.KeyDown &&
        //        currentEvent.character == '`')
        //    {
                
                
        //        currentEvent.Use();
        //        break;
        //    }
        //}
    }
}
