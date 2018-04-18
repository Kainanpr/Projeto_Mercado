using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MercadoETEC.model.dao;
using MercadoETEC.model.domain;

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

        //Atributo responsavel por realizar as operações de CRUD no telefone no banco de dados
        private TelefoneDAO telefoneDAO = new TelefoneDAO();

        public Cliente Create(Cliente cliente)
        {
            /* Grava o endereço do cliente no banco de dados e retorna o ultimo endereco inserido no banco 
             * ja com seu id setado para ser associado ao cliente correto */
            cliente.Endereco = enderecoDAO.Create(cliente.Endereco);

            /* Guardar o cliente no banco de dados e retorna o ultimo cliente inserido ja com seu id setado*/
            cliente.Id = clienteDAO.Create(cliente).Id;

            /* Guardar os telefones do cliente no banco de dados */
            foreach(Telefone tel in cliente.Telefones)
            {
                //Associa o id do telefone com o id do cliente
                tel.Id = cliente.Id;

                //Grava o telefone do criente no banco
                telefoneDAO.Create(tel);
            }

            /* Retorna o ultimo Cliente cadastrado no banco ja com seu id setado. 
             * Talves esse id possa ser necessario posteriormente para alguma associação */
            return cliente;
        }

        public Cliente Read(int id)
        {
            //Delega a pesquisa do cliente para o DAO correspondente
            Cliente cliente = clienteDAO.Read(id);

            return cliente;
        }

    }
}
