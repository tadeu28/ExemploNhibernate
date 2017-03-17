using ExNHibernate.DataBase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace ExNHibernate.DataBase.Repository
{
    public class PessoaRepository : RepositoryBase<Pessoa>
    {
        public PessoaRepository(ISession session) : base(session)
        {
        }
    }
}
