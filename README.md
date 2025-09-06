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
- Handles reading input (hex string or byte array), calling encryption/decryption, and displaying results.

**AES_Project_Domain**
The AES_Project_Domain project contains the following key classes, each serving a specific function in the AES encryption and decryption process:

- `AES_Cipher.cs`: Handles the primary encryption operations of the AES algorithm. This executes everything internally.
- `AES_InverseCipher.cs`: Implements the inverse operations required for AES decryption.
- `AES_Parameters.cs`: Defines and manages various parameters used in the AES algorithm, such as block size, key size, and number of rounds.
- `AesKeySize.cs`: Specifies and manages the supported key sizes for the AES implementation (e.g., 128, 192, 256 bits).
- `FiniteField.cs`: Contains logic related to operations within the Galois Finite Field (GF(2^8)), which is fundamental to AES transformations like MixColumns.
- `KeyExpansion.cs`: Manages the process of expanding the initial AES key into the round keys used in each stage of the encryption/decryption.
- `SBoxes.cs`: Implements the Substitution Boxes (S-box and Inverse S-box) used in the SubBytes transformation, providing non-linearity to the algorithm.
- `Utility.cs`: Contains various helper functions or common utilities used across different parts of the AES implementation.

**AES Core**
- Implements AES algorithm steps:
  - `Cipher(byte[] input, bool trace = false)` — encrypts a block.
  - `InvCipher(byte[] input, bool trace = false)` — decrypts a block.
  - Core transformations: `SubBytes`, `ShiftRows`, `MixColumns`, `AddRoundKey` and inverses.
- Manages the 4x4 AES state array.
- Optionally outputs trace logs for each round when `trace` is enabled.

---

## Build Instructions

1. Ensure you have **.NET 8 SDK** installed.
2. Open a terminal/command prompt in the project root folder.
3. Run:

```bash
dotnet build
