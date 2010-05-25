﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Text;
using System.Net;

namespace Demo.WindowsForms
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new MainForm());
      }
   }

   struct IpInfo
   {
      public string Ip;
      public int Port;
      public TcpState State;

      public string CountryName;
      public string RegionName;
      public string City;
      public double Latitude;
      public double Longitude;

      public DateTime Time;
   }

   class TraceRoute
   {
      readonly static string Data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
      readonly static byte[] DataBuffer;
      readonly static int timeout = 10000;

      static TraceRoute()
      {
         DataBuffer = Encoding.ASCII.GetBytes(Data);
      }

      public static List<IPAddress> GetTraceRoute(string hostNameOrAddress)
      {
         return GetTraceRoute(hostNameOrAddress, 1);
      }

      private static List<IPAddress> GetTraceRoute(string hostNameOrAddress, int ttl)
      {
         List<IPAddress> result = new List<IPAddress>();

         using(Ping pinger = new Ping())
         {
            PingOptions pingerOptions = new PingOptions(ttl, true);

            PingReply reply = pinger.Send(hostNameOrAddress, timeout, DataBuffer, pingerOptions);

            if(reply.Status == IPStatus.Success)
            {
               result.Add(reply.Address);
            }
            else if(reply.Status == IPStatus.TtlExpired)
            {
               // add the currently returned address
               result.Add(reply.Address);

               // recurse to get the next address...
               result.AddRange(GetTraceRoute(hostNameOrAddress, ttl + 1));
            }
            else
            {
               // failure... 
            }
         }

         return result;
      }
   }
}
