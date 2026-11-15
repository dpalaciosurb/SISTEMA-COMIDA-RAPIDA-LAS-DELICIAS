-- ============================================
-- 1. CREACIÓN DE LA BASE DE DATOS
-- ============================================

CREATE DATABASE DeliciasDB;
GO

USE DeliciasDB;
GO

-- ============================================
-- 2. CREACIÓN DE TABLAS
-- ============================================

-- Tabla: Roles
CREATE TABLE Roles
(
    idRol VARCHAR(10) PRIMARY KEY,
    nombreRol VARCHAR(50) NOT NULL,
    activo BIT NOT NULL DEFAULT 1
);
GO

-- Tabla: Usuarios
CREATE TABLE Usuarios
(
    idUsuario VARCHAR(10) PRIMARY KEY,
    idRol VARCHAR(10) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    usuario VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(100) NOT NULL,
    activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (idRol) REFERENCES Roles(idRol)
);
GO

-- Tabla: Categorias
CREATE TABLE Categorias
(
    idCategoria VARCHAR(10) PRIMARY KEY,
    nombreCategoria VARCHAR(50) NOT NULL,
    descripcion VARCHAR(255) NULL,
    activo BIT NOT NULL DEFAULT 1
);
GO

-- Tabla: Proveedores (NUEVA)
CREATE TABLE Proveedores
(
    idProveedor VARCHAR(10) PRIMARY KEY,
    nombreProveedor VARCHAR(100) NOT NULL,
    contacto VARCHAR(100) NULL,
    telefono VARCHAR(20) NULL,
    direccion VARCHAR(255) NULL,
    activo BIT NOT NULL DEFAULT 1
);
GO

-- Tabla: Productos (Actualizada con idProveedor)
CREATE TABLE Productos
(
    idProducto VARCHAR(10) PRIMARY KEY,
    idCategoria VARCHAR(10) NOT NULL,
    idProveedor VARCHAR(10) NOT NULL,
    nombreProducto VARCHAR(100) NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL,
    activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (idCategoria) REFERENCES Categorias(idCategoria),
    CONSTRAINT FK_Productos_Proveedores FOREIGN KEY (idProveedor) REFERENCES Proveedores(idProveedor)
);
GO

-- Tabla: Clientes
CREATE TABLE Clientes
(
    idCliente VARCHAR(10) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    telefono VARCHAR(20) NULL,
    direccion VARCHAR(255) NULL,
    activo BIT NOT NULL DEFAULT 1
);
GO

-- Tabla: Ventas (Cabecera)
CREATE TABLE Ventas
(
    idVenta VARCHAR(15) PRIMARY KEY,
    idUsuario VARCHAR(10) NOT NULL,
    idCliente VARCHAR(10) NOT NULL,
    fechaHora DATETIME NOT NULL DEFAULT GETDATE(),
    total DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_Ventas_Usuarios FOREIGN KEY (idUsuario) REFERENCES Usuarios(idUsuario),
    CONSTRAINT FK_Ventas_Clientes FOREIGN KEY (idCliente) REFERENCES Clientes(idCliente)
);
GO

-- Tabla: DetalleVenta
CREATE TABLE DetalleVenta
(
    idDetalle INT IDENTITY(1,1) PRIMARY KEY,
    idVenta VARCHAR(15) NOT NULL,
    idProducto VARCHAR(10) NOT NULL,
    cantidad INT NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_DetalleVenta_Ventas FOREIGN KEY (idVenta) REFERENCES Ventas(idVenta) ON DELETE CASCADE,
    CONSTRAINT FK_DetalleVenta_Productos FOREIGN KEY (idProducto) REFERENCES Productos(idProducto)
);
GO


-- ============================================
-- 3. PROCEDIMIENTOS ALMACENADOS: ROLES
-- ============================================

CREATE PROCEDURE sp_InsertarRol
    @nombreRol VARCHAR(50),
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId VARCHAR(10);
    DECLARE @UltimoNumero INT;

    SELECT @UltimoNumero = ISNULL(MAX(CAST(SUBSTRING(idRol, 5, LEN(idRol) - 4) AS INT)), 0)
    FROM Roles WHERE idRol LIKE 'ROL-%';

    SET @NuevoId = 'ROL-' + RIGHT('00' + CAST(@UltimoNumero + 1 AS VARCHAR), 2);

    INSERT INTO Roles (idRol, nombreRol, activo)
    VALUES (@NuevoId, @nombreRol, @activo);

    SELECT @NuevoId AS IdRolGenerado;
END;
GO

CREATE PROCEDURE sp_ListarRoles
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idRol, nombreRol, activo FROM Roles;
END;
GO


