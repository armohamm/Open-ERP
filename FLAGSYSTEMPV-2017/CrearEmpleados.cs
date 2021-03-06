﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace FLAGSYSTEMPV_2017
{
    public partial class CrearEmpleados : Form
    {
        public CrearEmpleados()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
       

        private void button1_Click(object sender, EventArgs e)
        {
            createorupdate.status = "create";
            if (Application.OpenForms.OfType<NuevoUser>().Count() == 1)
                Application.OpenForms.OfType<NuevoUser>().First().Focus();
            else
            {
                NuevoUser frm = new NuevoUser();
                frm.Show();
            }
        }

        private void Clientes_Load(object sender, EventArgs e)
        {
            this.Focus();
              
            Conexion.abrir();
            SqlCeCommand notelim = new SqlCeCommand();
            notelim.Parameters.AddWithValue("elim", "Eliminado");
            DataTable showacls = Conexion.Consultar("iduser,login as [Nombre Usuario],level as [Jerarquía],nombreusuario as [Nombre Real]", "Usuarios", " WHERE eliminado != @elim", "", notelim);
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = showacls;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = showacls;
            dataGridView1.Columns[0].Visible = false;
           
            dataGridView1.DataSource = SBind;
            dataGridView1.Refresh();
            if (showacls.Rows.Count > 0)
            {
                button2.Enabled = true;
                button9.Enabled = true;
                button4.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button9.Enabled = false;
                button4.Enabled = true;
            }
            if (registereduser.level == "Supervisor" || registereduser.level == "Admin")
            {
                button4.Enabled = true;
            }
            else
                button4.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                var row = this.dataGridView1.Rows[rowIndex];
                string name = row.Cells["Nombre Real"].Value.ToString();
                string id = row.Cells["iduser"].Value.ToString();
                string level = row.Cells["Jerarquía"].Value.ToString();
                if (level != "Admin")
                {
                    DialogResult borrar = MessageBox.Show("Está segudo de borrar este Usuario?\n" + name, "Borrar?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (borrar == DialogResult.Yes)
                    {
                        Conexion.abrir();
                        SqlCeCommand del = new SqlCeCommand();
                        del.Parameters.AddWithValue("@id", id);
                        del.Parameters.AddWithValue("@bo", "Eliminado");
                        Conexion.Actualizar("Usuarios", "eliminado = @bo", "WHERE iduser = @id", "", del);
                        this.Close();
                        CrearEmpleados showagn = new CrearEmpleados();
                        showagn.Show();
                    }
                }
                else MessageBox.Show("Este usuario está protegido");
            }
            else MessageBox.Show("No hay ningun usuario seleccionado para borrar");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {


                BindingSource bd = (BindingSource)dataGridView1.DataSource;
                DataTable dt = (DataTable)bd.DataSource;
                string formatstring = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == 0) formatstring += " CONVERT([" + dt.Columns[i].ColumnName + "],System.String) like '%{0}%' ";
                    else formatstring += " or CONVERT([" + dt.Columns[i].ColumnName + "],System.String) like '%{0}%' ";

                }


                dt.DefaultView.RowFilter = string.Format(formatstring, textBox1.Text.Trim().Replace("'", "''"));
                dataGridView1.Refresh();

            }
            catch (Exception) { }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                if (dataGridView1.Rows[rowIndex].Cells[2].Value.ToString() != "Admin")
                {
                    createorupdate.itemid = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    createorupdate.status = "update";
                    if (Application.OpenForms.OfType<NuevoUser>().Count() == 1)
                        Application.OpenForms.OfType<NuevoUser>().First().Focus();
                    else
                    {
                        NuevoUser frm = new NuevoUser();
                        frm.Show();
                    }
                }
                else MessageBox.Show("Este usuario está protegido");
            }
            else MessageBox.Show("No hay ningun usuario seleccionado para edición");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Permisos>().Count() == 1)
                Application.OpenForms.OfType<Permisos>().First().Focus();
            else
            {
                Permisos frm = new Permisos();
                frm.Show();
            }
        }

        private void CrearEmpleados_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (e.KeyCode == Keys.F1 && button1.Enabled == true)
                button1.PerformClick();
            if (e.KeyCode == Keys.F2 && button9.Enabled == true)
                button9.PerformClick();
            if (e.KeyCode == Keys.F3 && button2.Enabled == true)
                button2.PerformClick();
            if (e.KeyCode == Keys.F4 && button4.Enabled == true)
                button4.PerformClick();
            if (e.KeyCode == Keys.F5)
                textBox1.Select();

            if (e.KeyCode == Keys.Up && dataGridView1.Focused == false)
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows[rowIndex - 1].Cells[1].Selected = true;
                }
                catch (Exception)
                {

                }

            }
            if (e.KeyCode == Keys.Down && dataGridView1.Focused == false)
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows[rowIndex + 1].Cells[1].Selected = true;
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
