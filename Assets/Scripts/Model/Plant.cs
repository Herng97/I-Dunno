namespace KBEngine
{
    using UnityEngine;
    using System.Collections;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.SceneManagement;

    public class Plant : Entity
    {
        public byte Id { get { return (byte)getDefinedProperty("Id"); } }
        public byte Level { get { return (byte)getDefinedProperty("level"); } }
        public bool IsWater { get { return System.Convert.ToBoolean(getDefinedProperty("isWater")); } }

        public void set_level(object old)
        {
            Event.fireOut("set_Level", this);
        }

        public void set_isWater(object old)
        {
            Event.fireOut("set_IsWater", this);
        }
    }
}