
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Proxy
{
    Dictionary<string, byte> MessageIDs = new Dictionary<string, byte>()
    {

        {"MOVE_START", 10},
        {"MOVE_STOP", 12},
        {"ATTACK1", 20},
        {"ATTACK2", 22},
        {"ATTACK3", 24},
        {"ECHO", 252},
    };


    public void MOVE_START(byte Dir, UInt16 X, UInt16 Y,  NetworkManager sesion = null)
    {
        JNET_PROTOCOL.SIMPLE_MSG_HDR hdr = new JNET_PROTOCOL.SIMPLE_MSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (Byte)(sizeof(byte) + sizeof(UInt16) + sizeof(UInt16));
        hdr.MsgType = MessageIDs["MOVE_START"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<JNET_PROTOCOL.SIMPLE_MSG_HDR>(hdr, bytesHdr);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, Marshal.SizeOf(hdr)); offset += Marshal.SizeOf(hdr);
        bytes[offset++] = Dir;
        Buffer.BlockCopy(BitConverter.GetBytes(X), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        Buffer.BlockCopy(BitConverter.GetBytes(Y), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        RPC.Network.SendBytes(bytes);          
    }

    public void MOVE_STOP(byte Dir, UInt16 X, UInt16 Y,  NetworkManager sesion = null)
    {
        JNET_PROTOCOL.SIMPLE_MSG_HDR hdr = new JNET_PROTOCOL.SIMPLE_MSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (Byte)(sizeof(byte) + sizeof(UInt16) + sizeof(UInt16));
        hdr.MsgType = MessageIDs["MOVE_STOP"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<JNET_PROTOCOL.SIMPLE_MSG_HDR>(hdr, bytesHdr);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, Marshal.SizeOf(hdr)); offset += Marshal.SizeOf(hdr);
        bytes[offset++] = Dir;
        Buffer.BlockCopy(BitConverter.GetBytes(X), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        Buffer.BlockCopy(BitConverter.GetBytes(Y), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        RPC.Network.SendBytes(bytes);          
    }

    public void ATTACK1(byte Dir, UInt16 X, UInt16 Y,  NetworkManager sesion = null)
    {
        JNET_PROTOCOL.SIMPLE_MSG_HDR hdr = new JNET_PROTOCOL.SIMPLE_MSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (Byte)(sizeof(byte) + sizeof(UInt16) + sizeof(UInt16));
        hdr.MsgType = MessageIDs["ATTACK1"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<JNET_PROTOCOL.SIMPLE_MSG_HDR>(hdr, bytesHdr);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, Marshal.SizeOf(hdr)); offset += Marshal.SizeOf(hdr);
        bytes[offset++] = Dir;
        Buffer.BlockCopy(BitConverter.GetBytes(X), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        Buffer.BlockCopy(BitConverter.GetBytes(Y), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        RPC.Network.SendBytes(bytes);          
    }

    public void ATTACK2(byte Dir, UInt16 X, UInt16 Y,  NetworkManager sesion = null)
    {
        JNET_PROTOCOL.SIMPLE_MSG_HDR hdr = new JNET_PROTOCOL.SIMPLE_MSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (Byte)(sizeof(byte) + sizeof(UInt16) + sizeof(UInt16));
        hdr.MsgType = MessageIDs["ATTACK2"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<JNET_PROTOCOL.SIMPLE_MSG_HDR>(hdr, bytesHdr);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, Marshal.SizeOf(hdr)); offset += Marshal.SizeOf(hdr);
        bytes[offset++] = Dir;
        Buffer.BlockCopy(BitConverter.GetBytes(X), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        Buffer.BlockCopy(BitConverter.GetBytes(Y), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        RPC.Network.SendBytes(bytes);    

    }

    public void ATTACK3(byte Dir, UInt16 X, UInt16 Y,  NetworkManager sesion = null)
    {
        JNET_PROTOCOL.SIMPLE_MSG_HDR hdr = new JNET_PROTOCOL.SIMPLE_MSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (Byte)(sizeof(byte) + sizeof(UInt16) + sizeof(UInt16));
        hdr.MsgType = MessageIDs["ATTACK3"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<JNET_PROTOCOL.SIMPLE_MSG_HDR>(hdr, bytesHdr);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, Marshal.SizeOf(hdr)); offset += Marshal.SizeOf(hdr);
        bytes[offset++] = Dir;
        Buffer.BlockCopy(BitConverter.GetBytes(X), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        Buffer.BlockCopy(BitConverter.GetBytes(Y), 0, bytes, offset, sizeof(UInt16)); offset += sizeof(UInt16);
        RPC.Network.SendBytes(bytes);          
    }

    public void ECHO(UInt32 Time,  NetworkManager sesion = null)
    {
        JNET_PROTOCOL.SIMPLE_MSG_HDR hdr = new JNET_PROTOCOL.SIMPLE_MSG_HDR();
        hdr.Code = RPC.ValidCode;
        hdr.MsgLen = (Byte)(sizeof(UInt32));
        hdr.MsgType = MessageIDs["ECHO"];

        byte[] bytes = new byte[Marshal.SizeOf(hdr) + hdr.MsgLen];

        byte[] bytesHdr = new byte[Marshal.SizeOf(hdr)];
        RPC.Network.MessageToBytes<JNET_PROTOCOL.SIMPLE_MSG_HDR>(hdr, bytesHdr);

        int offset = 0;
        Buffer.BlockCopy(bytesHdr, 0, bytes, offset, Marshal.SizeOf(hdr)); offset += Marshal.SizeOf(hdr);
        Buffer.BlockCopy(BitConverter.GetBytes(Time), 0, bytes, offset, sizeof(UInt32)); offset += sizeof(UInt32);
        RPC.Network.SendBytes(bytes);          
    }

}
