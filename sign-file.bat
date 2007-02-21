@echo Usage: sign MyFile

@echo off
signcode -spc xmlmvpcert.spc -v xmlmvpkey.pvk -t http://timestamp.verisign.com/scripts/timstamp.dll %1