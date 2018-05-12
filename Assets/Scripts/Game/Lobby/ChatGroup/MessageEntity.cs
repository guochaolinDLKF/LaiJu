//===================================================
//Author      : DRB
//CreateTime  ：10/13/2017 8:48:15 PM
//Description ：
//===================================================
using UnityEngine;


public class MessageEntity
{
    public byte[] message;

    public ChatType type;

    public PlayerEntity sendPlayer;

    public bool isPlayer;

    public MessageEntity() { }

    public MessageEntity(byte[] message, ChatType type, bool isPlayer)
    {
        this.message = message;
        this.type = type;
        this.isPlayer = isPlayer;
    }

}
