# KeepIT
Simple C# console application that work in .NET 4.0, the app display and store passwords and emails in encryption method.
using the Rijndael algorithm aka AES in CBC mode (Cipher Block Chaining).
The key for the encryption is derived using a passphrase (secret key) and a salt value which equals to passphrase, with 2 iterations of the Rfc2898DeriveBytes algorithm. The block size is 128 bits and the key size is 256 bits. The initialization vector (IV) is hard coded value.

# Warning
The passphrase (secret key) can be pulled from the memory of the application if the app isn't close.
