using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SynchronousSocketListener
{

    // Incoming data from the client.  
    public static string data = null;

    public static void StartListening()
    {
        // Data buffer for incoming data.  
        byte[] bytes = new Byte[1024];

        // Establish the local endpoint for the socket.  
        // Dns.GetHostName returns the name of the   
        // host running the application.  

        IPAddress ipAddress = IPAddress.Any;
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 7878);

        // Create a TCP/IP socket.  
        Socket listener = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and   
        // listen for incoming connections.  
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            // Start listening for connections.  
            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                // Program is suspended while waiting for an incoming connection.  
                Socket handler = listener.Accept();
                Console.WriteLine("Incoming connection");
                Thread NeuerThread = new Thread(() => SendSomething(handler));
                NeuerThread.Start();

            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }

    public static void SendSomething(Socket handler)
    {
        try
        {
            string machinenumber = "987654321";
            int duration = 30;

            while (true)
            {
                //20 Sekunden Läuft
                for (int i = 0; i <= duration; i++)
                {
                    string DatumUhrzeit = DateTime.UtcNow.ToString("u");
                    Console.WriteLine("Send Run");
                    //data = string.Format("{0}|{1}_2|AVAILABLE|{1}_92|ACTIVE|{1}_91|AUTOMATIC\n", DatumUhrzeit, machinenumber);
                    data = string.Format("{0}|{1}_2|AVAILABLE|{1}_92|ACTIVE|{1}_91|AUTOMATIC|{1}_86|ON|{1}_1|ARMED\n", DatumUhrzeit, machinenumber);// Powerstate: |{1}_86|ON // Notschalter|{1}_1|ARMED\n
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    System.Threading.Thread.Sleep(1000);
                }
                //20 Sekunden Steht
                for (int i = 0; i <= duration; i++)
                {
                    string DatumUhrzeit = DateTime.UtcNow.ToString("u");
                    Console.WriteLine("Send Stop");
                    //data = string.Format("{0}|{1}_2|AVAILABLE|{1}_92|READY|{1}_91|AUTOMATIC\n", DatumUhrzeit, machinenumber);
                    data = string.Format("{0}|{1}_2|AVAILABLE|{1}_92|READY|{1}_91|AUTOMATIC|{1}_86|ON|{1}_1|ARMED\n", DatumUhrzeit, machinenumber);
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);

                    data = string.Format("{0}|{1}_106|NORMAL\n", DatumUhrzeit, machinenumber);
                    msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    System.Threading.Thread.Sleep(1000);
                }

                //20 Sekunden Alarm
                for (int i = 0; i <= duration; i++)
                {
                    string DatumUhrzeit = DateTime.UtcNow.ToString("u");
                    Console.WriteLine("Send Alarm");
                    //data = string.Format("{0}|{1}_2|AVAILABLE|{1}_92|READY|{1}_91|AUTOMATIC|{1}_86|ON|{1}_1|ARMED|0815|2|3|MEL Alles besser\n", DatumUhrzeit, machinenumber);
                    data = string.Format("{0}|{1}_2|AVAILABLE|{1}_92|READY|{1}_91|AUTOMATIC|{1}_86|ON|{1}_1|ARMED\n", DatumUhrzeit, machinenumber);

                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);

                    data = string.Format("{0}|{1}_106|FAULT|700459|||MEL, Allgemeiner Alarm!\n", DatumUhrzeit, machinenumber);
                    msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    System.Threading.Thread.Sleep(1000);
                }

                //20 Sekunden Aus
                for (int i = 0; i <= duration; i++)
                {
                    string DatumUhrzeit = DateTime.UtcNow.ToString("u");
                    Console.WriteLine("Send Off");
                    data = string.Format("{0}|{1}_2|UNAVAILABLE|{1}_92|UNAVAILABLE|{1}_91|UNAVAILABLE|{1}_86|UNAVAILABLE\n", DatumUhrzeit, machinenumber);
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    data = string.Format("{0}|{1}_106|UNAVAILABLE\n", DatumUhrzeit, machinenumber);
                    msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    System.Threading.Thread.Sleep(1000);
                }


            }


            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch( Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }

    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}
