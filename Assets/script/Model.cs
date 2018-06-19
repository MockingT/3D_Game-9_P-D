using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.Mygame;

public class Model : MonoBehaviour
{

    Stack<GameObject> priests_start = new Stack<GameObject>();
    Stack<GameObject> priests_end = new Stack<GameObject>();
    Stack<GameObject> devils_start = new Stack<GameObject>();
    Stack<GameObject> devils_end = new Stack<GameObject>();

    GameObject[] boat = new GameObject[2];
    GameObject boat_obj;
    int side = 1;               // side records where boat docks  

    Vector3 boatStartPos = new Vector3(-11, -3, 0); // boat start from left
    Vector3 boatEndPos = new Vector3(-1, -3, 0); // boat end at right
    Vector3 shoreStartPos = new Vector3(-19, -5, 0); // the left shore
    Vector3 shoreEndPos = new Vector3(8, -5, 0);  // the right shore
    Vector3 priestsStartPos = new Vector3(-18, -1.5f, 0);
    Vector3 priestsEndPos = new Vector3(7, -1.5f, 0);
    Vector3 devilsStartPos = new Vector3(-24, -1.5f, 0);
    Vector3 devilsEndPos = new Vector3(13, -1.5f, 0);
    // the original postion

    public float speed = 50f;

    float gap = 1.5f;

    void Start()
    {
        GameSceneController.GetInstance().setModel(this);
        loadSrc();
    }

