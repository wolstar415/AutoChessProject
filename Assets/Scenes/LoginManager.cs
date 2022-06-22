using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro;
using Firebase;
using GameS;
using Photon.Pun;
using UnityEngine.AddressableAssets;
using RobinBird.FirebaseTools.Storage.Addressables;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoginManager : MonoBehaviour
{
    [SerializeField] TMP_InputField IdField;
    [SerializeField] TMP_InputField passField;


    [SerializeField] TMP_InputField CreateField;
    [SerializeField] TMP_InputField Cre_passField;
    [SerializeField] TMP_InputField Cre_passField2;
    [SerializeField] TextMeshProUGUI Cre_Text;
    [SerializeField] TextMeshProUGUI PathText;
    [SerializeField] AudioSource audios;

    public GameObject LobyOb;
    [SerializeField] GameObject CreateOb;
    public GameObject PathOb;
    [SerializeField] GameObject Loby2Ob;

    public Button LoginButton;
    public Button CreateButton;

    private Coroutine taskcheck1;
    private Coroutine taskcheck2;
    public Transform canvasParent;
    Firebase.Auth.FirebaseAuth auth;
    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;
    AsyncOperationHandle handle;
    bool LoginSu = false;
    bool CreatSu = false;
    public AssetReferenceT<AudioClip> arf;
    [SerializeField] private GameObject waitob;
    private Coroutine corcheck1;
    public AssetReferenceT<GameObject> developOb;
    public LobyUiManager loby;

    EventSystem system;
    public Selectable firstInput;
    public Selectable firstInput2;
    public Selectable firstInput3;
    public Button submitButton;
    public Button submitButton2;

    private void Start()
    {
        // FirebaseStorage storageInstance = FirebaseStorage.DefaultInstance;
        system = EventSystem.current;
        // 처음은 이메일 Input Field를 선택하도록 한다.
        firstInput.Select();

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(x =>
        {
            if (x.Result == DependencyStatus.Available)
            {

                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;



            }
        });
        //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageAssetBundleProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageJsonAssetProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageHashProvider());

        // This requires Addressables >=1.75 and can be commented out for lower versions
        Addressables.InternalIdTransformFunc += FirebaseAddressablesCache.IdTransformFunc;
    }

    public void LoginBtn()
    {
        if (IdField.text == "" || passField.text == "")
        {
            return;
        }

        if (taskcheck1!=null) StopCoroutine(taskcheck1);
        taskcheck1=StartCoroutine(LoginCheck());
        firebaseAuth.SignInWithEmailAndPasswordAsync(IdField.text, passField.text).ContinueWith(x =>
        {
            if (!x.IsCanceled && !x.IsFaulted && x.IsCompleted)
            {
                LoginSu = true;
            }
            else
            {
                Debug.Log("실패");
                StopCoroutine(taskcheck1);
                taskcheck1 = null;
            }
        });
    }

    IEnumerator LoginCheck()
    {
        yield return new WaitUntil(() => LoginSu == true);
        PhotonNetwork.LocalPlayer.NickName = IdField.text;
        GameManager.inst.OriNickName = IdField.text;
        string[] s = GameManager.inst.OriNickName.Split('@');
        GameManager.inst.NickName = s[0];
        LobyOb.SetActive(false);
        DataManager.inst.StartFunc();
        yield return YieldInstructionCache.WaitForSeconds(1);
        PathBtn();
        //PathOb.SetActive(true);
    }
    
    IEnumerator CreateCheck()
    {
        CreatSu = false;
        yield return new WaitUntil(() => CreatSu == true);
        IdField.text = CreateField.text;
        LobyOb.SetActive(true);
        CreateOb.SetActive(false);
        firstInput3.Select();
    }

    public void CreateBtn()
    {
        firstInput2.Select();
        Cre_Text.text = "아이디는 이메일형식으로\n 비밀번호는 6자리 이상으로 해주세요.";
    }

    public void IdCreateBtn()
    {
        if (CreateField.text == "" || Cre_passField2.text == "" || Cre_passField.text == "")
        {
            return;
        }
        if (taskcheck2!=null) StopCoroutine(taskcheck2);
        taskcheck2=StartCoroutine(CreateCheck());
        if (Cre_passField.text != Cre_passField2.text)
        {
            //Debug.Log("비번이 달라");
            Cre_Text.text = "비밀번호가 다릅니다";
            return;
        }

        firebaseAuth.CreateUserWithEmailAndPasswordAsync(CreateField.text, Cre_passField.text).ContinueWith(x =>
        {
            if (!x.IsCanceled && !x.IsFaulted)
            {

                CreatSu = true;

            }
            else
            {
                //Debug.Log("실패");
                StopCoroutine(taskcheck2);
                taskcheck2 = null;
                //Cre_Text.text = "아이디는 이메일형식으로\n 비밀번호는 6자리 이상으로 해주세요.";
            }
        });
    }



    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            // Tab + LeftShift는 위의 Selectable 객체를 선택
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Tab은 아래의 Selectable 객체를 선택
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {

            if (LobyOb.activeSelf)
            {
                submitButton.onClick.Invoke();
            }
            else
            {
                submitButton2.onClick.Invoke();
            }
            
            
        }
    }

    public void PathBtn()
    {
        FirebaseAddressablesManager.IsFirebaseSetupFinished = true;


        Addressables.GetDownloadSizeAsync("GoGo").Completed +=
            (AsyncOperationHandle<long> SizeHandle) =>
            {
                
                if (SizeHandle.Result>0)
                {
                    waitob.SetActive(true);
                    corcheck1=StartCoroutine(IPathText());
                    //PathOb.SetActive(true);
                    handle = Addressables.DownloadDependenciesAsync("GoGo", true);
                    handle.Completed += (AsyncOperationHandle Obj) => {
                        //PathOb.SetActive(false);
                        
                        StopCoroutine(corcheck1);
                        handle = Obj;

                        Loaddevelop();

                        Addressables.Release(Obj);
                        Addressables.Release(handle);

                    };
                }
                else
                {


                    Loaddevelop();
                }
               
            
            };



        

        
        //StartCoroutine(IPathText());
    }

    void Loaddevelop()
    {
        waitob.SetActive(false);
        Loby2Ob.SetActive(true);
        developOb.LoadAssetAsync().Completed += (AsyncOperationHandle<GameObject> ad) =>
        {
            if (ad.Status == AsyncOperationStatus.Succeeded)
            {
                    
                Instantiate(ad.Result, canvasParent);
                loby.RankingSet();
            }
        };
            
        //Addressables.Release(developOb);
    }
    IEnumerator IPathText()
    {
        //yield return new WaitUntil(() => handle.Status == AsyncOperationStatus.Succeeded);
        
        while (true)
        {
            float f = handle.PercentComplete * 100;
            PathText.text = f.ToString("f2")+"%";
            
            yield return null;
        }
    }
}