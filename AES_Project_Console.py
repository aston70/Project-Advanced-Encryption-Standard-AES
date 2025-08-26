import subprocess

EXE_PATH = r".\AES_Project\AES_Project_Console\bin\Release\net8.0\AES_Project_Console.exe"

TEST_VECTORS = [
    {
        "name": "AES-128",
        "key": "000102030405060708090a0b0c0d0e0f",
        "plaintext": "00112233445566778899aabbccddeeff",
        "ciphertext": "69c4e0d86a7b0430d8cdb78070b4c55a",
        "size": "AES128",
    },
    {
        "name": "AES-192",
        "key": "000102030405060708090a0b0c0d0e0f1011121314151617",
        "plaintext": "00112233445566778899aabbccddeeff",
        "ciphertext": "dda97ca4864cdfe06eaf70a0ec0d7191",
        "size": "AES192",
    },
    {
        "name": "AES-256",
        "key": "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f",
        "plaintext": "00112233445566778899aabbccddeeff",
        "ciphertext": "8ea2b7ca516745bfeafc49904b496089",
        "size": "AES256",
    },
]

def run_exe(args):
    """Run the console exe with args, return stdout as string"""
    result = subprocess.run([EXE_PATH] + args, capture_output=True, text=True)
    return result.stdout.strip()

def run_tests(decrypt=False):
    mode = "Decrypt" if decrypt else "Encrypt"
    print(f"=== Running AES_Project_Console.exe Appendix C {mode} Tests ===\n")

    for vector in TEST_VECTORS:
        print(f"Running {vector['name']}...")

        # Arguments for exe
        if decrypt:
            args = [vector["size"], vector["key"], vector["ciphertext"], "decrypt"]
            expected = vector["plaintext"]
        else:
            args = [vector["size"], vector["key"], vector["plaintext"], "encrypt"]
            expected = vector["ciphertext"]

        output = run_exe(args)

        # Extract the line that contains Encrypt or Decrypt result
        result_line = None
        for line in output.splitlines():
            if decrypt and line.startswith("Decrypt:"):
                result_line = line.split(":", 1)[1].strip()
            elif not decrypt and line.startswith("Encrypt:"):
                result_line = line.split(":", 1)[1].strip()

        if result_line is None:
            print("Could not find result in output:")
            print(output)
            continue

        result = "PASS" if result_line.lower() == expected.lower() else "FAIL"

        print(f"Input:    {vector['plaintext'] if not decrypt else vector['ciphertext']}")
        print(f"Output:   {result_line}")
        print(f"Expected: {expected}")
        print(f"Result:   {result}")
        print("-" * 60)


def main():
    run_tests(decrypt=False)
    print("="*60)
    run_tests(decrypt=True)

if __name__ == "__main__":
    main()
