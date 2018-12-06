using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

public class AnimationScript : MonoBehaviour
{

    public Animator animatorController;
    public string responseText = "";
    public int responseNumber;
    public int priority = 0;

    //Mqtt declarations
    private MqttClient client;
    private string brokerHostname = "139.162.46.222";
    private int brokerPort = 1883;
    private string userName = "";
    private string password = "";
    public string msg;

    //other declarations
    private bool triggerAnimation;
    private GameObject[] ZoneList;
    private GameObject[] peopleList;
    private GameObject[] drill;
    private GameObject[] bullDozer;
    private GameObject[] dumper;
    private GameObject[] van;

    private bool controleTrigger = false;
    
    object mobile_assets = new object();
    object values = new object();

    //private float speed = .1f;
    private const string URL = "http://blriip455956d:8080/animationData/getAnimationData";
    public int stepInitializer = 0;


    void Start()
    {

       

        ZoneList = GameObject.FindGameObjectsWithTag("Zone");
        animatorController = GetComponent<Animator>();
        client = new MqttClient(brokerHostname, brokerPort, false, null);

        // register to message received 
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId, userName, password);

        for (int i = 0; i < ZoneList.Length; i++)
        {
        
       
            if (ZoneList[i].ToString().Contains("Zone1"))
            {
                CalculateAsset("Zone1",i);
            }
            else if (ZoneList[i].ToString().Contains("Zone2"))
            {
                CalculateAsset("Zone2", i);
            }
            else if (ZoneList[i].ToString().Contains("Zone3"))
            {
                CalculateAsset("Zone3", i);
            }
            else if (ZoneList[i].ToString().Contains("Zone4"))
            {
                CalculateAsset("Zone4", i);
            }
            else if (ZoneList[i].ToString().Contains("Zone5"))
            {
                CalculateAsset("Zone5", i);
            }
            else if (ZoneList[i].ToString().Contains("Zone6"))
            {
                CalculateAsset("Zone6", i);

            }
        }
      

