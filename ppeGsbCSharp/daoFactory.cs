using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;

namespace ppeGsbCSharp
{
    class daoFactory
    {
        public SqlConnection connexionBDD;
        public string serveur;
        public string bdd;
        public string user;
        public string mdp;

        public daoFactory()
        {
            Initialize();
        }

        public void Initialize()
        {
            // Informations de connexion à la base de données
            serveur = "172.17.21.10";
            bdd = "TeamA_Piat";
            user = "piat";
            mdp = "btssio-2015";

            string connexionString = "SERVER=" + serveur + ";" + "DATABASE=" +
            bdd + ";" + "UID=" + user + ";" + "PASSWORD=" + mdp + ";";

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            
            String data = new String('a', 32);
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

            try
            {
               IPHostEntry entry = Dns.GetHostEntry("192.168.1.2");
                PingReply reply = pingSender.Send(entry.AddressList[0], timeout, buffer, options);
            
                if (reply.Status == IPStatus.Success)
                {
                    connexionBDD = new SqlConnection(connexionString);
                }
                else
                {
                    MessageBox.Show("Un problème de connexion avec la base de données est survenu. Merci de vérifiervotre connexion au réseau local et de réessayer.");
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("Un problème de connexion avec la base de données est survenu. Merci de vérifiervotre connexion au réseau local et de réessayer. Erreur : "+excep.ToString());
                
            }
             Application.Exit();
        }


        //Ouverture de la connexion à la base de données
        public bool OuvrirConnexion()
        {
            try
            {
                connexionBDD.Open();
                return true;
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Impossible de se connecter au serveur. Veuillez contacter l'administrateur");
                        break;

                    case 1045:
                        MessageBox.Show("Le couple nom d'utilisateur/mot de passe est invalide");
                        break;
                }
                return false;
            }

        }

        //Fermeture de la connexion à la base de données
        public bool CloseConnection()
        {
            try
            {
                connexionBDD.Close();
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        //execution requete
        public SqlDataReader execSql(String req)
        {
            SqlCommand maCommand;
            maCommand = new SqlCommand(req);
            maCommand.Connection = connexionBDD;
            SqlDataReader reader = maCommand.ExecuteReader();
            return reader;

        }

        public String testConnection(){
            System.Net.NetworkInformation.Ping google = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply reply = google.Send("www.google.com");
            return(reply.Status.ToString());
        }
    }
}
