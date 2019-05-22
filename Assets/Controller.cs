using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

    [System.Serializable]
    public struct TaplighUnit
    {
        public string name;
        public string code;
    }

    public TaplighUnit[] units;
    public Text message = null;

    private string verifyToken = null;
    private readonly string TOKEN = "7f15b5e9-88e1-4cb7-aa56-1ae9421b0553"; // This field specifies your Tapligh application token.
    private readonly bool TEST_MODE = false;  // This field specifies that you are in test mode or not.
    
    void Start()
    {
        Tapligh.Reward.Initialize(TOKEN, TEST_MODE);
        message.text = "Tapligh SDK Initialized : " + Tapligh.Reward.IsInitializeDone();
    }


    /************************************************ Main methods ************************************************/


    /*
     * If you have many unit codes you can import them to [TaplighUnit units object] and call LoadAdByUnits function.
     * In LoadAdByUnits function we get your unit name and find unit code in your units. Then LoadAd will be call. 
     */
    public void LoadAdByUnits(string unit_name)
    {
        string code = GetUnitCode(unit_name);
        if (code != null)
        {
            Tapligh.Reward.OnAdListener = OnAdResult;
            Tapligh.Reward.OnLoadReadyListener = OnAdReady;
            Tapligh.Reward.OnLoadErrorListener = OnLoadError;
            Tapligh.Reward.OnRewardReadyListener = OnRewardReady;
            Tapligh.Reward.LoadAd(code);
        }
    }

    /*
     * If you want load ad just with one unit code you can use LoadAdByUnit.
     * In LoadAdByUnit function we get yout unit code and then LoadAd will be call.
     */
    public void LoadAdByUnit(string unit)
    {
        Tapligh.Reward.OnAdListener = OnAdResult;
        Tapligh.Reward.OnLoadReadyListener = OnAdReady;
        Tapligh.Reward.OnLoadErrorListener = OnLoadError;
        Tapligh.Reward.OnRewardReadyListener = OnRewardReady;
        Tapligh.Reward.LoadAd(unit);
    }

    /*
     * VerifyToken function will check your token verification.
     */
    public void VerifyToken()
    {
        Tapligh.Reward.OnTokenVerifyFinishedListener = OnVerifyListener;
        Tapligh.Reward.VerifyToken(verifyToken);
    }

    /*
     * GetTaplighVersion function will show Tapligh sdk version.
     */
    public void GetTaplighVersion()
    {
        message.text = "SDK Version : " + Tapligh.Reward.GetTaplighVersion();
    }


    /************************************************ Private methods ************************************************/


    /*
     * You can receive reward here if the ad impression process completed successfully.
     */
    private void OnRewardReady(string reward)
    {
        message.text = "Reward : " + reward;
    }

    /*
     * In the load ad process, if all is OK, this function will be called.
     */
    private void OnAdReady(string unit, string token)
    {
        Debug.Log("Unity3D Controller: On Ad Ready: VERIFY TOKEN [" + token + "] & UNIT [" + unit + "]");
        if (message != null)
        {
            verifyToken = token;
            message.text = "Verify Token [" + token + "]";
        }

        Tapligh.Reward.ShowAd(unit);
    }

    /*
     * OnAdResult function will be called to display the action result.
     */
    private void OnAdResult(AdResult result, string token)
    {
        string msg = null;

        switch (result) {
            case AdResult.NO_INTERNET_ACSSES: msg = "No internet accsses"; break;
            case AdResult.BAD_TOKEN_USED: msg = "Bad token used"; break;
            case AdResult.NO_AD_READY: msg = "No ad ready"; break;
            case AdResult.INTERNAL_ERROR: msg = "Enternal error"; break;
            case AdResult.TOKEN_EXPIRED: msg = "Token expired"; break;
            case AdResult.AD_CLICKED: msg = "Ad clicked"; break;
            case AdResult.AD_IMAGE_CLOSED: msg = "Ad image closed"; break;
            case AdResult.AD_VIDEO_CLOSED_AFTER_FULL_VIEW: msg = "Ad video closed after full view"; break;
            case AdResult.AD_VIDEO_CLOSED_ON_VIEW: msg = "Ad video closed on view"; break;
            case AdResult.SKIP_AND_CLICKED: msg = "Skip and clicked"; break;
            case AdResult.SKIP_AND_CLOSED: msg = "Skip and closed"; break;
        }

        message.text = "Ad Result : " + msg;
    }

    /*
     * In the load ad process, if an error has occurred, this function will be called.
     */
    private void OnLoadError(string unit, LoadErrorStatus error)
    {
        string msg = null;

        switch (error) {
            case LoadErrorStatus.NO_INTERNET_ACCSSES: msg = "No internet accsses"; break;
            case LoadErrorStatus.APP_NOT_FOUND: msg = "App not found"; break;
            case LoadErrorStatus.AD_UNIT_DISABLED: msg = "Ad unit disabled"; break;
            case LoadErrorStatus.AD_UNIT_NOT_FOUND: msg = "Ad unit not found"; break;
            case LoadErrorStatus.AD_UNIT_NOT_READY: msg = "Ad unit not ready"; break;
            case LoadErrorStatus.INTERNAL_ERROR: msg = "Enternal error"; break;
            case LoadErrorStatus.NO_AD_READY: msg = "No ad ready"; break;
        }

        Debug.Log("Unity3D Controller: On Load Error: UNIT [" + unit + "]");
        message.text = "On Load Error : " + msg;
    }

    /*
     * OnVerifyListener function will be called to display the ad token status.
     */
    private void OnVerifyListener(TokenResult tokenResult)
    {
        string msg = null;

        switch (tokenResult) {
            case TokenResult.INTERNAL_ERROR: msg = "Enternal error"; break;
            case TokenResult.NOT_USED: msg = "Not used"; break;
            case TokenResult.SUCCESS: msg = "Success"; break;
            case TokenResult.TOKEN_EXPIRED: msg = "Token expired"; break;
            case TokenResult.TOKEN_NOT_FOUND: msg = "Token not found"; break;
        }
        message.text = "Token Verify : " + msg;
    }

    private string GetUnitCode(string name)
    {
        string code = null;
        var i = System.Array.FindIndex(units, x => x.name == name);
        if (i >= 0) code = units[i].code;
       
        if (code == null || code == "")
        {
            message.text = "Unit Code [" + name + "] Dose not exist";
            return null;
        }

        return code;
    }

}
