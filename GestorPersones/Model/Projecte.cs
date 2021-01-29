using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPersones
{
    public class Projecte
    {


        private static List<Projecte> _projectes;

        public static List<Projecte> GetProjectes()
        {
            if (_projectes == null)
            {
                _projectes = new List<Projecte>();


                _projectes.Add(new Projecte(1, "A"));
                _projectes.Add(new Projecte(2, "B"));
                _projectes.Add(new Projecte(3, "C"));
            }
            return _projectes;
        }

        public Projecte(int pCodi, string pNom)
        {
            Codi = pCodi;
            Nom = pNom;
        }

        private int mCodi;

        public int Codi
        {
            get { return mCodi; }
            set { mCodi = value; }
        }


        private String mNom;

        public String Nom
        {
            get { return mNom; }
            set { mNom = value; }
        }


        private List<Empleat> mEmpleats;
        public void AddEmpleat(Empleat nou)
        {
            if (!mEmpleats.Contains(nou))
            {
                mEmpleats.Add(nou);
                nou.AddProjecte(this);
            }            
        }

        public void RemoveEmpleat(Empleat e)
        {
            if (mEmpleats.Contains(e))
            {
                mEmpleats.Remove(e);
               e.RemoveProjecte(this);
            }
        }

        public List<Empleat>.Enumerator GetEmpleats()
        {
            return mEmpleats.GetEnumerator();
        }

        public override bool Equals(object o)
        {
            if (o != null && o.GetType() == typeof(Projecte))
            {
                return ((Projecte)o).Codi.Equals(this.Codi);
            }
            return false;
        }
    }
}