-- ============================================
-- 4. PROCEDIMIENTOS ALMACENADOS: CATEGORIAS
-- ============================================

CREATE PROCEDURE sp_InsertarCategoria
    @nombreCategoria VARCHAR(50),
    @descripcion VARCHAR(255),
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId VARCHAR(10);
    DECLARE @UltimoNumero INT;

    SELECT @UltimoNumero = ISNULL(MAX(CAST(SUBSTRING(idCategoria, 5, LEN(idCategoria) - 4) AS INT)), 0)
    FROM Categorias WHERE idCategoria LIKE 'CAT-%';

    SET @NuevoId = 'CAT-' + RIGHT('00' + CAST(@UltimoNumero + 1 AS VARCHAR), 2);

    INSERT INTO Categorias (idCategoria, nombreCategoria, descripcion, activo)
    VALUES (@NuevoId, @nombreCategoria, @descripcion, @activo);

    SELECT @NuevoId AS IdCategoriaGenerado;
END;
GO

CREATE PROCEDURE sp_ListarCategorias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idCategoria, nombreCategoria, descripcion, activo FROM Categorias;
END;
GO


-- ============================================
-- 5. PROCEDIMIENTOS ALMACENADOS: PROVEEDORES (NUEVO)
-- ============================================

CREATE PROCEDURE sp_InsertarProveedor
    @nombreProveedor VARCHAR(100),
    @contacto VARCHAR(100),
    @telefono VARCHAR(20),
    @direccion VARCHAR(255),
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId VARCHAR(10);
    DECLARE @UltimoNumero INT;

    SELECT @UltimoNumero = ISNULL(MAX(CAST(SUBSTRING(idProveedor, 6, LEN(idProveedor) - 5) AS INT)), 0)
    FROM Proveedores WHERE idProveedor LIKE 'PROV-%';

    SET @NuevoId = 'PROV-' + RIGHT('00' + CAST(@UltimoNumero + 1 AS VARCHAR), 2);

    INSERT INTO Proveedores (idProveedor, nombreProveedor, contacto, telefono, direccion, activo)
    VALUES (@NuevoId, @nombreProveedor, @contacto, @telefono, @direccion, @activo);

    SELECT @NuevoId AS IdProveedorGenerado;
END;
GO

CREATE PROCEDURE sp_ListarProveedores
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idProveedor, nombreProveedor, contacto, telefono, direccion, activo FROM Proveedores;
END;
GO

CREATE PROCEDURE sp_ActualizarProveedor
    @idProveedor VARCHAR(10),
    @nombreProveedor VARCHAR(100),
    @contacto VARCHAR(100),
    @telefono VARCHAR(20),
    @direccion VARCHAR(255),
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Proveedores
    SET 
        nombreProveedor = @nombreProveedor,
        contacto = @contacto,
        telefono = @telefono,
        direccion = @direccion,
        activo = @activo
    WHERE idProveedor = @idProveedor;
END;
GO


-- ============================================
-- 6. PROCEDIMIENTOS ALMACENADOS: PRODUCTOS (ACTUALIZADO)
-- ============================================

CREATE PROCEDURE sp_InsertarProducto
    @idCategoria VARCHAR(10),
    @idProveedor VARCHAR(10),
    @nombreProducto VARCHAR(100),
    @precio DECIMAL(10,2),
    @stock INT,
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId VARCHAR(10);
    DECLARE @UltimoNumero INT;

    SELECT @UltimoNumero = ISNULL(MAX(CAST(SUBSTRING(idProducto, 5, LEN(idProducto) - 4) AS INT)), 0)
    FROM Productos WHERE idProducto LIKE 'PRO-%';

    SET @NuevoId = 'PRO-' + RIGHT('00' + CAST(@UltimoNumero + 1 AS VARCHAR), 2);

    INSERT INTO Productos (idProducto, idCategoria, idProveedor, nombreProducto, precio, stock, activo)
    VALUES (@NuevoId, @idCategoria, @idProveedor, @nombreProducto, @precio, @stock, @activo);

    SELECT @NuevoId AS IdProductoGenerado;
END;
GO

CREATE PROCEDURE sp_ListarProductos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        p.idProducto,
        p.idCategoria,
        c.nombreCategoria,
        p.idProveedor,
        pr.nombreProveedor,
        p.nombreProducto,
        p.precio,
        p.stock,
        p.activo
    FROM Productos p
    INNER JOIN Categorias c ON p.idCategoria = c.idCategoria
    INNER JOIN Proveedores pr ON p.idProveedor = pr.idProveedor;
END;
GO

