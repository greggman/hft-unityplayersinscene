using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using HappyFunTimes;

namespace HappyFunTimesExample {

class ExampleSimplePlayer : MonoBehaviour {
    private class MessageColor : MessageCmdData {
        public MessageColor(Color _color) {
            color = _color;
        }
        public Color color;
    };

    private class MessageMove : MessageCmdData {
        public float x = 0;
        public float y = 0;
    };

    private class MessageSetName : MessageCmdData {
        public MessageSetName() {  // needed for deserialization
        }
        public MessageSetName(string _name) {
            name = _name;
        }
        public string name = "";
    };

    private class MessageBusy : MessageCmdData {
        public bool busy = false;
    }

    // NOTE: This message is only sent, never received
    // therefore it does not need a no parameter constructor.
    // If you do receive one you'll get an error unless you
    // add a no parameter constructor.
    private class MessageScored : MessageCmdData {
        public MessageScored(int _points) {
            points = _points;
        }

        public int points;
    }

    void InitializeNetPlayer(SpawnInfo spawnInfo) {
        // Save the netplayer object so we can use it send messages to the phone
        m_netPlayer = spawnInfo.netPlayer;

        // Register handler to call if the player disconnects from the game.
        m_netPlayer.OnDisconnect += Disconnected;

        // Setup events for the different messages.
        m_netPlayer.RegisterCmdHandler<MessageMove>("move", OnMove);
        m_netPlayer.RegisterCmdHandler<MessageSetName>("setName", OnSetName);
        m_netPlayer.RegisterCmdHandler<MessageBusy>("busy", OnBusy);

        ExampleSimpleGameSettings settings = ExampleSimpleGameSettings.settings();
        m_position = new Vector3(m_rand.Next(settings.areaWidth), 0, m_rand.Next(settings.areaHeight));
        transform.localPosition = m_position;

        // Tell controller to go to play mode (it was in in waiting mode)
        m_netPlayer.SendCmd("play");

        // Tell controller what color
        m_netPlayer.SendCmd("color", new MessageColor(m_renderer.material.color));

        SetName(spawnInfo.name);
    }

    void Start() {
        m_renderer = gameObject.GetComponent<Renderer>();
        m_position = gameObject.transform.localPosition;
    }

    public void Update() {
    }

    private void SetName(string name) {
        m_name = name;
    }

    public void OnTriggerEnter(Collider other) {
        // Because of physics layers we can only collide with the goal
        m_netPlayer.SendCmd("scored", new MessageScored(m_rand.Next(5, 15)));
    }

    private void Disconnected(object sender, EventArgs e) {
        // I don't think we need to do anything?
    }

    private void OnMove(MessageMove data) {
        ExampleSimpleGameSettings settings = ExampleSimpleGameSettings.settings();
        m_position.x = data.x * settings.areaWidth;
        m_position.z = settings.areaHeight - (data.y * settings.areaHeight) - 1;  // because in 2D down is positive.

        gameObject.transform.localPosition = m_position;
    }

    private void OnSetName(MessageSetName data) {
        if (data.name.Length == 0) {
            m_netPlayer.SendCmd("setName", new MessageSetName(m_name));
        } else {
            SetName(data.name);
        }
    }

    private void OnBusy(MessageBusy data) {
        // not used.
    }

    private System.Random m_rand = new System.Random();
    private NetPlayer m_netPlayer;
    private Renderer m_renderer;
    private Vector3 m_position;
    private string m_name;
}

}  // namespace HappyFunTimesExample

