using ClientServer.Model;
using Garden.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Model
{
    class JSONReport : IReport
    {
        private IPlantPersistanceDB plantPersistance;
        public void generate()
        {
            ChannelFactory<IPlantPersistanceDB> plantsChannel;
            NetTcpBinding tcp = new NetTcpBinding();
            tcp.OpenTimeout = new TimeSpan(0, 60, 0);
            tcp.SendTimeout = new TimeSpan(0, 60, 0);
            tcp.ReceiveTimeout = new TimeSpan(0, 60, 0);
            tcp.CloseTimeout = new TimeSpan(0, 60, 0);
            tcp.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcp.Security.Mode = SecurityMode.Transport;
            tcp.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            tcp.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            string s = ConfigurationManager.ConnectionStrings["IPAddress"].ConnectionString;
            plantsChannel = new ChannelFactory<IPlantPersistanceDB>(tcp, "net.tcp://" + s + ":52001/Plants");
            try
            {
                this.plantPersistance = plantsChannel.CreateChannel();
                List<Plant> result = this.plantPersistance.loadPlants();
                var filepath = "../../data/JSON_Report.json";
                using (StreamWriter writer = new StreamWriter(new FileStream(filepath,
                FileMode.Create, FileAccess.Write)))
                {
                    foreach (Plant p in result)
                    {
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                        writer.WriteLine(json);
                    }
                }
                MessageBox.Show("JSON Report generated succesfully!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
