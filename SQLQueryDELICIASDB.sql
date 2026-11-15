create database DeliciasDB;
go

use DeliciasDB;
go

create table Roles
(
    idRol varchar(10) primary key,
    nombreRol varchar(50) not null,
    activo bit not null default 1
);
go

create table Usuarios
(
    idUsuario varchar(10) primary key,
    idRol varchar(10) not null,
    nombre varchar(100) not null,
    usuario varchar(50) not null unique,
    password varchar(100) not null,
    activo bit not null default 1,
    constraint FK_Usuarios_Roles foreign key (idRol) references Roles(idRol)
);
go

create table Proveedores
(
    idProveedor varchar(10) primary key,
    nombreProveedor varchar(100) not null,
    contacto varchar(100) null,
    telefono varchar(20) null,
    direccion varchar(255) null,
    activo bit not null default 1
);
go

create table Categorias
(
    idCategoria varchar(10) primary key,
    nombreCategoria varchar(50) not null,
    descripcion varchar(255) null,
    activo bit not null default 1
);
go

create table Productos
(
    idProducto varchar(10) primary key,
    idCategoria varchar(10) not null,
    idProveedor varchar(10) not null,
    nombreProducto varchar(100) not null,
    precio decimal(10,2) not null,
    stock int not null,
    activo bit not null default 1,
    constraint FK_Productos_Categorias foreign key (idCategoria) references Categorias(idCategoria),
    constraint FK_Productos_Proveedores foreign key (idProveedor) references Proveedores(idProveedor)
);
go

create table Clientes
(
    idCliente varchar(10) primary key,
    nombre varchar(100) not null,
    telefono varchar(20) null,
    direccion varchar(255) null,
    activo bit not null default 1
);
go

create table Ventas
(
    idVenta varchar(15) primary key,
    idUsuario varchar(10) not null,
    idCliente varchar(10) not null,
    fechaHora datetime not null default getdate(),
    total decimal(10,2) not null,
    constraint FK_Ventas_Usuarios foreign key (idUsuario) references Usuarios(idUsuario),
    constraint FK_Ventas_Clientes foreign key (idCliente) references Clientes(idCliente)
);
go

create table DetalleVenta
(
    idDetalle int identity(1,1) primary key,
    idVenta varchar(15) not null,
    idProducto varchar(10) not null,
    cantidad int not null,
    subtotal decimal(10,2) not null,
    constraint FK_DetalleVenta_Ventas foreign key (idVenta) references Ventas(idVenta) on delete cascade,
    constraint FK_DetalleVenta_Productos foreign key (idProducto) references Productos(idProducto)
);
go

create or alter procedure sp_InsertarRol
    @nombreRol varchar(50),
    @activo bit
as
begin
    set nocount on;
    declare @NuevoId varchar(10);
    declare @UltimoNumero int;

    select @UltimoNumero = isnull(max(cast(substring(idRol, 5, len(idRol) - 4) as int)), 0)
    from Roles where idRol like 'ROL-%';

    set @NuevoId = 'ROL-' + right('00' + cast(@UltimoNumero + 1 as varchar), 2);

    insert into Roles (idRol, nombreRol, activo)
    values (@NuevoId, @nombreRol, @activo);

    select @NuevoId as IdRolGenerado;
end;
go

create or alter procedure sp_InsertarCategoria
    @nombreCategoria varchar(50),
    @descripcion varchar(255),
    @activo bit
as
begin
    set nocount on;
    declare @NuevoId varchar(10);
    declare @UltimoNumero int;

    select @UltimoNumero = isnull(max(cast(substring(idCategoria, 5, len(idCategoria) - 4) as int)), 0)
    from Categorias where idCategoria like 'CAT-%';

    set @NuevoId = 'CAT-' + right('00' + cast(@UltimoNumero + 1 as varchar), 2);

    insert into Categorias (idCategoria, nombreCategoria, descripcion, activo)
    values (@NuevoId, @nombreCategoria, @descripcion, @activo);

    select @NuevoId as IdCategoriaGenerado;
end;
go



create or alter procedure sp_InsertarCliente
    @nombre varchar(100),
    @telefono varchar(20),
    @direccion varchar(255),
    @activo bit
