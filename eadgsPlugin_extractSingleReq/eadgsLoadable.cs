/* SPDX license identifier: MPL-2.0
 * Author: Guido Pennella
 * Copyright ©2014, Magneti Marelli S.p.A. All rights reserved
 * Version: MPL 2.0
 * 
 * This file is part of Magneti Marelli Genivi Document Generator
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 2.0. If a copy of the MPL was not distributed with this file, you can obtain one at http://mozilla.org/MPL/2.0/. 
 * 
 * For further information see http://www.genivi.org/. 
 *
 * ***** END LICENSE BLOCK *****/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace eadgsPlugin_extractSingleReq
{
    public class eadgsLoadable
    {
        private String connectionString = "";
        private OdbcConnection eapConnection = null;

        public Boolean extractSingleReq(String prm)
        {
            // extract parameters
            String[] param = prm.Split('|');
            // Decode parameters
            String funame = param[0];
            String[] pkg_tree = param[1].Split('.');
            String req_alias = param[2];
            String output_format = param[3];
            String eapFile = param[4];
            String auxdir = param[5];

            connectionString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq=" + eapFile + ";Uid=Admin;Pwd=";

            eapConnection = new OdbcConnection();
            eapConnection.ConnectionString = connectionString;
            eapConnection.Open();

            // Recursive acquisition of the final package ID
            String pkgID = eadgsPlugin_Utilies.eadgsPlugin_Utilies.acquirePackageID(0, pkg_tree, "1", eapConnection);

            // Acquire the requirement
            String sql = "select Object_ID, Note, Name, Stereotype from t_object where Object_Type = 'Requirement' and Package_ID = " + pkgID + " and Alias = '" + req_alias + "'";

            OdbcCommand eapCommand = eapConnection.CreateCommand();
            eapCommand.CommandText = sql;
            OdbcDataReader eapReader = eapCommand.ExecuteReader();
            eapReader.Read();

            String objectID = eapReader[0].ToString();
            String note = eapReader[1].ToString();
            String name = eapReader[2].ToString();
            String stereotype = eapReader[3].ToString();

            eapReader.Close();

            sql = "select Value from t_objectproperties where property ='Priority' and Object_ID = " + objectID;
            eapCommand.CommandText = sql;
            eapReader = eapCommand.ExecuteReader();
            eapReader.Read();

            String Priority = eapReader[0].ToString();

            eapCommand.Dispose();

            String output_to_html = eadgsPlugin_Utilies.eadgsPlugin_Utilies.html_upper;
            if (output_format == "1")
            {
                // Single requirement on two rows
                output_to_html += "\n<table border=\"1\" cellpadding=\"1\" cellspacing=\"2\" width=\"100%\"><tr><td width=\"20%\">" + req_alias + "</td><td width=\"60%\">" + name + "</td><td width=\"20%\">" + Priority + "</td>";
                output_to_html += "</tr><tr><td colspan=\"3\">" + note + "</td></tr></table>";
            }
            else
            {
                // Single requirement on a single row
                output_to_html += "\n<table border=\"1\" cellpadding=\"1\" cellspacing=\"2\" width=\"100%\"><tr><td width=\"15%\">" + req_alias + "</td><td width=\"30%\">" + name + "</td><td width=\"15%\">" + Priority + "</td>";
                output_to_html += "<td width=\"40%\">" + note + "</td></tr></table>";
            }
            output_to_html += eadgsPlugin_Utilies.eadgsPlugin_Utilies.html_lower;

            String filename = auxdir + "\\" + funame + ".xhtml";

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.Write(output_to_html);
            file.Close();

            // close the db connection
            eapConnection.Close();

            return true;
        }
    }
}
