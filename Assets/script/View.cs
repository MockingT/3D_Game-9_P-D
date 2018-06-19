using UnityEngine;
using System.Collections;
using Com.Mygame;

public class View : MonoBehaviour
{

    float width;
    float height;
    float castw(float a)
    {
        return (Screen.width - width) / a;
    }
    float casth(float a)
    {
        return (Screen.height - height) / a;
    }

    GameSceneController scene;
    IQueryGameStatus state;
    IUserActions action;

    // Use this for initialization  
    void Start()
    {
        scene = GameSceneController.GetInstance();
        state = GameSceneController.GetInstance() as IQueryGameStatus;
        action = GameSceneController.GetInstance() as IUserActions;
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 20;
        width = Screen.width / 12;
        height = Screen.height / 12;

        string message = state.isMessage();
        // win or fail
        if (message != "")
        {
            if (GUI.Button(new Rect(castw(2f), casth(2f), width, height), message))
            {
                action.restart();
            }
        }
        else
        {
            if (GUI.Button(new Rect(castw(2f), casth(6f), width, height), "GO"))
            {
                action.boat_move();
            }
            if (GUI.Button(new Rect(castw(2.5f), casth(1f), width, height), "OFF"))
            {
                action.Left_off_boat();
            }
            if (GUI.Button(new Rect(castw(1.6f), casth(1f), width, height), "OFF"))
            {
                action.Right_off_boat();
            }
            if (GUI.Button(new Rect(castw(1.2f), casth(4f), width, height), "ON"))
            {
                action.priest_end();
            }
            if (GUI.Button(new Rect(castw(1f), casth(4f), width, height), "ON"))
            {
                action.devil_end();
            }
            if (GUI.Button(new Rect(castw(10f), casth(4f), width, height), "ON"))
            {
                action.devil_start();
            }
            if (GUI.Button(new Rect(castw(4.5f), casth(4f), width, height), "ON"))
            {
                action.priest_start();
            }
            if (GUI.Button(new Rect(castw(3f), casth(4f), width, height), "NEXT"))
            {
                StartCoroutine(action.nextStep());
            }
        }
    }
}
