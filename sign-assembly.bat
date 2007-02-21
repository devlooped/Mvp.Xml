@echo Usage: sign MyAssembly[dll or exe]

@echo off
sn -Ra %1 Mvp-Xml.Net20.snk
signcode -spc xmlmvpcert.spc -v xmlmvpkey.pvk -t http://timestamp.verisign.com/scripts/timstamp.dll %1