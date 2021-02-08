using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPersones
{
    public class Empleat
    {
        private static List<Empleat> _empleats;

        /// <summary>
        ///     #Llistat Empleats
        ///     Llsta d'empleats amb la empresa actual i la data d'incorporació.
        /// </summary>
        /// <returns>
        ///     Llista d'empleats.
        /// </returns>
        public static List<Empleat> GetEmpleats()
        {
            if (_empleats == null)
            {
                _empleats = new List<Empleat>();
                Empresa empresa = new Empresa("IES Milà");
                DateTime hora = DateTime.Now;
                hora = hora.AddDays(1);

                _empleats.Add(new Empleat(empresa, "Paco", "Jones", "11111111H", hora));
                _empleats.Add(new Empleat(empresa, "Ester", "Minator", "22222222J", hora));
                _empleats.Add(new Empleat(empresa, "Toni", "Casue", "33333333P", hora));
                _empleats.Add(new Empleat(empresa, "Ester", "Colero", "44444444A", hora));

                _empleats[0].AddProjecte(Projecte.GetProjectes()[0]);
                _empleats[0].AddProjecte(Projecte.GetProjectes()[1]);
                _empleats[1].AddProjecte(Projecte.GetProjectes()[0]);
                _empleats[2].AddProjecte(Projecte.GetProjectes()[1]);

            }
            return _empleats;
        }

        /// <summary>
        /// Constructor d'un empleat.
        ///     
        /// ## Exemple constructor
        /// <code language="csharp" region="exempleConstructor" source="..\Model\Empleat.cs"></code>
        /// </summary>
        /// <param name="pEmpresa">Empresa en la que treballa actualment.</param>
        /// <param name="pNom">Nom de l'empleat, es obligatori amb longitud minima de 4 caracters.</param>
        /// <param name="pCognoms">Cognoms de l'empleat, es obligatori amb longitud minima de 4 caracters.</param>
        /// <param name="pNIF">NIF de l'empleat, 8 numeros i 1 lletra.</param>
        /// <param name="pDataIncorporacio">Data d'incorporacio a l'empresa. La data ha de ser superior a la data d'avui.</param>

        public Empleat(Empresa pEmpresa, String pNom, String pCognoms, String pNIF, DateTime pDataIncorporacio)
        {
            EmpresaActual = pEmpresa;
            Nom = pNom;
            Cognoms = pCognoms;
            NIF = pNIF;
            DataIncorporacio = pDataIncorporacio;
            mProjectesOnTreballo = new List<Projecte>();
        }

        private Empresa mEmpresaActual;


        private String mNom;

        /// <summary>
        /// Nom de l'empleat
        /// Salta una Exception si el nom no es valid
        /// </summary>
        public String Nom
        {
            get { return mNom; }
            set
            {
                if (!validaNomICognoms(value)) throw new Exception("El nom es obligatori i minim de 4 caràcters.");
                mNom = value;
            }
        }

        private String mCognoms;

        /// <summary>
        /// Cognoms de l'empleat
        /// Salta una Exception si el cognom no es valid
        /// </summary>
        public String Cognoms
        {
            get { return mCognoms; }
            set
            {
                if (!validaNomICognoms(value)) throw new Exception("El cognom es obligatori i minim de 4 caràcters.");
                mCognoms = value;
            }
        }

        private String mNIF;

        /// <summary>
        /// NIF de l'empleat
        /// Salta una Exception si el NIF no es valid
        /// </summary>
        public String NIF
        {
            get { return mNIF; }
            set
            {
                if (!validaNif(value)) throw new Exception("Format del NIF incorrecte.");
                mNIF = value;
            }
        }

        private List<Projecte> mProjectesOnTreballo;

        /// <summary>
        /// Llista dels Projectes on treballa l'empleat
        /// </summary>
        public List<Projecte> ProjectesOnTreballo
        {
            get { return mProjectesOnTreballo; }
        }

        /// <summary>
        /// Llista dels Projectes on no treballa l'empleat
        /// </summary>
        public List<Projecte> ProjectesOnNoTreballo
        {
            get
            {
                List<Projecte> all = new List<Projecte>();
                all.AddRange(Projecte.GetProjectes());
                all.RemoveAll(p => mProjectesOnTreballo.Contains(p));
                return all;
            }

        }

        /// <summary>
        /// Numero de projectes assignats.
        /// </summary>
        /// <returns></returns>
        public List<Projecte>.Enumerator GetProjectes()
        {
            return mProjectesOnTreballo.GetEnumerator();
        }

        /// <summary>
        /// Assignar un nou projecte a un empleat.
        /// </summary>
        /// <param name="p"></param>
        public void AddProjecte(Projecte p)
        {
            if (!mProjectesOnTreballo.Contains(p))
            {
                mProjectesOnTreballo.Add(p);
            }
        }

        /// <summary>
        /// Desasignar un projecte a un projecte.
        /// </summary>
        /// <param name="p"></param>
        public void RemoveProjecte(Projecte p)
        {
            if (mProjectesOnTreballo.Contains(p))
            {
                mProjectesOnTreballo.Remove(p);
                p.RemoveEmpleat(this);
            }
        }

        private DateTime mDataIncorporacio;

        /// <summary>
        /// Data d'incorporació a l'empresa
        /// </summary>
        public DateTime DataIncorporacio
        {
            get { return mDataIncorporacio; }
            set
            {
                validaDataEntrada(value);
                mDataIncorporacio = value;
            }
        }

        /// <summary>
        /// Empesa Actual
        /// </summary>
        public Empresa EmpresaActual
        {
            get { return mEmpresaActual; }
            set { mEmpresaActual = value; }
        }

        /// <summary>
        /// Validacio del NIF amb la lletra correcta
        /// </summary>
        /// <param name="data">NIF en format text</param>
        /// <returns>
        /// Si no pot llegir una lletra retorna una Excepcio.
        /// </returns>
        public Boolean validaNif(String data)
        {
            if (data == String.Empty)
                return false;
            try
            {
                String letra;
                letra = data.Substring(data.Length - 1, 1);
                data = data.Substring(0, data.Length - 1);
                int nifNum = int.Parse(data);
                int resto = nifNum % 23;
                String tmp = getLetra(resto);
                if (tmp.ToLower() != letra.ToLower())
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        private String getLetra(int id)
        {
            Dictionary<int, String> letras = new Dictionary<int, String>();
            letras.Add(0, "T");
            letras.Add(1, "R");
            letras.Add(2, "W");
            letras.Add(3, "A");
            letras.Add(4, "G");
            letras.Add(5, "M");
            letras.Add(6, "Y");
            letras.Add(7, "F");
            letras.Add(8, "P");
            letras.Add(9, "D");
            letras.Add(10, "X");
            letras.Add(11, "B");
            letras.Add(12, "N");
            letras.Add(13, "J");
            letras.Add(14, "Z");
            letras.Add(15, "S");
            letras.Add(16, "Q");
            letras.Add(17, "V");
            letras.Add(18, "H");
            letras.Add(19, "L");
            letras.Add(20, "C");
            letras.Add(21, "K");
            letras.Add(22, "E");
            return letras[id];
        }

        /// <summary>
        /// Validacio del Nom i el Cognom, han de tenir una longitud minima de 4 caraceters.
        /// </summary>
        /// <param name="text">El nom o Cognom</param>
        /// <returns>Retorna fals si el Nom o el Cognom no son correctes.</returns>
        public Boolean validaNomICognoms(String text)
        {
            return text.Trim().Length >= 4;
        }

        /// <summary>
        /// Validacio de data d'Incorporacio que ha de ser posterior a la data actual.
        /// </summary>
        /// <param name="data">Data de la incorporació.</param>
        /// <returns>Retorna fals si la data no es valida.</returns>
        public Boolean validaDataEntrada(DateTime data)
        {
            return data > DateTime.Now;
        }

        /// <summary>
        /// Dos empleats son iguals si tenen el mateix NIF.
        /// </summary>
        /// <param name="o">Empleat amb el que es vol fer la comparació</param>
        /// <returns>Retorna fals si no son iguals.</returns>
        public override bool Equals(object o)
        {
            if (o != null && o.GetType() == typeof(Empleat))
            {
                return ((Empleat)o).NIF.Equals(this.NIF);
            }
            return false;
        }

        internal Empleat Clonar()
        {
            Empleat e = new GestorPersones.Empleat(this.EmpresaActual, this.Nom, this.Cognoms, this.NIF, this.DataIncorporacio);

            // ens assegurem de clonar també la llista de projectes....
            // així no la liem !
            e.mProjectesOnTreballo.AddRange(this.mProjectesOnTreballo);

            return e;

        }

        #region exempleConstructor
        private static void exempleConstructor()
        {
            Empresa empresa = new Empresa("IES Milà");
            DateTime dataIncorporacio = new DateTime(2021, 2, 28);

            Empleat e = new Empleat(empresa, "Anna Mª", "Reyes Bello", "47112681X", dataIncorporacio);
        }
        #endregion exempleConstructor
    }
}
