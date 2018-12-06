using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

public class mqttTest : MonoBehaviour {
	private MqttClient client;
    public string brokerHostname = "m15.cloudmqtt.com";
   // public string brokerHostname = "ec2-11-111-11.us-west-2.compute.amazonaws.com";
    public int brokerPort = 12942;
    public string userName = "betaaqcu";
    public string password = "JYHFUFtbsmqj";
    // Use this for initialization
    void Start () {
        // create client instance 
        //client = new MqttClient(IPAddress.Parse("m15.cloudmqtt.com"),8080 , false , null ); 
        client = new MqttClient(brokerHostname, brokerPort, false, null);

        // register to message received 
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
		
		string clientId = Guid.NewGuid().ToString(); 
		client.Connect(clientId,userName,password); 
		
		// subscribe to the topic "/home/temperature" with QoS 2 
		client.Subscribe(new string[] { "hello/world" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }); 
        

	}
	void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{
        string msg =Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received: " + Encoding.UTF8.GetString(e.Message)  );
	} 

	void OnGUI(){
		if ( GUI.Button (new Rect (20,40,80,20), "Level 1")) {
			Debug.Log("sending...");
			client.Publish("hello/world", Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
			Debug.Log("sent");
		}
	}

    private void Publish(string _topic, string msg)
    {
        client.Publish(_topic, Encoding.UTF8.GetBytes(msg),
            MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);


    }
	// Update is called once per frame
	void Update () {



	}
}
