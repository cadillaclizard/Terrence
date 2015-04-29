;--------------------------------
;Include Modern UI

  !include "MUI.nsh"

;--------------------------------
; General

  Name "Terrence"
  OutFile "Terrence Installer.exe"
  InstallDir "$LOCALAPPDATA\Terrence"
  InstallDirRegKey HKCU "Software\Terrence" ""
  RequestExecutionLevel admin
  ShowInstDetails show

;--------------------------------
; Interface Configuration

  !define MUI_ABORTWARNING

;--------------------------------
; Pages

  !insertmacro MUI_PAGE_WELCOME
  ; !insertmacro MUI_PAGE_LICENSE ""
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_INSTFILES

 
  ; Finish Page Configuration
  !insertmacro MUI_PAGE_FINISH
 
;--------------------------------
; Languages

  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
; Installer Sections

Section "Terrence"

  SectionIn RO
  SetOutPath "$INSTDIR"

  File ..\Terrence.exe

  ; Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"  

  ; Registry key for exe
  WriteRegStr HKCU "Software\Terrence" "" $INSTDIR 

  ; Registry keys for uninstaller
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Terrence" \
                 "DisplayName" "Terrence"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Terrence" \
                 "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Terrence" \
                 "DisplayIcon" "$INSTDIR\Terrence.exe"
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Terrence" \
                 "NoModify" 1
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Terrence" \
                 "NoRepair" 1

  ; Registry shell ex
  WriteRegStr HKCR "pngfile\shell\Search by image\command" "" "$\"$INSTDIR\Terrence.exe$\" -google %1"
  WriteRegStr HKCR "jpgfile\shell\Search by image\command" "" "$\"$INSTDIR\Terrence.exe$\" -google %1"
  WriteRegStr HKCR "jpegfile\shell\Search by image\command" "" "$\"$INSTDIR\Terrence.exe$\" -google %1"
  WriteRegStr HKCR "giffile\shell\Search by image\command" "" "$\"$INSTDIR\Terrence.exe$\" -google %1"

SectionEnd

;--------------------------------
 
Section "Uninstall"

  ; Delete files
  Delete "$INSTDIR\Terrence.exe"
  Delete "$INSTDIR\Uninstall.exe"

  ; Delete directories
  RMDir "$INSTDIR"

  ; Remove registry keys
  DeleteRegKey HKCU "Software\Terrence"
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Terrence"

  DeleteRegKey HKCR "pngfile\shell\Search by image\"
  DeleteRegKey HKCR "jpgfile\shell\Search by image\"
  DeleteRegKey HKCR "jpegfile\shell\Search by image\"
  DeleteRegKey HKCR "giffile\shell\Search by image\"

SectionEnd