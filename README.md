# Project-Advanced-Encryption-Standard-AES
.Net implementation of AES

## References

- [FIPS PUB 197 — Advanced Encryption Standard (AES)](https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.197.pdf)


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

## Unit Tests

This project includes unit tests using MSTest Project called `AES_Project_UnitTests`. These tests ensure the core function of the AES implementation function as expected.

These Include:

*   **AES Cipher Tests (`AES_Cipher_Tests.cs`):**
    *   `AES128_AppendixC_Vector()`: Verifies AES-128 encryption/decryption against Appendix C test vectors.
    *   `AES192_AppendixC_Vector()`: Verifies AES-192 encryption/decryption against Appendix C test vectors.
    *   `AES256_AppendixC_Vector()`: Verifies AES-256 encryption/decryption against Appendix C test vectors.

*   **Finite Field Tests (`FiniteField_Tests.cs`):**
    *   `Test_GFAdd()`: Tests addition operations in the Galois Field GF(2^8).
    *   `Test_GF_XTime()`: Tests the XTime operation in GF(2^8).
    *   `Test_GF_Multiply()`: Tests multiplication operations in GF(2^8).

*   **Key Expansion Tests (`KeyExpansion_Tests.cs`):**
    *   `RotWord_ShouldRotateLeft()`: Verifies the `RotWord` transformation.
    *   `SubWord_ShouldSubstituteBytes()`: Verifies the `SubWord` transformation.
    *   `ExpandKey_AES128_ShouldReturnCorrectLength()`: Ensures correct key expansion for AES-128.
    *   `ExpandKey_InvalidKeyLength_ShouldThrow()`: Confirms error handling for invalid key lengths during expansion.

*   **S-Boxes Tests (`SBoxes_Tests.cs`):**
    *   `SBox_Length_Is256()`: Checks the size of the S-Box.
    *   `InvSBox_Length_Is256()`: Checks the size of the Inverse S-Box.
    *   `SBox_Known_Values()`: Verifies known values in the S-Box.
    *   `InvSBox_Known_Values()`: Verifies known values in the Inverse S-Box.

### Running the Tests

To execute the unit tests:

1.  Open the solution in Visual Studio.
2.  Navigate to **Test > Test Explorer**.
3.  Click "Run All Tests" or select specific tests to run them.

Alternatively, you can run tests from the command line using `dotnet test` within the `AES_Project_UnitTests` directory.

---

### Build Instructions

The `AES_Project_Console'` is the one needed to compile and execute.

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
Once built, executable files should reside in .\AES_Project_Console\bin\Release\net8.0\

### 3. Execute

# Windows:
```bat
:: AES-128 Test
AES_Project_Console.exe AES128 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f > AES_output.txt

:: AES-192 Test
AES_Project_Console.exe AES192 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f1011121314151617 > AES_output.txt

:: AES-256 Test
AES_Project_Console.exe AES256 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f > AES_output.txt
```

# Linux:
```bash
# AES-128 Test
./AES_Project_Console AES128 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f > AES_output.txt

# AES-192 Test
./AES_Project_Console AES192 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f1011121314151617 > AES_output.txt

# AES-256 Test
./AES_Project_Console AES256 00112233445566778899aabbccddeeff 000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f > AES_output.txt
```

# Handled Exceptions
```bash
C.2   AES-192 (Nk=6, Nr=12))

PLAINTEXT:          000102030405060708090a0b0c0d0e0f1011121314151617
KEY:                00112233445566778899aabbccddeeff

Error: Key length mismatch
```





