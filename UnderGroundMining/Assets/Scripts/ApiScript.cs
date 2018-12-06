
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.IO;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class ApiScript : MonoBehaviour
{

    public static ApiScript Instance { set; get; }

    public InputField userName;
    public InputField password;
    public Text fText;
    public Text debugText;


    public string un;
    public string pwd;

   


    public Text userId;
    public static string userId01;

    public string sceneName = "ImageTarget";


    public GameObject dataPrefab;


    //public Button button1;

    //public int status;

    private const string URL = "http://ec2-52-65-167-54.ap-southeast-2.compute.amazonaws.com/api/auth/signin";
    //private const string URL  =  "http://10.219.78.134/api/auth/signin";





    // Use this for initialization
    void Start()
    {
        dataPrefab.SetActive(false);
        // gameObject.SetActive(false);

}

// Update is called once per frame
void Update()
    {
        // Validate();
        //dataPrefab.SetActive(false);


    }



    public void Validate()
    {

        un = userName.text.ToString();
        pwd = password.text.ToString();

        if ((userName.text == "") && (password.text == ""))
        {
            InvalidDetails();
        }
        else
        {
            string data = "{\"username\": \"" + un + "\", \"password\": \"" + pwd + "\"}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.ContentType = "application/json";
            print("after request");
            request.Method = "POST";
            print("after post");
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
                print("In Using" + streamWriter);
            }


            print("Before HttpWebResponse");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            print("After HttpWebResponse");

            StreamReader reader = new StreamReader(response.GetResponseStream());

            print("After StreamReader");

            var jsonResponse = reader.ReadToEnd();

            print("After jsonResponse");




            string JSONToParse = "{\"response\":" + jsonResponse + "}";
            JSONNode jsonNode = SimpleJSON.JSON.Parse(JSONToParse);
            userId.text = jsonNode["response"]["userId"].ToString();
            userId01 = jsonNode["response"]["userId"].ToString();





            int status = (int)response.StatusCode;
            if (status == 200)
            {

                print(status);
                print("success");
                dataPrefab.SetActive(true);
                //SceneManager.LoadScene("ImageTarget"); //Function for loading a Scene by passing the Scene name




            }
            else
            {
                InvalidDetails();
            }

        }
    }

    public void LogIn()
    {
        Validate();

    }

    public void InvalidDetails()
    {
        fText.text = "Invalid Deatils";
    }


}


