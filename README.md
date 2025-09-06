# Project-Advanced-Encryption-Standard-AES
.Net implementation of AES

# AES_Project_Console

## Overview

`AES_Project_Console` is a C# console application that implements the AES encryption and decryption algorithm (FIPS-197).  
It supports encrypting and decrypting 16-byte blocks using multiple key sizes (128, 192, or 256 bits), with optional trace output showing the state of each AES round.


---

## Architecture

**Main Project: `AES_Project_Console`**
- Entry point: `Program.cs` with `Main()` method.
- Handles reading input (key size string, input string, key string, mode string (encrypt/decrypt)), calling AES_Cipher.cs -> Cipher() or InvCipher().

**AES_Project_Domain**
The AES_Project_Domain project contains all the core AES classes broken out into smaller pieces, each serving a specific function in the AES encryption and decryption process:

- `AES_Cipher.cs`: Handles the primary encryption operations of the AES algorithm.
- `AES_Parameters.cs`: Defines and manages various parameters used in the AES algorithm, such as block size, key size, and number of rounds.
- `AesKeySize.cs`: Enum for key sizes for the AES implementation (e.g., 128, 192, 256 bits).
- `FiniteField.cs`: Contains math logic related to operations fundamental to AES transformations such as Add(), Multiply(), and XTime().
- `KeyExpansion.cs`: Manages the process of expanding the initial AES key into the round keys used in each stage of the encryption/decryption.
- `SBoxes.cs`: Substitution Boxes byte arrays (S-box and Inverse S-box).
- `Utility.cs`: Contains various helper functions; moslty conversion functions.

**AES Core Functions**
- Implements AES algorithm steps:
  - `Cipher(byte[] input, bool trace = false)` — encrypts a block.
  - `InvCipher(byte[] input, bool trace = false)` — decrypts a block.
  - Core transformations: `SubBytes`, `ShiftRows`, `MixColumns`, `AddRoundKey` and inverse equivalents.
- Manages the 4x4 AES state array.
- Optionally outputs trace logs for each round when `trace` is enabled.

---

## Build Instructions

# Project-Advanced-Encryption-Standard-AES

A .NET 8 console application implementing the AES (Advanced Encryption Standard) algorithm with support for multiple key sizes (128, 192, 256 bits). Includes both encryption and decryption, with optional round-by-round tracing.

## Project Architecture

The solution contains two main projects:

### AES_Project_Console
- Main console application.
- Entry point: `Program.cs` (`Main()` method).
- Handles user input and output, calls into the domain project for encryption/decryption.

### AES_Project_Domain
Contains the core AES logic, including the following classes:

- **AES_Cipher.cs**: Handles primary AES encryption operations.
- **AES_InverseCipher.cs**: Implements inverse operations for AES decryption.
- **AES_Parameters.cs**: Defines AES parameters such as block size, key size, and number of rounds.
- **AesKeySize.cs**: Specifies supported key sizes (128, 192, 256 bits).
- **FiniteField.cs**: Contains Galois Field (GF(2^8)) logic for MixColumns and other transformations.
- **KeyExpansion.cs**: Expands the initial AES key into round keys for each stage.
- **SBoxes.cs**: Implements S-box and Inverse S-box for SubBytes and inverse transformations.
- **Utility.cs**: General helper functions used across the AES implementation.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Command-line terminal (PowerShell, Command Prompt on Windows; bash/zsh on Linux)

### Project Setup and Execution Instructions

### 1. Clone the repository
```bash
git clone https://github.com/aston70/Project-Advanced-Encryption-Standard-AES.git
cd Project-Advanced-Encryption-Standard-AES
```

### 2. Build the project
```bash
dotnet build
dotnet run --project AES_Project_Console
dotnet publish -c Release -r win-x64 --self-contained true
dotnet publish -c Release -r linux-x64 --self-contained true
```

### 3. Execute

# Windows:
```bat
:: AES-128 Test
AES_Project_Console.exe AES128 000102030405060708090a0b0c0d0e0f 00112233445566778899aabbccddeeff > AES_output.txt

:: AES-192 Test
AES_Project_Console.exe AES192 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f1011121314151617 >> AES_output.txt

:: AES-256 Test
AES_Project_Console.exe AES256 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f>> AES_output.txt
```

# Linux:
```bash
# AES-128 Test
./AES_Project_Console AES128 000102030405060708090a0b0c0d0e0f 00112233445566778899aabbccddeeff > AES_output.txt

# AES-192 Test
./AES_Project_Console AES192 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f1011121314151617 >> AES_output.txt

# AES-256 Test
./AES_Project_Console AES256 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f >> AES_output.txt
```

# Handled Exceptions
```bash
C.2   AES-192 (Nk=6, Nr=12))

PLAINTEXT:          000102030405060708090a0b0c0d0e0f1011121314151617
KEY:                00112233445566778899aabbccddeeff

Error: Key length mismatch
```





