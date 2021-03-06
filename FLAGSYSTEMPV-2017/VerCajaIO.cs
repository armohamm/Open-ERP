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
    public partial class VerCajaIO : Form
    {
        public VerCajaIO()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VerCajaIO_Load(object sender, EventArgs e)
        {
            label2.Text = "Mostrando " + CIO.entradaosalida + " de caja";
            getarts();

        }
      

        void getarts()
        {
            Conexion.abrir();
            DataTable showarts = Conexion.Consultar("fecha,motivo,total", CIO.entradaosalida+"Caja", " order by fecha desc", "", new SqlCeCommand());
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = showarts;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = showarts;
            dataGridView1.Columns[2].DefaultCellStyle.Format = "c";
            dataGridView1.DataSource = SBind;
            dataGridView1.Refresh();

            if (showarts.Rows.Count > 0)
            {
                
                dataGridView1.DataSource = showarts; //mostramos lo que hay
          
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var bd = dataGridView1.DataSource;
            
            var dt = (DataTable)bd;
            dt.DefaultView.RowFilter = string.Format("CONVERT([fecha],System.String) like '%{0}%' or CONVERT([motivo],System.String) like '%{0}%'  or CONVERT([total],System.String) like '%{0}%'", textBox1.Text.Trim().Replace("'", "''"));
            dataGridView1.Refresh();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.F1)
            {
                textBox1.Select();
            }
        }

        private void VerCajaIO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

        }
    }
}
