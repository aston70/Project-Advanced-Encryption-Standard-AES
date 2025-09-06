@echo off
setlocal enabledelayedexpansion

:: Path to your compiled exe
set EXE=".\AES_Project_Console.exe"

:: AES-128 Test
%EXE% AES128 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f > AES_output.txt
echo. >> AES_output.txt

:: AES-192 Test
%EXE% AES192 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f1011121314151617 >> AES_output.txt
echo. >> AES_output.txt

:: AES-256 Test
%EXE% AES256 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f >> AES_output.txt
