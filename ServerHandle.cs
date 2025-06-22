using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Server2023
{
    class ServerHandle
    {
        public static void Welcome(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();
            int _avatar = _packet.ReadInt();
            string _avatarUrl = _packet.ReadString();

            Console.Write($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected and is now named {_fromClient}");
            //Server.clients[_fromClient].SendIntoGame(_username);
            Server.clients[_fromClient].user = _username;
            Server.clients[_fromClient].avatar = _avatar;
            Server.clients[_fromClient].avatarUrl = _avatarUrl;

            ServerSend.LobbyMessage();
        }

        public static void VoiceMessage(int _fromClient, Packet _packet)
        {
            int size = _packet.ReadInt();
            float[] data = new float[size];
            for (int x = 0; x < size; x++)
            {
                float value = _packet.ReadFloat();
                data[x] = value;
            }
            Console.WriteLine("Received array of size: " + size);
            ServerSend.VoiceMessage(data);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            Vector3 _position = _packet.ReadVector3();
            Quaternion _rotation = _packet.ReadQuaternion();

            Vector3 _headPosition = _packet.ReadVector3();
            Quaternion _headRotation = _packet.ReadQuaternion();

            Vector3 _leftPosition = _packet.ReadVector3();
            Quaternion _leftRotation = _packet.ReadQuaternion();

            Vector3 _rightPosition = _packet.ReadVector3();
            Quaternion _rightRotation = _packet.ReadQuaternion();

            Server.clients[_fromClient].player.SetStats(_position, _rotation, _headPosition, _headRotation, _leftPosition, _leftRotation, _rightPosition, _rightRotation);
        }

        public static void ChatMessage(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _msg = _packet.ReadString();

            Console.WriteLine("CHAT from client: " + Server.clients[_fromClient].user + " Message: " + _msg);

            ServerSend.ChatMessage(_fromClient, _msg);
        }

        public static void DeployMessage(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            int _environment = _packet.ReadInt();
            Console.WriteLine("DEPLOY!");

            foreach (Client _client in Server.clients.Values)
            {
                if(_client.user != null)
                {
                    Console.WriteLine("ID: " + _client.id + " user: " + _client.user + " avatar: " + _client.avatar + " avatarUrl: " + _client.avatarUrl);
                    Server.clients[_client.id].SendIntoGame(_client.user, _client.avatar, _client.id);
                }
            }

            ServerSend.SpawnEnvironment(_environment);
        }
    }
}