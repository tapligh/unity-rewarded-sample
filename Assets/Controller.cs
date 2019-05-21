using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

    private string TOKEN = "K3R8Z6GOGK8SMJFHV7WSR9LV9FWOX4";
    private string UNIT_CODE = "D53ADD3EDB88034464187CEAD03ADB";
    private bool TEST_MODE = false;

    private string verifyToken = "";

    public Text message = null;

    void Start()
    {
        Tapligh.Reward.Initialize(TOKEN, TEST_MODE);
        Tapligh.Reward.OnAdListener = OnAdResult;
        Tapligh.Reward.OnRewardReadyListener = OnRewardReady;
        IsInitialized();
    }

    /************************************************ Main methods ************************************************/

    public void LoadAd()
    {
        Tapligh.Reward.OnLoadReadyListener = OnAdReady;
        Tapligh.Reward.OnLoadErrorListener = OnLoadError;
        Tapligh.Reward.LoadAd(UNIT_CODE);
    }

    public void ShowAd()
    {
        Tapligh.Reward.ShowAd(UNIT_CODE);
    }

    public void VerifyToken()
    {
        Tapligh.Reward.OnTokenVerifyFinishedListener = OnVerifyListener;
        Tapligh.Reward.VerifyToken(verifyToken);
    }

    public void GetTaplighVersion()
    {
        message.text = "Reward SDK Version : " + Tapligh.Reward.GetTaplighVersion();
    }

    /************************************************ Private methods ************************************************/

    private void OnRewardReady(string reward)
    {
        message.text = "Reward : " + reward;
    }

    private void OnAdReady(string unit, string token)
    {
        Debug.Log("Unity3D Controller: On Ad Ready: VERIFY TOKEN [" + token + "] & UNIT [" + unit + "]");
        if (message != null)
        {
            verifyToken = token;
            message.text = "VERIFY TOKEN [" + token + "]";
        }
    }

    private void OnAdResult(AdResult result, string token)
    {
        string msg = null;

        switch (result) {
            case AdResult.NO_INTERNET_ACSSES: msg = "NO INTERNET ACSSES"; break;
            case AdResult.BAD_TOKEN_USED: msg = "BAD TOKEN USED"; break;
            case AdResult.NO_AD_READY: msg = "NO AD READY"; break;
            case AdResult.INTERNAL_ERROR: msg = "INTERNAL ERROR"; break;
            case AdResult.TOKEN_EXPIRED: msg = "TOKEN EXPIRED"; break;
            case AdResult.AD_CLICKED: msg = "AD CLICKED"; break;
            case AdResult.AD_IMAGE_CLOSED: msg = "AD IMAGE CLOSED"; break;
            case AdResult.AD_VIDEO_CLOSED_AFTER_FULL_VIEW: msg = "AD VIDEO CLOSED AFTER FULL VIEW"; break;
            case AdResult.AD_VIDEO_CLOSED_ON_VIEW: msg = "AD VIDEO CLOSED ON VIEW"; break;
            case AdResult.SKIP_AND_CLICKED: msg = "SKIP AND CLICKED"; break;
            case AdResult.SKIP_AND_CLOSED: msg = "SKIP AND CLOSED"; break;
        }

        message.text = "Ad Result : " + msg;
    }

    private void OnLoadError(string unit, LoadErrorStatus error)
    {
        string msg = null;

        switch (error) {
            case LoadErrorStatus.NO_INTERNET_ACCSSES: msg = "NO INTERNET ACCSSES"; break;
            case LoadErrorStatus.APP_NOT_FOUND: msg = "APP NOT FOUND"; break;
            case LoadErrorStatus.AD_UNIT_DISABLED: msg = "AD UNIT DISABLED"; break;
            case LoadErrorStatus.AD_UNIT_NOT_FOUND: msg = "AD UNIT NOT FOUND"; break;
            case LoadErrorStatus.AD_UNIT_NOT_READY: msg = "AD UNIT NOT READY"; break;
            case LoadErrorStatus.INTERNAL_ERROR: msg = "INTERNAL ERROR"; break;
            case LoadErrorStatus.NO_AD_READY: msg = "NO AD READY"; break;
        }

        Debug.Log("Unity3D Controller: On Load Error: UNIT [" + unit + "]");
        message.text = "On Load Error : " + msg;
    }

    private void OnVerifyListener(TokenResult tokenResult)
    {
        string msg = null;

        switch (tokenResult) {
            case TokenResult.INTERNAL_ERROR: msg = "ENTERNAL ERRROR"; break;
            case TokenResult.NOT_USED: msg = "NOT USED"; break;
            case TokenResult.SUCCESS: msg = "SUCCESS"; break;
            case TokenResult.TOKEN_EXPIRED: msg = "TOKEN EXPIRED"; break;
            case TokenResult.TOKEN_NOT_FOUND: msg = "TOKEN NOT FOUND"; break;
        }
        message.text = "Token Verify : " + msg;
    }

    private void IsInitialized()
    {
        message.text = "Tapligh SDK Initialized : " + Tapligh.Reward.IsInitializeDone();
    }

}
