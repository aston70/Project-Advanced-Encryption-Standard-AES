@echo off
setlocal enabledelayedexpansion

:: Path to your compiled exe
set EXE=".\AES_Project\AES_Project_Console\bin\Release\net8.0\AES_Project_Console.exe"

echo AES Appendix C Test Harness (Batch)
echo ==================================
echo.

:: AES-128 Test
%EXE% AES128 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f > AES_output.txt
echo.

:: AES-192 Test
%EXE% AES192 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f1011121314151617 > AES_output.txt
echo.

:: AES-256 Test
%EXE% AES256 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f > AES_output.txt
echo.

echo All Appendix C tests complete.
pause
