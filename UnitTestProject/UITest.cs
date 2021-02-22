using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using GestorPersones;
using TestStack.White.UIItems.ListBoxItems;

namespace UnitTestProject
{
    [TestClass]
    public class UITest
    {
        private static Window StartApplicationAndGetWindow()
        {
            string ruta = System.AppDomain.CurrentDomain.BaseDirectory;
            ruta += @"\GestorPersones.exe";
            Console.WriteLine(">" + ruta);
            Application app = Application.Launch(ruta);
            Window w = app.GetWindows()[0];
            return w;
        }

        [TestMethod]
        public void SeleccioEmpleat()
        {
            Window w = StartApplicationAndGetWindow();
            ListView dgrEmpleats = w.Get<ListView>("dgrEmpleats");
            int indexEmpleatSeleccionat = 0;
            dgrEmpleats.Rows[indexEmpleatSeleccionat].Click();
            TextBox txbNIF = w.Get<TextBox>("txbNIF");
            TextBox txbNom = w.Get<TextBox>("txbNom");
            TextBox txbCognoms = w.Get<TextBox>("txbCognoms");
            DateTimePicker dtpData = w.Get<DateTimePicker>("dtpData");
            ListView dgrProjectes = w.Get<ListView>("dgrProjectes");

            Empleat empleatSeleccionat = Empleat.GetEmpleats()[indexEmpleatSeleccionat];
            Assert.AreEqual(empleatSeleccionat.NIF, txbNIF.Text);
            Assert.AreEqual(empleatSeleccionat.Nom, txbNom.Text);
            Assert.AreEqual(empleatSeleccionat.Cognoms, txbCognoms.Text);
            /*
             * Faig comprovacio individual per Dia, Mes i Any perque si comprovo per dates
             * al no ser els segons exactament els mateixos el test falla, per evitar-ho,
             * he decidit fer la comprovació aixi.
             */
            Assert.AreEqual(empleatSeleccionat.DataIncorporacio.Day, ((DateTime)dtpData.Date).Day);
            Assert.AreEqual(empleatSeleccionat.DataIncorporacio.Month, ((DateTime)dtpData.Date).Month);
            Assert.AreEqual(empleatSeleccionat.DataIncorporacio.Year, ((DateTime)dtpData.Date).Year);
            for (int i = 0; i < empleatSeleccionat.ProjectesOnTreballo.Count; i++)
            {
                Assert.AreEqual(empleatSeleccionat.ProjectesOnTreballo[i].Codi,
                                Int32.Parse(dgrProjectes.Rows[i].Cells[0].Text));
                Assert.AreEqual(empleatSeleccionat.ProjectesOnTreballo[i].Nom,
                                dgrProjectes.Rows[i].Cells[1].Text);
            }

            w.Close();
        }

        [TestMethod]
        public void Validacions()
        {
            Window w = StartApplicationAndGetWindow();
            ListView dgrEmpleats = w.Get<ListView>("dgrEmpleats");
            int indexEmpleatSeleccionat = 0;
            dgrEmpleats.Rows[indexEmpleatSeleccionat].Click();
            TextBox txbNIF = w.Get<TextBox>("txbNIF");
            TextBox txbNom = w.Get<TextBox>("txbNom");
            TextBox txbCognoms = w.Get<TextBox>("txbCognoms");
            DateTimePicker dtpData = w.Get<DateTimePicker>("dtpData");
            DateTime d = DateTime.Today;

            Button btnSave = w.Get<Button>("btnSave");

            txbNIF.Text = "47112681";
            Assert.AreEqual(false, btnSave.Enabled);
            txbNIF.Text = "47112681X";
            Assert.AreEqual(true, btnSave.Enabled);

            txbNom.Text = "A";
            Assert.AreEqual(false, btnSave.Enabled);
            txbNom.Text = "Anna";
            Assert.AreEqual(true, btnSave.Enabled);

            txbCognoms.Text = "R";
            Assert.AreEqual(false, btnSave.Enabled);
            txbCognoms.Text = "Reyes Bello";
            Assert.AreEqual(true, btnSave.Enabled);

            d = d.AddDays(-2);
            dtpData.Date = d;
            Assert.AreEqual(false, btnSave.Enabled);

            d = d.AddDays(4);
            dtpData.Date = d;
            Assert.AreEqual(true, btnSave.Enabled);

            /*
             * No valido el borde dels textos perque la llibreria no retorna be
             * el color del borde.
             */

            w.Close();
        }

