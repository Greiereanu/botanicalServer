using ClientServer.Model;
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
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1.Controller
{
    class CLogin
    {
        private LogIn loginView;
        private IEmployeePersistanceDB employeePersistance;
        int language = 1;
        private void createConnection()
        {
            ChannelFactory<IEmployeePersistanceDB> employeeChannel;
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
            employeeChannel = new ChannelFactory<IEmployeePersistanceDB>(tcp, "net.tcp://" + s + ":52001/Employees");
            try
            {
                this.employeePersistance = employeeChannel.CreateChannel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void handleEvents()
        {
            this.loginView.exitButton().Click += new EventHandler(exitApp);
            this.loginView.loginButton().Click += new EventHandler(loginEmp);
        }

        public LogIn accessLoginView()
        {
            return this.loginView;
        }
        public void changeLanguage(int languageID)
        {
            language = languageID;
            switch (languageID)
            {
                case 1:
                    this.loginView.exitButton().Text = "Iesire";
                    this.loginView.loginButton().Text = "Logare";
                    this.loginView.accountLabel().Text = "Cont";
                    this.loginView.passwordLabel().Text = "Parola";
                    break;

                case 2:
                    this.loginView.exitButton().Text = "Exit";
                    this.loginView.loginButton().Text = "Log In";
                    this.loginView.accountLabel().Text = "Account";
                    this.loginView.passwordLabel().Text = "Password";
                    break;

                case 3:
                    this.loginView.exitButton().Text = "Sortir";
                    this.loginView.loginButton().Text = "Connexion";
                    this.loginView.accountLabel().Text = "Compte";
                    this.loginView.passwordLabel().Text = "Devise";
                    break;

                case 4:
                    this.loginView.exitButton().Text = "Ausfahrt";
                    this.loginView.loginButton().Text = "Anmeldung";
                    this.loginView.accountLabel().Text = "Konto";
                    this.loginView.passwordLabel().Text = "Passwort";
                    break;
            }
        }
        public CLogin(int languageID)
        {
            this.loginView = new LogIn();
            this.createConnection();
            this.handleEvents();
            this.changeLanguage(languageID);
        }

        private void loginEmp(object sender, EventArgs e)
        {
            Employee emp = employeePersistance.findEmployee(this.loginView.accountTxt().Text);

            if (emp.getAccount() == "not" || emp.getPassword() != this.loginView.passwordTxt().Text)
            {
                MessageBox.Show("Incorrect username or password");
            }
            else
            {
                if (emp.getRole() == "admin")
                {
                    this.accessLoginView().Hide();
                    CAdmin adminPage = new CAdmin(language);
                    adminPage.accessAdminView().Show();
                }
                else
                {
                    this.accessLoginView().Hide();
                    CEmployee employeePage = new CEmployee(language);
                    employeePage.accessEmployeeView().Show();
                }
            }

        }

        private void exitApp(object sender, EventArgs e)
        {
            this.loginView.Hide();
            CVisitor returnPage = new CVisitor(language);
            returnPage.accessVisitorView().ShowDialog();
        }
    }
}
