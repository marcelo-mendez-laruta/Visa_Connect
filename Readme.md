## Generar Certificado P12
Para generar el certificado usar el siguiente comando:

```bash
openssl pkcs12 -export -in cert.pem -inkey "key.pem" -out COFCertBundle.p12
```