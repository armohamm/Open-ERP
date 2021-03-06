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
    public partial class ActualizaPrecios : Form
    {
        public ActualizaPrecios()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0 && comboBox2.SelectedIndex >= 0)
            {
                if (comboBox2.SelectedIndex >= 0 && comboBox2.SelectedIndex < 2)
                {
                    if (comboBox2.SelectedIndex == 0)
                    {
                        string art = createorupdate.itemid;
                        SqlCeCommand el_id = new SqlCeCommand();
                        el_id.Parameters.AddWithValue("id", art);
                        el_id.Parameters.AddWithValue("porc",float.Parse(numericUpDown1.Value.ToString().Replace("-","")));
                        float porcentajeDelForm = float.Parse(numericUpDown1.Value.ToString().Replace("-",""));
                        Conexion.abrir();
                        DataTable getmydata = Conexion.Consultar("precio, costo, porcentaje", "Articulos", "WHERE idarticulo = @id", "", el_id);
                        string precio = getmydata.Rows[0][0].ToString();
                        string costo = getmydata.Rows[0][1].ToString().Replace("$","");
                        string porcent = getmydata.Rows[0][2].ToString();
                       
                        if (comboBox1.SelectedIndex == 0 && numericUpDown1.Value >=0) Conexion.Actualizar("Articulos", "precio = (precio + (precio * (@porc / 100)))", "WHERE idarticulo = @id", "", el_id);
                        if (comboBox1.SelectedIndex == 0 && numericUpDown1.Value <0) Conexion.Actualizar("Articulos", "precio = (precio - (precio * (@porc / 100)))", "WHERE idarticulo = @id", "", el_id);
                        float nuevocosto = float.Parse(costo) + (float.Parse(costo) * (porcentajeDelForm / 100));
                        
                        if ( nuevocosto > float.Parse(precio.Replace("$","")) && numericUpDown1.Value >= 0)// si costo > precio y el valor es positivo
                        {
                            if (comboBox1.SelectedIndex == 1) //si elegimos editar un articulo 
                            {
                                Conexion.Actualizar("Articulos", "costo = (costo + (costo * @porc / 100))", "WHERE idarticulo = @id", "", el_id);
                                //Conexion.Actualizar("Articulos", "precio = (costo + (costo * (CAST(porcentaje AS float) / 100))) ", "WHERE idarticulo = @id", "", el_id);
                            }
                        }
                        if (nuevocosto < float.Parse(precio) && numericUpDown1.Value >= 0)
                        {
                            if (comboBox1.SelectedIndex == 1 && numericUpDown1.Value >= 0) Conexion.Actualizar("Articulos", "costo = (costo + (costo * @porc / 100))", "WHERE idarticulo = @id", "", el_id);
                        }
                        if (nuevocosto > float.Parse(precio) && numericUpDown1.Value < 0)
                        {
                            if (comboBox1.SelectedIndex == 1 && numericUpDown1.Value < 0)
                            {
                                Conexion.Actualizar("Articulos", "costo = (costo - (costo * @porc / 100))", "WHERE idarticulo = @id", "", el_id);
                                //Conexion.Actualizar("Articulos", "precio = (costo - (costo * CAST(porcentaje AS float) / 100)) ", "WHERE idarticulo = @id", "", el_id);
                            }
                        }
                        if (nuevocosto < float.Parse(precio) && numericUpDown1.Value < 0)
                        {
                            if (comboBox1.SelectedIndex == 1 && numericUpDown1.Value < 0) Conexion.Actualizar("Articulos", "costo = (costo - (costo * @porc / 100)) ", "WHERE idarticulo = @id", "", el_id);
                        }
                       
                        Conexion.cerrar();
                        this.Close();
                        if (Application.OpenForms.OfType<Articulos>().Count() == 1)
                            Application.OpenForms.OfType<Articulos>().First().Close();
                            Articulos frm = new Articulos();
                            frm.Show();

                        //hacemos codigo para editar solamente la linea seleccionada
                    }
                    if (comboBox2.SelectedIndex == 1)
                    {
                        string art = createorupdate.itemid;
                        SqlCeCommand el_id = new SqlCeCommand();
                        el_id.Parameters.AddWithValue("id", art);
                        el_id.Parameters.AddWithValue("porc", float.Parse(numericUpDown1.Value.ToString().Replace("-", "")));
                        float porcentajeDelForm = float.Parse(numericUpDown1.Value.ToString().Replace("-", ""));
                        Conexion.abrir();
                        if (comboBox1.SelectedIndex == 0)
                        {
                            if (comboBox1.SelectedIndex == 0 && numericUpDown1.Value >= 0) Conexion.Actualizar("Articulos", "precio = (precio + (precio * (@porc / 100)))", "", "", el_id);
                            if (comboBox1.SelectedIndex == 0 && numericUpDown1.Value < 0) Conexion.Actualizar("Articulos", "precio = (precio - (precio * (@porc / 100)))", "", "", el_id);
                        }
                        if (comboBox1.SelectedIndex == 1)
                        {
                           DialogResult sino =  MessageBox.Show("Tenga en cuenta que aumentar el costo de un producto conlleva a que su precio de venta aumente si es que el precio de compra + porcentaje de ganancia deseada supera al precio de venta actual.\nDesea continuar con la operacion?","Advertencia",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                           if (sino == DialogResult.Yes)
                           {
                               if (comboBox1.SelectedIndex == 1 && numericUpDown1.Value >= 0) //si elegimos editar un articulo 
                               {
                                   Conexion.Actualizar("Articulos", "costo = (costo + (costo * @porc / 100))", "", "", el_id);
                               }
                               if (comboBox1.SelectedIndex == 1 && numericUpDown1.Value < 0)
                               {
                                   Conexion.Actualizar("Articulos", "costo = (costo - (costo * @porc / 100))", "", "", el_id);
                               }
                           }
                           else
                               MessageBox.Show("La operación se ha cancelado");
                        }
                        Conexion.cerrar();
                        this.Close();
                        if (Application.OpenForms.OfType<Articulos>().Count() == 1)
                            Application.OpenForms.OfType<Articulos>().First().Close();
                        Articulos frm = new Articulos();
                        frm.Show();
                        //hacemos codigo para editar todos
                    }
                }
                if (comboBox2.SelectedIndex == 2 && comboBox3.SelectedIndex >= 0)
                {
                    //MessageBox.Show("tocamos proveedor o marca");
                    string proveedr = comboBox3.SelectedItem.ToString() ;
                    SqlCeCommand el_id = new SqlCeCommand();
                    el_id.Parameters.AddWithValue("provee", proveedr);
                    el_id.Parameters.AddWithValue("porc", float.Parse(numericUpDown1.Value.ToString().Replace("-", "")));
                    float porcentajeDelForm = float.Parse(numericUpDown1.Value.ToString().Replace("-", ""));
                    Conexion.abrir();
                    if (comboBox1.SelectedIndex == 0 && numericUpDown1.Value >= 0) Conexion.Actualizar("Articulos", "precio = (precio + (precio * (@porc / 100)))", "WHERE proveedor = @provee", "", el_id);
                    if (comboBox1.SelectedIndex == 0 && numericUpDown1.Value < 0) Conexion.Actualizar("Articulos", "precio = (precio - (precio * (@porc / 100)))", "WHERE proveedor = @provee", "", el_id);
                    if (comboBox1.SelectedIndex == 1 && numericUpDown1.Value >= 0) Conexion.Actualizar("Articulos", "costo = (costo + (costo * @porc / 100))", "", "WHERE proveedor = @provee", el_id);
                    if (comboBox1.SelectedIndex == 1 && numericUpDown1.Value < 0) Conexion.Actualizar("Articulos", "costo = (costo - (costo * @porc / 100))", "", "WHERE proveedor = @provee", el_id);
                    this.Close();
                    if (Application.OpenForms.OfType<Articulos>().Count() == 1)
                        Application.OpenForms.OfType<Articulos>().First().Close();
                    Articulos frm = new Articulos();
                    frm.Show();
                }
                if(comboBox2.SelectedIndex == 2 && comboBox3.SelectedIndex <0)
                {
                    MessageBox.Show("Datos Incompletos");
                }
            }
            else
            {
                MessageBox.Show("Debe completar algunos campos para poder realizar los cambios");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 2 || comboBox2.SelectedIndex == 3)
            {
                if (comboBox2.SelectedIndex == 2)
                {
                    comboBox3.Items.Clear();
                    Conexion.abrir();
                    DataTable proveeds = Conexion.Consultar("nombre", "proveedores", "WHERE Eliminado != 'Eliminado'", "", new SqlCeCommand());
                    Conexion.cerrar();
                    comboBox3.Visible = true;
                    for (int i = 0; i < proveeds.Rows.Count; i++) comboBox3.Items.Add(proveeds.Rows[i][0].ToString()); 
                }
            }
            else
            {
                comboBox3.Visible = false;
            }
        }

        private void ActualizaPrecios_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
            if (e.KeyCode == Keys.F1) button2.PerformClick();
            if (e.KeyCode == Keys.Enter && button1.Focused == false && button2.Focused == false) SendKeys.SendWait("{TAB}");
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            if (comboBox1.Enabled == true)
            {
                comboBox1.DroppedDown = true;
                comboBox1.Select();
            }
        }

        private void comboBox2_Enter(object sender, EventArgs e)
        {
            if (comboBox2.Enabled == true)
            {
                comboBox2.DroppedDown = true;
                comboBox2.Select();
            }
        }

        private void comboBox3_Enter(object sender, EventArgs e)
        {
            if (comboBox3.Enabled == true)
            {
                comboBox3.DroppedDown = true;
                comboBox3.Select();
            }
        }
    }
}
