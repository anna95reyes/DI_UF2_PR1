using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GestorPersones;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class EmpleatTest
    {
        [TestMethod]
        public void TestConstructorSenseErrors()
        {
            //Amb totes les dades
            Empresa emp = new Empresa("Milà i Fontanals");
            DateTime d = new DateTime();
            d.AddDays(1);
            Empleat e = new Empleat(emp, "Anna Maria", "Reyes Bello", "47112681X", d);
            Assert.AreEqual(emp, e.EmpresaActual);
            Assert.AreEqual("Anna Maria", e.Nom);
            Assert.AreEqual("Reyes Bello", e.Cognoms);
            Assert.AreEqual("47112681X", e.NIF);
            Assert.AreEqual(d, e.DataIncorporacio);

            // Empresa nula
            e = new Empleat(null, "Anna Maria", "Reyes Bello", "47112681X", d);
            Assert.AreEqual(null, e.EmpresaActual);
            Assert.AreEqual("Anna Maria", e.Nom);
            Assert.AreEqual("Reyes Bello", e.Cognoms);
            Assert.AreEqual("47112681X", e.NIF);
            Assert.AreEqual(d, e.DataIncorporacio);

            
        }

        [TestMethod]
        public void TestConstructorNomIncorrecte()
        {
            bool testErroni = false;
            Empresa emp = new Empresa("Milà i Fontanals");
            DateTime d = new DateTime();
            d.AddDays(1);

            //Nom de l'empleat massa curt
            try
            {
                Empleat em = new Empleat(emp, "Aaa", "Reyes Bello", "47112681X", d);
                testErroni = true;
            } 
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (testErroni) Assert.Fail("Nom incorrecte");

            //Nom de l'empleat null
            try
            {
                Empleat em = new Empleat(emp, null, "Reyes Bello", "47112681X", d);
                testErroni = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (testErroni) Assert.Fail("Nom incorrecte");

        }


        [TestMethod]
        public void TestConstructorCognomIncorrecte()
        {
            bool testErroni = false;
            Empresa emp = new Empresa("Milà i Fontanals");
            DateTime d = new DateTime();
            d.AddDays(1);

            //Cogom de l'empleat massa curt
            try
            {
                Empleat em = new Empleat(emp, "Anna Maria", "R", "47112681X", d);
                testErroni = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (testErroni) Assert.Fail("Cogom incorrecte");

            //Cogom de l'empleat null
            try
            {
                Empleat em = new Empleat(emp, "Anna Maria", null, "47112681X", d);
                testErroni = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (testErroni) Assert.Fail("Cogom incorrecte");

        }

        [TestMethod]
        public void TestConstructorNIFIncorrecte()
        {
            bool testErroni = false;
            Empresa emp = new Empresa("Milà i Fontanals");
            DateTime d = new DateTime();
            d.AddDays(1);

            //NIF de l'empleat sense lletra
            try
            {
                Empleat em = new Empleat(emp, "Anna Maria", "Reyes Bello", "47112681", d);
                testErroni = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (testErroni) Assert.Fail("Cogom incorrecte");

            //NIF de l'empleat a cadena buida
            try
            {
                Empleat em = new Empleat(emp, "Anna Maria", "Reyes Bello", "", d);
                testErroni = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (testErroni) Assert.Fail("Cogom incorrecte");

            //NIF de l'empleat null
            try
            {
                Empleat em = new Empleat(emp, "Anna Maria", "Reyes Bello", null, d);
                testErroni = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (testErroni) Assert.Fail("Cogom incorrecte");

        }

        /*
/// <param name="pNIF">NIF de l'empleat, 8 numeros i 1 lletra.</param>
/// <param name="pDataIncorporacio">Data d'incorporacio a l'empresa. La data ha de ser superior a la data d'avui.</param>

*/

        [TestMethod]
        public void TestConstructorDataIncorporacioIncorrecte()
        {
            bool testErroni = false;
            Empresa emp = new Empresa("Milà i Fontanals");
            DateTime d = new DateTime();

            //Data d'incorporacio amb data d'avui
            try
            {
                Empleat em = new Empleat(emp, "Anna Maria", "Reyes Bello", "47112681", d);
                testErroni = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (testErroni) Assert.Fail("Cogom incorrecte");
        }

        [TestMethod]
        public void TestGetEmpleats()
        {
            List<Empleat> empleats = Empleat.GetEmpleats();
            Assert.AreEqual(empleats, Empleat.GetEmpleats());
        }

        [TestMethod]
        public void TestProjectes()
        {
            List<Projecte> projectes = Projecte.GetProjectes();
            Empresa emp = new Empresa("Milà i Fontanals");
            DateTime d = new DateTime();
            d.AddDays(1);
            Empleat e = new Empleat(emp, "Anna Maria", "Reyes Bello", "47112681X", d);
            e.AddProjecte(projectes[0]);

            Debug.Write(e.GetProjectes());
            List<Projecte>.Enumerator en = e.GetProjectes();
            int pc = 0;
            Projecte p;
            while (en.MoveNext())
            {
                p = en.Current;
                Assert.AreEqual(p, projectes[0]);
                pc++;
            }
            Assert.AreEqual(1, pc);
            List<Projecte> projecteOnTreballo = new List<Projecte>();
            projecteOnTreballo.Add(projectes[0]);
            // comparació de les llistes comparant item per item amb Equals()
            Assert.IsTrue(projecteOnTreballo.SequenceEqual(e.ProjectesOnTreballo));
            Assert.IsFalse(projecteOnTreballo.SequenceEqual(e.ProjectesOnNoTreballo));
        }

        [TestMethod]
        public void TestAddProjectes()
        {
            List<Projecte> projectes = Projecte.GetProjectes();
            Empresa emp = new Empresa("Milà i Fontanals");
            DateTime d = new DateTime();
            d.AddDays(1);
            Empleat e = new Empleat(emp, "Anna Maria", "Reyes Bello", "47112681X", d);
            e.AddProjecte(projectes[0]);
            e.AddProjecte(projectes[1]);
            
            List<Projecte>.Enumerator en = e.GetProjectes();
            int pc = 0;
            Projecte p;
            while (en.MoveNext())
            {
                p = en.Current;
                Assert.AreEqual(p, projectes[pc]);
                pc++;
            }
            Assert.AreEqual(2, pc);
        }

        [TestMethod]
        public void TestRemoveProjectes()
        {
            Empleat e = Empleat.GetEmpleats()[0];
            List<Projecte> projecteOnTreballo = new List<Projecte>();
            List<Projecte>.Enumerator en = e.GetProjectes();
            int i = 0;
            while (en.MoveNext())
            {
                projecteOnTreballo.Add(en.Current);
                projecteOnTreballo[i].AddEmpleat(e);
                i++;
            }

            e.RemoveProjecte(projecteOnTreballo[0]);
            projecteOnTreballo.Remove(projecteOnTreballo[0]);

            en = e.GetProjectes();
            i = 0;
            while (en.MoveNext())
            {
                i++;
            }

            Assert.AreEqual(1, i);
           
        }

        [TestMethod]
        public void TestEmpleatEquals()
        {
            Empleat e1 = Empleat.GetEmpleats()[0];
            Empleat e2 = Empleat.GetEmpleats()[0];
            Empleat e3 = Empleat.GetEmpleats()[1];
            Empleat e4 = null;
            Assert.AreEqual(true, e1.Equals(e2));
            Assert.AreEqual(false, e1.Equals(e3));
            Assert.AreEqual(false, e1.Equals(e4));
        }
    }
}


