using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MercadoETEC.model.dao;
using MercadoETEC.model.domain;

using MercadoETEC.model.service.exception;

namespace MercadoETEC.model.service
{
    /* Classe responsavel por ter as regras de negócio relacionadas ao DAO 
     * (Grava todos os dados do cliente em seus respectivos DAOs)*/
    class ClienteService
    {
        //Atributo responsavel por realizar as operações de CRUD no cliente no banco de dados
        private ClienteDAO clienteDAO = new ClienteDAO();

        //Atributo responsavel por realizar as operações de CRUD na endereco no banco de dados
        private EnderecoDAO enderecoDAO = new EnderecoDAO();

        public Cliente Create(Cliente cliente)
        {
            /* Para registrar uma pessoa no banco é necessario ter um endereço cadastrado primeiro  
               porque na tabela pessoa possui um atributo idEndereco como chave estrangeira */

            /* Grava o endereço do cliente no banco de dados e retorna o ultimo endereco inserido no banco 
             * ja com seu id setado para ser associado ao cliente correto */
            cliente.Endereco = enderecoDAO.Create(cliente.Endereco);

            /* Guardar o cliente no banco de dados e retorna o ultimo cliente inserido ja com seu id setado. */
            /* Caso ocorra algum erro na gravação da pessoa o metodo retorna null. */
            /* Pode ocorrer problemas de UNIQUE e NOT NULL na coluna CPF. */
            Cliente ultimoClienteInseridoBD = clienteDAO.Create(cliente);

            //Verifica se o cliente é null, se retornar null não foi salvo no banco de dados
            if (ultimoClienteInseridoBD == null)
            {
                /* Se ocorrer problema na gravação do cliente devemos excluir
                 * o endereço que foi gravado anteriormente, pois o endereço 
                 * não foi asociado ao cliente correto */
                enderecoDAO.Delete(cliente.Endereco.Id);

                /* Lança nossa propria exeção para dizer ao usuario que não foi possivel cadastrar o cliente. 
                 * Essa exeção é necessaria pois não temos controle se o usuario irá digitar um CPF 
                 * que já está cadastrado */
                throw new ObjetoNotCreatedException("Cliente não foi cadastrado, digite um CPF válido!");

            }
            else
            {              
                /* Caso não retornar null associa o Id do ultimo cliente inserido
                 * com o cliente que foi enviado para o metodo */
                cliente.Id = ultimoClienteInseridoBD.Id;               
            }

            /* Esse return pode não ser utilizado. 
             * Se caso quem for utilizar o metodo precisar do cliente ja setado com seu Id 
             * para futuras associações terá esse Return a sua disposição. */
            return cliente;
        }

        public Cliente Read(int id)
        {
            //Delega a pesquisa do cliente para o DAO correspondente
            Cliente cliente = clienteDAO.Read(id);

            //Caso não encontre nenhum cliente será lançado a exceção que nos criamos
            if (cliente == null)
                throw new ObjetoNotFoundException("Cliente não encontrado");

            //Retorna o cliente pesquisado
            return cliente;
        }

        public void Update(Cliente cliente)
        {
            //Atualiza o cliente
            clienteDAO.Update(cliente);

            //Atualiza o endereço (Verifica se a pessoa possui um endereco)
            if (cliente.Endereco != null)
                enderecoDAO.Update(cliente.Endereco);        

        }

        public void Delete(Cliente cliente)
        {

            //Delete o cliente do banco
            clienteDAO.Delete(cliente.Id);

            //Verifica se o cliente tem endereço e deleta o endereco do banco
            if (cliente.Endereco != null)
                enderecoDAO.Delete(cliente.Endereco.Id);
        }

        public List<Cliente> ListAll()
        {
            //Lista todos os clientes
            List<Cliente> clientes = clienteDAO.ListAll();

            //Caso não encontre nenhum cliente será lançado a exceção que nos criamos
            if (clientes == null)
                throw new ObjetoNotFoundException("Clientes não encontrados");
            

            return clientes;
        }

        public List<Cliente> FindByName(string name)
        {
            //Lista todos os clientes
            List<Cliente> clientes = clienteDAO.FindByName(name);

            //Caso não encontre nenhum cliente será lançado a exceção que nos criamos
            if (clientes == null)          
                throw new ObjetoNotFoundException("Cliente não encontrado");
            

            return clientes;
        }

        public Cliente FindByCpf(string cpf)
        {
            //Delega a pesquisa do cliente para o DAO correspondente
            Cliente cliente = clienteDAO.FindByCpf(cpf);

            //Caso não encontre nenhum cliente será lançado a exceção que nos criamos
            if (cliente == null)
                throw new ObjetoNotFoundException("Cliente não encontrado");


            return cliente;
        }

    }//Fim da classe
}//Fim da namespace