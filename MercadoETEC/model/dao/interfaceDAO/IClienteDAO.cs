using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MercadoETEC.model.domain;

namespace MercadoETEC.model.dao.interfaceDAO
{
    /*Estabelece um contrato, ou seja, quem implementar essa interface 
     *terá que implementar seus metodos abstratos */
    interface IClienteDAO
    {
        Cliente Create(Cliente cliente);
        Cliente Read(int id);
        void Update(Cliente cliente);
        void Delete(int id);
        List<Cliente> ListAll();
        List<Cliente> FindByName(string name);

    }//Fim da interface
}//Fim da namespace