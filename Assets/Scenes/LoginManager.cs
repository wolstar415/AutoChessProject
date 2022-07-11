using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro;
using Firebase;
using GameS;
using Google;
using Photon.Pun;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
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
    //Firebase.Auth.FirebaseAuth auth;
    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;
    AsyncOperationHandle handle;
    bool LoginSu = false;
    bool CreatSu = false;
    public AssetReferenceT<AudioClip> arf;
    [SerializeField] private GameObject waitob;
    private Coroutine corcheck1;
    public AssetReferenceT<GameObject> developOb;
    public AssetReferenceT<AudioClip> aszzd;
    public LobyUiManager loby;

    EventSystem system;
    public Selectable firstInput;
    public Selectable firstInput2;
    public Selectable firstInput3;
    public Button submitButton;
    public Button submitButton2;
    
    [Header("구글 로그인")]
    public string webClientId = "867301402469-m9r5l5a57muanogh0h76d54bce5ge80m.apps.googleusercontent.com";
 
    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
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
                CheckFirebaseDependencies();


            }
        });
        //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageAssetBundleProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageJsonAssetProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageHashProvider());

        // This requires Addressables >=1.75 and can be commented out for lower versions
        Addressables.InternalIdTransformFunc += FirebaseAddressablesCache.IdTransformFunc;
        
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        FirebaseAddressablesManager.IsFirebaseSetupFinished = true;
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
        
        //StartCoroutine(check1());
        //return;

          Addressables.GetDownloadSizeAsync("abc").Completed +=
            (AsyncOperationHandle<long> SizeHandle) =>
            {
                
                
                if (SizeHandle.Result>=0)
                {
                        waitob.SetActive(true);
                        PathText.text = SizeHandle.Result.ToString("f2")+"Byte";
                         Addressables.DownloadDependenciesAsync("abc").Completed+= (AsyncOperationHandle Handle) =>
                         {
                        waitob.SetActive(false);
                             Loaddevelop();
                         };

                    
                    
                }
                else
                {
                
                
                    Loaddevelop();
                }

            };



        

        
        //StartCoroutine(IPathText());
    }
    

    IEnumerator check1()
    {
        string key = "abc";
        //Clear all cached AssetBundles
        // WARNING: This will cause all asset bundles to be re-downloaded at startup every time and should not be used in a production game
        // Addressables.ClearDependencyCacheAsync(key);

        //Check the download size
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
        yield return getDownloadSize;
        
        //If the download size is greater than 0, download all the dependencies.
        if (getDownloadSize.Result >= 0)
        {
            
            waitob.SetActive(true);
            PathText.text=string.Concat(getDownloadSize.Result, " byte");
            AsyncOperationHandle downloadDependencies = Addressables.DownloadDependenciesAsync(key);
            yield return downloadDependencies;
            waitob.SetActive(false);
                Loaddevelop();

            
        }
        else
        {
            Loaddevelop();
        }
    }

    private IEnumerator check2()
    {
        Addressables.ClearDependencyCacheAsync("abc");
        Addressables.CleanBundleCache();
        Addressables.ClearResourceLocators();
        yield return Addressables.InitializeAsync();
        AsyncOperationHandle< long > getAddresablesDownloadSize= Addressables.GetDownloadSizeAsync( "abc" );
        yield return getAddresablesDownloadSize;

        var downloadSize = getAddresablesDownloadSize.Result ;
        Debug.Log( $"GetDownloadSizeAsync: {downloadSize}." );
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
                developOb.ReleaseAsset();
            }
        };
        
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




 
    
    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    auth = FirebaseAuth.DefaultInstance;
                }
            }
            else
            {
                Debug.Log("실패");
            }
        });
    }
    
    public void SignInWithGoogle() { OnSignIn(); }
    
    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }
    
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            // Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            // Debug.Log("Email = " + task.Result.Email);
            // Debug.Log("Google ID Token = " + task.Result.IdToken);
            // Debug.Log("Email = " + task.Result.Email);
            PhotonNetwork.LocalPlayer.NickName = task.Result.Email;
            GameManager.inst.OriNickName = task.Result.Email;
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }
    
    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
 
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    Debug.Log("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                //Debug.Log("Sign In Successful.");
                

                string[] s = GameManager.inst.OriNickName.Split('@');
                GameManager.inst.NickName = s[0];
                LobyOb.SetActive(false);
                DataManager.inst.StartFunc();
                Invoke("PathBtn",1.0f);
            }
        });
    }
    
    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        Debug.Log("Calling SignIn Silently");
 
        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }
}