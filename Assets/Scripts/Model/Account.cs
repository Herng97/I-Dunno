namespace KBEngine
{
    using UnityEngine;
    using System.Collections;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.SceneManagement;

    public class Account : Entity
    {
        public override void __init__()
        {
            base.__init__();
            if (isPlayer())
                SceneManager.LoadScene("Game");
        }

    }
}