using Grpc.Core;
using ServidorConsola.Entidades;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    //ESTE SIIII
    static Mesa mesa;
    static List<Mesa> LisMesa = new List<Mesa>();

    static Personas persona = new Personas();
    static List<Personas> personaList = new List<Personas>();

    static Reservaciones reservacion;
    static List<Reservaciones> reservacionList = new List<Reservaciones>();

    public static int cont = 1;
    public static void init()
    {
        mesa = new Mesa();
        mesa.MesaId = 1;
        mesa.Ubicacion = "Frente a la playa";
        mesa.Capacidad = 2;
        mesa.Forma = "Circular";
        mesa.Precio = 1200.00;
        mesa.Dsiponibilidad = true;
        LisMesa.Add(mesa);

        mesa = new Mesa();
        mesa.MesaId = 2;
        mesa.Ubicacion = "Al lado del bar";
        mesa.Capacidad = 4;
        mesa.Forma = "Cuadrado";
        mesa.Precio = 1500.00;
        mesa.Dsiponibilidad = true;
        LisMesa.Add(mesa);

        mesa = new Mesa();
        mesa.MesaId = 3;
        mesa.Ubicacion = "Terraza";
        mesa.Capacidad = 6;
        mesa.Forma = "Cuadrado";
        mesa.Precio = 2500.00;
        mesa.Dsiponibilidad = true;
        LisMesa.Add(mesa);



    }

    public static void RegistroMesa()
    {
        int cant;
        Console.WriteLine("====================================");
        Console.WriteLine("        Registro de mesas         ");
        Console.WriteLine("====================================\n");
        Console.Write("Cuantas mesa desea registrar: ");
        cant = int.Parse(Console.ReadLine());

        for (int i = 0; i < cant; i++)
        {
            mesa = new Mesa();
            Console.Write("\nMesaId: ");
            mesa.MesaId = int.Parse(Console.ReadLine());
            Console.Write("\nUbicacion: ");
            mesa.Ubicacion = Console.ReadLine();
            Console.Write("\nCapacidad: ");
            mesa.Capacidad = int.Parse(Console.ReadLine());
            Console.Write("\nForma: ");
            mesa.Forma = Console.ReadLine();
            Console.Write("\nPrecio: ");
            mesa.Precio = double.Parse(Console.ReadLine());


            LisMesa.Add(mesa);
            Console.Write("\n\n");

        }
        Console.Write("\nGuardado. ");
        Thread.Sleep(2000);
    }

    public static void ListarMesa()
    {
        int i = 1;
        Console.WriteLine("==================================");
        Console.WriteLine("         Lista de Mesas         ");
        Console.WriteLine("==================================\n");
        foreach (var item in LisMesa)
        {

            Console.WriteLine("MesaId: " + item.MesaId);
            Console.WriteLine("Ubicacion: " + item.Ubicacion);
            Console.WriteLine("Capacidad: " + item.Capacidad);
            Console.WriteLine("Forma: " + item.Forma);
            Console.WriteLine("Precio: " + item.Precio);
            Console.WriteLine("Disponibilidad: " + item.Dsiponibilidad);
            Console.WriteLine("\n\n");
        }
        Console.ReadKey();

    }

    private static void Main(string[] args)
    {

        int opc = 0;
        init();
        Thread hilo = new Thread(server);
        hilo.Start();

        do
        {
            Console.Clear();
            Console.Write("1.Agregar Mesa\n2.Lista Mesa\n3.Listar Reservaciones\n4.Listar Cliente\n5.Salir\nElija una opcion: ");
            opc = int.Parse(Console.ReadLine());

            switch (opc)
            {
                case 1:
                    Console.Clear();
                    RegistroMesa();

                    break;
                case 2:
                    Console.Clear();
                    ListarMesa();
                    break;
                case 3:
                    Console.Clear();
                    ListarReservacion();


                    break;
                case 4:
                    Console.Clear();
                    listarCliente();

                    break;

                case 5:
                    Console.Clear();
                    Environment.Exit(0);

                    break;
                default:
                    Console.WriteLine("Opcion invalida...");
                    break;


            }


        } while (opc != 5);
    }


    public static void ListarReservacion()
    {
        Console.WriteLine("======================================");
        Console.WriteLine("         Listado de reservaciones         ");
        Console.WriteLine("======================================\n");
        foreach (var item in reservacionList)
        {

            Console.WriteLine("ReservacionId: " + item.reservacionId);
            Console.WriteLine("----------------");

            foreach (var itemP in personaList)
            {
                if (itemP.Id == item.personaId)
                {
                    Console.WriteLine("PersonaId: " + itemP.Id);
                    Console.WriteLine("Nombres: " + itemP.nombres);
                }
            }
            Console.WriteLine("----------------");
            foreach (var itemM in LisMesa)
            {
                if (itemM.MesaId == item.mesaId)
                {
                    Console.WriteLine("MesaId: " + itemM.MesaId);
                    Console.WriteLine("Ubicacion: " + itemM.Ubicacion);
                    Console.WriteLine("Forma: " + itemM.Forma);
                }
            }



            Console.WriteLine("\n\n");
        }
        Console.ReadKey();
    }
    public static void listarCliente()
    {
        Console.WriteLine("===================================");
        Console.WriteLine("         Listado de Clientes         ");
        Console.WriteLine("===================================\n");
        foreach (var item in personaList)
        {

            Console.WriteLine("PersonaId: " + item.Id);
            Console.WriteLine("Nombres: " + item.nombres);
            Console.WriteLine("Telefono: " + item.Telefono);
            Console.WriteLine("Email: " + item.Email);

            Console.WriteLine("\n\n");
        }
        Console.ReadKey();
    }
    public static void server()
    {
        int op = 0;
        Mesa mesas;
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 14800);

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
                    data = Encoding.ASCII.GetString(bytes, 0, byteRec);

                    if (data.IndexOf("<EOF>") > -1)
                        break;
                }

                switch (Convert.ToInt32(data.Replace("<EOF>", "")))
                {
                    case 1:
                        byte[] bytesM = null;

                        byte[] can = Encoding.ASCII.GetBytes(Convert.ToString(LisMesa.Count) + "<EOF>");
                        handler.Send(can);

                        //enviando lista de mesas
                        Thread.Sleep(500);
                        foreach (var item in LisMesa)
                        {
                            bytesM = new byte[1024];

                            bytesM = Encoding.ASCII.GetBytes(Convert.ToString(item.MesaId) + "<EOF>");
                            handler.Send(bytesM);
                            Thread.Sleep(100);
                            bytesM = Encoding.ASCII.GetBytes(item.Ubicacion + "<EOF>");
                            handler.Send(bytesM);
                            Thread.Sleep(100);
                            bytesM = Encoding.ASCII.GetBytes(Convert.ToString(item.Capacidad) + "<EOF>");
                            handler.Send(bytesM);
                            Thread.Sleep(100);
                            bytesM = Encoding.ASCII.GetBytes(item.Forma + "<EOF>");
                            handler.Send(bytesM);
                            Thread.Sleep(100);
                            bytesM = Encoding.ASCII.GetBytes(Convert.ToString(item.Precio) + "<EOF>");
                            handler.Send(bytesM);
                            Thread.Sleep(100);
                            bytesM = Encoding.ASCII.GetBytes(Convert.ToString(item.Dsiponibilidad) + "<EOF>");
                            handler.Send(bytesM);

                        }

                        while (true)
                        {

                            bool aux = false, fin = false;
                            while (true)
                            {
                                bytes = new byte[1024];
                                int byteRec = handler.Receive(bytes);
                                data = Encoding.ASCII.GetString(bytes, 0, byteRec);

                                if (data.IndexOf("<EOF>") > -1)
                                    break;
                            }

                            if (Convert.ToInt32(data.Replace("<EOF>", "")) == 1)
                            {
                                int reser = 1;

                                reservacion = new Reservaciones();
                                while (true)
                                {
                                    string dataRes = null;
                                    byte[] bytesRes = null;

                                    while (true)
                                    {
                                        bytesRes = new byte[1024];
                                        int byteRec = handler.Receive(bytesRes);
                                        dataRes += Encoding.ASCII.GetString(bytesRes, 0, byteRec);

                                        if (dataRes.IndexOf("<EOF>") > -1)
                                            break;

                                        if (Convert.ToInt32(dataRes) == 0)
                                        {

                                            aux = true;

                                            break;

                                        }


                                    }

                                    if (aux)
                                        break;

                                    if (reser == 1)
                                        reservacion.reservacionId = Convert.ToInt32(dataRes.Replace("<EOF>", ""));

                                    if (reser == 2)
                                        reservacion.personaId = Convert.ToInt32(dataRes.Replace("<EOF>", ""));

                                    if (reser == 3)
                                    {
                                        reservacion.mesaId = Convert.ToInt32(dataRes.Replace("<EOF>", ""));

                                        reservacionList.Add(reservacion);
                                        foreach (var m in LisMesa)
                                        {
                                            if (m.MesaId == reservacion.mesaId)
                                            {
                                                m.Dsiponibilidad = false;
                                            }
                                        }
                                        fin = true;
                                        break;
                                    }

                                    reser++;



                                }

                                if (fin)
                                    break;
                            }
                            else
                            {
                                if (Convert.ToInt32(data.Replace("<EOF>", "")) == 2)
                                {
                                    break;
                                }
                            }
                        }

                        break;
                    case 2:
                        int contP = 1;

                        while (true)
                        {
                            string dataP = null;
                            byte[] bytesP = null;

                            while (true)
                            {
                                bytesP = new byte[1024];
                                int byteRec = handler.Receive(bytesP);
                                dataP += Encoding.ASCII.GetString(bytesP, 0, byteRec);

                                if (data.IndexOf("<EOF>") > -1)
                                    break;
                            }

                            if (contP == 1)
                                persona.Id = Convert.ToInt32(dataP.Replace("<EOF>", ""));

                            if (contP == 2)
                                persona.nombres = dataP.Replace("<EOF>", "");

                            if (contP == 3)
                                persona.Telefono = dataP.Replace("<EOF>", "");

                            if (contP == 4)
                            {
                                persona.Email = dataP.Replace("<EOF>", "");
                                break;
                            }


                            contP++;
                        }
                        personaList.Add(persona);
                        break;
                    case 3:
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Opcion invalida...");
                        break;
                }


            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }






}