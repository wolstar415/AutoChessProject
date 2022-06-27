using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;


namespace GameS
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager inst;

        public List<int> Rankingint=new List<int>();
        public void Awake()
        {
            inst = this;
        }

        private DatabaseReference databaseReference;
        void Start()
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void StartFunc()
        {
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(GameManager.inst.NickName)
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        
                    }
                    else if (task.IsCompleted)
                    {

                        DataSnapshot snapshot = task.Result;
                        if (snapshot.HasChildren==false)
                        {
                            databaseReference.Child("users").Child(GameManager.inst.NickName).Child("Victory1").SetValueAsync(0);
                            databaseReference.Child("users").Child(GameManager.inst.NickName).Child("Victory2").SetValueAsync(0);
                            databaseReference.Child("users").Child(GameManager.inst.NickName).Child("Victory3").SetValueAsync(0);
                            databaseReference.Child("users").Child(GameManager.inst.NickName).Child("Victory4").SetValueAsync(0);
                            databaseReference.Child("users").Child(GameManager.inst.NickName).Child("Victory5").SetValueAsync(0);
                            databaseReference.Child("users").Child(GameManager.inst.NickName).Child("Victory6").SetValueAsync(0);
                            databaseReference.Child("users").Child(GameManager.inst.NickName).Child("Victory7").SetValueAsync(0);
                            databaseReference.Child("users").Child(GameManager.inst.NickName).Child("Victory8").SetValueAsync(0);
                            databaseReference.Child("users").Child(GameManager.inst.NickName).Child("Score").SetValueAsync(1000);
                            databaseReference.Child("users").Child(GameManager.inst.NickName).Child("CharIdx").SetValueAsync(0);
                        }
                        else
                        {

                            GameManager.inst.Score = int.Parse(snapshot
                                .Child("Score").Value.ToString());
                            GameManager.inst.Victory1 = int.Parse(snapshot
                                .Child("Victory1").Value.ToString());
                            GameManager.inst.Victory2 = int.Parse(snapshot
                                .Child("Victory2").Value.ToString());
                            GameManager.inst.Victory3 = int.Parse(snapshot
                                .Child("Victory3").Value.ToString());
                            GameManager.inst.Victory4 = int.Parse(snapshot
                                .Child("Victory4").Value.ToString());
                            GameManager.inst.Victory5 = int.Parse(snapshot
                                .Child("Victory5").Value.ToString());
                            GameManager.inst.Victory6 = int.Parse(snapshot
                                .Child("Victory6").Value.ToString());
                            GameManager.inst.Victory7 = int.Parse(snapshot
                                .Child("Victory7").Value.ToString());
                            GameManager.inst.Victory8 = int.Parse(snapshot
                                .Child("Victory8").Value.ToString());
                            GameManager.inst.CharIdx = int.Parse(snapshot
                                .Child("CharIdx").Value.ToString());
                            
                            

                        }

                    }
                   
                });
            
            
        }

        public void Ranking()
        {
            int so = GameManager.inst.Score;
            Rankingint.Clear();
            FirebaseDatabase.DefaultInstance.GetReference("users")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        
                    }
                    else if (task.IsCompleted)
                    {

                        DataSnapshot snapshot = task.Result;

                        foreach (var data in snapshot.Children)
                        {
                            IDictionary rankInfo = (IDictionary)data.Value;
                            //int a = (int)rankInfo["Score"];
                            string s = rankInfo["Score"].ToString();
                            Rankingint.Add(int.Parse(s));
                        }

                        Rankingint.Sort();
                        Rankingint.Reverse();

                        LobyUiManager.inst.playerdata[10].text =(Rankingint.IndexOf(so)+1).ToString() + " / " + Rankingint.Count;
                        //LobyUiManager.inst.playerdata[10].text =0 + " / " + snapshot.Children.Count();


                    }
                   
                });
            
            
            
           
        }

        public void SaveData(string name, int value)
        {
            databaseReference.Child("users").Child(GameManager.inst.NickName).Child(name).SetValueAsync(value);
            
        }


    }
}