as
begin
    set nocount on;
    declare @NuevoId varchar(10);
    declare @UltimoNumero int;

    select @UltimoNumero = isnull(max(cast(substring(idCliente, 5, len(idCliente) - 4) as int)), 0)
    from Clientes where idCliente like 'CLI-%';

    set @NuevoId = 'CLI-' + right('00' + cast(@UltimoNumero + 1 as varchar), 2);

    insert into Clientes (idCliente, nombre, telefono, direccion, activo)
    values (@NuevoId, @nombre, @telefono, @direccion, @activo);

    select @NuevoId as IdClienteGenerado;
end;
go

create or alter procedure sp_InsertarProveedor
    @nombreProveedor varchar(100),
    @contacto varchar(100),
    @telefono varchar(20),
    @direccion varchar(255),
    @activo bit
as
begin
    set nocount on;
    declare @NuevoId varchar(10);
    declare @UltimoNumero int;

    select @UltimoNumero = isnull(max(cast(substring(idProveedor, 6, len(idProveedor) - 5) as int)), 0)
    from Proveedores where idProveedor like 'PROV-%';

    set @NuevoId = 'PROV-' + right('00' + cast(@UltimoNumero + 1 as varchar), 2);

    insert into Proveedores (idProveedor, nombreProveedor, contacto, telefono, direccion, activo)
    values (@NuevoId, @nombreProveedor, @contacto, @telefono, @direccion, @activo);

    select @NuevoId as IdProveedorGenerado;
end;
go


create  or alter procedure sp_ListarClientes
as
begin
    set nocount on;
    select idCliente, nombre, telefono, direccion, activo from Clientes;
end;
go

create  or alter procedure sp_ActualizarCliente
    @idCliente varchar(10),
    @nombre varchar(100),
    @telefono varchar(20),
    @direccion varchar(255),
    @activo bit
as
begin
    set nocount on;
    update Clientes
    set 
        nombre = @nombre,
        telefono = @telefono,
        direccion = @direccion,
        activo = @activo
    where idCliente = @idCliente;
end;
go

create or alter procedure sp_EliminarCliente
    @idCliente varchar(10)
as
begin
    set nocount on;
    delete from Clientes where idCliente = @idCliente;
end;
go



create or alter procedure sp_InsertarProducto
    @idCategoria varchar(10),
    @idProveedor varchar(10),
    @nombreProducto varchar(100),
    @precio decimal(10,2),
    @stock int,
    @activo bit
as
begin
    set nocount on;
    declare @NuevoId varchar(10);
    declare @UltimoNumero int;

    select @UltimoNumero = isnull(max(cast(substring(idProducto, 5, len(idProducto) - 4) as int)), 0)
    from Productos where idProducto like 'PRO-%';

    set @NuevoId = 'PRO-' + right('00' + cast(@UltimoNumero + 1 as varchar), 2);

    insert into Productos (idProducto, idCategoria, idProveedor, nombreProducto, precio, stock, activo)
    values (@NuevoId, @idCategoria, @idProveedor, @nombreProducto, @precio, @stock, @activo);

    select @NuevoId as IdProductoGenerado;
end;
go

create or alter procedure sp_ListarProductos
as
begin
    set nocount on;
    select 
        p.idProducto,
        p.idCategoria,
        c.nombreCategoria,
        p.idProveedor,
        pr.nombreProveedor,
        p.nombreProducto,
        p.precio,
        p.stock,
        p.activo
    from Productos p
    inner join Categorias c on p.idCategoria = c.idCategoria
    inner join Proveedores pr on p.idProveedor = pr.idProveedor;
end;
go

create or alter procedure sp_ActualizarProducto
    @idProducto varchar(10),
    @idCategoria varchar(10),
    @idProveedor varchar(10),
    @nombreProducto varchar(100),
    @precio decimal(10,2),
    @stock int,
    @activo bit
as
begin
    set nocount on;
    update Productos
    set 
        idCategoria = @idCategoria,
        idProveedor = @idProveedor,
        nombreProducto = @nombreProducto,
        precio = @precio,
        stock = @stock,
        activo = @activo
    where idProducto = @idProducto;
end;
go

create or alter procedure sp_EliminarProducto
    @idProducto varchar(10)
