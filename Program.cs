using Grpc.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
       // server s = new Server();
        int Opcion = 0;
      
        //server();
        Console.WriteLine("============================");
        Console.WriteLine("MENU");
        Console.WriteLine("============================");
        Console.WriteLine("1-AGREGAR MESAS");
        Console.WriteLine("2-CONSULTAR MESAS");
        Opcion = int.Parse(Console.ReadLine());

        if (Opcion == 1)
        {
            AgregarMesa();

        }
        else
            Console.WriteLine("Faltan mas funciones");


        server();
    }

    public static void AgregarMesa()
    {
        int CantidadMesas = 0;
        int i = 0;
        int MesaId = 0;
        string UbicacionMesa;
        int CapacidadMesa = 0;
        string FormaMesa;
        int PrecioMesa = 0;
        string Disponibilidad;

        Console.WriteLine("Cuantas mesas desea agregar: ");
        CantidadMesas = int.Parse(Console.ReadLine());

        for(i = 0; i < CantidadMesas; i++)
        {
            Console.WriteLine("Digite Id de mesa: ");
            MesaId = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite la ubicacion de la mesa: ");
            UbicacionMesa = Console.ReadLine();

            Console.WriteLine("Digite la capacidad de la mesa: ");
            CapacidadMesa = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite la forma de la mesa: ");
            FormaMesa = Console.ReadLine();

            Console.WriteLine("Digite precio de la mesa: ");
            PrecioMesa = int.Parse(Console.ReadLine());

            Console.WriteLine("La mesa esta disponible? ");
            Disponibilidad = Console.ReadLine();
        }
    }



    public static void server()
    {
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11200);

        try
        {
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(localEndPoint);
            listener.Listen(10);
            Console.WriteLine("Esperando Conexion");

            Socket handler = listener.Accept();

            while (true)
            {
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int byteRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, byteRec);

                    if (data.IndexOf("<EOF>") > -1)
                        break;
                }

                Console.WriteLine("Texto del cliente: " + data.Replace("<EOF>", ""));

                //enviar mensaje de verificacion al cliente 
                byte[] msj = Encoding.ASCII.GetBytes("Recibido");
                handler.Send(msj);
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}