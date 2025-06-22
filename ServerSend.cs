using System;
using System.Collections.Generic;
using System.Text;

namespace Server2023
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i < Server.maxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i < Server.maxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }



        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i < Server.maxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }


        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i < Server.maxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }




        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }
        public static void VoiceMessage(float[] _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.voiceMessage))
            {

                int size = _msg.Length;
                _packet.Write(size);

                foreach (var x in _msg)
                {
                    _packet.Write(x);
                }

                SendTCPDataToAll(_packet);
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player, int _avatar, string _avatarUrl)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_avatar);
                _packet.Write(_avatarUrl);
                _packet.Write(_player.position);
                _packet.Write(_player.rotation);

                //SendTCPData(_toClient, _packet);
                SendTCPDataToAll(_packet);
            }
        }
        public static void SpawnEnvironment(int _environment)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnEnvironment))
            {
                _packet.Write(_environment);
                SendTCPDataToAll(_packet);
            }
        }

        public static void PlayerPosition(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.position);

                _packet.Write(_player.headPosition);
                _packet.Write(_player.leftPosition);
                _packet.Write(_player.rightPosition);
                SendTCPDataToAll(_player.id, _packet);
            }
        }
        public static void PlayerRotation(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.rotation);

                _packet.Write(_player.headRotation);
                _packet.Write(_player.leftRotation);
                _packet.Write(_player.rightRotation);
                SendTCPDataToAll(_player.id, _packet);
            }
        }
        public static void ChatMessage(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.chatMessage))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPDataToAll(_packet);
            }
        }

        public static void LobbyMessage()
        {
            using (Packet _packet = new Packet((int)ServerPackets.lobbyMessage))
            {
                string usernames = "";
                int avatarCounter = 0;

                foreach (Client _client in Server.clients.Values)
                {
                    if (_client.user != "")
                    {
                        usernames = usernames + _client.user + "\n";

                        if(_client.avatarUrl != null)
                        {
                            avatarCounter++;
                        }
                    }
                }
                _packet.Write(usernames);
                _packet.Write(avatarCounter);

                foreach (Client _client in Server.clients.Values)
                {
                    if (_client.user != "")
                    {
                        if (_client.avatarUrl != null)
                        {
                            Console.WriteLine(_client.avatarUrl);
                           _packet.Write(_client.avatarUrl);
                        }
                    }
                }

                SendTCPDataToAll(_packet);
            }
        }
    }
}