as
begin
    set nocount on;
    delete from DetalleVenta where idProducto = @idProducto;
    
    delete from Productos where idProducto = @idProducto;
end;
go

create or alter procedure sp_ListarProveedores
as
begin
    set nocount on;
    select idProveedor, nombreProveedor, contacto, telefono, direccion, activo from Proveedores;
end;
go

create or alter procedure sp_ActualizarProveedor
    @idProveedor varchar(10),
    @nombreProveedor varchar(100),
    @contacto varchar(100),
    @telefono varchar(20),
    @direccion varchar(255),
    @activo bit
as
begin
    set nocount on;
    update Proveedores
    set 
        nombreProveedor = @nombreProveedor,
        contacto = @contacto,
        telefono = @telefono,
        direccion = @direccion,
        activo = @activo
    where idProveedor = @idProveedor;
end;
go

create or alter procedure sp_EliminarProveedor
    @idProveedor varchar(10)
as
begin
    set nocount on;
    delete from DetalleVenta where idProducto in (select idProducto from Productos where idProveedor = @idProveedor);
    delete from Productos where idProveedor = @idProveedor;

    delete from Proveedores where idProveedor = @idProveedor;
end;
go


create or alter procedure sp_EliminarCategoria
    @idCategoria varchar(10)
as
begin
    set nocount on;
    -- Si tiene productos asociados, borra o desvincula primero si lo deseas, o borra directo:
    delete from Categorias where idCategoria = @idCategoria;
end;
go


exec sp_EliminarCategoria 'CAT-07';
go


exec sp_InsertarRol 'Administrador', 1;
exec sp_InsertarRol 'Cajero', 1;

insert into Usuarios (idUsuario, idRol, nombre, usuario, password, activo)
values ('USU-01', 'ROL-01', 'Dereck Palacios', 'admin', 'admin123', 1);

exec sp_InsertarCliente 'Cliente General', '00000000', 'Managua', 1;

exec sp_InsertarCategoria 'Hamburguesas', 'Combos y hamburguesas', 1;
exec sp_InsertarCategoria 'Bebidas', 'Gaseosas y jugos', 1;
exec sp_InsertarCategoria 'Hamburguesas', 'Hamburguesas y combos especiales', 1;
exec sp_InsertarCategoria 'Pizzas', 'Pizzas medianas y familiares', 1;
exec sp_InsertarCategoria 'Bebidas', 'Gaseosas, jugos y te frio', 1;
exec sp_InsertarCategoria 'Complementos', 'Papas fritas, aros de cebolla y nuggets', 1;
exec sp_InsertarCategoria 'Pollo Frito', 'Piezas de pollo crujiente y combos', 1;
exec sp_InsertarCategoria 'Postres', 'Helados, pays y reposteria', 1;
go


exec sp_InsertarProveedor 'Distribuidora Alimentos SA', 'Juan Perez', '88889999', 'Managua', 1;

exec sp_InsertarProducto 'CAT-01', 'PROV-01', 'Hamburguesa Doble', 120.00, 50, 1;
exec sp_InsertarProducto 'CAT-02', 'PROV-01', 'Gaseosa', 30.00, 100, 1;


go


-- insertar una venta de prueba
insert into Ventas (idVenta, idUsuario, idCliente, fechaHora, total)
values ('VEN-000001', 'USU-01', 'CLI-01', getdate(), 150.00);

-- insertar el detalle de esa venta
insert into DetalleVenta (idVenta, idProducto, cantidad, subtotal)
values ('VEN-000001', 'PRO-01', 1, 120.00);

insert into DetalleVenta (idVenta, idProducto, cantidad, subtotal)
values ('VEN-000001', 'PRO-02', 1, 30.00);



---TESTING

select 
    v.idVenta as codigo_venta,
    v.fechaHora as fecha_y_hora,
    c.nombre as cliente,
    u.nombre as cajero,
    p.nombreProducto as producto,
    dv.cantidad as cantidad,
    p.precio as precio_unitario,
    dv.subtotal as subtotal,
    v.total as total_general
from Ventas v
inner join Clientes c on v.idCliente = c.idCliente
inner join Usuarios u on v.idUsuario = u.idUsuario
inner join DetalleVenta dv on v.idVenta = dv.idVenta
inner join Productos p on dv.idProducto = p.idProducto;