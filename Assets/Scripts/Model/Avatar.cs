namespace KBEngine
{
    using UnityEngine;
    using System.Collections;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.SceneManagement;

    public class Avatar : Entity
    {
        player _player;
        public player Player
        {
            get
            {
                if (_player == null && renderObj != null)
                    _player = ((GameObject)renderObj).GetComponent<player>();
                return _player;
            }
        }
        public override void __init__()
        {
            base.__init__();
            if (isPlayer())
            {
                SceneManager.LoadScene("Game");
                Event.registerIn("UpdatePlayer", this, "UpdatePlayer");
            }
        }
        public void UpdatePlayer(Vector3 position)
        {
       
            this.position = position;
        }

    }
}