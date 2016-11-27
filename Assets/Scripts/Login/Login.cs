using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KBEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public Text Message;
    public InputField PasswordField;
    public InputField IdField;


    // Use this for initialization



    void Start()
    {
        KBEngine.Event.registerOut("onConnectStatus", this, "onConnectStatus");
        KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
        KBEngine.Event.registerOut("onCreateAccountResult", this, "onCreateAccountResult");
        KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
    }

    public void ChangAccountID(int value)
    {
        IdField.text = System.Convert.ToChar(((byte)'a') + value).ToString();
        PasswordField.text = System.Convert.ToChar(((byte)'a') + value).ToString();
    }

    public void onConnectStatus(bool status)
    {
        if (status == true)
            Message.text = ("Connect Success!");
        else
            Message.text = ("Connect Fail!");
    }

    public void SignUp()
    {
        KBEngine.Event.fireIn("createAccount", IdField.text, PasswordField.text, System.Text.Encoding.UTF8.GetBytes("kbengine_unity3d_demo"));
    }

    public void onCreateAccountResult(System.UInt16 retcode, byte[] datas)
    {
        if (retcode != 0)
        {
            Message.text = "Sign up Error! error=" + KBEngineApp.app.serverErr(retcode);
            return;
        }

        if (KBEngineApp.validEmail(IdField.text))
        {
            Message.text = "Sign up successfully! Please activate your Email!";
        }
        else
        {
            Message.text = "Sign up successfully!";
        }
    }

    public void onLoginFailed(System.UInt16 failedcode)
    {
        if (failedcode == 20)
        {
            Message.text = "Login Failed, Error=" + KBEngineApp.app.serverErr(failedcode) + ", " + System.Text.Encoding.ASCII.GetString(KBEngineApp.app.serverdatas());
        }
        else
        {
            Message.text = "login Failed, Error=" + KBEngineApp.app.serverErr(failedcode);
        }
    }

    public void onLoginSuccessfully(System.UInt64 rndUUID, System.Int32 eid, Account accountEntity)
    {
        Message.text = "Login Successfully!";
    }


    public void onLogin()
    {

        Message.text = "Please wait...";
        KBEngine.Event.fireIn("login", IdField.text, PasswordField.text, System.Text.Encoding.UTF8.GetBytes("2016.6.27"));

    }



}
