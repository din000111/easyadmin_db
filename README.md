# Установка
1. Нужно скопировать репозиторий
2. Внести изменения в appsettings.json и appsettings.Development.json в соответствии с инфраструктурой
```
{
    // Настройка подключения в БД.
    "ConnectionStrings": {
        "ConnectionString": "Host=db;Port=5432;Database=easyadmin;Username=easyadmin;Password=password"
    },
    // Настройка подключения к Active Directory
    "Ldap": {
        "url": "dc02.hostco.ru", // адрес сервера ldap
        "useSsl": true, // использовать Ssl
        "port": 636, // порт для подключения. Порты по умолчанию: 636 ssl и 389 noSsl
        "bindDn": "CN=Domain Reader,OU=AD,OU=SERVICE,DC=hostco,DC=ru", // учетка, с правами на чтение
        "bindCredentials": "***password***", // пароль от этой учетки
        "searchBase": "OU=GC_HOST_Organization,DC=hostco,DC=ru", // папка, в которой лежат пользователи
        "searchFilter": "(&(objectClass=User)(extensionAttribute1=*){0})", // фильтр, чтобы отсекать лишних пользователей. обязательно прописать {0} для работы фильтра в коде
        "adminCn": "CN=admin_easyadmin,OU=Admin,OU=SERVICE,DC=hostco,DC=ru", // группа, в которой находятся администраторы EasyAdmin
        "SearchGroupBase": "OU=SG_HOST,DC=hostco,DC=ru" // пока не используется
    },
    // Настройка почтового сервера
    "Mail": {
        "address": "wa.hostco.ru",
        "port": 587,
        "username": "vcentercop@hostco.ru",
        "password": "***password***",
        "authorizationRequired": true
    },
    // Настройка ключа для jwt авторизации.
    "Secret": {
        "secretKey": "YOUR_EPIC_SECRET_KEY"
    }
}
```
3. Скорректировать docker-compose
```
version: '3.4'
services:
  easyadmin.server:
    # image: ${DOCKER_REGISTRY-}easyadminserver
    build:
      context: .
      dockerfile: EasyAdmin/Server/Dockerfile
    links:
      - db
    volumes:
      - ./data:/var/lib/postgresql/data
    depends_on:
      - db
    ports:
      - 8080:80
  db:
    image: postgres
    restart: always
    environment:
      POSTGRES_PASSWORD: password
      POSTGRES_USER: easyadmin
  vmware:
    image: nexus3.hostco.ru:55555/easyadmin_vmware
    restart: always
    volumes:
      - ./config.py:/usr/src/app/config.py
```
4. Сделать docker-compose build
5. Сделать docker-compose up
