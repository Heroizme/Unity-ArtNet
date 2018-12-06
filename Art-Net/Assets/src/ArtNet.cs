﻿using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Net;
using System.IO;

public class ArtNet:MonoBehaviour
{

    public string destinationIP = "255.255.255.255";
    public byte universe = 0x0;
    public byte[] _data = new byte[512];

    private UdpClient _socket;
    private IPEndPoint _target;

    private byte[] _artNetPacket = new byte[530];

    public ArtNet()
    {
        _target = new IPEndPoint(IPAddress.Parse(destinationIP), 6454);

        _socket = new UdpClient();
        _socket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        _socket.Connect(_target);

        string str = "Art-Net";
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        encoding.GetBytes(str, 0, str.Length, _artNetPacket, 0);

        _artNetPacket[7] = 0x0;

        //opcode low byte first
        _artNetPacket[8] = 0x00;
        _artNetPacket[9] = 0x50;

        //proto ver high byte first
        _artNetPacket[10] = 0x0;
        _artNetPacket[11] = 14;

        //sequence 
        _artNetPacket[12] = 0x0;

        //physical port
        _artNetPacket[13] = 0x0;

        //universe low byte first
        _artNetPacket[14] = 0x0;
        _artNetPacket[15] = 0x0;

        //length high byte first
        _artNetPacket[16] = ((512 >> 8) & 0xFF);
        _artNetPacket[17] = (512 & 0xFF);
    }

    private void tx()
    {
        _artNetPacket[14] = universe; //0x0;
        Buffer.BlockCopy(_data, 0, _artNetPacket, 18, 512);
        tx();

        try
        {
            _socket.Send(_artNetPacket, _artNetPacket.Length);
        }
        catch (Exception e)
        {
            Debug.Log (this +" "+ e);
        }
    }

    public void Update()
    {
        tx();
    }
}