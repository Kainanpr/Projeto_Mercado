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
    //Implementa a interface ITelefoneDAO, sendo assim, é obrigado a implementar seus métodos abstratos
    class TelefoneDAO : ITelefoneDAO
    {
        //Atributo responsavel por realizara conexão com o banco de dados
        private DataBase dataBase;

        /* Método responsável pela iserção do telefone no bd. */
        public void Create(Telefone telefone)
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                //Query responsavel pela inserção de um telefone
                string query = "INSERT INTO Telefone(id, numero) VALUES(@Id, @Numero)";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Id", MySqlDbType.Int32);
                command.Parameters.Add("@Numero", MySqlDbType.String);

                //Atribuição de valores
                command.Parameters["@Id"].Value = telefone.Id;
                command.Parameters["@Numero"].Value = telefone.Numero;

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

        /* Pesquisa um telefone pelo seu ID */
        public Telefone Read(int id)
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            //Variavel local responsável por armazenar o telefone pesquisado de acordo com seu ID
            Telefone telefone = new Telefone();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar um telefone pelo seu id */
                string query = "SELECT * FROM Telefone WHERE id = @Id;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Id", MySqlDbType.Int32);

                //Atribuição de valores
                command.Parameters["@Id"].Value = id;

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Verifica se tem dados para ser lido
                if (dr.Read())
                {
                    telefone.Id = dr.IsDBNull(0) == false ? dr.GetInt32(0) : 0;
                    telefone.Numero = dr.IsDBNull(1) == false ? dr.GetString(1) : null;
                }
                else
                {
                    /*Caso não tenha nenhum dado para ser lido irá lançar uma 
                     *exceção para ser recuperada posteriormente no controler */
                    throw new Exception("Telefone não encontrado");
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

            /* Retorna o endereco pesquisado de acordo com seu ID */
            return telefone;
        }

        public void Update(Telefone telefone) { }
        public void Delete(int id) { }
        public List<Telefone> ListAll() { return null; }
        public List<Telefone> FindByNumero(int numero) { return null; }
        public void DeleteByNumero(int numero) { }

    }//Fim da classe
}//Fim da interface