        client.Subscribe(new string[] { "zoneupdate" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        client.Subscribe(new string[] { "fallupdate" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        //client.Publish("stopeupdate", Encoding.UTF8.GetBytes(stoop), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);

    }

    public void Post()
    {

        Debug.Log("in post");
        //StartCoroutine(Upload());
        StartCoroutine(PostRequest(URL));

    }


    void CalculateAsset(string zoneName, int threshold) {
        List<object> mobileAssetsList = new List<object>();
        List<object> stoopDetails = new List<object>();

        int zoneid = Int32.Parse(Regex.Match(zoneName, @"\d+").Value);

        peopleList = GameObject.FindGameObjectsWithTag(zoneName+"Worker");
     
        drill = GameObject.FindGameObjectsWithTag(zoneName + "Drill");
        
        bullDozer = GameObject.FindGameObjectsWithTag(zoneName + "BullDozer");
      
        dumper = GameObject.FindGameObjectsWithTag(zoneName + "Dumper");
        van = GameObject.FindGameObjectsWithTag(zoneName + "Van");
     
        if (drill.Length != 0)
        {

            mobile_assets = new
            {
                mobile_asset_name = "Drill Rig",
                mobile_asset_number = drill.Length
            };
            mobileAssetsList.Add(mobile_assets);

        }
        if (bullDozer.Length != 0)
        {
            mobile_assets = new
            {
                mobile_asset_name = "Bulldozer",
                mobile_asset_number = bullDozer.Length
            };
            mobileAssetsList.Add(mobile_assets);

        }
        if (dumper.Length != 0)
        {
            mobile_assets = new
            {
                mobile_asset_name = "Dumper",
                mobile_asset_number = dumper.Length
            };
            mobileAssetsList.Add(mobile_assets);

        }
        if(van.Length != 0)
        {
            mobile_assets = new
            {
                mobile_asset_name = "Van",
                mobile_asset_number = van.Length
            };
            mobileAssetsList.Add(mobile_assets);
        }
        values = new
        {
            zone_id = "zone"+ zoneid.ToString(),
            stope_name = "stope" + zoneid.ToString(),
            number_of_people = peopleList.Length,
            temperature = 50 + threshold,
            humidity = 20 + (threshold * 2),
            is_geofenced = false,
            ready_to_geofence = true,
            mobile_assets = mobileAssetsList
        };

        string stoop = JsonConvert.SerializeObject(values);
        print(zoneid + " : " + stoop);
        client.Publish("stopeupdate", Encoding.UTF8.GetBytes(stoop), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,false);


    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        msg = Encoding.UTF8.GetString(e.Message);
        Debug.Log("in  publish receievd");
        Debug.Log("Received: " + Encoding.UTF8.GetString(e.Message));

    }
    private void Publish(string _topic, string msg)
    {
        
        client.Publish(_topic, Encoding.UTF8.GetBytes(msg),
            MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);


    }


    void Animate()
    {
        JSONNode jsonNode;
    
        if (msg != "")
        {
             jsonNode = SimpleJSON.JSON.Parse(msg);
        }
        else {
             jsonNode = null;
        }
        //print(jsonNode["topic"].ToString());
        string topic = "";
        string zone = "";
        if (jsonNode != null)
        {
            topic = jsonNode["topic"].ToString();
            
           
        }
         if (topic.Contains("zoneupdate"))
        {
            zone = jsonNode["zone"].ToString();
            if (zone.Contains("zone1"))
            {
                msg = "";   
                animatorController.SetTrigger("nonGeoFenceZone");
                animatorController.SetTrigger("resetTrigger");
            }
            if (zone.Contains("zone2"))
            {
                controleTrigger = true;
                msg = "";
                animatorController.SetTrigger("geoFenceZone");
                animatorController.SetTrigger("resetTrigger");
            }
            //animatorController.SetBool("ventcontroleZone5", true);
        }
       else if (topic.Contains("fallupdate"))
       {
            topic = "";
            msg = "";
           animatorController.SetTrigger("manFallTrigger");
           animatorController.SetTrigger("resetTrigger");
       }
        else
        {
            animatorController.SetTrigger("resetTrigger");
        }
        
      /*  else
        {
            animatorController.SetTrigger("trafficControleTrigger");
            if(priority == 0)
            {
                animatorController.SetInteger("trafficPriority",0);

            }
            if (priority == 1)
            {
                animatorController.SetInteger("trafficPriority", 1);

            }
            if (priority == 2)
            {
                animatorController.SetInteger("trafficPriority", 2);

            }
            if (priority == 3)
            {
                animatorController.SetInteger("trafficPriority", 3);

            }
        }*/

    }
    public void VentAnimation()
    {
        animatorController.SetTrigger("ventControlTrigger");
        animatorController.SetTrigger("resetTrigger");

    }
    public void Evacuation()
    {
        print("clicked evac");
        
        animatorController.SetTrigger("gasEvacuation");
        animatorController.SetTrigger("resetTrigger");
    }


    public void TriggerGeoFenceAlarm()
    {
        print("in trigger update");
        if (controleTrigger) {
            
            client.Publish("evacuate", Encoding.UTF8.GetBytes("evacuate"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
        }
        
    }

    public void ExitingGeoFenceTrigger()
    {
        client.Publish("evacuated", Encoding.UTF8.GetBytes("evacuated"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
    }
    IEnumerator PostRequest(string url)
    {
        object values = new
        {
            id = 1,
            animationnumber = 10,
            noofpeople = 1,
            noofasserts = 1
        };


        string str = JsonConvert.SerializeObject(values);
        print(values);


        //string postData = "{animationnumber:1,id:1}";
        var request = new UnityWebRequest(url, "PUT");


        // JSONNode jsonNode = SimpleJSON.JSON.Parse(s.ToString());

        byte[] bodyRaw = new UTF8Encoding().GetBytes(str);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        var time = Time.time;
        print("initial Time" + time);
        yield return request.SendWebRequest();
        if (request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Response: 1 " + request.downloadHandler.text);

            time = Time.time - time;
            print("final time " + Time.time);
            print(time);
        }

        //  Debug.Log("Response: " + request.downloadHandler.text);
    }


    // Update is called once per frame
    void Update()

    {
        if (true)
        {
            triggerAnimation = false;
            Animate();
        }   


    }
}




