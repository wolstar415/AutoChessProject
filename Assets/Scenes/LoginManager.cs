using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro;
using Firebase;
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

    Firebase.Auth.FirebaseAuth auth;
    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;
    AsyncOperationHandle handle;
    bool LoginSu=false;
    public AssetReferenceT<AudioClip> arf;
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
        LobyOb.SetActive(false);
        PathOb.SetActive(true);
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

             Debug.Log("성공");
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







        handle = Addressables.DownloadDependenciesAsync("GoGo", true);
        
        handle.Completed += (AsyncOperationHandle Obj) => {
            handle = Obj;
            PathText.text = "100%";
            Loby2Ob.SetActive(true);
            PathOb.SetActive(false);
            StopCoroutine(IPathText());
            //arf.LoadAssetAsync().Completed += (AsyncOperationHandle<AudioClip> mu) => { };
            music();
            Addressables.Release(Obj);
            Addressables.Release(handle);

        };
        StartCoroutine(IPathText());
    }
    void music()
    {
        arf.LoadAssetAsync().Completed += (AsyncOperationHandle<AudioClip> mu) => {
            if (mu.Status == AsyncOperationStatus.Succeeded)
            {
                AudioClip clip = mu.Result;
                Debug.Log("확인");
                audios.clip = clip;
                audios.Play();
            }
        };
    }
    IEnumerator IPathText()
    {
        //yield return new WaitUntil(() => handle.Status == AsyncOperationStatus.Succeeded);
        
        while (true)
        {
            PathText.text = string.Concat((float)handle.PercentComplete * 100, "%");

            Debug.Log("다운 로드 상황 : " + (float)handle.PercentComplete);
            yield return null;
        }
    }
}