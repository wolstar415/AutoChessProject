using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
namespace GameS
{
    public class datatest : MonoBehaviour
    {
        private DatabaseReference databaseReference;
        
        void Start()
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            //
            //return;
            
        }

        public void f1()
        {
            
        }

        public void f2()
        {
            
        }
    }
}
