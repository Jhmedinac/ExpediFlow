﻿using System;
using System.Collections;
using System.Data;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using ExpediFlow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace ExpediFlow
{
    public class cGeneralFun
    {
        public class ListtoDataTableConverter
        {
            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
        }

        public List<MenuDinamico> ObtenerMenu(string sObjeto)
        {
            // Convierte el JSON en una lista de objetos MenuDinamico
            List<MenuDinamico> menuList = ListadoDeMenus().Where(menu => menu.Objeto == sObjeto).ToList();
            return menuList;
        }

        private List<MenuDinamico> ListadoDeMenus()
        {
            // Aquí puedes cargar los países desde cualquier fuente de datos, como un archivo de configuración o un servicio externo.
            // Por ejemplo, aquí se crea una lista de países en código duro para este ejemplo.
            var menu = new List<MenuDinamico>
        {
            new MenuDinamico { Objeto= "Perfil",Titulo= "Datos Personales",Area= "",Controler= "Usuario",Accion= "PersonalData" },
            new MenuDinamico { Objeto= "Perfil",Titulo= "Cambiar Correo Electrónico",Area= "",Controler= "Usuario",Accion= "Email" },
            new MenuDinamico { Objeto= "Perfil",Titulo= "Cambiar Contraseña",Area= "",Controler= "Usuario",Accion= "ChangePassword" },
        };
            return menu;
        }


    }
}
