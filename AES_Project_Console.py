import subprocess

# Path to your compiled console app
EXE_PATH = r".\AES_Project\AES_Project_Console\bin\Release\net8.0\AES_Project_Console.exe"

# Appendix C test vectors
TEST_VECTORS = [
    {
        "name": "AES-128",
        "key": "000102030405060708090a0b0c0d0e0f",
        "plaintext": "00112233445566778899aabbccddeeff",
        "expected": "69c4e0d86a7b0430d8cdb78070b4c55a",
        "size": "AES128",
    },
    {
        "name": "AES-192",
        "key": "000102030405060708090a0b0c0d0e0f1011121314151617",
        "plaintext": "00112233445566778899aabbccddeeff",
        "expected": "dda97ca4864cdfe06eaf70a0ec0d7191",
        "size": "AES192",
    },
    {
        "name": "AES-256",
        "key": "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f",
        "plaintext": "00112233445566778899aabbccddeeff",
        "expected": "8ea2b7ca516745bfeafc49904b496089",
        "size": "AES256",
    },
]

def run_exe(args=None):
    """Run the console exe with args, return stdout as string"""
    if args is None:
        result = subprocess.run([EXE_PATH], capture_output=True, text=True)
    else:
        result = subprocess.run([EXE_PATH] + args, capture_output=True, text=True)
    return result.stdout.strip()

def main():
    print("=== Running AES_Project_Console.exe Appendix C Tests ===")
    print()

    # Run built-in Appendix C (no args)
    output = run_exe()
    print("Built-in run (no args):")
    print(output)
    print("="*60)

    # Run with explicit arguments for each vector
    for vector in TEST_VECTORS:
        print(f"Running {vector['name']} with args...")
        args = [vector["size"], vector["key"], vector["plaintext"]]
        output = run_exe(args)
        print(output)
        print("-" * 60)

if __name__ == "__main__":
    main()
