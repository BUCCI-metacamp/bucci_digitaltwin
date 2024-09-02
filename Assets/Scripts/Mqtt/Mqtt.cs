using Best.HTTP.Shared;
using Best.MQTT;
using Best.MQTT.Packets;
using Best.MQTT.Packets.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Mqtt : MonoBehaviour
{
    MQTTClient client;

    public string host;
    public int port;
    public string[] subscriptionTopics;
    //public string controllTopic = "edge/edukit/control";

    public void Start()
    {
        //GetMqttSetting();
        GameObject.Find("MQTT").GetComponent<ControllMessage>().onControllMessage += PublishMessage;

    }

    public event Action portParsingError;
    public void Connect(string ip, string port, string topics)
    {
        //Debug.Log($"1MQTT CONNECTING... {ip}:{port} {topics}");
        host = ip;
        if (!int.TryParse(port, out int pd))
        {
            portParsingError?.Invoke();
            return;
        }
        this.port = pd;
        subscriptionTopics = topics.Split(',');
        //Debug.Log($"2MQTT CONNECTING... {host}:{this.port} {subscriptionTopics.Length}");
        Connect();
    }


    void Disconnect()
    {
        client?.CreateDisconnectPacketBuilder()
        .WithReasonCode(DisconnectReasonCodes.NormalDisconnection)
        //.WithReasonString("Bye")
        .BeginDisconnect();
    }
    public void Connect()
    {
        //Disconnect();
#if !UNITY_EDITOR
        HTTPManager.RootFolderName = "EdukitDT";
#endif
        client = new MQTTClientBuilder()
                    //#if !UNITY_WEBGL || UNITY_EDITOR
                    .WithOptions(new ConnectionOptionsBuilder().WithTCP(host, port))
                    //#else
                    //.WithOptions(new ConnectionOptionsBuilder().WithWebSocket(host, port).WithTLS())
                    //#endif
                    .WithEventHandler(OnConnected)
                    .WithEventHandler(OnStateChange)
                    .WithEventHandler(OnDisconnected)
                    .WithEventHandler(OnError)
                    .CreateClient();
        client.BeginConnect(ConnectPacketBuilderCallback);
    }

    public event Action<MQTTClient> onConnectedEvent;
    private void OnConnected(MQTTClient client)
    {
        var edukit = FindObjectOfType<Edukit.Edukit>();
        for (int i = 0; i < subscriptionTopics.Length; i++)
        {
            SubscriptionTopic(subscriptionTopics[i], edukit.ReceiveData);
            client.AddTopicAlias(subscriptionTopics[i]);

            client.CreateSubscriptionBuilder(subscriptionTopics[i])
                      .WithMessageCallback(OnMessage)
                      .WithAcknowledgementCallback(OnSubscriptionAcknowledged)
                      .WithMaximumQoS(QoSLevels.ExactlyOnceDelivery)
                      .BeginSubscribe();
        }
        onConnectedEvent?.Invoke(client);
        /*client.createapplicationmessagebuilder(controllTopics)
                .withpayload(ControllMessage)
                .withqos(best.mqtt.packets.qoslevels.exactlyoncedelivery)
                .withcontenttype("text/plain; charset=utf-8")
                .beginpublish();*/
    }
    
    Dictionary<string, Action<List<JsonData>>> topicSubscriptionTable = new();
    private void OnMessage(MQTTClient client, SubscriptionTopic topic, string topicName, ApplicationMessage message)
    {
        // Convert the raw payload to a string
        var payload = Encoding.UTF8.GetString(message.Payload.Data, message.Payload.Offset, message.Payload.Count);
        //Debug.Log($"Content-Type: '{message.ContentType}' Payload: '{payload}'");

        List<JsonData> data = JsonConvert.DeserializeObject<List<JsonData>>(payload);
        topicSubscriptionTable[topicName]?.Invoke(data);
    }

    public void SubscriptionTopic(string topic, Action<List<JsonData>> callback)
    {
        if (!topicSubscriptionTable.ContainsKey(topic))
            topicSubscriptionTable.Add(topic, null);
        topicSubscriptionTable[topic] += callback;
    }

    private void OnSubscriptionAcknowledged(MQTTClient client, SubscriptionTopic topic, SubscribeAckReasonCodes reasonCode)
    {
    //    if (reasonCode <= SubscribeAckReasonCodes.GrantedQoS2)
    //        Debug.Log($"Successfully subscribed with topic filter '{topic.Filter.OriginalFilter}'. QoS granted by the server: {reasonCode}");
    //    else
    //        Debug.Log($"Could not subscribe with topic filter '{topic.Filter.OriginalFilter}'! Error code: {reasonCode}");
    }


    void OnDestroy()
    {
        //Debug.Log("MQTT DESTROY");
        client?.CreateDisconnectPacketBuilder()
            .WithReasonCode(DisconnectReasonCodes.NormalDisconnection)
            //.WithReasonString("Bye")
            .BeginDisconnect();
    }
    private ConnectPacketBuilder ConnectPacketBuilderCallback(MQTTClient client, ConnectPacketBuilder builder)
    {
        return builder;
    }

    public event Action<MQTTClient, string> onErrorEvent;
    private void OnError(MQTTClient client, string error)
    {
        //Debug.Log($"OnError! :{error}");
        onErrorEvent?.Invoke(client, error);
        //throw new NotImplementedException();
    }

    public event Action<MQTTClient, DisconnectReasonCodes, string> onDisconnectedEvent;
    private void OnDisconnected(MQTTClient client, DisconnectReasonCodes reasonCode, string reasonMessage)
    {
        if (client == null)
        {
            Debug.Log("Client Missing");
            return;
        }
        onDisconnectedEvent?.Invoke(client, reasonCode, reasonMessage);
    }

    public event Action<MQTTClient, ClientStates, ClientStates> onStateChangeEvent;
    private void OnStateChange(MQTTClient client, ClientStates oldState, ClientStates newState)
    {
        onStateChangeEvent?.Invoke(client, oldState, newState);
        //throw new NotImplementedException();
    }
    
    public void PublishMessage(string message)
    {
        string controllTopic = "edge/edukit/control";
        byte[] payload = Encoding.UTF8.GetBytes(message);
        client.CreateApplicationMessageBuilder(controllTopic)
            .WithQoS(QoSLevels.AtMostOnceDelivery)
            .WithRetain(false)
            .WithPayload(message)
            .BeginPublish();
        
        Debug.Log(message);
    }
}