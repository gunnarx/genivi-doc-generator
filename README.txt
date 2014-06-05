Magneti Marelli Enterprise Architect Document Generation System
________________________________________________________________

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
________________________________________________________________ 
Version 1.0 Features
	- Main engine to generate documentation based on DOCX templates from a generic source, extensible via a plug-in based interface
	- Set of base plug-ins to generate document out of Enterprise Architect 8.x or later able to extract:
		* a single requirement, given its ID
		* a set of requirements, given their package
		* a diagram, given its name and package
	- A plug-in to generate document out of DBUS XML	

For a comprehensive introduction, see the presentation in the MANUAL directory	

________________________________________________________________
Version 1.0 to-do
	The following features are scheduled for a future release
	- Project file save: to be able to save the configuration in a file in order to retrieve it (currently the last configuration is saved into the windows registry of the user)
	- CHM manual, accessible via F1 and/or via the Help menu
	- User contribution ... additional generators/extractors also for other UML tools are welcome.
________________________________________________________________
Version 1.0 base installation needs
	In order to run the pre-compiled version of the SW that has an automatic installator (setup.exe file in this directory) the user shall fullfill the following base requirements
	- Windows7 or later
	- Microsoft .NET 4.5 Framework (http://www.microsoft.com/it-it/download/details.aspx?id=30653)
	- Microsoft OpenXML SDK 2.5 (http://msdn.microsoft.com/en-us/library/office/bb448854(v=office.15).aspx)
	- Enterprise Architect 8.x or later
	- Office 2007 or later
________________________________________________________________
Version 1.0 advanced installation needs	
	If the user shall re-build the system the following additional components are needed:
	- Microsoft(C) Visual Studio Express 11 (aka 2012) or later with C# development environment
	- Nullsoft Scriptable Install System (NSIS) to compile the script that build the automatic installator setup.exe - see http://nsis.sourceforge.net/Main_Page
	- To ease the edit of the NSIS script, we suggest to use HM NIS EDIT: A Free NSIS Editor/IDE - see HM NIS EDIT site: http://hmne.sourceforge.net/ 
________________________________________________________________
Contact information
	Feel free to contact me for feature request and/or code contributions.
	
	Guido Pennella
	
	guido.pennella@magnetimarelli.com
	guido.pennella@gmail.com
	
	Note: soon there will be also a mailing list and a bugzilla to submit bugs notifications.