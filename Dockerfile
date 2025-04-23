# Usar uma imagem base com mais pacotes (Ubuntu)
FROM ubuntu:20.04

# Atualizar e instalar wget (e qualquer outro pacote necessário)
RUN apt-get update && apt-get install -y wget curl

# Baixar o driver JDBC do SQL Server
RUN wget -O /opt/keycloak/providers/mssql-jdbc.jar https://repo1.maven.org/maven2/com/microsoft/sqlserver/mssql-jdbc/12.4.2.jre11/mssql-jdbc-12.4.2.jre11.jar

# Usar a imagem oficial do Keycloak
FROM quay.io/keycloak/keycloak:24.0.2

# Copiar o driver JDBC do SQL Server da imagem anterior
COPY --from=0 /opt/keycloak/providers/mssql-jdbc.jar /opt/keycloak/providers/mssql-jdbc.jar

# Continuar com a configuração do Keycloak
USER 1000
