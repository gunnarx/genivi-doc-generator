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

namespace eadgsPlugin_extractUseCase
{
    public class eadgsLoadable
    {
        private String connectionString = "";
        private OdbcConnection eapConnection = null;

        public Boolean extractUseCase(String prm)
        {
            // extract parameters
            String[] param = prm.Split('|');
            // Decode parameters
            String funame = param[0];
            String[] pkg_tree = param[1].Split('.');
            String title_level = param[2];
            String recursive = param[3];
            String eapFile = param[4];
            String auxdir = param[5];

            return true;
        }
}