CREATE PROCEDURE sp_ActualizarProducto
    @idProducto VARCHAR(10),
    @idCategoria VARCHAR(10),
    @idProveedor VARCHAR(10),
    @nombreProducto VARCHAR(100),
    @precio DECIMAL(10,2),
    @stock INT,
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Productos
    SET 
        idCategoria = @idCategoria,
        idProveedor = @idProveedor,
        nombreProducto = @nombreProducto,
        precio = @precio,
        stock = @stock,
        activo = @activo
    WHERE idProducto = @idProducto;
END;
GO


-- ============================================
-- 7. PROCEDIMIENTOS ALMACENADOS: CLIENTES
-- ============================================

CREATE PROCEDURE sp_InsertarCliente
    @nombre VARCHAR(100),
    @telefono VARCHAR(20),
    @direccion VARCHAR(255),
    @activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId VARCHAR(10);
    DECLARE @UltimoNumero INT;

    SELECT @UltimoNumero = ISNULL(MAX(CAST(SUBSTRING(idCliente, 5, LEN(idCliente) - 4) AS INT)), 0)
    FROM Clientes WHERE idCliente LIKE 'CLI-%';

    SET @NuevoId = 'CLI-' + RIGHT('00' + CAST(@UltimoNumero + 1 AS VARCHAR), 2);

    INSERT INTO Clientes (idCliente, nombre, telefono, direccion, activo)
    VALUES (@NuevoId, @nombre, @telefono, @direccion, @activo);

    SELECT @NuevoId AS IdClienteGenerado;
END;
GO

CREATE PROCEDURE sp_ListarClientes
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idCliente, nombre, telefono, direccion, activo FROM Clientes;
END;
GO


-- ============================================
-- 8. REGISTRO COMPLETO DE VENTAS (TRANSACCIÓN)
-- ============================================

CREATE PROCEDURE sp_RegistrarVenta
    @idUsuario VARCHAR(10),
    @idCliente VARCHAR(10),
    @total DECIMAL(10,2),
    @IdVentaGenerado VARCHAR(15) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NuevoId VARCHAR(15);
    DECLARE @UltimoNumero INT;

    SELECT @UltimoNumero = ISNULL(MAX(CAST(SUBSTRING(idVenta, 5, LEN(idVenta) - 4) AS INT)), 0)
    FROM Ventas WHERE idVenta LIKE 'VEN-%';

    SET @NuevoId = 'VEN-' + RIGHT('000000' + CAST(@UltimoNumero + 1 AS VARCHAR), 6);

    INSERT INTO Ventas (idVenta, idUsuario, idCliente, fechaHora, total)
    VALUES (@NuevoId, @idUsuario, @idCliente, GETDATE(), @total);

    SET @IdVentaGenerado = @NuevoId;
END;
GO

CREATE PROCEDURE sp_InsertarDetalleVenta
    @idVenta VARCHAR(15),
    @idProducto VARCHAR(10),
    @cantidad INT,
    @subtotal DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

    -- Insertar línea de detalle
    INSERT INTO DetalleVenta (idVenta, idProducto, cantidad, subtotal)
    VALUES (@idVenta, @idProducto, @cantidad, @subtotal);

    -- Descontar del inventario automáticamente
    UPDATE Productos
    SET stock = stock - @cantidad
    WHERE idProducto = @idProducto;
END;
GO


-- ============================================
-- 9. DATOS DE PRUEBA INICIALES (SEED DATA)
-- ============================================

EXEC sp_InsertarRol 'Administrador', 1;
EXEC sp_InsertarRol 'Cajero', 1;

INSERT INTO Usuarios (idUsuario, idRol, nombre, usuario, password, activo)
VALUES ('USU-01', 'ROL-01', 'Dereck Palacios', 'admin', 'admin123', 1);

EXEC sp_InsertarCliente 'Cliente General (Mostrador)', '00000000', 'Managua', 1;

EXEC sp_InsertarCategoria 'Hamburguesas', 'Combos y hamburguesas individuales', 1;
EXEC sp_InsertarCategoria 'Bebidas', 'Gaseosas, jugos y batidos', 1;

-- Inserción de proveedor de prueba
EXEC sp_InsertarProveedor 'Distribuidora Alimentos S.A.', 'Juan Pérez', '8888-9999', 'Managua, Nicaragua', 1;

-- Inserción de productos asociados al proveedor ('PROV-01') y categorías
EXEC sp_InsertarProducto 'CAT-01', 'PROV-01', 'Hamburguesa Doble con Queso', 120.00, 50, 1;
EXEC sp_InsertarProducto 'CAT-02', 'PROV-01', 'Gaseosa 500ml', 30.00, 100, 1;
GO