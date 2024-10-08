using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Proxy
{
    Dictionary<string, byte> MessageIDs = new Dictionary<string, byte>()
    {
        {"MOVE_START", 10 },
        {"MOVE_STOP", 12 },
        {"ATTACK1", 20 },
        {"ATTACK2", 22 },
        {"ATTACK3", 24 },
    };


    public void MOVE_START(byte Dir, UInt16 X, UInt16 Y)
    {
        stMSG_HDR hdr = new stMSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (byte)(Marshal.SizeOf(Dir) + Marshal.SizeOf(X) + Marshal.SizeOf(Y));
        hdr.MsgType = MessageIDs["MOVE_START"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<stMSG_HDR>(hdr, bytesHdr);

        //byte[] bytesX = BitConverter.GetBytes(X);
        //byte[] bytesY = BitConverter.GetBytes(Y);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, bytesHdr.Length); offset += bytesHdr.Length;
        bytes[offset] = Dir; offset += 1;
        Buffer.BlockCopy(BitConverter.GetBytes(X), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        Buffer.BlockCopy(BitConverter.GetBytes(Y), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
       
        RPC.Network.SendBytes(bytes);   
    }

    public void MOVE_STOP(byte Dir, UInt16 X, UInt16 Y)
    {
        stMSG_HDR hdr = new stMSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (byte)(Marshal.SizeOf(Dir) + Marshal.SizeOf(X) + Marshal.SizeOf(Y));
        hdr.MsgType = MessageIDs["MOVE_STOP"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<stMSG_HDR>(hdr, bytesHdr);

        byte[] bytesX = BitConverter.GetBytes(X);
        byte[] bytesY = BitConverter.GetBytes(Y);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, bytesHdr.Length); offset += bytesHdr.Length;
        bytes[offset] = Dir; offset += 1;
        Buffer.BlockCopy(bytesX, 0, bytes, offset, bytesX.Length); offset += bytesX.Length;
        Buffer.BlockCopy(bytesY, 0, bytes, offset, bytesY.Length); offset += bytesY.Length;

        RPC.Network.SendBytes(bytes);
    }

    public void ATTACK1(byte Dir, UInt16 X, UInt16 Y)
    {
        stMSG_HDR hdr = new stMSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (byte)(Marshal.SizeOf(Dir) + Marshal.SizeOf(X) + Marshal.SizeOf(Y));
        hdr.MsgType = MessageIDs["ATTACK1"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<stMSG_HDR>(hdr, bytesHdr);

        byte[] bytesX = BitConverter.GetBytes(X);
        byte[] bytesY = BitConverter.GetBytes(Y);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, bytesHdr.Length); offset += bytesHdr.Length;
        bytes[offset] = Dir; offset += 1;
        Buffer.BlockCopy(bytesX, 0, bytes, offset, bytesX.Length); offset += bytesX.Length;
        Buffer.BlockCopy(bytesY, 0, bytes, offset, bytesY.Length); offset += bytesY.Length;

        RPC.Network.SendBytes(bytes);
    }

    public void ATTACK2(byte Dir, UInt16 X, UInt16 Y)
    {
        stMSG_HDR hdr = new stMSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (byte)(Marshal.SizeOf(Dir) + Marshal.SizeOf(X) + Marshal.SizeOf(Y));
        hdr.MsgType = MessageIDs["ATTACK2"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<stMSG_HDR>(hdr, bytesHdr);

        byte[] bytesX = BitConverter.GetBytes(X);
        byte[] bytesY = BitConverter.GetBytes(Y);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, bytesHdr.Length); offset += bytesHdr.Length;
        bytes[offset] = Dir; offset += 1;
        Buffer.BlockCopy(bytesX, 0, bytes, offset, bytesX.Length); offset += bytesX.Length;
        Buffer.BlockCopy(bytesY, 0, bytes, offset, bytesY.Length); offset += bytesY.Length;

        RPC.Network.SendBytes(bytes);
    }

    public void ATTACK3(byte Dir, UInt16 X, UInt16 Y)
    {
        stMSG_HDR hdr = new stMSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (byte)(Marshal.SizeOf(Dir) + Marshal.SizeOf(X) + Marshal.SizeOf(Y));
        hdr.MsgType = MessageIDs["ATTACK3"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<stMSG_HDR>(hdr, bytesHdr);

        byte[] bytesX = BitConverter.GetBytes(X);
        byte[] bytesY = BitConverter.GetBytes(Y);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, bytesHdr.Length); offset += bytesHdr.Length;
        bytes[offset] = Dir; offset += 1;
        Buffer.BlockCopy(bytesX, 0, bytes, offset, bytesX.Length); offset += bytesX.Length;
        Buffer.BlockCopy(bytesY, 0, bytes, offset, bytesY.Length); offset += bytesY.Length;

        RPC.Network.SendBytes(bytes);
    }

    public void ECHO(UInt32 Time)
    {
    }
}
