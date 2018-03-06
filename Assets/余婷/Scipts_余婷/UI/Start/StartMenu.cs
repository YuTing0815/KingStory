using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
//using UnityEngine.Audio;
public class StartMenu : MonoBehaviour
{
    //StartMenuPanel按钮 开始页面
    AudioSource audioSource;

    RectTransform StartPanel;
    Image gameTitle;
    RectTransform HelpPanel;
    Button btnStartLogin;
    Button btnExitGame;
    Button btnGameHelp;
    Button btnClose;
    Vector3 gameTitlePos;
    Vector3 HelpPanelPos;
    Vector3 btnStartLoginPos;
    Vector3 btnGameHelpPos;
    Vector3 btnExitGamePos;

    //登录界面 
    RectTransform LoginPanel;

    RectTransform LoginPanelBg;
    InputField inputLoginUserName;
    InputField inputLoginPassWord;
    Button btnLogin;
    Button btnRegisterShow;
    Button btnLoginClose;
    Vector3 LoginPanelBgPos;

    //注册界面
    RectTransform RegisterPanel;

    RectTransform RegisterPanelBg;
    InputField inputRegisterUserName;
    InputField inputRegisterPassWord;
    InputField inputRegisterConfirmPassWord;
    Button btnRegisterAndLogin;
    Button btnCancel;
    Button btnRegisterClose;
    Vector3 RegisterPanelBgPos;

    //GameRecordPanel按钮  记录界面
    RectTransform GameRecord;

    Button map1;
    Button map2;
    Button map3;
    Button map4;

    Button returnStartMenuPanelBtn;

    // Use this for initialization
    void Awake()
    {       
        Screen.SetResolution(480, 800, false);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        //StartMenuPanel按钮初始化
        StartPanel = gameObject.FindComponent<RectTransform>("StartFace");
        gameTitle = StartPanel.gameObject.FindComponent<Image>("gametitle");
        btnStartLogin = StartPanel.gameObject.FindComponent<Button>("btnLogin");
        btnGameHelp = StartPanel.gameObject.FindComponent<Button>("btnGameHelp");
        btnExitGame = StartPanel.gameObject.FindComponent<Button>("btnExitGame");
        HelpPanel = StartPanel.gameObject.FindComponent<RectTransform>("HelpPanel");
        btnClose = HelpPanel.gameObject.FindComponent<Button>("btnClose");
        //记录带有动画的物体位置
        gameTitlePos = gameTitle.transform.position;
        HelpPanelPos = HelpPanel.transform.localScale;
        btnStartLoginPos = btnStartLogin.transform.position;
        btnGameHelpPos = btnGameHelp.transform.localScale;
        btnExitGamePos = btnExitGame.transform.localScale;
        //播放动画
        StartCoroutine(ComeStartAni());
        //响应按钮事件
        btnStartLogin.onClick.AddListener(OnBtnStartLoginClick);
        btnExitGame.onClick.AddListener(OnBtnExitGameClick);
        btnGameHelp.onClick.AddListener(OnBtnGameHelpClick);
        btnClose.onClick.AddListener(OnBtnStartHelpPanelClose);

        //LoginPanel UI初始化
        LoginPanel = gameObject.FindComponent<RectTransform>("LoginPanel");
        LoginPanelBg = LoginPanel.gameObject.FindComponent<RectTransform>("bg/loginBg");
        inputLoginUserName = LoginPanel.gameObject.FindComponent<InputField>("bg/loginBg/InputFieldUser");
        inputLoginPassWord = LoginPanel.gameObject.FindComponent<InputField>("bg/loginBg/InputFieldPwd");
        btnLogin = LoginPanel.gameObject.FindComponent<Button>("bg/loginBg/btnLogin");
        btnRegisterShow = LoginPanel.gameObject.FindComponent<Button>("bg/loginBg/btnRegisterShow");
        btnLoginClose = LoginPanel.gameObject.FindComponent<Button>("bg/loginBg/btnReturn");
        //记录带有动画的物体位置
        LoginPanelBgPos = LoginPanelBg.transform.localScale;
        //响应按钮事件
        btnLogin.onClick.AddListener(OnBtnLoginClick);
        btnRegisterShow.onClick.AddListener(OnRegisterShowClick);
        btnLoginClose.onClick.AddListener(OnBtnLoginReturnClick);




        //RegisterPanel UI初始化
        RegisterPanel = gameObject.FindComponent<RectTransform>("RegisterPanel");
        RegisterPanelBg = RegisterPanel.gameObject.FindComponent<RectTransform>("bg/registerBg");
        inputRegisterPassWord = RegisterPanel.gameObject.FindComponent<InputField>("bg/registerBg/InputFieldUser");
        inputRegisterUserName = RegisterPanel.gameObject.FindComponent<InputField>("bg/registerBg/InputFieldPwd");
        inputRegisterConfirmPassWord =
            RegisterPanel.gameObject.FindComponent<InputField>("bg/registerBg/InputconfirmFieldPwd");
        btnRegisterAndLogin = RegisterPanel.gameObject.FindComponent<Button>("bg/registerBg/btnRegisterAndLogin");
        btnCancel = RegisterPanel.gameObject.FindComponent<Button>("bg/registerBg/btnCancel");
        btnRegisterClose = RegisterPanel.gameObject.FindComponent<Button>("bg/registerBg/btnReturn");
        //记录带有动画的物体位置
        RegisterPanelBgPos = RegisterPanelBg.transform.localScale;
        //按钮事件响应
        btnRegisterAndLogin.onClick.AddListener(OnBtnRegisterAndLogin);
        btnRegisterClose.onClick.AddListener(OnBtnRegisterClose);
        btnCancel.onClick.AddListener(OnBtnRegisterCancle);


        //GameRecordPanel按钮初始化
        GameRecord = gameObject.FindComponent<RectTransform>("GameRecord");
        map1 = GameRecord.gameObject.FindComponent<Button>("Map1/map1");
        map2 = GameRecord.gameObject.FindComponent<Button>("Map2/map2");
        map3 = GameRecord.gameObject.FindComponent<Button>("Map3/map3");
        map4 = GameRecord.gameObject.FindComponent<Button>("Map4/map4");
        returnStartMenuPanelBtn = GameRecord.gameObject.FindComponent<Button>("ReturnStartMenu");
        //响应按钮事件
        map1.onClick.AddListener(OnMap1Click);
        map2.onClick.AddListener(OnMap2Click);
        map3.onClick.AddListener(OnMap3Click);
        map4.onClick.AddListener(OnMap4Click);
        returnStartMenuPanelBtn.onClick.AddListener(OnReturnStartMenuClick);

        if (SaveIndexMgr.Instance.SaveSceneId != 0)
        {
            StartPanel.gameObject.SetActive(false);
            LoginPanel.gameObject.SetActive(false);
            RegisterPanel.gameObject.SetActive(false);
            GameRecord.gameObject.SetActive(true);
        }
    }

