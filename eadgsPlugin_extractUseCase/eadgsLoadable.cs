﻿/* ***** BEGIN LICENSE BLOCK *****
 * Author: Guido Pennella
 * Copyright ©2014, Magneti Marelli S.p.A. All rights reserved
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