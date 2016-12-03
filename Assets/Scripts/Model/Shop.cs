namespace KBEngine
{
    using UnityEngine;
    using System.Collections;
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.SceneManagement;

    public class Shop : Entity
    {
        ShopUnit _unit;
        public ShopUnit Unit
        {
            get
            {
                if (_unit == null && renderObj != null)
                    _unit = ((GameObject)renderObj).GetComponent<ShopUnit>();
                return _unit;
            }
        }
        public List<object> Product { get { return (List<object>)getDefinedProperty("product"); } }

    }
}