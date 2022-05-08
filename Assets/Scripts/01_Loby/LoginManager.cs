using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using TMPro;
using Firebase;

public class LoginManager : MonoBehaviour
{
    [SerializeField] TMP_InputField IdField;
    [SerializeField] TMP_InputField passField;


    [SerializeField] TMP_InputField CreateField;
    [SerializeField] TMP_InputField Cre_passField;
    [SerializeField] TMP_InputField Cre_passField2;

    [SerializeField] GameObject LobyOb;
    [SerializeField] GameObject CreateOb;

    public Button LoginButton;
    public Button CreateButton;

    Firebase.Auth.FirebaseAuth auth;
    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;

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
        
    }

    public void LoginBtn()
    {
        if (IdField.text ==""||passField.text=="")
        {
            return;
        }
        firebaseAuth.SignInWithEmailAndPasswordAsync(IdField.text, passField.text).ContinueWith(x => {
            if (!x.IsCanceled && !x.IsFaulted && x.IsCompleted)
            {
                Debug.Log("성공");

            }
            else
            {
                Debug.Log("실패");
            }
        });
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
}
