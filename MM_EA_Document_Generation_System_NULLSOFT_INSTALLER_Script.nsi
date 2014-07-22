; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "Magneti Marelli Enterprise Architect Document Generation System"
!define PRODUCT_VERSION "1.0d"
!define PRODUCT_PUBLISHER "Guido Pennella - Magneti Marelli SPA"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\MM_GENIVI_EA_Document_Generation_System.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

; MUI 1.67 compatible ------
!include "MUI.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
!insertmacro MUI_PAGE_LICENSE "LICENCE.txt"
; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.exe"
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Setup.exe"
InstallDir "$PROGRAMFILES\Magneti Marelli Enterprise Architect Document Generation System"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

Section "SezionePrincipale" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite try
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_extractDBusXML.dll"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_extractDBusXML.pdb"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_extractImageByName.dll"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_extractImageByName.pdb"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_extractReqList.dll"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_extractReqList.pdb"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_extractSingleReq.dll"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_extractSingleReq.pdb"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_extractUseCase.dll"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_extractUseCase.pdb"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_Utilies.dll"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\eadgsPlugin_Utilies.pdb"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\MM_GENIVI_EA_Document_Generation_System.exe"
  CreateDirectory "$SMPROGRAMS\MM EA Document Generation System"
  CreateShortCut "$SMPROGRAMS\MM EA Document Generation System\Magneti Marelli Enterprise Architect Document Generation System.lnk" "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.exe"
  CreateShortCut "$DESKTOP\Magneti Marelli Enterprise Architect Document Generation System.lnk" "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.exe"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\MM_GENIVI_EA_Document_Generation_System.exe.config"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\MM_GENIVI_EA_Document_Generation_System.pdb"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\MM_GENIVI_EA_Document_Generation_System.vshost.exe"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\MM_GENIVI_EA_Document_Generation_System.vshost.exe.config"
  File "MM_GENIVI_EA_Document_Generation_System\bin\Debug\MM_GENIVI_EA_Document_Generation_System.vshost.exe.manifest"
SectionEnd

Section -AdditionalIcons
  CreateShortCut "$SMPROGRAMS\MM EA Document Generation System\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd


Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) � stato completamente rimosso dal tuo computer."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Sei sicuro di voler completamente rimuovere $(^Name) e tutti i suoi componenti?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.vshost.exe.manifest"
  Delete "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.vshost.exe.config"
  Delete "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.vshost.exe"
  Delete "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.pdb"
  Delete "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.exe.config"
  Delete "$INSTDIR\MM_GENIVI_EA_Document_Generation_System.exe"
  Delete "$INSTDIR\eadgsPlugin_Utilies.pdb"
  Delete "$INSTDIR\eadgsPlugin_Utilies.dll"
  Delete "$INSTDIR\eadgsPlugin_extractSingleReq.pdb"
  Delete "$INSTDIR\eadgsPlugin_extractSingleReq.dll"
  Delete "$INSTDIR\eadgsPlugin_extractReqList.pdb"
  Delete "$INSTDIR\eadgsPlugin_extractReqList.dll"
  Delete "$INSTDIR\eadgsPlugin_extractImageByName.pdb"
  Delete "$INSTDIR\eadgsPlugin_extractImageByName.dll"
  Delete "$INSTDIR\eadgsPlugin_extractDBusXML.pdb"
  Delete "$INSTDIR\eadgsPlugin_extractDBusXML.dll"
  Delete "$INSTDIR\eadgsPlugin_extractUseCase.dll"
  Delete "$INSTDIR\eadgsPlugin_extractUseCase.pdb"
  Delete "$SMPROGRAMS\MM EA Document Generation System\Uninstall.lnk"
  Delete "$DESKTOP\Magneti Marelli Enterprise Architect Document Generation System.lnk"
  Delete "$SMPROGRAMS\MM EA Document Generation System\Magneti Marelli Enterprise Architect Document Generation System.lnk"

  RMDir "$SMPROGRAMS\MM EA Document Generation System"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd