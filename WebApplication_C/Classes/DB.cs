﻿using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using WebApplication_C.Classes;
using System.Runtime.ConstrainedExecution;
using System.Web.Helpers;

namespace WebApplication_C.Classes
{
    public static class DB
    {
        private static MySqlConnection conn = null;
        private static string server = "";
        private static string database = "";
        private static string uid = "";
        private static string password = "";


        //Constructor
        static DB()
        {
            Initialize();
        }

        //Initialize values
        private static void Initialize()
        {
            //server = "localhost";
            server = "10.200.116.197";
            //database = "connectcsharptomysql";
            database = "loja-online";
            //uid = "username";
            uid = "admin";
            //password = "password";
            password = "senai";
            string myConnectionString;
            myConnectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = myConnectionString;
                //conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }

        //open connection to database
        private static bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }

        }

        //Close connection
        private static bool CloseConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        //Inserir Usuario no BD
        public static void Insert_Usuario(Usuario Usuario)
        {
            string query = "INSERT INTO Usuarios (cpf,nome,sobrenome,senha,email,genero,enderecoRua,cep,telefone,admin,enderecoNumero,enderecoComplemento) VALUES('" + Usuario.CPF + "','" + Usuario.Nome + "','" + Usuario.Sobrenome + "','" + Usuario.Senha + "','" + Usuario.Email + "','" + Usuario.Genero + "','" + Usuario.EnderecoRua + "','" + Usuario.CEP + "','" + Usuario.Telefone + "','" + Usuario.Admin + "','" + Usuario.EnderecoNumero + "','" + Usuario.EnderecoComplemento + "')";

            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        //Atualizar o Nome de uma Usuario atravéz do cpf
        public static void Update_Nome(string nome, string cpf)
        {
            string query = "UPDATE Usuarios SET nome='" + nome + "' WHERE cpf='" + cpf + "'";

            //Open connection
            if (OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = conn;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        //Deletar uma Usuario
        public static void Delete(string cpf)
        {
            string query = "DELETE FROM Usuarios WHERE cpf='" + cpf + "'";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        //Retorna os dados de todas as Usuarios do DB em uma Lista
        public static List<Usuario> Lista_Usuarios()
        {
            string query = "SELECT * FROM Usuarios";

            //Create a list to store the result
            List<Usuario> Usuarios = new List<Usuario>();

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    bool tem;
                    Boolean adm = Boolean.TryParse(dataReader["Admin"] + "", out tem);
                    Usuario Usuario = new Usuario(long.Parse(dataReader["cpf"] + ""), dataReader["Nome"] + "", dataReader["Sobrenome"] + "", dataReader["Senha"] + "", dataReader["Email"] + "", dataReader["Genero"] + "", dataReader["EnderecoRua"] + "", int.Parse(dataReader["EnderecoNumero"] + ""), dataReader["EnderecoComplemento"] + "", long.Parse(dataReader["CEP"] + ""), long.Parse(dataReader["Telefone"] + ""), adm);
                    Usuarios.Add(Usuario);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return Usuarios;
            }
            else
            {
                return Usuarios;
            }
        }

        //Retorna os dados de uma Usuario do DB
        public static Usuario Busca_Usuario(long cpf)
        {
            string query = "SELECT * FROM Usuarios WHERE cpf = '" + cpf + "'";
            Usuario Usuario = new Usuario();
            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    Usuario.CPF = long.Parse(dataReader["cpf"] + "");
                    Usuario.Nome = dataReader["nome"] + "";
                    Usuario.Sobrenome = dataReader["Sobrenome"] + "";
                    Usuario.Senha = dataReader["Senha"] + "";
                    Usuario.Email = dataReader["Email"] + "";
                    Usuario.Genero = dataReader["Genero"] + "";
                    Usuario.EnderecoRua = dataReader["EnderecoRua"] + "";
                    Usuario.EnderecoNumero = int.Parse(dataReader["EnderecoNumero"] + "");
                    Usuario.EnderecoComplemento = dataReader["EnderecoComplemento"] + "";
                    Usuario.CEP = long.Parse(dataReader["CEP"] + "");
                    Usuario.Telefone = long.Parse(dataReader["Telefone"] + "");
                    bool tem = false;
                    Boolean adm = Boolean.TryParse(dataReader["Admin"] + "", out tem);
                    Usuario.Admin = adm;
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return Usuario;
            }
            else
            {
                return Usuario;
            }
        }

        //Conta quantas Usuarios estão cadastradas no DB
        public static int Count_Usuario()
        {
            string query = "SELECT Count(*) FROM Usuarios";
            int Count = -1;

            //Open Connection
            if (OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }
        //Buscar CPF de Usuario
        public static string Get_CPF_Usuario(string nome)
        {
            string query = "SELECT cpf FROM Usuarios " +
                "WHERE nome ='" + nome + "'";
            String cpf_Usuario = "";

            //Open Connection
            if (OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //ExecuteScalar will return one value
                cpf_Usuario = cmd.ExecuteScalar() + "";

                //close Connection
                CloseConnection();

                return cpf_Usuario;
            }
            else
            {
                return cpf_Usuario;
            }
        }

    }
}