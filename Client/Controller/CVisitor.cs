﻿using ClientServer.Model;
using Garden.Model;
using Garden.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1.Controller
{
    class CVisitor
    {
        private VisitorView visitorView;
        private IPlantPersistanceDB plantPersistance;
        int language = 1;
        private void handleEvents()
        {
            this.visitorView.exitButton().Click += new EventHandler(exitApp);
            this.visitorView.searchButton().Click += new EventHandler(findPlant);
            this.visitorView.filterButton().Click += new EventHandler(filterPlants);
            this.visitorView.statsButton().Click += new EventHandler(showStatistics);
            this.visitorView.loginButton().Click += new EventHandler(showLogin);
            this.visitorView.romanaButton().Click += new EventHandler(changeLanguage);
            this.visitorView.engButton().Click += new EventHandler(changeLanguage);
            this.visitorView.frButton().Click += new EventHandler(changeLanguage);
            this.visitorView.deButton().Click += new EventHandler(changeLanguage);
        }
        private void createConnection()
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ShowInfo()
        {
            List<Plant> result = plantPersistance.loadPlants();
            foreach (Plant plant in result)
                this.visitorView.table().Rows.Add(plant.getName(), plant.getType(), plant.getSpecies(), plant.getIsCarnivorous(), plant.getGardenZone());
        }

        public VisitorView accessVisitorView()
        {
            return this.visitorView;
        }
        public void changeLanguage(int languageID)
        {
            language = languageID;
            switch(languageID)
            {
                case 1:
                    this.visitorView.exitButton().Text = "Iesire";
                    this.visitorView.searchButton().Text = "Cautare";
                    this.visitorView.filterButton().Text = "Filtreaza";
                    this.visitorView.statsButton().Text = "Statistici";
                    this.visitorView.loginButton().Text = "Logare";
                    break;

                case 2:
                    this.visitorView.exitButton().Text = "Exit";
                    this.visitorView.searchButton().Text = "Search";
                    this.visitorView.filterButton().Text = "Filter";
                    this.visitorView.statsButton().Text = "Statistics";
                    this.visitorView.loginButton().Text = "Log In";
                    break;

                case 3:
                    this.visitorView.exitButton().Text = "Sortir";
                    this.visitorView.searchButton().Text = "Rechercher";
                    this.visitorView.filterButton().Text = "Filtre";
                    this.visitorView.statsButton().Text = "Statistiques";
                    this.visitorView.loginButton().Text = "Connexion";
                    break;

                case 4:
                    this.visitorView.exitButton().Text = "Ausfahrt";
                    this.visitorView.searchButton().Text = "Suche";
                    this.visitorView.filterButton().Text = "Filter";
                    this.visitorView.statsButton().Text = "Statistiken";
                    this.visitorView.loginButton().Text = "Anmeldung";
                    break;



            }
        }
        public void changeLanguage(object sender, EventArgs e)
        {
            switch((sender as Button).Text)
            {
                case "Romana":
                    this.changeLanguage(1);
                    break;
                case "English":
                    this.changeLanguage(2);
                    break;
                case "Français":
                    this.changeLanguage(3);
                    break;
                case "Deutsche":
                    this.changeLanguage(4);
                    break;
            }
        }
        
        public CVisitor(int languageID)
        {
            this.visitorView = new VisitorView();
            this.createConnection();
            this.handleEvents();
            this.ShowInfo();
            this.changeLanguage(languageID);
        }

        private void exitApp(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public void findPlant(object sender, EventArgs e)
        {
            Plant p = plantPersistance.findPlant(this.visitorView.searchText().Text);
            if (p.getName() == "not")
            {
                var info = String.Format("Plant {0} does not exist!", this.visitorView.searchText().Text);
                MessageBox.Show(info, "Plant Info");
            }
            else
            {
                MessageBox.Show(p.showInfo(), "Plant Info");
            }
        }

        public void filterPlants(object sender, EventArgs e)
        {
            List<Plant> filter = plantPersistance.filterPlants(this.visitorView.filterCmb().Text, this.visitorView.filterText().Text);
            this.visitorView.table().Rows.Clear();
            this.visitorView.table().Refresh();
            foreach (Plant plant in filter)
            {
                this.visitorView.table().Rows.Add(plant.getName(), plant.getType(), plant.getSpecies(), plant.getIsCarnivorous(), plant.getGardenZone());
            }
        }

        public void showStatistics(object sender, EventArgs e)
        {
            List<Plant> plants = plantPersistance.loadPlants();
            if (this.visitorView.statsCmb().Text == "Carnivorous")
            {
                int number1 = 0, number2 = 0;
                foreach (Plant p in plants)
                {
                    if (p.getIsCarnivorous() == "True")
                        number1++;
                    else
                        number2++;
                }
                Chart chart1 = this.visitorView.charts();
                chart1.Series.Clear();
                chart1.Legends.Clear();
                chart1.Legends.Add("MyLegend");
                chart1.Legends[0].LegendStyle = LegendStyle.Table;
                chart1.Legends[0].Docking = Docking.Bottom;
                chart1.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                chart1.Legends[0].Title = "Tipul plantelor";
                chart1.Legends[0].BorderColor = System.Drawing.Color.Black;

                //Add a new chart-series
                string seriesname = "MySeriesName";
                chart1.Series.Add(seriesname);
                //set the chart-type to "Pie"
                chart1.Series[seriesname].ChartType = SeriesChartType.Pie;


                chart1.Series[seriesname].Points.AddXY("Carnivore", number1);
                chart1.Series[seriesname].Points.AddXY("Non-Carnivore", number2);
                chart1.Series[seriesname].IsValueShownAsLabel = true;

                chart1.Visible = true;
            }
            if (this.visitorView.statsCmb().Text == "Zone")
            {
                int north = 0; int south = 0; int west = 0; int east = 0;
                foreach (Plant p in plants)
                {
                    if (p.getGardenZone() == "north")
                        north++;
                    if (p.getGardenZone() == "south")
                        south++;
                    if (p.getGardenZone() == "west")
                        west++;
                    if (p.getGardenZone() == "east")
                        east++;
                    Chart chart1 = this.visitorView.charts();
                    chart1.Series.Clear();
                    chart1.Legends.Clear();
                    chart1.Legends.Add("MyLegend");
                    chart1.Legends[0].LegendStyle = LegendStyle.Table;
                    chart1.Legends[0].Docking = Docking.Bottom;
                    chart1.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
                    chart1.Legends[0].Title = "Garden Zone";
                    chart1.Legends[0].BorderColor = System.Drawing.Color.Black;

                    string seriesname = "Zone";
                    chart1.Series.Add(seriesname);
                    //set the chart-type to "Pie"
                    chart1.Series[seriesname].ChartType = SeriesChartType.Column;

                    chart1.Series[seriesname].Points.AddXY("North", north);
                    chart1.Series[seriesname].Points.AddXY("South", south);
                    chart1.Series[seriesname].Points.AddXY("East", east);
                    chart1.Series[seriesname].Points.AddXY("West", west);
                    chart1.Series[seriesname].IsValueShownAsLabel = true;
                    chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
                    chart1.Visible = true;

                }
            }

        }

        public void showLogin(object sender, EventArgs e)
        {
            this.visitorView.Hide();
            CLogin loginView = new CLogin(language);
            loginView.accessLoginView().Show();
        }
    }
}

