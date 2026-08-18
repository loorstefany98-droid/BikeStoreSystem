CREATE DATABASE BD_BikeStore;
GO

USE BD_BikeStore;
GO

CREATE TABLE dbo.Categoria(
	IdCategoria int IDENTITY(1,1) NOT NULL,
	Nombre nvarchar(50) NOT NULL,
	Descripcion nvarchar(200) NULL,
	Activo bit NULL,
PRIMARY KEY CLUSTERED (IdCategoria ASC)
);
GO

CREATE TABLE dbo.Bicicleta(
	IdBicicleta int IDENTITY(1,1) NOT NULL,
	IdCategoria int NOT NULL,
	Marca nvarchar(50) NOT NULL,
	Modelo nvarchar(50) NOT NULL,
	Precio decimal(10, 2) NOT NULL,
	Stock int NOT NULL,
	Estado nvarchar(20) NOT NULL,
PRIMARY KEY CLUSTERED (IdBicicleta ASC)
);
GO

CREATE TABLE dbo.Cliente(
	IdCliente int IDENTITY(1,1) NOT NULL,
	Cedula nvarchar(20) NOT NULL,
	Nombres nvarchar(100) NOT NULL,
	Apellidos nvarchar(100) NOT NULL,
	Telefono nvarchar(20) NULL,
	Correo nvarchar(100) NULL,
PRIMARY KEY CLUSTERED (IdCliente ASC),
UNIQUE NONCLUSTERED (Cedula ASC)
);
GO

CREATE TABLE dbo.Venta(
	IdVenta int IDENTITY(1,1) NOT NULL,
	Fecha datetime NOT NULL,
	IdCliente int NOT NULL,
	Total decimal(10, 2) NOT NULL,
PRIMARY KEY CLUSTERED (IdVenta ASC)
);
GO

CREATE TABLE dbo.Detalle_Venta(
	IdDetalle int IDENTITY(1,1) NOT NULL,
	IdVenta int NOT NULL,
	IdBicicleta int NOT NULL,
	Cantidad int NOT NULL,
	Precio decimal(10, 2) NOT NULL,
	SubTotal decimal(10, 2) NOT NULL,
PRIMARY KEY CLUSTERED (IdDetalle ASC)
);
GO

ALTER TABLE dbo.Categoria ADD DEFAULT ((1)) FOR Activo;
GO
ALTER TABLE dbo.Venta ADD DEFAULT (getdate()) FOR Fecha;
GO

ALTER TABLE dbo.Bicicleta WITH CHECK ADD CONSTRAINT FK_Bicicleta_Categoria FOREIGN KEY(IdCategoria)
REFERENCES dbo.Categoria (IdCategoria);
GO

ALTER TABLE dbo.Detalle_Venta WITH CHECK ADD CONSTRAINT FK_DetalleVenta_Bicicleta FOREIGN KEY(IdBicicleta)
REFERENCES dbo.Bicicleta (IdBicicleta);
GO

ALTER TABLE dbo.Detalle_Venta WITH CHECK ADD CONSTRAINT FK_DetalleVenta_Venta FOREIGN KEY(IdVenta)
REFERENCES dbo.Venta (IdVenta);
GO

ALTER TABLE dbo.Venta WITH CHECK ADD CONSTRAINT FK_Venta_Cliente FOREIGN KEY(IdCliente)
REFERENCES dbo.Cliente (IdCliente);
GO

INSERT INTO dbo.Categoria (Nombre, Descripcion, Activo) VALUES 
('Montaña', 'Bicicletas todoterreno para montaña', 1),
('Ruta', 'Bicicletas de velocidad para carretera', 1),
('BMX', 'Bicicletas para acrobacias', 1);
GO

INSERT INTO dbo.Bicicleta (IdCategoria, Marca, Modelo, Precio, Stock, Estado) VALUES 
(1, 'Giant', 'Talon 3', 600.00, 5, 'Disponible'),
(2, 'Specialized', 'Allez Sport', 950.00, 3, 'Disponible'),
(3, 'Venzo', 'FR-1', 350.00, 8, 'Disponible');
GO

INSERT INTO dbo.Cliente (Cedula, Nombres, Apellidos, Telefono, Correo) VALUES 
('1309119327', 'Josselyn', 'Cedeño', '0987654321', 'josselyn@mail.com'),
('1712345678', 'Carlos', 'Pérez', '0991234567', 'carlos@mail.com');
GO