        [TestMethod]
        public void BotonsAddAndDeleteProject()
        {
            Window w = StartApplicationAndGetWindow();
            ListView dgrEmpleats = w.Get<ListView>("dgrEmpleats");
            int indexEmpleatSeleccionat = 0;
            dgrEmpleats.Rows[indexEmpleatSeleccionat].Click();
            ListView dgrProjectes = w.Get<ListView>("dgrProjectes");
            Button btnDeleteProj = w.Get<Button>("btnDeleteProj");
            Button btnAddProj = w.Get<Button>("btnAddProj");
            ComboBox cbProjectes = w.Get<ComboBox>("cbProjectes");

            Assert.AreEqual(false, btnDeleteProj.Enabled);
            Assert.AreEqual(false, btnAddProj.Enabled);

            cbProjectes.Select("C");
            Assert.AreEqual(false, btnDeleteProj.Enabled);
            Assert.AreEqual(true, btnAddProj.Enabled);
            btnAddProj.Click();
            Assert.AreEqual(dgrProjectes.Items.Count, 3);

            dgrProjectes.Rows[0].Click();
            Assert.AreEqual(true, btnDeleteProj.Enabled);
            Assert.AreEqual(false, btnAddProj.Enabled);
            btnDeleteProj.Click();
            Assert.AreEqual(dgrProjectes.Items.Count, 2);

            cbProjectes.Select("A");
            dgrProjectes.Rows[0].Click();
            Assert.AreEqual(true, btnDeleteProj.Enabled);
            Assert.AreEqual(true, btnAddProj.Enabled);

            w.Close();
        }

        [TestMethod]
        public void BotonEsborrarEmpleat()
        {
            Window w = StartApplicationAndGetWindow();
            ListView dgrEmpleats = w.Get<ListView>("dgrEmpleats");
            Button btnDelete = w.Get<Button>("btnDelete");

            Assert.AreEqual(false, btnDelete.Enabled);

            int indexEmpleatSeleccionat = 0;
            dgrEmpleats.Rows[indexEmpleatSeleccionat].Click();
            Assert.AreEqual(true, btnDelete.Enabled);
            btnDelete.Click();

            TextBox txbNIF = w.Get<TextBox>("txbNIF");
            TextBox txbNom = w.Get<TextBox>("txbNom");
            TextBox txbCognoms = w.Get<TextBox>("txbCognoms");
            DateTimePicker dtpData = w.Get<DateTimePicker>("dtpData");
            ListView dgrProjectes = w.Get<ListView>("dgrProjectes");

            Assert.AreEqual("", txbNIF.Text);
            Assert.AreEqual("", txbNom.Text);
            Assert.AreEqual("", txbCognoms.Text);
            Assert.AreEqual(null, dtpData.Date);
            Assert.AreEqual(0, dgrProjectes.Items.Count);

            Assert.AreEqual(false, btnDelete.Enabled);
            w.Close();
        }

        [TestMethod]
        public void BotonCancelar()
        {
            Window w = StartApplicationAndGetWindow();
            ListView dgrEmpleats = w.Get<ListView>("dgrEmpleats");
            int indexEmpleatSeleccionat = 0;
            dgrEmpleats.Rows[indexEmpleatSeleccionat].Click();

            Button btnCancel = w.Get<Button>("btnCancel");

            Assert.AreEqual(false, btnCancel.Visible);

            TextBox txbNIF = w.Get<TextBox>("txbNIF");
            txbNIF.Text = "47112681X";

            Assert.AreEqual("47112681X", txbNIF.Text);

            Assert.AreEqual(true, btnCancel.Visible);

            btnCancel.Click();

            Empleat empleatSeleccionat = Empleat.GetEmpleats()[indexEmpleatSeleccionat];
            Assert.AreEqual(empleatSeleccionat.NIF, txbNIF.Text);

            w.Close();
        }

        [TestMethod]
        public void BotonDesar()
        {
            Window w = StartApplicationAndGetWindow();
            ListView dgrEmpleats = w.Get<ListView>("dgrEmpleats");
            int indexEmpleatSeleccionat = 0;
            dgrEmpleats.Rows[indexEmpleatSeleccionat].Click();

            Button btnSave = w.Get<Button>("btnSave");

            Assert.AreEqual(false, btnSave.Visible);

            TextBox txbNIF = w.Get<TextBox>("txbNIF");
            txbNIF.Text = "47112681X";

            Assert.AreEqual("47112681X", txbNIF.Text);

            Assert.AreEqual(true, btnSave.Visible);

            btnSave.Click();

            Assert.AreEqual(dgrEmpleats.Rows[0].Cells[2].Text, txbNIF.Text);

            w.Close();
        }
    }
}
