using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AdResult
{
    NO_INTERNET_ACSSES,
    BAD_TOKEN_USED,
    NO_AD_READY,
    INTERNAL_ERROR,
    TOKEN_EXPIRED,
    AD_CLICKED,
    AD_IMAGE_CLOSED,
    AD_VIDEO_CLOSED_AFTER_FULL_VIEW,
    AD_VIDEO_CLOSED_ON_VIEW,
    SKIP_AND_CLICKED,
    SKIP_AND_CLOSED
}

public enum LoadErrorStatus
{
    NO_INTERNET_ACCSSES,
    APP_NOT_FOUND,
    AD_UNIT_DISABLED,
    AD_UNIT_NOT_FOUND,
    AD_UNIT_NOT_READY,
    INTERNAL_ERROR,
    NO_AD_READY
}

public enum TokenResult
{
    TOKEN_NOT_FOUND,
    TOKEN_EXPIRED,
    NOT_USED,
    SUCCESS,
    INTERNAL_ERROR
}

class Tapligh : MonoBehaviour
{

    #if !UNITY_EDITOR && UNITY_ANDROID
        AndroidJavaClass _taplighJavaInterface;
        AndroidJavaObject _currentActivity;
    #endif

    private static Tapligh instance;
    private string TaplighJavaSrc = "com.tapligh.sdk.adview.TaplighUnity";

    private Action<AdResult, string> _onAdListener = null;
    public System.Action<AdResult, string> OnAdListener
    {
        get { return _onAdListener; }
        set { _onAdListener = value; }
    }

    private Action<string, string> _onLoadReadyListener = null;
    public System.Action<string, string> OnLoadReadyListener
    {
        get { return _onLoadReadyListener; }
        set { _onLoadReadyListener = value; }
    }

    private Action<string, LoadErrorStatus> _onLoadErrorListener = null;
    public System.Action<string, LoadErrorStatus> OnLoadErrorListener
    {
        get { return _onLoadErrorListener; }
        set { _onLoadErrorListener = value; }
    }

    private Action<TokenResult> _onTokenVerifyFinishedListener = null;
    public System.Action<TokenResult> OnTokenVerifyFinishedListener
    {
        get { return _onTokenVerifyFinishedListener; }
        set { _onTokenVerifyFinishedListener = value; }
    }

    private Action<string> _onRewardReadyListener = null;
    public System.Action<string> OnRewardReadyListener
    {
        get { return _onRewardReadyListener; }
        set { _onRewardReadyListener = value; }
    }

    public static Tapligh Reward
    {
        get {
            if (instance == null)
            {
                GameObject obj = new GameObject("TaplighUnityObject");
                obj.AddComponent<Tapligh>();
                instance = obj.GetComponent<Tapligh>();
            }
            return instance;
        }
    }
    
    void Awake()
    {
        Debug.Log("Unity3D Reward: Tapligh CREATED - CURRENT ACTIVITY IS DONE");
        DontDestroyOnLoad(this.gameObject);
    }

    /******************************** Generate public methods for usage ********************************/

    public void Initialize(string token, bool testMode)
    {
        Debug.Log("Unity3D Reward: Start Initializing In Java (" + TaplighJavaSrc + ")");

        #if !UNITY_EDITOR && UNITY_ANDROID
            _taplighJavaInterface = new AndroidJavaClass(TaplighJavaSrc);
            _taplighJavaInterface.CallStatic("initialize", token, testMode);
            Debug.Log("Tapligh Initilizing is Done");
        #endif
    }

    public void LoadAd(string unitCode)
    {
        Debug.Log("Unity3D Reward: Load Reward Ad");
        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null){
                _taplighJavaInterface.CallStatic("loadAd", this.gameObject.name, "onAdReady", "onLoadError", unitCode);
            }else{
                Debug.Log(" Tapligh Object in Unity is Null");
            }
        #endif
    }

    public void ShowAd(string unitCode)
    {
        Debug.Log("Unity3D Reward: Show Reward Ad");
        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null) {
                _taplighJavaInterface.CallStatic("showAd", this.gameObject.name, "onAdResult", "onRewardReady", unitCode);
            } else { 
                Debug.Log("Tapligh Object in Unity is Null");
            }
        #endif
    }

    public string GetTaplighVersion()
    {
        Debug.Log("Unity3D Reward: Get SDK Version");
        string taplighVersion = "";
        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null){
                taplighVersion = _taplighJavaInterface.CallStatic<string>("getTaplighVersion");
            }else{ 
                Debug.Log("Tapligh Object in Unity is Null"); 
            }
        #endif
        return taplighVersion; 
    }

    public void VerifyToken(string token)
    {
        Debug.Log("Unity3D Reward: Start Verify Token");
        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null) 
               _taplighJavaInterface.CallStatic("verifyToken", this.gameObject.name, "OnTokenVerifyJavaListener", token);
            else 
                Debug.Log("Tapligh Object in Unity is Null");
        #endif
    }

    public bool IsInitializeDone()
    {
        Debug.Log("Unity3D Reward: Check Is Initialize Done?");
        bool IsInitialize = false;
        #if !UNITY_EDITOR && UNITY_ANDROID
            if(_taplighJavaInterface != null)
                 IsInitialize = _taplighJavaInterface.CallStatic<bool>("isInitializeDone");
            else
                Debug.Log("Tapligh Object in Unity is Null");
        #endif

        return IsInitialize;
    }

    /******************************** Generate private methods for functionality ********************************/

    private void onAdReady(string response)
    {
        Debug.Log("Unity3D Reward: On Ad Ready [" + response + "]");
        List<string> result = GetResultArguments(response);
        String unit = result[0];
        String token = result[1];
        if (_onLoadReadyListener != null) { _onLoadReadyListener(unit, token); }
    }

    private void onLoadError(string response)
    {
        Debug.Log("Unity3D Reward: On Load Error [" + response + "]");
        List<string> result = GetResultArguments(response);
        String unit = result[0];
        String number = result[1];
        LoadErrorStatus res_num = (LoadErrorStatus)(Int32.Parse(number));
        if (_onLoadErrorListener != null) { _onLoadErrorListener(unit, res_num); }
    }

    private void onAdResult(string response)
    {
        Debug.Log("Unity3D Reward: On Ad Result [" + response + "]");
        List<string> info = GetResultArguments(response);
        AdResult result = (AdResult)(Int32.Parse(info[0]));
        if (_onAdListener != null) { _onAdListener(result, info[1]); }
    }

    private void onRewardReady(string reward)
    {
        Debug.Log("Unity3D Reward: On Reward Ready [" + reward + "]");
        if (_onRewardReadyListener != null) { _onRewardReadyListener(reward); }
    }

    private void OnTokenVerifyJavaListener(string response)
    {
        Debug.Log("Unity3D Reward: On Token Verify [" + response + "]");
        TokenResult result = (TokenResult)(Int32.Parse(response));
        if (_onTokenVerifyFinishedListener != null) { _onTokenVerifyFinishedListener(result); }
    }

    private List<string> GetResultArguments(string result)
    {
        List<string> arguments = new List<string>();
        int deviderIndex = result.IndexOf(';');
        arguments.Add( result.Substring( 0 , deviderIndex ) );
        for (; deviderIndex < result.Length ; deviderIndex++) { if (result[deviderIndex] != ';') { break; } }
        arguments.Add( result.Substring( deviderIndex)); 
        return arguments; 
    }

}