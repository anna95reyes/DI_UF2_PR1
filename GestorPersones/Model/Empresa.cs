using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPersones
{
    public class Empresa
    {
        public Empresa(string pNom)
        {
            Nom = pNom;
        }

        private String mNom;

        public String Nom
        {
            get { return mNom; }
            set { mNom = value; }
        }

        public override string ToString()
        {
            return Nom;
        }
    }
}
