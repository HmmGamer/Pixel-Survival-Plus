using System.IO;
using UnityEngine;

public static class A
{
    public static class Tags
    {
        public const string player = "Player";
        public const string playerWeapon = "player weapon";
        public const string enemy = "enemy";
        public const string home = "home";
        public const string arrow = "Arrow";
        public const string tower = "tower";
    }
    public static class LayerMasks
    {
        public static LayerMask player = LayerMask.GetMask("player");
        public static LayerMask floor = LayerMask.GetMask("floor");
        public static LayerMask enemy = LayerMask.GetMask("enemy");
    }
    public static class Layers
    {
        public const int player = 8;
        public const int enemy = 9;
        public const int default1 = 0;
        public const int item = 7;
        public const int floor = 6;
    }
    public static class Anim
    {
        public const string playerJump = "jump";
        public const string playerSpeed = "Speed";
        public const string playerIsOnWall = "isOnWall";
        public const string playerIsGrounded = "isOnGround";
        public const string playerIsJumping = "isJumping";
        public const string PlayerAttack = "attack";
    }
    public static class DataKey
    {
        private const string keyIdData = "ji385";
        public const string money = "rrr123";
        public static string GetKeyIdData(int iId)
        {
            return keyIdData + iId;
        }
    }

}
