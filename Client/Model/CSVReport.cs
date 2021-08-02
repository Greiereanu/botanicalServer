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
    class CSVReport : IReport
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
                        var filepath = "../../data/CSV_Report.csv";
                        using (StreamWriter writer = new StreamWriter(new FileStream(filepath,
                        FileMode.Create, FileAccess.Write)))
                        {
                            string fields = String.Format("Name,Type,Species,IsCarnivorous,Zone");
                            writer.WriteLine(fields);
                            foreach (Plant p in result)
                            {
                                string info = String.Format("{0},{1},{2},{3},{4}", p.getName(), p.getType(), p.getSpecies(), p.getIsCarnivorous(), p.getGardenZone());
                                writer.WriteLine(info);
                            }
                        }

                        MessageBox.Show("CSV Report generated succesfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            
        }
    }
}
