using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Server2023
{
    class Player
    {
        public int id;
        public string username;

        public Vector3 position;
        public Quaternion rotation;

        public Vector3 headPosition;
        public Quaternion headRotation;
        public Vector3 leftPosition;
        public Quaternion leftRotation;
        public Vector3 rightPosition;
        public Quaternion rightRotation;

        private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
        private bool[] inputs;

        public Player(int _id, string _username, Vector3 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = Quaternion.Identity;

            headPosition = _spawnPosition;
            rotation = Quaternion.Identity;
            leftPosition = _spawnPosition;
            leftRotation = Quaternion.Identity;
            rightPosition = _spawnPosition;
            rightRotation = Quaternion.Identity;

            inputs = new bool[4];
        }


        public void Update()
        {
            Move();
        }

        public void Move()
        {
            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }

        public void SetInput(bool[] _inputs, Quaternion _rotation)
        {
            inputs = _inputs;
            rotation = _rotation;
        }

        public void SetStats(Vector3 _position, Quaternion _rotation)
        {
            position = _position;
            rotation = _rotation;
        }

        public void SetStats(Vector3 _position, Quaternion _rotation, Vector3 _headPosition, Quaternion _headRotation, Vector3 _leftPosition, Quaternion _leftRotation, Vector3 _rightPosition, Quaternion _rightRotation)
        {
            position = _position;
            rotation = _rotation;

            headPosition = _headPosition;
            headRotation = _headRotation;

            leftPosition = _leftPosition;
            leftRotation = _leftRotation;

            rightPosition = _rightPosition;
            rightRotation = _rightRotation;
        }
    }
}
