using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExNHibernate.DataBase.Model
{
    public class Pessoa
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string TipoDocumento { get; set; }
        public virtual string NumDocumento { get; set; }
        public virtual DateTime DtNascimento { get; set; }
        public virtual string Telefone { get; set; }
    }

    public class PessoaMap : ClassMapping<Pessoa>
    {
        public PessoaMap()
        {
            //PK da Tabela
            Id<int>(x => x.Id, 
                    m => {
                        m.Generator(Generators.Identity);
                    });

            Property<string>(x => x.Nome);
            Property<string>(x => x.TipoDocumento);
            Property<string>(x => x.NumDocumento);
            Property<DateTime>(x => x.DtNascimento);
            Property<string>(x => x.Telefone);
        }
    }
}
