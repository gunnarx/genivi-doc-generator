/* ***** BEGIN LICENSE BLOCK *****
 * Author: Guido Pennella
 * Copyright ©2013, Magneti Marelli S.p.A. All rights reserved
 * Version: MPL 2.0
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 2.0 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the Licence.
 *
 * ***** END LICENSE BLOCK *****/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace eadgsPlugin_extractReqList
{
    public class eadgsLoadable
    {
        private String connectionString = "";
        private OdbcConnection eapConnection = null;
        private Dictionary<string, eadgs_requirement> requirements_list = new Dictionary<string, eadgs_requirement>();

        public Boolean extractReqList(String prm)
        {
            
            // extract parameters
            String[] param = prm.Split('|');
            // Decode parameters
            String funame = param[0];
            String[] pkg_tree = param[1].Split('.');
            String output_format = param[2];
            String recursive = param[3];
            String eapFile = param[4];
            String auxdir = param[5];

            connectionString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq=" + eapFile + ";Uid=Admin;Pwd=";

            eapConnection = new OdbcConnection();
            eapConnection.ConnectionString = connectionString;
            eapConnection.Open();

            // Recursive acquisition of the final package ID
            String pkgID = eadgsPlugin_Utilies.eadgsPlugin_Utilies.acquirePackageID(0, pkg_tree, "1", eapConnection);

            String pkg_list = pkgID;
            String sql = "";

            if (recursive == "1")
            {
                pkg_list = eadgsPlugin_Utilies.eadgsPlugin_Utilies.acquirePackageListID(pkg_list, pkgID, eapConnection);
                sql = "select Object_ID, Note, Name, Stereotype, Alias from t_object where Object_Type = 'Requirement' and Package_ID in (" + pkg_list + ")";
            }
            else
            {
                sql = "select Object_ID, Note, Name, Stereotype, Alias from t_object where Object_Type = 'Requirement' and Package_ID = " + pkg_list ;
            }

            // Acquire the requirement
            

            OdbcCommand eapCommand = eapConnection.CreateCommand();
            eapCommand.CommandText = sql;
            OdbcDataReader eapReader = eapCommand.ExecuteReader();

            while (eapReader.Read())
            {
                eadgs_requirement req = new eadgs_requirement();
                req.objectID = eapReader[0].ToString();
                req.req_text = eapReader[1].ToString();
                req.req_name = eapReader[2].ToString();
                req.req_stereo = eapReader[3].ToString();
                req.req_alias = eapReader[4].ToString();
                requirements_list.Add(eapReader[0].ToString(), req);
            }

            eapReader.Close();

            foreach (KeyValuePair<string, eadgs_requirement> req in requirements_list)
            {

                sql = "select Value from t_objectproperties where property ='Priority' and Object_ID = " + req.Value.objectID;
                eapCommand.CommandText = sql;
                eapReader = eapCommand.ExecuteReader();
                eapReader.Read();

                requirements_list[req.Value.objectID].req_prio = eapReader[0].ToString();

                eapReader.Close();
            }


            eapCommand.Dispose();
            String output_to_html = eadgsPlugin_Utilies.eadgsPlugin_Utilies.html_upper;
            if (output_format == "1")
            {
                // Single requirements
                output_to_html += "\n<table border=\"1\" cellpadding=\"1\" cellspacing=\"2\" width=\"100%\">";
                foreach (KeyValuePair<string, eadgs_requirement> req in requirements_list)
                {
                    // Single requirement on two rows
                    output_to_html += "<tr><td width=\"20%\">" + req.Value.req_alias + "</td><td width=\"60%\">" + req.Value.req_name + "</td><td width=\"20%\">" + req.Value.req_prio + "</td>";
                    output_to_html += "</tr><tr><td colspan=\"3\">" + req.Value.req_text + "</td></tr>";
                }
                output_to_html += "\n</table>";
            }
            else
            {
                // Tabular report
                output_to_html += "\n<table border=\"1\" cellpadding=\"1\" cellspacing=\"2\" width=\"100%\">";
                output_to_html += "<tr style=\"background-color:rgb(221,221,221)\"><td width=\"15%\"><b>Requirement ID</b></td><td width=\"30%\"><b>Requirement Name</b></td><td width=\"15%\"><b>Priority</b></td>";
                output_to_html += "<td width=\"40%\"><b>Text</b></td></tr>";
                int row = 1;
                String formatting = "";
                foreach (KeyValuePair<string, eadgs_requirement> req in requirements_list)
                {
                    // Single requirement on two rows
                    if ((row % 2) != 0)
                    {
                        formatting = "";
                    }
                    else
                    {
                        formatting = "style=\"background-color:lightblue\"";
                    }

                    output_to_html += "<tr "+formatting+"><td width=\"15%\">" + req.Value.req_alias + "</td><td width=\"30%\">" + req.Value.req_name + "</td><td width=\"15%\">" + req.Value.req_prio + "</td>";
                    output_to_html += "<td width=\"40%\">" + req.Value.req_text + "</td></tr>";
                    row++;
                }
                output_to_html += "\n</table>";
            }
            output_to_html += eadgsPlugin_Utilies.eadgsPlugin_Utilies.html_lower;

            String filename = auxdir + "\\" + funame + ".xhtml";

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.Write(output_to_html);
            file.Close();
            return true;
        }
    }
}
