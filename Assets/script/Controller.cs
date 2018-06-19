using UnityEngine;
using System.Collections;
using Com.Mygame;

namespace Com.Mygame
{
    public interface IUserActions
    {
        void priest_start();
        void priest_end();
        void devil_start();
        void devil_end();
        void boat_move();
        void Left_off_boat();
        void Right_off_boat();
        void restart();
        IEnumerator nextStep();
    }

    public interface IQueryGameStatus
    {
        bool isMoving();
        void setMoving(bool state);
        string isMessage();
        void setMessage(string message);
    }

    public class GameSceneController : System.Object, IUserActions, IQueryGameStatus
    {

        private static GameSceneController _instance;
        private Model _gen_game_obj;
        private bool moving = false;
        private string message = "";

        public static GameSceneController GetInstance()
        {
            if (null == _instance)
            {
                _instance = new GameSceneController();
            }
            return _instance;
        }

        // registration  

        public Model getModel()
        {
            return _gen_game_obj;
        }

        internal void setModel(Model ggo)
        {
            if (null == _gen_game_obj)
            {
                _gen_game_obj = ggo;
            }
        }

        // IQueryGameStatus  
        public bool isMoving() { return moving; }
        public void setMoving(bool state) { this.moving = state; }
        public string isMessage() { return message; }
        public void setMessage(string message) { this.message = message; }

        // IUserActions  
        public void priest_start() { _gen_game_obj.priestStartOnBoat(); }
        public void priest_end() { _gen_game_obj.priestEndOnBoat(); }
        public void devil_start() { _gen_game_obj.devilStartOnBoat(); }
        public void devil_end() { _gen_game_obj.devilEndOnBoat(); }
        public void boat_move() { _gen_game_obj.boat_move(); }
        public void Left_off_boat() { _gen_game_obj.getOffTheBoat(0); }
        public void Right_off_boat() { _gen_game_obj.getOffTheBoat(1); }
        public IEnumerator nextStep() { return _gen_game_obj.nextStep(); }

        public void restart()
        {
            moving = false;
            message = "";
            Application.LoadLevel(Application.loadedLevelName);
        }
    }

 
}

public class BaseCode : MonoBehaviour
{

    public string gameName;
    public string gameRule;

    void Start()
    {
        GameSceneController my = GameSceneController.GetInstance();
    }
}