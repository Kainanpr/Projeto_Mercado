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
    class ClienteDAO : IClienteDAO
    {
        //Atributo responsavel por realizara conexão com o banco de dados
        private DataBase dataBase;

        //Atributo responsavel por realizar as operações de CRUD na pessoa no banco de dados
        private PessoaDAO pessoaDAO = new PessoaDAO();

        /* Método responsável pela iserção do cliente no bd. */
        public Cliente Create(Cliente cliente)
        {

            /* Guarda a pessoa no banco de dados 
             * (O metodo retorna a ultima pessoa inserida no banco já com seu id setado). 
             * Caso ocorra algum problema o metodo returna null, caso contrario returna o cliente */
            cliente = pessoaDAO.Create<Cliente>(cliente);

            //Caso a ultima pessoa a ser inserida tenha problemas o metodo já retorna null
            if (cliente == null)
            {
                return null;
            }

            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

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

                //Caso ocorra algum problema retorna null
                return null;
            }
            finally
            {
                //Independente se der erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            //Retorna o cliente cadastrado ja com seu id setado, ou seja, id gerado no banco
            return cliente;
        }

        /* Pesquisa um cliente pelo seu ID */
        public Cliente Read(int id)
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            //Variavel local responsável por armazenar o cliente pesquisado de acordo com seu ID
            //Chama o metodo generico da classe pessoaDAO
            Cliente cliente = pessoaDAO.Read<Cliente>(id);

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar um cliente pelo seu id */
                string query = "SELECT email FROM Cliente WHERE id = @Id;";

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
                    cliente = setCliente(dr, cliente);
                }
                else
                {
                    //Caso não encontre nenhum cliente retorna null
                    return null;
                }

            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);

                //Caso ocorra algum problema retorna null
                return null;
            }
            finally
            {
                //Independente se ocorrer erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna o endereco pesquisado de acordo com seu ID */
            return cliente;
        }

        public void Update(Cliente cliente)
        {
            /* Atualiza os dados da pessoa na tabela pessoa */
            pessoaDAO.Update<Cliente>(cliente);

            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por atualizar o cliente na tabela cliente do banco */
                string query = "UPDATE Cliente SET email = @Email WHERE id = @Id;";

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

        public void Delete(int id)
        {

            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por deletar o cliente na tabela cliente do banco */
                string query = "DELETE FROM Cliente WHERE id = @Id;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Id", MySqlDbType.Int32);

                //Atribuição de valores
                command.Parameters["@Id"].Value = id;

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

            /* Apaga os dados da pessoa na tabela pessoa */
            pessoaDAO.Delete(id);
        }

        public List<Cliente> ListAll()
        {

            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            /* Variavel local responsável por armazenar o todos os clientes vindas do banco */
            //Chama o metodo generico da classe pessoaDAO
            List<Cliente> clientes = pessoaDAO.ListAll<Cliente>();

            /* Variavel local responsável por armazenar o cliente pesquisado no banco de dados */
            Cliente cliente = new Cliente();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar uma pessoa pelo seu id */
                string query = "SELECT email FROM Cliente ORDER BY id";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Percorrer esse objeto até obter todos os dados
                if (dr.HasRows)
                {
                    //Contador para percorrer a lista
                    int count = 0;

                    while (dr.Read())
                    {
                        //Chama o metodo auxiliar para setar a Pessoa vindo do banco 
                        cliente = setCliente(dr, cliente);

                        //Percorre a lista e altera o email de cada cliente
                        clientes[count++].Email = cliente.Email;
                    }
                }
                else
                {
                    //Caso ocorra algum problema retorna null
                    return null;
                }

            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);

                //Caso ocorra algum problema retorna null
                return null;
            }
            finally
            {
                //Independente se ocorrer erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna todos os clientes pesquisados */
            return clientes;
        }


        public Cliente FindByCpf(string cpf) 
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            //Variavel local responsável por armazenar o cliente pesquisado de acordo com seu ID
            //Chama o metodo generico da classe pessoaDAO
            Cliente cliente = pessoaDAO.FindByCpf<Cliente>(cpf);

            //Caso não encontre a pessoa o metodo já retorna null
            if (cliente == null)
            {
                return null;
            }

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar um cliente pelo seu id */
                string query = "SELECT email FROM Cliente WHERE id = @Id;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros e espeficicação dos tipos
                command.Parameters.Add("@Id", MySqlDbType.Int32);

                //Atribuição de valores
                command.Parameters["@Id"].Value = cliente.Id;

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Verifica se tem dados para ser lido
                if (dr.Read())
                {
                    cliente = setCliente(dr, cliente);
                }
                else
                {
                    //Caso não encontre nenhum cliente retorna null
                    return null;
                }

            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);

                //Caso ocorra algum problema retorna null
                return null;
            }
            finally
            {
                //Independente se ocorrer erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna o endereco pesquisado de acordo com seu ID */
            return cliente; 
        }

        public List<Cliente> FindByName(string name) 
        {
            //Recupera a instancia unica do banco de dados
            dataBase = DataBase.GetInstance();

            /* Variavel local responsável por armazenar o todos os clientes vindas do banco */
            //Chama o metodo generico da classe pessoaDAO
            List<Cliente> clientes = pessoaDAO.FindByName<Cliente>(name);

            /* Variavel local responsável por armazenar o cliente pesquisado no banco de dados */
            Cliente cliente = new Cliente();

            try
            {
                //Tenta abrir a conexao
                dataBase.AbrirConexao();

                /* Query responsavel por buscar um cliente pelo seu nome */
                string query = "SELECT c.email FROM Pessoa p "
                                + "LEFT JOIN Endereco e ON p.idEndereco = e.id "
                                + "INNER JOIN Cliente c ON p.id = c.id "
                                + "WHERE lower(p.nome) LIKE @Name ORDER BY p.id;";

                //Comando responsavel pela query
                MySqlCommand command = new MySqlCommand(query, dataBase.GetConexao());

                //Adição de parametros ja com valor
                command.Parameters.AddWithValue("@Name", "%" + name + "%");

                //Executar instrução com retorno de dados, retorna objeto do tipo MySqlDataReader
                MySqlDataReader dr = command.ExecuteReader();

                //Percorrer esse objeto até obter todos os dados
                if (dr.HasRows)
                {
                    //Contador para percorrer a lista
                    int count = 0;

                    while (dr.Read())
                    {
                        //Chama o metodo auxiliar para setar a Pessoa vindo do banco 
                        cliente = setCliente(dr, cliente);

                        //Percorre a lista e altera o email de cada cliente
                        clientes[count++].Email = cliente.Email;
                    }
                }
                else
                {
                    //Caso ocorra algum problema retorna null
                    return null;
                }

            }
            //Caso ocorra algum tipo de exceção será tratado aqui.
            catch (MySqlException ex)
            {
                //Mostrar o erro na tela
                MessageBox.Show("Erro: " + ex.Message);

                //Caso ocorra algum problema retorna null
                return null;
            }
            finally
            {
                //Independente se ocorrer erro ou não a conexão com o banco de dados será fechada
                dataBase.FecharConexao();
            }

            /* Retorna todos os clientes pesquisados */
            return clientes; 
        }

        /* Metodo auxiliar responsavel por setar os dados da pessoa vindo do dados do banco
         * (Método generico) */
        private Cliente setCliente(MySqlDataReader dr, Cliente cliente)
        {

            cliente.Email = dr.IsDBNull(0) == false ? dr.GetString(0) : null;

            return cliente;
        }


    }//Fim da classe
}//Fim da interface