    void loadSrc()
    {
        // shore  
        Instantiate(Resources.Load("Prefabs/Shore"), shoreStartPos, Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/Shore"), shoreEndPos, Quaternion.identity);
        // boat  
        boat_obj = Instantiate(Resources.Load("Prefabs/Boat"), boatStartPos, Quaternion.identity) as GameObject;
        // priests & devils  
        for (int i = 0; i < 3; ++i)
        {
            GameObject priest = Instantiate(Resources.Load("Prefabs/Priest")) as GameObject;
            priest.transform.position = getCharacterPosition(priestsStartPos, i);
            priest.tag = "Priest";
            priests_start.Push(priest);
            GameObject devil = Instantiate(Resources.Load("Prefabs/Devil")) as GameObject;
            devil.transform.position = getCharacterPosition(devilsStartPos, i);
            devil.tag = "Devil";
            devils_start.Push(devil);
        }
        // light  
        Instantiate(Resources.Load("Prefabs/Light"));
    }

    int boatCapacity()
    {
        int capacity = 0;
        for (int i = 0; i < 2; ++i)
        {
            if (boat[i] == null) capacity++;
        }
        return capacity;
    }

    void getOnTheBoat(GameObject obj)
    {
        if (boatCapacity() != 0)
        {
            obj.transform.parent = boat_obj.transform;
            Vector3 target = new Vector3();
            if (boat[0] == null)
            {
                boat[0] = obj;
                target = boat_obj.transform.position +  new Vector3(-1f, 1.5f, 0);
            }
            else
            {
                boat[1] = obj;
                target = boat_obj.transform.position + new Vector3(1f, 1.5f, 0);
            }
            actionmanager.ActionManager.GetInstance().ApplyMoveToYZAction(obj, target, speed);
        }
    }

    public void boat_move()
    {
        if (boatCapacity() != 2)
        {
            if (side == 1)
            {
                actionmanager.ActionManager.GetInstance().ApplyMoveToAction(boat_obj, boatEndPos, speed);
                side = 2;
            }
            else if (side == 2)
            {
                actionmanager.ActionManager.GetInstance().ApplyMoveToAction(boat_obj, boatStartPos, speed);
                side = 1;
            }
        }
    }

    public void getOffTheBoat(int bside)
    {
        if (boat[bside] != null)
        {
            boat[bside].transform.parent = null;
            Vector3 target = new Vector3();
            if (side == 1)
            {
                if (boat[bside].tag == "Priest")
                {
                    priests_start.Push(boat[bside]);
                    target = getCharacterPosition(priestsStartPos, priests_start.Count - 1);
                }
                else if (boat[bside].tag == "Devil")
                {
                    devils_start.Push(boat[bside]);
                    target = getCharacterPosition(devilsStartPos, devils_start.Count - 1);
                }
            }
            else if (side == 2)
            {
                if (boat[bside].tag == "Priest")
                {
                    priests_end.Push(boat[bside]);
                    target = getCharacterPosition(priestsEndPos, priests_end.Count - 1);
                }
                else if (boat[bside].tag == "Devil")
                {
                    devils_end.Push(boat[bside]);
                    target = getCharacterPosition(devilsEndPos, devils_end.Count - 1);
                }
            }
            actionmanager.ActionManager.GetInstance().ApplyMoveToYZAction(boat[bside], target, speed);
            boat[bside] = null;
        }
    }

    public void priestStartOnBoat()
    {
        if (priests_start.Count != 0 && boatCapacity() != 0 && side == 1)
            getOnTheBoat(priests_start.Pop());
    }

    public void priestEndOnBoat()
    {
        if (priests_end.Count != 0 && boatCapacity() != 0 && side == 2)
            getOnTheBoat(priests_end.Pop());
    }

    public void devilStartOnBoat()
    {
        if (devils_start.Count != 0 && boatCapacity() != 0 && side == 1)
            getOnTheBoat(devils_start.Pop());
    }

    public void devilEndOnBoat()
    {
        if (devils_end.Count != 0 && boatCapacity() != 0 && side == 2)
            getOnTheBoat(devils_end.Pop());
    }

    Vector3 getCharacterPosition(Vector3 pos, int index)
    {
        return new Vector3(pos.x + gap * index, pos.y, pos.z);
    }

    void check()
    {
        GameSceneController scene = GameSceneController.GetInstance();
        int pOnb = 0, dOnb = 0;
        int priests_s = 0, devils_s = 0, priests_e = 0, devils_e = 0;

        if (priests_end.Count == 3 && devils_end.Count == 3)
        {
            scene.setMessage("Win!");
            return;
        }

        for (int i = 0; i < 2; ++i)
        {
            if (boat[i] != null && boat[i].tag == "Priest") pOnb++;
            else if (boat[i] != null && boat[i].tag == "Devil") dOnb++;
        }
        if (side == 1)
        {
            priests_s = priests_start.Count + pOnb;
            devils_s = devils_start.Count + dOnb;
            priests_e = priests_end.Count;
            devils_e = devils_end.Count;
        }
        else if (side == 2)
        {
            priests_s = priests_start.Count;
            devils_s = devils_start.Count;
            priests_e = priests_end.Count + pOnb;
            devils_e = devils_end.Count + dOnb;
        }
        if ((priests_s != 0 && priests_s < devils_s) || (priests_e != 0 && priests_e < devils_e))
        {
            scene.setMessage("Lose!");
        }
    }

    private int randomValue()
    {
        float num = Random.Range(0f, 1f);
        if (num <= 0.5f) return 1;
        else return 2;
    }

    public IEnumerator nextStep()
    {
        if (side == 1 && priests_start.Count == 3 && devils_start.Count == 3)
        {
            int turn = randomValue();
            if (turn == 1)
            {
                priestStartOnBoat();
                devilStartOnBoat();
            }
            else
            {
                devilStartOnBoat();
                devilStartOnBoat();
            }
        }
        else if (side == 2 && priests_start.Count == 2 && devils_start.Count == 2)
        {

            priestEndOnBoat();
        }
        else if (side == 2 && priests_start.Count == 3 &&
               devils_start.Count == 1)
        {
            devilEndOnBoat();
        }
        else if (side == 1 && priests_start.Count == 3 &&
               devils_start.Count == 2)
        {

            devilStartOnBoat();
            devilStartOnBoat();
        }
        else if (side == 2 && priests_start.Count == 3 &&
               devils_start.Count == 0)
        {
            devilEndOnBoat();
        }
        else if (side == 1 && priests_start.Count == 3 &&
               devils_start.Count == 1)
        {
            priestStartOnBoat();
            priestStartOnBoat();
        }
        else if (side == 2 && priests_start.Count == 1 &&
               devils_start.Count == 1)
        {
            priestEndOnBoat();
            devilEndOnBoat();
        }
        else if (side == 1 && priests_start.Count == 2 &&
               devils_start.Count == 2)
        {
            priestStartOnBoat();
            priestStartOnBoat();
        }
        else if (side == 2 && priests_start.Count == 0 &&
               devils_start.Count == 2)
        {
            devilEndOnBoat();
        }
        else if (side == 1 && priests_start.Count == 0 &&
               devils_start.Count == 3)
        {
            devilStartOnBoat();
            devilStartOnBoat();
        }
        else if (side == 2 && priests_start.Count == 0 &&
               devils_start.Count == 1)
        {
            int turn = randomValue();
            if (turn == 1)
            {
                devilEndOnBoat();
            }
            else
            {
                priestEndOnBoat();
            }
        }
        else if (side == 1 && priests_start.Count == 2 &&
               devils_start.Count == 1)
        {
            priestStartOnBoat();
        }
        else if (side == 1 && priests_start.Count == 0 &&
               devils_start.Count == 2)
        {
            devilStartOnBoat();
            devilStartOnBoat();
        }
        else if (side == 1 && priests_start.Count == 1 &&
               devils_start.Count == 1)
        {
            priestStartOnBoat();
            devilStartOnBoat();
        }
        yield return new WaitForSeconds(1.0f);
        boat_move();
        yield return new WaitForSeconds(1.0f);
        getOffTheBoat(1);
        getOffTheBoat(0);
    
    }

    void Update()
    {
        check();
    }

}