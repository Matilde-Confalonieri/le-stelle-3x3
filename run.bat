@echo off
title Le Stelle 3x3 - Server Always-On
cd /d "%~dp0"

echo ============================================
echo   LE STELLE 3x3 - Server + Tunnel
echo   Questo server rimane attivo finche' questa
echo   finestra resta aperta (NON chiuderla).
echo ============================================

:start
echo.
echo [%time%] Avvio server su porta 8888...
start "" /B dotnet run --urls "http://0.0.0.0:8888" --no-launch-profile
timeout /t 6 /nobreak >nul

echo [%time%] Avvio tunnel ngrok...
ngrok http 8888

echo [%time%] Tunnel caduto. Riavvio tra 5 secondi...
timeout /t 5 /nobreak
goto start
