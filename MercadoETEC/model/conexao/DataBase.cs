using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace MercadoETEC.model.conexao
{
    class DataBase
    {
        //Responsavel por realizar a conexao (Atributo estático)
        private static MySqlConnection objConexao;

        //Padrao Singleton (Garantir apenas uma instancia da classe) (Atributo estático)
        private static DataBase instance;

        //Variaveis de configuração de acesso ao banco de dados
        private string server = "localhost";
        private string database = "MercadoETECs";
        private string usuario = "root";
        private string senha = "";

        //(Padrao Singleton) O construtor privado proíbe a livre criação de objetos da classe DataBase 
        private DataBase(){
            objConexao = new MySqlConnection("server=" + server + "; database=" + database 
                                                + ";user id=" + usuario + ";password=" + senha);
        }

        /* Método que constrói uma instância única da classe DataBase. Por ser estático,
         * esse método pertence a classe e pode ser chamado sem a necessidade da criação 
         * prévia de um objeto. */
        public static DataBase GetInstance()
        {
            if (instance == null)
                instance = new DataBase();

            return instance;
        }

        /* Função que retorna a conexão. Por não ser uma função estática, só pode ser 
         * acessada por um objeto da classe DataBase. */
        public MySqlConnection GetConexao()
        {
            return objConexao;
        }

        //Método responsável por abrir a conexão
        public void AbrirConexao()
        {
            objConexao.Open();
        }

        //Método responsável por fechar a conexão
        public void FecharConexao()
        {
            objConexao.Close();
        }

    }//Fim da classe
}//Fim do namespace
