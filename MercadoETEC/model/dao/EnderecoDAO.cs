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
    class EnderecoDAO : IEnderecoDAO
    {
        /* Método responsável pela iserção do endereço no bd.
         * Esse método retorna um objeto do tipo Endereco que representa o ultimo Endereco
         * inserido no banco, para que esse endereço
         * possa ser associado a pessoa correta*/
        public Endereco Create(Endereco endereco)
        {
            //Recupera a instancia unica do banco de dados
            DataBase dataBase = DataBase.GetInstance();

            //Variavel local responsável por armazenar o ultimo endereco inserido na tabela Endereco
            Endereco ultimoEndereco = new Endereco();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Utilizamos uma Stored Procedure para retornar o ultimo endereco inserido na tabela Endereco. 
                 * Essa procedure foi feita pelo fato de o id ser auto_increment, sendo assim, sem o uso de procedure
                 * não teriamos o controle de qual id foi inserido por ultimo. */
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
                    ultimoEndereco.Id = dr.GetInt32(0);
                    ultimoEndereco.Rua = dr.GetString(1);
                    ultimoEndereco.Numero = dr.GetInt32(2);
                    ultimoEndereco.Cep = dr.GetInt32(3);
                    ultimoEndereco.Cidade = dr.GetString(4);
                    ultimoEndereco.Estado = dr.GetString(5);
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

            /* Retorna o ultimo endereco inserido na tabela Endereco 
             * para que possa ser asociado a Pessoa correta
             * (Esse retorno se torna necessario para controlar os ids da tabela, pois é um campo auto_increment) */
            return ultimoEndereco;
        }

        public Endereco Read(int id) { return null; }
        public void Update(Endereco endereco) { }
        public void Delete(int id) { }
        public List<Endereco> ListAll() { return null; }
        public List<Endereco> FindByNameRua(string nameRua) { return null; }
        public Endereco FindByCep(int cep) { return null; }
        public void DeleteByCep(int cep) { }


    }//Fim da classe
}//Fim da interface
