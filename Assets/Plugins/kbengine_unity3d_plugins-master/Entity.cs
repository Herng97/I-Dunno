﻿namespace KBEngine
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /*
		KBEngine逻辑层的实体基础类
		所有扩展出的游戏实体都应该继承于该模块
	*/
    public class Entity
    {
        /*----------    Extension   ----------*/
        public Int32 Uid { get { return id; } }
        public Vector3 Position { get { return position; } }

        // 当前玩家最后一次同步到服务端的位置与朝向
        // 这两个属性是给引擎KBEngine.cs用的，别的地方不要修改
        internal protected Vector3 _entityLastLocalPos = new Vector3(0f, 0f, 0f);
        internal protected Vector3 _entityLastLocalDir = new Vector3(0f, 0f, 0f);
        protected internal Int32 id = 0;
        public string className = "";
        public Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        internal protected float velocity = 0.0f;

        internal protected bool isOnGround = true;

        public object renderObj = null;

        internal protected Mailbox baseMailbox = null;
        internal protected Mailbox cellMailbox = null;

        // enterworld之后设置为true
        internal protected bool inWorld = false;

        /// <summary>
        /// 对于玩家自身来说，它表示是否自己被其它玩家控制了；
        /// 对于其它entity来说，表示我本机是否控制了这个entity
        /// </summary>
        internal protected bool isControlled = false;

        // __init__调用之后设置为true
        internal protected bool inited = false;

        // entityDef属性，服务端同步过来后存储在这里
        private Dictionary<string, Property> defpropertys_ =
            new Dictionary<string, Property>();

        private Dictionary<UInt16, Property> iddefpropertys_ =
            new Dictionary<UInt16, Property>();

        internal protected static void clear()
        {
        }

        public Entity()
        {
            foreach (Property e in EntityDef.moduledefs[GetType().Name].propertys.Values)
            {
                Property newp = new Property();
                newp.name = e.name;
                newp.utype = e.utype;
                newp.properUtype = e.properUtype;
                newp.properFlags = e.properFlags;
                newp.aliasID = e.aliasID;
                newp.defaultValStr = e.defaultValStr;
                newp.setmethod = e.setmethod;
                newp.val = newp.utype.parseDefaultValStr(newp.defaultValStr);
                defpropertys_.Add(e.name, newp);
                iddefpropertys_.Add(e.properUtype, newp);
            }
        }

        internal protected virtual void onDestroy()
        {
        }

        public bool isPlayer()
        {
            return id == KBEngineApp.app.entity_id;
        }

        internal protected void addDefinedProperty(string name, object v)
        {
            Property newp = new Property();
            newp.name = name;
            newp.properUtype = 0;
            newp.val = v;
            newp.setmethod = null;
            defpropertys_.Add(name, newp);
        }

        internal protected object getDefinedProperty(string name)
        {
            Property obj = null;
            if (!defpropertys_.TryGetValue(name, out obj))
            {
                return null;
            }

            return defpropertys_[name].val;
        }

        internal protected void setDefinedProperty(string name, object val)
        {
            defpropertys_[name].val = val;
        }

        internal protected object getDefinedPropertyByUType(UInt16 utype)
        {
            Property obj = null;
            if (!iddefpropertys_.TryGetValue(utype, out obj))
            {
                return null;
            }

            return iddefpropertys_[utype].val;
        }

        internal protected void setDefinedPropertyByUType(UInt16 utype, object val)
        {
            iddefpropertys_[utype].val = val;
        }

        /*
			KBEngine的实体构造函数，与服务器脚本对应。
			存在于这样的构造函数是因为KBE需要创建好实体并将属性等数据填充好才能告诉脚本层初始化
		*/
        internal protected virtual void __init__()
        {
        }

        public virtual void callPropertysSetMethods()
        {
            foreach (Property prop in iddefpropertys_.Values)
            {
                object oldval = getDefinedPropertyByUType(prop.properUtype);
                System.Reflection.MethodInfo setmethod = prop.setmethod;

                if (setmethod != null)
                {
                    if (prop.isBase())
                    {
                        if (inited && !inWorld)
                        {
                            //Dbg.DEBUG_MSG(className + "::callPropertysSetMethods(" + prop.name + ")"); 
                            setmethod.Invoke(this, new object[] { oldval });
                        }
                    }
                    else
                    {
                        if (inWorld)
                        {
                            if (prop.isOwnerOnly() && !isPlayer())
                                continue;

                            setmethod.Invoke(this, new object[] { oldval });
                        }
                    }
                }
                else
                {
                    //Dbg.DEBUG_MSG(className + "::callPropertysSetMethods(" + prop.name + ") not found set_*"); 
                }
            }
        }

        internal protected void baseCall(string methodname, params object[] arguments)
        {
            if (KBEngineApp.app.currserver == "loginapp")
            {
                Dbg.ERROR_MSG(className + "::baseCall(" + methodname + "), currserver=!" + KBEngineApp.app.currserver);
                return;
            }

            Method method = null;
            if (!EntityDef.moduledefs[className].base_methods.TryGetValue(methodname, out method))
            {
                Dbg.ERROR_MSG(className + "::baseCall(" + methodname + "), not found method!");
                return;
            }

            UInt16 methodID = method.methodUtype;

            if (arguments.Length != method.args.Count)
            {
                Dbg.ERROR_MSG(className + "::baseCall(" + methodname + "): args(" + (arguments.Length) + "!= " + method.args.Count + ") size is error!");
                return;
            }

            baseMailbox.newMail();
            baseMailbox.bundle.writeUint16(methodID);

            try
            {
                for (var i = 0; i < method.args.Count; i++)
                {
                    if (method.args[i].isSameType(arguments[i]))
                    {
                        method.args[i].addToStream(baseMailbox.bundle, arguments[i]);
                    }
                    else
                    {
                        throw new Exception("arg" + i + ": " + method.args[i].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Dbg.ERROR_MSG(className + "::baseCall(method=" + methodname + "): args is error(" + e.Message + ")!");
                baseMailbox.bundle = null;
                return;
            }

            baseMailbox.postMail(null);
        }

        internal protected void cellCall(string methodname, params object[] arguments)
        {
            if (KBEngineApp.app.currserver == "loginapp")
            {
                Dbg.ERROR_MSG(className + "::cellCall(" + methodname + "), currserver=!" + KBEngineApp.app.currserver);
                return;
            }

            Method method = null;
            if (!EntityDef.moduledefs[className].cell_methods.TryGetValue(methodname, out method))
            {
                Dbg.ERROR_MSG(className + "::cellCall(" + methodname + "), not found method!");
                return;
            }

            UInt16 methodID = method.methodUtype;

            if (arguments.Length != method.args.Count)
            {
                Dbg.ERROR_MSG(className + "::cellCall(" + methodname + "): args(" + (arguments.Length) + "!= " + method.args.Count + ") size is error!");
                return;
            }

            if (cellMailbox == null)
            {
                Dbg.ERROR_MSG(className + "::cellCall(" + methodname + "): no cell!");
                return;
            }

            cellMailbox.newMail();
            cellMailbox.bundle.writeUint16(methodID);

            try
            {
                for (var i = 0; i < method.args.Count; i++)
                {
                    if (method.args[i].isSameType(arguments[i]))
                    {
                        method.args[i].addToStream(cellMailbox.bundle, arguments[i]);
                    }
                    else
                    {
                        throw new Exception("arg" + i + ": " + method.args[i].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Dbg.ERROR_MSG(className + "::cellCall(" + methodname + "): args is error(" + e.Message + ")!");
                cellMailbox.bundle = null;
                return;
            }

            cellMailbox.postMail(null);
        }

        internal protected void enterWorld()
        {
            // Dbg.DEBUG_MSG(className + "::enterWorld(" + getDefinedProperty("uid") + "): " + id); 
            inWorld = true;

            try
            {
                onEnterWorld();
            }
            catch (Exception e)
            {
                Dbg.ERROR_MSG(className + "::onEnterWorld: error=" + e.ToString());
            }

            Event.fireOut("onEnterWorld", new object[] { this });
        }

        internal protected virtual void onEnterWorld()
        {
        }

        internal protected void leaveWorld()
        {
            // Dbg.DEBUG_MSG(className + "::leaveWorld: " + id); 
            inWorld = false;

            try
            {
                onLeaveWorld();
            }
            catch (Exception e)
            {
                Dbg.ERROR_MSG(className + "::onLeaveWorld: error=" + e.ToString());
            }

            Event.fireOut("onLeaveWorld", new object[] { this });
        }

        internal protected virtual void onLeaveWorld()
        {
        }

        internal protected virtual void enterSpace()
        {
            // Dbg.DEBUG_MSG(className + "::enterSpace(" + getDefinedProperty("uid") + "): " + id); 
            inWorld = true;

            try
            {
                onEnterSpace();
            }
            catch (Exception e)
            {
                Dbg.ERROR_MSG(className + "::onEnterSpace: error=" + e.ToString());
            }

            Event.fireOut("onEnterSpace", new object[] { this });
        }

        internal protected virtual void onEnterSpace()
        {
        }

        internal protected virtual void leaveSpace()
        {
            // Dbg.DEBUG_MSG(className + "::leaveSpace: " + id); 
            inWorld = false;

            try
            {
                onLeaveSpace();
            }
            catch (Exception e)
            {
                Dbg.ERROR_MSG(className + "::onLeaveSpace: error=" + e.ToString());
            }

            Event.fireOut("onLeaveSpace", new object[] { this });
        }

        internal protected virtual void onLeaveSpace()
        {
        }

        internal protected virtual void set_position(object old)
        {
            Vector3 v = (Vector3)getDefinedProperty("position");
            position = v;
            //Dbg.DEBUG_MSG(className + "::set_position: " + old + " => " + v); 

            if (isPlayer())
                KBEngineApp.app.entityServerPos(position);

            if (inWorld)
                Event.fireOut("set_position", new object[] { this });
        }

        internal protected virtual void onUpdateVolatileData()
        {
        }

        internal protected virtual void set_direction(object old)
        {
            Vector3 v = (Vector3)getDefinedProperty("direction");

            direction.x = v.x * 360 / ((float)System.Math.PI * 2);
            direction.y = v.y * 360 / ((float)System.Math.PI * 2);
            direction.z = v.z * 360 / ((float)System.Math.PI * 2);

            //Dbg.DEBUG_MSG(className + "::set_direction: " + old + " => " + v); 

            if (inWorld)
                Event.fireOut("set_direction", new object[] { this });
        }

        /// <summary>
        /// This callback method is called when the local entity control by the client has been enabled or disabled. 
        /// See the Entity.controlledBy() method in the CellApp server code for more infomation.
        /// </summary>
        /// <param name="isControlled">
        /// 对于玩家自身来说，它表示是否自己被其它玩家控制了；
        /// 对于其它entity来说，表示我本机是否控制了这个entity
        /// </param>
        internal protected virtual void onControlled(bool isControlled_)
        {

        }
    }
}