    //注册界面 取消注册按钮
    private void OnBtnRegisterCancle()
    {
        LoginPanel.gameObject.SetActive(true);
        StartCoroutine(SetActiveObj(RegisterPanel.gameObject));
        StartCoroutine(ResetRegisterAni());
        StartCoroutine(ComeLoginAni());
    }

    //注册界面 关闭注册面板
    private void OnBtnRegisterClose()
    {
        LoginPanel.gameObject.SetActive(true);
        StartCoroutine(SetActiveObj(RegisterPanel.gameObject));
        StartCoroutine(ResetRegisterAni());
        StartCoroutine(ComeLoginAni());
    }

    //注册界面 注册并登录按钮
    private void OnBtnRegisterAndLogin()
    {
        GameRecord.gameObject.SetActive(true);
        StartCoroutine(SetActiveObj(RegisterPanel.gameObject));
        StartCoroutine(ResetRegisterAni());
    }

    //登录面板  按钮点击实现到注册界面
    private void OnRegisterShowClick()
    {
        btnRegisterShow.BtnAudioPlay();
        RegisterPanel.gameObject.SetActive(true);
        StartCoroutine(SetActiveObj(LoginPanel.gameObject));
        StartCoroutine(ResetLoginAni());
        StartCoroutine(ComeRegisterAni());
    }

    //登录面板 返回首页按钮实现
    private void OnBtnLoginReturnClick()
    {
        btnLoginClose.BtnAudioPlay();
        StartPanel.gameObject.SetActive(true);
        StartCoroutine(SetActiveObj(LoginPanel.gameObject));
        StartCoroutine(ComeStartAni());
        StartCoroutine(ResetLoginAni());

    }

    //登录界面按钮  点击登录按钮实现
    private void OnBtnLoginClick()
    {
        btnLogin.BtnAudioPlay();
        //服务器验证  TODO
        GameRecord.gameObject.SetActive(true);
        StartCoroutine(SetActiveObj(LoginPanel.gameObject));
        StartCoroutine(ResetLoginAni());
    }

    //关卡面板中   返回首页按钮实现
    private void OnReturnStartMenuClick()
    {
        returnStartMenuPanelBtn.BtnAudioPlay();
        StartPanel.gameObject.SetActive(true);
        StartCoroutine(SetActiveObj(GameRecord.gameObject));
        StartCoroutine(ComeStartAni());
    }

    //关卡面板中 地图3按钮 方法实现
    private void OnMap3Click()
    {
        SceneManager.LoadScene("Scenes/MapThree");
    }
    //关卡面板中 地图4按钮 方法实现
    private void OnMap4Click()
    {
        SceneManager.LoadScene("Scenes/MapFour");
    }
    //关卡面板中 地图2按钮 方法实现
    private void OnMap2Click()
    {
        SceneManager.LoadScene("Scenes/MapTwo");
    }

