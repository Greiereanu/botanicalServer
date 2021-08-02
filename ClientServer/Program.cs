using ClientServer.Model;
using Garden.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Model;

namespace ClientServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Conectare la baza de date...");
            NetTcpBinding tcp = new NetTcpBinding();
            tcp.OpenTimeout = new TimeSpan(0, 60, 0);
            tcp.SendTimeout = new TimeSpan(0, 60, 0);
            tcp.ReceiveTimeout = new TimeSpan(0, 60, 0);
            tcp.CloseTimeout = new TimeSpan(0, 60, 0);
            tcp.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcp.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
            string s = ConfigurationManager.ConnectionStrings["IPAddress"].ConnectionString;
            tcp.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            tcp.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
            try
            {
                EmployeePersistanceDB employees = new EmployeePersistanceDB();
                ServiceHost gazda = new ServiceHost(employees);
                gazda.AddServiceEndpoint(typeof(IEmployeePersistanceDB), tcp, "net.tcp://" + s + ":52001/Employees");
                gazda.Open();
                Console.WriteLine("Conectare realizata.");

                PlantPersistanceDB plants = new PlantPersistanceDB();
                ServiceHost gazda2 = new ServiceHost(plants);
                gazda2.AddServiceEndpoint(typeof(IPlantPersistanceDB), tcp, "net.tcp://" + s + ":52001/Plants");
                gazda2.Open();


                Console.ReadLine();
                gazda.Close();
                gazda2.Close();
    
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nu s-a realizat conectarea la baza de date. " + ex.ToString());
                Console.ReadLine();
            }
        }
        
    }
}
