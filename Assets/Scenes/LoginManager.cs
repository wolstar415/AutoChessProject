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
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoginManager : MonoBehaviour
{
    [SerializeField] TMP_InputField IdField;
    [SerializeField] TMP_InputField passField;


    [SerializeField] TMP_InputField CreateField;
    [SerializeField] TMP_InputField Cre_passField;
    [SerializeField] TMP_InputField Cre_passField2;
    [SerializeField] TextMeshProUGUI PathText;
    [SerializeField] AudioSource audios;

    public GameObject LobyOb;
    [SerializeField] GameObject CreateOb;
    public GameObject PathOb;
    [SerializeField] GameObject Loby2Ob;

    public Button LoginButton;
    public Button CreateButton;

    public Transform canvasParent;
    Firebase.Auth.FirebaseAuth auth;
    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;
    AsyncOperationHandle handle;
    bool LoginSu=false;
    public AssetReferenceT<AudioClip> arf;
    [SerializeField] private GameObject waitob;
    private Coroutine corcheck1;
    public AssetReferenceT<GameObject> developOb;
    public LobyUiManager loby;
    private  void Start()
    {
       // FirebaseStorage storageInstance = FirebaseStorage.DefaultInstance;
       
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(x =>
        {
            if (x.Result==DependencyStatus.Available)
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
        if (IdField.text ==""||passField.text=="")
        {
            return;
        }
        StartCoroutine(LoginCheck());
        firebaseAuth.SignInWithEmailAndPasswordAsync(IdField.text, passField.text).ContinueWith(x =>
        {
            if (!x.IsCanceled && !x.IsFaulted && x.IsCompleted)
            {
                LoginSu = true;
            }
            else
            {
                Debug.Log("실패");
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
    public void IdCreateBtn()
    {
        if (CreateField.text == "" || Cre_passField2.text == "" || Cre_passField.text == "")
        {
            return;
        }
        if (Cre_passField.text!= Cre_passField2.text)
        {
            Debug.Log("비번이 달라");
            return;
        }
        firebaseAuth.CreateUserWithEmailAndPasswordAsync(CreateField.text, Cre_passField.text).ContinueWith(x =>
         {
             if (!x.IsCanceled && !x.IsFaulted)
             {
                 LobyOb.SetActive(true);
                 CreateOb.SetActive(false);
                 
             }
             else
             {
                 Debug.Log("실패");
             }
         });
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
                    PathOb.SetActive(true);
                    handle = Addressables.DownloadDependenciesAsync("GoGo", true);
                    corcheck1=StartCoroutine(IPathText());
                    handle.Completed += (AsyncOperationHandle Obj) => {
                        PathOb.SetActive(false);
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