    //关卡面板中 地图1按钮 方法实现
    private void OnMap1Click()
    {
        SceneManager.LoadScene("Scenes/MapOne");
    }

    //首页中  帮助按钮点击实现
    private void OnBtnGameHelpClick()
    {
        btnGameHelp.BtnAudioPlay();
        HelpPanel.gameObject.SetActive(true);
        HelpPanelTween();
    }

    //首页中退出按钮  方法实现
    private void OnBtnExitGameClick()
    {
        btnExitGame.BtnAudioPlay();
        Application.Quit();
    }

    //首页中 登录按钮点击实现到  登录面板
    private void OnBtnStartLoginClick()
    {
        btnStartLogin.BtnAudioPlay();
        LoginPanel.gameObject.SetActive(true);
        StartCoroutine(SetActiveObj(StartPanel.gameObject));
        StartCoroutine(ResetStartAni());
        StartCoroutine(ComeLoginAni());
    }

    //首页中帮助关闭按钮
    private void OnBtnStartHelpPanelClose()
    {
        btnGameHelp.BtnAudioPlay();
        StartCoroutine(SetActiveObj(HelpPanel.gameObject));
        HelpPanelBackTween();
    }

    //协程 延迟将面板失活
    IEnumerator SetActiveObj(GameObject obj)
    {
        yield return new WaitForSeconds(0.01f);
        obj.SetActive(false);
    }

    //开始界面的动画正向播放控制
    IEnumerator ComeStartAni()
    {
        yield return new WaitForSeconds(0.01f);
        StartCome();
    }

    //开始界面的动画反向播放控制
    IEnumerator ResetStartAni()
    {
        yield return new WaitForSeconds(0.01f);
        StartReset();
    }

    //开始界面动画播放
    private void StartCome()
    {
        Tweening.Instance.ComeDoTweenAnimationDOLocalMove(gameTitle.gameObject, new Vector3(0, 359, 0));
        Tweening.Instance.ComeDoTweenAnimationDOLocalMove(btnStartLogin.gameObject, new Vector3(0, -500, 0));
        Tweening.Instance.ComeDoTweenAnimationDoScale(btnGameHelp.gameObject, new Vector3(1, 1, 1));
        Tweening.Instance.ComeDoTweenAnimationDoScale(btnExitGame.gameObject, new Vector3(1, 1, 1));
    }

    //开始界面动画反向播放
    private void StartReset()
    {
        Tweening.Instance.BackDoTweenAnimationDOLocalMove(gameTitle.gameObject, gameTitlePos);
        Tweening.Instance.BackDoTweenAnimationDOLocalMove(btnStartLogin.gameObject, btnStartLoginPos);
        Tweening.Instance.BackDoTweenAnimationDoScale(btnGameHelp.gameObject, btnGameHelpPos);
        Tweening.Instance.BackDoTweenAnimationDoScale(btnExitGame.gameObject, btnExitGamePos);
    }

    public void HelpPanelTween()
    {
        Tweening.Instance.ComeDoTweenAnimationDoScale(HelpPanel.gameObject, new Vector3(1, 1, 1));
    }

    public void HelpPanelBackTween()
    {
        Tweening.Instance.BackDoTweenAnimationDoScale(HelpPanel.gameObject, HelpPanelPos);
    }

    //登录界面的动画正向播放控制
    IEnumerator ComeLoginAni()
    {
        yield return new WaitForSeconds(0.01f);
        LoginCome();
    }

    //登录界面的动画反向播放控制
    IEnumerator ResetLoginAni()
    {
        yield return new WaitForSeconds(0.01f);
        LoginReset();
    }

    //登录界面动画播放
    private void LoginCome()
    {
        Tweening.Instance.ComeDoTweenAnimationDoScale(LoginPanelBg.gameObject, new Vector3(1, 1, 1));
    }

    //登录界面动画反向播放
    private void LoginReset()
    {
        Tweening.Instance.BackDoTweenAnimationDoScale(LoginPanelBg.gameObject, LoginPanelBgPos);
    }

    IEnumerator ComeRegisterAni()
    {
        yield return new WaitForSeconds(0.01f);
        RegisterCome();
    }

    //登录界面的动画反向播放控制
    IEnumerator ResetRegisterAni()
    {
        yield return new WaitForSeconds(0.01f);
        RegisterReset();
    }

    private void RegisterCome()
    {
        Tweening.Instance.ComeDoTweenAnimationDoScale(RegisterPanelBg.gameObject, new Vector3(1, 1, 1));
    }

    private void RegisterReset()
    {
        Tweening.Instance.BackDoTweenAnimationDoScale(RegisterPanelBg.gameObject, RegisterPanelBgPos);
    }
}
