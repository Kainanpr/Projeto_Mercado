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
    //Implementa a interface IEnderecoDAO, sendo assim, é obrigado a implementar seus métodos abstratos
    class EnderecoDAO
    {
        /* Método responsável pela iserção do endereço no bd.
         * Esse método retorna um inteiro que representa o ultimo id
         * de endereco inserido no banco, para que esse endereço
         * possa ser associado a pessoa correta*/
        public int Create(Endereco endereco)
        {
            //Recupera a instancia unica do banco de dados
            DataBase dataBase = DataBase.GetInstance();

            //Variavel local responsável por armazenar o ultimo id inserido na tabela Endereco
            int ultimoId = 0;

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                //Utilizamos uma Stored Procedure para retornar o ultimo id inserido na tabela Endereco
                string query = "CALL sp_endereco_insert (@Rua, @Numero, @Cep, @Cidade, @Estado)";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Rua", MySqlDbType.String);
                command.Parameters.Add("@Numero", MySqlDbType.Int32);
                command.Parameters.Add("@Cep", MySqlDbType.Int32);
                command.Parameters.Add("@Cidade", MySqlDbType.String);
                command.Parameters.Add("@Estado", MySqlDbType.String);

                //Atribuição de valores
                command.Parameters["@Rua"].Value = endereco.Rua;
                command.Parameters["@Numero"].Value = endereco.Numero;
                command.Parameters["@Cep"].Value = endereco.Cep;
                command.Parameters["@Cidade"].Value = endereco.Cidade;
                command.Parameters["@Estado"].Value = endereco.Estado;

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Percorrer esse objeto até obter todos os dados
                while(dr.Read())
                {
                    ultimoId = dr.GetInt32(0);
                }
            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);
            }
            finally
            {
                //Independente se ocorrer erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna o ultimo id inserido na tabela Endereco 
             * para que possa ser asociado a Pessoa correta */
            return ultimoId;
        }

    }//Fim da classe
}//Fim da interface
