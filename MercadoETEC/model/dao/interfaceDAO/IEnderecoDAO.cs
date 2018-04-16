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
    interface IEnderecoDAO
    {
        void Create(Endereco endereco);
        Endereco Read(int id);
        void Update(Endereco endereco);
        void Delete(int id);
        List<Endereco> ListAll();
        List<Endereco> FindByNameRua(string nameRua);
        Endereco FindByCep(int cep);
        void DeleteByCep(int cep);

    }//Fim da interface
}//Fim da namespace

