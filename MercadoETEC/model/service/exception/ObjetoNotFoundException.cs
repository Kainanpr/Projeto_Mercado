using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercadoETEC.model.service.exception
{
    //Criando nossa propria excecao se caso algum objeto nao seja encontrado no banco de dados
    class ObjetoNotFoundException : Exception
    {
        //Repassa a mensagem do erro para o construtor da superclasse
        public ObjetoNotFoundException(String mensagem) : base(mensagem) { }

    }
}
