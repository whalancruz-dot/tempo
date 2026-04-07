CREATE EXTENSION IF NOT EXISTS "pgcrypto";

CREATE TABLE Usuarios (
    Id UUID DEFAULT gen_random_uuid() PRIMARY KEY,
    Nome VARCHAR(50) NOT NULL UNIQUE,
    SenhaHash TEXT NOT NULL,
    Email VARCHAR(100),
    DataCriacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE CidadesFavoritas (
    id UUID DEFAULT gen_random_uuid() PRIMARY KEY,
    cidadeid INT NOT NULL, 
    nome VARCHAR(100) NOT NULL,
    state VARCHAR(10) NOT NULL,
    usuarioId UUID NOT NULL 
)

