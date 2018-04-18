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
        //Atributo responsavel por realizara conexão com o banco de dados
        private DataBase dataBase;

        /* Método responsável pela iserção do endereço no bd.
         * Esse método retorna um objeto do tipo Endereco que representa o ultimo Endereco
         * inserido no banco, para que esse endereço
         * possa ser associado a pessoa correta*/
        public Endereco Create(Endereco endereco)
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

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

                //Chama o metodo auxiliar para setar o Endereco vindo do banco 
                ultimoEndereco = setEndereco(dr, ultimoEndereco);
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

        /* Pesquisa um endereco pelo seu ID */
        public Endereco Read(int id) 
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            //Variavel local responsável por armazenar o endereco pesquisado de acordo com seu ID
            Endereco endereco = new Endereco();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar um endereco pelo seu id */
                string query = "SELECT * FROM Endereco WHERE id = @Id";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Id", MySqlDbType.Int32);

                //Atribuição de valores
                command.Parameters["@Id"].Value = id;

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Chama o metodo auxiliar para setar o Endereco vindo do banco 
                endereco = setEndereco(dr, endereco);
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

            /* Retorna o endereco pesquisado de acordo com seu ID */
            return endereco;
        }

        public void Update(Endereco endereco) { }
        public void Delete(int id) { }
        public List<Endereco> ListAll() { return null; }
        public List<Endereco> FindByNameRua(string nameRua) { return null; }
        public Endereco FindByCep(int cep) { return null; }
        public void DeleteByCep(int cep) { }


        //Metodo auxiliar para preencher o endereço com os dados do banco
        private Endereco setEndereco(MySqlDataReader dr, Endereco endereco)
        {
            //Verifica se tem dados para ser lido
            if(dr.Read())
            {
                endereco.Id = dr.GetInt32(0);
                endereco.Rua = dr.GetString(1);
                endereco.Numero = dr.GetInt32(2);
                endereco.Cep = dr.GetInt32(3);
                endereco.Cidade = dr.GetString(4);
                endereco.Estado = dr.GetString(5);
            }
            else
            {
                /*Caso não tenha nenhum dado para ser lido irá lançar uma 
                 *exceção para ser recuperada posteriormente no controler */
                throw new Exception("Endereço não encontrado");
            }

            return endereco;
        }

    }//Fim da classe
}//Fim da interface
