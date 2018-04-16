using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MercadoETEC.model.dao.interfaceDAO;
using MercadoETEC.model.conexao;
using MercadoETEC.model.domain;

using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MercadoETEC.model.dao
{
    //Implementa a interface IClienteDAO, sendo assim, é obrigado a implementar seus métodos abstratos
    class ClienteDAO
    {

        /* Método responsável pela iserção do cliente no bd. */
        public void Create(Cliente cliente)
        {
            //Recupera a instancia unica do banco de dados
            DataBase dataBase = DataBase.GetInstance();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                //Query responsavel pela inserção de um Cliente
                string query = "INSERT INTO Cliente(id, email) VALUES(@Id, @Email)";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Id", MySqlDbType.Int32);
                command.Parameters.Add("@Email", MySqlDbType.String);

                //Atribuição de valores
                command.Parameters["@Id"].Value = cliente.Id;
                command.Parameters["@Email"].Value = cliente.Email;

                //Executar instrução sem retorno de dados
                command.ExecuteNonQuery();

                //MessageBox.Show("Conexão com banco de dados efetuada com sucesso");
            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);
            }
            finally
            {
                //Independente se der erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }
        }

    }//Fim da classe
}//Fim da interface