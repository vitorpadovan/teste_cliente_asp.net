# teste_cliente_asp.net

## 1. Gerando certrificado [Let's Encrypt](https://letsencrypt.org/)

Em desenvolvimento...

## 2. Instalando certificado
### 2.1. Gerar certificados

Necessário gerar certificados com os comandos abaixo, onde ele irá ficar na hora de gerar não é necessário ser específico, porem o arquivo ```aspnetapp.pfx``` deve estar dentro da pasta ```/https/``` dentro do docker depois, através de mapeamento no docker compose
```dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p <CREDENTIAL_PLACEHOLDER>```
```dotnet dev-certs https --trust```

### 2.2. Mapeando a pasta para o docker

Para funcionar o certificado acima, o mesmo deve estar dentro da pasta ```/https/aspnetapp.pfx``` do container, sendo assim o mapeamento final ficará assim:  ```~/.aspnet/https:/https:ro```.

### 2.3 Senha do certificado

Para não deixar a senha exposta, a mesma está através de variável ```SENHACERT```

## Referencias
- [Model validation in ASP.NET Core MVC and Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-6.0)
- [Model validation in ASP.NET Core MVC and Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-7.0)
- [Hosting ASP.NET Core images with Docker over HTTPS](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-6.0)