-- Tabla para el flujo de "olvidé mi contraseña" (envío de link por correo)
CREATE TABLE ReinicioContrasenia(
    Id INT IDENTITY PRIMARY KEY,
    IdUsuario INT NOT NULL,
    TokenHash VARCHAR(200) NOT NULL,   -- hash SHA256 del token, nunca el token en claro
    FechaCreacion DATETIME NOT NULL,
    FechaExpiracion DATETIME NOT NULL,
    Usado BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_ReinicioContrasenia_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);

CREATE INDEX IX_ReinicioContrasenia_IdUsuario ON ReinicioContrasenia(IdUsuario);
